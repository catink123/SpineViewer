const externalActions = {
    openFile({ version }) {
        fileOpenerInput.click();
        document.querySelector('p').innerText = version;
    }
}
let sharedBuffer;
const width = 300;
const height = 200;

const fileOpenerInput = document.querySelector('#fileOpenerInput');
fileOpenerInput.addEventListener('change', e => {
    console.log(e.files);
});

window.chrome.webview.addEventListener('message', e => {
    const actionType = e.data.type;
    const payload = e.data.data;
    if (actionType in externalActions)
        externalActions[actionType](payload);
});

function renderFrame() {
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
}

window.chrome.webview.addEventListener('sharedbufferreceived', e => {
    if (e.additionalData.bufferType === 'imageBuffer') {
        sharedBuffer = e.getBuffer();
        console.log('Image buffer received! It\'s size:', sharedBuffer.byteLength);

        renderFrame();
    } else if (e.additionalData.bufferType === 'fileBuffer') {
        console.log('File buffer received!', e.getBuffer());
    }
});

window.chrome.webview.postMessage({ type: 'bufferRequested', data: { width, height } });