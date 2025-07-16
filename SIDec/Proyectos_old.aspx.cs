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
using cnsSection = GLOBAL.CONST.clConstantes.Section;
using cnsEstAsoc = GLOBAL.CONST.clConstantes.PAEstadoAsociativo;
using tipoArchivo = GLOBAL.CONST.clConstantes.TipoArchivo;

namespace SIDec
{
    public partial class Proyectos_old : Page
    {
        #region-----------------------Atributos
        readonly BANCOPROYECTOS_DAL oBanco = new BANCOPROYECTOS_DAL();
        readonly LOCALIDADES_DAL oLocalidades = new LOCALIDADES_DAL();
        readonly UPZ_DAL oUpz = new UPZ_DAL();
        readonly IDENTIDADES_DAL oIdentidades = new IDENTIDADES_DAL();
        readonly PLANESP_DAL oPlanesP = new PLANESP_DAL();

        readonly PROYECTOS_DAL oProyectos = new PROYECTOS_DAL();
        readonly PROYECTOSPREDIOS_DAL oProyectosPredios = new PROYECTOSPREDIOS_DAL();
        readonly PROYECTOSCARTAS_DAL oProyectosCartas = new PROYECTOSCARTAS_DAL();
        readonly PROYECTOSLICENCIAS_DAL oProyectosLicencias = new PROYECTOSLICENCIAS_DAL();
        readonly USUARIOS_DAL oUsuarios = new USUARIOS_DAL();

        readonly clGlobalVar oVar = new clGlobalVar();
        readonly clUtil oUtil = new clUtil();
        readonly clLog oLog = new clLog();
        readonly clFile oFile = new clFile();
        readonly clPermisos oPermisos = new clPermisos();
        private readonly clBasic oBasic = new clBasic();

        private const string _SOURCEPAGE = "Proyectos";
        private struct Origen
        {
            public const string PLAN_PARCIAL = "PP";
            public const string PREDIO_DECLARADO = "PD";
            public const string PRIVADO_OTRO = "PT";
        }
        #endregion

        #region---General
        #region---Events
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterScript();

            if (!IsPostBack)
            {
                Page.Form.Enctype = "multipart/form-data";

                ViewState["CriterioBuscar"] = "";
                ViewState["AccionFinal"] = "";

                ViewState["SortExpProyectos"] = "esresponsable,completo_PA,au_proyecto";
                ViewState["SortExpProyectosPredios"] = "chip";
                ViewState["SortExpProyectosCartas"] = "fecha_radicado_manifestacion_interes";
                ViewState["SortExpProyectosLicencias"] = "origen";
                ViewState["SortExpProyectosActores"] = "documento";

                txtBuscar.Focus();
                LoadDropDowns();
                ProyectosLoad("");
                upProyectosSection.Update();
            }
            ValidateSP();
            Initialize();

            EnableButtons();
            ValidPdfProject();
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            mvProyectos.ActiveViewIndex = 0;
            ViewState["CriterioBuscar"] = txtBuscar.Text.Trim();
            ProyectosLoad(txtBuscar.Text);
            gvProyectos_SelectedIndexChanged(null, null);
        }
        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            bool permiso = false;
            //PROYECTOS
            if (ViewState["AccionFinal"].ToString().Substring(0, 3) == "pry")
            {
                switch (ViewState["AccionFinal"].ToString().Substring(3))
                {
                    case "Editar":
                        ProyectosEdit();
                        permiso = true;
                        break;
                    case "Agregar":
                        ProyectosAdd();
                        permiso = true;
                        break;
                    case "Eliminar":
                        ProyectosDelete();
                        permiso = true;
                        break;
                }
                if (!permiso)
                {
                    oBasic.AlertMain(msgMain, clConstantes.MSG_ERR_PERMISO, "danger");
                    oBasic.AlertSection(msgProyectos, "", "0");
                }
                ProyectosLoad(ViewState["CriterioBuscar"].ToString());
                gv_SelectedIndexChanged(gvProyectos);
                ProyectosLoadChildGrid(gvProyectos.SelectedDataKey["au_proyecto"].ToString());
                if (gvProyectos.SelectedIndex >= 0)
                    lblProyectosSection.Text = gvProyectos.Rows[gvProyectos.SelectedIndex].Cells[1].Text;
                hddProyEstado.Value = "0";
            }
            //PROYECTOS_Predios
            else if (ViewState["AccionFinal"].ToString().Substring(0, 3) == "pre")
            {
                switch (ViewState["AccionFinal"].ToString().Substring(3))
                {
                    case "Editar":
                        if (!ValidateAccess(cnsSection.PROY_ASOC_PREDIOS, cnsAction.EDITAR, true)) return;
                        PrediosEdit();
                        break;
                    case "Agregar":
                        if (!ValidateAccess(cnsSection.PROY_ASOC_PREDIOS, cnsAction.INSERTAR, true)) return;
                        PrediosAdd();
                        break;
                    case "Eliminar":
                        if (!ValidateAccess(cnsSection.PROY_ASOC_PREDIOS, cnsAction.ELIMINAR, true)) return;
                        PrediosDelete();
                        break;
                }
                PrediosLoad(gvProyectos.SelectedDataKey["au_proyecto"].ToString());
                gv_SelectedIndexChanged(gvProyectosPredios);
            }
            //PROYECTOS_Cartas
            else if (ViewState["AccionFinal"].ToString().Substring(0, 3) == "car")
            {
                switch (ViewState["AccionFinal"].ToString().Substring(3))
                {
                    case "Editar":
                        if (!ValidateAccess(cnsSection.PROY_ASOC_CARTAS_INTENCION, cnsAction.EDITAR, true)) return;
                        CartasEdit();
                        break;
                    case "Agregar":
                        if (!ValidateAccess(cnsSection.PROY_ASOC_CARTAS_INTENCION, cnsAction.INSERTAR, true)) return;
                        CartasAdd();
                        break;
                    case "Eliminar":
                        if (!ValidateAccess(cnsSection.PROY_ASOC_CARTAS_INTENCION, cnsAction.ELIMINAR, true)) return;
                        CartasDelete();
                        break;
                }
                CartasLoadGrill(gvProyectos.SelectedDataKey["au_proyecto"].ToString());
                gv_SelectedIndexChanged(gvProyectosCartas);
            }
            //PROYECTOS_Licencias
            else if (ViewState["AccionFinal"].ToString().Substring(0, 3) == "lic")
            {
                switch (ViewState["AccionFinal"].ToString().Substring(3))
                {
                    case "Editar":
                        if (!ValidateAccess(cnsSection.PROY_ASOC_LICENCIAS, cnsAction.EDITAR, true)) return;
                        LicenciasEdit();
                        break;
                    case "Agregar":
                        if (!ValidateAccess(cnsSection.PROY_ASOC_LICENCIAS, cnsAction.INSERTAR, true)) return;
                        LicenciasAdd();
                        break;
                    case "Eliminar":
                        if (!ValidateAccess(cnsSection.PROY_ASOC_LICENCIAS, cnsAction.ELIMINAR, true)) return;
                        LicenciasDelete();
                        break;
                }
                LicenciasLoad(gvProyectos.SelectedDataKey["au_proyecto"].ToString());
                gv_SelectedIndexChanged(gvProyectosLicencias);
            }

            Session["ReloadXFU"] = "1";
            EnableButtons();
            (this.Master as AuthenticNew).fReload();
        }
        protected void gv_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header)
                return;

            GridView gv = sender as GridView;
            string modulo = gv.ID.Substring(2);
            string sortExpression = ViewState["SortExp" + modulo].ToString();
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
        private void gv_SelectedIndexChanged(GridView gv)
        {
            string modulo = gv.ID.Substring(2);
            ViewState["Index" + modulo] = ((gv.PageIndex * gv.PageSize) + gv.SelectedIndex).ToString();
            try
            {
                UpdatePanel up = (UpdatePanel)divData.FindControl("up" + modulo + "Foot");
                oBasic.LblRegistros(up, gv.Rows.Count, Convert.ToInt32(ViewState["Index" + modulo] ?? "0"));
            }
            catch { }
        }
        private void gv_Sorting(GridView gv, string sortExpression, object ds)
        {
            string modulo = gv.ID.Substring(2);
            string expression = "", direction = "", sort = "";

            DataView dataView = new DataView(((DataSet)(ds)).Tables[0]);
            string[] expressions = (ViewState["SortExp" + modulo] ?? "").ToString().Split(',');
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
            oVar.prDataSet = oUtil.ConvertToDataSet(dataView);
            gv_SelectedIndexChanged(gv);
        }
        protected void MessageBox_Accept(string key)
        {
            try
            {
                switch (key)
                {
                    case "AddProy":
                        ProyectosAdd();
                        ProyectosLoad(ViewState["CriterioBuscar"].ToString());
                        gv_SelectedIndexChanged(gvProyectos);
                        ProyectosLoadChildGrid(gvProyectos.SelectedDataKey["au_proyecto"].ToString());
                        if (gvProyectos.SelectedIndex >= 0)
                            lblProyectosSection.Text = gvProyectos.Rows[gvProyectos.SelectedIndex].Cells[1].Text;
                        hddProyEstado.Value = "0";
                        break;
                    case "EditProy":
                        ProyectosEdit();
                        ProyectosLoad(ViewState["CriterioBuscar"].ToString());
                        gv_SelectedIndexChanged(gvProyectos);
                        ProyectosLoadChildGrid(gvProyectos.SelectedDataKey["au_proyecto"].ToString());
                        if (gvProyectos.SelectedIndex >= 0)
                            lblProyectosSection.Text = gvProyectos.Rows[gvProyectos.SelectedIndex].Cells[1].Text;
                        hddProyEstado.Value = "0";
                        break;
                    case "CreateTracing":
                        SeguimientoSaveNew();
                        break;
                    case "Delete":
                        oBasic.AlertSection(msgProyectos, clConstantes.MSG_D, "danger");
                        ProyectosDelete();
                        ProyectosLoad(ViewState["CriterioBuscar"].ToString());
                        gv_SelectedIndexChanged(gvProyectos);
                        ProyectosLoadChildGrid(gvProyectos.SelectedDataKey["au_proyecto"].ToString());
                        if (gvProyectos.SelectedIndex >= 0)
                            lblProyectosSection.Text = gvProyectos.Rows[gvProyectos.SelectedIndex].Cells[1].Text;
                        hddProyEstado.Value = "0";
                        break;
                    case "DeleteTermSheet":
                        oBasic.AlertSection(msgProyectosCartas, clConstantes.MSG_D, "danger");
                        CartasDelete();
                        CartasLoadGrill(gvProyectos.SelectedDataKey["au_proyecto"].ToString());
                        gv_SelectedIndexChanged(gvProyectosCartas);
                        break;
                    case "DeleteLicense":
                        oBasic.AlertSection(msgProyectosLicencias, clConstantes.MSG_D, "danger");
                        LicenciasDelete();
                        LicenciasLoad(gvProyectos.SelectedDataKey["au_proyecto"].ToString());
                        gv_SelectedIndexChanged(gvProyectosLicencias);
                        break;
                    case "DeleteProperties":
                        oBasic.AlertSection(msgProyectosPredios, clConstantes.MSG_D, "danger");
                        PrediosDelete();
                        PrediosLoad(gvProyectos.SelectedDataKey["au_proyecto"].ToString());
                        gv_SelectedIndexChanged(gvProyectosPredios);

                        break;
                    default:
                        break;
                }

                Session["ReloadXFU"] = "1";
                EnableButtons();
                (this.Master as AuthenticNew).fReload();
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region---Methods
        private void EnableButtons()
        {
            divPlanParcialFind.Visible = (ProyectosGetOrigen() == Origen.PLAN_PARCIAL);

            upProyectos.Update();

            oBasic.FixPanel(divData, "Proyectos", Convert.ToInt32(hddProyEstado.Value));
            txt_au_proyecto.Enabled = false;

            if (gvProyectosLicencias.Rows.Count > 0)
            {
                bool PY = gvProyectosLicencias.DataKeys[gvProyectosLicencias.SelectedIndex]["origen"].ToString() == "Proyecto";
                oBasic.EnableCtl(btnProyectosLicenciasVD, true);
                oBasic.EnableCtl(btnProyectosLicenciasEdit, oPermisos.TienePermisosAccion(cnsSection.PROY_ASOC_LICENCIAS, cnsAction.EDITAR) && PY);
                oBasic.EnableCtl(btnProyectosLicenciasDel, oPermisos.TienePermisosAccion(cnsSection.PROY_ASOC_LICENCIAS, cnsAction.ELIMINAR) && PY);
            }
            else
            {
                oBasic.EnableCtl(btnProyectosLicenciasVD, false);
                oBasic.EnableCtl(btnProyectosLicenciasEdit, false);
                oBasic.EnableCtl(btnProyectosLicenciasDel, false);
            }
        }
        private void Initialize()
        {
            cal_fecha_ejecutoria.StartDate
                = cal_fecha_radicado_manifestacion_interes.StartDate = cal_fecha_radicado_carta_intencion.StartDate = cal_fecha_radicado_otrosi.StartDate = cal_fecha_firma.StartDate
                = cal_fecha_inicio_ventas.StartDate = cal_fecha_inicio_obras.StartDate = new DateTime(2008, 1, 1);

            rvfecha_ejecutoria.MinimumValue
                = rvfecha_radicado_manifestacion_interes.MinimumValue = rvfecha_radicado_carta_intencion.MinimumValue = rvfecha_radicado_otrosi.MinimumValue = rvfecha_firma.MinimumValue
                = rv_fecha_inicio_ventas.MinimumValue = rv_fecha_inicio_obras.MinimumValue = (new DateTime(2008, 1, 1)).ToString("yyyy-MM-dd");

            cal_fecha_ejecutoria.EndDate
                = cal_fecha_radicado_manifestacion_interes.EndDate = cal_fecha_radicado_carta_intencion.EndDate = cal_fecha_radicado_otrosi.EndDate = cal_fecha_firma.EndDate
                = cal_fecha_inicio_ventas.EndDate = cal_fecha_inicio_obras.EndDate = DateTime.Today;

            rvfecha_ejecutoria.MaximumValue
                = rvfecha_radicado_manifestacion_interes.MaximumValue = rvfecha_radicado_carta_intencion.MaximumValue = rvfecha_radicado_otrosi.MaximumValue = rvfecha_firma.MaximumValue
                = rv_fecha_inicio_ventas.MaximumValue = rv_fecha_inicio_obras.MaximumValue = (DateTime.Today).ToString("yyyy-MM-dd");

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
            LoadDropDownIdentidad(ddl_id_documento_constitucion_proyecto, "54");
            LoadDropDownIdentidad(ddl_id_clasificacion_suelo, "31");
            LoadDropDownIdentidad(ddlb_id_fuente_informacion, "15");
            LoadDropDownIdentidad(ddl_id_tipo_licencia, "16");
            LoadDropDownIdentidad(ddlb_id_obligacion_VIS, "17");
            LoadDropDownIdentidad(ddlb_id_obligacion_VIP, "18");
            LoadDropDownIdentidad(ddl_id_estado_proyecto, "55", "3"); //3-ignore las opciones exclusivas de Proyectos estratégicos
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
        private void RegisterScript()
        {
            Page.Form.Enctype = "multipart/form-data";
            btnConfirmarProyectos.Attributes.Add("onclick", "document.getElementById('" + btnConfirmarProyectos.ClientID + "').disabled=true;" + ClientScript.GetPostBackEventReference(btnConfirmarProyectos, "Click") + "; return false;");
            lbSubirCarta.Attributes.Add("onclick", "document.getElementById('" + fuSubirCarta.ClientID + "').click();return false;");
            lbSubirLic.Attributes.Add("onclick", "document.getElementById('" + fuSubirLic.ClientID + "').click();return false;");

            StringBuilder scriptViewTop = new StringBuilder();
            scriptViewTop.Append("<script type='text/javascript'> ");
            scriptViewTop.Append(" window.addEventListener('click', function(e) { ");
            scriptViewTop.Append(" if (document.getElementById('" + upProyectos.ClientID + "').contains(e.target)) { $('#" + hdd_topview.ClientID + "').val(0);  }   ");
            scriptViewTop.Append(" else if (document.getElementById('" + upProyectosFoot.ClientID + "').contains(e.target)) { $('#" + hdd_topview.ClientID + "').val(0);   }   ");
            scriptViewTop.Append(" else if (document.getElementById('" + upProyectosSection.ClientID + "').contains(e.target)) { $('#" + hdd_topview.ClientID + "').val(1000);   }   ");
            scriptViewTop.Append(" }); ");

            scriptViewTop.Append(" function viewTop(){ ");
            scriptViewTop.Append(" var varscrollTop = $('#" + hdd_topview.ClientID + "').val();");
            scriptViewTop.Append(" setTimeout(() => { $('html').animate({ scrollTop: varscrollTop }, 1000);},500)}");
            scriptViewTop.Append(" $(window).on('load', viewTop());");
            scriptViewTop.Append(" </script> ");

            if (!Page.ClientScript.IsStartupScriptRegistered(Page.GetType(), "scriptViewTop"))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "scriptViewTop", scriptViewTop.ToString(), false);
            }


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

        }
        private bool ValidateAccess(string section, string action, bool validateResponsible = false, bool requeridedResponsible = true)
        {
            string cod_usu_responsable = (Session["Proyecto.cod_usu_responsable"] ?? 0).ToString();
            string message = oPermisos.ValidateAccess(section, action, cod_usu_responsable, validateResponsible, requeridedResponsible);

            if (message == "")
                return true;

            MessageInfo.ShowMessage(message);
            return false;
        }
        private void ValidateSP()
        {
            oBasic.EnableCtl(btnProyectosAdd, oPermisos.TienePermisosAccion(cnsSection.PROYECTO_ASOCIATIVO, cnsAction.INSERTAR));
            oBasic.EnableCtl(btnProyectosEdit, oPermisos.TienePermisosAccion(cnsSection.PROYECTO_ASOCIATIVO, cnsAction.EDITAR));
            oBasic.EnableCtl(btnProyectosDel, oPermisos.TienePermisosAccion(cnsSection.PROYECTO_ASOCIATIVO, cnsAction.ELIMINAR));
            upProyectosFoot.Update();

            oBasic.EnableCtl(btnProyectosPrediosAdd, oPermisos.TienePermisosAccion(cnsSection.PROY_ASOC_PREDIOS, cnsAction.INSERTAR));
            oBasic.EnableCtl(btnProyectosPrediosEdit, oPermisos.TienePermisosAccion(cnsSection.PROY_ASOC_PREDIOS, cnsAction.EDITAR));
            oBasic.EnableCtl(btnProyectosPrediosDel, oPermisos.TienePermisosAccion(cnsSection.PROY_ASOC_PREDIOS, cnsAction.ELIMINAR));
            upProyectosPrediosFoot.Update();

            oBasic.EnableCtl(btnProyectosCartasAdd, oPermisos.TienePermisosAccion(cnsSection.PROY_ASOC_CARTAS_INTENCION, cnsAction.INSERTAR));
            oBasic.EnableCtl(btnProyectosCartasEdit, oPermisos.TienePermisosAccion(cnsSection.PROY_ASOC_CARTAS_INTENCION, cnsAction.EDITAR));
            oBasic.EnableCtl(btnProyectosCartasDel, oPermisos.TienePermisosAccion(cnsSection.PROY_ASOC_CARTAS_INTENCION, cnsAction.ELIMINAR));
            upProyectosCartasFoot.Update();

            oBasic.EnableCtl(btnProyectosLicenciasAdd, oPermisos.TienePermisosAccion(cnsSection.PROY_ASOC_LICENCIAS, cnsAction.INSERTAR));
            oBasic.EnableCtl(btnProyectosLicenciasEdit, oPermisos.TienePermisosAccion(cnsSection.PROY_ASOC_LICENCIAS, cnsAction.EDITAR));
            oBasic.EnableCtl(btnProyectosLicenciasDel, oPermisos.TienePermisosAccion(cnsSection.PROY_ASOC_LICENCIAS, cnsAction.ELIMINAR));
            upProyectosLicenciasFoot.Update();
        }
        #endregion
        #endregion


        #region---Proyectos
        #region---Proyectos - Events
        protected void btnGoBanco_Click(object sender, EventArgs e)
        {
            Session["Retorno.Proyecto.Index"] = ViewState["IndexProyectos"] ?? "0";
            Session["Retorno.Proyecto.Data"] = (DataSet)oVar.prDSProyectos;
            Session["Retorno.Proyecto.filter"] = ViewState["CriterioBuscar"].ToString();

            Session["Retorno.Banco.Origen"] = "proyectos";
            Session["Proyecto.Banco.Id"] = hdd_idBanco.Value;
            Response.Redirect("Banco");
        }
        protected void btnGoPlanP_Click(object sender, EventArgs e)
        {
            Session["Retorno.Proyecto.Index"] = ViewState["IndexProyectos"] ?? "0";
            Session["Retorno.Proyecto.Data"] = (DataSet)oVar.prDSProyectos;
            Session["Retorno.Proyecto.filter"] = ViewState["CriterioBuscar"].ToString();

            Session["Retorno.PlanesP.Origen"] = "proyectos";
            Session["Proyecto.PlanesP.Id"] = ddl_cod_planp.SelectedValue;
            Response.Redirect("Planesp");
        }
        protected void btnGoPredio_Click(object sender, EventArgs e)
        {
            Session["Retorno.Proyecto.Index"] = ViewState["IndexProyectos"] ?? "0";
            Session["Retorno.Proyecto.Data"] = (DataSet)oVar.prDSProyectos;
            Session["Retorno.Proyecto.filter"] = ViewState["CriterioBuscar"].ToString();

            Session["Retorno.Predios.Origen"] = "proyectos";
            Session["Proyecto.Predios.chip"] = hdd_chip.Value;
            Response.Redirect("Predios");
        }
        protected void btnProyectosAccion_Click(object sender, EventArgs e)
        {
            btnGoPlanP.Visible = false;
            btnGoPredio.Visible = false;
            oBasic.EnableControls(pProyectosDetalle, true, true);
            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = btnAccionSource.CommandName;
            oVar.prViewState = btnAccionSource.CommandName.ToString();
            oVar.prIndexValue = gvProyectos.SelectedIndex;

            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    if (!ValidateAccess(cnsSection.PROYECTO_ASOCIATIVO, cnsAction.EDITAR, true, false)) return;
                    ProyectosDetail();
                    ProyectosEnabled(true);
                    oBasic.AlertSection(msgProyectos, clConstantes.MSG_U, "warning");
                    break;
                case "Agregar":
                    if (!ValidateAccess(cnsSection.PROYECTO_ASOCIATIVO, cnsAction.INSERTAR, true, false)) return;
                    ProyectosClear();
                    ProyectosLoadDropDownResponsibleUser(null);
                    ProyectosEnabled(true);
                    gvProyectos.SelectedIndex = -1;
                    ProyectosLoadChildGrid("-1");
                    if (gvProyectos.SelectedIndex >= 0)
                        lblProyectosSection.Text = gvProyectos.Rows[gvProyectos.SelectedIndex].Cells[1].Text;
                    oBasic.AlertSection(msgProyectos, clConstantes.MSG_I, "info");
                    break;
                case "Eliminar":
                    if (!ValidateAccess(cnsSection.PROYECTO_ASOCIATIVO, cnsAction.ELIMINAR, true)) return;
                    MessageBox1.ShowConfirmation("Delete", "¿Está seguro de continuar con la acción solicitada?", type: "danger");
                    return;
            }
            hddProyEstado.Value = "2";
            ProyectosRelationship();
            EnableButtons();
        }
        protected void btnProyectosAccionFinal_Click(object sender, EventArgs e)
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
                    MessageBox1.ShowConfirmation("EditProy",
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
            if (txt_au_proyecto.Text != "0")
            {
                MessageBox1.ShowConfirmation("EditProy", "¿Está seguro de actualizar la información?", type: "warning");
            }
            else
                MessageBox1.ShowConfirmation("AddProy", "¿Está seguro de continuar con la acción solicitada?");
        }
        protected void btnProyectosCancelar_Click(object sender, EventArgs e)
        {
            ProyectosLoad(ViewState["CriterioBuscar"].ToString());
            hddProyEstado.Value = "0";
            oBasic.AlertSection(msgProyectos, "", "0");
            EnableButtons();
            upProyectosSection.Update();
        }
        protected void btnProyectosNavegacion_Click(object sender, EventArgs e)
        {
            int index = 0;
            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    index = Convert.ToInt16(ViewState["IndexProyectos"] ?? "0") - 1;
                    break;
                case "Next":
                    index = Convert.ToInt16(ViewState["IndexProyectos"] ?? "0") + 1;
                    break;
                case "Last":
                    index = ((DataSet)oVar.prDSProyectos).Tables[0].Rows.Count - 1;
                    break;
            }
            gvProyectos.SelectedIndex = index;
            ViewState["IndexProyectos"] = index.ToString();
            hdd_idactor.Value = gvProyectos.SelectedDataKey["idactor"].ToString();
            ProyectosDetail();
            ProyectosRelationship();
            ProyectosEnabled(false);
            ProyectosLoadChildGrid(txt_au_proyecto.Text);
            if (gvProyectos.SelectedIndex >= 0)
                lblProyectosSection.Text = gvProyectos.Rows[gvProyectos.SelectedIndex].Cells[1].Text;
            EnableButtons();

            upProyectosSection.Update();
        }
        protected void btnProyectosSection_Click(object sender, EventArgs e)
        {
            int oldIndex = mvProyectosSection.ActiveViewIndex;
            int newIndex = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            string oldID = "lbProyectosSection_" + oldIndex.ToString();
            string newID = "lbProyectosSection_" + newIndex.ToString();
            LinkButton lbOld = (LinkButton)ulProyectosSection.FindControl(oldID);
            LinkButton lbNew = (LinkButton)ulProyectosSection.FindControl(newID);
            oBasic.ActiveNav(mvProyectosSection, lbOld, lbNew, newIndex);

            EnableButtons();
        }
        protected void btnProyectosVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            switch (cmdArg)
            {
                case 0:
                    oBasic.EnablePanel(pProyectosDetalle, true, true);
                    int iPagina = Convert.ToInt16(ViewState["IndexProyectos"] ?? "0") / gvProyectos.PageSize;
                    int index;
                    if (iPagina > 0)
                        index = Convert.ToInt16(ViewState["IndexProyectos"] ?? "0") % gvProyectos.PageSize;
                    else
                        index = Convert.ToInt16(ViewState["IndexProyectos"] ?? "0");
                    gvProyectos.PageIndex = iPagina;
                    gvProyectos.SelectedIndex = index;
                    break;
                case 1:
                    ProyectosDetail();
                    ProyectosEnabled(false);
                    break;
            }

            hddProyEstado.Value = cmdArg.ToString();
            EnableButtons();
        }
        protected void ddl_codlocalidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProyectosLoadDropDownUPZ();
        }
        protected void ddl_cod_planp_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProyectosRelationship();
        }
        protected void ddl_id_origen_proyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProyectosRelationship();
            EnableButtons();
        }
        protected void ddl_id_resultado_proyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            dvDocumentoPA.Visible = (ddl_id_resultado_proyecto.SelectedValue == cnsEstAsoc.ESTADO_ASOC_CUMPLIDO ||
                                    ddl_id_resultado_proyecto.SelectedValue == cnsEstAsoc.ESTADO_ASOC_VENC_TERMI);
            ValidPdfProject();
            rfv_InfoFile.Enabled = dvDocumentoPA.Visible;
            //pnlPrueba.Visible = dvDocumentoPA.Visible;
            upProyectos.Update();
        }
        protected void gvProyectos_DataBinding(object sender, EventArgs e)
        {
        }
        protected void gvProyectos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvProyectos, "Select$" + e.Row.RowIndex.ToString()));
                string idEstadoProyecto = gvProyectos.DataKeys[e.Row.DataItemIndex]["id_resultado_proyecto"].ToString();
                if (idEstadoProyecto == cnsEstAsoc.ESTADO_ASOC_CUMPLIDO || idEstadoProyecto == cnsEstAsoc.ESTADO_ASOC_VENC_TERMI)
                {
                    e.Row.ForeColor = System.Drawing.Color.FromArgb(55, 55, 55);
                    e.Row.BackColor = System.Drawing.Color.FromArgb(241, 242, 243);
                    e.Row.Font.Italic = true;
                }
            }
        }
        protected void gvProyectos_SelectedIndexChanged(object sender, EventArgs e)
        {
            gv_SelectedIndexChanged(gvProyectos);
            if (gvProyectos.Rows.Count > 0)
            {
                oVar.prProyectosAu = gvProyectos.SelectedDataKey["au_proyecto"].ToString();
                string idestadoAsociativo = gvProyectos.SelectedDataKey["id_resultado_proyecto"].ToString();
                string cod_usu_responsable = gvProyectos.SelectedDataKey["cod_usu_responsable"].ToString();

                if (idestadoAsociativo == cnsEstAsoc.ESTADO_ASOC_CUMPLIDO || idestadoAsociativo == cnsEstAsoc.ESTADO_ASOC_VENC_TERMI)
                    cod_usu_responsable = "-" + cod_usu_responsable;
                Session["Proyecto.cod_usu_responsable"] = cod_usu_responsable;
                hdd_idactor.Value = gvProyectos.SelectedDataKey["idactor"].ToString();
                ProyectosDetail();
                ProyectosLoadChildGrid(gvProyectos.SelectedDataKey["au_proyecto"].ToString());
                if (gvProyectos.SelectedIndex >= 0)
                    lblProyectosSection.Text = gvProyectos.Rows[gvProyectos.SelectedIndex].Cells[1].Text;
            }
            upProyectosSection.Update();
        }
        protected void gvProyectos_Sorting(object sender, GridViewSortEventArgs e)
        {
            gv_Sorting(gvProyectos, e.SortExpression.ToString(), oVar.prDSProyectos);
            oVar.prDSProyectos = oVar.prDataSet;
            ProyectosLoadChildGrid(gvProyectos.SelectedDataKey["au_proyecto"].ToString());
            if (gvProyectos.SelectedIndex >= 0)
                lblProyectosSection.Text = gvProyectos.Rows[gvProyectos.SelectedIndex].Cells[1].Text;
        }
        protected void lblPdfProject_Click(object sender, EventArgs e)
        {
            string fileName = hdd_ruta_archivo.Value;
            string pathFile = Path.Combine(oVar.prPathDocumentosProyectos.ToString(), fileName);

            oFile.GetPath(pathFile);

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "verDoc();", true);
        }
        protected void txt_chipfind_TextChanged(object sender, EventArgs e)
        {
            ProyectosRelationship();
        }
        #endregion

        #region---Proyectos - Methods
        private void ProyectosAdd()
        {
            if (!ValidateAccess(cnsSection.PROYECTO_ASOCIATIVO, cnsAction.INSERTAR, true, false)) return;

            ProyectosReadProyectoAreasZones();
            string strResultado = oProyectos.sp_i_proyecto(
            #region---params
                txt_nombre_proyecto.Text,
                oBasic.fInt(ddl_cod_planp),
                txt_chipfind.Text,
                txt_direccion_proyecto.Text,
                ddl_cod_localidad.Text,
                ddl_idupz.Text,
                oBasic.fInt(ddl_id_origen_proyecto),
                oBasic.fInt(ddl_id_clasificacion_suelo),
                oBasic.fInt(ddl_id_destino_catastral),
                oBasic.fInt(ddl_id_tratamiento_urbanistico),
                oBasic.fInt(ddl_id_instrumento_gestion),
                oBasic.fInt(ddl_id_instrumento_desarrollo),
                oBasic.fPerc(txt_porcentaje_SE_total),
                oBasic.fInt(ddl_id_estado_proyecto),
                oBasic.fInt(ddl_id_resultado_proyecto),
                txt_areas_zonas.Text,
                oBasic.fDec(txt_area_bruta),
                oBasic.fDec(txt_area_neta_urbanizable),
                oBasic.fDec(txt_area_util),
                oBasic.fInt(txt_UP_VIP),
                oBasic.fInt(txt_UP_VIS),
                oBasic.fInt(txt_UP_no_VIS),
                oBasic.fInt(txt_UE_VIP),
                oBasic.fInt(txt_UE_VIS),
                oBasic.fInt(txt_UE_no_VIS),
                oBasic.fInt(txt_empleos),
                oBasic.fInt(txt_inversion),
                txt_fecha_inicio_ventas.Text,
                txt_fecha_inicio_obras.Text,
                txt_observacion.Text,
                oBasic.fInt(ddl_cod_usu_responsable)
            #endregion
            );

            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                oBasic.SPOk(msgMain, msgProyectos, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                Session["Proyectos.cod_usu_responsable"] = oBasic.fInt(ddl_cod_usu_responsable);
            }
            else
                oBasic.SPError(msgMain, msgProyectos, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
        }
        private void ProyectosClear()
        {
            oBasic.fClearControls(vProyectosDetalle);
            DataSet dsTmp = (DataSet)oVar.prDSProyectos;
            if (dsTmp.Tables.Count > 0)
            {
                DataRow dRow = dsTmp.Tables[0].NewRow();
                oBasic.fValueControls(vProyectosDetalle, dRow);
            }
        }
        private void ProyectosDelete()
        {
            if (!ValidateAccess(cnsSection.PROYECTO_ASOCIATIVO, cnsAction.ELIMINAR, true)) return;

            string strResultado = oProyectos.sp_d_proyecto(gvProyectos.SelectedDataKey["au_proyecto"].ToString());
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                oBasic.fClearControls(vProyectosDetalle);

                if (Convert.ToInt16(ViewState["IndexProyectos"] ?? "0") > 0)
                    ViewState["IndexProyectos"] = Convert.ToInt16(ViewState["IndexProyectos"] ?? "0") - 1;
                else
                    ViewState["IndexProyectos"] = 0;

                oBasic.SPOk(msgMain, msgProyectos, "d", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
                oBasic.SPError(msgMain, msgProyectos, "d", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
        }
        private void ProyectosDetail()
        {
            int Indice = Convert.ToInt16(ViewState["IndexProyectos"] ?? "0");
            DataSet dsTmp = (DataSet)oVar.prDSProyectos;
            DataRow dRow = dsTmp.Tables[0].Rows[Indice];
            oBasic.LblRegistros(upProyectosFoot, dsTmp.Tables[0].Rows.Count, Indice);

            oBasic.fValueControls(vProyectosDetalle, dRow);
            if (lbl_fec_auditoria_proyecto.Text.Trim().Length > 0)
            {
                lbl_fec_auditoria_proyecto.Text = "Modificado el: " + lbl_fec_auditoria_proyecto.Text;
                lbl_fec_auditoria_proyecto.ToolTip = lbl_fec_auditoria_proyecto.Text;
            }
            ProyectosLoadDropDownUPZ();
            if (fuLoadProject.HasFile)
                fuLoadProject.ClearAllFilesFromPersistedStore();
            dvDocumentoPA.Visible = (hdd_ruta_archivo.Value != "" ||
                                        ddl_id_resultado_proyecto.SelectedValue == cnsEstAsoc.ESTADO_ASOC_CUMPLIDO ||
                                        ddl_id_resultado_proyecto.SelectedValue == cnsEstAsoc.ESTADO_ASOC_VENC_TERMI);
            //pnlPrueba.Visible = dvDocumentoPA.Visible;
            lblPdfProject.Visible = hdd_ruta_archivo.Value != "";
            lblInfoFileProject.Text = "";

            oBasic.fDetalleDropDown(ddl_idupz, dRow["idupz"].ToString());

            ProyectosLoadProyectoAreasZones();
            ddl_cod_planp_SelectedIndexChanged(null, null);
            ProyectosLoadDropDownResponsibleUser(dRow);
        }
        private void ProyectosEdit()
        {
            string strValidacion = clConstantes.DB_ACTION_OK;

            var fileName = "";

            if (!ValidateAccess(cnsSection.PROYECTO_ASOCIATIVO, cnsAction.EDITAR, true, false)) return;
            ProyectosReadProyectoAreasZones();
            if (ddl_id_resultado_proyecto.SelectedValue == cnsEstAsoc.ESTADO_ASOC_CUMPLIDO || ddl_id_resultado_proyecto.SelectedValue == cnsEstAsoc.ESTADO_ASOC_VENC_TERMI)
            {
                fileName = hdd_ruta_archivo.Value;
                strValidacion = oProyectos.sp_v_proyecto_validar(oBasic.fInt(txt_au_proyecto));
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

            string strResultado = oProyectos.sp_u_proyecto(oBasic.fInt(txt_au_proyecto), txt_nombre_proyecto.Text, oBasic.fInt(ddl_cod_planp), txt_chipfind.Text,
                txt_direccion_proyecto.Text, ddl_cod_localidad.Text, ddl_idupz.Text, oBasic.fInt(ddl_id_origen_proyecto), oBasic.fInt(ddl_id_clasificacion_suelo),
                oBasic.fInt(ddl_id_destino_catastral), oBasic.fInt(ddl_id_tratamiento_urbanistico), oBasic.fInt(ddl_id_instrumento_gestion), oBasic.fInt(ddl_id_instrumento_desarrollo),
                oBasic.fPerc(txt_porcentaje_SE_total), oBasic.fInt(ddl_id_estado_proyecto), oBasic.fInt(ddl_id_resultado_proyecto), txt_areas_zonas.Text, oBasic.fDec(txt_area_bruta),
                oBasic.fDec(txt_area_neta_urbanizable), oBasic.fDec(txt_area_util), oBasic.fInt(txt_UP_VIP), oBasic.fInt(txt_UP_VIS), oBasic.fInt(txt_UP_no_VIS),
                oBasic.fInt(txt_UE_VIP), oBasic.fInt(txt_UE_VIS), oBasic.fInt(txt_UE_no_VIS), oBasic.fInt(txt_empleos), oBasic.fInt(txt_inversion), txt_fecha_inicio_ventas.Text,
                txt_fecha_inicio_obras.Text, txt_observacion.Text, oBasic.fInt(ddl_cod_usu_responsable), fileName,null,null,null);

            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                oBasic.SPOk(msgMain, msgProyectos, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (strValidacion.Substring(0, 5) != clConstantes.DB_ACTION_OK)
                {
                    MessageInfo.ShowMessage(strValidacion.Substring(6), type: "info", title: "Operación Fallida");
                    return;
                }
            }
            else
                oBasic.SPError(msgMain, msgProyectos, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
        }
        private void ProyectosEnabled(bool b)
        {
            oBasic.EnableControls(pProyectosDetalle, b, true);
            btnGoPlanP.Visible = !b && (ddl_cod_planp.SelectedIndex > 0);
            btnGoPredio.Visible = !b && (hdd_chip.Value.Length > 1);
            btnGoPlanP.Enabled = !b;
            btnGoPredio.Enabled = !b;

            lblPdfProject.Enabled = true;
            lbLoadProject.Visible = b;

            txt_au_proyecto.Enabled = false;
            txt_UD_VIP.Enabled = false;
            txt_UD_VIS.Enabled = false;
            txt_UD_no_VIS.Enabled = false;

            txt_UP_vivienda.Enabled = false;
            txt_UE_vivienda.Enabled = false;
            txt_UD_vivienda.Enabled = false;
        }
        private string ProyectosGetOrigen()
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
        private void ProyectosLoadDropDownResponsibleUser(DataRow dRow)
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
        private void ProyectosLoadProyectoAreasZones()
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
        }
        private void ProyectosLoad(string Parametro)
        {
            //Se llena sesión y grilla
            bool fromAnotherPage = (Session["Retorno.Proyecto.Page"] ?? "").ToString() != "";
            if (string.IsNullOrEmpty(Parametro))
                Parametro = "%";
            if (fromAnotherPage)
            {
                oVar.prDSProyectos = (DataSet)Session["Retorno.Proyecto.Data"];
                ViewState["IndexProyectos"] = Session["Retorno.Proyecto.Index"];
                ViewState["CriterioBuscar"] = txtBuscar.Text = (Session["Retorno.Proyecto.filter"] ?? "").ToString();
                Session["Retorno.Proyecto.Data"] = null;
                Session["Retorno.Proyecto.Index"] = null;
                Session["Retorno.Proyecto.filter"] = null;
                Session["Retorno.Proyecto.Page"] = null;
            }
            else
            {
                if (oPermisos.TienePermisosAccion(cnsSection.PROYECTO_ASOCIATIVO, cnsAction.CONSULTAR, "", ""))
                {
                    string usuario = (string)((oPermisos.TienePermisosAccion(cnsSection.PROYECTO_ASOCIATIVO, cnsAction.CONSULTAR, "", oVar.prUserCod.ToString())) ? "" : oVar.prUserCod.ToString());
                    string PROY_ASOCIATIVO = "414";
                    oVar.prDSProyectos = oProyectos.sp_s_proyectos_nombre(Parametro, PROY_ASOCIATIVO, usuario);
                }
                else return;
            }

            gvProyectos.DataSource = ((DataSet)(oVar.prDSProyectos));
            gvProyectos.DataBind();

            if (gvProyectos.Rows.Count > 0)
            {
                gvProyectos.Visible = true;
                gvProyectos.SelectedIndex = Convert.ToInt16(ViewState["IndexProyectos"] ?? "0");
                if (gvProyectos.Rows.Count <= gvProyectos.SelectedIndex)
                    gvProyectos.SelectedIndex = gvProyectos.Rows.Count - 1;

                if (fromAnotherPage)
                {
                    ProyectosDetail();
                    ProyectosEnabled(false);
                    ProyectosRelationship();
                    hddProyEstado.Value = "1";
                }
                else
                {
                    gv_Sorting(gvProyectos, "", oVar.prDSProyectos);
                    oVar.prDSProyectos = oVar.prDataSet;
                }

                GetSelectedIndexProyect();

                gvProyectos.SelectedIndex = Convert.ToInt32(ViewState["IndexProyectos"] ?? "0");
                oVar.prProyectosAu = gvProyectos.SelectedDataKey["au_proyecto"].ToString();

                string idestadoAsociativo = gvProyectos.SelectedDataKey["id_resultado_proyecto"].ToString();
                string cod_usu_responsable = gvProyectos.SelectedDataKey["cod_usu_responsable"].ToString();

                if (idestadoAsociativo == cnsEstAsoc.ESTADO_ASOC_CUMPLIDO || idestadoAsociativo == cnsEstAsoc.ESTADO_ASOC_VENC_TERMI)
                    cod_usu_responsable = "-" + cod_usu_responsable;
                Session["Proyecto.cod_usu_responsable"] = cod_usu_responsable;

                hdd_idactor.Value = gvProyectos.SelectedDataKey["idactor"].ToString();
                ProyectosLoadChildGrid(gvProyectos.SelectedDataKey["au_proyecto"].ToString());
                if (gvProyectos.SelectedIndex >= 0)
                    lblProyectosSection.Text = gvProyectos.Rows[gvProyectos.SelectedIndex].Cells[1].Text;
            }
            else
            {
                gvProyectos.Visible = false;
                hdd_idactor.Value = "-1";
                ProyectosLoadChildGrid("-1");
                lblProyectosSection.Text = "";
                oBasic.FixPanel(divData, "Proyectos", 3);
            }
        }
        private void GetSelectedIndexProyect()
        {
            for (int i = 0; i < gvProyectos.Rows.Count; i++)
            {
                if (gvProyectos.DataKeys[i]["au_proyecto"].ToString() == (oVar.prProyectosAu ?? "0").ToString())
                    ViewState["IndexProyectos"] = i;
            }
        }
        private void ProyectosLoadChildGrid(string idProyecto)
        {
            PrediosLoad(idProyecto);
            CartasLoadGrill(idProyecto);
            LicenciasLoad(idProyecto);
            ActoresLoad(idProyecto);
            VisitasLoad(idProyecto);
            SeguimientoLoad(idProyecto);
            EnableButtons();
        }
        /// <summary>
        /// Método que carga la información correspondiente a los planos parciales y los predios declarados 
        /// al que se encuentra relacionado el proyecto en la selección del plan parcial o el valor del chip
        /// </summary>
        private void ProyectosRelationship()
        {
            int.TryParse(ProyectosGetOrigen() == Origen.PLAN_PARCIAL ? ddl_cod_planp.SelectedValue : "-1", out int codPlanP);
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
                        ProyectosLoadDropDownUPZ();
                        if (dtProyecPlan.Rows[0]["idupz"].ToString() != "") oBasic.fDetalle(ddl_idupz, dtProyecPlan.Rows[0]["idupz"].ToString());
                    }
                    if (dtProyecPlan.Rows[0]["id_tipo_tratamiento"].ToString() != "") oBasic.fDetalle(ddl_id_tratamiento_urbanistico, dtProyecPlan.Rows[0]["id_tipo_tratamiento"].ToString());
                    if (dtProyecPlan.Rows[0]["id_clasificacion_suelo"].ToString() != "") oBasic.fDetalle(ddl_id_clasificacion_suelo, dtProyecPlan.Rows[0]["id_clasificacion_suelo"].ToString());

                    if (codPlanP > 0)
                    {
                        oBasic.fValueControls(divAreas, dtProyecPlan.Rows[0]);
                        oBasic.fValueControls(divViviendas, dtProyecPlan.Rows[0]);
                        txt_areas_zonas.Text = dtProyecPlan.Rows[0]["areas_zonas"].ToString();
                        ProyectosLoadProyectoAreasZones();
                    }
                }
            }
            oBasic.StyleCtl("E", (codPlanP <= 0 && (chip == "" || hdd_chip.Value.Trim() == "")), txt_direccion_proyecto, false);
            oBasic.StyleCtl("E", (codPlanP <= 0 && (chip == "" || hdd_chip.Value.Trim() == "")), ddl_cod_localidad, false);
            oBasic.StyleCtl("E", (codPlanP <= 0 && (chip == "" || hdd_chip.Value.Trim() == "")), ddl_idupz, false);

            oBasic.StyleCtl("E", (codPlanP <= 0), ddl_id_clasificacion_suelo, false);
            oBasic.StyleCtl("E", (codPlanP <= 0), ddl_id_tratamiento_urbanistico, false);
            oBasic.EnableControls(divAreas, (codPlanP <= 0), true);
            oBasic.EnableControls(divViviendas, (codPlanP <= 0), true);
            oBasic.EnableControls(divAreasZonas, (codPlanP <= 0), true);
        }
        private void ProyectosLoadDropDownUPZ()
        {
            ddl_idupz.Items.Clear();
            ddl_idupz.DataSource = oUpz.sp_s_upz(ddl_cod_localidad.SelectedValue);
            ddl_idupz.DataTextField = "nombre_upz";
            ddl_idupz.DataValueField = "cod_upz";

            ddl_idupz.Items.Insert(0, new ListItem("-- Seleccione opción", "0"));
            ddl_idupz.DataBind();
        }
        private void ProyectosReadProyectoAreasZones()
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
                //lblInfoFileProject.Text = hdd_ruta_archivo.Value;
            }
        }
        #endregion
        #endregion


        #region---Actores o responsables

        #region---Actores - Methods
        private void ActoresLoad(string p_idproyecto)
        {
            if (string.IsNullOrEmpty(p_idproyecto))
                p_idproyecto = "0";

            Int32.TryParse(hdd_idactor.Value, out int idactor);
            ucRepresentative.ReferenceID = Convert.ToInt32(p_idproyecto);
            ucRepresentative.ActorID = idactor;
            ucRepresentative.ResponsibleUserCode = (Session["Proyecto.cod_usu_responsable"] ?? 0).ToString();
            ucRepresentative.LoadGrid();

            oBasic.FixPanel(divData, "ProyectosActores", 0);
        }
        #endregion
        #endregion

        #region---Cartas de intención
        #region---Cartas de intención - Events
        protected void btnCartasAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = "car" + btnAccionSource.CommandName;
            oVar.prIndexValue = gvProyectosCartas.SelectedIndex;

            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    if (!ValidateAccess(cnsSection.PROY_ASOC_CARTAS_INTENCION, cnsAction.EDITAR, true) || gvProyectosCartas.Rows.Count == 0) return;
                    CartasDetail();
                    CartasEnabled(true);
                    oBasic.AlertSection(msgProyectosCartas, clConstantes.MSG_U, "warning");
                    break;
                case "Agregar":
                    if (!ValidateAccess(cnsSection.PROY_ASOC_CARTAS_INTENCION, cnsAction.INSERTAR, true)) return;

                    CartasClear();
                    CartasEnabled(true);
                    oBasic.AlertSection(msgProyectosCartas, clConstantes.MSG_I, "info");
                    break;
                case "Eliminar":
                    if (!ValidateAccess(cnsSection.PROY_ASOC_CARTAS_INTENCION, cnsAction.ELIMINAR, true) || gvProyectosCartas.Rows.Count == 0) return;
                    MessageBox1.ShowConfirmation("DeleteTermSheet", "¿Está seguro de continuar con la acción solicitada?", type: "danger");
                    return;
            }
            oBasic.FixPanel(divData, "ProyectosCartas", 2);
        }
        protected void btnCartasCancelar_Click(object sender, EventArgs e)
        {
            oBasic.FixPanel(divData, "ProyectosCartas", 0);
            oBasic.AlertSection(msgProyectosCartas, "", "0");
        }
        protected void btnCartasNavegacion_Click(object sender, EventArgs e)
        {
            int index = 0;
            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    index = Convert.ToInt16(ViewState["IndexProyectosCartas"] ?? "0") - 1;
                    break;
                case "Next":
                    index = Convert.ToInt16(ViewState["IndexProyectosCartas"] ?? "0") + 1;
                    break;
                case "Last":
                    index = ((DataSet)oVar.prDSProyectosCartas).Tables[0].Rows.Count - 1;
                    break;
            }
            gvProyectosCartas.SelectedIndex = index;
            ViewState["IndexProyectosCartas"] = index.ToString();
            CartasEnabled(false);
            CartasDetail();
        }
        protected void btnCartasVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            int index;
            mvProyectosCartas.ActiveViewIndex = cmdArg;
            switch (cmdArg)
            {
                case 0:
                    int iPagina = Convert.ToInt16(ViewState["IndexProyectosCartas"] ?? "0") / gvProyectosCartas.PageSize;
                    if (iPagina > 0)
                        index = Convert.ToInt16(ViewState["IndexProyectosCartas"] ?? "0") % gvProyectosCartas.PageSize;
                    else
                        index = Convert.ToInt16(ViewState["IndexProyectosCartas"] ?? "0");
                    gvProyectosCartas.PageIndex = iPagina;
                    gvProyectosCartas.SelectedIndex = index;
                    break;
                case 1:
                    CartasEnabled(false);
                    CartasDetail();
                    break;
            }
            oBasic.FixPanel(divData, "ProyectosCartas", cmdArg);
        }
        protected void gvCartas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                if (e.CommandName == "OpenFile")
                {
                    string ruta = gvProyectosCartas.DataKeys[rowIndex]["ruta_carta"].ToString();
                    ruta = oVar.prPathDocumentosProyectos + ruta;
                    oFile.GetPath(ruta);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "verDoc();", true);
                }
            }
        }
        protected void gvCartas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvProyectosCartas, "Select$" + e.Row.RowIndex.ToString()));
                string carta_intencion_firmada = gvProyectosCartas.DataKeys[e.Row.DataItemIndex % gvProyectosCartas.PageSize]["carta_intencion_firmada"].ToString();
                string meses_desarrollo = gvProyectosCartas.DataKeys[e.Row.DataItemIndex % gvProyectosCartas.PageSize]["meses_desarrollo"].ToString();
                string fecha_firma = gvProyectosCartas.DataKeys[e.Row.DataItemIndex % gvProyectosCartas.PageSize]["fecha_firma"].ToString();
                ((Label)e.Row.FindControl("lblSemaforo")).Visible = false;
                string idestadoAsociativo = gvProyectos.SelectedDataKey["id_resultado_proyecto"].ToString();

                bool finalizado = (idestadoAsociativo == cnsEstAsoc.ESTADO_ASOC_CUMPLIDO || idestadoAsociativo == cnsEstAsoc.ESTADO_ASOC_VENC_TERMI);


                if (carta_intencion_firmada == "1")
                {
                    ((Label)e.Row.FindControl("lblSemaforo")).ForeColor = System.Drawing.Color.DarkRed;
                    if (Int32.TryParse(meses_desarrollo, out int meses))
                    {
                        ((Label)e.Row.FindControl("lblSemaforo")).Visible = true && !finalizado;
                        ((Label)e.Row.FindControl("lblDiff")).Text = meses.ToString();
                        if (DateTime.TryParse(fecha_firma, out DateTime fechafirma))
                        {
                            DateTime fechavigencia = fechafirma.AddMonths(meses);
                            if (fechavigencia >= DateTime.Today && !finalizado)
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
        protected void gvCartas_SelectedIndexChanged(object sender, EventArgs e)
        {
            gv_SelectedIndexChanged(gvProyectosCartas);
        }
        protected void gvCartas_Sorting(object sender, GridViewSortEventArgs e)
        {
            gv_Sorting(gvProyectosCartas, e.SortExpression.ToString(), oVar.prDSProyectosCartas);
            oVar.prDSProyectosCartas = oVar.prDataSet;
        }
        #endregion
        #region---Cartas de intención - Methods
        private void CartasAdd()
        {
            var fileName = "";
            if (fuSubirCarta.HasFile)
            {
                fileName = "CIPY_" + Guid.NewGuid() + Path.GetExtension(fuSubirCarta.FileName);
                var pdf_file = Path.Combine(oVar.prPathDocumentosProyectos.ToString(), fileName);
                oBasic.LoadPdf(fuSubirCarta, pdf_file);
            }

            string strResultado = oProyectosCartas.sp_i_proyecto_carta(
                gvProyectos.SelectedDataKey["au_proyecto"].ToString(),
                txt_radicado_manifestacion_interes.Text,
                txt_fecha_radicado_manifestacion_interes.Text,
                txt_radicado_carta_intencion.Text,
                txt_fecha_radicado_carta_intencion.Text,
                chk_carta_intencion_firmada.Checked,
                txt_fecha_firma.Text,
                oBasic.fInt(ddl_id_documento_constitucion_proyecto),
                txt_radicado_otrosi.Text,
                txt_fecha_radicado_otrosi.Text,
                txt_meses_desarrollo.Text,
                txt_unidad_gestion_aplica_proyecto.Text,
                txt_etapa_aplica_proyecto.Text,
                oBasic.fDec(txt_area_util__carta),
                txt_area_minima_vivienda.Text,
                txt_localizacion_proyecto.Text,
                oBasic.fInt(txt_UP_VIP__carta),
                oBasic.fInt(txt_UP_VIS__carta),
                oBasic.fInt(txt_UP_E3),
                oBasic.fInt(txt_UP_E4),
                oBasic.fInt(txt_UP_E5),
                oBasic.fInt(txt_UP_E6),
                fileName,
                txt_observacion__carta.Text
            );
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                oBasic.SPOk(msgMain, msgProyectosCartas, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
            else
                oBasic.SPError(msgMain, msgProyectosCartas, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
        }
        private void CartasClear()
        {
            oBasic.fClearControls(vProyectosCartasDetalle);
            DataSet dsTmp = (DataSet)oVar.prDSProyectosCartas;
            if (dsTmp.Tables.Count > 0)
            {
                DataRow dRow = dsTmp.Tables[0].NewRow();
                oBasic.fValueControls(vProyectosCartasDetalle, dRow);
            }
        }
        private void CartasDelete()
        {
            try
            {
                string strResultado = oProyectosCartas.sp_d_proyecto_carta(gvProyectosCartas.SelectedDataKey.Value.ToString());
                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fProyectosCartasDelete:", clConstantes.MSG_OK_D);
                    oBasic.fClearControls(vProyectosCartasDetalle);

                    if (Convert.ToInt16(ViewState["IndexProyectosCartas"] ?? "0") > 0)
                        ViewState["IndexProyectosCartas"] = Convert.ToInt16(ViewState["IndexProyectosCartas"] ?? "0") - 1;
                    else
                        ViewState["IndexProyectosCartas"] = 0;

                    oBasic.SPOk(msgMain, msgProyectosCartas, "d", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                    oBasic.SPError(msgMain, msgProyectosCartas, "d", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
            }
            catch { }
        }
        private void CartasDetail()
        {
            mvProyectosCartas.ActiveViewIndex = 1;
            int Indice = Convert.ToInt16(ViewState["IndexProyectosCartas"] ?? "0");
            DataSet dsTmp = (DataSet)oVar.prDSProyectosCartas;
            if (dsTmp.Tables[0].Rows.Count > 0)
            {
                DataRow dRow = dsTmp.Tables[0].Rows[Indice];

                oBasic.fValueControls(vProyectosCartasDetalle, dRow);
                if (lbl_fec_auditoria_cartas.Text.Trim().Length > 0)
                {
                    lbl_fec_auditoria_cartas.Text = "Modificado el: " + lbl_fec_auditoria_cartas.Text;
                    lbl_fec_auditoria_cartas.ToolTip = lbl_fec_auditoria_cartas.Text;
                }
                oBasic.LblRegistros(upProyectosCartasFoot, dsTmp.Tables[0].Rows.Count, Indice);
            }
        }
        private void CartasEdit()
        {
            var fileName = "";
            if (fuSubirCarta.HasFile)
            {
                fileName = "CIPY_" + Guid.NewGuid() + Path.GetExtension(fuSubirCarta.FileName);
                var pdf_file = Path.Combine(oVar.prPathDocumentosProyectos.ToString(), fileName);
                oBasic.LoadPdf(fuSubirCarta, pdf_file);
            }

            string strResultado = oProyectosCartas.sp_u_proyecto_carta(
            oBasic.fInt(txt_au_proyecto_carta),
            txt_radicado_manifestacion_interes.Text,
            txt_fecha_radicado_manifestacion_interes.Text,
            txt_radicado_carta_intencion.Text,
            txt_fecha_radicado_carta_intencion.Text,
            chk_carta_intencion_firmada.Checked,
            txt_fecha_firma.Text,
            oBasic.fInt(ddl_id_documento_constitucion_proyecto),
            txt_radicado_otrosi.Text,
            txt_fecha_radicado_otrosi.Text,
            txt_meses_desarrollo.Text,
            txt_unidad_gestion_aplica_proyecto.Text,
            txt_etapa_aplica_proyecto.Text,
            oBasic.fDec(txt_area_util__carta),
            txt_area_minima_vivienda.Text,
            txt_localizacion_proyecto.Text,
            oBasic.fInt(txt_UP_VIP__carta),
            oBasic.fInt(txt_UP_VIS__carta),
            oBasic.fInt(txt_UP_E3),
            oBasic.fInt(txt_UP_E4),
            oBasic.fInt(txt_UP_E5),
            oBasic.fInt(txt_UP_E6),
            fileName,
            txt_observacion__carta.Text);

            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                oBasic.SPOk(msgMain, msgProyectosCartas, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
            else
                oBasic.SPError(msgMain, msgProyectosCartas, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
        }
        private void CartasEnabled(bool HabilitarCampos)
        {
            txt_au_proyecto_carta.Enabled = false;
            txt_cod_proyecto__carta.Enabled = false;
            oBasic.EnableControls(vProyectosCartasDetalle, HabilitarCampos, true);
            oBasic.EnableCtl(lbSubirCarta, HabilitarCampos);
        }
        private void CartasLoadGrill(string p_cod_proyecto)
        {
            if (string.IsNullOrEmpty(p_cod_proyecto))
                p_cod_proyecto = "0";
            oVar.prDSProyectosCartas = oProyectosCartas.sp_s_proyectos_cartas_cod_proyecto(p_cod_proyecto);

            gvProyectosCartas.DataSource = ((DataSet)(oVar.prDSProyectosCartas));
            gvProyectosCartas.DataBind();

            if (gvProyectosCartas.Rows.Count > 0)
            {
                gvProyectosCartas.Visible = true;
                oBasic.FixPanel(divData, "ProyectosCartas", 0);
            }
            else
            {
                gvProyectosCartas.Visible = false;
            }

            gv_Sorting(gvProyectosCartas, ViewState["SortExpProyectosCartas"].ToString(), oVar.prDSProyectosCartas);
            oVar.prDSProyectosCartas = oVar.prDataSet;
            oBasic.FixPanel(divData, "ProyectosCartas", 0);
        }
        #endregion
        #endregion

        #region---Licencias
        #region---Licencias - Events
        protected void btnLicenciasAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = "lic" + btnAccionSource.CommandName;
            oVar.prIndexValue = gvProyectosLicencias.SelectedIndex;

            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    if (!ValidateAccess(cnsSection.PROY_ASOC_LICENCIAS, cnsAction.EDITAR, true) || gvProyectosLicencias.Rows.Count == 0) return;
                    string origen = gvProyectosLicencias.SelectedRow.Cells[1].Text;
                    if (origen != "Proyecto")
                        return;
                    LicenciasDetail();
                    LicenciasEnabled(true);
                    oBasic.AlertSection(msgProyectosLicencias, clConstantes.MSG_U, "warning");
                    break;
                case "Agregar":
                    if (!ValidateAccess(cnsSection.PROY_ASOC_LICENCIAS, cnsAction.INSERTAR, true)) return;
                    LicenciasClear();
                    LicenciasEnabled(true);
                    oBasic.AlertSection(msgProyectosLicencias, clConstantes.MSG_I, "info");
                    break;
                case "Eliminar":
                    if (!ValidateAccess(cnsSection.PROY_ASOC_LICENCIAS, cnsAction.ELIMINAR, true) || gvProyectosLicencias.Rows.Count == 0) return;
                    string fuent = gvProyectosLicencias.SelectedRow.Cells[1].Text;
                    if (fuent != "Proyecto")
                        return;
                    MessageBox1.ShowConfirmation("DeleteLicense", "¿Está seguro de continuar con la acción solicitada?", type: "danger");
                    return;
            }
            oBasic.FixPanel(divData, "ProyectosLicencias", 2);
        }
        protected void btnLicenciasCancelar_Click(object sender, EventArgs e)
        {
            oBasic.FixPanel(divData, "ProyectosLicencias", 0);
            oBasic.AlertSection(msgProyectosLicencias, "", "0");
        }
        protected void btnLicenciasNavegacion_Click(object sender, EventArgs e)
        {
            int index = 0;
            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    index = Convert.ToInt16(ViewState["IndexProyectosLicencias"] ?? "0") - 1;
                    break;
                case "Next":
                    index = Convert.ToInt16(ViewState["IndexProyectosLicencias"]) + 1;
                    break;
                case "Last":
                    index = ((DataSet)oVar.prDSProyectosLicencias).Tables[0].Rows.Count - 1;
                    break;
            }
            gvProyectosLicencias.SelectedIndex = index;
            ViewState["IndexProyectosLicencias"] = index.ToString();
            LicenciasEnabled(false);
            LicenciasDetail();
        }
        protected void btnLicenciasVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            mvProyectosLicencias.ActiveViewIndex = cmdArg;
            switch (cmdArg)
            {
                case 0:
                    int index;
                    int iPagina = Convert.ToInt16(ViewState["IndexProyectosLicencias"] ?? "0") / gvProyectosLicencias.PageSize;
                    if (iPagina > 0)
                        index = Convert.ToInt16(ViewState["IndexProyectosLicencias"] ?? "0") % gvProyectosLicencias.PageSize;
                    else
                        index = Convert.ToInt16(ViewState["IndexProyectosLicencias"] ?? "0");
                    gvProyectosLicencias.PageIndex = iPagina;
                    gvProyectosLicencias.SelectedIndex = index;
                    break;
                case 1:
                    LicenciasEnabled(false);
                    if (gvProyectosLicencias.Rows.Count > 0)
                        LicenciasDetail();
                    break;
            }
            oBasic.FixPanel(divData, "ProyectosLicencias", cmdArg);
        }
        protected void gvLicencias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                if (e.CommandName == "OpenFile")
                {
                    string ruta = gvProyectosLicencias.DataKeys[rowIndex]["ruta_licencia"].ToString();
                    string origen = gvProyectosLicencias.DataKeys[rowIndex]["origen"].ToString();
                    ruta = origen == "Proyecto" ? oVar.prPathDocumentosProyectos + ruta : oVar.prPathPlanesPLicencias + ruta + ".pdf";
                    oFile.GetPath(ruta);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "verDoc();", true);
                }
            }
        }
        protected void gvLicencias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (gvProyectosLicencias.DataKeys[e.Row.DataItemIndex]["origen"].ToString() != "Proyecto")
                {
                    e.Row.ForeColor = System.Drawing.Color.FromArgb(55, 55, 55);
                    e.Row.BackColor = System.Drawing.Color.FromArgb(241, 242, 243);
                    e.Row.Font.Italic = true;
                }
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvProyectosLicencias, "Select$" + e.Row.RowIndex.ToString()));
            }
        }
        protected void gvLicencias_SelectedIndexChanged(object sender, EventArgs e)
        {
            gv_SelectedIndexChanged(gvProyectosLicencias);
            EnableButtons();
        }
        protected void gvLicencias_Sorting(object sender, GridViewSortEventArgs e)
        {
            gv_Sorting(gvProyectosLicencias, e.SortExpression.ToString(), oVar.prDSProyectosLicencias);
            oVar.prDSProyectosLicencias = oVar.prDataSet;
        }
        #endregion

        #region---Licencias - Methods
        private void LicenciasAdd()
        {
            var fileName = "";
            if (fuSubirLic.HasFile)
            {
                fileName = "LCPY_" + Guid.NewGuid() + Path.GetExtension(fuSubirLic.FileName);
                var pdf_file = Path.Combine(oVar.prPathDocumentosProyectos.ToString(), fileName);
                oBasic.LoadPdf(fuSubirLic, pdf_file);
            }

            string strResultado = oProyectosLicencias.sp_i_proyecto_licencia(
                gvProyectos.SelectedDataKey["au_proyecto"].ToString(),
                oBasic.fInt(ddlb_id_fuente_informacion),
                oBasic.fInt(ddl_id_tipo_licencia),
                txt_licencia.Text,
                oBasic.fInt(ddl_curador),
                txt_fecha_ejecutoria.Text,
                oBasic.fInt(txt_termino_vigencia_meses),
                txt_plano_urbanistico_aprobado.Text,
                txt_nombreproyecto.Text,
                oBasic.fDec(txt_area_bruta__licencia),
                oBasic.fDec(txt_area_neta),
                oBasic.fDec(txt_area_util__licencia),
                oBasic.fDec(txt_area_cesion_zonas_verdes),
                oBasic.fDec(txt_area_cesion_vias),
                oBasic.fDec(txt_area_cesion_eq_comunal),
                oBasic.fPerc(txt_porcentaje_ejecucion_urbanismo),
                oBasic.fInt(ddlb_id_obligacion_VIS),
                oBasic.fInt(ddlb_id_obligacion_VIP),
                oBasic.fDec(txt_area_terreno_VIS),
                oBasic.fDec(txt_area_terreno_no_VIS),
                oBasic.fDec(txt_area_terreno_VIP),
                oBasic.fDec(txt_area_construida_VIS),
                oBasic.fDec(txt_area_construida_no_VIS),
                oBasic.fDec(txt_area_construida_VIP),
                oBasic.fDec(txt_porcentaje_obligacion_VIS),
                oBasic.fDec(txt_porcentaje_obligacion_VIP),
                oBasic.fInt(txt_unidades_vivienda_VIS),
                oBasic.fInt(txt_unidades_vivienda_no_VIS),
                oBasic.fInt(txt_unidades_vivienda_VIP),
                oBasic.fDec(txt_area_comercio),
                oBasic.fDec(txt_area_oficina),
                oBasic.fDec(txt_area_institucional),
                oBasic.fDec(txt_area_industria),
                oBasic.fDec(txt_area_lote),
                oBasic.fDec(txt_area_sotano),
                oBasic.fDec(txt_area_semisotano),
                oBasic.fDec(txt_area_primer_piso),
                oBasic.fDec(txt_area_pisos_restantes),
                oBasic.fDec(txt_area_construida_total),
                oBasic.fDec(txt_area_libre_primer_piso),
                oBasic.fPerc(txt_porcentaje_ejecucion_construccion),
                txt_observacion__licencia.Text,
                fileName
            );
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                oBasic.SPOk(msgMain, msgProyectosLicencias, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
            else
                oBasic.SPError(msgMain, msgProyectosLicencias, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
        }
        private void LicenciasClear()
        {
            oBasic.fClearControls(vProyectosLicenciasDetalle);
            DataSet dsTmp = (DataSet)oVar.prDSProyectosLicencias;
            if (dsTmp.Tables.Count > 0)
            {
                DataRow dRow = dsTmp.Tables[0].NewRow();
                oBasic.fValueControls(vProyectosLicenciasDetalle, dRow);
            }
        }
        private void LicenciasDelete()
        {
            try
            {
                string strResultado = oProyectosLicencias.sp_d_proyecto_licencia(gvProyectosLicencias.SelectedDataKey.Value.ToString());
                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fProyectosLicenciasDelete:", clConstantes.MSG_OK_D);
                    oBasic.fClearControls(vProyectosLicenciasDetalle);

                    if (Convert.ToInt16(ViewState["IndexProyectosLicencias"] ?? "0") > 0)
                        ViewState["IndexProyectosLicencias"] = Convert.ToInt16(ViewState["IndexProyectosLicencias"] ?? "0") - 1;
                    else
                        ViewState["IndexProyectosLicencias"] = 0;

                    oBasic.SPOk(msgMain, msgProyectosLicencias, "d", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                    oBasic.SPError(msgMain, msgProyectosLicencias, "d", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
            }
            catch { }
        }
        private void LicenciasDetail()
        {
            mvProyectosLicencias.ActiveViewIndex = 1;
            int Indice = Convert.ToInt16(ViewState["IndexProyectosLicencias"] ?? "0");
            DataSet dsTmp = (DataSet)oVar.prDSProyectosLicencias;
            if (dsTmp.Tables[0].Rows.Count > 0)
            {
                DataRow dRow = dsTmp.Tables[0].Rows[Indice];

                oBasic.fValueControls(vProyectosLicenciasDetalle, dRow);
                if (lbl_fec_auditoria_licencia.Text.Trim().Length > 0)
                {
                    lbl_fec_auditoria_licencia.Text = "Modificado el: " + lbl_fec_auditoria_licencia.Text;
                    lbl_fec_auditoria_licencia.ToolTip = lbl_fec_auditoria_licencia.Text;
                }
                oBasic.LblRegistros(upProyectosLicenciasFoot, dsTmp.Tables[0].Rows.Count, Indice);
            }

            EnableButtons();
        }
        private void LicenciasEdit()
        {
            var fileName = "";
            if (fuSubirLic.HasFile)
            {
                fileName = "LCPY_" + Guid.NewGuid() + Path.GetExtension(fuSubirLic.FileName);
                var pdf_file = Path.Combine(oVar.prPathDocumentosProyectos.ToString(), fileName);
                oBasic.LoadPdf(fuSubirLic, pdf_file);
            }

            string strResultado = oProyectosLicencias.sp_u_proyecto_licencia(
                oBasic.fInt(txt_au_proyecto_licencia),
                oBasic.fInt(ddlb_id_fuente_informacion),
                oBasic.fInt(ddl_id_tipo_licencia),
                txt_licencia.Text,
                oBasic.fInt(ddl_curador),
                txt_fecha_ejecutoria.Text,
                oBasic.fInt(txt_termino_vigencia_meses),
                txt_plano_urbanistico_aprobado.Text,
                txt_nombreproyecto.Text,
                oBasic.fDec(txt_area_bruta__licencia),
                oBasic.fDec(txt_area_neta),
                oBasic.fDec(txt_area_util__licencia),
                oBasic.fDec(txt_area_cesion_zonas_verdes),
                oBasic.fDec(txt_area_cesion_vias),
                oBasic.fDec(txt_area_cesion_eq_comunal),
                oBasic.fPerc(txt_porcentaje_ejecucion_urbanismo),
                oBasic.fInt(ddlb_id_obligacion_VIS),
                oBasic.fInt(ddlb_id_obligacion_VIP),
                oBasic.fDec(txt_area_terreno_VIS),
                oBasic.fDec(txt_area_terreno_no_VIS),
                oBasic.fDec(txt_area_terreno_VIP),
                oBasic.fDec(txt_area_construida_VIS),
                oBasic.fDec(txt_area_construida_no_VIS),
                oBasic.fDec(txt_area_construida_VIP),
                oBasic.fDec(txt_porcentaje_obligacion_VIS),
                oBasic.fDec(txt_porcentaje_obligacion_VIP),
                oBasic.fInt(txt_unidades_vivienda_VIS),
                oBasic.fInt(txt_unidades_vivienda_no_VIS),
                oBasic.fInt(txt_unidades_vivienda_VIP),
                oBasic.fDec(txt_area_comercio),
                oBasic.fDec(txt_area_oficina),
                oBasic.fDec(txt_area_institucional),
                oBasic.fDec(txt_area_industria),
                oBasic.fDec(txt_area_lote),
                oBasic.fDec(txt_area_sotano),
                oBasic.fDec(txt_area_semisotano),
                oBasic.fDec(txt_area_primer_piso),
                oBasic.fDec(txt_area_pisos_restantes),
                oBasic.fDec(txt_area_construida_total),
                oBasic.fDec(txt_area_libre_primer_piso),
                oBasic.fPerc(txt_porcentaje_ejecucion_construccion),
                txt_observacion__licencia.Text,
                fileName
            );
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                oBasic.SPOk(msgMain, msgProyectosLicencias, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
            else
                oBasic.SPError(msgMain, msgProyectosLicencias, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
        }
        private void LicenciasEnabled(bool HabilitarCampos)
        {
            txt_au_proyecto_licencia.Enabled = false;
            txt_cod_proyecto__licencia.Enabled = false;
            oBasic.EnableControls(vProyectosLicenciasDetalle, HabilitarCampos, true);
            oBasic.EnableCtl(lbSubirLic, HabilitarCampos);
        }
        private void LicenciasLoad(string p_cod_proyecto)
        {
            if (string.IsNullOrEmpty(p_cod_proyecto))
                p_cod_proyecto = "0";
            oVar.prDSProyectosLicencias = oProyectosLicencias.sp_s_proyectos_licencias_cod_proyecto(p_cod_proyecto);

            gvProyectosLicencias.DataSource = ((DataSet)(oVar.prDSProyectosLicencias));
            gvProyectosLicencias.DataBind();

            if (gvProyectosLicencias.Rows.Count > 0)
            {
                gvProyectosLicencias.Visible = true;
                oBasic.FixPanel(divData, "ProyectosLicencias", 0);
            }
            else
            {
                gvProyectosLicencias.Visible = false;
            }
            gv_Sorting(gvProyectosLicencias, ViewState["SortExpProyectosLicencias"].ToString(), oVar.prDSProyectosLicencias);
            oVar.prDSProyectosLicencias = oVar.prDataSet;
            oBasic.FixPanel(divData, "ProyectosLicencias", 0);
        }
        #endregion
        #endregion

        #region---Predios
        #region---Predios - Events
        protected void btnPrediosAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = "pre" + btnAccionSource.CommandName;
            oVar.prIndexValue = gvProyectosPredios.SelectedIndex;
            btnGoPredio2.Visible = false;

            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    if (!ValidateAccess(cnsSection.PROY_ASOC_PREDIOS, cnsAction.EDITAR, true) || gvProyectosPredios.Rows.Count == 0) return;
                    PrediosDetail();
                    PrediosEnabled(true);
                    oBasic.AlertSection(msgProyectosPredios, clConstantes.MSG_U, "warning");
                    break;
                case "Agregar":
                    if (!ValidateAccess(cnsSection.PROY_ASOC_PREDIOS, cnsAction.INSERTAR, true)) return;
                    oBasic.fClearControls(vProyectosPrediosDetalle);
                    PrediosEnabled(true);
                    oBasic.AlertSection(msgProyectosPredios, clConstantes.MSG_I, "info");
                    break;
                case "Eliminar":
                    if (!ValidateAccess(cnsSection.PROY_ASOC_PREDIOS, cnsAction.ELIMINAR, true) || gvProyectosPredios.Rows.Count == 0) return;
                    MessageBox1.ShowConfirmation("DeleteProperties", "¿Está seguro de continuar con la acción solicitada?", type: "danger");
                    return;
            }
            oBasic.FixPanel(divData, "ProyectosPredios", 2);
        }
        protected void btnPrediosCancelar_Click(object sender, EventArgs e)
        {
            oBasic.FixPanel(divData, "ProyectosPredios", 0);
            oBasic.AlertSection(msgProyectosPredios, "", "0");
        }
        protected void btnPrediosNavegacion_Click(object sender, EventArgs e)
        {
            int index = 0;
            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    index = Convert.ToInt16(ViewState["IndexProyectosPredios"] ?? "0") - 1;
                    break;
                case "Next":
                    index = Convert.ToInt16(ViewState["IndexProyectosPredios"] ?? "0") + 1;
                    break;
                case "Last":
                    index = ((DataSet)oVar.prDSProyectosPredios).Tables[0].Rows.Count - 1;
                    break;
            }
            gvProyectosPredios.SelectedIndex = index;
            ViewState["IndexProyectosPredios"] = index.ToString();
            PrediosDetail();
            PrediosEnabled(false);
        }
        protected void btnPrediosVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            int index;
            mvProyectosPredios.ActiveViewIndex = cmdArg;
            switch (cmdArg)
            {
                case 0:
                    int iPagina = Convert.ToInt16(ViewState["IndexProyectosPredios"] ?? "0") / gvProyectosPredios.PageSize;
                    if (iPagina > 0)
                        index = Convert.ToInt16(ViewState["IndexProyectosPredios"] ?? "0") % gvProyectosPredios.PageSize;
                    else
                        index = Convert.ToInt16(ViewState["IndexProyectosPredios"] ?? "0");
                    gvProyectosPredios.PageIndex = iPagina;
                    gvProyectosPredios.SelectedIndex = index;
                    break;
                case 1:
                    PrediosDetail();
                    PrediosEnabled(false);
                    break;
            }
            oBasic.FixPanel(divData, "ProyectosPredios", cmdArg);
        }
        protected void gvPredios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                if (rowIndex >= gvProyectosPredios.Rows.Count)
                    rowIndex = 0;
                var chip = rowIndex >= 0 ? gvProyectosPredios.DataKeys[rowIndex]["chip"].ToString() : "0";

                switch (e.CommandName)
                {
                    case "_Go":
                        Session["Retorno.Proyecto.Index"] = ViewState["IndexProyectos"] ?? "0";
                        Session["Retorno.Proyecto.Data"] = (DataSet)oVar.prDSProyectos;
                        Session["Retorno.Proyecto.filter"] = ViewState["CriterioBuscar"].ToString();

                        Session["Proyecto.Predios.chip"] = chip;
                        Response.Redirect("Predios");
                        break;
                    default:
                        return;
                }
            }
        }
        protected void gvPredios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvProyectosPredios, "Select$" + e.Row.RowIndex.ToString()));
            }
        }
        protected void gvPredios_SelectedIndexChanged(object sender, EventArgs e)
        {
            gv_SelectedIndexChanged(gvProyectosPredios);
        }
        protected void gvPredios_Sorting(object sender, GridViewSortEventArgs e)
        {
            gv_Sorting(gvProyectosPredios, e.SortExpression.ToString(), oVar.prDSProyectosPredios);
            oVar.prDSProyectosPredios = oVar.prDataSet;
        }
        protected void txt_chip_TextChanged(object sender, EventArgs e)
        {
            var aux_chip = txt_chip.Text;
            var aux_idproypredio = txt_au_proyecto_predio.Text;
            var aux_cumple_funcion = chk_cumple_funcion_social.Checked;
            var aux_descripcion = txt_observacion__predio.Text;

            if (txt_chip.Text != "")
            {
                DataSet dsPredio = oProyectosPredios.sp_s_predio_chip_buscar(txt_chip.Text, gvProyectos.SelectedDataKey["au_proyecto"].ToString());

                if (dsPredio.Tables[0].Rows.Count > 0)
                {
                    DataRow dRow = dsPredio.Tables[0].Rows[0];
                    var id = dRow["au_proyecto_predio"].ToString();
                    if (id != txt_au_proyecto_predio.Text && id != "")
                    {
                        txt_chip.Text = "";
                        MessageInfo.ShowMessage("El CHIP ingresado ya corresponde al proyecto");
                        return;
                    }
                    oBasic.fValueControls(vProyectosPrediosDetalle, dRow);
                    txt_au_proyecto_predio.Text = aux_idproypredio;
                    PrediosEnabled(true);
                    return;
                }
            }
            oBasic.fClearControls(vProyectosPrediosDetalle);
            txt_chip.Text = aux_chip;
            txt_au_proyecto_predio.Text = aux_idproypredio;
            chk_cumple_funcion_social.Checked = aux_cumple_funcion;
            txt_observacion__predio.Text = aux_descripcion;
            PrediosEnabled(true);
        }
        #endregion

        #region---Predios - Methods
        private void PrediosAdd()
        {
            string strResultado = oProyectosPredios.sp_i_proyecto_predio(
                gvProyectos.SelectedDataKey["au_proyecto"].ToString(),
                txt_chip.Text,
                txt_matricula.Text,
                txt_direccion.Text,
                chk_cumple_funcion_social.Checked,
                txt_observacion__predio.Text
            );
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                oBasic.SPOk(msgMain, msgProyectosPredios, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
            else
                oBasic.SPError(msgMain, msgProyectosPredios, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
        }
        private void PrediosDelete()
        {
            try
            {
                string strResultado = oProyectosPredios.sp_d_proyecto_predio(gvProyectosPredios.SelectedDataKey.Value.ToString());
                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fProyectosPrediosDelete:", clConstantes.MSG_OK_D);
                    oBasic.fClearControls(vProyectosPrediosDetalle);

                    if (Convert.ToInt16(ViewState["IndexProyectosPredios"] ?? "0") > 0)
                        ViewState["IndexProyectosPredios"] = Convert.ToInt16(ViewState["IndexProyectosPredios"] ?? "0") - 1;
                    else
                        ViewState["IndexProyectosPredios"] = 0;

                    oBasic.SPOk(msgMain, msgProyectosPredios, "d", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                    oBasic.SPError(msgMain, msgProyectosPredios, "d", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
            }
            catch { }
        }
        private void PrediosDetail()
        {
            mvProyectosPredios.ActiveViewIndex = 1;
            int Indice = Convert.ToInt16(ViewState["IndexProyectosPredios"] ?? "0");
            DataSet dsTmp = (DataSet)oVar.prDSProyectosPredios;
            if (dsTmp.Tables[0].Rows.Count > 0)
            {
                DataRow dRow = dsTmp.Tables[0].Rows[Indice];

                oBasic.fValueControls(vProyectosPrediosDetalle, dRow);
                if (lbl_fec_auditoria_predios.Text.Trim().Length > 0)
                {
                    lbl_fec_auditoria_predios.Text = "Modificado el: " + lbl_fec_auditoria_predios.Text;
                    lbl_fec_auditoria_predios.ToolTip = lbl_fec_auditoria_predios.Text;
                }
                oBasic.LblRegistros(upProyectosPrediosFoot, dsTmp.Tables[0].Rows.Count, Indice);
            }
        }
        private void PrediosEdit()
        {
            string strResultado = oProyectosPredios.sp_u_proyecto_predio(
                    oBasic.fInt(txt_au_proyecto_predio),
                    txt_chip.Text,
                    txt_matricula.Text,
                    txt_direccion.Text,
                    chk_cumple_funcion_social.Checked,
                    txt_observacion__predio.Text
                );

            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                oBasic.SPOk(msgMain, msgProyectosPredios, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
            else
                oBasic.SPError(msgMain, msgProyectosPredios, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
        }
        private void PrediosEnabled(bool HabilitarCampos)
        {
            txt_au_proyecto_predio.Enabled = false;
            txt_cod_proyecto__predio.Enabled = false;
            txt_chip.Enabled = HabilitarCampos;
            chk_cumple_funcion_social.Enabled = HabilitarCampos;
            txt_observacion__predio.Enabled = HabilitarCampos;

            if (txt_declaratoria.Text != "")
            {
                txt_matricula.Enabled = false;
                txt_direccion.Enabled = false;
                btnGoPredio2.Visible = true && !HabilitarCampos;
            }
            else
            {
                txt_matricula.Enabled = true && HabilitarCampos;
                txt_direccion.Enabled = true && HabilitarCampos;
                btnGoPredio2.Visible = false;
            }
        }
        private void PrediosLoad(string p_cod_proyecto)
        {
            if (string.IsNullOrEmpty(p_cod_proyecto))
                p_cod_proyecto = "0";
            oVar.prDSProyectosPredios = oProyectosPredios.sp_s_proyectos_predios_cod_proyecto(p_cod_proyecto);

            gvProyectosPredios.DataSource = ((DataSet)(oVar.prDSProyectosPredios));
            gvProyectosPredios.DataBind();

            if (gvProyectosPredios.Rows.Count > 0)
            {
                gvProyectosPredios.Visible = true;
                oBasic.FixPanel(divData, "ProyectosPredios", 0);
            }
            else
            {
                gvProyectosPredios.Visible = false;
            }
            gv_Sorting(gvProyectosPredios, ViewState["SortExpProyectosPredios"].ToString(), oVar.prDSProyectosPredios);
            oVar.prDSProyectosPredios = oVar.prDataSet;
            oBasic.FixPanel(divData, "ProyectosPredios", 0);
        }
        #endregion
        #endregion

        #region ---Seguimiento
        #region---Seguimiento - Events
        protected void btnSeguimientoCrear_Click(object sender, EventArgs e)
        {
            if (!ValidateAccess(cnsSection.TRACING, cnsAction.INSERTAR, true)) return;
            MessageBox1.ShowConfirmation("CreateTracing", "Se creará el seguimiento al proyecto ¿desea continuar?", type: "warning");
        }
        #endregion

        #region---Seguimiento - Methods
        private void SeguimientoLoad(string p_idproyecto)
        {
            if (string.IsNullOrEmpty(p_idproyecto))
                p_idproyecto = "0";

            DataSet dSet = oBanco.sp_s_banco_consultar(0, Int32.Parse(p_idproyecto));
            if (dSet.Tables.Count > 0 && dSet.Tables[0].Rows.Count > 0 && dSet.Tables[0].Rows[0]["estado"].ToString() == "1")
            {
                DataRow dRow = dSet.Tables[0].Rows[0];
                hdd_idBanco.Value = dRow["idbanco"].ToString();
                pnlTracing.Visible = true;
                pnlNoTracing.Visible = false;
            }
            else
            {
                pnlTracing.Visible = false;
                pnlNoTracing.Visible = true;
            }
            upTracing.Update();
        }
        private string SeguimientoSaveNew()
        {
            string strResultado = null;
            string idTipoProyecto = null;
            DataSet ds = oIdentidades.sp_s_identidad_id_categoria_op("72", "1");

            int Indice = Convert.ToInt16(ViewState["IndexProyectos"] ?? "0");
            DataSet dsTmp = (DataSet)oVar.prDSProyectos;
            DataRow drProy = dsTmp.Tables[0].Rows[Indice];

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                idTipoProyecto = ds.Tables[0].Rows[0]["id_identidad"].ToString();

            if (drProy != null)
            {
                DataSet dSet = oBanco.sp_s_banco_consultar(0, Int32.Parse(drProy["au_proyecto"].ToString()));
                if (dSet.Tables.Count > 0 && dSet.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dSet.Tables[0].Rows[0];
                    strResultado = oBanco.sp_u_banco_reactivar(
                            dr["idbanco"].ToString());

                    if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                    {
                        oBasic.SPOk(msgMain, msgProyectos, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                        SeguimientoLoad(gvProyectos.SelectedDataKey["au_proyecto"].ToString());
                    }
                    else
                    {
                        oBasic.SPError(msgMain, msgProyectos, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
                        oLog.RegistrarLogInfo(_SOURCEPAGE, "SaveNewTracing:", clConstantes.MSG_ERR_I);
                    }
                }
                else
                {
                    strResultado = oBanco.sp_i_banco(
                        idTipoProyecto,
                        null,
                        drProy["nombre_proyecto"].ToString(),
                        null,
                        drProy["observacion"].ToString(),
                        null,
                        drProy["cod_localidad"].ToString(),
                        null,
                        null,//ddl_idupl,
                        null,
                        null,
                        null,
                        oBasic.fDec(drProy["area_bruta"].ToString()),
                        oBasic.fDec(drProy["area_neta_urbanizable"].ToString()),
                        oBasic.fDec(drProy["area_util"].ToString()),
                        drProy["UP_VIP"].ToString(),
                        drProy["UP_VIS"].ToString(),
                        drProy["UP_no_VIS"].ToString(),
                        null,
                        null,
                        oBasic.fDateTime(drProy["fecha_inicio_ventas"].ToString()),
                        oBasic.fDateTime(drProy["fecha_inicio_obras"].ToString()),
                        drProy["au_proyecto"].ToString(),
                        oBasic.fInt(ddl_cod_usu_responsable));

                    if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                    {
                        oBasic.SPOk(msgMain, msgProyectos, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                        SeguimientoLoad(gvProyectos.SelectedDataKey["au_proyecto"].ToString());
                    }
                    else
                    {
                        oBasic.SPError(msgMain, msgProyectos, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
                        oLog.RegistrarLogInfo(_SOURCEPAGE, "SaveNewTracing:", clConstantes.MSG_ERR_I);
                    }
                }
            }
            return strResultado;
        }

        #endregion

        #endregion

        #region --Visitas sitio
        #region--Visitas sitio - Events
        protected void ucVisita_UserControlException(object sender, Exception ex)
        {
            MessageInfo.ShowMessage("Se ha presentado un error en el sistema: " + ex.Message);
        }
        protected void ucVisita_ViewDoc(object sender)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "verDoc();", true);
        }
        #endregion
        #region--Visitas sitio - Methods
        private void VisitasLoad(string p_idproyecto)
        {
            if (string.IsNullOrEmpty(p_idproyecto))
                p_idproyecto = "0";

            Int32.TryParse(hdd_idvisita.Value, out int idvisita);
            ucVisita.FilePath = oVar.prPathDocumentosProyectos.ToString();
            ucVisita.Prefix = tipoArchivo.IMG_VPA;
            ucVisita.ReferenceID = Convert.ToInt32(p_idproyecto);
            ucVisita.VisitaID = idvisita;
            ucVisita.ResponsibleUserCode = (Session["Proyecto.cod_usu_responsable"] ?? 0).ToString().Replace("-", "");
            ucVisita.LoadControl();

            oBasic.FixPanel(divData, "ProyectosVisitas", 0);
        }
        #endregion
        #endregion



    }
}