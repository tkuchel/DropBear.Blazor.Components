window.contextMenuInterop = {
  initialize(element, dotnetHelper) {
    document.addEventListener('contextmenu', function (e) {
      e.preventDefault();
      dotnetHelper.invokeMethodAsync('Show', e.pageX, e.pageY);
    });

    document.addEventListener('click', function (e) {
      if (!element.contains(e.target)) {
        dotnetHelper.invokeMethodAsync('Hide');
      }
    });
  },

  adjustPosition(element) {
    const rect = element.getBoundingClientRect();
    const windowWidth = window.innerWidth;
    const windowHeight = window.innerHeight;

    if (rect.right > windowWidth) {
      element.style.left = (windowWidth - rect.width) + 'px';
    }

    if (rect.bottom > windowHeight) {
      element.style.top = (windowHeight - rect.height) + 'px';
    }
  },

  focusMenu(element) {
    element.focus();
  }
};
