using GLOBAL.DAL;
using GLOBAL.PERMISOS;
using GLOBAL.VAR;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using cnsAction = GLOBAL.CONST.clConstantes.Accion;
using cnsSection = GLOBAL.CONST.clConstantes.Section;

namespace SIDec.UserControls.Particular
{
    public partial class Actor : UserControl
    {
        private readonly ACTOR_DAL oActor = new ACTOR_DAL();
        private readonly IDENTIDADES_DAL oIdentidades = new IDENTIDADES_DAL();
        private readonly PERSONA_DAL oPersona = new PERSONA_DAL();

        private readonly clPermisos oPermisos = new clPermisos();
        private readonly clBasic oBasic = new clBasic();
        private readonly clGlobalVar oVar = new clGlobalVar();

        private const string _SOURCEPAGE = "Actor";

        #region Propiedades
        public int ActorID
        {
            get
            {
                return (Int32.TryParse(hddActorPrimary.Value, out int id) ? id : 0);
            }
            set
            {
                hddActorPrimary.Value = value.ToString();
                hdd_idactor.Value = hddActorPrimary.Value;
            }
        }
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
        public bool Enabled
        {
            get
            {
                return (Session[ControlID + ".Actor.Enabled"] ?? "0").ToString() == "1";
            }
            set
            {
                Session[ControlID + ".Actor.Enabled"] = value ? "1" : "0";
            }
        }
        public int ReferenceID
        {
            get
            {
                return (Int32.TryParse(hddReferenceID.Value, out int id) ? id : 0);
            }
            set
            {
                hddReferenceID.Value = value.ToString();
                ViewControls(false);
            }
        }
        public string ReferenceTypeID
        {
            get
            {
                return hddIdReferenceType.Value;
            }
            set
            {
                hddIdReferenceType.Value = value.ToString();
            }
        }
        /// <summary>
        /// Esta propiedad solo se debe asignar para asegurar que el contenedor de control requiera un usuario responsable
        /// </summary>
        public string ResponsibleUserCode
        {
            get
            {
                return (Session[ControlID + ".Actor.ResponsibleUserCode"] ?? "NoRequired").ToString();
            }
            set
            {
                Session[ControlID + ".Actor.ResponsibleUserCode"] = value;
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
            if (hdd_idactor.Value != "0")
            {
                MessageBox1.ShowConfirmation("EDIT", "¿Está seguro de actualizar la información?", type: "warning");
            }
            else
                MessageBox1.ShowConfirmation("ADD", "¿Está seguro de continuar con la acción solicitada?");
        }
        protected void btnActorAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            oBasic.AlertMain(msgActoresMain, "", "0");

            switch (btnAccionSource.CommandName)
            {
                case "Listar":
                    LoadGrid();
                    break;
                case "Editar":
                    if (!ValidateAccess(cnsSection.ACTOR, cnsAction.EDITAR)) return;
                    ViewEdit();
                    break;
                case "Agregar":
                    if (!ValidateAccess(cnsSection.ACTOR, cnsAction.INSERTAR)) return;
                    ViewAdd();
                    break;
                case "Eliminar":
                    if (!ValidateAccess(cnsSection.ACTOR, cnsAction.ELIMINAR)) return;
                    ViewDelete();
                    return;
            }
        }
        protected void btnActorAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateAccess(cnsSection.ACTOR, cnsAction.INSERTAR)) return;
            ViewAdd();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LoadGrilla();
        }
        protected void gvActors_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvActors, "Select$" + e.Row.RowIndex.ToString()));
            }
        }
        protected void gvActors_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            oBasic.AlertMain(msgActoresMain, "", "0"); 
            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                if (rowIndex >= gvActors.Rows.Count)
                    rowIndex = 0;
                hdd_idactor.Value = rowIndex >= 0 ? gvActors.DataKeys[rowIndex]["idactor"].ToString() : "0";

                switch (e.CommandName)
                {
                    case "_Detail":
                        ViewControls(true);
                        Enabled = false;
                        LoadDetail(); 
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
                string strResult = "";
                switch (key)
                {
                    case "ADD":
                        strResult = Save();
                        break;
                    case "EDIT":
                        strResult = Save();
                        break;
                    case "DELETE":
                        Delete();
                        break;
                    default:
                        break;
                }
                LoadGrid();
                ViewControls(false);
                gvActors.HeaderRow.Focus();
            }
            catch (Exception)
            {

            }
        }       
        #endregion



        #region Métodos Públicos
        public string GetActors()
        {
            string actores = string.Empty;
            DataSet ds = ((DataSet)oActor.sp_s_actores_listar_hijos(hdd_idactor.Value));
            if (ds.Tables.Count > 0)
            {
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    actores += ", " + dr["nombre"].ToString();
                }
            }
            return actores == string.Empty ? "" : actores.Substring(1);
        }
        public void LoadGrid()
        {
            if (!ValidateAccess(cnsSection.ACTOR, cnsAction.CONSULTAR, false)) 
            {
                hdd_idactor.Value = "-100";
                gvActors.EmptyDataText = "No cuenta con permisos suficientes para realizar esta acción";

                LoadGrilla();
                return;
            }

            LoadGrilla();
            upActor.Update();
        }
        #endregion



        #region Métodos Privados
        private void Delete()
        {
            if (!ValidateAccess(cnsSection.ACTOR, cnsAction.ELIMINAR)) return;
            
            string strResult = oActor.sp_d_actores(hdd_idactor.Value);
            oBasic.AlertUserControl(msgActores, msgActoresMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "d");
            hdd_idactor.Value = "0";
        }
        private void LoadGrilla()
        {
            gvActors.DataSource = ((DataSet)oActor.sp_s_actores_listar_hijos(hddActorPrimary.Value));
            gvActors.DataBind();
            gvActors.SelectedIndex = 0;
            ViewControls(false);

            oBasic.FixPanel(divData, "Actor", 0, pAdd: false, pEdit: false, pDelete: false);
        }
        private void LoadDetail()
        {
            DataSet dSet = oActor.sp_s_actores(hdd_idactor.Value);
            if (dSet != null && dSet.Tables.Count > 0 && dSet.Tables[0].Rows.Count > 0)
            {
                DataRow dRow = dSet.Tables[0].Rows[0];

                oBasic.fValueControls(pnlPBActorActions, dRow);
                if (lbl_fec_auditoria_actor.Text.Trim().Length > 0)
                {
                    lbl_fec_auditoria_actor.Text = "Modificado el: " + lbl_fec_auditoria_actor.Text;
                    lbl_fec_auditoria_actor.ToolTip = lbl_fec_auditoria_actor.Text;
                }
            }
            oBasic.EnableControls(pnlPBActorActions, Enabled, true);
            oBasic.FixPanel(divData, "Actor", Enabled ? 2 : 0, pList: true);
        }
        private void LoadDropDowns()
        {
            LoadDropDownIdentidad(ddl_idtipo_actor, "71");
            LoadDropDownIdentidad(ddl_idtipo_documento, "4");
            LoadDropDownIdentidad(ddl_idtipo_documento_rep, "4");
        }
        private void LoadDropDownIdentidad(DropDownList ddl, string id)
        {
            ddl.DataSource = oIdentidades.sp_s_identidad_id_categoria(id);
            ddl.DataTextField = "nombre_identidad";
            ddl.DataValueField = "id_identidad";
            ddl.DataBind();
        }
        private string Save()
        {
            string strResult = "";
            hdd_idactor.Value = hdd_idactor.Value == "" ? "0" : hdd_idactor.Value;
            Save_Persona();
            Save_PersonaRepresentante();
            string strID = Save_Actor();
            if (hddActorPrimary.Value == "0")
            {
                hddActorPrimary.Value = strID;
                strResult = oActor.sp_u_actores_referencia(hddActorPrimary.Value, hddReferenceID.Value, ReferenceTypeID);
            }

            return strResult;
        }
        private string Save_Actor()
        {
            if (hdd_idactor.Value == "0")
            {
                string strResult = oActor.sp_i_actores(hddActorPrimary.Value, oBasic.fInt2(ddl_idtipo_actor), hdd_idpersona.Value, hdd_idpersona_representante.Value);
                oBasic.AlertUserControl(msgActores, msgActoresMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "i");
                return strResult.Split(':')[1];
            }
            else
            {
                string strResult = oActor.sp_u_actores(hdd_idactor.Value, hddActorPrimary.Value, oBasic.fInt2(ddl_idtipo_actor), hdd_idpersona.Value, hdd_idpersona_representante.Value);
                oBasic.AlertUserControl(msgActores, msgActoresMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "u");
                return hdd_idactor.Value;
            }
        }
        private void Save_Persona()
        {
            DataSet dsPersona = oPersona.sp_s_persona_x_identificacion(oBasic.fInt2(ddl_idtipo_documento), txt_documento.Text);
            string idPersona = dsPersona != null && dsPersona.Tables.Count > 0 && dsPersona.Tables[0].Rows.Count > 0 ?
                                        dsPersona.Tables[0].Rows[0]["idpersona"].ToString() : hdd_idpersona.Value;
            if (idPersona == "0")
            {
                string strResult = oPersona.sp_i_persona(
                    oBasic.fInt2(ddl_idtipo_documento),
                    txt_documento.Text,
                    txt_nombre.Text,
                    txta_direccion.Text,
                    txt_telefono.Text,
                    txt_correo.Text);
                
                oBasic.AlertUserControl(msgActores, msgActoresMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "i");
                hdd_idpersona.Value =(strResult.Split(':'))[1];
            }
            else
            {
                string strResult = oPersona.sp_u_persona(
                    idPersona,
                    oBasic.fInt2(ddl_idtipo_documento),
                    txt_documento.Text,
                    txt_nombre.Text,
                    txta_direccion.Text,
                    txt_telefono.Text,
                    txt_correo.Text);

                oBasic.AlertUserControl(msgActores, msgActoresMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "u");
                hdd_idpersona.Value = idPersona;
            }
        }
        private void Save_PersonaRepresentante()
        {
            if (ddl_idtipo_documento_rep.SelectedIndex > 0 || txt_documento_rep.Text.Trim().Length >= 3)
            {
                DataSet dsPersona = oPersona.sp_s_persona_x_identificacion(oBasic.fInt2(ddl_idtipo_documento_rep), txt_documento_rep.Text);
                string idPersona = dsPersona != null && dsPersona.Tables.Count > 0 && dsPersona.Tables[0].Rows.Count > 0 ?
                                            dsPersona.Tables[0].Rows[0]["idpersona"].ToString() : hdd_idpersona_representante.Value;

                if (idPersona == "0" || idPersona == "")
                {
                    string strResult = oPersona.sp_i_persona(
                        oBasic.fInt2(ddl_idtipo_documento_rep),
                        txt_documento_rep.Text,
                        txt_nombre_rep.Text,
                        txt_direccion_rep.Text,
                        txt_telefono_rep.Text,
                        txt_correo_rep.Text);

                    oBasic.AlertUserControl(msgActores, msgActoresMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "i");
                    hdd_idpersona_representante.Value = (strResult.Split(':'))[1];
                }
                else
                {
                    string strResult = oPersona.sp_u_persona(
                        idPersona,
                        oBasic.fInt2(ddl_idtipo_documento_rep),
                        txt_documento_rep.Text,
                        txt_nombre_rep.Text,
                        txt_direccion_rep.Text,
                        txt_telefono_rep.Text,
                        txt_correo_rep.Text);

                    oBasic.AlertUserControl(msgActores, msgActoresMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "u");
                    hdd_idpersona_representante.Value = idPersona;
                }
            }
        }
        private bool ValidateAccess(string section, string action, bool validresponsible = true)
        {
            if (ResponsibleUserCode == "0" && validresponsible)
            {
                MessageInfo.ShowMessage("Esta acción requiere que se asigne un responsable al proyecto");
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
            hdd_idactor.Value = "0";
            ViewControls(true);
            Enabled = true;
            oBasic.EnableControls(pnlPBActorActions, Enabled, true);
            oBasic.FixPanel(divData, "Actor", 2);
        }
        private void ViewControls(bool _visible)
        {
            dvActorActions.Visible = _visible;
            gvActors.Visible = !_visible;
            oBasic.fClearControls(dvActorActions);
            hdd_idpersona.Value = hdd_idpersona_representante.Value = "0";
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