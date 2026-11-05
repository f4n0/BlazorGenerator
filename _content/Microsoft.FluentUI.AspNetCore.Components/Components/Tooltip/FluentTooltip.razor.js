export var Microsoft;
(function (Microsoft) {
    var FluentUI;
    (function (FluentUI) {
        var Blazor;
        (function (Blazor) {
            var Tooltip;
            (function (Tooltip) {
                function FluentTooltipInitialize(id) {
                    const element = document.getElementById(id);
                    if (element && element.anchorElement && typeof element.connectedCallback === "function") {
                        element.connectedCallback();
                    }
                    if (element) {
                        element.originalPositionning = element.getAttribute("positioning");
                        element.addEventListener('toggle', (e) => {
                            if (e.newState != "open") {
                                element.setAttribute("positioning", element.originalPositionning);
                                return;
                            }
                            if (!isElementInViewport(element)) {
                                const positionning = element.getAttribute("positioning");
                                if (positionMap[positionning]) {
                                    element.setAttribute("positioning", positionMap[positionning]);
                                }
                            }
                        });
                    }
                }
                Tooltip.FluentTooltipInitialize = FluentTooltipInitialize;
                const positionMap = {
                    "above-start": "below-end",
                    "above": "below",
                    "above-end": "below-start",
                    "below-start": "above-end",
                    "below": "above",
                    "below-end": "above-start",
                    "before-top": "after-bottom",
                    "before": "after",
                    "before-bottom": "after-top",
                    "after-top": "before-bottom",
                    "after": "before",
                    "after-bottom": "before-top",
                };
                function isElementInViewport(element) {
                    const rect = element.getBoundingClientRect();
                    return (rect.top >= 0 &&
                        rect.left >= 0 &&
                        rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
                        rect.right <= (window.innerWidth || document.documentElement.clientWidth));
                }
            })(Tooltip = Blazor.Tooltip || (Blazor.Tooltip = {}));
        })(Blazor = FluentUI.Blazor || (FluentUI.Blazor = {}));
    })(FluentUI = Microsoft.FluentUI || (Microsoft.FluentUI = {}));
})(Microsoft || (Microsoft = {}));
//# sourceMappingURL=FluentTooltip.razor.js.map