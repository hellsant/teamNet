var bootstrap = angular.module('myApp', ['ui.bootstrap']);

bootstrap.controller('Controller', ['$scope', function($scope) {
  $scope.suggestions = [];
  $scope.totalItems = defaultSuggestions.length;
  $scope.currentPage = 1;
  $scope.itemsPerPage = 3;
  $scope.maxSize = 4;

  $scope.pageChanged = function() {
  	$scope.figureOutTodosToDisplay();
  };

  $scope.figureOutTodosToDisplay = function() {
    var begin = (($scope.currentPage - 1) * $scope.itemsPerPage);
    var end = begin + $scope.itemsPerPage;
    $scope.suggestions = defaultSuggestions.slice(begin, end);
  };
  
  $scope.figureOutTodosToDisplay();

  $scope.rate = 5;
  $scope.max = 10;
  $scope.isReadonly = false;

  $scope.hoveringOver = function(value) {
    $scope.overStar = value;
    $scope.percent = 100 * (value / $scope.max);
  };

  $scope.ratingStates = [
    {stateOn: 'glyphicon-ok-sign', stateOff: 'glyphicon-ok-circle'},
    {stateOn: 'glyphicon-star', stateOff: 'glyphicon-star-empty'},
    {stateOn: 'glyphicon-heart', stateOff: 'glyphicon-ban-circle'},
    {stateOn: 'glyphicon-heart'},
    {stateOff: 'glyphicon-off'}
  ];

  $scope.selection = 0;
}]);

/*
app.controller('RESTController', ['$scope', '$http', function($scope, $http) {
	$http.get('http://localhost:10189/api/suggestions')
	.success(function(data) {
	});
}]);
*/

var defaultSuggestions = [{image: 'images/junior.png', alt: 'Junior', description: 'Suggestion Number 1', point: 'Inprovider', area: 'Test', autor:"Mauricio Marquez", time: 3},
						  {image: 'images/junior.png', alt: 'Junior', description: 'Suggestion Number 2', point: 'au', area: 'Research', autor:"Jose Perez", time: 12},
        				  {image: 'images/junior.png', alt: 'Junior', description: 'Suggestion Number 3', point: 'au', area: 'Design', autor:"Maradonio Estadra", time: 2},
        				  {image: 'images/staff.png', alt: 'Staff', description: 'Suggestion Number 14', point: 'au', area: 'Design', autor:"Edwin Marquez", time: 16},
        				  {image: 'images/staff.png', alt: 'Staff', description: 'Suggestion Number 15', point: 'au', area: 'lol', autor:"Mauricio Mostacedo", time: 8},
        				  {image: 'images/staff.png', alt: 'Staff', description: 'Suggestion Number 16', point: 'au', area: 'lol', autor:"Mauricio Marquez", time: 2},
        				  {image: 'images/staff.png', alt: 'Staff', description: 'Suggestion Number 18', point: 'au', area: 'lol', autor:"Mauricio Marquez", time: 4},
        				  {image: 'images/staff.png', alt: 'Staff', description: 'Suggestion Number 19', point: 'au', area: 'lol', autor:"Esteban Suarez", time: 9},
        				  {image: 'images/junior.png', alt: 'Staff', description: 'Suggestion Number 20', point: 'au', area: 'lol', autor:"Pedro Arancibia", time: 10},
        				  {image: 'images/junior.png', alt: 'Staff', description: 'Suggestion Number 21', point: 'au', area: 'lol', autor:"Mauricio Marquez", time: 20},
        				  { image: 'images/staff.png', alt: 'Staff', description: 'Suggestion Number 23', point: 'au', area: 'lol', autor: "Mauricio Marquez", time: 5 }];
app.controller('EditCtrl',
  function ($scope, $http, $location, $routeParams, Projects) {
      $scope.token = sessionStorage.getItem('token');
      $http.defaults.headers.common["Authorization"] = 'Bearer ' + $scope.token;

      var projectId = $routeParams.projectId,
          projectIndex;

      $scope.projects = Projects;
      projectIndex = $scope.projects.$indexFor(projectId);
      $scope.project = $scope.projects[projectIndex];

      $scope.destroy = function () {
          $scope.projects.$remove($scope.project).then(function (data) {
              $location.path('/');
          });
      };
      $scope.save = function () {
          $http.post('http://localhost:5859/api/suggestion', CreateSuggestionJson(suggestion))
              .success(function (data) {

              }

      )
      };
  });
function CreateSuggestionJson(suggestion) {
    var jsonSuggested = {
        name: suggestion.name,
        description: suggestion.description,
        owner: suggestion.owner,
        valoration: suggestion.valoration,
        timeestimated: suggestion.timeestimated,
        point: suggestion.point,
        sugestedto: suggestion.sugestedto,
        score: suggestion.score,
        level : sugestion.level
    };
    var jsonString = angular.toJson(jsonTask);
    return jsonString;
}