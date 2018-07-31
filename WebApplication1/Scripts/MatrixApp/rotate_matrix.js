; (function () {
    var moduleName = 'matrixApp';
    var module;
    try { module = angular.module(moduleName); }
    catch (err) { module = angular.module(moduleName, []); }
    module
        .controller('rotateController', ['$scope', '$http', rotateCtrl]);

    function rotateCtrl($scope, $http) {
        $scope.matrix = {};
        $scope.m_range = 5;
        $scope.n_range = 5;

        $scope.generateRandomMatrix = function () {
            $http.get('/Home/GenerateRandomMatrix', { params: { col: $scope.m_range, row: $scope.n_range } })
                .then(function (response) { $scope.matrix = response.data; });
        };

        $scope.rotate = function (matrix) { $scope.matrix = rotate(matrix); }

        $scope.exportToCSV = function () {
            var url = '../Home/ExportToCSV';

            $.ajax({
                url: url,
                type: 'post',
                data: { 'matrix': $scope.matrix },
                success: function (name) { window.location = "../Home/DownloadFile?fileName=" + name; }
            });
        }

        $scope.submitForm = function () {

            var filename = document.getElementById("bulkDirectFile");

            if (filename.value.length < 1) { $scope.warning = true; }
            else {
                $scope.warning = false;
                var file = filename.files[0];
                if (file) {
                    var csv = [];
                    var matrixArr = [];
                    var reader = new FileReader();

                    reader.onload = (function (f) {
                        csv = (reader.result).split("\n");
                        csv.pop();

                        for (var i = 0; i < csv.length; i++) {

                            var csvRow = csv[i].split(/,|;/).map(function (item) { return parseInt(item, 10); });
                            if (isNaN(csvRow[csvRow.length - 1])) { csvRow.pop(); }

                            matrixArr.push(csvRow);
                        }
                        $scope.$apply(function () { $scope.matrix = matrixArr; });
                    });

                    reader.readAsText(file);
                }
            }
        }
    }

    function rotate(matrix) {
        matrix = transpose(matrix);
        matrix.map(function (array) { array.reverse(); });
        return matrix;
    }

    function createEmptyMatrix(matrixLength) {
        var result = new Array();
        for (var i = 0; i < matrixLength; i++) { result.push([]); }
        return result;
    }

    function transpose(matrix) {
        var matrixLength = 0;
        for (var i = 0; i < matrix.length; i++) {
            matrixLength = matrix.length < matrix[i].length ? matrix[i].length : matrix.length;
        }
        var result = createEmptyMatrix(matrixLength);

        for (var i = 0; i < matrix.length; i++) {
            for (var j = 0; j < matrix[i].length; j++) {
                var temp = matrix[i][j];
                result[j][i] = temp;
            }
        }
        return result;
    }
})();