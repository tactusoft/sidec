<%@ Page Title="" Language="C#" MasterPageFile="~/AuthenticNew.Master" AutoEventWireup="true" CodeBehind="Declaratorias.aspx.cx" Inherits="SIDec.Declaratorias" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
	
  <%--********************************************************************--%>
  <%--*************** gridviews --%>
  <%--********************************************************************--%>
	<asp:HiddenField ID="hfGVDeclaratoriasSV" runat="server" />
	<asp:HiddenField ID="hfGVDeclaratoriasSH" runat="server" />

  <div id="divData" runat="server">
    <div class="col-12" role="main">
			
      
      <%--********************************************************************--%>
      <%--*************** Declaratorias --%>
      <%--********************************************************************--%>  
      <div class="card mt-3 mb-5">
        <div class="card-header card-header-main">
          <div class="row">
            <div class="col-sm-6 text-primary">
              <h4>Declaratorias</h4>
            </div>
          </div>
        </div>
				
        <div class="card-body">
					<asp:UpdatePanel runat="server" ID="upDeclaratorias" UpdateMode="Conditional">
						<ContentTemplate>
							<asp:MultiView runat="server" ID="mvDeclaratorias" ActiveViewIndex="0">
								<asp:View runat="server" ID="vDeclaratoriasGrid">
									<div class="gv-w">
										<asp:GridView ID="gvDeclaratorias" CssClass="gv" runat="server" DataKeyNames="cod_declaratoria" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="50"
											OnDataBinding="gvDeclaratorias_DataBinding" OnRowDataBound="gvDeclaratorias_RowDataBound" OnRowCommand="gvDeclaratorias_RowCommand">
											<Columns>
												<asp:BoundField DataField="cod_declaratoria" HeaderText="Código" SortExpression="cod_declaratoria" Visible="false" />
												<asp:BoundField DataField="resolucion_declaratoria" HeaderText="Resolución" ItemStyle-CssClass="" />
												<asp:BoundField DataField="fecha_resolucion_declaratoria" HeaderText="Fecha" HtmlEncode="false" DataFormatString="{0:dd/MMM/yyyy}" ItemStyle-CssClass="t-c" />
												<asp:BoundField DataField="tipo_declaratoria" HeaderText="Tipo" />
												<asp:BoundField DataField="desc_declaratoria" HeaderText="Descripción" />
												<asp:BoundField DataField="estado_declaratoria" HeaderText="Estado" SortExpression="estado_declaratoria" ItemStyle-CssClass="" />
												<asp:TemplateField ShowHeader="true" HeaderText="Doc" ItemStyle-CssClass="t-c w40">
													<ItemTemplate>
														<asp:ImageButton runat="server" ImageUrl="~/images/icon/pdf-icon.png" CommandName="OpenFile" ToolTip="Abrir documento" CommandArgument='<%# Container.DisplayIndex %>' />
													</ItemTemplate>
												</asp:TemplateField>
											</Columns>
											<SelectedRowStyle CssClass="gvItemSelected" />
											<HeaderStyle CssClass="gvHeader" />
											<RowStyle CssClass="gvItem" />
											<PagerStyle CssClass="gvPager" />
										</asp:GridView>
									</div>
								</asp:View>
							</asp:MultiView>
						</ContentTemplate>
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="gvDeclaratorias" EventName="SelectedIndexChanged" />
						</Triggers>
					</asp:UpdatePanel>
				</div>
			</div>
    </div>
  </div>

	<script>
		function pageLoad() {
			//*****************************************ESTILO GRIDVIEWS
			$('#<%=gvDeclaratorias.ClientID%>').gridviewScroll({
				height: 350,
				freezesize: 1,
				startVertical: $("#<%=hfGVDeclaratoriasSV.ClientID%>").val(),
				startHorizontal: $("#<%=hfGVDeclaratoriasSH.ClientID%>").val(),
				onScrollVertical: function (delta) { $("#<%=hfGVDeclaratoriasSV.ClientID%>").val(delta); },
				onScrollHorizontal: function (delta) { $("#<%=hfGVDeclaratoriasSH.ClientID%>").val(delta); }
			});
		}

		
	</script>
</asp:Content>
