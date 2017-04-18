var myApp = angular.module('myApp', ["angularUtils.directives.dirPagination", "angular.filter"]);



function MyController($scope, $http) {

    $scope.sortType = ''; // set the default sort type
    $scope.sortReverse = true;  // set the default sort order
    $scope.searchAddress = '';     // set the default search/filter term

    $scope.currentPage = 1;
    $scope.pageSize = 10;
  //  $scope.houses = [
  //{
  //    "DateOfSale": "/Date(1268092800000)/",
  //    "Address": "Apartment 1, 55 Mountjoy square",
  //    "Price": 170000
  //},
  //{
  //    "DateOfSale": "/Date(1269561600000)/",
  //    "Address": "Apt 35 Stapleton House, 33 Mountjoy Square, Dublin 1",
  //    "Price": 125000
  //},
  //{
  //    "DateOfSale": "/Date(1276642800000)/",
  //    "Address": "Apt 40, Number 47-52 Mountjoy Square, Dublin 1",
  //    "Price": 180000
  //},
  //{
  //    "DateOfSale": "/Date(1277852400000)/",
  //    "Address": "Apartment 2, 64  Mountjoy Square",
  //    "Price": 112000
  //},
  //{
  //    "DateOfSale": "/Date(1279839600000)/",
  //    "Address": "APARTMENT 53, 52 MOUNTJOY SQUARE",
  //    "Price": 179000
  //},
  //{
  //    "DateOfSale": "/Date(1289347200000)/",
  //    "Address": "Apt 51 No. 55, Mountjoy Square, Dublin 1.",
  //    "Price": 110000
  //},
  //{
  //    "DateOfSale": "/Date(1309906800000)/",
  //    "Address": "15 Wellington Court, Mountjoy Street, Dublin 7",
  //    "Price": 132855
  //},
  //{
  //    "DateOfSale": "/Date(1313449200000)/",
  //    "Address": "apartment 39, 55 mountjoy square, dublin 1",
  //    "Price": 100000
  //},
  //{
  //    "DateOfSale": "/Date(1323993600000)/",
  //    "Address": "52 Mountjoy Square, Dublin 1",
  //    "Price": 75000
  //},
  //{
  //    "DateOfSale": "/Date(1330387200000)/",
  //    "Address": "Apartment 14, 52 Mountjoy Square",
  //    "Price": 90000
  //},
  //{
  //    "DateOfSale": "/Date(1336086000000)/",
  //    "Address": "Apt. 16, 55 Mountjoy Square",
  //    "Price": 135000
  //},
  //{
  //    "DateOfSale": "/Date(1336690800000)/",
  //    "Address": "Apt. 39  Stapleton House, 33 Mountjoy Sq., Dublin 1",
  //    "Price": 69000
  //},
  //{
  //    "DateOfSale": "/Date(1340924400000)/",
  //    "Address": "Apt.10 Mountjoy Square, Dublin 1",
  //    "Price": 91000
  //},
  //{
  //    "DateOfSale": "/Date(1342134000000)/",
  //    "Address": "Apt. 108, 35 Mountjoy Square, Dublin 1",
  //    "Price": 100000
  //},
  //{
  //    "DateOfSale": "/Date(1343862000000)/",
  //    "Address": "Apartment 3, 33 Mountjoy Square",
  //    "Price": 190000
  //},
  //{
  //    "DateOfSale": "/Date(1349132400000)/",
  //    "Address": "33 Stapleton House, Mountjoy Square",
  //    "Price": 70000
  //},
  //{
  //    "DateOfSale": "/Date(1349391600000)/",
  //    "Address": "36 Mountjoy Street, Dublin 7",
  //    "Price": 125000
  //},
  //{
  //    "DateOfSale": "/Date(1349737200000)/",
  //    "Address": "Apartment No. 50, Block A 35 Mountjoy Square, Dublin City",
  //    "Price": 100000
  //},
  //{
  //    "DateOfSale": "/Date(1354233600000)/",
  //    "Address": "apartment 26, 35 mountjoy square, dublin1",
  //    "Price": 90000
  //},
  //{
  //    "DateOfSale": "/Date(1358985600000)/",
  //    "Address": "13 Middle Mountjoy Street, Dublin 1",
  //    "Price": 300000
  //},
  //{
  //    "DateOfSale": "/Date(1363046400000)/",
  //    "Address": "Apt 17  64 Mountjoy Square, Mountjoy Square, Dublin 1",
  //    "Price": 134000
  //},
  //{
  //    "DateOfSale": "/Date(1365462000000)/",
  //    "Address": "37 No 52 Mountjoy Square, Dublin 1",
  //    "Price": 99500
  //},
  //{
  //    "DateOfSale": "/Date(1367535600000)/",
  //    "Address": "No. 35  52 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 82500
  //},
  //{
  //    "DateOfSale": "/Date(1371164400000)/",
  //    "Address": "Apartment 49, 35 Mountjoy Square, Dublin 1",
  //    "Price": 84000
  //},
  //{
  //    "DateOfSale": "/Date(1373324400000)/",
  //    "Address": "Apt 19, 64 Mountjoy Sq.",
  //    "Price": 95000
  //},
  //{
  //    "DateOfSale": "/Date(1374188400000)/",
  //    "Address": "19 Mountjoy Street, Dublin 7",
  //    "Price": 155000
  //},
  //{
  //    "DateOfSale": "/Date(1377471600000)/",
  //    "Address": "Apt. 18, 52 Mountjoy Square",
  //    "Price": 120000
  //},
  //{
  //    "DateOfSale": "/Date(1377730800000)/",
  //    "Address": "67A  Mountjoy Square rear to 67, otherwise 27 Grenville Lane, Dublin 1",
  //    "Price": 35000
  //},
  //{
  //    "DateOfSale": "/Date(1379631600000)/",
  //    "Address": "Apartment 6, Wellington Court, Mountjoy Street",
  //    "Price": 226617
  //},
  //{
  //    "DateOfSale": "/Date(1384128000000)/",
  //    "Address": "APT 6, 52 MOUNTJOY SQ, DUBLIN 1",
  //    "Price": 83000
  //},
  //{
  //    "DateOfSale": "/Date(1384905600000)/",
  //    "Address": "55  No. 52 Mountjoy Square, Dublin 1",
  //    "Price": 123000
  //},
  //{
  //    "DateOfSale": "/Date(1384905600000)/",
  //    "Address": "Apartment 44, 33 Mountjoy Sqaure, Dublin 1",
  //    "Price": 82000
  //},
  //{
  //    "DateOfSale": "/Date(1386288000000)/",
  //    "Address": "APARTMENT 29, 52 MOUNTJOY SQ, DUBLIN 1",
  //    "Price": 78000
  //},
  //{
  //    "DateOfSale": "/Date(1387411200000)/",
  //    "Address": "APT 95, 33 MOUNTJOY SQ, DUBLIN 1",
  //    "Price": 107000
  //},
  //{
  //    "DateOfSale": "/Date(1387756800000)/",
  //    "Address": "16 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 200000
  //},
  //{
  //    "DateOfSale": "/Date(1388966400000)/",
  //    "Address": "15 MOUNTJOY ST MIDDLE, BROADSTONE, DUBLIN 7",
  //    "Price": 130000
  //},
  //{
  //    "DateOfSale": "/Date(1389830400000)/",
  //    "Address": "APARTMENT 4, 6 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 95000
  //},
  //{
  //    "DateOfSale": "/Date(1389916800000)/",
  //    "Address": "APT 73 RUSSEL HOUSE, 33 MOUNTJOY SQ, DUBLIN 1",
  //    "Price": 160000
  //},
  //{
  //    "DateOfSale": "/Date(1392854400000)/",
  //    "Address": "APARTMENT 107, 35 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 138000
  //},
  //{
  //    "DateOfSale": "/Date(1393286400000)/",
  //    "Address": "39 STAPLETON HOUSE, 33 MOUNTJOY SQUARE, DUBLIN",
  //    "Price": 110000
  //},
  //{
  //    "DateOfSale": "/Date(1393286400000)/",
  //    "Address": "APARTMENT 30, 52 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 130000
  //},
  //{
  //    "DateOfSale": "/Date(1394582400000)/",
  //    "Address": "24 STAPLETON HOUSE, 33 MOUNTJOY SQ, DUBLIN 1",
  //    "Price": 88000
  //},
  //{
  //    "DateOfSale": "/Date(1394755200000)/",
  //    "Address": "2 MOUNTJOY PARADE, DUBLIN 1, DUBLIN",
  //    "Price": 86000
  //},
  //{
  //    "DateOfSale": "/Date(1395964800000)/",
  //    "Address": "1 MOUNTJOY PARADE, DUBLIN 1, DUBLIN",
  //    "Price": 87000
  //},
  //{
  //    "DateOfSale": "/Date(1397689200000)/",
  //    "Address": "APT 55, WELLINGTON COURT, MOUNTJOY STREET",
  //    "Price": 257500
  //},
  //{
  //    "DateOfSale": "/Date(1401058800000)/",
  //    "Address": "BASEMENT APARTMENT, 5 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 100000
  //},
  //{
  //    "DateOfSale": "/Date(1402354800000)/",
  //    "Address": "13 Mountjoy Square, Dublin 1",
  //    "Price": 650000
  //},
  //{
  //    "DateOfSale": "/Date(1402354800000)/",
  //    "Address": "APT 36, 55 MOUNTJOY SQ, DUBLIN 1",
  //    "Price": 90000
  //},
  //{
  //    "DateOfSale": "/Date(1402614000000)/",
  //    "Address": "APARTMENT 27, 35 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 145000
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "35 Wellington Court, Mountjoy Street",
  //    "Price": 142700
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "Apt 13 Wellington Court, Mountjoy Street",
  //    "Price": 107000
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "Apt 16 Wellington Court, Mountjoy Street",
  //    "Price": 133800
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "Apt 23 Wellington Court, Mountjoy Street",
  //    "Price": 147200
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "Apt 25 Wellington Court, Mountjoy Street",
  //    "Price": 107000
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "Apt 28 Wellington Court, Mountjoy Street",
  //    "Price": 147200
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "Apt 30 Wellington Court, Mountjoy Street",
  //    "Price": 142700
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "Apt 31 Wellington Court, Mountjoy Street",
  //    "Price": 133800
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "Apt 34 Wellington Court, Mountjoy Street",
  //    "Price": 147200
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "Apt 36 Wellington Court, Mountjoy Street",
  //    "Price": 151700
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "Apt 37 Wellington Court, Mountjoy Street",
  //    "Price": 116000
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "Apt 38 Wellington Court, Mountjoy Street",
  //    "Price": 116000
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "Apt 39 Wellington Court, Mountjoy Street",
  //    "Price": 107000
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "Apt 43 Wellington Court, Mountjoy Street",
  //    "Price": 107000
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "Apt 45 Wellington Court, Mountjoy Street",
  //    "Price": 151700
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "Apt 46 Wellington Court, Mountjoy Street",
  //    "Price": 147200
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "Apt 47 Wellington Court, Mountjoy Street",
  //    "Price": 107000
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "Apt 48 Wellington Court, Mountjoy Street",
  //    "Price": 142700
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "Apt 49 Wellington Court, Mountjoy Street",
  //    "Price": 151700
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "Apt 51 Wellington Court, Mountjoy Street",
  //    "Price": 169500
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "Apt 52 Wellington Court, Mountjoy Street",
  //    "Price": 160600
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "Apt 53 Wellington Court, Mountjoy Street",
  //    "Price": 151700
  //},
  //{
  //    "DateOfSale": "/Date(1405378800000)/",
  //    "Address": "Apt 54 Wellington Court, Mountjoy Street",
  //    "Price": 165000
  //},
  //{
  //    "DateOfSale": "/Date(1405638000000)/",
  //    "Address": "APARTMENT 3, 52 MOUNTJOY SQUARE, DUBLIN",
  //    "Price": 140000
  //},
  //{
  //    "DateOfSale": "/Date(1406156400000)/",
  //    "Address": "46 MOUNTJOY ST, DUBLIN 7, DUBLIN",
  //    "Price": 257000
  //},
  //{
  //    "DateOfSale": "/Date(1406588400000)/",
  //    "Address": "APARTMENT 3, 6 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 100000
  //},
  //{
  //    "DateOfSale": "/Date(1409007600000)/",
  //    "Address": "APT 81 RUSSELL HOUSE, 33 MOUNTJOY SQ, DUBLIN 1",
  //    "Price": 175000
  //},
  //{
  //    "DateOfSale": "/Date(1410217200000)/",
  //    "Address": "2 MOUNTJOY PARADE, DUBLIN 1, DUBLIN",
  //    "Price": 158000
  //},
  //{
  //    "DateOfSale": "/Date(1410390000000)/",
  //    "Address": "APT 33, 55 MOUNTJOY SQ, DUBLIN",
  //    "Price": 60000
  //},
  //{
  //    "DateOfSale": "/Date(1410735600000)/",
  //    "Address": "Apartment 7, 6 Mountjoy Square, Dublin 1",
  //    "Price": 97000
  //},
  //{
  //    "DateOfSale": "/Date(1411081200000)/",
  //    "Address": "Ground Floor Apt 7, 7 Mountjoy Square, Dublin 1",
  //    "Price": 144000
  //},
  //{
  //    "DateOfSale": "/Date(1411599600000)/",
  //    "Address": "Apartment 8, 6 Mountjoy Square, Dublin 1",
  //    "Price": 101000
  //},
  //{
  //    "DateOfSale": "/Date(1412031600000)/",
  //    "Address": "1 MOUNTJOY PARADE, DUBLIN 1, DUBLIN",
  //    "Price": 170000
  //},
  //{
  //    "DateOfSale": "/Date(1415577600000)/",
  //    "Address": "52 MOUNTJOY STREET, DUBLIN 1",
  //    "Price": 130000
  //},
  //{
  //    "DateOfSale": "/Date(1416528000000)/",
  //    "Address": "Apartment 5, 6 Mountjoy Square, Dublin 1",
  //    "Price": 141000
  //},
  //{
  //    "DateOfSale": "/Date(1418688000000)/",
  //    "Address": "APT 14, 63 64 MOUNTJOY SQ, DUBLIN 1",
  //    "Price": 130000
  //},
  //{
  //    "DateOfSale": "/Date(1418860800000)/",
  //    "Address": "GROUND FLOOR APARTMENT, 5 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 146000
  //},
  //{
  //    "DateOfSale": "/Date(1418947200000)/",
  //    "Address": "Apt. 2, 64 Mountjoy Square",
  //    "Price": 155000
  //},
  //{
  //    "DateOfSale": "/Date(1421625600000)/",
  //    "Address": "14 Russell House, Mountjoy Square",
  //    "Price": 135000
  //},
  //{
  //    "DateOfSale": "/Date(1421884800000)/",
  //    "Address": "Apartment 1, 6 Mountjoy Square, Dublin 1",
  //    "Price": 140000
  //},
  //{
  //    "DateOfSale": "/Date(1422576000000)/",
  //    "Address": "APT 6, 6 MOUNTJOY SQ, DUBLIN 1",
  //    "Price": 117000
  //},
  //{
  //    "DateOfSale": "/Date(1426032000000)/",
  //    "Address": "APARTMENT 2, 6 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 225000
  //},
  //{
  //    "DateOfSale": "/Date(1428620400000)/",
  //    "Address": "APARATMENT 4, 4 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 140000
  //},
  //{
  //    "DateOfSale": "/Date(1428620400000)/",
  //    "Address": "APARTMENT 1, 4 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 160000
  //},
  //{
  //    "DateOfSale": "/Date(1428620400000)/",
  //    "Address": "APARTMENT 2, 4 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 120000
  //},
  //{
  //    "DateOfSale": "/Date(1428620400000)/",
  //    "Address": "APARTMENT 3, 4 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 140000
  //},
  //{
  //    "DateOfSale": "/Date(1428620400000)/",
  //    "Address": "APARTMENT 5, 4 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 180000
  //},
  //{
  //    "DateOfSale": "/Date(1430434800000)/",
  //    "Address": "48 MOUNTJOY SQ, DUBLIN 1, DUBLIN",
  //    "Price": 225000
  //},
  //{
  //    "DateOfSale": "/Date(1432767600000)/",
  //    "Address": "Apartment 14, 6 Mountjoy Square, Dublin 1",
  //    "Price": 106000
  //},
  //{
  //    "DateOfSale": "/Date(1434063600000)/",
  //    "Address": "17 Mountjoy Square, Dublin 1",
  //    "Price": 452500
  //},
  //{
  //    "DateOfSale": "/Date(1436742000000)/",
  //    "Address": "10 MOUNTJOY PARADE, DUBLIN 1, DUBLIN",
  //    "Price": 168000
  //},
  //{
  //    "DateOfSale": "/Date(1437087600000)/",
  //    "Address": "Apartment 15, 33 Mountjoy Square",
  //    "Price": 110000
  //},
  //{
  //    "DateOfSale": "/Date(1437087600000)/",
  //    "Address": "APT 34, 33 MOUNTJOY SQ, DUBLIN 1",
  //    "Price": 128300
  //},
  //{
  //    "DateOfSale": "/Date(1438210800000)/",
  //    "Address": "14 WELLINGTON COURT, MOUNTJOY ST, DUBLIN 7",
  //    "Price": 137000
  //},
  //{
  //    "DateOfSale": "/Date(1438210800000)/",
  //    "Address": "3 Middle Mountjoy Street, Dublin 7",
  //    "Price": 215000
  //},
  //{
  //    "DateOfSale": "/Date(1440630000000)/",
  //    "Address": "APARTMENT 20, 35 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 135000
  //},
  //{
  //    "DateOfSale": "/Date(1441666800000)/",
  //    "Address": "APARTMENT 20, 64 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 105000
  //},
  //{
  //    "DateOfSale": "/Date(1443135600000)/",
  //    "Address": "Apt 2, 55 Mountjoy Square, Dublin 1",
  //    "Price": 160000
  //},
  //{
  //    "DateOfSale": "/Date(1445382000000)/",
  //    "Address": "APT 13, 52 MOUNTJOY SQ, DUBLIN 1",
  //    "Price": 110000
  //},
  //{
  //    "DateOfSale": "/Date(1446163200000)/",
  //    "Address": "APT 59, 55 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 276600
  //},
  //{
  //    "DateOfSale": "/Date(1448236800000)/",
  //    "Address": "APT 1, 48 MOUNTJOY STREET, DUBLIN 7",
  //    "Price": 68750
  //},
  //{
  //    "DateOfSale": "/Date(1448236800000)/",
  //    "Address": "APT 2, 48 MOUNTJOY STREET, DUBLIN 7",
  //    "Price": 68750
  //},
  //{
  //    "DateOfSale": "/Date(1448236800000)/",
  //    "Address": "APT 3, 48 MOUNTJOY STREET, DUBLIN 7",
  //    "Price": 68750
  //},
  //{
  //    "DateOfSale": "/Date(1448236800000)/",
  //    "Address": "APT 4, 48 MOUNTJOY STREET, DUBLIN 7",
  //    "Price": 68750
  //},
  //{
  //    "DateOfSale": "/Date(1450396800000)/",
  //    "Address": "Apartment 56, 55 Mountjoy Square",
  //    "Price": 240668
  //},
  //{
  //    "DateOfSale": "/Date(1450396800000)/",
  //    "Address": "APT1, 61 MOUNTJOY SQ, DUBLIN 1",
  //    "Price": 1800000
  //},
  //{
  //    "DateOfSale": "/Date(1450656000000)/",
  //    "Address": "75 RUSSELL HOUSE, MOUNTJOY SQ APTS, CHARLES ST GREAT DUBLIN 1",
  //    "Price": 222000
  //},
  //{
  //    "DateOfSale": "/Date(1451520000000)/",
  //    "Address": "APT 7 60 MOUNTJOY SQ, DUBLIN 2, DUBLIN",
  //    "Price": 375000
  //},
  //{
  //    "DateOfSale": "/Date(1454284800000)/",
  //    "Address": "APT 18, 33 MOUNTJOY SQ, DUBLIN 1",
  //    "Price": 162000
  //},
  //{
  //    "DateOfSale": "/Date(1454457600000)/",
  //    "Address": "20 STAPLETON HOUSE, 33 MOUNTJOY SQ EAST, DUBLIN 1",
  //    "Price": 164000
  //},
  //{
  //    "DateOfSale": "/Date(1455148800000)/",
  //    "Address": "APARTMENT 3, 35 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 156000
  //},
  //{
  //    "DateOfSale": "/Date(1456876800000)/",
  //    "Address": "Apartment 51, 52 Mountjoy Square, Dublin",
  //    "Price": 182000
  //},
  //{
  //    "DateOfSale": "/Date(1457654400000)/",
  //    "Address": "APT 14, 52 MOUNTJOY SQ, DUBLIN",
  //    "Price": 195000
  //},
  //{
  //    "DateOfSale": "/Date(1459983600000)/",
  //    "Address": "APT 31, 55 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 148000
  //},
  //{
  //    "DateOfSale": "/Date(1463439600000)/",
  //    "Address": "49 MOUNTJOY SQ, DUBLIN 1, DUBLIN",
  //    "Price": 277000
  //},
  //{
  //    "DateOfSale": "/Date(1466982000000)/",
  //    "Address": "7 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 200000
  //},
  //{
  //    "DateOfSale": "/Date(1467068400000)/",
  //    "Address": "APT 54, MOUNTJOY SQ SOUTH, DUBLIN 1",
  //    "Price": 195000
  //},
  //{
  //    "DateOfSale": "/Date(1468278000000)/",
  //    "Address": "10 ST MARYS PLACE, OFF MOUNTJOY ST, DUBLIN 7",
  //    "Price": 167000
  //},
  //{
  //    "DateOfSale": "/Date(1477004400000)/",
  //    "Address": "APT 91 PEMBERTON, 33 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 130000
  //},
  //{
  //    "DateOfSale": "/Date(1480636800000)/",
  //    "Address": "APT 46, 55 MOUNTJOY SQ WEST, DUBLIN",
  //    "Price": 130000
  //},
  //{
  //    "DateOfSale": "/Date(1481587200000)/",
  //    "Address": "APT 27, STAPLETON HOUSE, 33 MOUNTJOY SQ",
  //    "Price": 163000
  //},
  //{
  //    "DateOfSale": "/Date(1482278400000)/",
  //    "Address": "APARTMENT 55, NO 35 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 202800
  //},
  //{
  //    "DateOfSale": "/Date(1484179200000)/",
  //    "Address": "APT 47, 52 MOUNTJOY SQUARE, DUBLIN 1",
  //    "Price": 275000
  //},
  //{
  //    "DateOfSale": "/Date(1485388800000)/",
  //    "Address": "APT 30, 55 MOUNTJOY SQ WEST, DUBLIN 1",
  //    "Price": 160000
  //},
  //{
  //    "DateOfSale": "/Date(1488240000000)/",
  //    "Address": "APT 88 PEMBERTON HOUSE, 33 MOUNTJOY SQ, DUBLIN 1",
  //    "Price": 170350
  //}
    //  ];
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

    //.then(function (result) {
    //    $scope.user = result;
    //    console.log(result);
    //}, function(result) {
    //    //some error
    //    console.log(result);
    //});

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

    //$scope.businesses = [
	//							{
	//							    category: "Take away",
	//							    address: "10 main street, saggart",
	//							    name: "Marsella's"
	//							},
	//							{
	//							    category: "Take away",
	//							    address: "88 main street, newcastle",
	//							    name: ""
	//							},
	//							{
	//							    category: "Take away",
	//							    address: "Barrow road, newcastle",
	//							    name: "Marsella's"
	//							},
	//							{
	//							    category: "Take away",
	//							    address: "High street, rathcoole",
	//							    name: "Luigis"
	//							},
	//							{
	//							    category: "Shop",
	//							    address: "50 main street, saggart",
	//							    name: "Conways"
	//							},
	//							{
	//							    category: "Shop",
	//							    address: "88 main street, newcastle",
	//							    name: "Spar"
	//							},
	//							{
	//							    category: "Shop",
	//							    address: "Barrow road, newcastle",
	//							    name: "Marsella's"
	//							},
	//							{
	//							    category: "Shop",
	//							    address: "High street, rathcoole",
	//							    name: "Centra"
	//							},
	//							{
	//							    category: "Pub",
	//							    address: "rathcoole",
	//							    name: "Cassidys"
	//							},
	//							{
	//							    category: "Pub",
	//							    address: "Saggart",
	//							    name: "Cassidys"
	//							},
	//							{
	//							    category: "Pub",
	//							    address: "Celbridge",
	//							    name: "Cassidys"
	//							},
	//							{
	//							    category: "Hairdresser",
	//							    address: "rathcoole",
	//							    name: "Bobs"
	//							},
	//							{
	//							    category: "Hairdresser",
	//							    address: "Saggart",
	//							    name: "Cassidys"
	//							},
	//							{
	//							    category: "Hairdresser",
	//							    address: "Celbridge",
	//							    name: "Unas"
	//							}
    //];
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

    //$http({
    //    method: 'GET',
    //    url: '/Analysis/GetBusinesses?id=' + $scope.townId,
    //}).then(function (data) {
    //    alert("success businesses: " + data[0]);
    //    $scope.businesses = data;
    //}).
    // error(function (data) {
    //     alert("error" + data);
    //     console.log(data);
    // });


    $scope.resetFilter = function () {
        $scope.cat = '';
        $scope.bus = '';
        $scope.sortBusinessType = 'category';

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