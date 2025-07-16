function totalFolios() {
    var folioInicial = $('#ContentPlaceHolder1_ucFolios_txt_folio_inicial').val().replace(/\./g, "") * 1;
    folioInicial = Number.isInteger(folioInicial * 1) ? folioInicial : 0;

    var numeroFolios = $('#ContentPlaceHolder1_ucFolios_txt_folios').val().replace(/\./g, "") * 1;
    numeroFolios = Number.isInteger(numeroFolios) ? numeroFolios * 1 : 0;

    var folioFinal = numeroFolios === 0 ? 0 : folioInicial + numeroFolios - 1;
    folioFinal = folioFinal.toLocaleString("es-Co", { minimumFractionDigits: 0, maximumFractionDigits: 0 });

    $('#ContentPlaceHolder1_ucFolios_txt_folio_final').val(folioFinal);
}

function serverAction() {
    $('#ContentPlaceHolder1_ucFolios_btnfechaevento').click();
}

function infoFileProject() {
    var name = $("input[ID*='ContentPlaceHolder1_ucFolios_fuLoadProject']").val();
    $('#ContentPlaceHolder1_ucFolios_lblPdfProject').hide();
    name = name.substring(name.lastIndexOf('\\') + 1);
    var ext = name.substring(name.lastIndexOf('.') + 1).toLowerCase();
    if (ext !== 'pdf')
    {
        $('#ContentPlaceHolder1_ucFolios_lblInfoFileProject').html('');
        $('#ContentPlaceHolder1_ucFolios_txt_ruta_archivo').val('');
        $('#ContentPlaceHolder1_ucFolios_lblErrorFileProject').html('Extensión inválida, solo se permite «pdf»');
        $('#ContentPlaceHolder1_ucFolios_lblErrorFileProject').attr('title','Extensión inválida, solo se permite «pdf»');
    } else {
        $('#ContentPlaceHolder1_ucFolios_lblErrorFileProject').html('');
        $('#ContentPlaceHolder1_ucFolios_lblErrorFileProject').attr('title', '');
        $('#ContentPlaceHolder1_ucFolios_lblInfoFileProject').html(name);
        $('#ContentPlaceHolder1_ucFolios_txt_ruta_archivo').val(name);
        Page_ClientValidate('vgFolio');
    }
}
