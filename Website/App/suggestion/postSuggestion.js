var suggested = angular.module("suggestion", ['ui.bootstrap', 'ui.filters', 'ui.select', 'app.config', 'RemoteResourse']);

suggested.config(['$httpProvider', function ($httpProvider) {
    delete $httpProvider.defaults.headers.common['X-Requested-With'];
    $httpProvider.defaults.useXDomain = true;
}]);

suggested.controller("SuggestionCtrl", ['$scope', '$http', '$window', 'SUG_CONFIG', 'MENU_CONFIG', 'URLs', 'RemoteResourseFactory',
    function ($scope, $http, $window, SUG_CONFIG, MENU_CONFIG, URLs, RemoteResourseFactory) {
        $scope.username = sessionStorage.getItem('username');
        $scope.lastname = sessionStorage.getItem('lastname');
        $scope.id = sessionStorage.getItem('orid');
        $scope.menus = MENU_CONFIG.ITEMS;
        $scope.config = SUG_CONFIG;
        $scope.levels = [];
        $scope.token = sessionStorage.getItem('token');
        $http.defaults.headers.common["Authorization"] = 'Bearer ' + $scope.token;
        /**
        * makes a request to the Rest services to recover the time.
        */
        RemoteResourseFactory.getPromise(URLs.SERVICE + URLs.TIME).then(function (data) {
            $scope.dataTimes = data;
            if ($scope.dataTimes.length > 0) {
                $scope.suggestion.Time = $scope.dataTimes[0];
            }
        }, function (status) {
            alert("error http connection:" + status);
        });

        /**
         * makes a request to the Rest services to recover the competency.
         */
        RemoteResourseFactory.getPromise(URLs.SERVICE + URLs.COMPETENCY).then(function (data) {
            $scope.competencies = data;
        }, function (status) {
            alert("error http connection:" + status);
        });

        /**
        * makes a request to the Rest services to recover the competency level.
        */
        RemoteResourseFactory.getPromise(URLs.SERVICE + URLs.COMPETENCY_LEVEL).then(function (data) {
            for (var i = 0; i < data.length; i++) {
                data[i].title = data[i].value + ' .- ' + data[i].description;
            }
            $scope.competenceLevel = data;
        }, function (status) {
            alert("error http connection:" + status);
        });

        /**
        * Sends information serialized form by post dase method to the data.
        */
        $scope.save = function (validForm) {
            if (validForm) {
                var newSuggestion = cloneSuggestion($scope.suggestion);
                $http.post(URLs.SERVICE + URLs.SUGGESTIONS, newSuggestion).success(function (created) {
                    if (created) {
                        $window.location.replace(URLs.SUGGESTION_VIEW_URL);
                    }
                });
            }
        };

        /**
        * It is responsible for selecting the name of the list of competencies to texbox. 
        */
        $scope.copyTo = function (index) {
            var start = index * SUG_CONFIG.LEVEL_COUNT;
            var end = start + SUG_CONFIG.LEVEL_COUNT;
            $scope.levels = $scope.competenceLevel.slice(start, end);
            $scope.suggestion.Competency = $scope.competencies[index];
            $scope.suggestion.ValueCompetecyLevel = $scope.levels[0];
        };

        /**
        * This method is charged with making the call to the service to request information from the user names in the database
        */
        $scope.searchUser = function () {
            var name = $('.undefined', '.custom-select-search').val();
            if (name.length > 1) {
                var param = '/' + name;
                RemoteResourseFactory.getPromise(URLs.SERVICE + URLs.USER + param).then(function (data) {
                    for (var i = 0; i < data.length; i++) {
                        data[i].fullname = data[i].name + ' ' + data[i].lastName;
                    }
                    $scope.users = data;
                }, function (status) {
                    alert("error http connection:" + status);
                });
            }
        };

        /**
        * Create the file JSon.
        */
        $scope.suggestion = {
            Id: '',
            Description: '',
            EstimatedTime: '',
            Time: {},
            Name: '',
            Score: '0',
            Valuation: '0',
            creatorofsuggestion: { name: $scope.username + ' ' + $scope.lastname, id: $scope.id },
            Competency: {},
            CreatedTo: { name: '', id: '' },
            ValueCompetecyLevel: {},
            DescriptionCompetencyLevel: '',
            Level: ''
        };
    }]);

function cloneSuggestion(suggestion) {
    var resultSuggestion = {
        Id: suggestion.Id,
        Description: suggestion.Description,
        EstimatedTime: suggestion.EstimatedTime,
        Time: getId(suggestion.Time.id),
        Name: suggestion.Name,
        Score: suggestion.Score,
        Valuation: suggestion.Valuation,
        creatorofsuggestion: getId(suggestion.creatorofsuggestion.id),
        Competency: getId(suggestion.Competency.id),
        CreatedTo: getId(suggestion.CreatedTo.id),
        ValueCompetecyLevel: getId(suggestion.ValueCompetecyLevel.id),
        DescriptionCompetencyLevel: suggestion.DescriptionCompetencyLevel,
        Level: suggestion.Level
    };
    return resultSuggestion;
}

function getId(orid) {
    var result = orid.split(":");
    return result[1];
}
