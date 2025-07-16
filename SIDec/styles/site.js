function LoginOption(opcion) {
    document.getElementById("divLogin").style.display = 'none';
    switch (opcion.id) {
        case 'btnRecuperar':
            document.getElementById("divRecuperar").style.display = 'block';
            document.getElementById("btnRecuperar").style.display = 'none';
            document.getElementById("btnRegistrar").style.display = 'none';
            break;
        case 'btnRegistrar':
            //DivLogin.style.display = 'none';
            break;
        case 'btnIngresar':
            DivLogin.style.display = 'block';
            break;
    }
}

function MensajeWeb(mensaje, tipo) {
    var pMensajeMaster = document.getElementById("pMensaje");
    pMensajeMaster.innerHTML = mensaje;
    pMensajeMaster.style.visibility = "visible";
    switch (tipo) {
        case 0: //info
            pMensajeMaster.className = "text-center alert alert-info m0";
            break;
        case 1: //success
            pMensajeMaster.className = "text-center alert alert-success m0";
            break;
        case 2: //warning
            pMensajeMaster.className = "text-center alert alert-warning m0";
            break;
        case 3: //danger
            pMensajeMaster.className = "text-center alert alert-danger m0";
            break;
        default: //Ocultar
            pMensajeMaster.className = "";
            pMensajeMaster.innerHTML = '';
            pMensajeMaster.style.visibility = "hidden";
            break;
    }
}

function LimpiarMensajeWeb() {
    var pMensaje = document.getElementById("pMensaje");
    pMensaje.className = "";
    pMensaje.innerHTML = '';
}

function SoloEntero(evt) {
    if (evt.charCode == 0)
        return true;
    if ((evt.charCode >= 48) && (evt.charCode <= 57)) {
        if (!evt.shiftKey) { return true; }
    }
    return false;
}

function SoloEnteroRango(evt, Min, Max) {
    if (evt.charCode == 0)
        return true;
    var MinValue = Min + 48;
    var MaxValue = Max + 48;
    if ((evt.charCode >= MinValue) && (evt.charCode <= MaxValue)) {
        if (!evt.shiftKey) { return true; }
    }
    return false;
}

function SoloDecimal(evt) {
    if (evt.charCode == 0)
        return true;
    if (evt.charCode == 46 && evt.currentTarget.value.indexOf(".") >= 0)
        return false;
    if (((evt.charCode >= 48) && (evt.charCode <= 57)) || evt.charCode == 46) {
        if (!evt.shiftKey) { return true; }
    }
    return false;
}

function Limpiar() {
    if (document.activeElement.id == "txtBuscar" || document.activeElement.id == "") {
        ctrlTxtBuscar = document.getElementById("txtBuscar");
        ctrlTxtBuscar.value = "";
        ctrlTxtBuscar.focus();
    }
    document.getElementById('<%= btnBuscar.ClientID %>').click();
}

function SetCursor(ctrlName) {
    var ctrl = document.getElementById(ctrlName);
    var LongitudTexto = document.getElementById(ctrlName).value.length;
    if (ctrl.setSelectionRange) {
        ctrl.focus();
        ctrl.setSelectionRange(LongitudTexto, LongitudTexto);
    }
    else if (ctrl.createTextRange) {
        var range = ctrl.createTextRange();
        range.collapse(true);
        range.moveEnd('character', LongitudTexto);
        range.moveStart('character', LongitudTexto);
        range.select();
    }
}

function gvValores(opcion) {
    switch (opcion) {
        case "railcolor":
            return "#ddd";
        case "barcolor":
            return "#aaa";
        case "barhovercolor":
            return "#888";
        case "bgcolor":
            return "F0F0F0";
        case "varrowtopimg":
            return "Images/icon/arrowvt.png";
        case "varrowbottomimg":
            return "Images/icon/arrowvb.png";
        case "harrowleftimg":
            return "Images/icon/arrowhl.png";
        case "harrowrightimg":
            return "Images/icon/arrowhr.png";
    }
}

function divClickAction(ctrlName) {
    document.getElementById(ctrlName).click();
}

function verDoc() {
    var myWin = window.open("doc.aspx", "mywindow", "menubar=0;toolbar=0,scrollbars=0,resizable=0,titlebar=0,width=1024,height=768");
}
//function verDoc() {
//    var myWin = window.open('doc.aspx', 'mywindow', 'width=860,height=480');
//}

function MaxLengthText(textField, maxLength) {
    if (textField.value.length > maxLength) {
        textField.value = textField.value.substring(0, maxLength);
    }
}