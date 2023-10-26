// Mobile device style: fill the whole browser client area with the game canvas:
var meta = document.createElement('meta');
meta.name = 'viewport';
meta.content = 'width=device-width, height=device-height, initial-scale=1.0, user-scalable=no, shrink-to-fit=yes';
document.getElementsByTagName('head')[0].appendChild(meta);

const canvas = document.querySelector("canvas");
canvas.width = window.innerWidth
canvas.height = window.innerHeight

//Create a listener to resize the canvas when the window is resized
window.addEventListener("resize", function() {
canvas.width = window.innerWidth
canvas.height = window.innerHeight
})

createUnityInstance(document.querySelector("#unity-canvas"), {
dataUrl: "Build/public.data",
frameworkUrl: "Build/public.framework.js",
codeUrl: "Build/public.wasm",
streamingAssetsUrl: "StreamingAssets",
companyName: "Griffin Teller",
productName: "personal-website",
productVersion: "0.1",
// matchWebGLToCanvasSize: false, // Uncomment this to separately control WebGL canvas render size and DOM element size.
// devicePixelRatio: 1, // Uncomment this to override low DPI rendering on high DPI displays.
});