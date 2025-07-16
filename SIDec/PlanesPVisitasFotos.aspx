<%@ Page Language="C#" MasterPageFile="~/PopupNew.Master" AutoEventWireup="true" CodeBehind="PlanesPVisitasFotos.aspx.cs" Inherits="SIDec.PlanesPVisitasFotos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
  <div class="">
    <asp:MultiView runat="server" ID="mvFotos" ActiveViewIndex="1">
      <asp:View runat="server" ID="vDefault">
        <div class="alert alert-danger" role="alert">
          No puede acceder directamente a esta opcion
        </div>
      </asp:View>
      <asp:View runat="server" ID="vOpciones">
        <div class="alert alert-info mt-0 card-header-sub" role="alert">
          Planes Parciales / Visitas Fotos
        </div>

        <div runat="server" id="divFotoEdicion" visible="false" class="col-12">
          <div class="form-inline">
            <div class="form-group col-md-4 col-lg-auto">
                <asp:FileUpload ID="FileUpload1" CssClass="" runat="server" accept="image/*" TabIndex="1" AllowMultiple="false" />
            </div>
          </div>
          <div class="form-inline mb-3">
            <div class="form-group">
              <label for="txtDescripcionFoto" class="mr-2">Descripción</label>
              <asp:TextBox runat="server" ID="txtDescripcionFoto" CssClass="form-control form-control-xs" TabIndex="3" MaxLength="20"></asp:TextBox>
            </div>
            <div class="form-group col-auto col-sm-auto">
              <div class="btn-group btn-group-sm">
                <asp:LinkButton ID="lbCancelar" runat="server" CssClass="btn btn-outline-secondary" OnClick="lbCancelar_Click" CausesValidation="false">
                  <i class="fas fa-times"></i>&nbspCancelar
                </asp:LinkButton>
                <asp:LinkButton ID="lbSubir" runat="server" CssClass="btn btn-outline-primary" TabIndex="5" Text="Aceptar" ValidationGroup="vgFotoVisita" OnClick="lbSubir_Click" CausesValidation="true">
                  <i class="fas fa-check"></i>&nbspAceptar
                </asp:LinkButton>
              </div>
            </div>
          </div>
        </div>

        <div runat="server" id="divFotoContainer" class="col-12">
          <div class="row row-cols-1 row-cols-md-3">
            <div class="col">
              <div class="card mb-3">
                <div class="card-header">
                  <asp:Label runat="server" ID="lblObs1" CssClass=""></asp:Label>
                  <div class="btn-group fr">
                    <asp:LinkButton ID="btnFotoAdd1" runat="server" CssClass="btn btn-outline-danger" CausesValidation="false" CommandName="A1" ToolTip="Agregar foto" OnClick="btnFotoAccion_Click">
								      <i class="fas fa-plus"></i>
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnFotoEdit1" runat="server" CssClass="btn btn-outline-danger" CausesValidation="false" CommandName="E1" ToolTip="Editar foto" OnClick="btnFotoAccion_Click">
								      <i class="fas fa-pencil-alt"></i>
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnFotoDel1" runat="server" CssClass="btn btn-outline-danger" CausesValidation="false" CommandName="D1" ToolTip="Eliminar foto" OnClick="btnFotoAccion_Click">
								      <i class="far fa-trash-alt"></i>
                    </asp:LinkButton>
                  </div>
                </div>
                <div class="m-1">
                  <asp:Image runat="server" ID="Image1" CssClass="w-100 rounded" />
                </div>
              </div>
            </div>

            <div class="col">
              <div class="card mb-3">
                <div class="card-header">
                  <asp:Label runat="server" ID="lblObs2" CssClass=""></asp:Label>
                  <div class="btn-group fr">
                    <asp:LinkButton ID="btnFotoAdd2" runat="server" CssClass="btn btn-outline-danger" CausesValidation="false" CommandName="A2" ToolTip="Agregar foto" OnClick="btnFotoAccion_Click">
								      <i class="fas fa-plus"></i>
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnFotoEdit2" runat="server" CssClass="btn btn-outline-danger" CausesValidation="false" CommandName="E2" ToolTip="Editar foto" OnClick="btnFotoAccion_Click">
								      <i class="fas fa-pencil-alt"></i>
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnFotoDel2" runat="server" CssClass="btn btn-outline-danger" CausesValidation="false" CommandName="D2" ToolTip="Eliminar foto" OnClick="btnFotoAccion_Click">
								      <i class="far fa-trash-alt"></i>
                    </asp:LinkButton>
                  </div>
                </div>
                <div class="m-1">
                  <asp:Image runat="server" ID="Image2" CssClass="w-100 rounded" />
                </div>
              </div>
            </div>

            <div class="col">
              <div class="card mb-3">
                <div class="card-header">
                  <asp:Label runat="server" ID="lblObs3" CssClass=""></asp:Label>
                  <div class="btn-group fr">
                    <asp:LinkButton ID="btnFotoAdd3" runat="server" CssClass="btn btn-outline-danger" CausesValidation="false" CommandName="A3" ToolTip="Agregar foto" OnClick="btnFotoAccion_Click">
								      <i class="fas fa-plus"></i>
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnFotoEdit3" runat="server" CssClass="btn btn-outline-danger" CausesValidation="false" CommandName="E3" ToolTip="Editar foto" OnClick="btnFotoAccion_Click">
								      <i class="fas fa-pencil-alt"></i>
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnFotoDel3" runat="server" CssClass="btn btn-outline-danger" CausesValidation="false" CommandName="D3" ToolTip="Eliminar foto" OnClick="btnFotoAccion_Click">
								      <i class="far fa-trash-alt"></i>
                    </asp:LinkButton>
                  </div>
                </div>
                <div class="m-1">
                  <asp:Image runat="server" ID="Image3" CssClass="w-100 rounded" />
                </div>
              </div>
            </div>
          </div>
        </div>

        <!--MODAL POPUP-->
        <%--<div class="modal fade" data-backdrop="static" role="dialog" aria-hidden="true">
          <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title">Confirmar acción</h5>
              </div>
              <div class="modal-body">
                ¿Desea eliminar la imagen?
              </div>
              <div class="modal-footer">
                <asp:LinkButton ID="btnNoEliminar" runat="server" CssClass="btn btn-outline-secondary btn-sm" CausesValidation="false">
                  <i class="fas fa-times"></i>&nbsp&nbspCancelar
                </asp:LinkButton>
                <asp:LinkButton ID="btnSiEliminar" runat="server" Text="" CssClass="btn btn-outline-primary" CausesValidation="false" OnClick="btnSiEliminar_Click" data-dismiss="modal">
							      <i class="fas fa-check"></i>&nbsp&nbspAceptar
                </asp:LinkButton>
              </div>
            </div>
          </div>
        </div>--%>


        <asp:LinkButton ID="lbDummyEliminar" runat="server"></asp:LinkButton>
        <ajaxToolkit:ModalPopupExtender ID="mpeEliminar" runat="server" PopupControlID="pnlPopupEliminar" TargetControlID="lbDummyEliminar" CancelControlID="btnNoEliminar" BehaviorID="puEliminar"></ajaxToolkit:ModalPopupExtender>
        <asp:Panel ID="pnlPopupEliminar" runat="server" CssClass="modal-dialog modal-dialog-centered w-50" data-backdrop="static" role="dialog" Style="display: none">
          <div class="modal-content">
            <div class="modal-header modal-bg-info">
              <h5 class="modal-title">Confirmar acción</h5>
            </div>
            <div class="modal-body">
              ¿Desea eliminar la imagen?
            </div>
            <div class="modal-footer">
              <asp:LinkButton ID="btnNoEliminar" runat="server" CssClass="btn btn-outline-secondary" CausesValidation="false">
                <i class="fas fa-times"></i>&nbsp&nbspCancelar
              </asp:LinkButton>
              <asp:LinkButton ID="btnSiEliminar" runat="server" CssClass="btn btn-outline-primary" CausesValidation="false" OnClick="btnSiEliminar_Click">
                <i class="fas fa-check"></i>&nbsp&nbspAceptar
              </asp:LinkButton>
            </div>
          </div>
        </asp:Panel>
      </asp:View>
    </asp:MultiView>
  </div>
  <script>
    function closeWindow() {
      window.close();
    }
  </script>
</asp:Content>
