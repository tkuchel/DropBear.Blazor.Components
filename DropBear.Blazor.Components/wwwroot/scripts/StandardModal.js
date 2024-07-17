class StandardModal {
  constructor() {
    this.activeModal = null;
  }

  initialize(element) {
    if (element && element.classList) {
      element.querySelector('.modal-close')?.addEventListener('click', () => this.hide(element));
      element.addEventListener('click', event => {
        if (event.target === element) {
          this.hide(element);
        }
      });
    } else {
      console.error('Invalid element passed to StandardModal.initialize');
    }
  }

  show(element) {
    if (element && element.classList) {
      element.style.display = 'flex';
      setTimeout(() => {
        element.classList.add('active');
        this.activeModal = element;
        this.trapFocus(element);
      }, 10);
    }
  }

  hide(element) {
    if (element && element.classList) {
      element.classList.remove('active');
      setTimeout(() => {
        element.style.display = 'none';
        this.activeModal = null;
      }, 300);
    }
  }

  trapFocus(element) {
    const focusableElements = element.querySelectorAll('button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])');
    const firstFocusableElement = focusableElements[0];
    const lastFocusableElement = focusableElements[focusableElements.length - 1];

    element.addEventListener('keydown', e => {
      if (e.key === 'Tab') {
        if (e.shiftKey) {
          if (document.activeElement === firstFocusableElement) {
            lastFocusableElement.focus();
            e.preventDefault();
          }
        } else {
          if (document.activeElement === lastFocusableElement) {
            firstFocusableElement.focus();
            e.preventDefault();
          }
        }
      }

      if (e.key === 'Escape') {
        this.hide(element);
      }
    });

    firstFocusableElement.focus();
  }
}

window.standardModal = new StandardModal();
