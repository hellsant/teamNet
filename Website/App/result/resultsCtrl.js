var app = angular.module('TeamNet', ['ngTable', 'ui.bootstrap', 'app.config', 'RemoteResourse'])
var custom = false;

app.config(['$httpProvider', function ($httpProvider) {
    delete $httpProvider.defaults.headers.common['X-Requested-With'];
    $httpProvider.defaults.useXDomain = true;
}]);
var levels = ['Junior', 'Staff', 'Senior'];
/**
* Variable where the configuration's values for this script are stored. Will be filled when the controller is initialized.
*/
var config;
/**
* Variable to store the urls.
*/
var urls;

var visitor = false;
var resultsId = null;

app.factory('$log', function () {
    var ajaxAppender, logger;
    logger = log4javascript.getLogger('teamnet_logger');
    var authorization = 'Bearer ' + sessionStorage.getItem('token');
    ajaxAppender = new log4javascript.AjaxAppender("http://localhost:5859/api/log");
    ajaxAppender.setLayout(new log4javascript.JsonLayout());
    ajaxAppender.addHeader("Content-Type", "application/json; charset=utf-8");
    ajaxAppender.addHeader("Authorization", authorization);
    ajaxAppender.setThreshold(log4javascript.Level.ALL);
    logger.addAppender(ajaxAppender);
    return logger;
});

app.controller('TableController', function ($scope, $http, $filter, ngTableParams, RESULTS_CONFIG, MENU_CONFIG, URLs, $log, RemoteResourseFactory) {
    if (sessionStorage.getItem('token') == null) {
        $window.location.replace(URLs.LOGIN_URL);
    }

    resultsId = getQueryVariable("id");
    if (resultsId == null) {
        resultsId = getId(sessionStorage.getItem('orid'));
        visitor = true;
    } else {
        visitor = false;
    }

    $scope.username = sessionStorage.getItem('username');
    $scope.lastname = sessionStorage.getItem('lastname');
    $scope.menus = MENU_CONFIG.ITEMS;
    $scope.config = RESULTS_CONFIG;
    $scope.token = sessionStorage.getItem('token');
    $http.defaults.headers.common["Authorization"] = 'Bearer ' + $scope.token;
    config = RESULTS_CONFIG;
    urls = URLs;
    $scope.level = levels[0];
    $scope.menuLevels = levels;
    //The messages variable are set in this global variable.
    $scope.messages = config.MESSAGES;
    var types = [{ name: config.TYPE_1_NAME, secondName: config.TYPE_1_SECONDNAME, percentage: config.TYPE_1_PERCENTAGE },
             { name: config.TYPE_2_NAME, secondName: config.TYPE_2_SECONDNAME, percentage: config.TYPE_2_PERCENTAGE },
             { name: config.TYPE_3_NAME, secondName: config.TYPE_3_SECONDNAME, percentage: config.TYPE_3_PERCENTAGE },
             { name: config.TYPE_4_NAME, secondName: config.TYPE_4_SECONDNAME, percentage: config.TYPE_4_PERCENTAGE }]
    var max = config.MAX_SCORE;
    var max_difference = config.MAX_DIFFERENCE;
    var min_priority = config.HIGH_PRIORITY;
    var params = "/" + resultsId;
    RemoteResourseFactory.getPromise(URLs.SERVICE + URLs.RESULT + params).then(function (response) {
            $log.info("the results were loaded successfully");
            $scope.competencies = response.competences;

            createGraphic(response.competences);
            $log.info("the graphics were loaded successfully");
            $scope.tableParams = new ngTableParams({
                page: config.DEFAULT_VALUES.INITIAL_PAGE,           // show first page
                count: config.DEFAULT_VALUES.PAGES_COUNT          // count per page
            }, {
                groupBy: 'name',
                total: $scope.competencies.length, 
                /**
                 * Method of ng-table
                 * @method getData
                 * @param {} $defer
                 * @param {} params
                 * @return 
                 */
                getData: function ($defer, params) {
                    var orderedData = params.sorting() ?
                                        $filter('orderBy')($scope.competencies, params.orderBy()) :
                                        $scope.competencies;

                    $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                }
            });
            $scope.tableParams.reload();
            $log.info("Tables were reloaded successfully");

    }, function (status) {
        $log.error("no results were obtained from the server.");
        alert("error http connection:" + status);
    });

    /**
     * This method order by priority the competencies recieved.
     * @method orderByPriority
     * @param {} competency1
     * @param {} competency2
     * @return BinaryExpression
     */
    function orderByPriority(competency1, competency2) {
        var priorityA = competency1.expecteds[$scope.currentIndex].priority;
        var priorityB = competency2.expecteds[$scope.currentIndex].priority;
        return priorityB - priorityA;
    }

    /**
     * Calculate the required or extra values respect to the expected.
     * @method calculateRequired
     * @param {} expected
     * @param {} value
     * @return need
     */
    function calculateRequired(expected, value) {
        var need = (expected - value).toFixed(config.DECIMAL);
        return need;
    }

    /**
     * Calculate the value in percentage.
     * @method calculatePercentage
     * @param {} expected
     * @param {} result
     * @return percentage
     */
    function calculatePercentage(expected, result) {
        var percentage = ((result * 100) / expected).toFixed(config.DECIMAL);
        return percentage;
    }

    /**
     * Add a new result to array of the competencies, the average of the all evaluators.
     * @method addEvaluator
     * @param {} competency
     * @param {} evaluator
     * @return 
     */
    function addEvaluator(competency, evaluator) {
        if (!(competency.results[0].evaluator === evaluator)) {
            var newResult = [{ evaluator: evaluator, value: competency.average }];
            for (var i = 0; i < competency.results.length; i++) {
                newResult.push(competency.results[i]);
            }
            competency.results = newResult;
            $log.info("the evaluator has been added.");
        }
    }

    /**
     * set the competency's value to show in view.
     * @method definingResults
     * @param {} competency
     * @param {} index
     * @return 
     */
    function definingResults(competency, index) {
        competency.competencyStacked = [];
        competency.expectedAvg = competency.expecteds[index].valueExpected;
        competency.needAvg = calculateRequired(competency.expectedAvg, 
                                           competency.average);
        competency.percentage = calculatePercentage(competency.expectedAvg, 
                                                    competency.average);
            }

    /**
     * Set the results value to show in view
     * @method definingEvaluator
     * @param {} result
     * @param {} competency
     * @param {} index
     * @return 
     */
    function definingEvaluator(result, competency, index) {
        result.resultStacked = [];
        result.expected = competency.expecteds[index].valueExpected;
        result.need = calculateRequired(result.expected, result.value);
        result.percentage = calculatePercentage(result.expected, result.value);
        }

    /**
     * Function that work with Angular's forEach, to show or hide an element of the table.
     * @method switchedGroup
     * @param {} state
     * @return 
     */
    $scope.switchedGroup = function (state) {
        angular.forEach($scope.competencies, function (competency) {
            competency.$hideRows = state;
        });
    }

    /**
     * Initializes the competency average.
     * @method initAverages
     * @param {} competency
     * @return 
     */
    $scope.initAverages = function (competency) {
        competency.expectedAvg = competency.expecteds[0].valueExpected;
    }

    /**
     * Update the view when select a level.
     * show the dates in order the priority or their weight
     * @method selectLevel
     * @param {} index
     * @return 
     */
    $scope.selectLevel = function (index) {
        var competencySort = [];
        $scope.currentIndex = index;
        $scope.level = levels[index];

        competencySort = $scope.competencies.sort(orderByPriority);
        createGraphic(competencySort);
        $log.info("the graph has been organized");

        for (var i = 0; i < $scope.competencies.length; i++) {
            $scope.progressAvg($scope.competencies[i], index);
            var results = $scope.competencies[i].results;
            for (var j = 0; j < results.length; j++) {
                $scope.progressEvaluator(results[j], $scope.competencies[i], index);
    }
        }
    }

    /**
     * Function that calculate the progress's values for AVG
     * @method progressAvg
     * @param {} competency
     * @param {} index
     * @return 
     */
    $scope.progressAvg = function (competency, index) {
        definingResults(competency, index);

        var typeBar = types[types.length - 1];
        var first = {
            value: competency.average,
                      percentage: competency.percentage, 
                      type: typeBar.name, 
                      show: true
                    };
        competency.competencyStacked.push(first);

        addEvaluator(competency, 'AVG');

        for (var i = 0; i < types.length; i++) {
            var expectedPercentage = types[i].percentage * competency.expectedAvg;

            if (competency.average <= expectedPercentage) {
                competency.competencyStacked = [];
                typeBar = types[i];
                first = {
                    value: competency.average,
                          percentage: competency.percentage,
                          type: typeBar.name, 
                          show: true 
                         };
                competency.competencyStacked.push(first);

                if (competency.needAvg != 0) {
                    second = {
                        value: competency.needAvg,
                               type: typeBar.secondName, 
                               show: false
                             };
                    competency.competencyStacked.push(second);
                }
                i = types.length;
            }
        }
    }

    /**
     * Function that calculate the progress's values for Evaluators  
     * @method progressEvaluator
     * @param {} result
     * @param {} competency
     * @param {} index
     * @return 
     */
    $scope.progressEvaluator = function (result, competency, index) {
        definingEvaluator(result, competency, index);

        var typeResult = types[types.length - 1];
        var first = {
            value: result.value,
                      percentage: result.percentage, 
                      type: typeResult.name, 
                      show: true 
                    };
        result.resultStacked.push(first);

        for (var i = 0; i < types.length; i++) {
            var typeValue = types[i].percentage * result.expected;
            if (result.value <= typeValue) {
                result.resultStacked = [];
                typeResult = types[i];
                first = {
                    value: result.value,
                                 percentage: result.percentage,
                                 type: typeResult.name,
                                 show: true
                        };
                result.resultStacked.push(first);
                if (result.need != 0) {
                    var second = {
                        value: result.need,
                                   type: typeResult.secondName, 
                                   show: false 
                                 };
                    result.resultStacked.push(second);
                }
                i = types.length;
            }
        }
    }

    /**
     * Condition to highlight a competency by the score difference criteria.
     * @method isDiffSuggested
     * @param {} competency
     * @return BinaryExpression
     */
    $scope.isDiffSuggested = function (competency) {
        return competency.expectedAvg - competency.average >= max_difference;
    }

    /**
     * Condition to highlight a competency by the priority criteria.
     * @method isValueSuggested
     * @param {} competency
     * @return BinaryExpression
     */
    $scope.isValueSuggested = function (competency) {
        return competency.expectedAvg >= min_priority;
    }

    /**
     * Condition to highlight a competency by both the previous criteria.
     * @method bothSuggested
     * @param {} competency
     * @return LogicalExpression
     */
    $scope.bothSuggested = function (competency) {
        return !$scope.isSelected(competency) && $scope.isDiffSuggested(competency) && $scope.isValueSuggested(competency);
    }

    /**
     * Condition to highlight a competency when it's selected.
     * @method isSelected
     * @param {} competency
     * @return BinaryExpression
     */
    $scope.isSelected = function (competency) {
        return competency.selected == true;
    }

    /**
     * Function to change the competency's message when it alternates between selected and it's initial state.
     * @method changeMessage
     * @param {} competency
     * @return 
     */
    $scope.changeMessage = function (competency) {
        if ($scope.isSelected(competency)) {
            competency.message = $scope.messages.SELECTED;
        } else if ($scope.bothSuggested(competency)) {
            competency.message = $scope.messages.BOTH;
        } else if ($scope.isDiffSuggested(competency)) {
            competency.message = $scope.messages.DIFF_SUGGESTED;
        } else if ($scope.isValueSuggested(competency)) {
            competency.message = $scope.messages.VALUE_SUGGESTED;
        } else {
            competency.message = $scope.messages.DEFAULT_TEXT;
    }
    }

    /**
     * Initialize all necesary methods in view.
     * @method initialize
     * @param {} competency
     * @return 
     */
    $scope.initialize = function (competency) {
        competency.selected = false;
        competency.message = competency.name;
        $scope.initAverages(competency);
        $scope.changeMessage(competency);
    }

    /**
     * Alternates the state of the competency between selected and unselected.
     * @method changeSelection
     * @param {} competency
     * @return 
     */
    $scope.changeSelection = function (competency) {
        competency.selected = !competency.selected;
        $scope.changeMessage(competency);
            }
        });

app.controller('TasksModalCtrl', function ($scope, $modal) {
    $scope.items = ['item1', 'item2', 'item3'];
    $scope.open = function () {

        var modalInstance = $modal.open({
            templateUrl: '/tasksList.html',
            controller: 'TasksModalInstanceCtrl',
            size: 'sm',
            resolve: {
                items: function () {
                    return $scope.items;
                }
            }
        });
        modalInstance.result.then(function (selectedItem) {
            $scope.selected = selectedItem;
        }, function () {

        });
    };
    $scope.deleteItem = function (item) {
        var index = $scope.items.indexOf(item);
        $scope.items.splice(index, 1);
        $scope.$apply();
    }
});

app.controller('TasksModalInstanceCtrl', function ($scope, $modalInstance, items) {

    $scope.items = items;
    $scope.selected = {
        item: $scope.items[0]
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
});

var ModalInstanceCtrl = function ($scope, $modalInstance, $http, $filter, data) {
    $scope.task = createJsonTask();
    $scope.pointname = data.name;
    var result = data.results[0];
    $scope.resultScore = result.value;
    $scope.expectedScore = result.expected;
    $scope.create = function () {
        var task = createJsonTask();
        updateTask(task, $scope.task, $filter);
        $http.post(urls.SERVICE + urls.TASK, task).
            success(function (data) {
                $scope.taskCreated = data;
            });
        $modalInstance.close(task);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
};

//insert the value to the array.
function insertValues(array, label, value) {
    array.push({
        label: label,
        data: value
    });
}

//create the array with points to draw.
function createArrayOfArrays(Array, length) {
    for (i = 0; i < length; i++) {
        Array[i] = [];
    }
}

//insert competence to specific point
function insertValueToPosition(array, positionGeneral, position, value) {
    array[positionGeneral].push([position, value]);
}

//insert label to competence to specific position
function insertLabelToPosition(array, position, nameLabel) {
    array[position] = (nameLabel);
}

function createGraphic(series) {
    var listPoints = series;
    var dataValues = new Array();
    var dataExpecteds = new Array();
    var dataLabels = [];
    var dataLabelResult = [];
    var dataLabelsExpecteds = [];
    var lenghtResults = listPoints[0].results.length;
    var lenghtExpecteds = listPoints[0].expecteds.length;

    createArrayOfArrays(dataValues, lenghtResults);
    createArrayOfArrays(dataExpecteds, lenghtExpecteds);

    //insert the values of the arrays
    $.each(listPoints, function (position, point) {
        var positionPoint = position;
        var namePoint = point.name;
        dataLabels.push([position, namePoint]);
        $.each(point.results, function (positionResult, result) {
            var resValue = result.value;
            var nameValue = result.evaluator;
            insertLabelToPosition(dataLabelResult, positionResult, nameValue);
            insertValueToPosition(dataValues, positionResult, position, resValue);
        });
        $.each(point.expecteds, function (positionExpected, expected) {
            var nameExpected = expected.level;
            var resValue = expected.valueExpected;
            insertValueToPosition(dataExpecteds, positionExpected, position, resValue);
            insertLabelToPosition(dataLabelsExpecteds, positionExpected, nameExpected);
        });
    });;

    //inserts the dots in a matrix to draw
    var data2 = [];
    for (j = 0; j < lenghtResults; j++) {
        insertValues(data2, dataLabelResult[j], dataValues[j])
    }

    for (i = 0; i < lenghtExpecteds; i++) {
        insertValues(data2, dataLabelsExpecteds[i], dataExpecteds[i]);
    }
    
    removeComponents("choices");
    
    var minValue = -1;
    var maxValue = listPoints.length + 1;
    if (listPoints.length > 24) {
        var lengthDiv = 40 * listPoints.length;
        var stringWidth = "width:" + lengthDiv + "px";
        var stringW = lengthDiv + "px";
        document.getElementById("placeholder").style.width = stringW;
    }
    else {
        minValue = -0.2;
        maxValue = listPoints.length - 0.8;
    }

    //set the options to draw
    var options = {
        series: {
            lines: {
                show: true,
                fillColor: { colors: [{ opacity: 0.3 }, { opacity: 0.1 }] }
            },
            points: {
                show: true,
                radius: 3
            }
        },
        grid: {
            hoverable: true,
            clickable: true
        },
        xaxis: {
            min: minValue,
            max: maxValue,
            tickLength: 0,
            labelAngle: 315,
            axisLabel: "Points",
            axisLabelUseCanvas: true,
            axisLabelPadding: 10,
            show: true,
            ticks: dataLabels
        }
    };

    // insert checkboxes to the graphic
    var choiceContainer = $("#choices");
    $.each(data2, function (key, val) {
        choiceContainer.append("<br/><input type='checkbox' name='" + key +
        "' checked='checked' id='id" + key + "'></input>" +
        "<label for='id" + key + "'>"
        + val.label + "</label>");
    });

    choiceContainer.find("input").click(plotAccordingToChoices);

    function plotAccordingToChoices() {

        var data = [];

        choiceContainer.find("input:checked").each(function () {
            var key = $(this).attr("name");
            if (key && data2[key]) {
                data.push(data2[key]);
            }
        });

        if (data.length > 0) {
            $.plot("#placeholder", data, options);
        }
    }

    plotAccordingToChoices();
    addToolTip();
    hoverPoints(dataLabels);
}

//hover of points on the graphic
function hoverPoints(dataLabels) {
    $("#placeholder").bind("plothover", function (event, pos, item) {

        if ($("#enablePosition:checked").length > 0) {
            var str = "(" + pos.x.toFixed(2) + ", " + pos.y.toFixed(2) + ")";
            $("#hoverdata").text(str);
        }

        if ($("#enableTooltip:checked").length > 0) {
            if (item) {
                var x = item.datapoint[0].toFixed(2),
                y = item.datapoint[1].toFixed(2);

                var xposition = parseInt(x);

                $("#tooltip").html(item.series.label + " of " + dataLabels[xposition][1] + " = " + y).css({ top: item.pageY + 5, left: item.pageX + 5 }).fadeIn(200);
            } else {
                $("#tooltip").hide();
            }
        }
    });

    $("#placeholder").bind("plotclick", function (event, pos, item) {
        if (item) {
            $("#clickdata").text(" - click point " + dataLabels[item.dataIndex][1] + " in " + item.series.label);
            plot.highlight(item.series, item.datapoint);
        }
    });
}

//adds the tooltip to the graphic
function addToolTip() {
    $("<div id='tooltip'></div>").css({
        position: "absolute",
        display: "none",
        border: "1px solid #fdd",
        padding: "2px",
        "background-color": "#fee",
        opacity: 0.80
    }).appendTo("body");
}

//remove the childrens of a component.
function removeComponents(componentName) {
    var nodes = document.getElementById(componentName);
    while (nodes.firstChild) {
        nodes.removeChild(nodes.firstChild);
    }
}


function createJsonTask() {
    var jsonTask = {
        name: "",
        description: "",
        progress: 0,
        userId: "#13:0",
        competencyId: "#12:0",
        initialDate: "",
        endDate: "",
        reviewer: "",
        finalProduct: "",
        score: 0,
        isSuggestion: false,
        state: "enabled"
    };
    return jsonTask;
}

function getQueryVariable(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == variable) { return pair[1]; }
    }
    return (null);
}

function getId(orid) {
    var result = orid.split(":");
    return result[1];
}

function updateTask(oldTask, newTask, filter) {
    oldTask.name = newTask.name;
    oldTask.description = newTask.description;
    oldTask.progress = newTask.progress;
    oldTask.userId = newTask.userId;
    oldTask.competencyId = newTask.competencyId;
    oldTask.initialDate = newTask.initialDate;
    oldTask.endDate = newTask.endDate;
    oldTask.reviewer = newTask.reviewer;
    oldTask.finalProduct = newTask.finalProduct;
    oldTask.score = newTask.score;
    oldTask.isSuggestion = newTask.isSuggestion;
    oldTask.state = newTask.state;
    var date = new Date(newTask.initialDate);
    oldTask.initialDate = filter('date')(date, 'yyyy-MM-dd');
    date = new Date(newTask.endDate);
    oldTask.endDate = filter('date')(date, 'yyyy-MM-dd');
}
