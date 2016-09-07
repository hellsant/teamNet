var app = angular.module('TeamNet', ['ui.bootstrap', 'app.config']);
var config;
app.config(function ($httpProvider) {
    $httpProvider.defaults.useXDomain = true;
    delete $httpProvider.defaults.headers.common['X-Requested-With'];
});


app.controller('RequestsCtrl', ['$scope', '$http', '$window', 'REQUEST_CONFIG', 'INDEX_CONFIG', 'MENU_CONFIG', 'URLs',
    function ($scope, $http, $window, REQUEST_CONFIG, INDEX_CONFIG, MENU_CONFIG, URLs) {
        if (sessionStorage.getItem('token') == null) {
            $window.location.replace(URLs.LOGIN_URL);
        }
    var orid = sessionStorage.getItem('orid');
    $scope.username = sessionStorage.getItem('username');
    $scope.lastname = sessionStorage.getItem('lastname');
    $scope.menus = MENU_CONFIG.ITEMS;
    $scope.userId = getId(orid);
    $scope.error = false;
    $scope.token = sessionStorage.getItem('token');
    $http.defaults.headers.common["Authorization"] = 'Bearer ' + $scope.token;

    if ($scope.userId == null) {
        $window.location.replace(URLs.LOGIN_URL);
    } else {
        config = REQUEST_CONFIG;
        $scope.config = REQUEST_CONFIG;
        $scope.indexConfig = INDEX_CONFIG;
        var url = URLs.SERVICE + URLs.REQUEST_REVIEWER + "/" + $scope.userId;
        $http.get(url).
        success(function (result) {
            $scope.requests = result;
            $scope.error = false;
        })
        .error(function () {
            $scope.error = true;
        });
        $scope.isCollapsed = false;
        $scope.checkRequest = function (isApproved, request) {
            var requestForUpdate =
                {
                    id: request.requestId,
                    commentsFromReviewer: request.commentsFromReviewer,
                    approved: isApproved
                };
            var jsonString = angular.toJson(requestForUpdate);
            var urlPut = URLs.SERVICE + URLs.REQUEST;
            $http.put(urlPut, jsonString).
            success(function (data) {
                alert("It was modified succesfully");
            })
            .error(function (data, status, headers, config) {
                alert(config.ERRORS.NETWORK);
            });
        };
    }

}]);

function getId(orid) {
    var result = orid.split(":");
    return result[1];
}
