function modelsBuilderController($scope, $http, umbRequestHelper, staticFileGeneratorResource) {

    var vm = this;

    vm.generate = generate;
    vm.dashboard = null;
    vm.saved = false;
    function generate() {
        vm.generating = true;
        umbRequestHelper.resourcePromise(
            $http.post(umbRequestHelper.getApiUrl("StaticFileGenerator", "SaveConfig"), vm.dashboard.config),
            'Failed to generate.')
            .then(function (result) {
                vm.generating = false;
                vm.dashboard = result;
                vm.loading = false;
                vm.saved = true;
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