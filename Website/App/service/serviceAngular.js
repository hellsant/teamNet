angular
.module("AngularModule", [])
.factory("AngularFactory", ['$http', '$scope', '$modal', 'RemoteResourseFactory', function ($http, $scope, $modal, RemoteResourseFactory) {

    var factorie = {};
    factorie.getData = function (urlToSend) {
        var request;
        RemoteResourseFactory.getPromiseParams(urlToSend).then(function (data) {
             request = data;
        }, function (status) {
            alert("error http connection:" + status);
        });
        return request;
    };

}])