var moduleName = 'xstLoyaltyModule';

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .run(['platformWebApp.bladeNavigationService', 'platformWebApp.toolbarService', 'platformWebApp.widgetService',
        function (bladeNavigationService, toolbarService, widgetService) {
        }
    ]);
