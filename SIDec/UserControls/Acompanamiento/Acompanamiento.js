
function disabledLink(prefijo) {
    var i = 1;
    if (sessionStorage.getItem("tab_selected." + prefijo) === null)
        sessionStorage.setItem("tab_selected." + prefijo, 1);

    while ($("#tab_" + i).length > 0) {
        (function (index) {
            $("#tab_" + index).click(function (event) {
                event.preventDefault();
                select_tab('tab', index);
            });
        })(i);
        i++;
    }
}

function select_tab(prefijo, tab) {
    var i = 1;
    tab = tab === 0 ? sessionStorage.getItem("tab_selected." + prefijo) : tab;
    sessionStorage.setItem("tab_selected." + prefijo, tab);
    while ($("#" + prefijo + "-" + i).length > 0) { 
        $("#" + prefijo + "-" + i).addClass("hidden");
        $("#" + prefijo + "_" + i).removeClass("active").addClass("nav-link");
        i++;
    }

    $("#" + prefijo + "-" + tab).removeClass("hidden");
    $("#" + prefijo + "_" + tab).addClass("active");
    
}

function esCampoVisible(idCampo) {
    var elemento = document.getElementById(idCampo);

    // Verificar la visibilidad del campo
    while (elemento) {
        var estilo = window.getComputedStyle(elemento);
        if (estilo.display === 'none') {
            return elemento.id; // Al menos un elemento padre tiene display: none
        }
        elemento = elemento.parentElement;
    }

    return ""; // El campo es visible
}

function obtenerCamposConError(validationGroupName) {
    var camposConError = "";

    Page_ClientValidate(validationGroupName);
    for (i = 0; i < Page_Validators.length; i++) {
        if (!Page_Validators[i].isvalid) {
            camposConError = esCampoVisible(Page_Validators[i].controltovalidate);
            if (camposConError !== "") {
                var array = camposConError.split('-')
                select_tab(array[0], array[1])
            }
            $(Page_Validators[i].controltovalidate).focus;
            break;
        }
    }
}


function infoFileLicense() {
    var name = $("input[ID*='ContentPlaceHolder1_ucLicencias_fuLoadLicense']").val();
    $('#ContentPlaceHolder1_ucLicencias_lblPdfLicense').hide();
    name = name.substring(name.lastIndexOf('\\') + 1);
    var ext = name.substring(name.lastIndexOf('.') + 1).toLowerCase();
    if (ext !== 'pdf') {
        $('#ContentPlaceHolder1_ucLicencias_lblInfoFileLicense').html('');
        $('#ContentPlaceHolder1_ucLicencias_txt_ruta_licencia').val('');
        $('#ContentPlaceHolder1_ucLicencias_lblErrorFileLicense').html('Extensión inválida, solo se permite «pdf»');
        $('#ContentPlaceHolder1_ucLicencias_lblErrorFileLicense').attr('title', 'Extensión inválida, solo se permite «pdf»');
    } else {
        $('#ContentPlaceHolder1_ucLicencias_lblErrorFileLicense').html('');
        $('#ContentPlaceHolder1_ucLicencias_lblErrorFileLicense').attr('title', '');
        $('#ContentPlaceHolder1_ucLicencias_lblInfoFileLicense').html(name);
        $('#ContentPlaceHolder1_ucLicencias_txt_ruta_licencia').val(name);
        Page_ClientValidate('vgFvgProyectoLicenciaolio');
    }
}
