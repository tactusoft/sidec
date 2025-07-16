using GLOBAL.DAL;
using GLOBAL.PERMISOS;
using GLOBAL.VAR;
using SigesTO;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using cnsAction = GLOBAL.CONST.clConstantes.Accion;
using cnsSection = GLOBAL.CONST.clConstantes.Section;

namespace SIDec.UserControls.PrediosDeclarados
{
    public partial class Interesado : UserControl
    {
        private readonly INTERESADO_DAL oInteresado = new INTERESADO_DAL();
        private readonly IDENTIDADES_DAL oIdentidades = new IDENTIDADES_DAL();

        private readonly clPermisos oPermisos = new clPermisos();
        private readonly clBasic oBasic = new clBasic();
        private readonly clGlobalVar oVar = new clGlobalVar();

        private const string _SOURCEPAGE = "Interesado";

        #region Propiedades
        /// <summary>
        /// Identificador de un registro específico
        /// </summary>
        public int InteresadoID
        {
            get
            {
                return (int.TryParse(hddInteresadoPrimary.Value, out int id) ? id : 0);
            }
            set
            {
                hddInteresadoPrimary.Value = value.ToString();
                hdd_idinteresado.Value = hddInteresadoPrimary.Value;
            }
        }
        /// <summary>
        /// Identificador del control en caso de requerirse varios en un mismo formulario
        /// </summary>
        public string ControlID
        {
            get
            {
                return (hddId.Value);
            }
            set
            {
                hddId.Value = value;
            }
        }
        /// <summary>
        /// Estado de edición en el control
        /// </summary>
        public bool Enabled
        {
            get
            {
                return (Session[ControlID + ".Interesado.Enabled"] ?? "0").ToString() == "1";
            }
            set
            {
                Session[ControlID + ".Interesado.Enabled"] = value ? "1" : "0";
            }
        }
        /// <summary>
        /// Identificador del padre de la informción que se maneja en el control
        /// </summary>
        public int ReferenceID
        {
            get
            {
                return (Int32.TryParse(hddReferenceID.Value, out int id) ? id : 0);
            }
            set
            {
                hddReferenceID.Value = value.ToString();
                oBasic.AlertMain(msgInteresadoesMain, "", "0");
                ViewControls(false);
            }
        }
        /// <summary>
        /// Esta propiedad solo se debe asignar para asegurar que el contenedor de control requiera un usuario responsable
        /// </summary>
        public string ResponsibleUserCode
        {
            get
            {
                return (Session[ControlID + ".Interesado.ResponsibleUserCode"] ?? "NoRequired").ToString();
            }
            set
            {
                Session[ControlID + ".Interesado.ResponsibleUserCode"] = value;
            }
        }
        #endregion



        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropDowns();
                ViewControls(false);
            }
        }
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            if (hdd_idinteresado.Value != "0")
            {
                MessageBox1.ShowConfirmation("EDIT", "¿Está seguro de actualizar la información?", type: "warning");
            }
            else
                MessageBox1.ShowConfirmation("ADD", "¿Está seguro de continuar con la acción solicitada?");
        }
        protected void btnInteresadoAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            oBasic.AlertMain(msgInteresadoesMain, "", "0");

            switch (btnAccionSource.CommandName)
            {
                case "Listar":
                    LoadGrid();
                    break;
                case "Editar":
                    if (!ValidateAccess(cnsSection.PRED_DECL_INTERESADO, cnsAction.EDITAR)) return;
                    ViewEdit();
                    break;
                case "Agregar":
                    if (!ValidateAccess(cnsSection.PRED_DECL_INTERESADO, cnsAction.INSERTAR)) return;
                    ViewAdd();
                    break;
                case "Eliminar":
                    if (!ValidateAccess(cnsSection.PRED_DECL_INTERESADO, cnsAction.ELIMINAR)) return;
                    ViewDelete();
                    return;
            }
        }
        protected void btnInteresadoAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateAccess(cnsSection.PRED_DECL_INTERESADO, cnsAction.INSERTAR)) return;
            ViewAdd();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            hdd_idinteresado.Value = hddInteresadoPrimary.Value;
            Enabled = false;
            if (hdd_idinteresado.Value == "0")
                LoadGrilla();
            else
                LoadDetail();
        }
        protected void gvInteresados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvInteresados, "Select$" + e.Row.RowIndex.ToString()));
            }
        }
        protected void gvInteresados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            oBasic.AlertMain(msgInteresadoesMain, "", "0");
            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                if (rowIndex >= gvInteresados.Rows.Count)
                    rowIndex = 0;
                InteresadoID = rowIndex >= 0 ? Convert.ToInt32(gvInteresados.DataKeys[rowIndex]["idinteresado"].ToString()) : 0;

                switch (e.CommandName)
                {
                    case "_Detail":
                        ViewControls(true);
                        Enabled = false;
                        LoadDetail();
                        break;
                    case "_Delete":
                        if (!ValidateAccess(cnsSection.PRED_DECL_INTERESADO, cnsAction.ELIMINAR)) return;
                        ViewDelete();
                        break;
                    case "_Edit":
                        if (!ValidateAccess(cnsSection.PRED_DECL_INTERESADO, cnsAction.EDITAR)) return;
                        ViewEdit();
                        break;
                    default:
                        return;
                }
            }
        }
        protected void MessageBox_Accept(string key)
        {
            try
            {
                switch (key)
                {
                    case "ADD":
                        Save();
                        break;
                    case "EDIT":
                        Save();
                        break;
                    case "DELETE":
                        Delete();
                        break;
                    default:
                        break;
                }
                LoadGrid();
                ViewControls(false);
                gvInteresados.HeaderRow.Focus();
            }
            catch (Exception)
            {

            }
        }
        #endregion



        #region Métodos Públicos
        public void LoadGrid()
        {
            if (!ValidateAccess(cnsSection.PRED_DECL_INTERESADO, cnsAction.CONSULTAR, false))
            {
                hdd_idinteresado.Value = "-100";
                gvInteresados.EmptyDataText = "No cuenta con permisos suficientes para realizar esta acción";

                LoadGrilla();
                return;
            }

            LoadGrilla();
            upInteresado.Update();
        }
        #endregion



        #region Métodos Privados
        private void Delete()
        {
            if (!ValidateAccess(cnsSection.PRED_DECL_INTERESADO, cnsAction.ELIMINAR)) return;

            string strResult = oInteresado.sp_d_interesado(hdd_idinteresado.Value);
            oBasic.AlertUserControl(msgInteresadoes, msgInteresadoesMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "d");
            InteresadoID = 0;
        }
        private void LoadGrilla()
        {
            gvInteresados.DataSource = ((DataSet)oInteresado.sp_s_interesados(ReferenceID.ToString()));
            gvInteresados.DataBind();
            gvInteresados.SelectedIndex = 0;
            ViewControls(false);

            oBasic.FixPanel(divData, "Interesado", 0, pAdd: false, pEdit: false, pDelete: false);
        }
        private void LoadDetail()
        {
            DataSet dSet = oInteresado.sp_s_interesado(hdd_idinteresado.Value);
            if (dSet != null && dSet.Tables.Count > 0 && dSet.Tables[0].Rows.Count > 0)
            {
                DataRow dRow = dSet.Tables[0].Rows[0];

                oBasic.fValueControls(pnlPBInteresadoActions, dRow);
                if (lbl_fec_auditoria_interesado.Text.Trim().Length > 0)
                {
                    lbl_fec_auditoria_interesado.Text = "Modificado el: " + lbl_fec_auditoria_interesado.Text;
                    lbl_fec_auditoria_interesado.ToolTip = lbl_fec_auditoria_interesado.Text;
                }
            }
            oBasic.EnableControls(pnlPBInteresadoActions, Enabled, true);
            oBasic.FixPanel(divData, "Interesado", Enabled ? 2 : 0, pList: true);
        }
        private void LoadDropDowns()
        {
            LoadDropDownIdentidad(ddl_idtipo_interesado, "79");
        }
        private void LoadDropDownIdentidad(DropDownList ddl, string id)
        {
            ddl.DataSource = oIdentidades.sp_s_identidad_id_categoria(id);
            ddl.DataTextField = "nombre_identidad";
            ddl.DataValueField = "id_identidad";
            ddl.DataBind();
        }
        private void Save()
        {
            hdd_idinteresado.Value = hdd_idinteresado.Value == "" ? "0" : hdd_idinteresado.Value;
            Save_Interesado();
        }
        private string Save_Interesado()
        {
            InteresadoTO interesado = new InteresadoTO()
            {
                IdPredioDeclarado = ReferenceID,
                IdTipoInteresado = Convert.ToInt32(ddl_idtipo_interesado.SelectedValue),
                Documento = txt_documento.Text,
                Nombre = txt_nombre.Text,
                Telefono = txt_telefono.Text,
                Direccion = txt_direccion.Text,
                Correo = txt_correo.Text,
                Otro = txt_otro.Text
            };
            if (hdd_idinteresado.Value == "0")
            {
                string strResult = oInteresado.sp_i_interesado(interesado);
                oBasic.AlertUserControl(msgInteresadoes, msgInteresadoesMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "i");
                return strResult.Split(':')[1];
            }
            else
            {
                interesado.IdInteresado = Convert.ToInt32(hdd_idinteresado.Value);
                string strResult = oInteresado.sp_u_interesado(interesado);
                oBasic.AlertUserControl(msgInteresadoes, msgInteresadoesMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "u");
                return hdd_idinteresado.Value;
            }
        }
        private bool ValidateAccess(string section, string action, bool validresponsible = true)
        {
            if (ResponsibleUserCode == "0" && validresponsible)
            {
                MessageInfo.ShowMessage("Esta acción requiere que se asigne un responsable al predio");
                return false;
            }
            string cod_usu_responsable = (ResponsibleUserCode == "NoRequired" ? oVar.prUserCod : ResponsibleUserCode).ToString();
            if (!oPermisos.TienePermisosAccion(section, action))
            {
                MessageInfo.ShowMessage("No cuenta con permisos suficientes para realizar esta acción");
                return false;
            }
            else if (!oPermisos.TienePermisosAccion(section, action, cod_usu_responsable, oVar.prUserCod.ToString()))
            {
                if (cod_usu_responsable.Contains("-"))
                {
                    MessageInfo.ShowMessage("Este registro no es editable. Si requiere su habilitación, por favor comunicarse con el administrador");
                    return false;
                }
                MessageInfo.ShowMessage("Para realizar esta acción comuníquese con el usuario responsable del proyecto");
                return false;
            }
            return true;
        }
        private void ViewAdd()
        {
            hdd_idinteresado.Value = "0";
            ViewControls(true);
            Enabled = true;
            oBasic.EnableControls(pnlPBInteresadoActions, Enabled, true);
            oBasic.FixPanel(divData, "Interesado", 2);
        }
        private void ViewControls(bool _visible)
        {
            dvInteresadoActions.Visible = _visible;
            gvInteresados.Visible = !_visible;
            oBasic.fClearControls(dvInteresadoActions);
        }
        private void ViewDelete()
        {
            ViewControls(true);
            Enabled = false;
            LoadDetail();
            MessageBox1.ShowConfirmation("DELETE", "¿Esta seguro que desea eliminar el registro?", type: "danger");
        }
        private void ViewEdit()
        {
            ViewControls(true);
            Enabled = true;
            LoadDetail();
        }

        #endregion


    }
}