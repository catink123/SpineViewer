const externalActions = {
    openFile() {
        fileOpenerInput.click();
    }
}
let sharedBuffer;
const width = 300;
const height = 200;

const fileOpenerInput = document.querySelector('#fileOpenerInput');
fileOpenerInput.addEventListener('change', e => {
    console.log(e.files);
})

window.chrome.webview.addEventListener('message', e => {
    const actionType = e.data.type;
    const payload = e.data.data;
    if (actionType in externalActions)
        externalActions[actionType]();
});

window.chrome.webview.addEventListener('sharedbufferreceived', e => {
    sharedBuffer = e.getBuffer();
    console.log('Shared buffer received! It\'s size:', sharedBuffer.length);

    const sbArr = new Uint8ClampedArray(sharedBuffer);

    const canv = document.createElement('canvas');
    canv.width = width;
    canv.height = height;

    const c = canv.getContext('2d');

    c.textBaseline = 'top';
    c.fillText('Test String!', 10, 10);

    const { data } = c.getImageData(0, 0, width, height);
    sbArr.set(data);

    window.chrome.webview.postMessage({ type: 'frameRendered', data: { frame: 0 } });
});

window.chrome.webview.postMessage({ type: 'bufferRequested', data: { width, height } });