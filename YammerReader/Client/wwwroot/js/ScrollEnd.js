var dotNetHelper;
var ScrollEndMethodName = "";

window.RegisterPage = (dotNetObject, methodName) => {
    if (dotNetHelper == null) {
        dotNetHelper = dotNetObject;
        ScrollEndMethodName = methodName;
    }
}

window.UnRegisterPage = () => {
    dotNetHelper = null;
}

window.addEventListener("scroll", () => {
    //console.log("----------");
    //console.log(window.innerHeight);//視窗高度
    //console.log(window.scrollY);//捲軸垂值位移量
    //console.log(document.body.scrollHeight);
    if ((window.innerHeight + window.scrollY) >= document.body.scrollHeight * 0.8) {
        if (dotNetHelper != null) {
            dotNetHelper.invokeMethodAsync(ScrollEndMethodName);
        }
    }
}); 