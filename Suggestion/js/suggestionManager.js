var app = angular.module('Suggestions', []);

app.config(function ($httpProvider) {
    $httpProvider.defaults.useXDomain = true;
    delete $httpProvider.defaults.headers.common['X-Requested-With'];
});

app.controller('dataController', ['$scope', '$http', dataController]);

function dataController($scope, $http) {
    $http.get('http://localhost:5859/api/suggestion')
            .success(function (response) {
                $scope.suggestion = response;
            }
        ).error(function () {
            alert("error");
        }
        );
}