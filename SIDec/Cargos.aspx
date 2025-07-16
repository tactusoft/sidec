<%@ Page Title="" Language="C#" MasterPageFile="~/Authentic.Master" AutoEventWireup="true" CodeBehind="Cargos.aspx.cs" Inherits="SIDec.Cargos" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
  <div class="divContent">

    <div class="divBuscar">
      <asp:TextBox runat="server" ID="txtBuscar" CssClass="txtBuscar" TextMode="Search" ClientIDMode="Static"/> 
      <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="btn-sm btn-primary w100 mr5" CausesValidation="false" OnClick="btnBuscar_Click" />
      <input type="button" onclick="Limpiar();" class="btn-sm btn-secondary w100" value="Limpiar" />

      <%--Valores para scroll de los GV--%>
      <asp:HiddenField ID="hfGVCargosSV" runat="server"/> 
      <asp:HiddenField ID="hfGVCargosSH" runat="server"/>  
    </div>
   
    <asp:UpdatePanel runat="server" ID="upTab" ChildrenAsTriggers="true" UpdateMode="Conditional">
      <ContentTemplate>
        <div class="divSP" onclick="HideMensajeCRUD();">    
          <div class="divSPHeader">
            <asp:UpdatePanel runat="server" ID="upCargosBtnVistas" UpdateMode="Conditional">
              <ContentTemplate>
                <div class="divSPHeaderTitle">Cargos</div>
                <div class="divSPHeaderButton">
                  <asp:LinkButton ID="btnCargosVG" runat="server" CssClass="btn7" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnCargosVista_Click">
                    <i class="fas fa-th"></i>
                  </asp:LinkButton>
                  <asp:LinkButton ID="btnCargosVD" runat="server" CssClass="btn7" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnCargosVista_Click">
                    <i class="fas fa-bars"></i>
                  </asp:LinkButton>  
                </div>
              </ContentTemplate>
            </asp:UpdatePanel>      
          </div> 
  
          <div class="">
            <asp:UpdatePanel runat="server" ID="upCargos" UpdateMode="Conditional">
              <ContentTemplate>
                <div class="divSPGV"> 
                  <asp:MultiView runat="server" ID="mvCargos" ActiveViewIndex="0" OnActiveViewChanged="mvCargos_ActiveViewChanged">
                    <asp:View runat="server" ID="vCargosGrid">
                      <asp:GridView id="gvCargos" CssClass="gv" runat="server" DataKeyNames="au_cargo" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="50" AllowSorting="true"
                                OnSelectedIndexChanged="gvCargos_SelectedIndexChanged" OnSorting="gvCargos_Sorting" OnPageIndexChanging="gvCargos_PageIndexChanging" 
                                OnRowCreated="gvCargos_RowCreated" OnDataBinding="gvCargos_DataBinding" OnRowDataBound="gvCargos_RowDataBound">
                        <Columns>                
                          <asp:BoundField DataField="au_cargo" HeaderText="Código"/>
                          <asp:BoundField DataField="nombre_cargo" HeaderText="Cargo" SortExpression="usuario"/>                        
                        </Columns> 
                        <SelectedRowStyle CssClass="gvItemSelected"/>
                        <HeaderStyle CssClass="gvHeader"/>                            
                          <RowStyle CssClass="gvItem"/>
                          <PagerStyle CssClass="gvPager"/>       
                      </asp:GridView>
                    </asp:View>

                    <asp:View runat="server" ID="vCargosDetalle">                
                      <asp:ValidationSummary runat="server" ID="vsCargos" DisplayMode="SingleParagraph" HeaderText="Falta informar: " ShowMessageBox="false" ShowSummary="true" ValidationGroup="vgCargos" EnableClientScript="false"/> 
                      <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Cargo} " Display="Dynamic" ValidationGroup="vgCargos" ControlToValidate="txtCargo" />
                    
                      <div class="w_96a pt20">
                        <label class="lbl2 lblDis">Código</label>
                          <asp:TextBox runat="server" ID="txtAuCargo" CssClass="txt2 txtDis w100" Enabled="false"></asp:TextBox>
                        <br />
                        <label class="lbl2">Cargo</label>
                          <asp:TextBox runat="server" ID="txtCargo" CssClass="txt2 w200" MaxLength="50" TabIndex="1"></asp:TextBox>
                      </div>
                    </asp:View>         
                  </asp:MultiView>
                </div>           
              </ContentTemplate>
              <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click"/>                                        
                <asp:AsyncPostBackTrigger ControlID="btnCargosVG" EventName="Click"/>
                <asp:AsyncPostBackTrigger ControlID="btnCargosVD" EventName="Click"/>
                <asp:AsyncPostBackTrigger ControlID="btnFirstCargos" EventName="Click"/>
                <asp:AsyncPostBackTrigger ControlID="btnBackCargos" EventName="Click"/>
                <asp:AsyncPostBackTrigger ControlID="btnNextCargos" EventName="Click"/>
                <asp:AsyncPostBackTrigger ControlID="btnLastCargos" EventName="Click"/>          
                <asp:AsyncPostBackTrigger ControlID="btnCargosEdit" EventName="Click"/>
                <asp:AsyncPostBackTrigger ControlID="btnCargosAdd" EventName="Click"/>
                <asp:AsyncPostBackTrigger ControlID="btnCargosDel" EventName="Click"/>
                <asp:AsyncPostBackTrigger ControlID="gvCargos" EventName="SelectedIndexChanged"/>
              </Triggers>
            </asp:UpdatePanel>
          </div>  
    
          <asp:UpdatePanel runat="server" ID="upCargosFoot" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:HiddenField runat="server" ID="hfEvtGVCargos" Value=""/>
              <div class="divSPMessage" id="DivMsgCargos"></div>

              <div class="divSPFooter">
                <asp:Panel class="divSPFooter1" ID="divCargosNavegacion" runat="server" > 
                  <asp:LinkButton ID="btnFirstCargos" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnCargosNavegacion_Click">
                    <i class="fas fa-angle-double-left"></i>
                  </asp:LinkButton>
                  <asp:LinkButton ID="btnBackCargos" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnCargosNavegacion_Click">
                    <i class="fas fa-angle-left"></i>
                  </asp:LinkButton>
                    <asp:LinkButton ID="btnNextCargos" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnCargosNavegacion_Click">
                    <i class="fas fa-angle-right"></i>
                  </asp:LinkButton>
                  <asp:LinkButton ID="btnLastCargos" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnCargosNavegacion_Click">
                    <i class="fas fa-angle-double-right"></i>
                  </asp:LinkButton>
                </asp:Panel> 

                <div class="divSPFooter2">
                  <asp:Label runat="server" ID="lblCargosCuenta"></asp:Label>
                </div>        
          
                <div class="divSPFooter3">
                  <asp:LinkButton ID="btnCargosCancelar" runat="server" CssClass="btn btn-outline-secondary btn-xs" CommandArgument="0" OnClick="btnCargosCancelar_Click" CausesValidation="false">
                    <i class="fas fa-times"></i>&nbspCancelar
                  </asp:LinkButton>
                  <asp:LinkButton ID="btnCargosAccionFinal" runat="server" CssClass="btn btn-outline-primary btn-xs ml10" ValidationGroup="vgCargos" OnClientClick="if(Page_ClientValidate('vgCargos') && HayCambiosCargos()) return ShowModalPopup();">
                    <i class="fas fa-check"></i>&nbspAceptar
                  </asp:LinkButton>
                </div>
          
                <div class="divSPFooter4" id="divCargosAction">
                  <asp:LinkButton ID="btnCargosAdd" runat="server" CssClass="btn4" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnCargosAccion_Click" >
                    <i class="fas fa-plus"></i>
                  </asp:LinkButton> 
                  <asp:LinkButton ID="btnCargosEdit" runat="server" CssClass="btn4" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnCargosAccion_Click" >
                    <i class="fas fa-pencil-alt"></i>
                  </asp:LinkButton>             
                  <asp:LinkButton ID="btnCargosDel" runat="server" CssClass="btn4" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnCargosAccion_Click" OnClientClick="LimpiarMensajeWeb();">
                    <i class="far fa-trash-alt"></i>
                  </asp:LinkButton>
                </div>
              </div>

              <!--MODAL POPUP-->
              <asp:LinkButton ID="lbDummyCargos" runat="server"></asp:LinkButton>
              <ajaxToolkit:ModalPopupExtender ID="mpeCargos" runat="server" PopupControlID="pnlPopupCargos" TargetControlID="lbDummyCargos" 
                        CancelControlID="btnHideCargos" BackgroundCssClass="modalBackground" BehaviorID="puCargos">
              </ajaxToolkit:ModalPopupExtender>
              <asp:Panel ID="pnlPopupCargos" runat="server" CssClass="modalPopup" Style="display: none">
                  <div class="puHeader">Confirmar acción</div>
                  <div class="puBody">
                      ¿Está seguro de continuar con la acción solicitada?.
                      <br /><br />
	                <div class="btn btn-info glyphicon glyphicon-remove ml20" onclick="divClickAction('<%=btnHideCargos.ClientID%>');">
    	              <asp:Button ID="btnHideCargos" runat="server" Text="Cancelar" CssClass="btnYN" CausesValidation="false" OnClientClick="return HideModalPopup();" />
	                </div>
                  <div class="btn btn-primary glyphicon glyphicon-ok ml20" onclick="divClickAction('<%=btnConfirmarCargos.ClientID%>');">
    	              <asp:Button ID="btnConfirmarCargos" runat="server" Text="Aceptar" CssClass="btnYN" OnClick="btnConfirmar_Click" />
	                </div>
                  </div>
              </asp:Panel>

            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnCargosEdit" EventName="Click" />
              <asp:AsyncPostBackTrigger ControlID="btnCargosAdd" EventName="Click" />
              <asp:AsyncPostBackTrigger ControlID="btnCargosDel" EventName="Click" />
            </Triggers>
          </asp:UpdatePanel>
        </div>           
      </ContentTemplate>
    </asp:UpdatePanel>

  </div>

  <script>
    function pageLoad() {

      //*****************************************LECTURA PARAMETROS CONFIGURACION
      var Ver = '<%=((NameValueCollection)ConfigurationManager.GetSection("PARAMETRIZACION/BotonesAccion"))["Cargos"].ToString()%>';
      if (Ver == "False")
      {
        if (document.getElementById('divCargosAction') != null)
          document.getElementById('divCargosAction').style.display = 'none';
      }


      //*****************************************ESTILO GRIDVIEWS             
      $('#<%=gvCargos.ClientID%>').gridviewScroll({
        width: 400,
        height: 350,
        railcolor: gvValores("railcolor"),
        barcolor: gvValores("barcolor"),
        barhovercolor: gvValores("barhovercolor"),
        bgcolor: gvValores("bgcolor"),
        varrowtopimg: gvValores("varrowtopimg"),
        varrowbottomimg: gvValores("varrowbottomimg"),
        harrowleftimg: gvValores("harrowleftimg"),
        harrowrightimg: gvValores("harrowrightimg"),
        freezesize: 1,
        arrowsize: 16,
        headerrowcount: 1,
        railsize: 16,
        barsize: 12,

        startVertical: $("#<%=hfGVCargosSV.ClientID%>").val(),
        startHorizontal: $("#<%=hfGVCargosSH.ClientID%>").val(),
        onScrollVertical: function (delta) {
          $("#<%=hfGVCargosSV.ClientID%>").val(delta);
        },
        onScrollHorizontal: function (delta) {
          $("#<%=hfGVCargosSH.ClientID%>").val(delta);
        }
       });     
    }

    //*****************************************VERIFICAR CAMBIOS DE DATOS EN PAGINA
    function HayCambiosCargos()
    {
      if (document.getElementById('<%=hfEvtGVCargos.ClientID%>').value != "1")
         return true;

      if (document.getElementById('<%=txtCargo.ClientID%>').defaultValue != document.getElementById('<%=txtCargo.ClientID%>').value)
        return true;
      else
        MensajeCRUD("No se han realizado cambios, no se requiere actualización.", 2, 'DivMsgCargos')

       return false;
    }

     function HideMensajeCRUD() {
       document.getElementById('DivMsgCargos').style.display = "none";
     }

     function MensajeCRUD(mensaje, tipo) {
       var pMensajeCRUD = document.getElementById('DivMsgCargos');
       pMensajeCRUD.innerHTML = mensaje;

       pMensajeCRUD.style.visibility = "visible";

       switch (tipo) {
         case 0: //info
           pMensajeCRUD.className = "divSPMessageOK alert-info";
           break;
         case 1: //success
           pMensajeCRUD.className = "divSPMessageOK alert-success";
           break;
         case 2: //warning
           pMensajeCRUD.className = "divSPMessageOK alert-warning";
           break;
         case 3: //danger
           pMensajeCRUD.className = "divSPMessageOK alert-danger";
           break;
         default: //Ocultar
           pMensajeCRUD.className = "";
           pMensajeCRUD.innerHTML = '';
           pMensajeCRUD.style.visibility = "hidden";
           break;
       }
     }

     function ShowModalPopup() {
       $find('puCargos').show();
       return false;
     }

     function HideModalPopup() {
       $find('puCargos').hide();
       return false;
     }

  </script>

</asp:Content>