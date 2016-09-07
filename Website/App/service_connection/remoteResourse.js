angular
.module('RemoteResourse', [])
.factory('RemoteResourseFactory', ['$q', '$http', function ($q, $http) {
    var factories = {};
    var token = sessionStorage.getItem('token');
    $http.defaults.headers.common["Authorization"] = 'Bearer ' + token;
    /**
    * Call the servive when the URL dont recive a params.
    */
    factories.getPromise = function (urlRemote) {

        var defered = $q.defer();
        var promise = defered.promise;
        $http({
            method: 'GET',
            url: urlRemote,
            cache: true
        })
            .success(function (data, status, headers, config) {
                defered.resolve(data);
            })
            .error(function (data, status, headers, config) {
                defered.reject(status);
            });
        return promise;
    };

    /**
    * Call the service when the URL recive a params to sent.
    */
    factories.getPromiseParams = function (urlRemote, contains) {
        var defered = $q.defer();
        var promise = defered.promise;
        $http.get(urlRemote + contains)
            .success(function (data, status, headers, config) {
            defered.resolve(data);
            })
            .error(function (data, status, headers, config) {
                defered.reject(status);
            });
        return promise;
    };
    return factories;
}])
