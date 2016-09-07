var suggested = angular.module("suggestion", ['ui.bootstrap', 'app.config', 'RemoteResourse']);
var config;

suggested.controller("SuggestionCtrl", ['$scope', 'SUG_CONFIG', 'MENU_CONFIG', function ($scope, SUG_CONFIG, MENU_CONFIG) {
    $scope.username = sessionStorage.getItem('username');
    $scope.lastname = sessionStorage.getItem('lastname');
    $scope.menus = MENU_CONFIG.ITEMS;
    $scope.config = SUG_CONFIG;
    config = SUG_CONFIG;
}]);

suggested.controller('ListCtrl', ['$scope', '$http', 'URLs', 'RemoteResourseFactory',
    function ($scope, $http, URLs, RemoteResourseFactory) {
        if (sessionStorage.getItem('token') == null) {
            $window.location.replace(URLs.LOGIN_URL);
        }
        $scope.token = sessionStorage.getItem('token');
        $http.defaults.headers.common["Authorization"] = 'Bearer ' + $scope.token;

    var currentSuggestions = [];
    RemoteResourseFactory.getPromise(URLs.SERVICE + URLs.SUGGESTIONS).then(function (data) {
        currentSuggestions = data;
        $scope.showSuggestions();
        loadSuggestion();
    }, function (status) {
        alert("error http connection:" + status);
    });
    $scope.itemsPerPage = config.ITEMS_PER_PAGE;
    $scope.currentPage = config.INITIAL_PAGE;
    $scope.maxSize = config.MAX_SIZE;

    $scope.showSuggestions = function () {
        var begin = (($scope.currentPage - 1) * $scope.itemsPerPage);
        var end = begin + $scope.itemsPerPage;
        $scope.suggestions = currentSuggestions.slice(begin, end);
    };

    $scope.pageChanged = function () {
        $scope.showSuggestions();
    };

    $scope.showSuggestions();

    function loadSuggestion() {
        var pathImage = '';
        $scope.totalItems = currentSuggestions.length;
        for (var i = 0; i < currentSuggestions.length; i++) {
            switch (currentSuggestions[i].level) {
                case JUNIOR: pathImage = config.IMAGES.JUNIOR; break;
                case STAFF: pathImage = config.IMAGES.STAFF; break;
                case SENIOR: pathImage = config.IMAGES.SENIOR; break;
            }
            currentSuggestions[i].image = pathImage;
        }
    }
}]);

var JUNIOR = 'junior';
var STAFF = 'staff';
var SENIOR = 'senior';