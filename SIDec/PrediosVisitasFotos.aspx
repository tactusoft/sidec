<%@ Page Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" CodeBehind="PrediosVisitasFotos.aspx.cs" Inherits="SIDec.PrediosVisitasFotos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="">
		<asp:MultiView runat="server" ID="mvFotos" ActiveViewIndex="1">
			<asp:View runat="server" ID="vDefault">
				<br />
				<br />
				<h3 style="color:red; margin:auto;"><span class="glyphicon glyphicon-alert ml100"></span>&nbsp;No puede acceder directamente a esta opcion.</h3>
			</asp:View>
			<asp:View runat="server" ID="vOpciones">
                <div class="divSPHeaderTitle">Predios / Visitas fotos</div>
                <br />
                <br />
                
                <div runat="server" id="divFotoEdicion" visible="false" class ="ml0 mtb10 d-i">   
					<asp:FileUpload ID="FileUpload1" CssClass="w230 fs12" runat="server" accept="image/*" TabIndex="1"  AllowMultiple="false"/>
					<label class="lbl2 w80 ml0">Tipo de foto</label>
					<asp:DropDownList runat="server" ID="ddlb_tipo_foto" CssClass="txt2 mt0 w150" TabIndex="1" AppendDataBoundItems="true">
						<asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
					</asp:DropDownList>
					<label class="lbl2a w80 vam">Descripción</label>
					<asp:TextBox runat="server" ID="txtDescripcionFoto" CssClass="txt2 w130 mr20" TabIndex="3" MaxLength="20"></asp:TextBox>
					<asp:LinkButton ID="lbCancelar" runat="server" CssClass="btn btn-outline-secondary btn-xs" OnClick="lbCancelar_Click" CausesValidation="false">
                        <i class="fas fa-times"></i>&nbspCancelar
					</asp:LinkButton>
					<asp:LinkButton ID="lbSubir" runat="server" CssClass="btn btn-outline-primary btn-xs ml10" TabIndex="5" Text="Aceptar" ValidationGroup="vgFotoVisita" OnClick="lbSubir_Click" CausesValidation="true">
                        <i class="fas fa-check"></i>&nbspAceptar
					</asp:LinkButton>
				</div>
                <br />

				<div runat="server" id="divFotoContainer">

					<div class="panel panel-default panelFotosVisitas2">
						<div class="panel-heading panelFotoH">
							<asp:Label runat="server" ID="lblTipoFoto1" CssClass="lblTipoFoto" Text=""></asp:Label>
							<asp:Label runat="server" ID="lblObs1" CssClass="lblDescFoto"></asp:Label>
							<asp:LinkButton ID="btnFotoDel1" runat="server" CssClass="btn6" CausesValidation="false" CommandName="D1" ToolTip="Eliminar foto" OnClick="btnFotoAccion_Click">
								<i class="far fa-trash-alt"></i>
							</asp:LinkButton>
							<asp:LinkButton ID="btnFotoEdit1" runat="server" CssClass="btn6" CausesValidation="false" CommandName="E1" ToolTip="Editar foto" OnClick="btnFotoAccion_Click">
								<i class="fas fa-pencil-alt"></i>
							</asp:LinkButton>
							<asp:LinkButton ID="btnFotoAdd1" runat="server" CssClass="btn6" CausesValidation="false" CommandName="A1" ToolTip="Agregar foto" OnClick="btnFotoAccion_Click">
								<i class="fas fa-plus"></i>
							</asp:LinkButton>
						</div>
						<div class="panel-body panelFotoB2">
							<asp:Image runat="server" ID="Image1" CssClass="imgVisita2"/>
						</div>
					</div>

					<div class="panel panel-default panelFotosVisitas2">
						<div class="panel-heading panelFotoH">
							<asp:Label runat="server" ID="lblTipoFoto2" CssClass="lblTipoFoto" Text=""></asp:Label>
							<asp:Label runat="server" ID="lblObs2" CssClass="lblDescFoto"></asp:Label>
							<asp:LinkButton ID="btnFotoDel2" runat="server" CssClass="btn6" CausesValidation="false" CommandName="D2" ToolTip="Eliminar foto" OnClick="btnFotoAccion_Click">
								<i class="far fa-trash-alt"></i>
							</asp:LinkButton>
							<asp:LinkButton ID="btnFotoEdit2" runat="server" CssClass="btn6" CausesValidation="false" CommandName="E2" ToolTip="Editar foto" OnClick="btnFotoAccion_Click">
								<i class="fas fa-pencil-alt"></i>
							</asp:LinkButton>
							<asp:LinkButton ID="btnFotoAdd2" runat="server" CssClass="btn6" CausesValidation="false" CommandName="A2" ToolTip="Agregar foto" OnClick="btnFotoAccion_Click">
								<i class="fas fa-plus"></i>
							</asp:LinkButton>
						</div>
						<div class="panel-body panelFotoB2">
							<asp:Image runat="server" ID="Image2" CssClass="imgVisita2"/>
						</div>
					</div>

					<div class="panel panel-default panelFotosVisitas2">
						<div class="panel-heading panelFotoH">
							<asp:Label runat="server" ID="lblTipoFoto3" CssClass="lblTipoFoto" Text=""></asp:Label>
							<asp:Label runat="server" ID="lblObs3" CssClass="lblDescFoto"></asp:Label>
							<asp:LinkButton ID="btnFotoDel3" runat="server" CssClass="btn6" CausesValidation="false" CommandName="D3" ToolTip="Eliminar foto" OnClick="btnFotoAccion_Click">
								<i class="far fa-trash-alt"></i>
							</asp:LinkButton>
							<asp:LinkButton ID="btnFotoEdit3" runat="server" CssClass="btn6" CausesValidation="false" CommandName="E3" ToolTip="Editar foto" OnClick="btnFotoAccion_Click">
								<i class="fas fa-pencil-alt"></i>
							</asp:LinkButton>
							<asp:LinkButton ID="btnFotoAdd3" runat="server" CssClass="btn6" CausesValidation="false" CommandName="A3" ToolTip="Agregar foto" OnClick="btnFotoAccion_Click">
								<i class="fas fa-plus"></i>
							</asp:LinkButton>
						</div>
						<div class="panel-body panelFotoB2">
							<asp:Image runat="server" ID="Image3" CssClass="imgVisita2"/>
						</div>
					</div>

					<div class="panel panel-default panelFotosVisitas2">
						<div class="panel-heading panelFotoH">
							<asp:Label runat="server" ID="lblTipoFoto4" CssClass="lblTipoFoto" Text=""></asp:Label>
							<asp:Label runat="server" ID="lblObs4" CssClass="lblDescFoto"></asp:Label>
							<asp:LinkButton ID="btnFotoDel4" runat="server" CssClass="btn6" CausesValidation="false" CommandName="D4" ToolTip="Eliminar foto" OnClick="btnFotoAccion_Click">
								<i class="far fa-trash-alt"></i>
							</asp:LinkButton>
							<asp:LinkButton ID="btnFotoEdit4" runat="server" CssClass="btn6" CausesValidation="false" CommandName="E4" ToolTip="Editar foto" OnClick="btnFotoAccion_Click">
								<i class="fas fa-pencil-alt"></i>
							</asp:LinkButton>
							<asp:LinkButton ID="btnFotoAdd4" runat="server" CssClass="btn6" CausesValidation="false" CommandName="A4" ToolTip="Agregar foto" OnClick="btnFotoAccion_Click">
								<i class="fas fa-plus"></i>
							</asp:LinkButton>
						</div>
						<div class="panel-body panelFotoB2">
							<asp:Image runat="server" ID="Image4" CssClass="imgVisita2"/>
						</div>
					</div>

					<div class="panel panel-default panelFotosVisitas2">
						<div class="panel-heading panelFotoH">
							<asp:Label runat="server" ID="lblTipoFoto5" CssClass="lblTipoFoto" Text=""></asp:Label>
							<asp:Label runat="server" ID="lblObs5" CssClass="lblDescFoto"></asp:Label>
							<asp:LinkButton ID="btnFotoDel5" runat="server" CssClass="btn6" CausesValidation="false" CommandName="D5" ToolTip="Eliminar foto" OnClick="btnFotoAccion_Click">
								<i class="far fa-trash-alt"></i>
							</asp:LinkButton>
							<asp:LinkButton ID="btnFotoEdit5" runat="server" CssClass="btn6" CausesValidation="false" CommandName="E5" ToolTip="Editar foto" OnClick="btnFotoAccion_Click">
								<i class="fas fa-pencil-alt"></i>
							</asp:LinkButton>
							<asp:LinkButton ID="btnFotoAdd5" runat="server" CssClass="btn6" CausesValidation="false" CommandName="A5" ToolTip="Agregar foto" OnClick="btnFotoAccion_Click">
								<i class="fas fa-plus"></i>
							</asp:LinkButton>
						</div>
						<div class="panel-body panelFotoB2">
							<asp:Image runat="server" ID="Image5" CssClass="imgVisita2"/>
						</div>
					</div>

					<div class="panel panel-default panelFotosVisitas2">
						<div class="panel-heading panelFotoH">
							<asp:Label runat="server" ID="lblTipoFoto6" CssClass="lblTipoFoto" Text=""></asp:Label>
							<asp:Label runat="server" ID="lblObs6" CssClass="lblDescFoto"></asp:Label>
							<asp:LinkButton ID="btnFotoDel6" runat="server" CssClass="btn6" CausesValidation="false" CommandName="D6" ToolTip="Eliminar foto" OnClick="btnFotoAccion_Click">
								<i class="far fa-trash-alt"></i>
							</asp:LinkButton>
							<asp:LinkButton ID="btnFotoEdit6" runat="server" CssClass="btn6" CausesValidation="false" CommandName="E6" ToolTip="Editar foto" OnClick="btnFotoAccion_Click">
								<i class="fas fa-pencil-alt"></i>
							</asp:LinkButton>
							<asp:LinkButton ID="btnFotoAdd6" runat="server" CssClass="btn6" CausesValidation="false" CommandName="A6" ToolTip="Agregar foto" OnClick="btnFotoAccion_Click">
								<i class="fas fa-plus"></i>
							</asp:LinkButton>
						</div>
						<div class="panel-body panelFotoB2">
							<asp:Image runat="server" ID="Image6" CssClass="imgVisita2"/>
						</div>
					</div>

					<div class="panel panel-default panelFotosVisitas2">
						<div class="panel-heading panelFotoH">
							<asp:Label runat="server" ID="lblTipoFoto7" CssClass="lblTipoFoto" Text=""></asp:Label>
							<asp:Label runat="server" ID="lblObs7" CssClass="lblDescFoto"></asp:Label>
							<asp:LinkButton ID="btnFotoDel7" runat="server" CssClass="btn6" CausesValidation="false" CommandName="D7" ToolTip="Eliminar foto" OnClick="btnFotoAccion_Click">
								<i class="far fa-trash-alt"></i>
							</asp:LinkButton>
							<asp:LinkButton ID="btnFotoEdit7" runat="server" CssClass="btn6" CausesValidation="false" CommandName="E7" ToolTip="Editar foto" OnClick="btnFotoAccion_Click">
								<i class="fas fa-pencil-alt"></i>
							</asp:LinkButton>
							<asp:LinkButton ID="btnFotoAdd7" runat="server" CssClass="btn6" CausesValidation="false" CommandName="A7" ToolTip="Agregar foto" OnClick="btnFotoAccion_Click">
								<i class="fas fa-plus"></i>
							</asp:LinkButton>
						</div>
						<div class="panel-body panelFotoB2">
							<asp:Image runat="server" ID="Image7" CssClass="imgVisita2"/>
						</div>
					</div>

					<div class="panel panel-default panelFotosVisitas2">
						<div class="panel-heading panelFotoH">
							<asp:Label runat="server" ID="lblTipoFoto8" CssClass="lblTipoFoto" Text=""></asp:Label>
							<asp:Label runat="server" ID="lblObs8" CssClass="lblDescFoto"></asp:Label>
							<asp:LinkButton ID="btnFotoDel8" runat="server" CssClass="btn6" CausesValidation="false" CommandName="D8" ToolTip="Eliminar foto" OnClick="btnFotoAccion_Click">
								<i class="far fa-trash-alt"></i>
							</asp:LinkButton>
							<asp:LinkButton ID="btnFotoEdit8" runat="server" CssClass="btn6" CausesValidation="false" CommandName="E8" ToolTip="Editar foto" OnClick="btnFotoAccion_Click">
								<i class="fas fa-pencil-alt"></i>
							</asp:LinkButton>
							<asp:LinkButton ID="btnFotoAdd8" runat="server" CssClass="btn6" CausesValidation="false" CommandName="A8" ToolTip="Agregar foto" OnClick="btnFotoAccion_Click">
								<i class="fas fa-plus"></i>
							</asp:LinkButton>
						</div>
						<div class="panel-body panelFotoB2">
							<asp:Image runat="server" ID="Image8" CssClass="imgVisita2"/>
						</div>
					</div>
				</div>

                <!--MODAL POPUP-->
				<asp:LinkButton ID="lbDummyEliminar" runat="server"></asp:LinkButton>
				<ajaxToolkit:ModalPopupExtender ID="mpeEliminar" runat="server" PopupControlID="pnlPopupEliminar" TargetControlID="lbDummyEliminar" CancelControlID="btnNoEliminar" BackgroundCssClass="modalBackground" BehaviorID="puEliminar"></ajaxToolkit:ModalPopupExtender>
				<asp:Panel ID="pnlPopupEliminar" runat="server" CssClass="modalPopup" Style="display: none">
					<div class="puHeader">Confirmar acción</div>
					<div class="puBody">
                        <br />
                        <p>¿Desea eliminar la foto?</p>
                        <asp:LinkButton ID="btnNoEliminar" runat="server" CssClass="btn btn-outline-secondary btn-sm" CausesValidation="false">
                            <i class="fas fa-times"></i>&nbsp&nbspCancelar
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnSiEliminar" runat="server" CssClass="btn btn-outline-primary btn-sm ml10" CausesValidation="false" OnClick="btnSiEliminar_Click">
                            <i class="fas fa-check"></i>&nbsp&nbspAceptar
                        </asp:LinkButton>
					</div>
				</asp:Panel>
			</asp:View>
		</asp:MultiView>
		<br />
	</div>
	<script>
		function closeWindow() {
			window.close();
        }
	</script>
</asp:Content>