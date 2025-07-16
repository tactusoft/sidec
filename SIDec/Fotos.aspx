<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Fotos.aspx.cs" Inherits="SIDec.Fotos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
	    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
	    <title>Fotos visita</title>
        <link rel="stylesheet" href="styles/bootstrap/bootstrap.min.css" />
		<link rel="stylesheet" href="styles/fontawesome/5.3.1/css/all.min.css"/>
        <link rel="stylesheet" href="styles/site.css?v=1"/>
        <link rel="stylesheet" href="styles/global.css?v=1"/>
    </head>
    <body class="w1100">
	    <form id="form1" runat="server">
		    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
		    <!-- <div class ="divHeader"> -->
			    <!-- <img alt="" src="./images/header.png" class="imgLogo"/> -->
		    <!-- </div> -->
			<asp:MultiView runat="server" ID="mvFotos" ActiveViewIndex="1">
				<asp:View runat="server" ID="vDefault">
					<br />
					<br />
					<h3 style="color:red; margin:auto;"><span class="glyphicon glyphicon-alert ml100"></span>&nbsp;No puede acceder directamente a esta opcion.</h3>
				</asp:View>
				<asp:View runat="server" ID="vOpciones">
                    <div class="">
					    <label class="lbl6 fl">Fotos visita</label>
					    <div runat="server" id="divUF" visible="false" class ="w600 ml20 fl mb0">                        
						    <asp:ValidationSummary runat="server" ID="vsFotoVisita" DisplayMode="SingleParagraph" HeaderText="Falta informar: " ShowMessageBox="false" ShowSummary="true" ValidationGroup="vgFotoVisita" EnableClientScript="false"/>
						    <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Tipo de foto} " Display="Dynamic" ValidationGroup="vgFotoVisita" ControlToValidate="ddlbTipoFoto" />
                            <br />

						    <label class="lbl2 w80 ml0">Tipo de foto</label>
						    <asp:DropDownList runat="server" ID="ddlbTipoFoto" CssClass="txt2 mt0 w170" TabIndex="1" AppendDataBoundItems="true">
							    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
						    </asp:DropDownList>
						    <label class="lbl2 w80">Descripción</label>
						    <asp:TextBox runat="server" ID="txtDescripcionFoto" CssClass="txt2 mt0 w150" TabIndex="2" MaxLength="20"></asp:TextBox>
                            <br />

						    <asp:FileUpload ID="FileUpload1" CssClass="fl w300 fs11 mt5" runat="server" TabIndex="3" AllowMultiple="false"/>
							<asp:LinkButton ID="lbCancelar" runat="server" CssClass="btn btn-outline-secondary btn-xs" OnClick="lbCancelar_Click" CausesValidation="false">
                                <i class="fas fa-times"></i>&nbspCancelar
							</asp:LinkButton>
						    <%--<asp:LinkButton ID="lbCancelar" runat="server" CssClass="btn btn-outline-secondary btn-xs fl" OnClick="lbCancelar_Click">
							    <span aria-hidden="true" class="glyphicon glyphicon-remove">&nbsp;Cancelar</span>
						    </asp:LinkButton>--%>
							<asp:LinkButton ID="lbSubir" runat="server" CssClass="btn btn-outline-primary btn-xs ml10" TabIndex="1000" Text="Aceptar" ValidationGroup="vgFotoVisita" OnClick="lbSubir_Click" CausesValidation="true">
                                <i class="fas fa-check"></i>&nbspAceptar
							</asp:LinkButton>
						    <%--<asp:LinkButton ID="lbSubir" runat="server" CssClass="btn btn-outline-primary btn-xs ml10 fl" OnClick="lbSubir_Click" CausesValidation="true" ValidationGroup="vgFotoVisita">
							    <span aria-hidden="true" class="glyphicon glyphicon-ok">&nbsp;Aceptar</span>
						    </asp:LinkButton>--%>
					    </div>
					    <asp:LinkButton ID="lbSalir" runat="server" CssClass="btn btn-outline-warning btn-xs mt5 fr" OnClientClick= "closeWindow();" CausesValidation="false">
						    <%--<span aria-hidden="true" class="glyphicon glyphicon-log-out">&nbsp;Salir</span>--%>
                            <span aria-hidden="true" class="fas fa-sign-out-alt col-w"></span>&nbspSalir
					    </asp:LinkButton>
                    </div>
                        
                    <div class="divSP"></div>

					<div runat="server" id="divFotoContainer" class="mtb0 ml2">
						<div class="panel panel-default panelFotosVisitas">
							<div class="panel-heading panelFotoH">
								<asp:Label runat="server" ID="lblTF1" CssClass="lblTipoFoto" Text=""></asp:Label>
								<asp:LinkButton ID="btnAsignar1" runat="server" CssClass="btn6" Text="" CausesValidation="false" CommandName="A1" ToolTip="Agregar foto" OnClick="btnAsignarFoto_Click">
									<i class="fas fa-plus"></i>
								</asp:LinkButton>
								<asp:LinkButton ID="btnEditar1" runat="server" CssClass="btn6" Text=""  CausesValidation="false" CommandName="E1" ToolTip="Editar foto" OnClick="btnEditar_Click">
									<i class="fas fa-pencil-alt"></i>
								</asp:LinkButton>
								<asp:LinkButton ID="btnEliminar1" runat="server" CssClass="btn6" Text="" CausesValidation="false" CommandName="B1" ToolTip="Eliminar foto" OnClick="btnEliminarFoto_Click">
									<i class="far fa-trash-alt"></i>
								</asp:LinkButton>
							</div>
							<div class="panel-body panelFotoB">
								<asp:Image runat="server" ID="Image1" CssClass="imgVisita"/>
							</div>
							<asp:Label runat="server" ID="lblObs1" CssClass="lblDescFoto"></asp:Label>
						</div>

						<div class="panel panel-default panelFotosVisitas">
							<div class="panel-heading panelFotoH">
								<asp:Label runat="server" ID="lblTF2" CssClass="lblTipoFoto" Text=""></asp:Label>
								<asp:LinkButton ID="btnAsignar2" runat="server" CssClass="btn6" Text="" CausesValidation="false" CommandName="A2" ToolTip="Agregar foto" OnClick="btnAsignarFoto_Click">
									<i class="fas fa-plus"></i>
								</asp:LinkButton>
								<asp:LinkButton ID="btnEditar2" runat="server" CssClass="btn6" Text=""  CausesValidation="false" CommandName="E2" ToolTip="Editar foto" OnClick="btnEditar_Click">
									<i class="fas fa-pencil-alt"></i>
								</asp:LinkButton>
								<asp:LinkButton ID="btnEliminar2" runat="server" CssClass="btn6" Text="" CausesValidation="false" CommandName="B2" ToolTip="Eliminar foto" OnClick="btnEliminarFoto_Click">
									<i class="far fa-trash-alt"></i>
								</asp:LinkButton>
							</div>
							<div class="panel-body panelFotoB">
								<asp:Image runat="server" ID="Image2" CssClass="imgVisita"/>
							</div>
							<asp:Label runat="server" ID="lblObs2" CssClass="lblDescFoto"></asp:Label>
						</div>

						<div class="panel panel-default panelFotosVisitas">
							<div class="panel-heading panelFotoH">
								<asp:Label runat="server" ID="lblTF3" CssClass="lblTipoFoto" Text=""></asp:Label>
								<asp:LinkButton ID="btnAsignar3" runat="server" CssClass="btn6" Text="" CausesValidation="false" CommandName="A3" ToolTip="Agregar foto" OnClick="btnAsignarFoto_Click">
									<i class="fas fa-plus"></i>
								</asp:LinkButton>
								<asp:LinkButton ID="btnEditar3" runat="server" CssClass="btn6" Text=""  CausesValidation="false" CommandName="E3" ToolTip="Editar foto" OnClick="btnEditar_Click">
									<i class="fas fa-pencil-alt"></i>
								</asp:LinkButton>
								<asp:LinkButton ID="btnEliminar3" runat="server" CssClass="btn6" Text="" CausesValidation="false" CommandName="B3" ToolTip="Eliminar foto" OnClick="btnEliminarFoto_Click">
									<i class="far fa-trash-alt"></i>
								</asp:LinkButton>
							</div>
							<div class="panel-body panelFotoB">
								<asp:Image runat="server" ID="Image3" CssClass="imgVisita"/>
							</div>
							<asp:Label runat="server" ID="lblObs3" CssClass="lblDescFoto"></asp:Label>
						</div>

						<div class="panel panel-default panelFotosVisitas">
							<div class="panel-heading panelFotoH">
								<asp:Label runat="server" ID="lblTF4" CssClass="lblTipoFoto" Text=""></asp:Label>
								<asp:LinkButton ID="btnAsignar4" runat="server" CssClass="btn6" Text="" CausesValidation="false" CommandName="A4" ToolTip="Agregar foto" OnClick="btnAsignarFoto_Click">
									<i class="fas fa-plus"></i>
								</asp:LinkButton>
								<asp:LinkButton ID="btnEditar4" runat="server" CssClass="btn6" Text=""  CausesValidation="false" CommandName="E4" ToolTip="Editar foto" OnClick="btnEditar_Click">
									<i class="fas fa-pencil-alt"></i>
								</asp:LinkButton>
								<asp:LinkButton ID="btnEliminar4" runat="server" CssClass="btn6" Text="" CausesValidation="false" CommandName="B4" ToolTip="Eliminar foto" OnClick="btnEliminarFoto_Click">
									<i class="far fa-trash-alt"></i>
								</asp:LinkButton>
							</div>
							<div class="panel-body panelFotoB">
								<asp:Image runat="server" ID="Image4" CssClass="imgVisita"/>
							</div>
							<asp:Label runat="server" ID="lblObs4" CssClass="lblDescFoto"></asp:Label>
						</div>

						<div class="panel panel-default panelFotosVisitas">
							<div class="panel-heading panelFotoH">
								<asp:Label runat="server" ID="lblTF5" CssClass="lblTipoFoto" Text=""></asp:Label>
								<asp:LinkButton ID="btnAsignar5" runat="server" CssClass="btn6" Text="" CausesValidation="false" CommandName="A5" ToolTip="Agregar foto" OnClick="btnAsignarFoto_Click">
									<i class="fas fa-plus"></i>
								</asp:LinkButton>
								<asp:LinkButton ID="btnEditar5" runat="server" CssClass="btn6" Text=""  CausesValidation="false" CommandName="E5" ToolTip="Editar foto" OnClick="btnEditar_Click">
									<i class="fas fa-pencil-alt"></i>
								</asp:LinkButton>
								<asp:LinkButton ID="btnEliminar5" runat="server" CssClass="btn6" Text="" CausesValidation="false" CommandName="B5" ToolTip="Eliminar foto" OnClick="btnEliminarFoto_Click">
									<i class="far fa-trash-alt"></i>
								</asp:LinkButton>
							</div>
							<div class="panel-body panelFotoB">
								<asp:Image runat="server" ID="Image5" CssClass="imgVisita"/>
							</div>
							<asp:Label runat="server" ID="lblObs5" CssClass="lblDescFoto"></asp:Label>
						</div>

						<div class="panel panel-default panelFotosVisitas">
							<div class="panel-heading panelFotoH">
								<asp:Label runat="server" ID="lblTF6" CssClass="lblTipoFoto" Text=""></asp:Label>
								<asp:LinkButton ID="btnAsignar6" runat="server" CssClass="btn6" Text="" CausesValidation="false" CommandName="A6" ToolTip="Agregar foto" OnClick="btnAsignarFoto_Click">
									<i class="fas fa-plus"></i>
								</asp:LinkButton>
								<asp:LinkButton ID="btnEditar6" runat="server" CssClass="btn6" Text=""  CausesValidation="false" CommandName="E6" ToolTip="Editar foto" OnClick="btnEditar_Click">
									<i class="fas fa-pencil-alt"></i>
								</asp:LinkButton>
								<asp:LinkButton ID="btnEliminar6" runat="server" CssClass="btn6" Text="" CausesValidation="false" CommandName="B6" ToolTip="Eliminar foto" OnClick="btnEliminarFoto_Click">
									<i class="far fa-trash-alt"></i>
								</asp:LinkButton>
							</div>
							<div class="panel-body panelFotoB">
								<asp:Image runat="server" ID="Image6" CssClass="imgVisita"/>
							</div>
							<asp:Label runat="server" ID="lblObs6" CssClass="lblDescFoto"></asp:Label>
						</div>

						<div class="panel panel-default panelFotosVisitas">
							<div class="panel-heading panelFotoH">
								<asp:Label runat="server" ID="lblTF7" CssClass="lblTipoFoto" Text=""></asp:Label>
								<asp:LinkButton ID="btnAsignar7" runat="server" CssClass="btn6" Text="" CausesValidation="false" CommandName="A7" ToolTip="Agregar foto" OnClick="btnAsignarFoto_Click">
									<i class="fas fa-plus"></i>
								</asp:LinkButton>
								<asp:LinkButton ID="btnEditar7" runat="server" CssClass="btn6" Text=""  CausesValidation="false" CommandName="E7" ToolTip="Editar foto" OnClick="btnEditar_Click">
									<i class="fas fa-pencil-alt"></i>
								</asp:LinkButton>
								<asp:LinkButton ID="btnEliminar7" runat="server" CssClass="btn6" Text="" CausesValidation="false" CommandName="B7" ToolTip="Eliminar foto" OnClick="btnEliminarFoto_Click">
									<i class="far fa-trash-alt"></i>
								</asp:LinkButton>
							</div>
							<div class="panel-body panelFotoB">
								<asp:Image runat="server" ID="Image7" CssClass="imgVisita"/>
							</div>
							<asp:Label runat="server" ID="lblObs7" CssClass="lblDescFoto"></asp:Label>
						</div>

						<div class="panel panel-default panelFotosVisitas">
							<div class="panel-heading panelFotoH">
								<asp:Label runat="server" ID="lblTF8" CssClass="lblTipoFoto" Text=""></asp:Label>
								<asp:LinkButton ID="btnAsignar8" runat="server" CssClass="btn6" Text="" CausesValidation="false" CommandName="A8" ToolTip="Agregar foto" OnClick="btnAsignarFoto_Click">
									<i class="fas fa-plus"></i>
								</asp:LinkButton>
								<asp:LinkButton ID="btnEditar8" runat="server" CssClass="btn6" Text=""  CausesValidation="false" CommandName="E8" ToolTip="Editar foto" OnClick="btnEditar_Click">
									<i class="fas fa-pencil-alt"></i>
								</asp:LinkButton>
								<asp:LinkButton ID="btnEliminar8" runat="server" CssClass="btn6" Text="" CausesValidation="false" CommandName="B8" ToolTip="Eliminar foto" OnClick="btnEliminarFoto_Click">
									<i class="far fa-trash-alt"></i>
								</asp:LinkButton>
							</div>
							<div class="panel-body panelFotoB">
								<asp:Image runat="server" ID="Image8" CssClass="imgVisita"/>
							</div>
							<asp:Label runat="server" ID="lblObs8" CssClass="lblDescFoto"></asp:Label>
						</div>
					</div>

					<!--MODAL POPUP-->
					<asp:LinkButton ID="lbDummyReemplazar" runat="server"></asp:LinkButton>
					<ajaxToolkit:ModalPopupExtender ID="mpeReemplazar" runat="server" PopupControlID="pnlPopupReemplazar" TargetControlID="lbDummyReemplazar" CancelControlID="btnNoReemplazar" BackgroundCssClass="modalBackground" BehaviorID="puReemplazar">
					</ajaxToolkit:ModalPopupExtender>
					<asp:Panel ID="pnlPopupReemplazar" runat="server" CssClass="modalPopup" Style="display:none">
   						<div class="puHeader">Confirmar acción</div>
   						<div class="puBody">Ya existe una foto.	Desea reemplazarla?.       
       						<br /><br />
							<%--<asp:LinkButton ID="btnNoReemplazar" runat="server" CssClass="btn btn-outline-secondary btn-xs" OnClick="btnNoReemplazar_Click">
								<span aria-hidden="true" class="glyphicon glyphicon-remove">&nbsp; No</span>
							</asp:LinkButton>--%>
							<div class="btn btn-info glyphicon glyphicon-remove ml20">
								<asp:Button ID="btnNoReemplazar" runat="server" Text="Cancelar" CausesValidation="false" CssClass="btnYN" OnClick="btnNoReemplazar_Click" />
							</div>
							<%--<asp:LinkButton ID="btnSiReemplazar" runat="server" CssClass="btn btn-outline-primary btn-xs ml10" OnClick="btnSiReemplazar_Click">
								<span aria-hidden="true" class="glyphicon glyphicon-ok">&nbsp; Si</span>
							</asp:LinkButton>--%>
							<div class="btn btn-primary glyphicon glyphicon-ok ml20">
								<asp:Button ID="btnSiReemplazar" runat="server" Text="Aceptar" CausesValidation="false" CssClass="btnYN" OnClick="btnSiReemplazar_Click" />
							</div>
   						</div>
					</asp:Panel>

					<!--MODAL POPUP-->
					<asp:LinkButton ID="lbDummyBorrar" runat="server"></asp:LinkButton>
					<ajaxToolkit:ModalPopupExtender ID="mpeBorrar" runat="server" PopupControlID="pnlPopupBorrar" TargetControlID="lbDummyBorrar" CancelControlID="btnNoBorrar" BackgroundCssClass="modalBackground" BehaviorID="puBorrar">
					</ajaxToolkit:ModalPopupExtender>
					<asp:Panel ID="pnlPopupBorrar" runat="server" CssClass="modalPopup" Style="display:none">
   						<div class="puHeader">Confirmar acción</div>
   						<div class="puBody">Ya existe una foto. Desea eliminarla?.
       						<br /><br />
							<div class="btn btn-info glyphicon glyphicon-remove ml20">
								<asp:Button ID="btnNoBorrar" runat="server" Text="Cancelar" CausesValidation="false" CssClass="btnYN" OnClick="btnNoBorrar_Click" />
							</div>
							<div class="btn btn-primary glyphicon glyphicon-ok ml20">
								<asp:Button ID="Button1" runat="server" Text="Aceptar" CausesValidation="false" CssClass="btnYN" OnClick="btnSiBorrar_Click" />
							</div>
   						</div>
					</asp:Panel>
<%--					<asp:Panel ID="pnlPopupBorrar" runat="server" CssClass="modalPopup" Style="display:none">
   						<div class="puHeader">Confirmar acción</div>
	   						<div class="puBody">Ya existe una foto. Desea eliminarla?.
	       						<br /><br />
								<asp:LinkButton ID="btnNoBorrar" runat="server" CssClass="btn btn-info ml20" OnClick="btnNoBorrar_Click">
								    <span aria-hidden="true" class="glyphicon glyphicon-remove">&nbsp; No</span>
								</asp:LinkButton>
								<asp:LinkButton ID="btnSiBorrar" runat="server" CssClass="btn btn-primary ml20" OnClick="btnSiBorrar_Click">
									<span aria-hidden="true" class="glyphicon glyphicon-ok">&nbsp; Si</span>
								</asp:LinkButton>
	   						</div>
					</asp:Panel>--%>
				</asp:View>
			</asp:MultiView>
			<br />
	    </form>
	    <script>
		    function closeWindow() {
			    window.close();
		    }
	    </script>
    </body>
</html>