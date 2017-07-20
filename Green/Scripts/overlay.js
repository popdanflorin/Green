function LoadingOverlay() {
}

LoadingOverlay.prototype = {

    constructor: LoadingOverlay,

    show: function () {
        var overlay = '<div class="x-panel-overlay"><div class="x-panel-mask" style="position: absolute;"></div><div class="x-panel-loading"></div></div>';
        $("body").append(overlay).find('> .x-panel-overlay').show();
    },

    hide: function () {
        $("body").find('> .x-panel-overlay').hide().remove();
    }
}
