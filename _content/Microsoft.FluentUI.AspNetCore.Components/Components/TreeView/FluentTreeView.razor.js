export var Microsoft;
(function (Microsoft) {
    var FluentUI;
    (function (FluentUI) {
        var Blazor;
        (function (Blazor) {
            var TreeView;
            (function (TreeView) {
                const controlledCheckboxes = new WeakSet();
                function UpdateCheckStates(id) {
                    const treeView = document.getElementById(id);
                    if (!treeView) {
                        return;
                    }
                    for (const checkbox of treeView.querySelectorAll('fluent-tree-item > fluent-checkbox')) {
                        if (checkbox.closest('fluent-tree') !== treeView) {
                            continue;
                        }
                        const controlled = checkbox.hasAttribute('check-state');
                        if (controlled || controlledCheckboxes.has(checkbox)) {
                            checkbox.checked = checkbox.hasAttribute('checked');
                            checkbox.indeterminate = checkbox.hasAttribute('indeterminate');
                            if (controlled) {
                                controlledCheckboxes.add(checkbox);
                            }
                            else {
                                controlledCheckboxes.delete(checkbox);
                            }
                        }
                    }
                }
                TreeView.UpdateCheckStates = UpdateCheckStates;
                function Initialize(id, multiple) {
                    const treeView = document.getElementById(id);
                    if (treeView && multiple) {
                        treeView.addEventListener('keydown', (event) => {
                            if (event.code === 'Space' && event.target instanceof HTMLElement) {
                                const checkbox = event.target.querySelector('fluent-checkbox');
                                if (checkbox) {
                                    checkbox.click();
                                }
                            }
                        });
                    }
                }
                TreeView.Initialize = Initialize;
            })(TreeView = Blazor.TreeView || (Blazor.TreeView = {}));
        })(Blazor = FluentUI.Blazor || (FluentUI.Blazor = {}));
    })(FluentUI = Microsoft.FluentUI || (Microsoft.FluentUI = {}));
})(Microsoft || (Microsoft = {}));
//# sourceMappingURL=FluentTreeView.razor.js.map