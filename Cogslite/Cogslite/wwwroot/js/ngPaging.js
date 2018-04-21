(function () {
    angular
        .module('cogs.paging', [])
        .directive('paging', ['$timeout',
            function ($timeout) {
                return {
                    restrict: 'E',
                    require: 'ngModel',
                    replace: true,
                    priority: 1,
                    scope: {
                        pages: '=',                        
                    },
                    template:
                    '<div class="paging">' +                        
                    '</div>',
                    link: function (scope, element, attr, ngModel) {

                        $timeout(function () {

                            // Expand choices object to key value pair array
                            scope.choices = $.map(scope.choices, function (value, name) {

                                var choice = {
                                    name: name,
                                    value: value,
                                    active: value === ngModel.$modelValue
                                }

                                return choice;
                            });
                        });

                        scope.onChoose = function (choice) {

                            for (var i = 0; i < scope.choices.length; ++i)
                                scope.choices[i].active = false;

                            choice.active = true;

                            ngModel.$setViewValue(choice.value);
                        };

                        ngModel.$render = function () {

                            for (var i = 0; i < scope.choices.length; ++i)
                                if (scope.choices[i].value === ngModel.$modelValue)
                                    scope.onChoose(scope.choices[i]);
                        }
                    }
                }
            }]);
})();
