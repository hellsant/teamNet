var app = angular.module('main', ['ngTable', 'ui.bootstrap'])
var custom = false;
//The default tooltip messages for the points highlighting feature are stored in this variable.
var state_message = {
    diffsuggested: 'Your score is too low on this point compared to what you need, you should probably work on it',
    valuesuggested: 'This point has a high relevance for the level you are at, you should probably work on it',
    both: 'This point has high relevance for your level and your score is too low, you should work on it',
    selected: 'You have chosen to work on this point',
    default_text: 'Click the plus button to add tasks on this point and work on it'
}

var order_by= ['Junior', 'Staff', 'Senior'];
var selected=0;
var max_difference = 0;
var min_priority = 2;

var max_expected = 5;

var types = [{name: 'danger', percentage: 0.25},
             {name: 'warning', percentage: 0.50},
             {name: 'info', percentage: 0.75},
             {name: 'success', percentage: 1}];

var result_points = [];


app.config(function ($httpProvider) {
    $httpProvider.defaults.useXDomain = true;
    delete $httpProvider.defaults.headers.common['X-Requested-With'];
});


app.controller('DemoCtrl', function ($scope, $http, $filter, ngTableParams) {
    $scope.menusOrder = order_by;
    //The messages variable are set in this global variable.
    $scope.messages = state_message;
    $http.get('http://localhost:5859/api/result')
        .success(function (response) {
            $scope.points= response;


            for (var i = 0; i > response.expecteds.length; i++) {
                $scope.levels[i] = response.expecteds[i];
            }

            $scope.tableParams = new ngTableParams({
                page: 1,            // show first page
                count: 10           // count per page
            }, {
                groupBy: 'name',

            });

            $scope.tableParams.reload();
        });

        $scope.status = {
         isopen: false
        };
    //Initializes the average values for the point.
    $scope.initAverages = function (item) {
        var sum = 0;
        for (var result in item.results) {
            var val = item.results[result].value * 1;
            sum += val;
        }
        item.resultAvg = (sum / item.results.length).toFixed(3);
    }
    function orderByPriority(point1, point2) {
        var priorityA = point1.expecteds[$scope.currentIndex].priority;
        var priorityB = point2.expecteds[$scope.currentIndex].priority;
        return priorityB - priorityA;
    }

    $scope.selectLevel = function(index) {
        $scope.currentIndex = index;        
        $scope.points.sort(orderByPriority);

        for(var i = 0; i< $scope.points.length; i++) {
            $scope.progressAvg($scope.points[i], index);
            var results = $scope.points[i].results;
            for(var j = 0; j < results.length; j++) {
                //results[j].expected = expecteds[index].value;
                $scope.progressEvaluator(results[j], $scope.points[i], index);
            }
     }

        }
    

    $scope.progressAvg = function(point, index) {
        console.log(point.results[0].evaluator);
        if(!(point.results[0].evaluator === 'AVG')) {
            var newResult = [{evaluator: 'AVG', value: point.resultAvg}];
            for (var i = 0; i < point.results.length; i++) {
                newResult.push(point.results[i]);
    }
            point.results = newResult;
        }
       point.type = types[types.length-1].name;

        for (var i = 0; i < types.length; i++) {
            point.expectedAvg = point.expecteds[index].valueExpected;
            var expected =  types[i].percentage * point.expectedAvg;
            if (point.resultAvg <= expected) {
                point.type = types[i].name;
                
                
                i = types.length;
                }
            }
        }

    $scope.progressEvaluator = function(result, point, index){
        result.type= types[types.length-1].name;
        for (var i = 0; i < types.length; i++) {
            result.expected = point.expecteds[index].valueExpected;
            result.need = (result.expected - result.value).toFixed(3);
            var typeValue =  types[i].percentage * result.expected;
            if (result.value <= typeValue) {
                result.type = types[i].name; 
                i = types.length;
                }
            }
        }

    //Condition to highlight a point by the score difference criteria.
    $scope.isDiffSuggested = function (item) {
        return item.expectedAvg - item.resultAvg >= max_difference;
    }

    //Condition to highlight a point by the priority criteria.
    $scope.isValueSuggested = function (item) {
        return item.expectedAvg >= min_priority;
    }

    //Condition to highlight a point by both the previous criteria.
    $scope.bothSuggested = function (item) {
        return !$scope.isSelected(item) && $scope.isDiffSuggested(item) && $scope.isValueSuggested(item);
        }

    //Condition to highlight a point when it's selected.
    $scope.isSelected = function (item) {
        return item.selected == true;
    }

    //Function to change the point's message when it alternates between selected and it's initial state.
    $scope.changeMessage = function (item) {
        if ($scope.isSelected(item)) {
            item.message = $scope.messages.selected;
        } else if ($scope.bothSuggested(item)) {
            item.message = $scope.messages.both;
        } else if ($scope.isDiffSuggested(item)) {
            item.message = $scope.messages.diffsuggested;
        } else if ($scope.isValueSuggested(item)) {
            item.message = $scope.messages.valuesuggested;
        } else {
            item.message = $scope.messages.default_text;
        }
    }

    //Alternates the state of the point between selected and unselected.
    $scope.changeSelection = function (item) {
        item.selected = !item.selected;
        $scope.changeMessage(item);
    }


$(document).ready(function () {
    $.ajax({
        url: "http://localhost:5859/api/result",
        type: "GET",
        dataType: "json",
        success: onDataReceived
    });

    function onDataReceived(series) {
        var plotarea = $("#placeholder");
        var listPoints = series;
        var dataValues = new Array();
        var dataExpecteds = new Array();
        var dataLabels = [];
        var dataLabelResult = [];
        var dataLabelsExpecteds = [];
        var lenghtResults = listPoints[0].results.length;
        var lenghtExpecteds = listPoints[0].expecteds.length;
        
        for (i = 0; i < lenghtResults; i++) {
            dataValues[i] = [];
            dataExpecteds[i] = [];
        }

        $.each(listPoints, function (position, point) {
            var positionPoint = position;
            var namePoint = point.name;
            dataLabels.push([position, namePoint]);
            $.each(point.results, function (positionResult, result) {
                var resValue = result.value;
                var nameValue = result.evaluator;
                dataLabelResult[positionResult] = (nameValue);
                dataValues[positionResult].push([position, resValue]);
            });
            $.each(point.expecteds, function (positionExpected, expected) {
                var nameExpected = expected.level;
                var resValue = expected.valueExpected;
                dataExpecteds[positionExpected].push([position, resValue]);
                dataLabelsExpecteds[positionExpected] = (nameExpected);
            });
        });;

        var options = {
            series: {
                lines: {
                    show: true,
                    //fill: true,
                    fillColor: { colors: [{ opacity: 0.3 }, { opacity: 0.1 }] }
                },
                grid: {
                    hoverable: true,
                    clickable: true
                },
                points: {
                    show: true,
                    radius: 3
                }
            },
            xaxis: {
                min: -0.1,
                max: listPoints.length - 0.8,
                tickLength: 0,
                axisLabel: "Points",
                axisLabelUseCanvas: true,
                axisLabelPadding: 10,
                show: true,
                ticks: dataLabels
            }//,
            //colors: ["#FF7070", "#0022FF"],
        };

        var data2 = [];
        for (j = 0; j < lenghtResults; j++) {
            data2.push({
                label: dataLabelResult[j],
                data: dataValues[j]
            });
            //data2.push({
            //    label: dataLabelsExpecteds[j],
            //    data: dataExpecteds[j]
            //});
        }
        data2.push({
            label: dataLabelsExpecteds[0],
            data: dataExpecteds[0]
        });
        
        $.plot($("#placeholder"), data2, options);
    }    
});
});

