var myApp = angular.module('myApp', ["angularUtils.directives.dirPagination", "angular.filter"]);



function MyController($scope, $http) {

    $scope.sortType = ''; // set the default sort type
    $scope.sortReverse = true;  // set the default sort order
    $scope.searchAddress = '';     // set the default search/filter term

    $scope.currentPage = 1;
    $scope.pageSize = 10;
 
    $scope.houses = [];
    $scope.hello = "hello world";

    $scope.init = function (name) {
        $scope.townName = name;
    }

    $http.get('/Analysis/GetHouses?townName=' + name)
        .then(function (response) {         //
            //alert("success with houses");
            $scope.houses = response.data;
            console.log($scope.houses);
            }),
        (function (data) {
            alert("error" + data);
            console.log(data);
        });

    $scope.resetFilter = function () {
        $scope.cat = '';
        $scope.bus = '';
        $scope.sortBusinessType = 'category';
        $scope.q = '';
        $scope.sortType = ''; // set the default sort type
        $scope.sortReverse = false;  // set the default sort order
        $scope.searchAddress = '';     // set the default search/filter term

        $scope.currentPage = 1;
        $scope.pageSize = 10;
    };

    $scope.pageChangeHandler = function (num) {
        console.log('houses page changed to ' + num);
    };
}

// Local businesses 
function BusinessController($scope, $http) {

    $scope.sortBusinessType = 'category'; // set the default sort type
    $scope.sortBusinessReverse = false;  // set the default sort order
    $scope.searchBusinessAddress = '';     // set the default search/filter term
    $scope.searchBusinessName = '';     // set the default search/filter term

    $scope.currentPage = 1;
    $scope.pageSize = 10;

  
    $scope.businesses = [];

    $scope.init = function (id) {
        $scope.townId = id;   
           $http({
               method: 'GET',
               url: '/Analysis/GetBusinesses?id=' + $scope.townId,
           }).then(function (response) {
               //alert("success businesses: ");
               $scope.businesses = response.data;
           }),
        (function (data) {
            alert("error" + data);
            console.log(data);
        });
    }

    $scope.resetFilter = function () {
        $scope.cat = '';
        $scope.bus = '';
        $scope.sortBusinessType = 'category';
        $scope.q = '';
        $scope.sortType = ''; // set the default sort type
        $scope.sortReverse = true;  // set the default sort order
        $scope.searchAddress = '';     // set the default search/filter term

        $scope.currentPage = 1;
        $scope.pageSize = 10;
    };

    $scope.pageChangeHandler = function (num) {
        console.log('Businesses page changed to ' + num);
    };
}
//  ----------   *********   --------------

// page filtering
function OtherController($scope) {
    $scope.pageChangeHandler = function (num) {
        console.log('going to page ' + num);
    };
}

function resetFilter() {
    $scope.cat = '';
    $scope.bus = '';
    $scope.sortBusinessType = 'category';

    $scope.sortType = ''; // set the default sort type
    $scope.sortReverse = true;  // set the default sort order
    $scope.searchAddress = '';     // set the default search/filter term

    $scope.currentPage = 1;
    $scope.pageSize = 10;
};

myApp.controller('MyController', MyController);
myApp.controller('BusinessController', BusinessController);
myApp.controller('OtherController', OtherController);