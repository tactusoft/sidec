using GLOBAL.DAL;
using GLOBAL.LOG;
using GLOBAL.PERMISOS;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using SigesTO;
using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

using cnsAction = GLOBAL.CONST.clConstantes.Accion;
using cnsSection = GLOBAL.CONST.clConstantes.Section;

namespace SIDec.UserControls.Particular
{
    public partial class ProyectoCarta : UserControl
    {
        private readonly PROYECTOSCARTAS_DAL oProyectoCarta = new PROYECTOSCARTAS_DAL();
        private readonly IDENTIDADES_DAL oIdentidades = new IDENTIDADES_DAL();

        private readonly clPermisos oPermisos = new clPermisos();
        private readonly clBasic oBasic = new clBasic();
        private readonly clFile oFile = new clFile();
        private readonly clLog oLog = new clLog();
        private readonly clGlobalVar oVar = new clGlobalVar();

        private const string _SOURCEPAGE = "ProyectoCarta";

        public delegate void OnViewDocEventHandler(object sender);
        public event OnViewDocEventHandler ViewDoc;

        #region Propiedades
        /// <summary>
        /// Identificador de un registro específico
        /// </summary>
        public int ProyectoCartaID
        {
            get
            {
                return (int.TryParse(hddProyectoCartaPrimary.Value, out int id) ? id : 0);
            }
            set
            {
                hddProyectoCartaPrimary.Value = value.ToString();
                hdd_idproyecto_carta.Value = hddProyectoCartaPrimary.Value;
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
                return (Session[ControlID + ".ProyectoCarta.Enabled"] ?? "0").ToString() == "1";
            }
            set
            {
                Session[ControlID + ".ProyectoCarta.Enabled"] = value ? "1" : "0";
            }
        }
        /// <summary>
        /// Identificador del padre de la informción que se maneja en el control
        /// </summary>
        public int ReferenceID
        {
            get
            {
                return (int.TryParse(hddReferenceID.Value, out int id) ? id : 0);
            }
            set
            {
                hddReferenceID.Value = value.ToString();
                oBasic.AlertMain(msgProyectoCartaesMain, "", "0");
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
                return (Session[ControlID + ".ProyectoCarta.ResponsibleUserCode"] ?? "NoRequired").ToString();
            }
            set
            {
                Session[ControlID + ".ProyectoCarta.ResponsibleUserCode"] = value;
            }
        }
        #endregion



        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterScript();
            if (!IsPostBack)
            {
                LoadDropDowns();
                ViewControls(false);
            }
            Initialize();
        }
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            if (hdd_idproyecto_carta.Value != "0")
            {
                MessageBox1.ShowConfirmation("EDIT", "¿Está seguro de actualizar la información?", type: "warning");
            }
            else
                MessageBox1.ShowConfirmation("ADD", "¿Está seguro de continuar con la acción solicitada?");
        }
        protected void btnCartaAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateAccess(cnsSection.PROY_ASOC_CARTAS_INTENCION, cnsAction.INSERTAR)) return;
            ViewAdd();
        }
        protected void btnProyectoCartaAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            oBasic.AlertMain(msgProyectoCartaesMain, "", "0");

            switch (btnAccionSource.CommandName)
            {
                case "Listar":
                    LoadGrid();
                    break;
                case "Editar":
                    if (!ValidateAccess(cnsSection.PROY_ASOC_CARTAS_INTENCION, cnsAction.EDITAR)) return;
                    ViewEdit();
                    break;
                case "Agregar":
                    if (!ValidateAccess(cnsSection.PROY_ASOC_CARTAS_INTENCION, cnsAction.INSERTAR)) return;
                    ViewAdd();
                    break;
                case "Eliminar":
                    if (!ValidateAccess(cnsSection.PROY_ASOC_CARTAS_INTENCION, cnsAction.ELIMINAR)) return;
                    ViewDelete();
                    return;
            }
        }
        protected void btnProyectoCartaAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateAccess(cnsSection.PROY_ASOC_CARTAS_INTENCION, cnsAction.INSERTAR)) return;
            ViewAdd();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            hdd_idproyecto_carta.Value = hddProyectoCartaPrimary.Value;
            Enabled = false;
            if (hdd_idproyecto_carta.Value == "0")
                LoadGrilla();
            else
                LoadDetail();
        }
        protected void gvProyectoCartas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvProyectoCartas, "Select$" + e.Row.RowIndex.ToString()));
                string carta_intencion_firmada = gvProyectoCartas.DataKeys[e.Row.DataItemIndex % gvProyectoCartas.PageSize]["carta_intencion_firmada"].ToString();
                string meses_desarrollo = gvProyectoCartas.DataKeys[e.Row.DataItemIndex % gvProyectoCartas.PageSize]["meses_desarrollo"].ToString();
                string fecha_firma = gvProyectoCartas.DataKeys[e.Row.DataItemIndex % gvProyectoCartas.PageSize]["fecha_firma"].ToString();
                ((Label)e.Row.FindControl("lblSemaforo")).Visible = false;
                bool enProceso = gvProyectoCartas.DataKeys[e.Row.DataItemIndex % gvProyectoCartas.PageSize]["proy_en_proceso"].ToString() == "1";

                if (carta_intencion_firmada == "1")
                {
                    ((Label)e.Row.FindControl("lblSemaforo")).ForeColor = System.Drawing.Color.DarkRed;
                    if (Int32.TryParse(meses_desarrollo, out int meses))
                    {
                        ((Label)e.Row.FindControl("lblSemaforo")).Visible = true && enProceso;
                        ((Label)e.Row.FindControl("lblDiff")).Text = meses.ToString();
                        if (DateTime.TryParse(fecha_firma, out DateTime fechafirma))
                        {
                            DateTime fechavigencia = fechafirma.AddMonths(meses);
                            if (fechavigencia >= DateTime.Today && enProceso)
                            {
                                TimeSpan difFechas = fechavigencia - DateTime.Today;

                                ((Label)e.Row.FindControl("lblDiff")).Text = String.Format("{0:N0}", (difFechas.Days / 30.0)) + "/" + meses.ToString();
                                double porcentaje = difFechas.Days * 100.0 / (meses * 30.0);
                                ((Label)e.Row.FindControl("lblSemaforo")).CssClass = "";
                                if (porcentaje <= 30)
                                {
                                    ((Label)e.Row.FindControl("lblSemaforo")).ForeColor = System.Drawing.Color.Red;
                                    ((Label)e.Row.FindControl("lblSemaforo")).CssClass = "animated flash";
                                }
                                else if (porcentaje <= 60)
                                {
                                    ((Label)e.Row.FindControl("lblSemaforo")).ForeColor = System.Drawing.Color.Yellow;
                                }
                                else
                                {
                                    ((Label)e.Row.FindControl("lblSemaforo")).ForeColor = System.Drawing.Color.Green;
                                }
                            }

                        }
                    }
                }
            }
        }
        protected void gvProyectoCartas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            oBasic.AlertMain(msgProyectoCartaesMain, "", "0");
            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                if (rowIndex >= gvProyectoCartas.Rows.Count)
                    rowIndex = 0;
                ProyectoCartaID = rowIndex >= 0 ? Convert.ToInt32(gvProyectoCartas.DataKeys[rowIndex]["au_proyecto_carta"].ToString()) : 0;

                switch (e.CommandName)
                {
                    case "_Detail":
                        ViewControls(true);
                        Enabled = false;
                        LoadDetail();
                        break;
                    case "OpenFile":
                        string ruta = gvProyectoCartas.DataKeys[rowIndex]["ruta_carta"].ToString();
                        hdd_ruta_carta.Value = Path.Combine(oVar.prPathDocumentosProyectos.ToString(), ruta);
                        ViewDocument();
                        break;
                    default:
                        return;
                }
            }
        }
        protected void lblPdfCarta_Click(object sender, EventArgs e)
        {
            ViewDocument();
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
                gvProyectoCartas.HeaderRow.Focus();
            }
            catch (Exception)
            {

            }
        }
        #endregion



        #region Métodos Públicos
        public void LoadGrid()
        {
            if (!ValidateAccess(cnsSection.PROY_ASOC_CARTAS_INTENCION, cnsAction.CONSULTAR, false))
            {
                hdd_idproyecto_carta.Value = "-100";
                gvProyectoCartas.EmptyDataText = "No cuenta con permisos suficientes para realizar esta acción";

                LoadGrilla();
                return;
            }

            LoadGrilla();
            upProyectoCarta.Update();
        }
        #endregion



        #region Métodos Privados
        private void Delete()
        {
            if (!ValidateAccess(cnsSection.PROY_ASOC_CARTAS_INTENCION, cnsAction.ELIMINAR)) return;

            string strResult = oProyectoCarta.sp_d_proyecto_carta(hdd_idproyecto_carta.Value);
            oBasic.AlertUserControl(msgProyectoCartaes, msgProyectoCartaesMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "d");
            ProyectoCartaID = 0;
        }
        private void Initialize()
        {
            cal_fecha_radicado_manifestacion_interes.StartDate = cal_fecha_radicado_carta_intencion.StartDate = cal_fecha_radicado_otrosi.StartDate
                = cal_fecha_firma.StartDate = new DateTime(2008, 1, 1);

            rvfecha_radicado_manifestacion_interes.MinimumValue = rvfecha_radicado_carta_intencion.MinimumValue = rvfecha_radicado_otrosi.MinimumValue
                = rvfecha_firma.MinimumValue = (new DateTime(2008, 1, 1)).ToString("yyyy-MM-dd");

            cal_fecha_radicado_manifestacion_interes.EndDate = cal_fecha_radicado_carta_intencion.EndDate = cal_fecha_radicado_otrosi.EndDate
                = cal_fecha_firma.EndDate = DateTime.Today;

            rvfecha_radicado_manifestacion_interes.MaximumValue = rvfecha_radicado_carta_intencion.MaximumValue = rvfecha_radicado_otrosi.MaximumValue
                = rvfecha_firma.MaximumValue = (DateTime.Today).ToString("yyyy-MM-dd");
        }
        private void LoadGrilla()
        {
            gvProyectoCartas.DataSource = ((DataSet)oProyectoCarta.sp_s_proyectos_cartas_cod_proyecto(ReferenceID.ToString()));
            gvProyectoCartas.DataBind();
            gvProyectoCartas.SelectedIndex = 0;
            ViewControls(false);

            oBasic.FixPanel(divData, "ProyectoCarta", 0, pAdd: false, pEdit: false, pDelete: false);
        }
        private void LoadDetail()
        {
            DataSet dSet = oProyectoCarta.sp_s_proyecto_carta(hdd_idproyecto_carta.Value);
            if (dSet != null && dSet.Tables.Count > 0 && dSet.Tables[0].Rows.Count > 0)
            {
                DataRow dRow = dSet.Tables[0].Rows[0];

                oBasic.fValueControls(pnlPBProyectoCartaActions, dRow);
                if (lbl_fec_auditoria_proyecto_carta.Text.Trim().Length > 0)
                {
                    lbl_fec_auditoria_proyecto_carta.Text = "Modificado el: " + lbl_fec_auditoria_proyecto_carta.Text;
                    lbl_fec_auditoria_proyecto_carta.ToolTip = lbl_fec_auditoria_proyecto_carta.Text;
                }
            }
            oBasic.EnableControls(pnlPBProyectoCartaActions, Enabled, true);
            lblInfoFileCarta.Text = "";
            oBasic.FixPanel(divData, "ProyectoCarta", Enabled ? 2 : 0, pList: true);
            ViewButtoms();
        }
        private void LoadDropDowns()
        {
            LoadDropDownIdentidad(ddl_id_documento_constitucion_proyecto, "54");
        }
        private void LoadDropDownIdentidad(DropDownList ddl, string id)
        {
            ddl.DataSource = oIdentidades.sp_s_identidad_id_categoria(id);
            ddl.DataTextField = "nombre_identidad";
            ddl.DataValueField = "id_identidad";
            ddl.DataBind();
        }
        private void RegisterScript()
        {
            Page.Form.Enctype = "multipart/form-data";
            lbLLoadCarta.Attributes.Add("onclick", "$(\"input[ID*='" + fuLoadCarta.ClientID + "']\").click();return false;");
        }
        private void Save()
        {
            hdd_idproyecto_carta.Value = hdd_idproyecto_carta.Value == "" ? "0" : hdd_idproyecto_carta.Value;
            Save_ProyectoCarta();
        }
        private string Save_ProyectoCarta()
        {
            var fileName = "";
            if (fuLoadCarta.HasFile)
            {
                fileName = "CIPY_" + Guid.NewGuid() + Path.GetExtension(fuLoadCarta.FileName);
                var pdf_file = Path.Combine(oVar.prPathDocumentosProyectos.ToString(), fileName);
                try
                {
                    fuLoadCarta.SaveAs(pdf_file);
                }
                catch (Exception e)
                {
                    oLog.RegistrarLogError("Error subiendo archivo " + e.Message + ":" + fuLoadCarta.FileName + ":::" + fuLoadCarta.PostedFile.ContentLength, _SOURCEPAGE, "SaveProyectoLicencia");
                }
            }
            ProyectoCartaTO proyecto_carta = new ProyectoCartaTO()
            {
                IdProyecto = ReferenceID.ToString(),
                IdProyectoCarta = oBasic.fInt(txt_au_proyecto_carta),
                RadicadoManifestacionInteres = txt_radicado_manifestacion_interes.Text,
                FechaRadicadoManifestacionInteres = txt_fecha_radicado_manifestacion_interes.Text,
                RadicadoCartaIntencion = txt_radicado_carta_intencion.Text,
                FechaRadicadoCartaIntencion = txt_fecha_radicado_carta_intencion.Text,
                CartaIntencionFirmada = chk_carta_intencion_firmada.Checked,
                FechaFirma = txt_fecha_firma.Text,
                IdDocumentoConstitucionProyecto = oBasic.fInt(ddl_id_documento_constitucion_proyecto),
                RadicadoOtrosi = txt_radicado_otrosi.Text,
                FechaRadicadoOtrosi = txt_fecha_radicado_otrosi.Text,
                MesesDesarrollo = txt_meses_desarrollo.Text,
                UnidadGestionAplicaProyecto = txt_unidad_gestion_aplica_proyecto.Text,
                EtapaAplicaProyecto = txt_etapa_aplica_proyecto.Text,
                AreaUtil = oBasic.fDec(txt_area_util__carta),
                AreaMinimaVivienda = txt_area_minima_vivienda.Text,
                LocalizacionProyecto = txt_localizacion_proyecto.Text,
                UP_VIP = oBasic.fInt(txt_UP_VIP__carta),
                UP_VIS = oBasic.fInt(txt_UP_VIS__carta),
                UP_E3 = oBasic.fInt(txt_UP_E3),
                UP_E4 = oBasic.fInt(txt_UP_E4),
                UP_E5 = oBasic.fInt(txt_UP_E5),
                UP_E6 = oBasic.fInt(txt_UP_E6),
                RutaArchivo = fileName,
                Observacion = txt_observacion__carta.Text
            };
            if (hdd_idproyecto_carta.Value == "0")
            {
                string strResult = oProyectoCarta.sp_i_proyecto_carta(proyecto_carta);
                oBasic.AlertUserControl(msgProyectoCartaes, msgProyectoCartaesMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "i");
                return strResult.Split(':')[1];
            }
            else
            {
                proyecto_carta.IdProyectoCarta = hdd_idproyecto_carta.Value;
                string strResult = oProyectoCarta.sp_u_proyecto_carta(proyecto_carta);
                oBasic.AlertUserControl(msgProyectoCartaes, msgProyectoCartaesMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "u");
                return hdd_idproyecto_carta.Value;
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
            hdd_idproyecto_carta.Value = "0";
            ViewControls(true);
            Enabled = true;
            oBasic.EnableControls(pnlPBProyectoCartaActions, Enabled, true);
            oBasic.FixPanel(divData, "ProyectoCarta", 2);
        }
        private void ViewButtoms()
        {
            lblPdfCarta.Visible = hdd_ruta_carta.Value.Length > 2;
            lbLLoadCarta.Visible = Enabled;
        }
        private void ViewControls(bool _visible)
        {
            dvProyectoCartaActions.Visible = _visible;
            gvProyectoCartas.Visible = !_visible;
            oBasic.fClearControls(dvProyectoCartaActions);
        }
        private void ViewDelete()
        {
            ViewControls(true);
            Enabled = false;
            LoadDetail();
            MessageBox1.ShowConfirmation("DELETE", "¿Esta seguro que desea eliminar el registro?", type: "danger");
        }
        private void ViewDocument()
        {
            string fileName = hdd_ruta_carta.Value;
            string pathFile = Path.Combine(oVar.prPathDocumentosProyectos.ToString(), fileName);
            oFile.GetPath(pathFile);
            ViewDoc?.Invoke(this);
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