/// <reference path="../build/build/build.loader.js" />
/// <reference path="../build/build/build.loader.js" />
// wwwroot/webgl/webglInterop.js

//window.initializeWebGL = function (canvasId) {
//    const canvas = document.getElementById(canvasId);
//    const loaderScript = `webgl/Build.loader.js`;


//    console.log("initializeWebGL탔다2");

//    // UnityLoader를 사용하여 WebGL 애플리케이션 초기화
//    UnityLoader.instantiate(canvas, loaderScript
//        , {
//            dataUrl: `webgl/Build.data.unityweb`,
//            frameworkUrl: `webgl/Build.framework.js.unityweb`,
//            wasmUrl: `webgl/Build.wasm.unityweb`
//        }
//    );
//    console.log("initializeWebGL탔다3");
//}


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
    var loaderUrl = buildUrl + "/Build.loader.js";
    var config = {
        dataUrl: buildUrl + "/Build.data.unityweb",
        frameworkUrl: buildUrl + "/Build.framework.js.unityweb",
        codeUrl: buildUrl + "/Build.wasm.unityweb",
        streamingAssetsUrl: "StreamingAssets",
        companyName: "DefaultCompany",
        productName: "My project",
        productVersion: "1.0",
        showBanner: unityShowBanner,
    };

    var script = document.createElement("script");
    script.src = loaderUrl;
    script.onload = () => {
        createUnityInstance(canvas, config
            //, (progress) => {
            //progressBarFull.style.width = 100 * progress + "%";}
        
        ).then((unityInstance) => {
            //loadingBar.style.display = "none";
            //fullscreenButton.onclick = () => {
            //    unityInstance.SetFullscreen(1);
            //};
        }).catch((message) => {
            alert(message);
        });
    };

    document.body.appendChild(script);
}