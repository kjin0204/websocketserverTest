// 전역 변수로 unityInstance 설정
var unityInstance = null;

function unityShowBanner(msg, type) {
    function updateBannerVisibility() {
        warningBanner.style.display = warningBanner.children.length ? 'block' : 'none';
    }
    var div = document.createElement('div');
    div.innerHTML = msg;
    warningBanner.appendChild(div);
    if (type == 'error') div.style = 'background: red; padding: 10px;';
    else {
        if (type == 'warning') div.style = 'background: yellow; padding: 10px;';
        setTimeout(function () {
            warningBanner.removeChild(div);
            updateBannerVisibility();
        }, 5000);
    }
    updateBannerVisibility();
}


window.initializeWebGL = function (canvasId) {
    const canvas = document.getElementById(canvasId);
    var buildUrl = "Build/Build";
    var loaderUrl = buildUrl + "/Build2.loader.js";
    var config = {
        dataUrl: buildUrl + "/Build2.data.unityweb",
        frameworkUrl: buildUrl + "/Build2.framework.js.unityweb",
        codeUrl: buildUrl + "/Build2.wasm.unityweb",
        streamingAssetsUrl: "StreamingAssets",
        companyName: "DefaultCompany",
        productName: "My project",
        productVersion: "1.0",
        showBanner: unityShowBanner,
    };

    var script = document.createElement("script");
    script.src = loaderUrl;

    script.onload = () => {
        createUnityInstance(canvas, config).then((instance) => {
            unityInstance = instance;  // 전역 변수로 unityInstance 설정
        }).catch((message) => {
            alert(message);
        });
    };


    document.body.appendChild(script);
}