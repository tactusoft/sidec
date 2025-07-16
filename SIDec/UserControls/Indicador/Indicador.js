function gridviewScrollIndicador() {
    // Verificar que el script jQuery está cargado
    if (typeof $ === 'undefined') {
        console.error("jQuery no está cargado.");
        return;
    }

    // Verificar que el plugin gridviewScroll está cargado
    if (typeof $.fn.gridviewScroll === 'undefined') {
        console.error("gridviewScroll plugin no está cargado.");
        return;
    }

    var gridview = $('#ContentPlaceHolder1_gvDetail');
    var startVertical = $("#ContentPlaceHolder1_hfgvDetalleSV");
    var startHorizontal = $("#ContentPlaceHolder1_hfgvDetalleSH");

    // Verificar que los elementos existen
    if (gridview.length === 0) return;
    if (startVertical.length === 0) return;
    if (startHorizontal.length === 0) return;

    //*****************************************ESTILO GRIDVIEWS
    gridview.gridviewScroll({
        height: 500,
        startVertical: startVertical.val(),
        startHorizontal: startHorizontal.val(),
        onScrollVertical: function (delta) { startVertical.val(delta); },
        onScrollHorizontal: function (delta) { startHorizontal.val(delta); }
    });
}