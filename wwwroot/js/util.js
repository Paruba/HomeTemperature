let conteinerDotnetHelper = null;

window.setConteinerDotnetHelper = function setConteinerDotnetHelper(helper) {
    conteinerDotnetHelper = helper;
    document.addEventListener('keydown', handleKeyNavigationDown);
}

window.initDetailView = function initDetailView() {
    keyNavigation();
}

window.clickButton = (buttonId) => {
    var button = document.getElementById(buttonId);
    if (button) {
        button.click();
    }
}

window.scrollToBottom = function () {
    window.scrollTo(0, document.body.scrollHeight);
}

window.downloadData = function (filename, content) {
    var element = document.createElement('a');
    element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(content));
    element.setAttribute('download', filename);
    element.style.display = 'none';
    document.body.appendChild(element);
    element.click();
    document.body.removeChild(element);
}

window.downloadExcelData = function (filename, base64Content) {
    var element = document.createElement('a');
    element.setAttribute('href', 'data:application/vnd.ms-excel;base64,' + base64Content);
    element.setAttribute('download', filename);
    element.style.display = 'none';
    document.body.appendChild(element);
    element.click();
    document.body.removeChild(element);
}

window.downloadZipData = function (filename, base64Content) {
    var element = document.createElement('a');
    element.setAttribute('href', 'data:application/zip;base64,' + base64Content);
    element.setAttribute('download', filename);
    element.style.display = 'none';
    document.body.appendChild(element);
    element.click();
    document.body.removeChild(element);
}

window.cookieFunctions = {
    setCookie: function (cname, cvalue, exdays) {
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
        console.log("Cookie setted");
    },
    getCookie: function (cname) {
        var name = cname + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }
}

window.viewFunctions = {
    getViewportHeight: function () {
        return window.innerHeight ||
            document.documentElement.clientHeight ||
            document.body.clientHeight;
    },
    registerResizeCallback: function (dotNetReference) {
        window.addEventListener('resize', function () {
            dotNetReference.invokeMethodAsync('UpdateViewportHeight', window.viewFunctions.getViewportHeight());
        });
    },
    isScrolledToBottom: function () {
        return (window.innerHeight + window.scrollY) >= document.body.offsetHeight;
    },
    setupScrollListener: function (dotNetReference) {
        window.addEventListener('scroll', function () {
            dotNetReference.invokeMethodAsync('CheckScrollPosition');
        });
    }
};


window.onscroll = debounce(function () {
    if (window.scrollInfoService != null) {
        window.scrollInfoService.invokeMethodAsync('OnScroll', window.pageYOffset);
    }
});

window.RegisterScrollInfoService = (scrollInfoService) => {
    window.scrollInfoService = scrollInfoService;
}

window.UnRegisterScrollInfoService = () => {
    window.scrollInfoService = null;
}
/**
* Authentication
* */
const LOGOUT_IN_ALL_TABS = 'logoutInAllTabs';
const LOGIN_IN_ALL_TABS = 'loginInAllTabs';
const AUTH_COOKIE_NAME = 'AuthToken';

function getCookie(name) {
    var cookieArr = document.cookie.split(";");

    for (var i = 0; i < cookieArr.length; i++) {
        var cookiePair = cookieArr[i].split("=");

        if (name == cookiePair[0].trim()) {
            return decodeURIComponent(cookiePair[1]);
        }
    }

    return null;
}

function logoutInAllTabs() {
    window.localStorage.removeItem(LOGOUT_IN_ALL_TABS);
    window.sessionStorage.clear();
    document.cookie = AUTH_COOKIE_NAME + '=;Path=/;Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    window.location.href = window.location.origin + '/identity/login'; // Musí být href, aby nastala faktická navigace v prohlížeči a vysypal se state Fluxoru

    setTimeout(function () {
        window.location.reload();
    }, 500);

    window.identityDestroySessionTimeout();
}

function loginInAllTabs() {
    window.localStorage.removeItem(LOGIN_IN_ALL_TABS)
    window.location.replace(window.location.origin); // Auth token je přes cookie, jen tedy refreshneme na hlavní stránku

    window.identityInitializeSessionTimeout(true);
}

window.identityFireLogoutInAllTabs = function fireLogoutInAllTabs() {
    window.localStorage.setItem(LOGOUT_IN_ALL_TABS, true);

    setTimeout(function () {
        window.localStorage.removeItem(LOGOUT_IN_ALL_TABS);
    }, 2000);
};

window.identityFireLoginInAllTabs = function fireLoginInAllTabs() {
    window.localStorage.setItem(LOGIN_IN_ALL_TABS, true);

    setTimeout(function () {
        window.localStorage.removeItem(LOGIN_IN_ALL_TABS);
    }, 2000);
};

window.identityClearAuthCookie = function clearAuthCookie() {
    document.cookie = AUTH_COOKIE_NAME + '=;Path=/;Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
};
window.identityGetAuthCookie = function getAuthCookie() {
    return getCookie(AUTH_COOKIE_NAME);
};

function keyNavigation() {
    document.addEventListener('keydown', handleKeyNavigationDown);
}

function handleKeyNavigationDown(e) {
    switch (e.keyCode) {
        case 37: //left
            conteinerDotnetHelper.invokeMethodAsync('HandleKeyDown', e.keyCode);
            break;
        case 39: //right
            conteinerDotnetHelper.invokeMethodAsync('HandleKeyDown', e.keyCode);
        case 27: //esc
            conteinerDotnetHelper.invokeMethodAsync('HandleKeyDown', e.keyCode);
    }
}

function debounce(func) {
    var timer;
    return function (event) {
        if (timer) clearTimeout(timer);
        timer = setTimeout(func, 100, event);
    };
}