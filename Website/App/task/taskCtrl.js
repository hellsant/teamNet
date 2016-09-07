var app = angular.module('TeamNet', ['ui.bootstrap', 'ui.filters', 'ui.select', 'app.config', 'RemoteResourse', 'RequestForReview', 'ShareTask']);

/**
* Variable where the configuration's values for this script are stored. Will be filled when the controller is initialized.
*/
var config;

app.factory('taskFactory', function () {
    var newTask = {};
    return newTask;
});

app.controller('TasksCtrl', ['$scope', '$http', '$window', '$filter', 'INDEX_CONFIG', 'URLs', 'MENU_CONFIG', 'RemoteResourseFactory', '$modal', 'taskFactory',
    function ($scope, $http, $window, $filter, INDEX_CONFIG, URLs, MENU_CONFIG, RemoteResourseFactory, $modal, taskFactory) {
        if (sessionStorage.getItem('token') == null) {
            $window.location.replace(URLs.LOGIN_URL);
        }
        $scope.username = sessionStorage.getItem('username');
        $scope.lastname = sessionStorage.getItem('lastname');
        $scope.levels = INDEX_CONFIG.LEVELS;
        $scope.menus = MENU_CONFIG.ITEMS;
        $scope.config = INDEX_CONFIG;
        config = INDEX_CONFIG;
        var serviceUrl = URLs.SERVICE + URLs.PUBLICATION;
        $scope.token = sessionStorage.getItem('token');
        $http.defaults.headers.common["Authorization"] = 'Bearer ' + $scope.token;

        RemoteResourseFactory.getPromise(serviceUrl).then(function (result) {
            $scope.publications = result;
        }, function (status) {
            alert("error http connection:" + status);
        });

        var userId = getId(sessionStorage.getItem('orid'));
        var params = "/" + userId;

        $scope.createTask = function () {
            $window.location.replace(URLs.PROFILE_URL)
        };

        RemoteResourseFactory.getPromiseParams(URLs.SERVICE + URLs.TASK, params).then(function (result) {
            $scope.tasks = result;
            $scope.task = createJsonTask();
            $scope.isCreatingTask = true;
            $scope.taskFormEnabled = false;
            $scope.taskSuggestionsEnabled = false;
            var selectedTask = createJsonTask();
            RemoteResourseFactory.getPromise(URLs.SERVICE + URLs.COMPETENCY + params).then(function (data) {
                $scope.points = data;
            }, function (status) {
                alert("error http connection:" + status);
            });
            //Getting all users , TODO - Get by typing
            RemoteResourseFactory.getPromise(URLs.SERVICE + URLs.USER_BASIC + userId).then(function (data) {
                $scope.reviewers = data;
            }, function (status) {
                alert("error http connection:" + status);
            });
            //End
            $scope.editProgress = function (task) {
                task.isEditing = true;
            };

            $scope.hitEnterProgress = function (evt, task) {
                if (angular.equals(evt.keyCode, 13)) {
                    if ((task.progress >= 0) && (task.progress <= 100)) {
                        task.isEditing = false;
                        $http.put(URLs.SERVICE + URLs.TASK, task).
                        success(function (data) {
                            $scope.taskCreated = data;
                        });
                    }
                }
            };

            $scope.openRequestModal = function (task) {
                taskFactory.task = task;
                var modalInstance = $modal.open({
                    templateUrl: 'App/partials/requestmodal.html',
                    controller: 'RequesterCtrl',
                    size: 'lg',
                    resolve: {
                        task: function () {
                            return task;
                        }
                    }
                });
            };

            $scope.shareTask = function (task) {
                var modalInstance = $modal.open({
                    templateUrl: 'App/partials/sharemodal.html',
                    controller: 'ShareTaskCtrl',
                    size: 'sm',
                    resolve: {
                        task: function () {
                            return task;
                        }
                    }
                });
            };

            $scope.isRequest = function (task) {
                var show = false;
                if (task.progress == 100 && !task.isApproved) {
                    show = true;
                }
                return show;
            }

            $scope.open = function (openedTask) {
                $scope.task = createJsonTask();
                $scope.taskFormEnabled = true;
                $scope.taskSuggestionsEnabled = false;
                $scope.isCreatingTask = false;
                selectedTask = openedTask;
                updateTask($scope.task, openedTask, $filter);
            };

            $scope.deleteTask = function (task) {
                if (confirm("Are  you sure you want to remove the task ?")) {
                    var rid = getId(task.id);
                    var del = URLs.SERVICE + URLs.TASK + '/' + rid;
                    $http.delete(del)
                        .success(function (data, status) {
                            var index = $scope.tasks.indexOf(task);
                            $scope.tasks.splice(index, 1);
                            $scope.taskDeleted = data;
                            console.log(status);
                        })
                        .error(function (status) {
                            alert("dont remove the task  error: " + status);
                            console.log(status);
                        });
                }
            };

            $scope.cancel = function () {
                $scope.taskFormEnabled = false;
                $scope.taskSuggestionsEnabled = false;
                $scope.isCreatingTask = false;
                var selectedTask = null;
            };

            $scope.ok = function (formCondition) {
                if (formCondition) {
                    var condition = $scope.isCreatingTask;
                    if (condition) {
                        var newtask = createJsonTask();
                        updateTask(newtask, $scope.task, $filter);
                        $scope.isCreatingTask = false;
                        convertTaskDates(newtask, $filter);
                        var selectedRev = $scope.reviewer.id;
                        var newTask = angular.toJson(createTaskWithCompetencyId(newtask, $scope.point.id, selectedRev));
                        var url = URLs.SERVICE + URLs.TASK;
                        $http.post(url, newTask)
                        success(function (data) {
                            $scope.taskCreated = data;

                        }).
                    error(function (data, status, headers, config) {
                        console.log('Task was not inserted because missing parameters');
                        alert('Task was not inserted because UserId and PointId are null (This mesagge should be deleted when this bug will be fixed)');
                    });

                        parseTaskDates(newtask);
                        $scope.tasks.unshift(newtask);
                        console.log(newtask.initialDate + 'true');
                    } else {
                        updateTask(selectedTask, $scope.task, $filter);
                        convertTaskDates(selectedTask, $filter);
                        $http.put(config.URLs.SERVICE_URL + URLs.TASK, selectedTask)
                            success(function (data) {
                                $scope.taskCreated = data;
                            });
                        parseTaskDates(selectedTask);
                        console.log('error puttask');
                    }
                    $scope.taskFormEnabled = false;
                    $scope.taskSuggestionsEnabled = false;
                }
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
    }]);

function createJsonTask() {
    var jsonTask = {
        name: "",
        description: "",
        progress: 0,
        userId: sessionStorage.getItem('orid'),
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

function createTaskWithCompetencyId(newtask, competencyId, selectedRev) {
    var taskResult = {
        name: newtask.name,
        description: newtask.description,
        progress: newtask.progress,
        userId: sessionStorage.getItem('orid'),
        competencyId: competencyId,
        reviewerId: selectedRev,
        initialDate: newtask.initialDate,
        endDate: newtask.endDate,
        finalProduct: newtask.finalProduct,
        score: newtask.score,
        isSuggestion: newtask.isSuggestion,
        state: newtask.state
    };
    return taskResult;
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

function getId(orid) {
    var result = orid.split(":");
    return result[1];
}
