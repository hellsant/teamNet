angular
    .module('Group', ['ui.bootstrap', 'app.config', 'GroupRegister', 'selectize', 'RemoteResourse'])

    .controller('GroupsCtrl', function ($scope, $http, $window, GROUPS_CONFIG, URLs, MENU_CONFIG, $modal, RemoteResourseFactory) {

        if (sessionStorage.getItem('token') == null) {
            $window.location.replace(URLs.LOGIN_URL);
        }

        $scope.username = sessionStorage.getItem('username');
        $scope.lastname = sessionStorage.getItem('lastname');
        $scope.menus = MENU_CONFIG.ITEMS;
        config = GROUPS_CONFIG;
        $scope.config = GROUPS_CONFIG;
        $scope.isOwner = true;
        $scope.groupSelected = false;

        $scope.token = sessionStorage.getItem('token');
        $http.defaults.headers.common["Authorization"] = 'Bearer ' + $scope.token;

        var currentGroups = [];
        RemoteResourseFactory.getPromise(URLs.SERVICE + URLs.GROUPS).then(function (groups) {
            $scope.currentGroups = groups;
        }, function (status) {
            alert("error http connection:" + status);
        });


        $scope.selectedGroup = {
            id: '',
            name: '',
            description: ''
        };

        $scope.viewInformation = function (group) {
            $scope.selectedGroup = group;
        };

        $scope.ok = function (index, remMembers) {
            var actual = $scope.remMembers[index];
            var actualRol = "member";
            var roleUser = createRole($scope.selectedGroup, remMembers, actual, actualRol);
            $http.put(URLs.SERVICE + URLs.USERROLE, roleUser)
                    .success(function (data) {
                        $scope.newAdministrator = data;
                        alert(data.name + "Is a new owner os the group");
                    })
                    .error(function (status) {
                        console.log("error : " + status);
                    });
        };

        $scope.open = function () {
            var modalInstance = $modal.open({
                templateUrl: '../partials/registergroupmodal.html',
                controller: 'GroupRegisterCtrl',
                size: 'md',
                resolve: {
                    oldGroup: function () {
                        return 'new';
                    }
                }
            });

            modalInstance.result.then(function (newGroupToOpen) {
                $scope.selectedGroup = newGroupToOpen;
                $scope.groupSelected = true;
            });
        };

        $scope.editGroup = function () {
            var modalInstance = $modal.open({
                templateUrl: '../partials/registergroupmodal.html',
                controller: 'GroupRegisterCtrl',
                size: 'md',
                resolve: {
                    oldGroup: function () {
                        return $scope.selectedGroup;
                    }
                }
            });
        };

        $scope.chooseGroup = function (group) {
            $scope.selectedGroup = group;
            var userid = getId(sessionStorage.getItem('orid'));
            $scope.groupSelected = true;
            $scope.admins = [];
            $scope.remMembers = [];
            $scope.admins = $scope.selectedGroup.administrators;
            $scope.remMembers = remainingMembers($scope.admins, $scope.selectedGroup.members);

            for (var i = 0 ; i < $scope.admins.length ; i++) {
                if ($scope.username == $scope.admins[i].name) {
                    RemoteResourseFactory.getPromise(URLs.SERVICE + URLs.ROLE).then(function (data) {
                        $scope.roles = data;
                        if ($scope.roles.length > 0) {
                            $scope.roleMember = $scope.roles[1];
                        }
                    }, function (status) {
                        alert("error http connection:" + status);
                    });
                    $scope.isOwner = true;
                }
            }
            var groupRid = getId($scope.selectedGroup.id);
            var serviceUrl = URLs.SERVICE + URLs.GROUPS + '/' + URLs.PUBLICATION;
            var param = '/' + getId($scope.selectedGroup.id);


            $http.get(URLs.SERVICE + URLs.GROUPTASK + param)
            .success(function (data) {
                $scope.tasks = data;
            }).error(function (status) {
                alert('adfasdfasdfa')
                console.log(status);
            });

            RemoteResourseFactory.getPromiseParams(serviceUrl, param).then(function (result) {
                $scope.publications = result;
            }, function (status) {
                alert("error http connection:" + status);
            });
        };

        $scope.closeSelectedGroup = function () {
            $scope.groupSelected = false;
        };
        function remainingMembers(admins, members) {
            var result = [];
            for (var memberIndex in members) {
                var member = members[memberIndex];
                if (!contains(admins, member)) {
                    result.push(members[memberIndex]);
                }
            }
            return result;
        }

        function contains(admins, member) {
            for (var index in admins) {
                if (admins[index].id == member.id) {
                    return true;
                }
            }
            return false;
        }

        $http.get(URLs.SERVICE + URLs.USER).
                success(function (data) {
                    $scope.emails = data;
                });
        $scope.addMembers = function () {
            var groupName = $scope.selectedGroup.name;
            var members = $scope.emails;
            var groupMembers = {
                id: "",
                member: "",
                groupName: groupName,
                ridMembers: members
            }
            var groupMembersJson = angular.toJson(groupMembers);
            $http.post(URLs.SERVICE + URLs.USERROLE, groupMembersJson).
                success(function (data) {
                    $scope.membersAdded = data;
                })
        }

        
        $scope.disable = false;



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

        $scope.idSelected = null;
        $scope.link = null;

        $scope.selectUser = function (newId) {
            $scope.idSelected = getId(newId);
            $scope.link = "../profile/profile.html?id=" + $scope.idSelected;
        }

    })



function getId(orid) {
    var result = orid.split(":");
    return result[1];
};

function createRole(group, roleMember, actual, actualRol) {
    var role;
    role = {
        id: '',
        name: actual.id + group.name + actualRol,
        groupName: group.name,
        roleName: roleMember,
        userId: actual.id
    }
    return role;
};