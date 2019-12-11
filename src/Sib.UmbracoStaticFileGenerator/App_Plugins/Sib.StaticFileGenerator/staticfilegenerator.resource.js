function staticFileGeneratorResource($q, $http, umbRequestHelper) {

    return {
        getDashboard: function () {
            return umbRequestHelper.resourcePromise(
                $http.get(umbRequestHelper.getApiUrl("StaticFileGenerator", "GetDashboard")),
                "Failed to get dashboard");
        }
    };
};
angular.module("umbraco.resources").factory("staticFileGeneratorResource", staticFileGeneratorResource);
