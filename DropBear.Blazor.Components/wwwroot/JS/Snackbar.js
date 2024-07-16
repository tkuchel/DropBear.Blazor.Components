window.snackbarInterop = {
  initialize(element) {
    // This function can be used to set up any necessary event listeners or initializations
  },

  animateProgress(snackbarId, duration) {
    const snackbar = document.querySelector(`[data-snackbar-id="${snackbarId}"]`);
    if (snackbar) {
      const progress = snackbar.querySelector('.snackbar-progress');
      if (progress) {
        progress.style.transition = `width ${duration}ms linear`;
        progress.style.width = '0%';
      }
    }
  },

  removeSnackbar(snackbarId) {
    const snackbar = document.querySelector(`[data-snackbar-id="${snackbarId}"]`);
    if (snackbar) {
      snackbar.style.animation = 'slideOutAndShrink 0.3s ease-out forwards';
      setTimeout(() => snackbar.remove(), 300);
    }
  }
};
