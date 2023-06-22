const externalActions = {
    openFile() {
        fileOpenerInput.click();
    }
}

const fileOpenerInput = document.querySelector('#fileOpenerInput');
fileOpenerInput.addEventListener('change', e => {
    console.log(e.files);
})

window.chrome.webview.addEventListener('message', e => {
    const actionType = e.data.type;
    const payload = e.data.data;
    if (actionType in externalActions)
        externalActions[actionType]();
})