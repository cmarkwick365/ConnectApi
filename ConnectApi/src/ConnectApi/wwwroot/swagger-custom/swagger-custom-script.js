(function () {
    window.addEventListener("load", function () {
        setTimeout(function () {
            var logo = document.getElementsByClassName('link'); //For Changing The Link On The Logo Image
            logo[0].href = "https://lightspeedautomation.com/";
            logo[0].target = "_blank";
            logo[0].children[0].alt = "LightSpeed Automation";
            logo[0].children[0].src = "https://lightspeedautomation.com/wp-content/uploads/2021/11/logo.png"; //For Changing The Logo Image
        });
    });
})();