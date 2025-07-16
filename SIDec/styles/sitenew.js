var SecondsAlert = 10;
var sidenav = 180;
timeAlert = 5000;
fadeAlert = 200;

function fnScroll() {
  if ($(document).height() > $(window).height()) {
    if ($('#sidenav').width() == sidenav) {
      $('.gv-w').removeClass('active1');
      $('.gv-w').addClass('active');
    }
    else {
      $('.gv-w').removeClass('active');
      $('.gv-w').addClass('active1');
    }
  }
  else {
    $('.gv-w').removeClass('active');
    $('.gv-w').removeClass('active1');
  }
}

/* ------------------------------------------------------------------------------------------------------
 ---------------------------------------------------   popup modal
-------------------------------------------------------------------------------------------------------- */
function openModal(modal) {
    $('#' + modal).modal('show');
}

function MsgMain(opt) {
  if (opt === 0) {
    $('.alert.alert-dismissible').addClass("d-none");
  }
  else if (opt === 1) {
    $('#msgMain').removeClass("d-none");
  }
  window.resize();
}

/* ------------------------------------------------------------------------------------------------------
 ---------------------------------------------------   gridview
-------------------------------------------------------------------------------------------------------- */
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
  }
}

/* ------------------------------------------------------------------------------------------------------
 ---------------------------------------------------   valores permitidos en textbox
-------------------------------------------------------------------------------------------------------- */
function SoloEntero(evt) {
  if (evt.charCode === 0)
    return true;
  if ((evt.charCode >= 48) && (evt.charCode <= 57)) {
    if (!evt.shiftKey) { return true; }
  }
  return false;
}

function SoloEnteroRango(evt, Min, Max) {
  if (evt.charCode === 0)
        return true;
  var MinValue = Min + 48;
  var MaxValue = Max + 48;
  if ((evt.charCode >= MinValue) && (evt.charCode <= MaxValue)) {
    if (!evt.shiftKey) { return true; }
  }
  return false;
}

function SoloDecimal(evt) {
    var commaCode = (1 / 2).toLocaleString("es-Co").charAt(1);
    if (evt.charCode === 0)
        return true;
    if (evt.charCode >= 48 && evt.charCode <= 57) 
        if (!evt.shiftKey) { return true; }    
    if ((evt.charCode === 44 || evt.charCode === 46) && evt.currentTarget.value.indexOf(commaCode) < 0) {
        evt.target.value = evt.target.value.substring(0, evt.target.selectionStart) + commaCode + evt.target.value.substring(evt.target.selectionStart, evt.target.length);
    }
    return false;
}

function FormatDecimal(control, precision=2, scale=10) {
    const d = control.value; 
    var num = "0";
    var commaCode = (1 / 2).toLocaleString("es-Co").charCodeAt(1);
    var maxValue = 10 ** (scale - precision)

    for (var i = 0; i < d.length; i++) {
        var code = d.charCodeAt(i);
        if(code === commaCode) num += ".";
        if (code >= 48 && code <= 57) num += d.charAt(i);
    }
    control.value = (Number(num) % maxValue).toLocaleString("es-Co", { minimumFractionDigits: precision, maximumFractionDigits: precision });
}

function MaxLengthText(textField, maxLength) {
    if (textField.value.length > maxLength) {
        textField.value = textField.value.substring(0, maxLength);
    } 
}

/* ------------------------------------------------------------------------------------------------------
 ---------------------------------------------------   Anterior
-------------------------------------------------------------------------------------------------------- */

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

function onCalendarHidden() {
    var cal = $find("calendar1");

    if (cal._monthsBody) {
        for (var i = 0; i < cal._monthsBody.rows.length; i++) {
            var row = cal._monthsBody.rows[i];
            for (var j = 0; j < row.cells.length; j++) {
                Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call);
            }
        }
    }
}

function onCalendarShown() {
    var cal = $find("calendar1");

    cal._switchMode("months", true);

    if (cal._monthsBody) {
        for (var i = 0; i < cal._monthsBody.rows.length; i++) {
            var row = cal._monthsBody.rows[i];
            for (var j = 0; j < row.cells.length; j++) {
                Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call);
            }
        }
    }
}

function call(eventElement) {
    var target = eventElement.target;
    switch (target.mode) {
        case "month":
            var cal = $find("calendar1");
            cal._visibleDate = target.date;
            cal.set_selectedDate(target.date);
            cal._blur.post(true);
            cal.raiseDateSelectionChanged();
            break;
    }
}

function Limpiar() {
  if (document.activeElement.id === "txtBuscar" || document.activeElement.id === "") {
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

function divClickAction(ctrlName) {
  document.getElementById(ctrlName).click();
}

function cleanValueZero(e) {
    if (e.value === "0,00" || e.value === "0.00" || e.value === "0")
        e.value = "";
}

/* ------------------------------------------------------------------------------------------------------
 ---------------------------------------------------   load
-------------------------------------------------------------------------------------------------------- */
$(window).resize(function () {

  $('.gvHBar').addClass('active')
  $('.gvHRail').addClass('active')
  $('.gvHArrowR').addClass('active')

  fnScroll();

  var h = $('#header').height() + 15;
  $('.card-header-main').css('top', h);
});

$(document).ready(function () {
  /*modal*/
  $('[role="dialog"].modal').appendTo("body");

  /*sidenav*/
  $('#sidenavCollapse').click(function () {
    $('#sidenav').toggleClass('active');
    if ($('#sidenav').hasClass('active')) {
      $('#sidenavCollapse').css("margin-left", "180px");
      $(this).find($(".fas")).removeClass('fas fa-bars').addClass('fas fa-times');
    }
    else {
      $('#sidenavCollapse').css("margin-left", "0px");
      $(this).find($(".fas")).removeClass('fas fa-times').addClass('fas fa-bars');
    }
  });

  $('.btn-group').on('click', '.btn', function () {
    $(this).addClass('active').siblings().removeClass('active');
  });

  /*tooltip*/
    ////$('a, .btn').tooltip({
    ////    delay: { show: 500, hide: 0 }
    ////});
    ////$('a, .btn').on('click', function () {
    ////    $('a, .btn').tooltip('hide');
    ////    $('[id^=tooltip]').tooltip('hide');
    ////})

  /*alert*/
  $('.alert').alert()
  $('.alert-main').fadeTo(timeAlert, fadeAlert).slideUp(fadeAlert,  function () {
    $(this).slideUp(fadeAlert);
  })

  //var parameter = Sys.WebForms.PageRequestManager.getInstance();
  //parameter.add_endRequest(function () {
  //  /*tooltip*/
  //    $('a, .btn').tooltip({
  //        delay: { show: 500, hide: 0 }
  //    });
  //    $('a, .btn').on('click', function () {
  //        $('a, .btn').tooltip('hide');
  //        $('[id^=tooltip]').tooltip('hide');
  //    })
  //});

});

///Método para usar en los CustomValidator para validar las fecha inválidas
/// requisito el validador debe llamarse igual al textbox cambiando txt_ por cv_
function DateClientValidate(sender, args) {
    var n = sender.id.replace('cv_', 'txt_');
    var v = document.getElementById(n);
    args.IsValid = !v.validity.badInput;
}

$(window).on("load", function () {
  fnScroll();
})

function verDoc() {
    var myWin = window.open("doc.aspx", "mywindow", "menubar=0;toolbar=0,scrollbars=0,resizable=0,titlebar=0,width=1024,height=768");
}

var time;
function onchangetime(t, v) {
    // limpio el timeout cada vez que se ejecute el evento
    clearTimeout(time);
    // vuelvo a setear para resetear el tiempo
    /*serverAction debe ser creada por codigo ScriptManager.RegisterStartupScript*/
    /*v es la opción para multiples llamados*/
    time = setTimeout(function () { serverAction(v) }, t);
}
