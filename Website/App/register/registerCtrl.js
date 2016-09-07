angular

.module('UserRegister', ['ui.bootstrap', 'app.config'])

.config(function ($httpProvider) {
    $httpProvider.defaults.useXDomain = true;
    delete $httpProvider.defaults.headers.common['X-Requested-With'];
})

.factory('UserFactory', function () {
    var user = {
        name: "",
        lastname: "",
        email: "",
        password: "",
    }
    return user;
})

.controller('UserRegisterCtrl', ['$scope', '$http', '$window', 'UserFactory', 'LOGIN_CONFIG', 'URLs', '$modalInstance',
    function ($scope, $http, $window, UserFactory, LOGIN_CONFIG, URLs, $modalInstance) {
        $scope.token = sessionStorage.getItem('token');
        $http.defaults.headers.common["Authorization"] = 'Bearer ' + $scope.token;
    $scope.emailExistent = false;
    $scope.passwordsNotEquals = false;
    $scope.saveUser = function () {
        if (!$scope.emailExistent && !$scope.passwordsNotEquals) {
            var newUser = UserFactory;
            recoverUser(newUser, $scope.newUser);
            $http.post(URLs.SERVICE + URLs.USER, newUser).
                success(function (data) {
                    $scope.userCreated = data;
                    sessionStorage.setItem('username', data.name);
                    sessionStorage.setItem('lastname', data.lastName);
                    sessionStorage.setItem('orid', data.orid);
                    sessionStorage.setItem('userEmail', data.email);
                    $window.location.replace(URLs.LOGIN_VIEW_URL);
            })
        }
    };

    $scope.verifyFormat = function () {
        $http.get(URLs.SERVICE + URLs.USER + "/" + URLs.USER + "/" + $scope.newUser.email).
            success(function (data) {
                $scope.userExistent = data;
                var name = $scope.userExistent.name;
                if (name != "") {
                    $scope.emailExistent = true;
                }
        })
    };

    $scope.comparePasswords = function () {
        var pass = $scope.newUser.password;
        var confirmPass = $scope.newUser.confirmpassword;
        if (pass != confirmPass) {
            $scope.passwordsNotEquals = true;
        }
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
}]);

function recoverUser(user, newUser) {
    user.name = newUser.name;
    user.lastname = newUser.lastname;
    user.email = newUser.email;
    user.password = newUser.password;
}