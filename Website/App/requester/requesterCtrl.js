var app = angular.module('RequestForReview', ['ui.bootstrap', 'app.config']);
var config;

app.controller('RequesterCtrl', ['$scope', '$http', '$window', 'REQUESTER_CONFIG', 'INDEX_CONFIG', 'URLs', '$modalInstance', 'taskFactory', function ($scope, $http, $window, REQUESTER_CONFIG, INDEX_CONFIG, URLs, $modalInstance, taskFactory) {
    if (sessionStorage.getItem('token') == null) {
        $window.location.replace(URLs.LOGIN_URL);
    }
    $scope.config = REQUESTER_CONFIG;
    
    $scope.commentRequester = "";
    $scope.task = taskFactory.task;
    $scope.nameRef = "";
    $scope.ref = "";
    $scope.thereAreReferences = true;
    $scope.references = [];
    $scope.delete = true;
    $scope.token = sessionStorage.getItem('token');
    $http.defaults.headers.common["Authorization"] = 'Bearer ' + $scope.token;

    var url = URLs.SERVICE + URLs.USER_ID + "/" + getId($scope.task.reviewerId);
    $http.get(url).
    success(function (result) {
        $scope.nameReviewer = result;
    })
    .error(function () {
        alert("error");
    });

    $scope.addLink = function (name, url) {
        if (name != "" && url != "") {
            var reference = {
                name: name,
                url: url,
                taskId: $scope.task.id
            };
            reference.delete = true;
            $scope.references.push(reference);
            $scope.nameRef = "";
            $scope.ref = "";
            $scope.thereAreReferences = false;
        }
    };

    $scope.send = function () {
        var urlRequest = URLs.SERVICE + URLs.REQUEST;
        $http.post(urlRequest, createJsonRequest()).
                        success(function (data) {
                            $scope.requestSend = data;
                        });

        var urlReference = URLs.SERVICE + URLs.REFERENCE;
        $http.post(urlReference, $scope.references).
                        success(function (data) {
                            $scope.requestReference = data;
                        })
                        .error(function () {
                            alert("Same url aren't correct!");
                        });

        $modalInstance.close();
    };

    $scope.cancel = function () {
        if (confirm($scope.config.CONFIRM)) {
            $modalInstance.dismiss('cancel');
        }
    };

    $scope.removeRef = function (index, ref) {
        $scope.references.splice(index, 1);
        ref.delete = false;
    };

    function createJsonRequest() {
        var jsonRequest = {
            commentsFromRequester: $scope.commentRequester,
            reviewerId: $scope.task.reviewerId,
            taskId: $scope.task.id
        }
        return jsonRequest;
    }
}]);
