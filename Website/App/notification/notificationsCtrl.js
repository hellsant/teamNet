var app = angular.module('TeamNet', ['ui.bootstrap', 'app.config']);
var config;
app.config(function ($httpProvider) {
    $httpProvider.defaults.useXDomain = true;
    delete $httpProvider.defaults.headers.common['X-Requested-With'];
});


app.controller('NotificationsCtrl', ['$scope', '$http', '$window', 'REQUEST_CONFIG', 'INDEX_CONFIG', 'MENU_CONFIG', 'URLs',
    function ($scope, $http, $window, REQUEST_CONFIG, INDEX_CONFIG, MENU_CONFIG, URLs) {
        if (sessionStorage.getItem('token') == null) {
            $window.location.replace(URLs.LOGIN_URL);
        }
    var orid = sessionStorage.getItem('orid');
    $scope.username = sessionStorage.getItem('username');
    $scope.lastname = sessionStorage.getItem('lastname');
    $scope.token = sessionStorage.getItem('token');
    $http.defaults.headers.common["Authorization"] = 'Bearer ' + $scope.token;
    $scope.menus = MENU_CONFIG.ITEMS;
    $scope.userId = getId(orid);
    $scope.error = false;
    if ($scope.userId == null) {
        $window.location.replace(URLs.LOGIN_URL);
    } else {
        config = REQUEST_CONFIG;
        $scope.config = REQUEST_CONFIG;
        $scope.indexConfig = INDEX_CONFIG;


        var url = URLs.SERVICE + URLs.REQUEST_OWNER + "/" + $scope.userId;
        $http.get(url).
        success(function (result) {
            $scope.requests = result;
            $scope.error = false;
        })
        .error(function () {
            $scope.error = true;
        });
        
    };
    
}]);

function getId(orid) {
    var result = orid.split(":");
    return result[1];
}