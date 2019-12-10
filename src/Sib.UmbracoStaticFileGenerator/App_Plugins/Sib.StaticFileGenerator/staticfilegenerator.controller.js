function modelsBuilderController($scope, $http, umbRequestHelper, staticFileGeneratorResource) {

    var vm = this;

    vm.reload = reload;
    vm.generate = null;
    vm.dashboard = null;

    //function generate() {
    //    vm.generating = true;
    //    umbRequestHelper.resourcePromise(
    //            $http.post(umbRequestHelper.getApiUrl("modelsBuilderBaseUrl", "BuildModels")),
    //            'Failed to generate.')
    //        .then(function (result) {
    //            vm.generating = false;
    //            vm.dashboard = result;
    //        });
    //}

    function reload() {
        vm.loading = true;
        staticFileGeneratorResource.getDashboard().then(function (result) {
            vm.dashboard = result;
            vm.loading = false;
        });
    }

    function init() {
        vm.loading = true;
        staticFileGeneratorResource.getDashboard().then(function (result) {
            vm.dashboard = result;
            vm.loading = false;
        });
    }

    init();
}
angular.module("umbraco").controller("Umbraco.Dashboard.StaticSiteGenerator", modelsBuilderController);