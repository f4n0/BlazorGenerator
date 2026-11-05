export var Microsoft;
(function (Microsoft) {
    var FluentUI;
    (function (FluentUI) {
        var Blazor;
        (function (Blazor) {
            var DataGrid;
            (function (DataGrid) {
                const headerUiSelector = '[col-header-ui]';
                const closeHeaderUi = (gridElement) => {
                    gridElement.dispatchEvent(new CustomEvent('closecolumnheaderui', { bubbles: true }));
                };
                const hideHeaderUi = (gridElement) => {
                    const element = gridElement.querySelector(headerUiSelector);
                    if (element) {
                        element.style.visibility = 'hidden';
                    }
                };
                const getDeepActiveElement = () => {
                    let activeElement = document.activeElement;
                    while (activeElement instanceof HTMLElement && activeElement.shadowRoot?.activeElement) {
                        activeElement = activeElement.shadowRoot.activeElement;
                    }
                    return activeElement instanceof HTMLElement ? activeElement : null;
                };
                const getFocusedGridElement = (gridElement, event) => {
                    const composedPath = event.composedPath();
                    for (const entry of composedPath) {
                        if (!(entry instanceof HTMLElement)) {
                            continue;
                        }
                        const tableCell = entry.closest('td,th');
                        if (tableCell instanceof HTMLElement && gridElement.contains(tableCell)) {
                            return tableCell;
                        }
                        if (entry === gridElement) {
                            return gridElement;
                        }
                    }
                    const activeElement = getDeepActiveElement();
                    if (activeElement) {
                        const tableCell = activeElement.closest('td,th');
                        if (tableCell instanceof HTMLElement && gridElement.contains(tableCell)) {
                            return tableCell;
                        }
                        const table = activeElement.closest('table');
                        if (table instanceof HTMLElement && table === gridElement) {
                            return table;
                        }
                    }
                    return null;
                };
                const handledArrowNavigationEventFlag = '__fluentDataGridArrowNavigationHandled';
                let grids = [];
                function Initialize(gridElement, autoFocus) {
                    if (!gridElement) {
                        return;
                    }
                    const controller = new AbortController();
                    const { signal } = controller;
                    UpdatePinnedColumnOffsets(gridElement);
                    let start = gridElement.querySelector('td:first-child');
                    if (autoFocus && start) {
                        start.focus();
                    }
                    const tryCloseHeaderUi = (event) => {
                        const element = gridElement?.querySelector(headerUiSelector);
                        if (!element) {
                            return false;
                        }
                        if (event && event.composedPath().indexOf(element) >= 0) {
                            return false;
                        }
                        closeHeaderUi(gridElement);
                        return true;
                    };
                    const bodyClickHandler = (event) => {
                        tryCloseHeaderUi(event);
                    };
                    const bodyKeyDownHandler = (event) => {
                        if (event.key === "Escape") {
                            const closedHeaderUi = tryCloseHeaderUi();
                            if (closedHeaderUi) {
                                gridElement.focus();
                            }
                        }
                    };
                    const keyboardNavigation = (sibling) => {
                        if (sibling) {
                            if (sibling.matches('th')) {
                                const headerButton = sibling.querySelector('[col-sort-button], [col-options-button]');
                                if (headerButton) {
                                    sibling = headerButton;
                                }
                            }
                            const stickyHeader = gridElement.querySelector("tr[row-type='sticky-header']");
                            const focusSibling = () => {
                                if (sibling) {
                                    if (stickyHeader && !stickyHeader.contains(sibling)) {
                                        sibling.style.setProperty('scroll-margin-block-start', `${stickyHeader.offsetHeight}px`);
                                    }
                                    else {
                                        sibling.style.removeProperty('scroll-margin-block-start');
                                    }
                                }
                                sibling?.focus({ preventScroll: true });
                                sibling?.scrollIntoView({ block: 'nearest', inline: 'nearest' });
                                start = sibling ?? null;
                            };
                            setTimeout(() => {
                                focusSibling();
                                requestAnimationFrame(() => {
                                    const activeElement = getDeepActiveElement();
                                    if (!activeElement || !gridElement.contains(activeElement)) {
                                        focusSibling();
                                    }
                                });
                            }, 0);
                        }
                    };
                    const getHeaderButtons = () => {
                        return Array.from(gridElement.querySelectorAll("th[cell-type='columnheader'] [col-sort-button], th[cell-type='columnheader'] [col-options-button]")).filter(button => button.tabIndex >= 0 && !button.hasAttribute('disabled'));
                    };
                    const moveHeaderFocus = (current, shiftKey) => {
                        const headerButtons = getHeaderButtons();
                        const currentIndex = headerButtons.indexOf(current);
                        if (currentIndex < 0) {
                            return false;
                        }
                        const nextIndex = shiftKey ? currentIndex - 1 : currentIndex + 1;
                        if (nextIndex < 0 || nextIndex >= headerButtons.length) {
                            return false;
                        }
                        headerButtons[nextIndex].focus({ preventScroll: true });
                        return true;
                    };
                    const moveFocusIntoHeader = () => {
                        const headerButtons = getHeaderButtons();
                        const firstHeaderButton = headerButtons[0];
                        if (!firstHeaderButton) {
                            return false;
                        }
                        firstHeaderButton.focus({ preventScroll: true });
                        return true;
                    };
                    const getAdjacentRowCell = (cell, direction) => {
                        const row = cell.parentElement;
                        if (!row) {
                            return null;
                        }
                        const rowGroupName = row.parentElement?.tagName.toLowerCase();
                        let targetRow = direction === 'up'
                            ? row.previousElementSibling
                            : row.nextElementSibling;
                        if (!targetRow) {
                            const table = row.closest('table');
                            if (direction === 'down' && rowGroupName === 'thead') {
                                targetRow = table?.querySelector('tbody tr');
                            }
                            else if (direction === 'up' && rowGroupName === 'tbody') {
                                targetRow = table?.querySelector('thead tr:last-child');
                            }
                        }
                        if (!targetRow) {
                            return null;
                        }
                        return targetRow.cells[cell.cellIndex];
                    };
                    const keyDownHandler = (event) => {
                        if (event[handledArrowNavigationEventFlag]) {
                            return;
                        }
                        const isArrowKey = event.key === "ArrowRight" || event.key === "ArrowLeft" || event.key === "ArrowDown" || event.key === "ArrowUp";
                        const targetElement = event.target instanceof HTMLElement ? event.target : null;
                        const isMenuInteraction = event.composedPath().some((entry) => entry instanceof HTMLElement &&
                            (entry.matches('fluent-menu, fluent-menu-list, fluent-menu-item, [role="menu"], [role="menuitem"]') ||
                                !!entry.closest('fluent-menu, fluent-menu-list, fluent-menu-item, [role="menu"], [role="menuitem"]')));
                        if (isArrowKey && isMenuInteraction) {
                            return;
                        }
                        if (event.key === "Tab" && !event.shiftKey) {
                            const activeElement = getDeepActiveElement();
                            if (activeElement && !activeElement.matches('[col-sort-button], [col-options-button]')) {
                                const focusableElements = Array.from(document.querySelectorAll('button, [href], input, select, textarea, [tabindex]:not([tabindex=\"-1\"])')).filter(element => element.tabIndex >= 0 && !element.hasAttribute('disabled') && element.getClientRects().length > 0);
                                const currentIndex = focusableElements.indexOf(activeElement);
                                if (currentIndex >= 0) {
                                    const nextFocusable = focusableElements[currentIndex + 1] ?? null;
                                    if (nextFocusable && getHeaderButtons().includes(nextFocusable) && moveFocusIntoHeader()) {
                                        event.preventDefault();
                                        event.stopPropagation();
                                        return;
                                    }
                                }
                            }
                        }
                        if (event.key === "Tab" && targetElement && (targetElement.matches('[col-sort-button]') || targetElement.matches('[col-options-button]'))) {
                            if (moveHeaderFocus(targetElement, event.shiftKey)) {
                                event.preventDefault();
                                event.stopPropagation();
                                return;
                            }
                        }
                        const headerUiElement = gridElement?.querySelector(headerUiSelector);
                        if (headerUiElement && headerUiElement.contains(event.target)) {
                            if (isArrowKey) {
                                event.stopPropagation();
                                return;
                            }
                        }
                        if (!isArrowKey) {
                            return;
                        }
                        const focusedGridElement = getFocusedGridElement(gridElement, event);
                        if (!(focusedGridElement instanceof HTMLTableCellElement)) {
                            return;
                        }
                        if (targetElement && targetElement !== focusedGridElement && targetElement.closest('[role="gridcell"]') === focusedGridElement) {
                            return;
                        }
                        if (start !== focusedGridElement) {
                            start = focusedGridElement;
                        }
                        if (start !== null && (gridElement.contains(start) || gridElement === start)) {
                            event[handledArrowNavigationEventFlag] = true;
                            const isRTL = getComputedStyle(gridElement).direction === 'rtl';
                            if (event.key === "ArrowUp") {
                                event.preventDefault();
                                const previousSibling = getAdjacentRowCell(start, 'up');
                                keyboardNavigation(previousSibling);
                                event.stopPropagation();
                            }
                            else if (event.key === "ArrowDown") {
                                event.preventDefault();
                                const nextSibling = getAdjacentRowCell(start, 'down');
                                keyboardNavigation(nextSibling);
                                event.stopPropagation();
                            }
                            else if (event.key === "ArrowLeft") {
                                event.preventDefault();
                                const previousSibling = isRTL ? start.nextElementSibling : start.previousElementSibling;
                                keyboardNavigation(previousSibling);
                                event.stopPropagation();
                            }
                            else if (event.key === "ArrowRight") {
                                event.preventDefault();
                                const nextsibling = isRTL ? start.previousElementSibling : start.nextElementSibling;
                                keyboardNavigation(nextsibling);
                                event.stopPropagation();
                            }
                        }
                    };
                    const cells = gridElement.querySelectorAll('[role="gridcell"]');
                    cells.forEach((cell) => {
                        cell.columnDefinition = {
                            columnDataKey: "",
                            cellInternalFocusQueue: true,
                            cellFocusTargetCallback: (cell) => {
                                return cell.children[0];
                            }
                        };
                        cell.addEventListener("keydown", (event) => {
                            if (event.target.role !== "gridcell" && (event.key === "ArrowRight" || event.key === "ArrowLeft")) {
                                event.stopPropagation();
                            }
                        }, { signal });
                    });
                    document.body.addEventListener('click', bodyClickHandler, { signal });
                    document.body.addEventListener('mousedown', bodyClickHandler, { signal });
                    document.body.addEventListener('keydown', bodyKeyDownHandler, { signal });
                    gridElement.addEventListener('keydown', keyDownHandler, { signal, capture: true });
                    return {
                        stop: () => {
                            controller.abort();
                            const grid = grids.find(g => g.id === gridElement.id);
                            if (grid?.resizeController) {
                                grid.resizeController.abort();
                            }
                            if (grid?.reorderController) {
                                grid.reorderController.abort();
                            }
                            grids = grids.filter(grid => grid.id !== gridElement.id);
                        }
                    };
                }
                DataGrid.Initialize = Initialize;
                function CheckColumnPopupPosition(gridElement) {
                    const colPopup = gridElement.querySelector(headerUiSelector);
                    if (colPopup) {
                        const gridRect = gridElement.getBoundingClientRect();
                        const popupRect = colPopup.getBoundingClientRect();
                        const leftOverhang = Math.max(0, gridRect.left - popupRect.left);
                        const rightOverhang = Math.max(0, popupRect.right - gridRect.right);
                        if (leftOverhang || rightOverhang) {
                            const applyOffset = leftOverhang && rightOverhang ? (leftOverhang - rightOverhang) / 2 : (leftOverhang - rightOverhang);
                            colPopup.style.transform = `translateX(${applyOffset}px)`;
                        }
                        colPopup.style.visibility = 'visible';
                        colPopup.scrollIntoViewIfNeeded?.();
                        requestAnimationFrame(() => {
                            const autoFocusElem = colPopup.querySelector('[autofocus]');
                            if (autoFocusElem) {
                                autoFocusElem.focus({ preventScroll: true });
                                return;
                            }
                            const firstFocusable = colPopup.querySelector('button:not([disabled]), [href], input:not([disabled]), select:not([disabled]), textarea:not([disabled]), [tabindex]:not([tabindex="-1"])');
                            if (firstFocusable && firstFocusable.getClientRects().length > 0) {
                                firstFocusable.focus({ preventScroll: true });
                            }
                        });
                    }
                }
                DataGrid.CheckColumnPopupPosition = CheckColumnPopupPosition;
                function DisableColumnResizing(gridElement) {
                    if (!gridElement) {
                        return;
                    }
                    const grid = grids.find(grid => grid.id === gridElement.id);
                    grid?.resizeController?.abort();
                    if (grid) {
                        grid.resizeController = undefined;
                    }
                }
                DataGrid.DisableColumnResizing = DisableColumnResizing;
                function EnableColumnResizing(gridElement, resizeColumnOnAllRows = true) {
                    if (!gridElement) {
                        return;
                    }
                    DisableColumnResizing(gridElement);
                    const columns = [];
                    const headers = gridElement.querySelectorAll("th[cell-type='columnheader'][resizable='true']");
                    if (headers.length === 0) {
                        return;
                    }
                    const id = gridElement.id;
                    let grid = grids.find((g) => g.id === id);
                    const localController = new AbortController();
                    const { signal } = localController;
                    const isGrid = gridElement.getAttribute('display-mode') === 'grid';
                    const handles = [];
                    headers.forEach((header) => {
                        columns.push({
                            header,
                            size: `${isGrid ? header.offsetWidth : header.clientWidth}px`,
                        });
                        const div = createDiv();
                        header.appendChild(div);
                        handles.push(div);
                        if (header.nextElementSibling && header.nextElementSibling.getAttribute('col-pinned') === 'end') {
                            div.style.insetInlineEnd = '-1px';
                        }
                        setListeners(div, signal);
                    });
                    let initialWidths;
                    if (gridElement.style.gridTemplateColumns) {
                        initialWidths = gridElement.style.gridTemplateColumns;
                    }
                    else {
                        initialWidths = columns.map(({ size }) => size).join(' ');
                        if (isGrid) {
                            gridElement.style.gridTemplateColumns = initialWidths;
                        }
                    }
                    if (!grid) {
                        grid = {
                            id,
                            columns,
                            initialWidths,
                            resizeController: localController,
                        };
                        grids.push(grid);
                    }
                    else {
                        const columnsChanged = grid.columns.length !== columns.length;
                        grid.columns = columns;
                        if (columnsChanged) {
                            grid.initialWidths = initialWidths;
                        }
                        grid.resizeController = localController;
                    }
                    const updateHandleGeometry = () => {
                        const height = resizeColumnOnAllRows
                            ? gridElement.offsetHeight
                            : headers[0].offsetHeight - 14;
                        const tops = Array.from(headers, header => header.querySelector('[resize-handle]')?.offsetTop ?? 2);
                        handles.forEach((handle, index) => {
                            handle.style.height = Math.max(0, height - 4) + 'px';
                            handle.style.top = tops[index] + 'px';
                        });
                    };
                    let geometryFrame;
                    const observer = new ResizeObserver(() => {
                        if (geometryFrame === undefined) {
                            geometryFrame = requestAnimationFrame(() => {
                                geometryFrame = undefined;
                                updateHandleGeometry();
                                UpdatePinnedColumnOffsets(gridElement);
                            });
                        }
                    });
                    observer.observe(gridElement);
                    headers.forEach(header => observer.observe(header));
                    signal.addEventListener('abort', () => {
                        observer.disconnect();
                        if (geometryFrame !== undefined) {
                            cancelAnimationFrame(geometryFrame);
                        }
                        handles.forEach(handle => handle.remove());
                    }, { once: true });
                    updateHandleGeometry();
                    function setListeners(div, signal) {
                        let pageX, curCol, curColWidth;
                        let previousDraggable = null;
                        const moveHandler = (e) => {
                            requestAnimationFrame(() => {
                                gridElement.style.tableLayout = 'fixed';
                                if (curCol) {
                                    const isRTL = getComputedStyle(gridElement).direction === 'rtl';
                                    const diffX = isRTL ? (pageX - e.pageX) : (e.pageX - pageX);
                                    const column = columns.find(({ header }) => header === curCol);
                                    const minWidth = getMinWidthPx(column.header);
                                    column.size = Math.max(minWidth, curColWidth + diffX) + 'px';
                                    columns.forEach((col) => {
                                        if (col.size.startsWith('minmax')) {
                                            col.size = (isGrid ? column.header.offsetWidth : col.header.clientWidth) + 'px';
                                        }
                                    });
                                    if (isGrid) {
                                        gridElement.style.gridTemplateColumns = columns
                                            .map(({ size }) => size)
                                            .join(' ');
                                    }
                                    else {
                                        curCol.style.width = column.size;
                                    }
                                    UpdatePinnedColumnOffsets(gridElement);
                                }
                            });
                        };
                        const upHandler = function () {
                            gridElement.removeEventListener('pointermove', moveHandler);
                            gridElement.removeEventListener('pointerup', upHandler);
                            gridElement.removeEventListener('pointerleave', upHandler);
                            gridElement.removeEventListener('pointercancel', upHandler);
                            if (grid) {
                                grid.isResizing = false;
                            }
                            if (curCol) {
                                if (previousDraggable === null) {
                                    curCol.removeAttribute('draggable');
                                }
                                else {
                                    curCol.setAttribute('draggable', previousDraggable);
                                }
                            }
                            curCol = undefined;
                            curColWidth = undefined;
                            pageX = undefined;
                            previousDraggable = null;
                        };
                        signal?.addEventListener('abort', upHandler, { once: true });
                        div.addEventListener('pointerdown', function (e) {
                            curCol = e.target.parentElement;
                            pageX = e.pageX;
                            previousDraggable = curCol.getAttribute('draggable');
                            curCol.setAttribute('draggable', 'false');
                            if (grid) {
                                grid.isResizing = true;
                            }
                            const isGrid = gridElement.getAttribute('display-mode') === 'grid';
                            refreshColumnWidths(columns, isGrid);
                            const padding = isGrid ? 0 : paddingDiff(curCol);
                            curColWidth = curCol.offsetWidth - padding;
                            gridElement.addEventListener('pointermove', moveHandler, { signal });
                            gridElement.addEventListener('pointerup', upHandler, { signal });
                            gridElement.addEventListener('pointerleave', upHandler, { signal });
                            gridElement.addEventListener('pointercancel', upHandler, { signal });
                        }, { signal });
                        div.addEventListener('pointerover', function (e) {
                            e.target.style.borderInlineEnd = 'var(--fluent-data-grid-resize-handle-width) solid var(--fluent-data-grid-resize-handle-color)';
                            if (e.target.previousElementSibling) {
                                e.target.previousElementSibling.style.visibility = 'hidden';
                            }
                        }, { signal });
                        div.addEventListener('pointerup', removeBorder, { signal });
                        div.addEventListener('pointercancel', removeBorder, { signal });
                        div.addEventListener('pointerleave', removeBorder, { signal });
                        div.addEventListener('dragstart', function (e) {
                            e.preventDefault();
                            e.stopPropagation();
                        }, { signal });
                    }
                    function createDiv() {
                        const div = document.createElement('div');
                        div.setAttribute('actual-resize-handle', '');
                        div.style.position = 'absolute';
                        div.style.cursor = 'col-resize';
                        div.style.userSelect = 'none';
                        div.style.width = '6px';
                        div.style.opacity = 'var(--fluent-data-grid-header-opacity)';
                        div.style.insetInlineEnd = '0';
                        return div;
                    }
                    function paddingDiff(col) {
                        if (getStyleVal(col, 'box-sizing') === 'border-box') {
                            return 0;
                        }
                        const padLeft = getStyleVal(col, 'padding-left');
                        const padRight = getStyleVal(col, 'padding-right');
                        return parseInt(padLeft) + parseInt(padRight);
                    }
                    function getStyleVal(elm, css) {
                        return window.getComputedStyle(elm, null).getPropertyValue(css);
                    }
                    function removeBorder(e) {
                        e.target.style.borderInlineEnd = '';
                        if (e.target.previousElementSibling) {
                            e.target.previousElementSibling.style.visibility = 'visible';
                        }
                    }
                }
                DataGrid.EnableColumnResizing = EnableColumnResizing;
                function refreshColumnWidths(columns, isGrid) {
                    columns.forEach(column => {
                        column.size = `${isGrid ? column.header.offsetWidth : column.header.clientWidth}px`;
                    });
                }
                function ResetColumnWidths(gridElement) {
                    const isGrid = gridElement.getAttribute('display-mode') === 'grid';
                    const grid = grids.find(grid => grid.id === gridElement.id);
                    if (!grid) {
                        return;
                    }
                    if (isGrid) {
                        gridElement.style.gridTemplateColumns = grid.initialWidths;
                        const resolvedWidths = window.getComputedStyle(gridElement).gridTemplateColumns.split(' ');
                        grid.columns.forEach((column, index) => {
                            column.size = resolvedWidths[index];
                            column.header.style.width = "";
                        });
                    }
                    else {
                        const columnsWidths = grid.initialWidths.split(' ');
                        grid.columns.forEach((column, index) => {
                            column.size = columnsWidths[index];
                            column.header.style.width = column.size;
                        });
                    }
                    gridElement.dispatchEvent(new CustomEvent('closecolumnheaderui', { bubbles: true }));
                    gridElement.focus();
                }
                DataGrid.ResetColumnWidths = ResetColumnWidths;
                function DisableColumnReordering(gridElement) {
                    if (!gridElement) {
                        return;
                    }
                    const grid = grids.find(grid => grid.id === gridElement.id);
                    grid?.reorderController?.abort();
                    if (grid) {
                        grid.reorderController = undefined;
                    }
                }
                DataGrid.DisableColumnReordering = DisableColumnReordering;
                function EnableColumnReordering(gridElement, dotNetHelper) {
                    if (!gridElement) {
                        return;
                    }
                    const id = gridElement.id;
                    let grid = grids.find((g) => g.id === id);
                    DisableColumnReordering(gridElement);
                    const controller = new AbortController();
                    const { signal } = controller;
                    if (!grid) {
                        grid = {
                            id,
                            columns: [],
                            initialWidths: ''
                        };
                        grids.push(grid);
                    }
                    grid.reorderController = controller;
                    const headers = Array.from(gridElement.querySelectorAll("th[cell-type='columnheader'][reorderable='true']"));
                    headers.forEach(header => header.setAttribute('draggable', 'true'));
                    let sourceColumnKey = null;
                    let sourceColumnIndex = null;
                    const setColumnAttribute = (columnIndex, attributeName, enabled = true) => {
                        if (!columnIndex) {
                            return;
                        }
                        const columnCells = gridElement.querySelectorAll(`[col-index="${columnIndex}"]`);
                        columnCells.forEach(cell => {
                            if (enabled) {
                                cell.setAttribute(attributeName, 'true');
                            }
                            else {
                                cell.removeAttribute(attributeName);
                            }
                        });
                    };
                    const clearColumnAttribute = (attributeName) => {
                        const cells = gridElement.querySelectorAll(`[${attributeName}]`);
                        cells.forEach(cell => {
                            cell.removeAttribute(attributeName);
                        });
                    };
                    const clearDragState = () => {
                        clearColumnAttribute('col-reorder-dragging');
                        clearColumnAttribute('col-reorder-drop-target');
                        gridElement.removeAttribute('col-reorder-active');
                        sourceColumnKey = null;
                        sourceColumnIndex = null;
                    };
                    signal.addEventListener('abort', () => {
                        clearDragState();
                        headers.forEach(header => header.removeAttribute('draggable'));
                    }, { once: true });
                    headers.forEach(header => {
                        header.addEventListener('dragstart', event => {
                            if (grid?.isResizing) {
                                event.preventDefault();
                                return;
                            }
                            hideHeaderUi(gridElement);
                            sourceColumnKey = header.dataset.columnKey ?? null;
                            sourceColumnIndex = header.getAttribute('col-index');
                            if (!sourceColumnKey || !sourceColumnIndex) {
                                return;
                            }
                            gridElement.setAttribute('col-reorder-active', 'true');
                            setColumnAttribute(sourceColumnIndex, 'col-reorder-dragging');
                            event.dataTransfer?.setData('text/plain', sourceColumnKey);
                            if (event.dataTransfer) {
                                event.dataTransfer.effectAllowed = 'move';
                            }
                        }, { signal });
                        header.addEventListener('dragend', () => {
                            clearDragState();
                            closeHeaderUi(gridElement);
                        }, { signal });
                        header.addEventListener('dragover', event => {
                            if (!sourceColumnKey || !sourceColumnIndex) {
                                return;
                            }
                            const targetColumnIndex = header.getAttribute('col-index');
                            if (!targetColumnIndex || targetColumnIndex === sourceColumnIndex) {
                                return;
                            }
                            event.preventDefault();
                            clearColumnAttribute('col-reorder-drop-target');
                            setColumnAttribute(targetColumnIndex, 'col-reorder-drop-target');
                        }, { signal });
                        header.addEventListener('drop', event => {
                            if (!sourceColumnKey || !sourceColumnIndex) {
                                clearColumnAttribute('col-reorder-drop-target');
                                return;
                            }
                            event.preventDefault();
                            const targetColumnKey = header.dataset.columnKey ?? null;
                            const targetColumnIndex = header.getAttribute('col-index');
                            clearColumnAttribute('col-reorder-drop-target');
                            if (!targetColumnKey || !targetColumnIndex || targetColumnIndex === sourceColumnIndex) {
                                return;
                            }
                            const insertAfter = Number(sourceColumnIndex) < Number(targetColumnIndex);
                            dotNetHelper.invokeMethodAsync('ReorderColumnFromDragAsync', sourceColumnKey, targetColumnKey, insertAfter)
                                .catch((err) => console.error('Error invoking Blazor method:', err));
                        }, { signal });
                    });
                }
                DataGrid.EnableColumnReordering = EnableColumnReordering;
                function ResizeColumnDiscrete(gridElement, column, change) {
                    const isGrid = gridElement.getAttribute('display-mode') === 'grid';
                    const columns = [];
                    let headerBeingResized;
                    if (!column) {
                        const targetElement = document.activeElement?.parentElement;
                        if (!(targetElement && targetElement.getAttribute('cell-type') === 'columnheader' && targetElement.getAttribute('resizable') === 'true')) {
                            return;
                        }
                        headerBeingResized = targetElement;
                    }
                    else {
                        headerBeingResized = gridElement.querySelector("th[cell-type='columnheader'][col-index='" + column + "']");
                    }
                    const grid = grids.find(grid => grid.id === gridElement.id);
                    refreshColumnWidths(grid.columns, isGrid);
                    grid.columns.forEach((column) => {
                        if (column.header === headerBeingResized) {
                            const width = headerBeingResized.offsetWidth + change;
                            if (change < 0) {
                                column.size = Math.max(getMinWidthPx(column.header), width) + 'px';
                            }
                            else {
                                column.size = width + 'px';
                            }
                            column.header.style.width = column.size;
                        }
                        if (isGrid) {
                            if (column.size.startsWith('minmax')) {
                                column.size = column.header.offsetWidth + 'px';
                            }
                            columns.push(column.size);
                        }
                    });
                    if (isGrid) {
                        gridElement.style.gridTemplateColumns = columns.join(' ');
                    }
                }
                DataGrid.ResizeColumnDiscrete = ResizeColumnDiscrete;
                function ResizeColumnExact(gridElement, column, width) {
                    const isGrid = gridElement.getAttribute('display-mode') === 'grid';
                    const columns = [];
                    let headerBeingResized = gridElement.querySelector("th[cell-type='columnheader'][col-index='" + column + "']");
                    if (!headerBeingResized) {
                        return;
                    }
                    const grid = grids.find(grid => grid.id === gridElement.id);
                    refreshColumnWidths(grid.columns, isGrid);
                    grid.columns.forEach((column) => {
                        if (column.header === headerBeingResized) {
                            column.size = Math.max(getMinWidthPx(column.header), width) + 'px';
                            column.header.style.width = column.size;
                        }
                        if (isGrid) {
                            if (column.size.startsWith('minmax')) {
                                column.size = column.header.offsetWidth + 'px';
                            }
                            columns.push(column.size);
                        }
                    });
                    if (isGrid) {
                        gridElement.style.gridTemplateColumns = columns.join(' ');
                    }
                    gridElement.dispatchEvent(new CustomEvent('closecolumnheaderui', { bubbles: true }));
                    gridElement.focus();
                }
                DataGrid.ResizeColumnExact = ResizeColumnExact;
                function AutoFitGridColumns(gridElement, columnCount) {
                    let gridTemplateColumns = '';
                    for (let i = 0; i < columnCount; i++) {
                        const columnWidths = Array
                            .from(gridElement.querySelectorAll(`[col-index="${i + 1}"]`))
                            .map((x) => x.offsetWidth);
                        const maxColumnWidth = Math.max(...columnWidths);
                        gridTemplateColumns += ` ${maxColumnWidth}px`;
                    }
                    gridElement.style.gridTemplateColumns = gridTemplateColumns;
                    gridElement.removeAttribute('auto-fit');
                    const grid = grids.find(grid => grid.id === gridElement.id);
                    if (grid) {
                        grid.initialWidths = gridTemplateColumns;
                    }
                }
                DataGrid.AutoFitGridColumns = AutoFitGridColumns;
                function DynamicItemsPerPage(gridElement, dotNetObject, rowSize) {
                    const observer = new ResizeObserver(() => {
                        const visibleRows = calculateVisibleRows(gridElement, rowSize);
                        dotNetObject.invokeMethodAsync('UpdateItemsPerPageAsync', visibleRows)
                            .catch((err) => console.error('Error invoking Blazor method:', err));
                    });
                    const targetElement = gridElement.parentElement;
                    if (targetElement) {
                        observer.observe(targetElement);
                    }
                }
                DataGrid.DynamicItemsPerPage = DynamicItemsPerPage;
                function getMinWidthPx(header) {
                    const configuredMinWidth = header.style.minWidth;
                    if (configuredMinWidth) {
                        const parsedInlineMinWidth = parseInt(configuredMinWidth, 10);
                        return Number.isNaN(parsedInlineMinWidth) ? 100 : parsedInlineMinWidth;
                    }
                    const parsedComputedMinWidth = parseInt(getComputedStyle(header).minWidth, 10);
                    return Number.isNaN(parsedComputedMinWidth) ? 100 : parsedComputedMinWidth;
                }
                function calculateVisibleRows(gridElement, rowHeight) {
                    if (rowHeight <= 0) {
                        return 0;
                    }
                    const gridContainer = gridElement.parentElement;
                    if (!gridContainer) {
                        return 0;
                    }
                    const availableHeight = gridContainer?.clientHeight || window.visualViewport?.height || window.innerHeight;
                    const visibleRows = Math.max(Math.floor(availableHeight / rowHeight), 1);
                    return visibleRows;
                }
                function UpdatePinnedColumnOffsets(gridElement) {
                    const isGrid = gridElement.getAttribute('display-mode') === 'grid';
                    function headerWidth(header) {
                        return isGrid ? header.offsetWidth : header.clientWidth;
                    }
                    function applyOffset(header, offset, side) {
                        const colIndex = header.getAttribute('col-index');
                        if (!colIndex) {
                            return offset;
                        }
                        gridElement.querySelectorAll(`:scope > thead > tr > [col-index="${colIndex}"], :scope > tbody > tr:not([row-state='detail-content']) > [col-index="${colIndex}"]`)
                            .forEach(cell => { cell.style[side] = offset + 'px'; });
                        return offset + headerWidth(header);
                    }
                    const startPinnedHeaders = Array.from(gridElement.querySelectorAll(":scope > thead > tr > th[col-pinned='start']"));
                    let startOffset = 0;
                    for (const header of startPinnedHeaders) {
                        startOffset = applyOffset(header, startOffset, 'insetInlineStart');
                    }
                    const endPinnedHeaders = Array.from(gridElement.querySelectorAll(":scope > thead > tr > th[col-pinned='end']"));
                    let endOffset = 0;
                    for (let i = endPinnedHeaders.length - 1; i >= 0; i--) {
                        endOffset = applyOffset(endPinnedHeaders[i], endOffset, 'insetInlineEnd');
                    }
                }
                DataGrid.UpdatePinnedColumnOffsets = UpdatePinnedColumnOffsets;
            })(DataGrid = Blazor.DataGrid || (Blazor.DataGrid = {}));
        })(Blazor = FluentUI.Blazor || (FluentUI.Blazor = {}));
    })(FluentUI = Microsoft.FluentUI || (Microsoft.FluentUI = {}));
})(Microsoft || (Microsoft = {}));
//# sourceMappingURL=FluentDataGrid.razor.js.map