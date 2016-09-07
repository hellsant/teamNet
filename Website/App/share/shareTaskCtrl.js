var app = angular.module('ShareTask', ['ui.bootstrap', 'app.config', 'selectize', 'RemoteResourse']);
var config;

app.controller('ShareTaskCtrl', ['$scope', '$http', '$window', 'SHARE_CONFIG', 'INDEX_CONFIG', 'URLs', '$modalInstance', 'task', 'RemoteResourseFactory',
    function ($scope, $http, $window, SHARE_CONFIG, INDEX_CONFIG, URLs, $modalInstance, task, RemoteResourseFactory) {
        if (sessionStorage.getItem('token') == null) {
            $window.location.replace(URLs.LOGIN_URL);
        }
    $scope.config = SHARE_CONFIG;
    $scope.task = task;
    $scope.commentShare = "";

    var url = URLs.SERVICE + URLs.GROUPS;
    $scope.token = sessionStorage.getItem('token');
    $http.defaults.headers.common["Authorization"] = 'Bearer ' + $scope.token;
    $http.get(url)
        .success(function (result) {
        $scope.groups = result;
    })
    .error(function () {
        alert("error");
    });

    $scope.share = function () {
        var urlShare = URLs.SERVICE + URLs.PUBLICATION;
        $http.post(urlShare, createJsonShareTask()).
                        success(function (data) {
                            $scope.shared = data;
                        });
        
        $modalInstance.close();
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    function createJsonShareTask() {
        var idGroups = recoverIdGroups();
        var publication = {
            comment: $scope.commentShare,
            publicator: $scope.task.userId,
            groupsIds: idGroups,
            task: $scope.task.id
        }
        return publication;
    }

    function recoverIdGroups() {
        var result = [];
        angular.forEach($scope.groups, function (group) {
            result.push(group);
        });
        return result;
    }

    $scope.groups;
    $scope.groupsConfig = {
        persist: false,
        maxItems: null,
        valueField: 'id',
        labelField: 'name',
        options: [],
        searchField: ['name'],
        render: {
            item: function (item, escape) {
                return '<div>' +
                    (item.name ? '<span class="name">' + escape(item.name) + '</span>' : '') +
                '</div>';
            },
            option: function (item, escape) {
                var label = item.name;
                var caption = item.name ? "" : null;
                return '<div>' +
                    '<span>' + escape(label) + '</span>' +
                    (caption ? '<span class="caption">' + escape(caption) + '</span>' : '') +
                '</div>';
            }
        }

    }
}]);