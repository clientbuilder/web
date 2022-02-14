window.showModal = (identifier) => {
    const modalElement = document.getElementById(identifier);
    const modal = new bootstrap.Modal(modalElement);
    modal.show();
};

window.closeModal = (identifier) => {
    const modalElement = document.getElementById(identifier);
    const modal = bootstrap.Modal.getInstance(modalElement);
    modal.hide();
};

window.showToast = (identifier, dotNetRef) => {
    const toastElement = document.getElementById(identifier);
    const toast = new bootstrap.Toast(toastElement);
    toastElement.addEventListener('hidden.bs.toast', () => {
        dotNetRef.invokeMethodAsync('RemoveToast', identifier);
    })
    toast.show();
}