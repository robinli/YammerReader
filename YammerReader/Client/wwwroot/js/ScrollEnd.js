var ScrollEnd_DotNetHelper;
var ScrollEnd_MethodName = "";

window.Register_ScrollEndEvent = (dotNetObject, methodName) => {
    if (ScrollEnd_DotNetHelper == null) {
        ScrollEnd_DotNetHelper = dotNetObject;
        ScrollEnd_MethodName = methodName;
    }
}

window.UnRegisterPage = () => {
    ScrollEnd_DotNetHelper = null;
}

window.addEventListener("scroll", () => {
    //console.log("----------");
    //console.log(window.innerHeight);//視窗高度
    //console.log(window.scrollY);//捲軸垂值位移量
    //console.log(document.body.scrollHeight);
    if ((window.innerHeight + window.scrollY) >= document.body.scrollHeight * 0.8) {
        if (ScrollEnd_DotNetHelper != null) {
            ScrollEnd_DotNetHelper.invokeMethodAsync(ScrollEnd_MethodName);
        }
    }
}); 