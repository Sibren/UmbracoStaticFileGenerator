function staticFileGeneratorResource($q, $http, umbRequestHelper) {

    return {
        getDashboard: function () {
            return umbRequestHelper.resourcePromise(
                $http.get(umbRequestHelper.getApiUrl("StaticFileGenerator", "GetDashboard")),
                "Failed to get dashboard");
        },
        saveData: function (args) {
            umbRequestHelper.resourcePromise(
                $http.post(umbRequestHelper.getApiUrl("StaticFileGenerator", "SaveConfig"), args),
                'Failed to request translation for content')
                .then(function (data) {
                    $scope.error = false;
                    $scope.success = true;
                    $scope.busy = false;
                    $scope.translationRequests = data;

                }, function (err) {
                    $scope.success = false;
                    $scope.error = err;
                    $scope.busy = false;
                });
        }
    }
};
angular.module("umbraco.resources").factory("staticFileGeneratorResource", staticFileGeneratorResource);
