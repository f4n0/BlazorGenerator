export var Microsoft;
(function (Microsoft) {
    var FluentUI;
    (function (FluentUI) {
        var Blazor;
        (function (Blazor) {
            var Calendar;
            (function (Calendar) {
                function SetAccessibilityKeyboard(calendar, defaultFocusSelector = null) {
                    if (!calendar) {
                        return;
                    }
                    AddKeyAcceptListener(calendar, `.title div`, `div[part='months'] div[tabindex='0']`);
                    AddKeyAcceptListener(calendar, `.previous`, `.previous`);
                    AddKeyAcceptListener(calendar, `.next`, `.next`);
                    AddKeyAcceptListener(calendar, `.day`);
                    AddKeyAcceptListener(calendar, `.month`);
                    AddKeyAcceptListener(calendar, `.year`);
                    AddNavigateListener(calendar, `.day`);
                    AddNavigateListener(calendar, `.month`);
                    AddNavigateListener(calendar, `.year`);
                    if (defaultFocusSelector) {
                        SetFocus(calendar, defaultFocusSelector);
                    }
                }
                Calendar.SetAccessibilityKeyboard = SetAccessibilityKeyboard;
                function SetFirstFocusable(calendar) {
                    if (typeof calendar === "string") {
                        calendar = document.querySelector(calendar);
                    }
                    if (calendar) {
                        const firstSelected = calendar.querySelector("[tabindex='0'][selected]");
                        if (firstSelected) {
                            SetFocus(calendar, firstSelected);
                            return;
                        }
                        const today = calendar.querySelector("[tabindex='0'][today]");
                        if (today) {
                            SetFocus(calendar, today);
                            return;
                        }
                        const firstItem = calendar.querySelector("*[tabindex='0'].day, *[tabindex='0'].month, *[tabindex='0'].year");
                        if (firstItem) {
                            SetFocus(calendar, firstItem);
                            return;
                        }
                        SetFocus(calendar, "[tabindex='0']");
                    }
                }
                Calendar.SetFirstFocusable = SetFirstFocusable;
                function AddNavigateListener(calendar, itemSelector) {
                    if (calendar) {
                        const items = calendar.querySelectorAll(itemSelector);
                        for (const item of items) {
                            if (item && !item.__navigationRegistered) {
                                item.addEventListener("keydown", (event) => {
                                    if (event.code === "ArrowLeft" || event.code === "ArrowRight" ||
                                        event.code === "ArrowUp" || event.code === "ArrowDown") {
                                        event.preventDefault();
                                        event.stopPropagation();
                                        const nextItem = GetNextItem(items, item, event.code);
                                        if (nextItem) {
                                            SetFocus(calendar, nextItem);
                                        }
                                        else {
                                            if (event.code === "ArrowRight" || event.code === "ArrowDown") {
                                                const nextButton = calendar.querySelector(".next");
                                                nextButton?.click();
                                            }
                                            if (event.code === "ArrowLeft" || event.code === "ArrowUp") {
                                                const nextButton = calendar.querySelector(".previous");
                                                nextButton?.click();
                                            }
                                        }
                                    }
                                });
                            }
                            item.__navigationRegistered = "true";
                        }
                    }
                }
                function GetNextItem(items, item, keyCode) {
                    const itemArray = Array.from(items);
                    const currentIndex = itemArray.indexOf(item);
                    const nextLineIncrement = item.classList.contains("day") ? 7 : item.classList.contains("month") ? 4 : 4;
                    switch (keyCode) {
                        case "ArrowRight":
                            for (let i = currentIndex + 1; i < itemArray.length; i++) {
                                if (isEnableItem(itemArray[i])) {
                                    return itemArray[i];
                                }
                            }
                            break;
                        case "ArrowLeft":
                            for (let i = currentIndex - 1; i >= 0; i--) {
                                if (isEnableItem(itemArray[i])) {
                                    return itemArray[i];
                                }
                            }
                            break;
                        case "ArrowDown":
                            for (let i = currentIndex + nextLineIncrement; i < itemArray.length; i += nextLineIncrement) {
                                if (isEnableItem(itemArray[i])) {
                                    return itemArray[i];
                                }
                            }
                            break;
                        case "ArrowUp":
                            for (let i = currentIndex - nextLineIncrement; i >= 0; i -= nextLineIncrement) {
                                if (isEnableItem(itemArray[i])) {
                                    return itemArray[i];
                                }
                            }
                            break;
                    }
                    return null;
                    function isEnableItem(element) {
                        return !element.hasAttribute("disabled") && !element.hasAttribute("inactive");
                    }
                }
                function AddKeyAcceptListener(calendar, itemSelector, focusSelector = null) {
                    if (calendar) {
                        const items = calendar.querySelectorAll(itemSelector);
                        for (const item of items) {
                            if (item && !item.__keyAcceptRegistered) {
                                item.addEventListener("keydown", (event) => {
                                    if (event.code === "Space" || event.code === "Enter") {
                                        event.preventDefault();
                                        event.stopPropagation();
                                        item.click();
                                        SetFocus(calendar, focusSelector ?? item);
                                    }
                                });
                                item.__keyAcceptRegistered = "true";
                            }
                        }
                    }
                }
                function SetFocus(calendar, querySelectorOrItem, timeOut = 500) {
                    const intervalTime = 20;
                    let elapsedTime = 0;
                    const intervalId = setInterval(() => {
                        elapsedTime += intervalTime;
                        const element = (querySelectorOrItem instanceof HTMLElement)
                            ? querySelectorOrItem
                            : calendar.querySelector(querySelectorOrItem);
                        if (element) {
                            element.focus();
                            clearInterval(intervalId);
                            return;
                        }
                        if (elapsedTime >= timeOut) {
                            clearInterval(intervalId);
                        }
                    }, intervalTime);
                }
            })(Calendar = Blazor.Calendar || (Blazor.Calendar = {}));
        })(Blazor = FluentUI.Blazor || (FluentUI.Blazor = {}));
    })(FluentUI = Microsoft.FluentUI || (Microsoft.FluentUI = {}));
})(Microsoft || (Microsoft = {}));
//# sourceMappingURL=FluentCalendar.razor.js.map