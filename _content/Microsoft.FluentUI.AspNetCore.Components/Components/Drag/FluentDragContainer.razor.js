export var Microsoft;
(function (Microsoft) {
    var FluentUI;
    (function (FluentUI) {
        var Blazor;
        (function (Blazor) {
            var DragContainer;
            (function (DragContainer) {
                const ZONE_SELECTOR = '[data-droppable]';
                const DEFAULT_LONG_PRESS_DELAY = 200;
                const MOVE_TOLERANCE = 10;
                const OVER_THROTTLE = 100;
                const SCROLL_EDGE = 48;
                const SCROLL_SPEED = 12;
                const instances = new Map();
                function Initialize(containerId, dotNetRef, longPressDelay) {
                    const container = document.getElementById(containerId);
                    if (!container) {
                        return;
                    }
                    Dispose(containerId);
                    instances.set(containerId, new TouchDragHandler(container, dotNetRef, longPressDelay > 0 ? longPressDelay : DEFAULT_LONG_PRESS_DELAY));
                }
                DragContainer.Initialize = Initialize;
                function Dispose(containerId) {
                    const handler = instances.get(containerId);
                    if (handler) {
                        handler.dispose();
                        instances.delete(containerId);
                    }
                }
                DragContainer.Dispose = Dispose;
                class TouchDragHandler {
                    constructor(container, dotNetRef, longPressDelay) {
                        this.container = container;
                        this.dotNetRef = dotNetRef;
                        this.longPressDelay = longPressDelay;
                        this.queue = Promise.resolve();
                        this.touchId = null;
                        this.startX = 0;
                        this.startY = 0;
                        this.lastX = 0;
                        this.lastY = 0;
                        this.offsetX = 0;
                        this.offsetY = 0;
                        this.candidate = null;
                        this.source = null;
                        this.ghost = null;
                        this.target = null;
                        this.lastOver = 0;
                        this.onTouchStart = (e) => {
                            if (this.touchId !== null || e.touches.length !== 1) {
                                return;
                            }
                            const zone = this.findZone(e.target);
                            if (!zone || zone.getAttribute('data-draggable') !== 'true' || this.isInteractive(e.target, zone)) {
                                return;
                            }
                            if (e.__fluentDragHandled) {
                                return;
                            }
                            e.__fluentDragHandled = true;
                            const touch = e.changedTouches[0];
                            this.touchId = touch.identifier;
                            this.candidate = zone;
                            this.startX = this.lastX = touch.clientX;
                            this.startY = this.lastY = touch.clientY;
                            this.clearTimer();
                            this.timer = window.setTimeout(() => this.beginDrag(), this.longPressDelay);
                        };
                        this.onTouchMove = (e) => {
                            const touch = this.findTouch(e.changedTouches);
                            if (!touch) {
                                return;
                            }
                            this.lastX = touch.clientX;
                            this.lastY = touch.clientY;
                            if (!this.source) {
                                if (Math.hypot(touch.clientX - this.startX, touch.clientY - this.startY) > MOVE_TOLERANCE) {
                                    this.clearTimer();
                                    this.touchId = null;
                                    this.candidate = null;
                                }
                                return;
                            }
                            if (e.cancelable) {
                                e.preventDefault();
                            }
                            this.updateDrag();
                        };
                        this.onTouchEnd = (e) => {
                            const touch = this.findTouch(e.changedTouches);
                            if (!touch) {
                                return;
                            }
                            this.lastX = touch.clientX;
                            this.lastY = touch.clientY;
                            this.clearTimer();
                            if (!this.source) {
                                this.touchId = null;
                                this.candidate = null;
                                return;
                            }
                            if (e.cancelable) {
                                e.preventDefault();
                            }
                            this.updateTarget();
                            this.finish(true);
                        };
                        this.onTouchCancel = (e) => {
                            if (!this.findTouch(e.changedTouches)) {
                                return;
                            }
                            this.clearTimer();
                            if (this.source) {
                                this.finish(false);
                            }
                            else {
                                this.touchId = null;
                                this.candidate = null;
                            }
                        };
                        this.onNativeDragStart = (e) => {
                            if (this.candidate || this.source) {
                                e.preventDefault();
                                e.stopPropagation();
                            }
                        };
                        this.onContextMenu = (e) => {
                            if (this.candidate || this.source) {
                                e.preventDefault();
                            }
                        };
                        this.autoScroll = () => {
                            if (!this.source) {
                                return;
                            }
                            const scrollable = this.findScrollable(document.elementFromPoint(this.lastX, this.lastY));
                            let moved = false;
                            if (scrollable) {
                                const rect = scrollable === document.scrollingElement
                                    ? { top: 0, bottom: window.innerHeight, left: 0, right: window.innerWidth }
                                    : scrollable.getBoundingClientRect();
                                let dx = 0;
                                let dy = 0;
                                if (this.lastY < rect.top + SCROLL_EDGE) {
                                    dy = -SCROLL_SPEED;
                                }
                                else if (this.lastY > rect.bottom - SCROLL_EDGE) {
                                    dy = SCROLL_SPEED;
                                }
                                if (this.lastX < rect.left + SCROLL_EDGE) {
                                    dx = -SCROLL_SPEED;
                                }
                                else if (this.lastX > rect.right - SCROLL_EDGE) {
                                    dx = SCROLL_SPEED;
                                }
                                if (dx !== 0 || dy !== 0) {
                                    const beforeLeft = scrollable.scrollLeft;
                                    const beforeTop = scrollable.scrollTop;
                                    scrollable.scrollBy(dx, dy);
                                    moved = scrollable.scrollLeft !== beforeLeft || scrollable.scrollTop !== beforeTop;
                                }
                            }
                            if (moved) {
                                this.updateTarget();
                            }
                            this.scrollFrame = window.requestAnimationFrame(this.autoScroll);
                        };
                        container.addEventListener('touchstart', this.onTouchStart, { passive: true });
                        container.addEventListener('touchmove', this.onTouchMove, { passive: false });
                        container.addEventListener('touchend', this.onTouchEnd);
                        container.addEventListener('touchcancel', this.onTouchCancel);
                        container.addEventListener('dragstart', this.onNativeDragStart, true);
                        container.addEventListener('contextmenu', this.onContextMenu);
                    }
                    dispose() {
                        this.container.removeEventListener('touchstart', this.onTouchStart);
                        this.container.removeEventListener('touchmove', this.onTouchMove);
                        this.container.removeEventListener('touchend', this.onTouchEnd);
                        this.container.removeEventListener('touchcancel', this.onTouchCancel);
                        this.container.removeEventListener('dragstart', this.onNativeDragStart, true);
                        this.container.removeEventListener('contextmenu', this.onContextMenu);
                        this.clearTimer();
                        this.cleanup();
                    }
                    beginDrag() {
                        this.timer = undefined;
                        const zone = this.candidate;
                        if (!zone || !zone.isConnected) {
                            this.touchId = null;
                            this.candidate = null;
                            return;
                        }
                        this.source = zone;
                        const rect = zone.getBoundingClientRect();
                        this.offsetX = this.startX - rect.left;
                        this.offsetY = this.startY - rect.top;
                        this.ghost = this.createGhost(zone, rect);
                        document.body.appendChild(this.ghost);
                        zone.setAttribute('data-touch-dragging', 'true');
                        this.positionGhost();
                        if (navigator.vibrate) {
                            navigator.vibrate(10);
                        }
                        this.send('TouchDragStartAsync', zone.id);
                        this.updateTarget();
                        this.scrollFrame = window.requestAnimationFrame(this.autoScroll);
                    }
                    updateDrag() {
                        this.positionGhost();
                        const changed = this.updateTarget();
                        const now = Date.now();
                        if (!changed && this.target && now - this.lastOver >= OVER_THROTTLE) {
                            this.lastOver = now;
                            this.send('TouchDragOverAsync', this.source.id, this.target.id);
                        }
                    }
                    updateTarget() {
                        const zone = this.zoneFromPoint(this.lastX, this.lastY);
                        if (zone === this.target) {
                            return false;
                        }
                        const previous = this.target;
                        this.target = zone;
                        if (previous) {
                            this.send('TouchDragLeaveAsync', this.source.id, previous.id);
                        }
                        if (zone) {
                            this.send('TouchDragEnterAsync', this.source.id, zone.id);
                        }
                        return true;
                    }
                    finish(drop) {
                        const source = this.source;
                        const target = this.target;
                        if (source) {
                            if (drop && target && target.getAttribute('data-droppable') === 'true') {
                                this.send('TouchDropAsync', source.id, target.id);
                            }
                            else if (target) {
                                this.send('TouchDragLeaveAsync', source.id, target.id);
                            }
                            this.send('TouchDragEndAsync', source.id);
                        }
                        this.cleanup();
                    }
                    cleanup() {
                        if (this.scrollFrame !== undefined) {
                            window.cancelAnimationFrame(this.scrollFrame);
                            this.scrollFrame = undefined;
                        }
                        this.ghost?.remove();
                        this.source?.removeAttribute('data-touch-dragging');
                        this.ghost = null;
                        this.source = null;
                        this.target = null;
                        this.candidate = null;
                        this.touchId = null;
                    }
                    createGhost(zone, rect) {
                        const ghost = zone.cloneNode(true);
                        ghost.removeAttribute('id');
                        ghost.removeAttribute('data-droppable');
                        ghost.removeAttribute('dragged-over');
                        ghost.querySelectorAll('[id]').forEach(el => el.removeAttribute('id'));
                        const style = ghost.style;
                        style.position = 'fixed';
                        style.left = '0';
                        style.top = '0';
                        style.width = `${rect.width}px`;
                        style.height = `${rect.height}px`;
                        style.margin = '0';
                        style.boxSizing = 'border-box';
                        style.pointerEvents = 'none';
                        style.opacity = '0.85';
                        style.zIndex = '2147483647';
                        style.boxShadow = '0 8px 24px rgba(0, 0, 0, 0.3)';
                        style.transition = 'none';
                        style.userSelect = 'none';
                        style.setProperty('-webkit-user-select', 'none');
                        style.setProperty('-webkit-touch-callout', 'none');
                        return ghost;
                    }
                    positionGhost() {
                        if (this.ghost) {
                            const x = this.lastX - this.offsetX;
                            const y = this.lastY - this.offsetY;
                            this.ghost.style.transform = `translate3d(${x}px, ${y}px, 0) scale(1.03)`;
                        }
                    }
                    zoneFromPoint(x, y) {
                        const element = document.elementFromPoint(x, y);
                        let zone = this.findZone(element);
                        while (zone && zone !== this.source && zone.getAttribute('data-droppable') !== 'true') {
                            zone = this.findZone(zone.parentElement);
                        }
                        if (zone === this.source) {
                            return null;
                        }
                        return zone;
                    }
                    findZone(element) {
                        const escapedContainerId = CSS.escape(this.container.id);
                        const zone = element?.closest(`${ZONE_SELECTOR}[data-container-id="${escapedContainerId}"]`);
                        return zone && this.container.contains(zone) ? zone : null;
                    }
                    isInteractive(element, zone) {
                        const interactive = element.closest('button, a[href], input, select, textarea, [contenteditable="true"], [data-no-drag]');
                        return !!interactive && zone.contains(interactive);
                    }
                    findScrollable(element) {
                        let current = element;
                        while (current && current !== document.body && current !== document.documentElement) {
                            const style = getComputedStyle(current);
                            const canY = /(auto|scroll)/.test(style.overflowY) && current.scrollHeight > current.clientHeight;
                            const canX = /(auto|scroll)/.test(style.overflowX) && current.scrollWidth > current.clientWidth;
                            if (canY || canX) {
                                return current;
                            }
                            current = current.parentElement;
                        }
                        return document.scrollingElement;
                    }
                    findTouch(list) {
                        for (let i = 0; i < list.length; i++) {
                            if (list[i].identifier === this.touchId) {
                                return list[i];
                            }
                        }
                        return null;
                    }
                    clearTimer() {
                        if (this.timer !== undefined) {
                            window.clearTimeout(this.timer);
                            this.timer = undefined;
                        }
                    }
                    send(method, ...args) {
                        this.queue = this.queue
                            .then(() => this.dotNetRef.invokeMethodAsync(method, ...args))
                            .catch(() => { });
                    }
                }
            })(DragContainer = Blazor.DragContainer || (Blazor.DragContainer = {}));
        })(Blazor = FluentUI.Blazor || (FluentUI.Blazor = {}));
    })(FluentUI = Microsoft.FluentUI || (Microsoft.FluentUI = {}));
})(Microsoft || (Microsoft = {}));
//# sourceMappingURL=FluentDragContainer.razor.js.map