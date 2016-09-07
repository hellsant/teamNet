var app = angular.module('TeamNetLogin', ['ui.bootstrap', 'app.config', 'UserRegister']);
/**
* Variable where the configuration's values for this script are stored. Will be filled when the controller is initialized.
*/
var config;
//app.config(function ($httpProvider) {
//    $httpProvider.defaults.useXDomain = true;
//    delete $httpProvider.defaults.headers.common['X-Requested-With'];
//});

app.config(['$httpProvider', function ($httpProvider) {
    delete $httpProvider.defaults.headers.common['X-Requested-With'];
    $httpProvider.defaults.useXDomain = true;
}]);

app.controller('LoginCtrl', ['$scope', '$http', '$window', 'LOGIN_CONFIG', 'URLs', '$modal', function ($scope, $http, $window, LOGIN_CONFIG, URLs, $modal) {
    config = LOGIN_CONFIG;
    $scope.config = LOGIN_CONFIG;
    sessionStorage.removeItem('username');
    sessionStorage.removeItem('lastname');
    sessionStorage.removeItem('userEmail');
    sessionStorage.removeItem('orid');
    sessionStorage.removeItem('token');
    $scope.ok = function () {
        var userEmail = $scope.email;
        var userPassword = $scope.password;
        var dataJson = { email: userEmail, password: userPassword };
        var jsonString = angular.toJson(dataJson);
        var data = "grant_type=password&username=" + userEmail + "&password=" + userPassword;
        $http.post('http://localhost:5859/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {
            //$http.defaults.headers.common["Authorization"] = 'Bearer ' + response.access_token;
                $http.defaults.headers.common["Authorization"] = 'Bearer ' + response.access_token;
                $window.location.replace(URLs.LOGIN_VIEW_URL);
                sessionStorage.setItem('token', response.access_token);
                sessionStorage.setItem('lastname', response.lastName);
                sessionStorage.setItem('username', response.userName);
                sessionStorage.setItem('orid', response.idUser);
                sessionStorage.setItem('userEmail', $scope.email);
        }).error(function (response, status) {
            console.log(response);
            console.log(status);
        });
    };

    $scope.open = function () {
        var modalInstance = $modal.open({
            templateUrl: '../partials/registerusermodal.html',
            controller: 'UserRegisterCtrl',
            size: 'md'
        });
    };
}]);