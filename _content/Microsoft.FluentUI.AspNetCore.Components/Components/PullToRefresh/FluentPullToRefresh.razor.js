export var Microsoft;
(function (Microsoft) {
    var FluentUI;
    (function (FluentUI) {
        var Blazor;
        (function (Blazor) {
            var PullToRefresh;
            (function (PullToRefresh) {
                class PolyfilledTouch {
                    constructor(target, identifier, pos, deltaX = 0, deltaY = 0) {
                        this.target = target;
                        this.identifier = identifier;
                        this.radiusX = 0;
                        this.radiusY = 0;
                        this.rotationAngle = 0;
                        this.force = 0;
                        this.altitudeAngle = 0;
                        this.azimuthAngle = 0;
                        this.touchType = "direct";
                        this.clientX = pos.clientX + deltaX;
                        this.clientY = pos.clientY + deltaY;
                        this.screenX = pos.screenX + deltaX;
                        this.screenY = pos.screenY + deltaY;
                        this.pageX = pos.pageX + deltaX;
                        this.pageY = pos.pageY + deltaY;
                    }
                }
                let emulatorInitialized = false;
                function GetScrollDistToTop() {
                    const scrollTop = document.documentElement.scrollTop || document.body.scrollTop || 0;
                    return Math.round(scrollTop);
                }
                PullToRefresh.GetScrollDistToTop = GetScrollDistToTop;
                function GetScrollDistToBottom() {
                    const dist = document.documentElement.scrollHeight -
                        document.documentElement.scrollTop -
                        document.documentElement.clientHeight;
                    return Math.round(dist);
                }
                PullToRefresh.GetScrollDistToBottom = GetScrollDistToBottom;
                function InitTouchEmulator() {
                    loadTouchEmulator();
                }
                PullToRefresh.InitTouchEmulator = InitTouchEmulator;
                function loadTouchEmulator() {
                    if (typeof window === "undefined" || emulatorInitialized) {
                        return;
                    }
                    let eventTarget = null;
                    const supportTouch = "ontouchstart" in window;
                    const docAny = document;
                    if (!docAny.createTouch) {
                        docAny.createTouch = (view, target, identifier, pageX, pageY, screenX, screenY) => new PolyfilledTouch(target, identifier, {
                            pageX,
                            pageY,
                            screenX,
                            screenY,
                            clientX: pageX - window.scrollX,
                            clientY: pageY - window.scrollY,
                        });
                    }
                    if (!docAny.createTouchList) {
                        docAny.createTouchList = (...touches) => createTouchList(touches);
                    }
                    if (!Element.prototype.matches) {
                        Element.prototype.matches = Element.prototype.msMatchesSelector || Element.prototype.matches;
                    }
                    if (!Element.prototype.closest) {
                        Element.prototype.closest = function (selector) {
                            let el = this;
                            while (el && el.nodeType === 1) {
                                if (el.matches(selector)) {
                                    return el;
                                }
                                el = el.parentElement;
                            }
                            return null;
                        };
                    }
                    let initiated = false;
                    const onMouse = (touchType) => (ev) => {
                        if (ev.type === "mousedown") {
                            initiated = true;
                        }
                        if (ev.type === "mouseup") {
                            initiated = false;
                        }
                        if (ev.type === "mousemove" && !initiated) {
                            return;
                        }
                        const candidate = ev.target;
                        if (ev.type === "mousedown" || !eventTarget || (eventTarget && !eventTarget.dispatchEvent)) {
                            eventTarget = candidate;
                        }
                        if (eventTarget && eventTarget.closest && eventTarget.closest("[data-no-touch-simulate]") == null) {
                            triggerTouch(touchType, ev, eventTarget);
                        }
                        if (ev.type === "mouseup") {
                            eventTarget = null;
                        }
                    };
                    const triggerTouch = (eventName, mouseEv, target) => {
                        const touchEvent = new Event(eventName, { bubbles: true, cancelable: true });
                        touchEvent.altKey = mouseEv.altKey;
                        touchEvent.ctrlKey = mouseEv.ctrlKey;
                        touchEvent.metaKey = mouseEv.metaKey;
                        touchEvent.shiftKey = mouseEv.shiftKey;
                        const touches = getActiveTouches(mouseEv, target);
                        touchEvent.touches = touches;
                        touchEvent.targetTouches = touches;
                        touchEvent.changedTouches = createTouchListFromMouse(target, mouseEv);
                        target.dispatchEvent(touchEvent);
                    };
                    const createTouchFromMouse = (target, mouseEv) => new PolyfilledTouch(target, 1, {
                        pageX: mouseEv.pageX,
                        pageY: mouseEv.pageY,
                        screenX: mouseEv.screenX,
                        screenY: mouseEv.screenY,
                        clientX: mouseEv.clientX,
                        clientY: mouseEv.clientY,
                    });
                    const createTouchList = (touches) => {
                        const touchList = [...touches];
                        touchList.item = (index) => touchList[index] ?? null;
                        touchList.identifiedTouch = (id) => touchList[id + 1] ?? null;
                        return touchList;
                    };
                    const createTouchListFromMouse = (target, mouseEv) => createTouchList([createTouchFromMouse(target, mouseEv)]);
                    const getActiveTouches = (mouseEv, target) => {
                        if (mouseEv.type === "mouseup") {
                            return createTouchList([]);
                        }
                        return createTouchListFromMouse(target, mouseEv);
                    };
                    const touchEmulator = () => {
                        window.addEventListener("mousedown", onMouse("touchstart"), true);
                        window.addEventListener("mousemove", onMouse("touchmove"), true);
                        window.addEventListener("mouseup", onMouse("touchend"), true);
                    };
                    touchEmulator.multiTouchOffset = 75;
                    if (!supportTouch) {
                        touchEmulator();
                    }
                    emulatorInitialized = true;
                }
            })(PullToRefresh = Blazor.PullToRefresh || (Blazor.PullToRefresh = {}));
        })(Blazor = FluentUI.Blazor || (FluentUI.Blazor = {}));
    })(FluentUI = Microsoft.FluentUI || (Microsoft.FluentUI = {}));
})(Microsoft || (Microsoft = {}));
//# sourceMappingURL=FluentPullToRefresh.razor.js.map