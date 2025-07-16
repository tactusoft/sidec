<%@ Page Title="" Language="C#" MasterPageFile="~/Authentic.Master" AutoEventWireup="true" CodeBehind="Identidades.aspx.cs" Inherits="SIDec.Identidades" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="divContent">
	    <div class="divBuscar">
	        <asp:TextBox runat="server" ID="txtBuscar" CssClass="txtBuscar" TextMode="Search" ClientIDMode="Static"/>
	        <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="btn-sm btn-primary w100 mr5" CausesValidation="false" OnClick="btnBuscar_Click" />
	        <input type="button" onclick="Limpiar();" class="btn-sm btn-secondary w100" value="Limpiar" />

	        <%--Valores para scroll de los GV--%>
	        <asp:HiddenField ID="hfGVIdentidadesCatSV" runat="server"/> 
	        <asp:HiddenField ID="hfGVIdentidadesCatSH" runat="server"/>
	        <asp:HiddenField ID="hfGVIdentidadesSV" runat="server"/> 
	        <asp:HiddenField ID="hfGVIdentidadesSH" runat="server"/>
	    </div>

	    <%--********************************************************************************************************************************************************************************--%>
	    <%--*****************************************************************	 SECCION CATEGORIAS IDENTIDAD	 *****************************************************************--%>
	    <%--********************************************************************************************************************************************************************************--%>
	    <asp:UpdatePanel runat="server" ID="up1" ChildrenAsTriggers="true" UpdateMode="Conditional">
	        <ContentTemplate>
		        <div class="divSP" onclick="HideMensajeCRUD();">
		            <div class="divSPHeader">
			            <asp:UpdatePanel runat="server" ID="upIdentidadesCatBtnVistas" UpdateMode="Conditional">
			                <ContentTemplate>
			                    <div class="divSPHeaderTitle">Categorias Identidades</div>
			                    <div class="divSPHeaderButton">
				                <asp:LinkButton ID="btnIdentidadesCatVG" runat="server" CssClass="btn7" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnIdentidadesCatVista_Click">
				                    <i class="fas fa-th"></i>
				                </asp:LinkButton>
				                <asp:LinkButton ID="btnIdentidadesCatVD" runat="server" CssClass="btn7" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnIdentidadesCatVista_Click">
				                    <i class="fas fa-bars"></i>
				                </asp:LinkButton>
			                    </div>
			                </ContentTemplate>
		                </asp:UpdatePanel>
		            </div>

		            <div class="">
			            <asp:UpdatePanel runat="server" ID="upIdentidadesCat" UpdateMode="Conditional">
			                <ContentTemplate>
				                <div class="divSPGV">
				                    <asp:MultiView runat="server" ID="mvIdentidadesCat" ActiveViewIndex="0" OnActiveViewChanged="mvIdentidadesCat_ActiveViewChanged">
					                    <asp:View runat="server" ID="vIdentidadesCatGrid">
					                        <asp:GridView id="gvIdentidadesCat" CssClass="gv" runat="server" DataKeyNames="id_categoria_identidad" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="50" AllowSorting="true"
								                OnSelectedIndexChanged="gvIdentidadesCat_SelectedIndexChanged" OnSorting="gvIdentidadesCat_Sorting" OnPageIndexChanging="gvIdentidadesCat_PageIndexChanging"
								                OnRowCreated="gvIdentidadesCat_RowCreated" OnDataBinding="gvIdentidadesCat_DataBinding" OnRowDataBound="gvIdentidadesCat_RowDataBound">
						                    <Columns>
						                        <asp:BoundField DataField="id_categoria_identidad" HeaderText="Id Categoria" SortExpression="id_categoria_identidad"/>
						                        <asp:BoundField DataField="categoria_identidad" HeaderText="Categoria" SortExpression="categoria_identidad"/>
						                        <asp:BoundField DataField="descripcion_categoria_identidad" HeaderText="Descripción"/>
						                    </Columns>
						                    <SelectedRowStyle CssClass="gvItemSelected"/>
						                    <HeaderStyle CssClass="gvHeader"/> 
   						                    <RowStyle CssClass="gvItem"/>
   						                    <PagerStyle CssClass="gvPager"/>
					                        </asp:GridView>
					                    </asp:View>

					                    <asp:View runat="server" ID="vIdentidadesCatDetalle">
					                        <asp:ValidationSummary runat="server" ID="vsIdentidadesCat" DisplayMode="SingleParagraph" HeaderText="Falta informar: " ShowMessageBox="false" ShowSummary="true" ValidationGroup="vgIdentidadesCat" EnableClientScript="false"/>
					                        <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Categoria} " Display="Dynamic" ValidationGroup="vgIdentidadesCat" ControlToValidate="txtCategoria" />
					                        <div class="w_96a pt20">
						                        <label class="lbl2 w100">Id Categoria</label>
						                        <asp:TextBox runat="server" ID="txtIdCategoria" CssClass="txt2 txtDis w50" MaxLength="50" Enabled="false"></asp:TextBox>
						                        <label class="lbl2 w100">Categoria</label>
						                        <asp:TextBox runat="server" ID="txtCategoria" CssClass="txt2 w300" MaxLength="50" TabIndex="1"></asp:TextBox>
						                        <br />

						                        <label class="lbl2 w100">Descripción</label>
						                        <asp:TextBox runat="server" ID="txtDescripcionCat" CssClass="txt2 w700" MaxLength="200" TabIndex="2"></asp:TextBox>
					                        </div>
					                    </asp:View>
				                    </asp:MultiView>
				                </div>
			                </ContentTemplate>
			                <Triggers>
				                <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click"/>
				                <asp:AsyncPostBackTrigger ControlID="btnIdentidadesCatVG" EventName="Click"/>
				                <asp:AsyncPostBackTrigger ControlID="btnIdentidadesCatVD" EventName="Click"/>
				                <asp:AsyncPostBackTrigger ControlID="btnFirstIdentidadesCat" EventName="Click"/>
				                <asp:AsyncPostBackTrigger ControlID="btnBackIdentidadesCat" EventName="Click"/>
				                <asp:AsyncPostBackTrigger ControlID="btnNextIdentidadesCat" EventName="Click"/>
				                <asp:AsyncPostBackTrigger ControlID="btnLastIdentidadesCat" EventName="Click"/>
				                <asp:AsyncPostBackTrigger ControlID="btnIdentidadesCatEdit" EventName="Click"/>
				                <asp:AsyncPostBackTrigger ControlID="btnIdentidadesCatAdd" EventName="Click"/>
				                <asp:AsyncPostBackTrigger ControlID="gvIdentidadesCat" EventName="SelectedIndexChanged"/>
			                </Triggers>
			            </asp:UpdatePanel>
		            </div>

		            <asp:UpdatePanel runat="server" ID="upIdentidadesCatFoot" UpdateMode="Conditional">
			            <ContentTemplate>
			                <asp:HiddenField runat="server" ID="hfEvtGVIdentidadesCat" Value=""/>
			                <div class="divSPMessage" id="DivMsgIdentidadesCat"></div>

			                <div class="divSPFooter">
				                <asp:Panel class="divSPFooter1" ID="divIdentidadesCatNavegacion" runat="server" >
				                    <asp:LinkButton ID="btnFirstIdentidadesCat" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnIdentidadesCatNavegacion_Click">
					                    <i class="fas fa-angle-double-left"></i>
				                    </asp:LinkButton>
				                    <asp:LinkButton ID="btnBackIdentidadesCat" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnIdentidadesCatNavegacion_Click">
					                    <i class="fas fa-angle-left"></i>
				                    </asp:LinkButton>
				                    <asp:LinkButton ID="btnNextIdentidadesCat" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnIdentidadesCatNavegacion_Click">
					                    <i class="fas fa-angle-right"></i>
				                    </asp:LinkButton>
				                    <asp:LinkButton ID="btnLastIdentidadesCat" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnIdentidadesCatNavegacion_Click">
					                    <i class="fas fa-angle-double-right"></i>
				                    </asp:LinkButton>
				                </asp:Panel>

				                <div class="divSPFooter2">
				                    <asp:Label runat="server" ID="lblIdentidadesCatCuenta"></asp:Label>
				                </div>

				                <div class="divSPFooter3">
				                    <asp:LinkButton ID="btnIdentidadesCatCancelar" runat="server" CssClass="btn btn-outline-secondary btn-xs" CommandArgument="0" OnClick="btnIdentidadesCatCancelar_Click" CausesValidation="false">
					                    <i class="fas fa-times"></i>&nbspCancelar
				                    </asp:LinkButton>
				                    <asp:LinkButton ID="btnIdentidadesCatAccionFinal" runat="server" CssClass="btn btn-outline-primary btn-xs ml10" ValidationGroup="vgIdentidadesCat" OnClientClick="if(Page_ClientValidate('vgIdentidadesCat') && HayCambiosIdentidadesCat()) return ShowModalPopup('puIdentidadesCat');">
					                    <i class="fas fa-check"></i>&nbspAceptar
				                    </asp:LinkButton>
				                </div>

				                <div class="divSPFooter4" id="divIdentidadesCatAction">
				                    <asp:LinkButton ID="btnIdentidadesCatAdd" runat="server" CssClass="btn4" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnIdentidadesCatAccion_Click">
					                    <i class="fas fa-plus"></i>
				                    </asp:LinkButton>
				                    <asp:LinkButton ID="btnIdentidadesCatEdit" runat="server" CssClass="btn4" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnIdentidadesCatAccion_Click">
					                    <i class="fas fa-pencil-alt"></i>
				                    </asp:LinkButton>
				                    <%-- <asp:LinkButton ID="btnIdentidadesCatDel" runat="server" CssClass="btn4" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnIdentidadesCatAccion_Click" OnClientClick="LimpiarMensajeWeb();">
					                    <i class="far fa-trash-alt"></i>
				                    </asp:LinkButton>--%>
				                </div>
			                </div>

			                <!--MODAL POPUP-->
			                <asp:LinkButton ID="lbDummyIdentidadesCat" runat="server"></asp:LinkButton>
			                <ajaxToolkit:ModalPopupExtender ID="mpeIdentidadesCat" runat="server" PopupControlID="pnlPopupIdentidadesCat" TargetControlID="lbDummyIdentidadesCat" CancelControlID="btnHideIdentidadesCat" BackgroundCssClass="modalBackground" BehaviorID="puIdentidadesCat">
			                </ajaxToolkit:ModalPopupExtender>
			                <asp:Panel ID="pnlPopupIdentidadesCat" runat="server" CssClass="modalPopup" Style="display: none">
   				                <div class="puHeader">Confirmar acción</div>
   				                <div class="puBody">
       				                ¿Está seguro de continuar con la acción solicitada?.
       				                <br /><br />
					                <div class="btn btn-info glyphicon glyphicon-remove ml20" onclick="divClickAction('<%=btnHideIdentidadesCat.ClientID%>');">
					                    <asp:Button ID="btnHideIdentidadesCat" runat="server" Text="Cancelar" CssClass="btnYN" CausesValidation="false" OnClientClick="return HideModalPopup('puIdentidadesCat');" />
					                </div>
				                    <div class="btn btn-primary glyphicon glyphicon-ok ml20" onclick="divClickAction('<%=btnConfirmarIdentidadesCat.ClientID%>');">
					                    <asp:Button ID="btnConfirmarIdentidadesCat" runat="server" Text="Aceptar" CssClass="btnYN" OnClick="btnConfirmarIdentidadesCat_Click" />
					                </div>
   				                </div>
			                </asp:Panel>
			            </ContentTemplate>
			            <Triggers>
			                <asp:AsyncPostBackTrigger ControlID="btnIdentidadesCatEdit" EventName="Click" />
			                <asp:AsyncPostBackTrigger ControlID="btnIdentidadesCatAdd" EventName="Click" />
			            </Triggers>
		            </asp:UpdatePanel>
		        </div>
	        </ContentTemplate>
	    </asp:UpdatePanel>
	    <div class="divS h10"></div>

	    <%--********************************************************************************************************************************************************************************--%>
	    <%--**********************************************************************	  SECCION IDENTIDAD	   ************************************************************************--%>
	    <%--********************************************************************************************************************************************************************************--%>
	    <asp:UpdatePanel runat="server" ID="up2" ChildrenAsTriggers="true" UpdateMode="Conditional">
	        <ContentTemplate>
		        <div class="divSP" onclick="HideMensajeCRUD();">
		            <div class="divSPHeader">
			            <asp:UpdatePanel runat="server" ID="upIdentidadesBtnVistas" UpdateMode="Conditional">
			                <ContentTemplate>
				                <div class="divSPHeaderTitle">Identidades</div>
				                <div class="divSPHeaderButton">
				                    <asp:LinkButton ID="btnIdentidadesVG" runat="server" CssClass="btn7" CommandArgument="0" CausesValidation="false" ToolTip="Vista tabla" OnClick="btnIdentidadesVista_Click">
					                    <i class="fas fa-th"></i>
				                    </asp:LinkButton>
				                    <asp:LinkButton ID="btnIdentidadesVD" runat="server" CssClass="btn7" CommandArgument="1" CausesValidation="false" ToolTip="Vista detalle" OnClick="btnIdentidadesVista_Click">
					                    <i class="fas fa-bars"></i>
				                    </asp:LinkButton>
				                </div>
			                </ContentTemplate>
			            </asp:UpdatePanel>
		            </div>

                    <div class="">
                        <asp:UpdatePanel runat="server" ID="upIdentidades" UpdateMode="Conditional">
	                        <ContentTemplate>
	                            <div class="divSPGV">
		                            <asp:MultiView runat="server" ID="mvIdentidades" ActiveViewIndex="0" OnActiveViewChanged="mvIdentidades_ActiveViewChanged">
		                                <asp:View runat="server" ID="vIdentidadesGrid">
			                                <asp:GridView id="gvIdentidades" CssClass="gv" runat="server" DataKeyNames="id_identidad" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="50" AllowSorting="true"
					                            OnSelectedIndexChanged="gvIdentidades_SelectedIndexChanged" OnSorting="gvIdentidades_Sorting" OnPageIndexChanging="gvIdentidades_PageIndexChanging"
					                            OnRowCreated="gvIdentidades_RowCreated" OnDataBinding="gvIdentidades_DataBinding" OnRowDataBound="gvIdentidades_RowDataBound">
			                                    <Columns>
				                                    <asp:BoundField DataField="id_categoria_identidad" HeaderText="Categoria"/>
				                                    <asp:BoundField DataField="nombre_identidad" HeaderText="Identidad" SortExpression="nombre_identidad"/>
				                                    <asp:BoundField DataField="descripcion_identidad" HeaderText="Descripción"/>
			                                    </Columns>
			                                    <SelectedRowStyle CssClass="gvItemSelected"/>
			                                    <HeaderStyle CssClass="gvHeader"/> 
   				                                <RowStyle CssClass="gvItem"/>
   				                                <PagerStyle CssClass="gvPager"/>
			                                </asp:GridView>
		                                </asp:View>

		                                <asp:View runat="server" ID="vIdentidadesDetalle">
			                                <asp:ValidationSummary runat="server" ID="vsIdentidades" DisplayMode="SingleParagraph" HeaderText="Falta informar: " ShowMessageBox="false" ShowSummary="true" ValidationGroup="vgIdentidades" EnableClientScript="false"/>
			                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Identidad} " Display="Dynamic" ValidationGroup="vgIdentidades" ControlToValidate="txtIdentidad" />
			                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Descripción} " Display="Dynamic" ValidationGroup="vgIdentidades" ControlToValidate="txtDescripcionIdentidad" />
			                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Orden Identidad} " Display="Dynamic" ValidationGroup="vgIdentidades" ControlToValidate="txtOrdenIdentidad" />
			                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" CssClass="valg" ErrorMessage=" {Categoria} " Display="Dynamic" ValidationGroup="vgIdentidades" ControlToValidate="ddlbCategoria" />

			                                <div class="w_96a pt20">
				                                <label class="lbl2 lblDis">Id Identidad</label>
				                                <asp:TextBox runat="server" ID="txtIdIdentidad" CssClass="txt2 txtDis w50" Enabled="false"></asp:TextBox>
				                                <label class="lbl2 lblDis">Categoria</label>
				                                <asp:DropDownList runat="server" ID="ddlbCategoria" CssClass="txt2 txtDis w350" AppendDataBoundItems="true" TabIndex="1">
				                                    <asp:ListItem Value="">-- Seleccione opción</asp:ListItem>
				                                </asp:DropDownList>
			                                    <br />

			                                    <label class="lbl2">Identidad</label>
				                                <asp:TextBox runat="server" ID="txtIdentidad" CssClass="txt2 w500" MaxLength="50" TabIndex="2"></asp:TextBox>
			                                    <label class="lbl2 w110">Orden Identidad</label>
				                                <asp:TextBox runat="server" ID="txtOrdenIdentidad" CssClass="txt2 w30" MaxLength="3" TabIndex="3" onkeypress="return SoloEntero(event);"></asp:TextBox>
			                                    <asp:CheckBox runat="server" ID="chkbHabilitado" Text="Habilitado" TabIndex="4" />
			                                    <br />
			                                    <label class="lbl2">Descripción</label>
				                                <asp:TextBox runat="server" ID="txtDescripcionIdentidad" CssClass="txt2 w900" MaxLength="50" TabIndex="5"></asp:TextBox>
			                                    <br />

				                                <label class="lbl2 w130">Nombre Identidad 2</label>
				                                <asp:TextBox runat="server" ID="txtNombreIdentidad2" CssClass="txt2 w900" MaxLength="200" TabIndex="6"></asp:TextBox>
			                                </div>
		                                </asp:View>
		                            </asp:MultiView>
	                            </div>
	                        </ContentTemplate>
	                        <Triggers>
	                            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click"/>
	                            <asp:AsyncPostBackTrigger ControlID="btnIdentidadesVG" EventName="Click"/>
	                            <asp:AsyncPostBackTrigger ControlID="btnIdentidadesVD" EventName="Click"/>
	                            <asp:AsyncPostBackTrigger ControlID="btnFirstIdentidades" EventName="Click"/>
	                            <asp:AsyncPostBackTrigger ControlID="btnBackIdentidades" EventName="Click"/>
	                            <asp:AsyncPostBackTrigger ControlID="btnNextIdentidades" EventName="Click"/>
	                            <asp:AsyncPostBackTrigger ControlID="btnLastIdentidades" EventName="Click"/>
	                            <asp:AsyncPostBackTrigger ControlID="btnIdentidadesEdit" EventName="Click"/>
	                            <asp:AsyncPostBackTrigger ControlID="btnIdentidadesAdd" EventName="Click"/>
	                            <asp:AsyncPostBackTrigger ControlID="btnIdentidadesDel" EventName="Click"/>
	                            <asp:AsyncPostBackTrigger ControlID="gvIdentidades" EventName="SelectedIndexChanged"/>
	                        </Triggers>
                        </asp:UpdatePanel>
                    </div>

		            <asp:UpdatePanel runat="server" ID="upIdentidadesFoot" UpdateMode="Conditional">
			            <ContentTemplate>
			                <asp:HiddenField runat="server" ID="hfEvtGVIdentidades" Value=""/>
			                <div class="divSPMessage" id="DivMsgIdentidades"></div>

			                <div class="divSPFooter">
                                <asp:Panel class="divSPFooter1" ID="divIdentidadesNavegacion" runat="server" >
	                                <asp:LinkButton ID="btnFirstIdentidades" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="First" ToolTip="Ir al primer registro" OnClick="btnIdentidadesNavegacion_Click">
	                                    <i class="fas fa-angle-double-left"></i>
	                                </asp:LinkButton>
	                                <asp:LinkButton ID="btnBackIdentidades" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Back" ToolTip="Registro anterior" OnClick="btnIdentidadesNavegacion_Click">
	                                    <i class="fas fa-angle-left"></i>
	                                </asp:LinkButton>
	                                <asp:LinkButton ID="btnNextIdentidades" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Next" ToolTip="Registro siguiente" OnClick="btnIdentidadesNavegacion_Click">
	                                    <i class="fas fa-angle-right"></i>
	                                </asp:LinkButton>
	                                <asp:LinkButton ID="btnLastIdentidades" runat="server" CssClass="btn5" Text="" CausesValidation="false" CommandName="Last" ToolTip="Ir al último registro" OnClick="btnIdentidadesNavegacion_Click">
	                                    <i class="fas fa-angle-double-right"></i>
	                                </asp:LinkButton>
                                </asp:Panel>

                                <div class="divSPFooter2">
	                                <asp:Label runat="server" ID="lblIdentidadesCuenta"></asp:Label>
                                </div>

                                <div class="divSPFooter3">
	                                <asp:LinkButton ID="btnIdentidadesCancelar" runat="server" CssClass="btn btn-outline-secondary btn-xs" CommandArgument="0" OnClick="btnIdentidadesCancelar_Click" CausesValidation="false">
	                                    <i class="fas fa-times"></i>&nbspCancelar
	                                </asp:LinkButton>
	                                <asp:LinkButton ID="btnIdentidadesAccionFinal" runat="server" CssClass="btn btn-outline-primary btn-xs ml10" ValidationGroup="vgIdentidades" OnClientClick="if(Page_ClientValidate('vgIdentidades') && HayCambiosIdentidades()) return ShowModalPopup('puIdentidades');">
	                                    <i class="fas fa-check"></i>&nbspAceptar
	                                </asp:LinkButton>
                                </div>

                                <div class="divSPFooter4" id="divIdentidadesAction">
	                                <asp:LinkButton ID="btnIdentidadesAdd" runat="server" CssClass="btn4" Text="Agregar" CausesValidation="false" CommandName="Agregar" CommandArgument="2" ToolTip="Agregar registro" OnClick="btnIdentidadesAccion_Click" >
	                                    <i class="fas fa-plus"></i>
	                                </asp:LinkButton>
	                                <asp:LinkButton ID="btnIdentidadesEdit" runat="server" CssClass="btn4" Text="Editar" CausesValidation="false" CommandName="Editar" CommandArgument="1" ToolTip="Editar registro" OnClick="btnIdentidadesAccion_Click" >
	                                    <i class="fas fa-pencil-alt"></i>
	                                </asp:LinkButton>
	                                <asp:LinkButton ID="btnIdentidadesDel" runat="server" CssClass="btn4" Text="Eliminar" CausesValidation="false" CommandName="Eliminar" CommandArgument="3" ToolTip="Eliminar registro" OnClick="btnIdentidadesAccion_Click" OnClientClick="LimpiarMensajeWeb();">
	                                    <i class="far fa-trash-alt"></i>
	                                </asp:LinkButton>
                                </div>
                            </div>

                            <!--MODAL POPUP-->
                            <asp:LinkButton ID="lbDummyIdentidades" runat="server"></asp:LinkButton>
                            <ajaxToolkit:ModalPopupExtender ID="mpeIdentidades" runat="server" PopupControlID="pnlPopupIdentidades" TargetControlID="lbDummyIdentidades" CancelControlID="btnHideIdentidades" BackgroundCssClass="modalBackground" BehaviorID="puIdentidades">
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Panel ID="pnlPopupIdentidades" runat="server" CssClass="modalPopup" Style="display: none">
                                <div class="puHeader">Confirmar acción</div>
                                <div class="puBody">
                                    ¿Está seguro de continuar con la acción solicitada?.
                                    <br /><br />
                                    <div class="btn btn-info glyphicon glyphicon-remove ml20" onclick="divClickAction('<%=btnHideIdentidades.ClientID%>');">
					                    <asp:Button ID="btnHideIdentidades" runat="server" Text="Cancelar" CssClass="btnYN" CausesValidation="false" OnClientClick="return HideModalPopup('puIdentidades');" />
					                </div>
				                    <div class="btn btn-primary glyphicon glyphicon-ok ml20" onclick="divClickAction('<%=btnConfirmarIdentidades.ClientID%>');">
					                    <asp:Button ID="btnConfirmarIdentidades" runat="server" Text="Aceptar" CssClass="btnYN" OnClick="btnConfirmarIdentidades_Click" />
					                </div>
   				                </div>
			                </asp:Panel>
			            </ContentTemplate>
			            <Triggers>
			                <asp:AsyncPostBackTrigger ControlID="btnIdentidadesEdit" EventName="Click" />
			                <asp:AsyncPostBackTrigger ControlID="btnIdentidadesAdd" EventName="Click" />
			                <asp:AsyncPostBackTrigger ControlID="btnIdentidadesDel" EventName="Click" />
			            </Triggers>
		            </asp:UpdatePanel>
		        </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script>
        function pageLoad() {
            //*****************************************LECTURA PARAMETROS CONFIGURACION
            var Ver = '<%=((NameValueCollection)ConfigurationManager.GetSection("PARAMETRIZACION/BotonesAccion"))["IdentidadesCategoria"].ToString()%>';
            if (Ver == "False") {
                if (document.getElementById('divIdentidadesCatAction') != null)
                    document.getElementById('divIdentidadesCatAction').style.display = 'none';
            }


            //*****************************************ESTILO GRIDVIEWS
            $('#<%=gvIdentidadesCat.ClientID%>').gridviewScroll({
                width: 700,
                height: 150,
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

                startVertical: $("#<%=hfGVIdentidadesCatSV.ClientID%>").val(),
                startHorizontal: $("#<%=hfGVIdentidadesCatSH.ClientID%>").val(),
                onScrollVertical: function (delta) {
                    $("#<%=hfGVIdentidadesCatSV.ClientID%>").val(delta);
                },
                onScrollHorizontal: function (delta) {
                    $("#<%=hfGVIdentidadesCatSH.ClientID%>").val(delta);
                }
            });

            $('#<%=gvIdentidades.ClientID%>').gridviewScroll({
                width: 700,
                height: 300,
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

                startVertical: $("#<%=hfGVIdentidadesSV.ClientID%>").val(),
                startHorizontal: $("#<%=hfGVIdentidadesSH.ClientID%>").val(),
                onScrollVertical: function (delta) {
                    $("#<%=hfGVIdentidadesSV.ClientID%>").val(delta);
                },
                onScrollHorizontal: function (delta) {
                    $("#<%=hfGVIdentidadesSH.ClientID%>").val(delta);
                }
            });
        }

        //*****************************************ALMACENAR VALORES INICIALES DE DDLB
        var vIdCategoria;
        var vHabilitado;

        function defaultddlbIdentidades() {
            vIdCategoria = document.getElementById('<%=ddlbCategoria.ClientID%>').value;
            vHabilitado = document.getElementById('<%=chkbHabilitado.ClientID%>').checked;
        }

        //*****************************************VERIFICAR CAMBIOS DE DATOS EN PAGINA
        function HayCambiosIdentidadesCat() {
            if (document.getElementById('<%=txtCategoria.ClientID%>').defaultValue != document.getElementById('<%=txtCategoria.ClientID%>').value)
                return true;
            else if (document.getElementById('<%=txtDescripcionCat.ClientID%>').defaultValue != document.getElementById('<%=txtDescripcionCat.ClientID%>').value)
                return true;
            else
                MensajeCRUD("No se han realizado cambios, no se requiere actualización.", 2, 'DivMsgIdentidades')
            return false;
        }

        function HayCambiosIdentidades() {
            if (document.getElementById('<%=hfEvtGVIdentidades.ClientID%>').value != "1")
                return true;
            if (document.getElementById('<%=txtIdentidad.ClientID%>').defaultValue != document.getElementById('<%=txtIdentidad.ClientID%>').value)
                return true;
            else if (document.getElementById('<%=txtDescripcionIdentidad.ClientID%>').defaultValue != document.getElementById('<%=txtDescripcionIdentidad.ClientID%>').value)
                return true;
            else if (vIdCategoria != document.getElementById('<%=ddlbCategoria.ClientID%>').value)
                return true;
            else if (document.getElementById('<%=txtOrdenIdentidad.ClientID%>').defaultValue != document.getElementById('<%=txtOrdenIdentidad.ClientID%>').value)
                return true;
            else if (document.getElementById('<%=txtNombreIdentidad2.ClientID%>').defaultValue != document.getElementById('<%=txtNombreIdentidad2.ClientID%>').value)
                return true;
            else if (vHabilitado != document.getElementById('<%=chkbHabilitado.ClientID%>').checked)
                return true;
            else
                MensajeCRUD("No se han realizado cambios, no se requiere actualización.", 2, 'DivMsgIdentidades')
            return false;
        }
        
        function HideMensajeCRUD() {
            document.getElementById('DivMsgIdentidadesCat').style.display = "none";
            document.getElementById('DivMsgIdentidades').style.display = "none";
        }

        function MensajeCRUD(mensaje, tipo, divMsg) {
            var pMensajeCRUD = document.getElementById(divMsg);
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


        function ShowModalPopup(objPanel) {
            $find(objPanel).show();
            return false;
        }

        function HideModalPopup(objPanel) {
            $find(objPanel).hide();
            return false;
        }
    </script>
</asp:Content>