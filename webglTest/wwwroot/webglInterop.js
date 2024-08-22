// 전역 변수로 unityInstance 설정
var unityInstance = null; // Unity 인스턴스를 저장할 전역 변수입니다. Unity 인스턴스가 초기화되면 이 변수에 할당됩니다.

// Unity 로딩 중 또는 에러 시 배너를 표시하는 함수
function unityShowBanner(msg, type) {
    // 배너의 가시성을 업데이트하는 내부 함수
    function updateBannerVisibility() {
        warningBanner.style.display = warningBanner.children.length ? 'block' : 'none';
    }

    var div = document.createElement('div'); // 새로운 div 요소를 생성합니다.
    div.innerHTML = msg; // 배너에 표시할 메시지를 설정합니다.
    warningBanner.appendChild(div); // 배너에 새로 생성한 div를 추가합니다.

    // 배너의 종류에 따라 스타일을 설정합니다.
    if (type == 'error') {
        div.style = 'background: red; padding: 10px;'; // 에러 메시지일 경우 빨간 배경과 패딩을 설정합니다.
    } else {
        if (type == 'warning') {
            div.style = 'background: yellow; padding: 10px;'; // 경고 메시지일 경우 노란 배경과 패딩을 설정합니다.
        }
        // 경고 메시지는 5초 후에 자동으로 사라집니다.
        setTimeout(function () {
            warningBanner.removeChild(div); // 배너에서 메시지를 제거합니다.
            updateBannerVisibility(); // 배너의 가시성을 업데이트합니다.
        }, 5000);
    }
    updateBannerVisibility(); // 배너의 가시성을 업데이트합니다.
}

// WebGL 초기화 함수
window.initializeWebGL = function (canvasId) {
    const canvas = document.getElementById(canvasId); // HTML에서 캔버스 요소를 가져옵니다.
    var buildUrl = "Build/Build"; // Unity 빌드 파일들이 위치한 URL 경로입니다.
    var loaderUrl = buildUrl + "/Build2.loader.js"; // Unity 로더 스크립트 파일의 URL입니다.

    // Unity 인스턴스를 초기화할 때 사용할 구성 설정입니다.
    var config = {
        dataUrl: buildUrl + "/Build2.data.unityweb", // Unity 데이터 파일의 URL입니다.
        frameworkUrl: buildUrl + "/Build2.framework.js.unityweb", // Unity 프레임워크 파일의 URL입니다.
        codeUrl: buildUrl + "/Build2.wasm.unityweb", // Unity WebAssembly 파일의 URL입니다.
        streamingAssetsUrl: "StreamingAssets", // 스트리밍 자산의 URL입니다.
        companyName: "DefaultCompany", // Unity 프로젝트의 회사 이름입니다.
        productName: "My project", // Unity 프로젝트의 제품 이름입니다.
        productVersion: "1.0", // Unity 프로젝트의 버전입니다.
        showBanner: unityShowBanner, // 로딩 중 또는 에러 시 배너를 표시하는 함수입니다.
    };

    var script = document.createElement("script"); // 새로운 script 요소를 생성합니다.
    script.src = loaderUrl; // 로더 스크립트의 소스 URL을 설정합니다.

    // 로더 스크립트가 로드된 후 실행될 콜백 함수입니다.
    script.onload = () => {
        // Unity 인스턴스를 생성합니다.
        createUnityInstance(canvas, config).then((instance) => {
            unityInstance = instance; // Unity 인스턴스를 전역 변수에 할당합니다.
        }).catch((message) => {
            alert(message); // 에러가 발생하면 경고 메시지를 표시합니다.
        });
    };

    document.body.appendChild(script); // 로더 스크립트를 문서에 추가하여 실행합니다.
}
