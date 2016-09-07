var app = angular.module('TeamNet', ['ui.bootstrap', 'ui.filters', 'ui.select', 'app.config', 'RemoteResourse'])

app.config(['$httpProvider', function ($httpProvider) {
    delete $httpProvider.defaults.headers.common['X-Requested-With'];
    $httpProvider.defaults.useXDomain = true;
}]);
/**
* Variable where the configuration's values for this script are stored. Will be filled when the controller is initialized.
*/
var config;

var JUNIOR = 'junior';
var STAFF = 'staff';
var SENIOR = 'senior';

var visitor = false;
var currentId = null;
var currentName;
var currentLastName;
var currentEmail;
resultsURL = "../partials/resultsFrame.html";

app.factory('$log', function () {
    var consoleAppender, logger;
    logger = log4javascript.getLogger('teamnet_logger');
    consoleAppender = new log4javascript.BrowserConsoleAppender();
    consoleAppender.setThreshold(log4javascript.Level.ALL);
    logger.addAppender(consoleAppender);
    return logger;
});

app.factory('TaskFactory', function () {
    var newTask = createJsonTask();
    return newTask;
});

app.controller('ProfileCtrl', function ($scope, $http, PROFILE_CONFIG, MENU_CONFIG, URLs, $log, RemoteResourseFactory) {
    if (sessionStorage.getItem('token') == null) {
        $window.location.replace(URLs.LOGIN_URL);
    }
    $scope.username = sessionStorage.getItem('username');
    $scope.lastname = sessionStorage.getItem('lastname');
    $scope.token = sessionStorage.getItem('token');
    $scope.tab = 'wall';
    $http.defaults.headers.common["Authorization"] = 'Bearer ' + $scope.token;

    currentId = getQueryVariable("id");
    if (currentId != false) {
        if (currentId == -1) {
            $scope.tab = 'work';
            visitor = false;
            currentName = sessionStorage.getItem('username');
            currentLastName = sessionStorage.getItem('lastname');
            currentId = getId(sessionStorage.getItem('orid'));
            currentEmail = sessionStorage.getItem('userEmail');
        } else {
            visitor = true;
            resultsURL = resultsURL + "?id=" + currentId;
            $scope.currentProfileId = "#13:" + currentId;
            var params = "/" + currentId;
            RemoteResourseFactory.getPromise(URLs.SERVICE + URLs.USER_INFO + params).then(function (info) {
                currentName = info.name;
                currentLastName = info.lastName;
                currentEmail = info.email;
            });
        }
        $scope.currentProfileName = currentName;
        $scope.currentProfileLastname = currentLastName;
        $scope.currentProfileEmail = currentEmail;
    } else {
        visitor = false;
        currentName = sessionStorage.getItem('username');
        currentLastName = sessionStorage.getItem('lastname');
        currentId = getId(sessionStorage.getItem('orid'));
        currentEmail = sessionStorage.getItem('userEmail');

        $scope.currentProfileName = currentName;
        $scope.currentProfileLastname = currentLastName;
        $scope.currentProfileEmail = currentEmail;
        $scope.currentProfileId = currentId;
    }
    $scope.visitor = visitor;
    $scope.resultsURL = resultsURL;
    $scope.menus = MENU_CONFIG.ITEMS;
    $scope.config = PROFILE_CONFIG;
    if (currentId < 0) {
        $scope.tab = 'work';
    } else {

    }

    var userRid = getId(currentId);
    //var userRid = getId(sessionStorage.getItem('orid'));
    var serviceUrl = URLs.SERVICE + URLs.USER + '/' + URLs.PUBLICATION;
    RemoteResourseFactory.getPromiseParams(serviceUrl, userRid).then(function (result) {
        $scope.publications = result;
    }, function (status) {
        console.log('HTTP connection error: ' + status + ' getting publications.');
    });
});

app.controller('WorkCtrl', function ($scope, $http, $filter, URLs, PROFILE_CONFIG, TaskFactory, RemoteResourseFactory) {
    $scope.token = sessionStorage.getItem('token');
    $http.defaults.headers.common["Authorization"] = 'Bearer ' + $scope.token;
    TaskFactory.suggestionToTask = function (suggestion) {
        $scope.task = createJsonTask();
        $scope.task.name = suggestion.name;
        $scope.task.description = suggestion.description;
        $scope.task.initialDate = suggestion.initialDate;
        $scope.task.endDate = suggestion.endDate;
    };

    var userId = currentId;
    RemoteResourseFactory.getPromise(URLs.SERVICE + URLs.COMPETENCY + '/' + userId).then(function (resultCompetency) {
        $scope.points = resultCompetency;
        var competenciesList = $scope.points;
        RemoteResourseFactory.getPromise(URLs.SERVICE + URLs.TASK + '/' + userId).then(function (resultTasks) {
            var tasksList = resultTasks;
            var list = [];
            for (i = 0; i < competenciesList.length; i++) {
                var competency = createCompetency();
                competency.name = competenciesList[i].name;
                competency.id = competenciesList[i].id;
                list.push(competency);
            }
            $scope.competencies = list;
            for (ic = 0; ic < list.length; ic++) {
                var competency = list[ic];
                for (it = 0; it < tasksList.length; it++) {
                    var task = tasksList[it];
                    if (task.competencyId == competency.id) {
                        $scope.competencies[ic].tasks.push(task);
                    }
                }
            }
        }, function (status) {
            console.log('HTTP connection error: ' + status + ' getting tasks.');
        });
    }, function (status) {
        console.log('HTTP connection error: ' + status + ' getting competencies.');
    });
    $scope.levels = PROFILE_CONFIG.LEVELS;
    $scope.formTitle = "Task Creator";
    $scope.task = createJsonTask();
    $scope.isCreatingTask = true;
    $scope.taskFormEnabled = true;
    $scope.taskSuggestionsEnabled = true;
    var selectedTask = createJsonTask();

    RemoteResourseFactory.getPromise(URLs.SERVICE + URLs.USER).then(function (data) {
        for (var i = 0; i < data.length; i++) {
            data[i].fullname = data[i].name + ' ' + data[i].lastName;
        }
        $scope.reviewers = data;
    }, function (status) {
        console.log("error http connection:" + status);
    });

    $scope.selectReviewer = function () {
        $scope.task.reviewerId = $scope.taskReviewer.id;
    };

    $scope.openRequestModal = function (task) {
        var url = URLs.SERVICE + URLs.USER_ID + "/" + getId(task.reviewer);
        $http.get(url).
        success(function (result) {
            $scope.nameReviewer = result;
        })
        .error(function () {
            console.log("error");
        });

        var modalInstance = $modal.open({
            templateUrl: 'App/partials/requestmodal.html',
            controller: 'requesterCtrl',
            size: 'md',
            resolve: {
                task: function () {
                    return task;
                }
            }
        });
    };

    $scope.open = function (openedTask, openedCompetencyName) {
        $scope.formTitle = "Task Editor";
        $scope.task = createJsonTask();
        $scope.taskFormEnabled = true;
        $scope.taskSuggestionsEnabled = false;
        $scope.isCreatingTask = false;
        selectedTask = openedTask;
        $scope.selectedCompetencyName = openedCompetencyName;
        for (var i = 0; i < $scope.reviewers.length; i++) {
            if ($scope.reviewers[i].id == openedTask.reviewerId) {
                $scope.taskReviewer = $scope.reviewers[i];
            }
        }
        updateTask($scope.task, openedTask, $filter);
    };

    $scope.createTask = function () {
        $scope.formTitle = "Task Creator ";
        $scope.taskFormEnabled = true;
        $scope.taskSuggestionsEnabled = true;
        $scope.isCreatingTask = true;
        selectedTask = null;
        $scope.taskReviewer = null;
        $scope.selectedCompetencyName = null;
        $scope.task = createJsonTask();
    };

    $scope.deleteTask = function (task) {
        if (confirm("Are  you sure you want to remove the task ?")) {
            var index = $scope.tasks.indexOf(task);
            $scope.tasks.splice(index, 1);
            var rid = getId(task.id);
            var del = URLs.SERVICE + URLs.TASK + '/' + rid;
            $http.delete(del)
                .success(function (data, status) {
                    $scope.taskDeleted = data;
                    console.log(status);
                });
        }
    };

    $scope.cancel = function () {
        $scope.createTask();
    };

    $scope.ok = function (formCondition) {
        if (formCondition) {
            var condition = $scope.isCreatingTask;
            if (condition) {
                var newtask = createJsonTask();
                updateTask(newtask, $scope.task, $filter);
                convertTaskDates(newtask, $filter);
                var newTask = createTaskWithCompetencyId(newtask, getId($scope.point.id));
                $http.post(URLs.SERVICE + URLs.TASK, newTask).
                success(function (data) {
                    $scope.taskCreated = data;
                }).
            error(function (data, status, headers, config) {
                console.log('Task was not inserted because UserId and PointId');
                console.log('Task was not inserted because UserId and PointId are null (This mesagge should be deleted when this bug will be fixed)');
            });
                var competency = null;
                for (var i = 0; i < $scope.competencies.length; i++) {
                    if (getId($scope.competencies[i].id) == newTask.competencyId) {
                        competency = $scope.competencies[i];
                    }
                }
                competency.tasks.push(newtask);
            } else {
                updateTask(selectedTask, $scope.task, $filter);
                convertTaskDates(selectedTask, $filter);
                $http.put(URLs.SERVICE + URLs.TASK, selectedTask).
                    success(function (data) {
                        $scope.taskCreated = data;
                    });
                console.log('error puttask');
            }
            $scope.createTask();
        }
    };

    $scope.updateSuggestions = function () {
        TaskFactory.selectPoint($scope.level, $scope.point.name);
    };

    $scope.selectConpetence = function () {
        var limit = $scope.levels.length;
        for (var i = 0; i < limit; i++) {
            if ($scope.point.result <= $scope.levels[i]) {
                $scope.level = $scope.levels[i];
                i = limit;
            }
        }
        $scope.updateSuggestions();
    };

    $scope.value = false;
    /**
    * Shows the buttons when the mouse is moved by its panel of view.
    */
    $scope.displayButtons = function () {
        $scope.value = true;
    };

    /**
    * Stops displaying the buttons when the mouse is moved off the panel.
    */
    $scope.buttonsLeave = function () {
        $scope.value = false;
    };
});

app.controller('ListCtrl', function ($scope, $http, TaskFactory, URLs, PROFILE_CONFIG, RemoteResourseFactory) {
    $scope.token = sessionStorage.getItem('token');
    $http.defaults.headers.common["Authorization"] = 'Bearer ' + $scope.token;
    var currentSuggestions = [];
    var configSug = PROFILE_CONFIG;

    TaskFactory.selectPoint = function (level, name) {
        var params = '/' + level + '/' + name;
        $scope.callToServer(URLs.SERVICE + URLs.SUGGESTIONS + params);
    }

    $scope.callToServer = function (path) {
        RemoteResourseFactory.getPromise(path).then(function (result) {
            currentSuggestions = result;
            $scope.showSuggestions();
            loadSuggestion();
        }, function (status) {
            console.log('HTTP connection error: ' + status + ' getting suggestions.');
        });
    };

    $scope.callToServer(URLs.SERVICE + URLs.SUGGESTIONS);

    $scope.itemsPerPage = configSug.ITEMS_PER_PAGE;
    $scope.currentPage = configSug.INITIAL_PAGE;
    $scope.maxSize = configSug.MAX_SIZE;

    $scope.showSuggestions = function () {
        var begin = (($scope.currentPage - 1) * $scope.itemsPerPage);
        var end = begin + $scope.itemsPerPage;
        $scope.suggestions = currentSuggestions.slice(begin, end);
    };

    $scope.pageChanged = function () {
        $scope.showSuggestions();
    };

    $scope.showSuggestions();

    $scope.select = function (index) {
        TaskFactory.suggestionToTask($scope.suggestions[index]);
    }

    function loadSuggestion() {
        var pathImage = '';
        $scope.totalItems = currentSuggestions.length;
        for (var i = 0; i < currentSuggestions.length; i++) {
            switch (currentSuggestions[i].level) {
                case JUNIOR: pathImage = configSug.IMAGES.JUNIOR; break;
                case STAFF: pathImage = configSug.IMAGES.STAFF; break;
                case SENIOR: pathImage = configSug.IMAGES.SENIOR; break;
            }
            currentSuggestions[i].image = pathImage;
        }
    };
});

function getQueryVariable(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == variable) { return pair[1]; }
    }
    return (false);
}

function createCompetency() {
    var competency = {
        name: "",
        id: "",
        tasks: []
    };
    return competency;
}

function createJsonTask() {
    var jsonTask = {
        name: "",
        description: "",
        progress: 0,
        userId: currentId,
        competencyId: "",
        initialDate: null,
        endDate: null,
        reviewerId: "",
        finalProduct: "",
        score: 0,
        isSuggestion: false,
        state: "enabled"
    };
    return jsonTask;
}

function updateTask(oldTask, newTask, filter) {
    oldTask.name = newTask.name;
    oldTask.description = newTask.description;
    if (newTask.progress != null) {
        oldTask.progress = newTask.progress;
    }
    oldTask.userId = newTask.userId;
    oldTask.competencyId = newTask.competencyId;
    oldTask.initialDate = newTask.initialDate;
    oldTask.endDate = newTask.endDate;
    oldTask.reviewerId = newTask.reviewerId;
    oldTask.finalProduct = newTask.finalProduct;
    oldTask.score = newTask.score;
    oldTask.isSuggestion = newTask.isSuggestion;
    oldTask.state = newTask.state;
    var initDate = new Date(newTask.initialDate);
    var endDate = new Date(newTask.endDate);
    oldTask.initialDate = initDate;
    oldTask.endDate = endDate;
}

function convertTaskDates(targettask, filter) {
    var initDate = targettask.initialDate;
    var endDate = targettask.endDate;
    targettask.initialDate = filter('date')(initDate, 'yyyy-MM-dd');
    targettask.endDate = filter('date')(endDate, 'yyyy-MM-dd');
}

function parseTaskDates(targettask) {
    var initDate = targettask.initialDate;
    var endDate = targettask.endDate;
    targettask.initialDate = new Date(initDate);
    targettask.endDate = new Date(endDate);
    targettask.initialDate.setDate(targettask.initialDate.getDate() + 1);
    targettask.endDate.setDate(targettask.endDate.getDate() + 1);
}

function createTaskWithCompetencyId(newtask, competencyId) {
    var taskResult = {
        name: newtask.name,
        description: newtask.description,
        progress: newtask.progress,
        userId: currentId,
        competencyId: competencyId,
        initialDate: newtask.initialDate,
        endDate: newtask.endDate,
        reviewerId: getId(newtask.reviewerId),
        finalProduct: newtask.finalProduct,
        score: newtask.score,
        isSuggestion: newtask.isSuggestion,
        state: newtask.state
    };
    return taskResult;
}

function getId(orid) {
    var result = orid.split(":");
    return result[1];
}
