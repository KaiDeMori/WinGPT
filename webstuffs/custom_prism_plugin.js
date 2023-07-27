(function () {

    if (typeof Prism === 'undefined' || typeof document === 'undefined' || !document.querySelector) {
        return;
    }

    Prism.plugins.toolbar.registerButton('download-another-file', function (env) {
        var pre = env.element.parentNode;
        if (!pre || !/pre/i.test(pre.nodeName) || !pre.hasAttribute('data-src')) {
            return;
        }
        var src = pre.getAttribute('data-src');
        var a = document.createElement('a');
        a.href = src;
        return a;
    });

}());