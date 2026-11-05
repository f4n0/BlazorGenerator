export var Microsoft;
(function (Microsoft) {
    var FluentUI;
    (function (FluentUI) {
        var Blazor;
        (function (Blazor) {
            var InputFile;
            (function (InputFile) {
                function InitializeFileDropZone(containerElement, inputFile) {
                    function onDragHover(e) {
                        e.preventDefault();
                        containerElement.setAttribute("drop-files", "true");
                    }
                    function onDragLeave(e) {
                        e.preventDefault();
                        containerElement.removeAttribute("drop-files");
                    }
                    function onDrop(e) {
                        e.preventDefault();
                        containerElement.removeAttribute("drop-files");
                        inputFile.files = e.dataTransfer?.files;
                        const event = new Event('change', { bubbles: true });
                        inputFile.dispatchEvent(event);
                    }
                    containerElement?.addEventListener("dragenter", onDragHover);
                    containerElement?.addEventListener("dragover", onDragHover);
                    containerElement?.addEventListener("dragleave", onDragLeave);
                    containerElement?.addEventListener("drop", onDrop);
                    return {
                        dispose: () => {
                            containerElement?.removeEventListener('dragenter', onDragHover);
                            containerElement?.removeEventListener('dragover', onDragHover);
                            containerElement?.removeEventListener('dragleave', onDragLeave);
                            containerElement?.removeEventListener("drop", onDrop);
                        }
                    };
                }
                InputFile.InitializeFileDropZone = InitializeFileDropZone;
                function RaiseFluentInputFile(fileInputId) {
                    var item = document.getElementById(fileInputId);
                    if (!!item) {
                        item.click();
                    }
                }
                InputFile.RaiseFluentInputFile = RaiseFluentInputFile;
                function AttachClickHandler(buttonId, fileInputId) {
                    var button = document.getElementById(buttonId);
                    var fileInput = document.getElementById(fileInputId);
                    if (button && fileInput && !button.hasAttribute("fluentuiBlazorFileInputHandlerAttached")) {
                        button.addEventListener("click", e => {
                            fileInput.click();
                        });
                        button.setAttribute("fluentuiBlazorFileInputHandlerAttached", "true");
                    }
                }
                InputFile.AttachClickHandler = AttachClickHandler;
            })(InputFile = Blazor.InputFile || (Blazor.InputFile = {}));
        })(Blazor = FluentUI.Blazor || (FluentUI.Blazor = {}));
    })(FluentUI = Microsoft.FluentUI || (Microsoft.FluentUI = {}));
})(Microsoft || (Microsoft = {}));
//# sourceMappingURL=FluentInputFile.razor.js.map