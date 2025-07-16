using GLOBAL.CONST;
using GLOBAL.DAL;
using GLOBAL.LOG;
using GLOBAL.PERMISOS;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using cnsAction = GLOBAL.CONST.clConstantes.Accion;
using cnsEstAsoc = GLOBAL.CONST.clConstantes.PAEstadoAsociativo;
using cnsSection = GLOBAL.CONST.clConstantes.Section;

namespace SIDec.UserControls.Proyectos
{
    public partial class ProyectoInformacion : UserControl
    {
        private readonly PROYECTOS_DAL oProyecto = new PROYECTOS_DAL();

        private readonly clBasic oBasic = new clBasic();
        private readonly clFile oFile = new clFile();
        private readonly clPermisos oPermisos = new clPermisos();
        private readonly clUtil oUtil = new clUtil();
        private readonly clGlobalVar oVar = new clGlobalVar();

        private readonly BANCOPROYECTOS_DAL oBanco = new BANCOPROYECTOS_DAL();
        private readonly DISTRIBUCION_VIVIENDA_DAL oDistribucion = new DISTRIBUCION_VIVIENDA_DAL();
        private readonly IDENTIDADES_DAL oIdentidades = new IDENTIDADES_DAL();
        private readonly clLog oLog = new clLog();
        private readonly LOCALIDADES_DAL oLocalidades = new LOCALIDADES_DAL();
        private readonly PLANESP_DAL oPlanesP = new PLANESP_DAL();
        private readonly UPZ_DAL oUpz = new UPZ_DAL();
        private readonly USUARIOS_DAL oUsuarios = new USUARIOS_DAL();

        private const string _SOURCEPAGE = "ProyectoInformacion";

        public delegate void OnSelectProyectoEventHandler(object sender);
        public event OnSelectProyectoEventHandler SelectedProyecto;

        #region Propiedades
        public string Chip
        {
            get
            {
                return hddProyectoPrimary.Value;
            }
            set
            {
                hddProyectoPrimary.Value = value;
                hdd_chip.Value = hddProyectoPrimary.Value;
            }
        }
        public string IdActor
        {
            get
            {
                return (Session[ControlID + "." + _SOURCEPAGE + ".IdActor"] ?? "0").ToString();
            }
            set
            {

                Session[ControlID + "." + _SOURCEPAGE + ".IdActor"] = Int32.TryParse(value, out int v) ? value : "0";
            }
        }
        public string IdProyecto
        {
            get
            {
                return (Session[ControlID + "." + _SOURCEPAGE + ".IdProyecto"] ?? "0").ToString();
            }
            set
            {
                Session[ControlID + "." + _SOURCEPAGE + ".IdProyecto"] = Int32.TryParse(value, out int v) ? value : "0";
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
                return (Session[ControlID + "." + _SOURCEPAGE + ".Enabled"] ?? "0").ToString() == "1";
            }
            set
            {
                Session[ControlID + "." + _SOURCEPAGE + ".Enabled"] = value ? "1" : "0";
            }
        }
        public string Filter
        {
            get
            {
                return (Session[ControlID + "." + _SOURCEPAGE + ".CriterioBuscar"] ?? "").ToString();
            }
            set
            {
                Session[ControlID + "." + _SOURCEPAGE + ".CriterioBuscar"] = value.ToString();
            }
        }
        public string ResponsibleUserCode
        {
            get
            {
                return (Session[ControlID + "." + _SOURCEPAGE + ".ResponsibleUserCode"] ?? "0").ToString();
            }
            set
            {
                Session[ControlID + "." + _SOURCEPAGE + ".ResponsibleUserCode"] = value;
            }
        }
        /// <summary>
        /// Tipo de vista 0 - todos, 1 - único proyecto
        /// </summary>
        public string ViewType
        {
            get
            {
                return (hddViewType.Value);
            }
            set
            {
                hddViewType.Value = value;
            }
        }

        private object DataSource
        {
            get
            {
                return Session[ControlID + "." + _SOURCEPAGE + ".DataSource"];
            }
            set
            {
                Session[ControlID + "." + _SOURCEPAGE + ".DataSource"] = value;
            }
        }
        private struct Origen
        {
            public const string PLAN_PARCIAL = "PP";
            public const string PREDIO_DECLARADO = "PD";
            public const string PRIVADO_OTRO = "PT";
        }
        #endregion



        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterScript();
            if (!IsPostBack)
            {
                Page.Form.Enctype = "multipart/form-data";

                LoadDropDowns();

                if ((Session["Retorno.Proyecto.Page"] ?? "").ToString() != "")
                {
                    Session["Retorno.ucProyecto.ID"] = null;
                    ViewControls(true);
                    Enabled = false;
                    LoadDetail();
                }
            }
            Initialize();
            EnableButtons();
        }
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            if (ddl_id_resultado_proyecto.SelectedValue == cnsEstAsoc.ESTADO_ASOC_CUMPLIDO || ddl_id_resultado_proyecto.SelectedValue == cnsEstAsoc.ESTADO_ASOC_VENC_TERMI)
            {
                if (txt_au_proyecto.Text == "0")
                {
                    MessageBox1.ShowMessage("El Estado del asociativo «" + ddl_id_resultado_proyecto.SelectedItem.Text + "» no es válido para un proyecto nuevo");
                    return;
                }
                else
                {
                    MessageBox1.ShowConfirmation("EDIT",
                        "El Estado del asociativo «" + ddl_id_resultado_proyecto.SelectedItem.Text + "» genera el bloqueo del proyecto. <br> " +
                        "De continuar, no podrá editar la información posteriormente.<br> " +
                        "¿Está seguro de dar cierre al proyecto?", type: "danger", letHTML: true);
                    return;
                }
            }
            else
            {
                hdd_ruta_archivo.Value = "";
            }
            if (hdd_au_proyecto.Value != "0")
                MessageBox1.ShowConfirmation("EDIT", "¿Está seguro de actualizar la información?", type: "warning");
            else
                MessageBox1.ShowConfirmation("ADD", "¿Está seguro de continuar con la acción solicitada?");
        }
        protected void btnAccion_Click(object sender, EventArgs e)
        {
            btnGoPlanP.Visible = false;
            btnGoPredio.Visible = false;
            LinkButton btnAccionSource = (LinkButton)sender;
            oBasic.AlertMain(msgProyectoMain, "", "0");

            switch (btnAccionSource.CommandName)
            {
                case "Listar":
                    LoadControl();
                    break;
                case "Editar":
                    if (!ValidateAccess(cnsSection.PROYECTO_ASOCIATIVO, cnsAction.EDITAR, true)) return;
                    ViewEdit();
                    break;
                case "Eliminar":
                    if (!ValidateAccess(cnsSection.PROYECTO_ASOCIATIVO, cnsAction.ELIMINAR, true, false)) return;
                    ViewDelete();
                    return;
                case "Seguimiento":
                    if (!ValidateAccess(cnsSection.FICHA_BANCO, cnsAction.CONSULTAR, false, false)) return;
                    GoSeguimiento();
                    return;
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateAccess(cnsSection.PROYECTO_ASOCIATIVO, cnsAction.INSERTAR, true, false)) return;
            ViewAdd();
        }
        protected void BtnAddrArea_Click(object sender, EventArgs e)
        {
            DataTable dt = ViewState["DistribucionViviendas"] as DataTable;
            if (dt != null && dt.Rows.Count > 0)
            {
                DataView dv = dt.AsDataView();
                dv.RowFilter = "tipo_vivienda = " + ddlTipoVivienda.SelectedValue + " and area_m2 = " + txt_area.Text;
                if (dv.ToTable().Rows.Count > 0)
                {
                    MessageInfo.ShowMessage("Tipo de vivienda y Área, ya se encuentra registrada");
                    return;
                }
            }
            DataRow dr = dt.NewRow();
            dr["tipo_vivienda"] = ddlTipoVivienda.SelectedValue;
            dr["area_m2"] = txt_area.Text;
            dr["cantidad_viviendas"] = txt_cantidad.Text;
            dt.Rows.Add(dr);
            ViewState["DistribucionViviendas"] = dt;
            LoadDistribucionViviendas();
            ddlTipoVivienda.SelectedIndex = 0;
            txt_cantidad.Text = txt_area.Text = "";
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Enabled = false;
            if (IdProyecto == "0")
                LoadGrilla();
            else
                LoadDetail();
        }
        protected void btnGoPlanP_Click(object sender, EventArgs e)
        {
            Session["Retorno.ucProyecto.ID"] = IdProyecto;
            Session["Retorno.ucProyecto.ViewType"] = ViewType;
            Session["Retorno.ucProyecto.filter"] = Filter;

            Session["Retorno.PlanesP.Origen"] = "proyectos";
            Session["Proyecto.PlanesP.Id"] = ddl_cod_planp.SelectedValue;
            Response.Redirect("Planesp");
        }
        protected void btnGoPredio_Click(object sender, EventArgs e)
        {
            Session["Retorno.ucProyecto.ID"] = IdProyecto;
            Session["Retorno.ucProyecto.ViewType"] = ViewType;
            Session["Retorno.ucProyecto.filter"] = Filter;

            Session["Retorno.Predios.Origen"] = "proyectos";
            Session["Proyecto.Predios.chip"] = hdd_chip.Value;
            Response.Redirect("Predios");
        }
        protected void ddl_codlocalidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDropDownUPZ();
        }
        protected void ddl_cod_planp_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_nombre_proyecto.Text = "";
            txt_direccion_proyecto.Text = "";
            ddl_cod_localidad.SelectedIndex = 0;
            ddl_idupz.SelectedIndex = 0;
            ddl_id_clasificacion_suelo.SelectedIndex = 0;
            Relationship();
        }
        protected void ddl_id_origen_proyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            Relationship();
            EnableButtons();
        }
        protected void ddl_id_resultado_proyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            dvDocumentoPA.Visible = (ddl_id_resultado_proyecto.SelectedValue == cnsEstAsoc.ESTADO_ASOC_CUMPLIDO ||
                                    ddl_id_resultado_proyecto.SelectedValue == cnsEstAsoc.ESTADO_ASOC_VENC_TERMI);
            ValidPdfProject();
            rfv_InfoFile.Enabled = dvDocumentoPA.Visible;
            //pnlCumplido.Visible = ddl_id_resultado_proyecto.SelectedValue == cnsEstAsoc.ESTADO_ASOC_CUMPLIDO;
            upProyecto.Update();
        }
        protected void lblPdfProject_Click(object sender, EventArgs e)
        {
            string fileName = hdd_ruta_archivo.Value;
            string pathFile = Path.Combine(oVar.prPathDocumentosProyectos.ToString(), fileName);

            oFile.GetPath(pathFile);

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "verDoc();", true);
        }
        protected void gvProyecto_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            oBasic.AlertMain(msgProyectoMain, "", "0");
            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                if (rowIndex >= gvProyecto.Rows.Count)
                    rowIndex = 0;
                LoadDataKeys(rowIndex);

                switch (e.CommandName)
                {
                    case "_Detail":
                        ViewControls(true);
                        Enabled = false;
                        LoadDetail();
                        SelectedProyecto?.Invoke(this);
                        break;
                    default:
                        return;
                }
            }
        }
        protected void gvProyecto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvProyecto, "Select$" + e.Row.RowIndex.ToString()));
                string idEstadoProyecto = gvProyecto.DataKeys[e.Row.DataItemIndex]["id_resultado_proyecto"].ToString();
                if (idEstadoProyecto == cnsEstAsoc.ESTADO_ASOC_CUMPLIDO || idEstadoProyecto == cnsEstAsoc.ESTADO_ASOC_VENC_TERMI)
                {
                    e.Row.ForeColor = System.Drawing.Color.FromArgb(55, 55, 55);
                    e.Row.BackColor = System.Drawing.Color.FromArgb(241, 242, 243);
                    e.Row.Font.Italic = true;
                }
            }
        }
        protected void gvProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            gv_SelectedIndexChanged(gvProyecto);
            LoadDataKeys(gvProyecto.SelectedIndex);
            SelectedProyecto?.Invoke(this);
        }
        protected void gvProyecto_Sorting(object sender, GridViewSortEventArgs e)
        {
            gv_Sorting(gvProyecto, e.SortExpression.ToString(), DataSource);
            DataSource = Session[ControlID + ".prDataSet"];
            SelectedProyecto?.Invoke(this);
        }
        protected void gv_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header)
                return;

            GridView gv = sender as GridView;
            string modulo = gv.ID.Substring(2);
            string sortExpression = (ViewState["SortExp" + modulo] ?? "chip").ToString();
            string sortDirection = (ViewState["SortDir" + modulo] ?? "ASC").ToString();
            foreach (TableCell tableCell in e.Row.Cells)
            {
                if (!tableCell.HasControls())
                    continue;
                if (!(tableCell.Controls[0] is LinkButton lbSort))
                    continue;
                if (lbSort.CommandArgument == sortExpression)
                {
                    Image imageSort = new Image
                    {
                        ImageAlign = ImageAlign.AbsMiddle,
                        Width = 12
                    };
                    imageSort.Style.Add("margin-left", "3px");
                    if (sortDirection == "ASC")
                        imageSort.ImageUrl = "~/Images/icon/up.png";
                    else
                        imageSort.ImageUrl = "~/Images/icon/down.png";
                    tableCell.Controls.Add(imageSort);
                }
            }
        }
        protected void gvTipoVivienda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            DataTable dt = ViewState["DistribucionViviendas"] as DataTable;

            if (dt != null)
            {
                string tipoVivienda = e.CommandName;
                string area_m2 = (tipoVivienda == "1" ? gvVIP.DataKeys[index].Value.ToString() :
                                 (tipoVivienda == "2" ? gvVIS.DataKeys[index].Value.ToString() :
                                 (tipoVivienda == "3" ? gvNoV.DataKeys[index].Value.ToString() : "0")));
                DataRow[] rows = dt.Select($"tipo_vivienda = {tipoVivienda} and area_m2 = {area_m2}");

                if (index >= 0 && rows.Length == 1)
                {
                    dt.Rows.Remove(rows[0]);
                    ViewState["DistribucionViviendas"] = dt;

                    LoadDistribucionViviendas();
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
                        if (!Add())
                            return;
                        break;
                    case "EDIT":
                        if (!Edit())
                            return;
                        break;
                    case "DELETE":
                        Delete();
                        break;
                    default:
                        break;
                }
                LoadControl();
                ViewControls(false);
                gvProyecto.HeaderRow.Focus();
                EnableButtons();
            }
            catch (Exception)
            {

            }
        }
        protected void txt_chipfind_TextChanged(object sender, EventArgs e)
        {
            Relationship();
        }
        #endregion



        #region Métodos Públicos 
        public void LoadControl()
        {
            if (!ValidateAccess(cnsSection.PROYECTO_ASOCIATIVO, cnsAction.CONSULTAR, false, false))
            {
                IdProyecto = "-100";
                IdActor = "-100";
                gvProyecto.EmptyDataText = "No cuenta con permisos suficientes para realizar esta acción";

                LoadGrilla();
                return;
            }

            LoadGrilla();
            upProyecto.Update();
        }
        #endregion



        #region Métodos Privados
        private void Clear()
        {
            IdProyecto = "0";
            oBasic.fClearControls(mvProyectosSection);
            if (((DataSet)DataSource).Tables.Count > 0)
            {
                DataRow dRow = ((DataSet)DataSource).Tables[0].NewRow();
                oBasic.fValueControls(mvProyectosSection, dRow);
            }

            lblPdfProject.Visible = false;
            lbLoadProject.Visible = false;
        }
        public string DataTableToDelimitedString(DataTable table)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in table.Rows)
            {
                sb.Append(row["tipo_vivienda"].ToString());
                sb.Append("|");
                sb.Append(row["area_m2"].ToString());
                sb.Append("|");
                sb.Append(row["cantidad_viviendas"].ToString());
                sb.Append(";");
            }
            return sb.ToString().TrimEnd(';');
        }

        private string Delete()
        {
            if (!ValidateAccess(cnsSection.PROYECTO_ASOCIATIVO, cnsAction.ELIMINAR, true, false)) return "";

            string strResult = oProyecto.sp_d_proyecto(hdd_au_proyecto.Value);

            if (strResult.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                return clConstantes.DB_ACTION_OK;
            else
                return clConstantes.DB_ACTION_ERR_DATOS;
        }
        private bool Add()
        {

            if (!ValidateAccess(cnsSection.PROYECTO_ASOCIATIVO, cnsAction.INSERTAR, true, false)) return false; 
            
            ReadAreasZones();
            string strResultado = oProyecto.sp_i_proyecto(txt_nombre_proyecto.Text, oBasic.fInt(ddl_cod_planp), txt_chipfind.Text, txt_direccion_proyecto.Text, 
                ddl_cod_localidad.Text, ddl_idupz.Text, oBasic.fInt(ddl_id_origen_proyecto), oBasic.fInt(ddl_id_clasificacion_suelo), oBasic.fInt(ddl_id_destino_catastral), 
                oBasic.fInt(ddl_id_tratamiento_urbanistico), oBasic.fInt(ddl_id_instrumento_gestion), oBasic.fInt(ddl_id_instrumento_desarrollo),
                oBasic.fPerc(txt_porc_SE_total), oBasic.fInt(ddl_id_estado_proyecto), oBasic.fInt(ddl_id_resultado_proyecto), txt_areas_zonas.Text, 
                oBasic.fDec(txt_area_bruta), oBasic.fDec(txt_area_neta_urbanizable), oBasic.fDec(txt_area_util), oBasic.fInt(txt_UP_VIP), oBasic.fInt(txt_UP_VIS), 
                oBasic.fInt(txt_UP_no_VIS), oBasic.fInt(txt_UE_VIP), oBasic.fInt(txt_UE_VIS), oBasic.fInt(txt_UE_no_VIS), oBasic.fInt(txt_empleos), 
                oBasic.fInt(txt_inversion), txt_fecha_inicio_ventas.Text,  txt_fecha_inicio_obras.Text, txt_observacion.Text, oBasic.fInt(ddl_cod_usu_responsable) );
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                SaveAreas();
                oLog.RegistrarLogInfo(_SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, clConstantes.MSG_OK_U);
            }
            else
            {
                MessageInfo.ShowMessage(strResultado.Substring(6));
                return false;
            }
            return true;
        }
        private bool Edit()
        {
            string strValidacion = clConstantes.DB_ACTION_OK;

            var fileName = "";

            ReadAreasZones();
            if (ddl_id_resultado_proyecto.SelectedValue == cnsEstAsoc.ESTADO_ASOC_CUMPLIDO || ddl_id_resultado_proyecto.SelectedValue == cnsEstAsoc.ESTADO_ASOC_VENC_TERMI)
            {
                fileName = hdd_ruta_archivo.Value;
                strValidacion = oProyecto.sp_v_proyecto_validar(oBasic.fInt(txt_au_proyecto));
                if (strValidacion.Substring(0, 5) != clConstantes.DB_ACTION_OK)
                {
                    ddl_id_resultado_proyecto.SelectedValue = cnsEstAsoc.ESTADO_ASOC_EN_PROCESO;
                }
            }

            if (fuLoadProject.HasFile)
            {
                string contentType = fuLoadProject.ContentType;
                if (contentType == "application/pdf")
                {
                    fileName = "CCPY_" + txt_au_proyecto.Text + "_" + Guid.NewGuid() + Path.GetExtension(fuLoadProject.FileName);
                    string pathFile = Path.Combine(oVar.prPathDocumentosProyectos.ToString(), fileName);
                    try
                    {
                        fuLoadProject.SaveAs(pathFile);
                    }
                    catch (Exception e)
                    {
                        oLog.RegistrarLogError("Error subiendo archivo " + e.Message + ":" + fuLoadProject.FileName + ":::" + fuLoadProject.PostedFile.ContentLength, _SOURCEPAGE, "ProyectosEdit");
                    }
                }
            }

            string strResult = oProyecto.sp_u_proyecto(oBasic.fInt(txt_au_proyecto), txt_nombre_proyecto.Text, oBasic.fInt(ddl_cod_planp), txt_chipfind.Text,
                txt_direccion_proyecto.Text, ddl_cod_localidad.Text, ddl_idupz.Text, oBasic.fInt(ddl_id_origen_proyecto), oBasic.fInt(ddl_id_clasificacion_suelo),
                oBasic.fInt(ddl_id_destino_catastral), oBasic.fInt(ddl_id_tratamiento_urbanistico), oBasic.fInt(ddl_id_instrumento_gestion), oBasic.fInt(ddl_id_instrumento_desarrollo),
                oBasic.fPerc(txt_porc_SE_total), oBasic.fInt(ddl_id_estado_proyecto), oBasic.fInt(ddl_id_resultado_proyecto), txt_areas_zonas.Text, oBasic.fDec(txt_area_bruta),
                oBasic.fDec(txt_area_neta_urbanizable), oBasic.fDec(txt_area_util), oBasic.fInt(txt_UP_VIP), oBasic.fInt(txt_UP_VIS), oBasic.fInt(txt_UP_no_VIS),
                oBasic.fInt(txt_UE_VIP), oBasic.fInt(txt_UE_VIS), oBasic.fInt(txt_UE_no_VIS), oBasic.fInt(txt_empleos), oBasic.fInt(txt_inversion), txt_fecha_inicio_ventas.Text,
                txt_fecha_inicio_obras.Text, txt_observacion.Text, oBasic.fInt(ddl_cod_usu_responsable), fileName, null, null, null);

            if (strResult.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                SaveAreas();

                oLog.RegistrarLogInfo(_SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, clConstantes.MSG_OK_U);
                if (strValidacion.Substring(0, 5) != clConstantes.DB_ACTION_OK)
                {
                    Enabled = false;
                    LoadDetail();
                    EnableButtons();
                    MessageInfo.ShowMessage(strValidacion.Substring(6), type: "info", title: "Operación Fallida");
                    return false;
                }
            }
            else
            {
                MessageInfo.ShowMessage(strResult.Substring(6));
                return false;
            }
            return true;
        }
        private void EnableButtons()
        {
            divPlanParcialFind.Visible = (GetOrigen() == Origen.PLAN_PARCIAL);
            upProyecto.Update();

            txt_au_proyecto.Enabled = false;

            btnGoPlanP.Visible = !Enabled && (ddl_cod_planp.SelectedIndex > 0);
            btnGoPredio.Visible = !Enabled && (hdd_chip.Value.Length > 1);
            btnGoPlanP.Enabled = !Enabled;
            btnGoPredio.Enabled = !Enabled;

            lblPdfProject.Enabled = true;
            lbLoadProject.Visible = Enabled;
            dvTiposVivienda.Visible = Enabled;
            gvVIP.Columns[3].Visible = Enabled;
            gvVIS.Columns[3].Visible = Enabled;
            gvNoV.Columns[3].Visible = Enabled;
        }
        private string GetOrigen()
        {
            switch (ddl_id_origen_proyecto.SelectedValue)
            {
                case "422":
                case "423":
                    return Origen.PLAN_PARCIAL;
                case "424":
                case "425":
                    return Origen.PREDIO_DECLARADO;
                default:
                    return Origen.PRIVADO_OTRO;
            }
        }
        private void GoSeguimiento()
        {
            Session["Retorno.ucProyecto.ID"] = IdProyecto;
            Session["Retorno.ucProyecto.ViewType"] = ViewType;
            Session["Retorno.ucProyecto.filter"] = Filter;

            Session["Retorno.Banco.Origen"] = "proyectos";
            Session["Proyecto.Banco.Id"] = hdd_idBanco.Value;
            Response.Redirect("Banco");
        }
        protected void gv_SelectedIndexChanged(GridView gv)
        {
            string modulo = gv.ID.Substring(2);
            ViewState["Index" + modulo] = ((gv.PageIndex * gv.PageSize) + gv.SelectedIndex).ToString();
        }
        protected void gv_Sorting(GridView gv, string sortExpression, object ds)
        {
            string modulo = gv.ID.Substring(2);
            string expression = "", direction = "", sort = "";

            DataView dataView = new DataView(((DataSet)(ds)).Tables[0]);
            string[] expressions = (ViewState["SortExp" + modulo] ?? "esresponsable,completo_PA,au_proyecto").ToString().Split(',');
            string[] directions = (ViewState["SortDir" + modulo] ?? "").ToString().Split(',');

            if (sortExpression.Length > 0)
            {
                expression = sortExpression;
                direction = expressions[0] == sortExpression && directions[0] == "ASC" ? "DESC" : "ASC";
                sort = expression + " " + direction;
            }

            for (int i = 0; i < expressions.Length; i++)
                if (sortExpression != expressions[i] && expressions[i].Trim().Length > 0)
                {
                    direction = (direction.Length > 1 ? direction + "," : "") + (directions.Length > i ? directions[i].Length > 0 ? directions[i] : "ASC" : "ASC");
                    expression = (expression.Length > 1 ? expression + "," : "") + expressions[i];
                    sort = (sort.Length > 1 ? sort + ", " : "") + expressions[i] + " " + (directions.Length > i ? directions[i].Length > 0 ? directions[i] : "ASC" : "ASC");
                }

            ViewState["SortExp" + modulo] = expression;
            ViewState["SortDir" + modulo] = direction;

            dataView.Sort = sort;

            gv.SelectedIndex = 0;
            gv.DataSource = dataView;
            gv.DataBind();
            Session[ControlID + ".prDataSet"] = oUtil.ConvertToDataSet(dataView);
            gv_SelectedIndexChanged(gv);
            LoadDataKeys(0);
        }
        private void Initialize()
        {
            cal_fecha_inicio_ventas.StartDate = cal_fecha_inicio_obras.StartDate = new DateTime(2008, 1, 1);  //cal_fecha_cumple.StartDate = 
            rv_fecha_inicio_ventas.MinimumValue = rv_fecha_inicio_obras.MinimumValue = (new DateTime(2008, 1, 1)).ToString("yyyy-MM-dd");

            cal_fecha_inicio_ventas.EndDate = cal_fecha_inicio_obras.EndDate = DateTime.Today; //cal_fecha_cumple.EndDate = 
            rv_fecha_inicio_ventas.MaximumValue = rv_fecha_inicio_obras.MaximumValue = (DateTime.Today).ToString("yyyy-MM-dd");
        }
        private void LoadDropDownResponsibleUser(DataRow dRow)
        {
            DataSet ds = oUsuarios.sp_s_usuarios_filtro(12, Convert.ToInt32(oVar.prUserCod.ToString()));
            if (ds != null)
            {
                DataSet ds2 = ds.Clone();
                DataRow[] oDr = ds.Tables[0].Select(" cod_usuario in (" + (dRow != null ? (dRow["cod_usu_responsable"] ?? "0").ToString() : "0") + ", " + oVar.prUserCod.ToString() + ")");

                foreach (DataRow row in oDr)
                {
                    ds2.Tables[0].ImportRow(row);
                }
                ddl_cod_usu_responsable.Items.Clear();
                ddl_cod_usu_responsable.DataSource = ds2;
                ddl_cod_usu_responsable.DataTextField = "nombre_completo";
                ddl_cod_usu_responsable.DataValueField = "cod_usuario";
                ddl_cod_usu_responsable.DataBind();

                ddl_cod_usu_responsable.Items.Insert(0, new ListItem("--Seleccione opción", "0"));

                ListItem li = ddl_cod_usu_responsable.Items.FindByValue(dRow != null ? (dRow["cod_usu_responsable"] ?? "0").ToString() : "0");
                Session["Proyectos.Cod_Usu_Responsable"] = (dRow != null ? (dRow["cod_usu_responsable"] ?? "0").ToString() : "0");
                ddl_cod_usu_responsable.SelectedValue = li != null ? li.Value : "0";
            }
        }
        private void LoadDataKeys(int rowIndex)
        {
            IdProyecto = rowIndex >= 0 ? gvProyecto.DataKeys[rowIndex]["au_proyecto"].ToString() : "0";
            IdActor = rowIndex >= 0 ? gvProyecto.DataKeys[rowIndex]["idactor"].ToString() : "0";
            ResponsibleUserCode = rowIndex >= 0 ? gvProyecto.DataKeys[rowIndex]["cod_usu_responsable"].ToString() : "0";
        }
        private void LoadDetail()
        {
            DataView dv = ((DataSet)DataSource).Tables[0].DefaultView;
            dv.RowFilter = "au_proyecto = '" + IdProyecto + "'";

            if (dv != null && dv.Count > 0)
            {
                DataRow dRow = dv[0].Row;

                oBasic.fValueControls(pnlPBProyectoActions, dRow);
                LoadDropDownUPZ();

                if (fuLoadProject.HasFile)
                    fuLoadProject.ClearAllFilesFromPersistedStore();
                dvDocumentoPA.Visible = (hdd_ruta_archivo.Value != "" ||
                                            ddl_id_resultado_proyecto.SelectedValue == cnsEstAsoc.ESTADO_ASOC_CUMPLIDO ||
                                            ddl_id_resultado_proyecto.SelectedValue == cnsEstAsoc.ESTADO_ASOC_VENC_TERMI);
                lblPdfProject.Visible = hdd_ruta_archivo.Value != "";
                lblInfoFileProject.Text = "";
                //pnlCumplido.Visible = ddl_id_resultado_proyecto.SelectedValue == cnsEstAsoc.ESTADO_ASOC_CUMPLIDO;

                oBasic.fDetalleDropDown(ddl_idupz, dRow["idupz"].ToString());

                LoadProyectoAreasZones();
                Relationship();
                LoadDropDownResponsibleUser(dRow);
                LoadSeguimiento();
            }

            oBasic.EnableControls(mvProyectosSection, Enabled, true);
            Relationship();
            oBasic.FixPanel(divData, "Proyecto", Enabled ? 2 : 0, pList: true);
            ViewBotons();
        }
        private void LoadGrilla()
        {
            Filter = string.IsNullOrEmpty(Filter) ? "%" : Filter;
            string usuario = (string)((oPermisos.TienePermisosAccion(cnsSection.PROYECTO_ASOCIATIVO, cnsAction.CONSULTAR, "", oVar.prUserCod.ToString())) ? "" : oVar.prUserCod.ToString());
            string PROY_ASOCIATIVO = "414";

            DataSet ds = oProyecto.sp_s_proyectos_nombre(Filter, PROY_ASOCIATIVO, usuario);
            DataSource = ds;

            gvProyecto.DataSource = (DataSet)DataSource;
            gvProyecto.DataBind();

            if (gvProyecto.Rows.Count > 0)
            {
                int index = 0;
                string idProyecto = IdProyecto.ToString();
                gv_Sorting(gvProyecto, "", DataSource);
                foreach (GridViewRow row in gvProyecto.Rows)
                    if (gvProyecto.DataKeys[row.RowIndex]["au_proyecto"].ToString() == idProyecto)
                        index = row.RowIndex;
                LoadDataKeys(index);
                gvProyecto.SelectedIndex = index;
                DataSource = Session[ControlID + ".prDataSet"];
            }
            else
            {
                IdProyecto = "0";
                IdActor = "0";
            }
            SelectedProyecto?.Invoke(this);
            ViewControls(false);

            oBasic.FixPanel(divData, "Proyecto", 0, pAdd: false, pEdit: false, pDelete: false);
            ViewBotons();
        }
        private void LoadDropDowns()
        {
            ddl_cod_localidad.DataSource = oLocalidades.sp_s_localidades();
            ddl_cod_localidad.DataTextField = "nombre_localidad";
            ddl_cod_localidad.DataValueField = "cod_localidad";
            ddl_cod_localidad.DataBind();

            ddl_cod_planp.DataSource = oPlanesP.sp_s_planesp_lista(true);
            ddl_cod_planp.DataTextField = "nombre_planp";
            ddl_cod_planp.DataValueField = "au_planp";
            ddl_cod_planp.DataBind();

            LoadDropDownIdentidad(ddl_id_origen_proyecto, "57");
            LoadDropDownIdentidad(ddl_id_resultado_proyecto, "58");
            LoadDropDownIdentidad(ddl_id_destino_catastral, "53");
            LoadDropDownIdentidad(ddl_id_tratamiento_urbanistico, "21", "3"); //3-ignore las opciones exclusivas de Proyectos estratégicos
            LoadDropDownIdentidad(ddl_id_instrumento_gestion, "51", "3"); //3-ignore las opciones exclusivas de Proyectos estratégicos
            LoadDropDownIdentidad(ddl_id_instrumento_desarrollo, "52");
            LoadDropDownIdentidad(ddl_id_clasificacion_suelo, "31");
            LoadDropDownIdentidad(ddl_id_estado_proyecto, "55", "3"); //3-ignore las opciones exclusivas de Proyectos estratégicos
            //LoadDropDownIdentidad(ddl_id_como_cumple, "80");
        }
        private void LoadDropDownIdentidad(DropDownList ddl, string id, string filterIgnore = "")
        {
            DataView dv = oIdentidades.sp_s_identidad_id_categoria(id).Tables[0].DefaultView;
            dv.RowFilter = filterIgnore == "" ? "" : "opcion_identidad is null or opcion_identidad not in (" + filterIgnore + ")";
            ddl.DataSource = dv;
            ddl.DataTextField = "nombre_identidad";
            ddl.DataValueField = "id_identidad";
            ddl.DataBind();
        }
        private void LoadDropDownUPZ()
        {
            ddl_idupz.Items.Clear();
            ddl_idupz.DataSource = oUpz.sp_s_upz(ddl_cod_localidad.SelectedValue);
            ddl_idupz.DataTextField = "nombre_upz";
            ddl_idupz.DataValueField = "cod_upz";

            ddl_idupz.Items.Insert(0, new ListItem("-- Seleccione opción", "0"));
            ddl_idupz.DataBind();
        }
        private void LoadDistribucionViviendas()
        {
            DataTable dt;
            if (ViewState["DistribucionViviendas"] != null)
                dt = (DataTable)ViewState["DistribucionViviendas"];
            else
            {
                dt = oDistribucion.sp_s_distribucion_vivienda(IdProyecto).Tables[0];
                ViewState["DistribucionViviendas"] = dt;
            }

            DataView dvVIP = dt.AsDataView();
            DataView dvVIS = dt.AsDataView();
            DataView dvNoV = dt.AsDataView();

            dvVIP.RowFilter = "tipo_vivienda = 1";
            dvVIS.RowFilter = "tipo_vivienda = 2";
            dvNoV.RowFilter = "tipo_vivienda = 3";

            dvVIP.Sort = dvVIS.Sort = dvNoV.Sort = "area_m2";

            gvVIP.DataSource = dvVIP;
            gvVIP.DataBind();
            gvVIS.DataSource = dvVIS;
            gvVIS.DataBind();
            gvNoV.DataSource = dvNoV;
            gvNoV.DataBind();
        }
        private void LoadProyectoAreasZones()
        {
            txt_areas_zonas_desc.Text = "";
            for (int i = 1; i <= 23; i++)
            {
                CheckBox chkControl = (CheckBox)divAreasZonas.FindControl("chk_az_" + i.ToString());
                try
                {
                    if (txt_areas_zonas.Text.Contains(" " + i.ToString() + ", "))
                    {
                        chkControl.Checked = true;
                        txt_areas_zonas_desc.Text += chkControl.Text + ", ";
                    }
                    else
                        chkControl.Checked = false;
                }
                catch { }
            }
            if (txt_areas_zonas_desc.Text.Length > 0)
                txt_areas_zonas_desc.Text = txt_areas_zonas_desc.Text.Substring(0, txt_areas_zonas_desc.Text.Length - 2);

            LoadDistribucionViviendas();
        }
        private void LoadSeguimiento()
        {
            DataSet dSet = oBanco.sp_s_banco_consultar(0, Int32.Parse(IdProyecto));
            if (dSet.Tables.Count > 0 && dSet.Tables[0].Rows.Count > 0 && dSet.Tables[0].Rows[0]["estado"].ToString() == "1")
            {
                DataRow dRow = dSet.Tables[0].Rows[0];
                hdd_idBanco.Value = dRow["idbanco"].ToString();
                btnProyectoSeg.Visible = true;
            }
            else
                btnProyectoSeg.Visible = false;
        }
        private void ReadAreasZones()
        {
            txt_areas_zonas.Text = " ";
            for (int i = 1; i <= 23; i++)
            {
                CheckBox chkControl = (CheckBox)divAreasZonas.FindControl("chk_az_" + i.ToString());
                try
                {
                    if (chkControl.Checked)
                    {
                        txt_areas_zonas.Text += i.ToString() + ", ";
                    }
                }
                catch { }
            }
        }
        private void RegisterScript()
        {
            Page.Form.Enctype = "multipart/form-data";
            lbLoadProject.Attributes.Add("onclick", "$(\"input[ID*='" + fuLoadProject.ClientID + "']\").click();return false;");

            string key = "LoadFileProject";
            StringBuilder scriptSlider = new StringBuilder();
            scriptSlider.Append("<script type='text/javascript'> ");
            scriptSlider.Append("   function infoFileProject() {  ");
            scriptSlider.Append("       var name = $(\"input[ID*='" + fuLoadProject.ClientID + "']\").val(); ");
            scriptSlider.Append("       $('#" + lblPdfProject.ClientID + "').hide(); ");

            scriptSlider.Append("       name = name.substring(name.lastIndexOf('\\\\') + 1); ");
            scriptSlider.Append("       var ext = name.substring(name.lastIndexOf('.') + 1).toLowerCase(); ");

            scriptSlider.Append("       if(ext !== 'pdf'){");
            scriptSlider.Append("          $('#" + lblInfoFileProject.ClientID + "').html(''); ");
            scriptSlider.Append("          $('#" + txt_ruta_archivo.ClientID + "').val(''); ");
            scriptSlider.Append("           $('#" + lblErrorFileProject.ClientID + "').html('Extensión inválida, solo se permite «pdf»');} ");
            scriptSlider.Append("       else {   $('#" + lblErrorFileProject.ClientID + "').html(''); ");
            scriptSlider.Append("          $('#" + lblInfoFileProject.ClientID + "').html(name); ");
            scriptSlider.Append("          $('#" + txt_ruta_archivo.ClientID + "').val(name); ");
            scriptSlider.Append("          Page_ClientValidate('vgProyectos');}}");

            scriptSlider.Append(" </script> ");

            if (!Page.ClientScript.IsStartupScriptRegistered(key))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), key, scriptSlider.ToString(), false);
            }

            key = "disabledlink";
            scriptSlider = new StringBuilder();
            scriptSlider.Append("<script type='text/javascript'> ");
            scriptSlider.Append("   $(document).ready(function () {");
            scriptSlider.Append("   disabledLink('tab');  ");
            scriptSlider.Append("       select_tab('tab', 0); ");
            scriptSlider.Append("    });");
            scriptSlider.Append(" </script> ");

            if (!Page.ClientScript.IsStartupScriptRegistered(key))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), key, scriptSlider.ToString(), false);
            }

        }
        private void Relationship()
        {
            int.TryParse(GetOrigen() == Origen.PLAN_PARCIAL ? ddl_cod_planp.SelectedValue : "-1", out int codPlanP);
            string chip = txt_chipfind.Text;
            if (codPlanP > 0 || chip != "")
            {
                DataSet dsProyecPlan = oPlanesP.sp_s_proyecto_x_planesp(Convert.ToInt32(codPlanP), chip);
                if (dsProyecPlan != null && dsProyecPlan.Tables.Count > 0 && dsProyecPlan.Tables[0].Rows.Count > 0)
                {
                    DataTable dtProyecPlan = dsProyecPlan.Tables[0];
                    txt_nombre_proyecto.Text = dtProyecPlan.Rows[0]["nombre"].ToString().Trim() != "" && txt_nombre_proyecto.Text.Trim() == "" ? dtProyecPlan.Rows[0]["nombre"].ToString().Trim() : txt_nombre_proyecto.Text.Trim();
                    txt_direccion_proyecto.Text = dtProyecPlan.Rows[0]["direccion"].ToString().Trim() == "" ? txt_direccion_proyecto.Text : dtProyecPlan.Rows[0]["direccion"].ToString().Trim();
                    hdd_chip.Value = dtProyecPlan.Rows[0]["chip"].ToString().Trim();
                    if (dtProyecPlan.Rows[0]["idlocalidad"].ToString() != "")
                    {
                        oBasic.fDetalle(ddl_cod_localidad, dtProyecPlan.Rows[0]["idlocalidad"].ToString());
                        LoadDropDownUPZ();
                        if (dtProyecPlan.Rows[0]["idupz"].ToString() != "") oBasic.fDetalle(ddl_idupz, dtProyecPlan.Rows[0]["idupz"].ToString());
                    }
                    if (dtProyecPlan.Rows[0]["id_tipo_tratamiento"].ToString() != "") oBasic.fDetalle(ddl_id_tratamiento_urbanistico, dtProyecPlan.Rows[0]["id_tipo_tratamiento"].ToString());
                    if (dtProyecPlan.Rows[0]["id_clasificacion_suelo"].ToString() != "") oBasic.fDetalle(ddl_id_clasificacion_suelo, dtProyecPlan.Rows[0]["id_clasificacion_suelo"].ToString());

                    if (codPlanP > 0)
                    {
                        oBasic.fValueControls(divAreas, dtProyecPlan.Rows[0]);
                        oBasic.fValueControls(divViviendas, dtProyecPlan.Rows[0]);
                        txt_areas_zonas.Text = dtProyecPlan.Rows[0]["areas_zonas"].ToString();
                        LoadProyectoAreasZones();
                    }
                }
            }
            if (!Enabled)
                return;
            oBasic.StyleCtl("E", (codPlanP <= 0 && (chip == "" || hdd_chip.Value.Trim() == "")), txt_direccion_proyecto, false);
            oBasic.StyleCtl("E", (codPlanP <= 0 && (chip == "" || hdd_chip.Value.Trim() == "")), ddl_cod_localidad, false);
            oBasic.StyleCtl("E", (codPlanP <= 0 && (chip == "" || hdd_chip.Value.Trim() == "")), ddl_idupz, false);

            oBasic.StyleCtl("E", (codPlanP <= 0), ddl_id_clasificacion_suelo, false);
            oBasic.StyleCtl("E", (codPlanP <= 0), ddl_id_tratamiento_urbanistico, false);
            oBasic.EnableControls(divAreas, (codPlanP <= 0), true);
            oBasic.EnableControls(divViviendas, (codPlanP <= 0), true);
            oBasic.EnableControls(divAreasZonas, (codPlanP <= 0), true);
        }
        private void SaveAreas()
        {
            DataTable dt = ViewState["DistribucionViviendas"] as DataTable;
            if (dt != null && dt.Rows.Count > 0)
            {
                string delimitedData = DataTableToDelimitedString(dt);
                oDistribucion.sp_i_distribucion_vivienda(IdProyecto, delimitedData);
            }
        }
        protected void ValidPdfProject()
        {
            if (fuLoadProject.HasFile)
            {
                string EXCEPTION_EXTENSION = "Extensión inválida, solo se permite «pdf»";

                string extension = Path.GetExtension(fuLoadProject.FileName).Substring(1);
                bool isValidExtension = extension.ToLower() == "pdf";

                if (!isValidExtension)
                {
                    lblErrorFileProject.Text = EXCEPTION_EXTENSION;
                    lblInfoFileProject.Text = "";
                    fuLoadProject.ClearAllFilesFromPersistedStore();
                }
                else
                {
                    lblErrorFileProject.Text = "";
                    lblInfoFileProject.Text = fuLoadProject.FileName;
                }
            }
            else
            {
                lblErrorFileProject.Text = "";
            }
        }
        private bool ValidateAccess(string section, string action, bool validateResponsible = false, bool requeridedResponsible = true)
        {
            string message = oPermisos.ValidateAccess(section, action, ResponsibleUserCode, validateResponsible, requeridedResponsible);

            if (message == "")
                return true;

            MessageInfo.ShowMessage(message);
            return false;
        }
        private void ViewAdd()
        {
            Clear();
            LoadDropDownResponsibleUser(null);
            ViewControls(true);
            Enabled = true;
            oBasic.EnableControls(mvProyectosSection, Enabled, true);
            EnableButtons();
            oBasic.FixPanel(divData, "Proyecto", Enabled ? 2 : 0, pList: true);
            gvProyecto.SelectedIndex = -1;
            SelectedProyecto?.Invoke(this);
        }
        private void ViewBotons()
        {
            string section = cnsSection.PROYECTO_ASOCIATIVO;
            btnProyectoEdit.Visible = btnProyectoEdit.Visible && oPermisos.TienePermisosAccion(section, cnsAction.EDITAR);
            btnProyectoDel.Visible = btnProyectoDel.Visible && oPermisos.TienePermisosAccion(section, cnsAction.ELIMINAR);
            bool editTotal = oPermisos.TienePermisosAccion(section, cnsAction.EDITAR, customer: oVar.prUserCod.ToString());
            if (!(btnProyectoEdit.Visible && (oPermisos.TienePermisosAccion(section, cnsAction.INSERTAR) || btnProyectoDel.Visible || editTotal)))
                for (int i = 1; i <= 8; i++)
                {
                    Control div = this.FindControl("dvNoDetail" + i.ToString());
                    if (div != null)
                        div.Visible = false;
                }
            EnableButtons();
        }
        private void ViewControls(bool _visible)
        {
            dvProyectoActions.Visible = _visible;
            gvProyecto.Visible = !_visible;
            oBasic.fClearControls(dvProyectoActions);
            ViewState["DistribucionViviendas"] = null;
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

            oBasic.EnableControls(mvProyectosSection, Enabled, true);
            Relationship();
        }

        #endregion
    }
}