window.getFilesFromDataTransfer = () =>
  new Promise(resolve => {
    const handleDrop = e => {
      e.preventDefault();
      const files = [];
      if (e.dataTransfer.items) {
        for (let i = 0; i < e.dataTransfer.items.length; i++) {
          if (e.dataTransfer.items[i].kind === 'file') {
            const file = e.dataTransfer.items[i].getAsFile();
            files.push(JSON.stringify({
              name: file.name,
              size: file.size,
              type: file.type
            }));
          }
        }
      } else {
        for (let i = 0; i < e.dataTransfer.files.length; i++) {
          const file = e.dataTransfer.files[i];
          files.push(JSON.stringify({
            name: file.name,
            size: file.size,
            type: file.type
          }));
        }
      }
      document.removeEventListener('drop', handleDrop);
      resolve(files);
    };

    document.addEventListener('drop', handleDrop, {once: true});
  });
