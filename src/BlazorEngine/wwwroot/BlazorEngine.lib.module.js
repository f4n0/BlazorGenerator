import * as fluentUI from "../Microsoft.FluentUI.AspNetCore.Components/Microsoft.FluentUI.AspNetCore.Components.lib.module.js";

export const beforeStart = fluentUI.beforeStart;
export const beforeWebStart = fluentUI.beforeWebStart;
export const beforeServerStart = fluentUI.beforeServerStart;
export const beforeWebAssemblyStart = fluentUI.beforeWebAssemblyStart;

function initialize() {
  window.scrollToEnd = element => {
    if (element) element.scrollTop = element.scrollHeight;
  };

  window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const url = URL.createObjectURL(new Blob([arrayBuffer]));
    const anchor = document.createElement("a");
    try {
      anchor.href = url;
      anchor.download = fileName ?? "";
      anchor.click();
    } finally {
      anchor.remove();
      URL.revokeObjectURL(url);
    }
  };
}

export function afterStarted(blazor) {
  fluentUI.afterStarted(blazor);
  initialize();
}

export function afterWebStarted(blazor) {
  fluentUI.afterWebStarted(blazor);
  initialize();
}

export function afterServerStarted(blazor) {
  fluentUI.afterServerStarted(blazor);
  initialize();
}

export function afterWebAssemblyStarted(blazor) {
  fluentUI.afterWebAssemblyStarted(blazor);
  initialize();
}
