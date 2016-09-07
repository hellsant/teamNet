var config;
angular

.module('GroupRegister', ['ui.bootstrap', 'app.config', 'selectize'])

.config(function ($httpProvider) {
    $httpProvider.defaults.useXDomain = true;
    delete $httpProvider.defaults.headers.common['X-Requested-With'];
})

.factory('GroupFactory', function () {
    var group = {
        name: config.EDIT.NAME_PLCHOLDR,
        description: config.EDIT.DESC_PLCHOLDR,
        state: "",
        administrators: [],
        members: [],
        idAdministrators: [],
        idMembers: []
    }
    return group;
})

.controller('GroupRegisterCtrl', ['$scope', '$http', 'LOGIN_CONFIG', 'URLs', 'GROUPS_CONFIG', '$modalInstance', 'GroupFactory', 'oldGroup',
function ($scope, $http, LOGIN_CONFIG, URLs, GROUPS_CONFIG, $modalInstance, GroupFactory, oldGroup) {
    $scope.token = sessionStorage.getItem('token');
    $http.defaults.headers.common["Authorization"] = 'Bearer ' + $scope.token;
    config = GROUPS_CONFIG;
    if (editingGroup()) {
        $scope.title = config.VIEW.EDIT_TITLE;
    } else {
        $scope.title = config.VIEW.NEW_TITLE;
    }
    $http.get(URLs.SERVICE + URLs.USER).
            success(function (data) {
                $scope.emails = data;
            })

    $scope.saveGroup = function () {
        var newGroup = GroupFactory;
        recoverGroup(newGroup, $scope.newGroup, $scope.emails);
        if (editingGroup()) {
            $http.put(URLs.SERVICE + URLs.GROUPS, newGroup).
            success(function (data) {
                $scope.groupCreated = data;
                $modalInstance.dismiss('cancel');
            }).
        error(function (data, status, headers, config) {
            console.log(data.error);
            alert(data.error);
        });
        } else {
            $http.post(URLs.SERVICE + URLs.GROUPS, newGroup).
                success(function (data) {
                    $scope.groupCreated = data;
                    $modalInstance.close(newGroup);
                }).
            error(function (data, status, headers, config) {
                console.log(data.error);
                alert(data.error);
            });
        }
    };

    $scope.initFields = function () {
        $scope.newGroup = GroupFactory;
        if (editingGroup()) {
            $scope.newGroup = oldGroup;
        }
    }

    function editingGroup() {
        return oldGroup !== 'new';
    }

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
    //=======================================================
    //Email Contacts
    //=======================================================
    $scope.emails;


    var REGEX_EMAIL = '([a-z0-9!#$%&\'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&\'*+/=?^_`{|}~-]+)*@' +
                    '(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)';
    $scope.emailsConfig = {
        persist: false,
        maxItems: null,
        valueField: 'id',
        labelField: 'name',
        options: [],
        searchField: ['name', 'email'],
        render: {
            item: function (item, escape) {
                return '<div>' +
                    (item.name ? '<span class="name">' + escape(item.name) + '</span>' : '') +
                '</div>';
            },
            option: function (item, escape) {
                var label = item.name || item.email;
                var caption = item.name ? item.email : null;
                return '<div>' +
                    '<span>' + escape(label) + '</span>' +
                    (caption ? '<span class="caption">' + escape(caption) + '</span>' : '') +
                '</div>';
            }
        },
        createFilter: function (input) {
            var match, regex;

            // email@address.com
            regex = new RegExp('^' + REGEX_EMAIL + '$', 'i');
            match = input.match(regex);
            if (match) return !this.options.hasOwnProperty(match[0]);

            // name <email@address.com>
            regex = new RegExp('^([^<]*)\<' + REGEX_EMAIL + '\>$', 'i');
            match = input.match(regex);
            if (match) return !this.options.hasOwnProperty(match[2]);

            return false;
        },
        create: function (input) {
            if ((new RegExp('^' + REGEX_EMAIL + '$', 'i')).test(input)) {
                return { email: input };
            }
            var match = input.match(new RegExp('^([^<]*)\<' + REGEX_EMAIL + '\>$', 'i'));
            if (match) {
                return {
                    email: match[2],
                    name: $.trim(match[1])
                };
            }
            alert('Invalid email address.');
            return false;
        }
    }
}]);

function recoverGroup(group, newGroup, emails) {
    group.id = newGroup.id;
    group.name = newGroup.name;
    group.description = newGroup.description;
    group.state = 'Public';
    group.administrator = newGroup.administrators;
    group.members = newGroup.members;
    group.idAdministrators = [sessionStorage.getItem('orid')];
    group.idMembers = emails;
    group.hasPermission = true;
}