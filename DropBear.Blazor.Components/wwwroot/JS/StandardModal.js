class StandardModal {
  static initialize(element) {
    element.addEventListener('keydown', (e) => {
      if (e.key === 'Escape') {
        element.dispatchEvent(new Event('close'));
      }
    });

    this.setupFocusTrap(element);
  }

  static show(element) {
    element.classList.add('active');
    element.focus();
  }

  static hide(element) {
    element.classList.remove('active');
  }

  static setupFocusTrap(element) {
    const focusableElements = element.querySelectorAll('button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])');
    const firstFocusableElement = focusableElements[0];
    const lastFocusableElement = focusableElements[focusableElements.length - 1];

    element.addEventListener('keydown', (e) => {
      if (e.key === 'Tab') {
        if (e.shiftKey && document.activeElement === firstFocusableElement) {
          e.preventDefault();
          lastFocusableElement.focus();
        } else if (!e.shiftKey && document.activeElement === lastFocusableElement) {
          e.preventDefault();
          firstFocusableElement.focus();
        }
      }
    });
  }
}

window.standardModal = StandardModal;
