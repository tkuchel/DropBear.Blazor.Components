class ContextMenuInterop {
  static initialize(element, dotnetHelper) {
    document.addEventListener('contextmenu', function (e) {
      e.preventDefault();
      dotnetHelper.invokeMethodAsync('Show', e.pageX, e.pageY);
    });

    document.addEventListener('click', function (e) {
      if (!element.contains(e.target)) {
        dotnetHelper.invokeMethodAsync('Hide');
      }
    });
  }

  static adjustPosition(element) {
    const rect = element.getBoundingClientRect();
    const windowWidth = window.innerWidth;
    const windowHeight = window.innerHeight;

    if (rect.right > windowWidth) {
      element.style.left = (windowWidth - rect.width) + 'px';
    }

    if (rect.bottom > windowHeight) {
      element.style.top = (windowHeight - rect.height) + 'px';
    }
  }

  static focusMenu(element) {
    element.focus();
  }
}

// Make it globally accessible
window.ContextMenuInterop = ContextMenuInterop;
