using GLOBAL.CONST;
using GLOBAL.DAL;
using GLOBAL.LOG;
using GLOBAL.PERMISOS;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using OfficeOpenXml;
using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cnsSection = GLOBAL.CONST.clConstantes.Section;
using tipoArchivo = GLOBAL.CONST.clConstantes.TipoArchivo;
namespace SIDec
{
    public partial class PlanesP : Page
    {
        #region--------------------------------------------------------------------VARIABLES
        readonly LOCALIDADES_DAL oLocalidades = new LOCALIDADES_DAL();
        readonly UPZ_DAL oUPZ = new UPZ_DAL();
        readonly IDENTIDADES_DAL oIdentidades = new IDENTIDADES_DAL();

        readonly PLANESP_DAL oPlanesP = new PLANESP_DAL();
        readonly PLANESPMANZANAS_DAL oPlanesPManzanas = new PLANESPMANZANAS_DAL();
        readonly PLANESPCESIONES_DAL oPlanesPCesiones = new PLANESPCESIONES_DAL();
        readonly PLANESPACTOS_DAL oPlanesPActos = new PLANESPACTOS_DAL();
        readonly PLANESPLICENCIAS_DAL oPlanesPLicencias = new PLANESPLICENCIAS_DAL();
        readonly PLANESPVISITAS_DAL oPlanesPVisitas = new PLANESPVISITAS_DAL();

        readonly clGlobalVar oVar = new clGlobalVar();
        readonly clFile oFile = new clFile();
        readonly clUtil oUtil = new clUtil();
        readonly clLog oLog = new clLog();
        readonly clPermisos oPermisos = new clPermisos();
        readonly clBasic oBasic = new clBasic();

        string sIndex1;

        private const string _SOURCEPAGE = "Planes parciales";
        #endregion

        #region--Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            btnConfirmarPlanesP.Attributes.Add("onclick", "document.getElementById('" + btnConfirmarPlanesP.ClientID + "').disabled=true;" + ClientScript.GetPostBackEventReference(btnConfirmarPlanesP, "Click") + "; return false;");

            if (!IsPostBack)
            {
                this.Page.Form.Enctype = "multipart/form-data";

                ViewState["CriterioBuscar"] = "";
                ViewState["AccionFinal"] = "";

                InitSort("", "0", "nombre_planp", "ASC");
                InitSort("Manzanas", "0", "manzana", "ASC");
                InitSort("Cesiones", "0", "cesion", "ASC");
                InitSort("Actos", "0", "fecha_acto", "DESC");
                InitSort("Licencias", "0", "fecha_ejecutoria", "DESC");
                InitSort("Visitas", "0", "fecha_visita", "DESC");

                txtBuscar.Focus();
                LoadPlanes(ViewState["CriterioBuscar"].ToString());
                LoadDropDowns();
                lblPlanesPSection.Text = gvPlanesP.Rows[gvPlanesP.SelectedIndex].Cells[2].Text;
                upPlanesPSection.Update();
            }
            ValidarSP();
            oBasic.fValidarFecha_old("fecha iniciación viviendas", cal_fecha_iniciacion_viviendas, rv_fecha_iniciacion_viviendas, "0", 48);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            mvPlanesP.ActiveViewIndex = 0;
            ViewState["CriterioBuscar"] = txtBuscar.Text.Trim();
            LoadPlanes(ViewState["CriterioBuscar"].ToString());
            gvPlanesP_SelectedIndexChanged(null, null);
        }
        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Session["Retorno.Proyecto.Page"] = "PP";
            Response.Redirect(Session["Retorno.PlanesP.Origen"].ToString());
        }
        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            bool permiso = false;
            if (ViewState["AccionFinal"].ToString().Substring(0, 3) == "plp")
            {
                switch (ViewState["AccionFinal"].ToString().Substring(3))
                {
                    case "Editar":
                        if (!oPermisos.TienePermisosSP("sp_u_planp"))
                            break;
                        fSetRangoEdificabilidad();
                        GetPlanesAreas();
                        EditPlanes();
                        permiso = true;
                        break;
                    case "Agregar":
                        if (!oPermisos.TienePermisosSP("sp_i_planp"))
                            break;
                        fSetRangoEdificabilidad();
                        GetPlanesAreas();
                        AddPlanes();
                        permiso = true;
                        break;
                    case "Eliminar":
                        if (!oPermisos.TienePermisosSP("sp_d_planp"))
                            break;
                        DeletePlanes();
                        permiso = true;
                        break;
                }
                if (!permiso)
                {
                    oBasic.AlertMain(msgMain, clConstantes.MSG_ERR_PERMISO, "danger");
                    oBasic.AlertSection(msgPlanesP, "", "0");
                }
                LoadPlanes(ViewState["CriterioBuscar"].ToString());
                oUtil.fSetIndex(gvPlanesP);
                gv_SelectedIndexChanged(gvPlanesP);
                LoadPlanesChildGrid(gvPlanesP.SelectedDataKey["au_planp"].ToString());
                oBasic.FixPanel(divData, "PlanesP", 0);
            }

            //PLANESP_Manzanas
            else if (ViewState["AccionFinal"].ToString().Substring(0, 3) == "mzn")
            {
                switch (ViewState["AccionFinal"].ToString().Substring(3))
                {
                    case "Editar":
                        if (!oPermisos.TienePermisosSP("sp_u_planp_manzana"))
                            break;
                        fPlanesPManzanasUpdate();
                        permiso = true;
                        break;
                    case "Agregar":
                        if (!oPermisos.TienePermisosSP("sp_i_planp_manzana"))
                            break;
                        fPlanesPManzanasInsert();
                        permiso = true;
                        break;
                    case "Eliminar":
                        if (!oPermisos.TienePermisosSP("sp_d_planp_manzana"))
                            break;
                        fPlanesPManzanasDelete();
                        permiso = true;
                        break;
                }
                if (!permiso)
                {
                    oBasic.AlertMain(msgMain, clConstantes.MSG_ERR_PERMISO, "danger");
                    oBasic.AlertSection(msgPlanesPManzanas, "", "0");
                }
                oVar.prIndexValue = ViewState["IndexPlanesP"];
                sIndex1 = ViewState["IndexPlanesPManzanas"].ToString();
                LoadPlanes(ViewState["CriterioBuscar"].ToString());
                oUtil.fSetIndex(gvPlanesP);
                gv_SelectedIndexChanged(gvPlanesP);
                fPlanesPManzanasLoadGV(gvPlanesP.SelectedDataKey["au_planp"].ToString());
                upPlanesPManzanas.Update();
                ViewState["IndexPlanesPManzanas"] = sIndex1;
                oVar.prIndexValue = sIndex1;
                oUtil.fSetIndex(gvPlanesPManzanas);
                gv_SelectedIndexChanged(gvPlanesPManzanas);
                oBasic.FixPanel(divData, "PlanesPManzanas", 0);
            }

            //PLANESP_Cesiones
            else if (ViewState["AccionFinal"].ToString().Substring(0, 3) == "ces")
            {
                switch (ViewState["AccionFinal"].ToString().Substring(3))
                {
                    case "Editar":
                        if (!oPermisos.TienePermisosSP("sp_u_planp_cesion"))
                            break;
                        fPlanesPCesionesUpdate();
                        permiso = true;
                        break;
                    case "Agregar":
                        if (!oPermisos.TienePermisosSP("sp_i_planp_cesion"))
                            break;
                        fPlanesPCesionesInsert();
                        permiso = true;
                        break;
                    case "Eliminar":
                        if (!oPermisos.TienePermisosSP("sp_d_planp_cesion"))
                            break;
                        fPlanesPCesionesDelete();
                        permiso = true;
                        break;
                }
                if (!permiso)
                {
                    oBasic.AlertMain(msgMain, clConstantes.MSG_ERR_PERMISO, "danger");
                    oBasic.AlertSection(msgPlanesPCesiones, "", "0");
                }
                oVar.prIndexValue = ViewState["IndexPlanesP"];
                sIndex1 = ViewState["IndexPlanesPCesiones"].ToString();
                LoadPlanes(ViewState["CriterioBuscar"].ToString());
                oUtil.fSetIndex(gvPlanesP);
                gv_SelectedIndexChanged(gvPlanesP);
                fPlanesPCesionesLoadGV(gvPlanesP.SelectedDataKey["au_planp"].ToString());
                upPlanesPCesiones.Update();
                ViewState["IndexPlanesPCesiones"] = sIndex1;
                oVar.prIndexValue = sIndex1;
                oUtil.fSetIndex(gvPlanesPCesiones);
                gv_SelectedIndexChanged(gvPlanesPCesiones);
            }

            //PLANESP_Actos
            else if (ViewState["AccionFinal"].ToString().Substring(0, 3) == "act")
            {
                switch (ViewState["AccionFinal"].ToString().Substring(3))
                {
                    case "Editar":
                        if (!oPermisos.TienePermisosSP("sp_u_planp_acto"))
                            break;
                        fPlanesPActosUpdate();
                        permiso = true;
                        break;
                    case "Agregar":
                        if (!oPermisos.TienePermisosSP("sp_u_planp_acto"))
                            break;
                        fPlanesPActosInsert();
                        permiso = true;
                        break;
                    case "Eliminar":
                        if (!oPermisos.TienePermisosSP("sp_d_planp_acto"))
                            break;
                        fPlanesPActosDelete();
                        permiso = true;
                        oBasic.FileDelete(oVar.prPathPlanesPActos.ToString() + gvPlanesP.SelectedDataKey["au_planp"].ToString() + ".pdf");
                        break;
                }
                fPlanesPActosLoadGV(gvPlanesP.SelectedDataKey["au_planp"].ToString());
                oUtil.fSetIndex(gvPlanesPActos);
                gv_SelectedIndexChanged(gvPlanesPActos);
            }

            //PLANESP_Licencias
            else if (ViewState["AccionFinal"].ToString().Substring(0, 3) == "lic")
            {
                switch (ViewState["AccionFinal"].ToString().Substring(3))
                {
                    case "Editar":
                        if (!oPermisos.TienePermisosSP("sp_u_planp_licencia"))
                            break;
                        fPlanesPLicenciasUpdate();
                        permiso = true;
                        break;
                    case "Agregar":
                        if (!oPermisos.TienePermisosSP("sp_u_planp_licencia"))
                            break;
                        fPlanesPLicenciasInsert();
                        permiso = true;
                        break;
                    case "Eliminar":
                        if (!oPermisos.TienePermisosSP("sp_d_planp_licencia"))
                            break;
                        fPlanesPLicenciasDelete();
                        permiso = true;
                        oBasic.FileDelete(oVar.prPathPlanesPLicencias.ToString() + gvPlanesP.SelectedDataKey["au_planp"].ToString() + ".pdf");
                        break;
                }
                fPlanesPLicenciasLoadGV(gvPlanesP.SelectedDataKey["au_planp"].ToString());
                oUtil.fSetIndex(gvPlanesPLicencias);
                gv_SelectedIndexChanged(gvPlanesPLicencias);
            }

            //PLANESP_Visitas
            else if (ViewState["AccionFinal"].ToString().Substring(0, 3) == "vis")
            {
                switch (ViewState["AccionFinal"].ToString().Substring(3))
                {
                    case "Editar":
                        if (!oPermisos.TienePermisosSP("sp_u_planp_visita"))
                            break;
                        fPlanesPVisitasUpdate();
                        permiso = true;
                        break;
                    case "Agregar":
                        if (!oPermisos.TienePermisosSP("sp_u_planp_visita"))
                            break;
                        fPlanesPVisitasInsert();
                        permiso = true;
                        break;
                    case "Eliminar":
                        if (!oPermisos.TienePermisosSP("sp_d_planp_visita"))
                            break;
                        fPlanesPVisitasDelete();
                        permiso = true;
                        oBasic.FileDelete(oVar.prPathPlanesPVisitas.ToString() + gvPlanesPVisitas.SelectedDataKey.Value.ToString() + ".pdf");
                        break;
                }
                fPlanesPVisitasLoadGV(gvPlanesP.SelectedDataKey["au_planp"].ToString());
                oUtil.fSetIndex(gvPlanesPVisitas);
                gv_SelectedIndexChanged(gvPlanesPVisitas);
            }

            Session["ReloadXFU"] = "1";
            (this.Master as AuthenticNew).fReload();
        }



        #region---PLANESP
        protected void btnPlanesPAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = "plp" + btnAccionSource.CommandName;
            oVar.prViewState = btnAccionSource.CommandName.ToString();
            oVar.prIndexValue = gvPlanesP.SelectedIndex;

            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    if (!oPermisos.TienePermisosSP("sp_u_planp"))
                        return;
                    LoadPlanesDetail();
                    EnablePlanes(true);
                    oBasic.AlertSection(msgPlanesP, clConstantes.MSG_U, "warning");
                    break;
                case "Agregar":
                    if (!oPermisos.TienePermisosSP("sp_i_planp"))
                        return;
                    oBasic.fClearControls(vPlanesPDetalle);
                    EnablePlanes(true);
                    //mvPlanesP.ActiveViewIndex = 1;
                    gvPlanesP.SelectedIndex = -1;
                    LoadPlanesChildGrid("-1");
                    oBasic.AlertSection(msgPlanesP, clConstantes.MSG_I, "info");
                    break;
                case "Eliminar":
                    if (!oPermisos.TienePermisosSP("sp_d_planp"))
                        return;
                    oBasic.AlertSection(msgPlanesP, clConstantes.MSG_D, "danger");
                    return;
            }
            oBasic.FixPanel(divData, "PlanesP", 2);
        }
        protected void btnPlanesPCancelar_Click(object sender, EventArgs e)
        {
            oBasic.FixPanel(divData, "PlanesP", 0);
            oBasic.AlertSection(msgPlanesP, "", "0");
        }
        protected void btnPlanesPNavegacion_Click(object sender, EventArgs e)
        {
            int index = 0;
            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    index = Convert.ToInt16(ViewState["IndexPlanesP"]) - 1;
                    break;
                case "Next":
                    index = Convert.ToInt16(ViewState["IndexPlanesP"]) + 1;
                    break;
                case "Last":
                    index = ((DataSet)oVar.prDSPlanesP).Tables[0].Rows.Count - 1;
                    break;
            }
            gvPlanesP.SelectedIndex = index;
            ViewState["IndexPlanesP"] = index.ToString();
            EnablePlanes(false);
            LoadPlanesDetail();
            LoadPlanesChildGrid(txt_au_planp.Text);
        }
        protected void btnPlanesPVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            int index = 0;
            switch (cmdArg)
            {
                case 0:
                    int iPagina = Convert.ToInt16(ViewState["IndexPlanesP"]) / gvPlanesP.PageSize;
                    if (iPagina > 0)
                        index = Convert.ToInt16(ViewState["IndexPlanesP"]) % gvPlanesP.PageSize;
                    else
                        index = Convert.ToInt16(ViewState["IndexPlanesP"]);
                    gvPlanesP.PageIndex = iPagina;
                    gvPlanesP.SelectedIndex = index;
                    break;
                case 1:
                    index = Convert.ToInt16(ViewState["IndexPlanesP"]);
                    EnablePlanes(false);
                    LoadPlanesDetail();
                    break;
            }
            oBasic.FixPanel(divData, "PlanesP", cmdArg);
        }
        protected void btnPlanesPSub_Click(object sender, EventArgs e)
        {
            int oldIndex = mvPlanesPSub.ActiveViewIndex;
            int newIndex = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            string oldID = "lbPlanesPSub_" + oldIndex.ToString();
            string newID = "lbPlanesPSub_" + newIndex.ToString();
            LinkButton lbOld = (LinkButton)ulPlanesPSub.FindControl(oldID);
            LinkButton lbNew = (LinkButton)ulPlanesPSub.FindControl(newID);
            oBasic.ActiveNav(mvPlanesPSub, lbOld, lbNew, newIndex);
        }
        protected void btnPlanesPSection_Click(object sender, EventArgs e)
        {
            int oldIndex = mvPlanesPSection.ActiveViewIndex;
            int newIndex = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            string oldID = "lbPlanesPSection_" + oldIndex.ToString();
            string newID = "lbPlanesPSection_" + newIndex.ToString();
            LinkButton lbOld = (LinkButton)ulPlanesPSection.FindControl(oldID);
            LinkButton lbNew = (LinkButton)ulPlanesPSection.FindControl(newID);
            oBasic.ActiveNav(mvPlanesPSection, lbOld, lbNew, newIndex);
        }
        protected void btnPlanesPDoc1_Click(object sender, EventArgs e)
        {
            if (gvPlanesP.Rows.Count > 0)
            {
                string _plantilla_planesp = oVar.prPathFormatosOrigen.ToString() + oVar.prPathPlanesPDoc1.ToString();

                if (File.Exists(_plantilla_planesp))
                {
                    //Archivo Excel del cual crear la copia:
                    FileInfo templateFile = new FileInfo(_plantilla_planesp);

                    string file_name = "PlanP_" + Server.HtmlDecode(gvPlanesP.Rows[gvPlanesP.SelectedIndex].Cells[2].Text) + ".xlsx";

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (ExcelPackage pck = new ExcelPackage(templateFile, true))
                    {
                        //Abrir primera Hoja del Archivo Excel que se crea'
                        ExcelWorksheet ws = pck.Workbook.Worksheets[0];

                        if (mvPlanesP.ActiveViewIndex == 0)
                            LoadPlanesDetail();

                        GridView gv = null;
                        GridViewRow gvr = null;
                        int k = 0;
                        int iRow = 5;
                        int iActos = 0;
                        int iManzanas = 0;
                        string new_path = "";
                        int img_w = 350;
                        int img_h = 260;


                        // general
                        oUtil.fExcelWrite(ws, "G" + iRow.ToString(), ddlb_id_tipo_tratamiento.SelectedItem.Text);
                        oUtil.fExcelWrite(ws, "U" + iRow.ToString(), txt_nombre_planp.Text);
                        oUtil.fExcelWrite(ws, "AQ" + iRow.ToString(), ddlb_cod_localidad.SelectedItem.Text);

                        // actos
                        iRow++;
                        gv = gvPlanesPActos;

                        if (gv.Rows.Count > 0)
                        {
                            bool b = false;
                            if (ViewState["SortExpPlanesPActos"].ToString() != "fecha_acto")
                                b = true;
                            if (ViewState["SortDirPlanesPActos"].ToString() != "DESC")
                                b = true;
                            if (b)
                                gv_Sorting(gvPlanesPActos, "fecha_acto", "ASC", oVar.prDSPlanesPActos);

                            b = false;
                            for (int i = 0; i < gv.Rows.Count; i++)
                            {
                                gvr = gv.Rows[i];
                                string obs = Server.HtmlDecode(gvr.Cells[4].Text);
                                if (Server.HtmlDecode(gvr.Cells[1].Text) == "Decreto")
                                {
                                    if (obs.Contains("adopta") || obs.Contains("adopci") || obs.Contains("modifica"))
                                    {
                                        if (b)
                                        {
                                            ws.InsertRow(iRow, 1);
                                            ws.Cells[7, 1, 7, 50].Copy(ws.Cells[iRow, 1]);
                                            ws.Row(iRow).Height = 30 / 4 * 3;
                                            iActos++;
                                        }

                                        oUtil.fExcelWrite(ws, "D" + iRow.ToString(), gvr.Cells[2].Text);
                                        oUtil.fExcelWrite(ws, "M" + iRow.ToString(), gvr.Cells[3].Text);
                                        oUtil.fExcelWrite(ws, "R" + iRow.ToString(), gvr.Cells[4].Text);

                                        int m = 0;
                                        decimal n = 0;
                                        if (Server.HtmlDecode(gvr.Cells[4].Text).Length > 80)
                                            n = HttpUtility.HtmlDecode(gvr.Cells[4].Text).Length / 80;
                                        m = Convert.ToInt32(Math.Floor(n));
                                        ws.Row(iRow).Height = 30 / 4 * 3 + (m * 11 / 4 * 3);
                                        b = true;
                                    }
                                }
                            }
                        }

                        using (ExcelRange rng = ws.Cells[iRow + iActos, 1, iRow + iActos, 50])
                        {
                            rng.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            rng.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                        }

                        // urbanística
                        iRow = 10 + iActos;
                        oUtil.fExcelWrite(ws, "A" + iRow, ddlb_id_clasificacion_suelo.SelectedItem.Text);
                        oUtil.fExcelWrite(ws, "K" + iRow, txt_areas_zonas_desc.Text);
                        oUtil.fExcelWrite(ws, "AJ" + iRow, txt_rango_edificabilidad_desc.Text);
                        oUtil.fExcelWrite(ws, "AS" + iRow, txt_area_bruta.Text);

                        iRow += 2;
                        oUtil.fExcelWrite(ws, "A" + iRow, txt_area_afectaciones.Text);
                        oUtil.fExcelWrite(ws, "G" + iRow, txt_SP_cesion_control_ambiental.Text);
                        oUtil.fExcelWrite(ws, "M" + iRow, txt_area_neta_urbanizable.Text);
                        oUtil.fExcelWrite(ws, "U" + iRow, txt_SP_cesion_total.Text);
                        oUtil.fExcelWrite(ws, "AA" + iRow, txt_SP_cesion_parque.Text);
                        oUtil.fExcelWrite(ws, "AG" + iRow, txt_SP_cesion_equipamiento.Text);
                        oUtil.fExcelWrite(ws, "AM" + iRow, txt_SP_cesion_vias_locales.Text);
                        oUtil.fExcelWrite(ws, "AS" + iRow, txt_area_util.Text);

                        // aportes
                        iRow = 15 + iActos;
                        oUtil.fExcelWrite(ws, "H" + iRow, txt_unidades_potencial_vivienda.Text);
                        iRow++;
                        oUtil.fExcelWrite(ws, "H" + iRow, txt_habitantes_vivienda.Text);

                        // vivienda social
                        iRow = 20 + iActos;
                        oUtil.fExcelWrite(ws, "D" + iRow, txt_decreto_obligacion.Text);

                        if (Convert.ToDecimal(txt_SP_VIS.Text) > 0)
                        {
                            oUtil.fExcelWrite(ws, "AI" + iRow, chk_es_obligacion_VIS__planp.Checked ? "Si" : "No");
                            oUtil.fExcelWrite(ws, "AM" + iRow, chk_es_suelo_desarrollo_prioritario.Checked ? "Si" : "No");
                        }
                        else
                        {
                            oUtil.fExcelWrite(ws, "AI" + iRow, "N/A");
                            oUtil.fExcelWrite(ws, "AM" + iRow, "N/A");
                        }

                        iRow = 21 + iActos;
                        if (Convert.ToDecimal(txt_SP_VIP.Text) > 0)
                        {
                            oUtil.fExcelWrite(ws, "AI" + iRow, chk_es_obligacion_VIS__planp.Checked ? "Si" : "No");
                            oUtil.fExcelWrite(ws, "AM" + iRow, "N/A");
                        }
                        else
                        {
                            oUtil.fExcelWrite(ws, "AI" + iRow, "N/A");
                            oUtil.fExcelWrite(ws, "AM" + iRow, "N/A");
                        }

                        // productos inmobiliarios
                        iRow = 25 + iActos;
                        oUtil.fExcelWrite(ws, "K" + iRow, txt_unidades_potencial_VIP.Text);
                        oUtil.fExcelWrite(ws, "S" + iRow, txt_unidades_potencial_VIS.Text);
                        oUtil.fExcelWrite(ws, "AA" + iRow, txt_unidades_potencial_no_VIS.Text);
                        iRow = 26 + iActos;
                        oUtil.fExcelWrite(ws, "K" + iRow, txt_unidades_ejecutadas_VIP.Text);
                        oUtil.fExcelWrite(ws, "S" + iRow, txt_unidades_ejecutadas_VIS.Text);
                        oUtil.fExcelWrite(ws, "AA" + iRow, txt_unidades_ejecutadas_no_VIS.Text);

                        iRow = 29 + iActos;
                        oUtil.fExcelWrite(ws, "K" + iRow, txt_SP_VIP.Text);
                        oUtil.fExcelWrite(ws, "S" + iRow, txt_SP_VIS.Text);
                        oUtil.fExcelWrite(ws, "AA" + iRow, txt_SP_no_VIS.Text);
                        iRow = 30 + iActos;
                        oUtil.fExcelWrite(ws, "K" + iRow, txt_SE_VIP.Text);
                        oUtil.fExcelWrite(ws, "S" + iRow, txt_SE_VIS.Text);
                        oUtil.fExcelWrite(ws, "AA" + iRow, txt_SE_no_VIS.Text);

                        // imágenes propuesta urbanística
                        iRow = 35 + iActos - 1;
                        new_path = oVar.prPathPlanesP.ToString() + "\\" + gvPlanesP.SelectedDataKey["au_planp"].ToString() + "\\" + "U1";
                        oUtil.fExcelInsertImage(ws, iRow, 1, new_path, img_w, img_h);
                        new_path = oVar.prPathPlanesP.ToString() + "\\" + gvPlanesP.SelectedDataKey["au_planp"].ToString() + "\\" + "U2";
                        oUtil.fExcelInsertImage(ws, iRow, 18, new_path, img_w, img_h);
                        new_path = oVar.prPathPlanesP.ToString() + "\\" + gvPlanesP.SelectedDataKey["au_planp"].ToString() + "\\" + "U3";
                        oUtil.fExcelInsertImage(ws, iRow, 35, new_path, img_w, img_h);

                        // manzanas
                        gv = gvPlanesPManzanas;
                        iRow = 41 + iActos;
                        //iManzanas = gv.Rows.Count;
                        if (gv.Rows.Count > 10)
                        {
                            iManzanas = gv.Rows.Count - 10;
                            ws.InsertRow(iRow + 1, iManzanas);
                            for (int i = iRow + 1; i <= iRow + iManzanas; i++)
                            {
                                ws.Cells[iRow, 1, iRow, 38].Copy(ws.Cells[i, 1, i, 1]);
                                ws.Row(i).Height = 30 / 4 * 3;
                            }

                            using (ExcelRange rng = ws.Cells[iRow, 39, iRow + gv.Rows.Count, 50])
                            {
                                rng.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                                rng.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                            }
                        }

                        if (gv.Rows.Count > 0)
                        {
                            gvr = gv.Rows[0];
                            k = 0;
                            for (int i = iRow; i <= gv.Rows.Count + iRow - 1; i++)
                            {
                                k = i - iRow;
                                gvr = gv.Rows[k];
                                ws.Cells["A" + i].Value = Server.HtmlDecode(gvr.Cells[1].Text);
                                ws.Cells["I" + i].Value = Server.HtmlDecode(gvr.Cells[2].Text);
                                ws.Cells["O" + i].Value = Server.HtmlDecode(gvr.Cells[3].Text);
                                try
                                {
                                    ws.Cells["W" + i].Value = Convert.ToDecimal(Server.HtmlDecode(gvr.Cells[5].Text.Replace("%", ""))) / 100;
                                }
                                catch
                                {
                                    ws.Cells["W" + i].Value = Server.HtmlDecode(gvr.Cells[5].Text);
                                }
                                ws.Cells["AB" + i].Value = Convert.ToDecimal(Server.HtmlDecode(gvr.Cells[4].Text));
                            }
                        }

                        // imágenes desarrollo
                        iRow = 52 + iActos + iManzanas;
                        new_path = oVar.prPathPlanesP.ToString() + "\\" + gvPlanesP.SelectedDataKey["au_planp"].ToString() + "\\" + "E1";
                        oUtil.fExcelInsertImage(ws, iRow, 1, new_path, img_w, img_h);
                        new_path = oVar.prPathPlanesP.ToString() + "\\" + gvPlanesP.SelectedDataKey["au_planp"].ToString() + "\\" + "E2";
                        oUtil.fExcelInsertImage(ws, iRow, 18, new_path, img_w, img_h);
                        new_path = oVar.prPathPlanesP.ToString() + "\\" + gvPlanesP.SelectedDataKey["au_planp"].ToString() + "\\" + "E3";
                        oUtil.fExcelInsertImage(ws, iRow, 35, new_path, img_w, img_h);

                        // visitas plan parcial / imágenes
                        iRow = 58 + iActos + iManzanas;
                        if (gvPlanesPVisitas.Rows.Count > 0)
                        {
                            bool b = false;
                            if (ViewState["SortExpPlanesPVisitas"].ToString() != "fecha_visita")
                                b = true;
                            if (ViewState["SortDirPlanesPVisitas"].ToString() != "DESC")
                                b = true;
                            if (b)
                                gv_Sorting(gvPlanesPVisitas, "fecha_visita", "ASC", oVar.prDSPlanesPVisitas);

                            gvPlanesPVisitas.SelectedIndex = 0;
                            gvr = gvPlanesPVisitas.Rows[gvPlanesPVisitas.SelectedIndex];
                            ws.Cells["E" + iRow].Value = Server.HtmlDecode(gvr.Cells[1].Text);
                            string path = oVar.prPathPlanesPVisitas.ToString() + "\\";
                            oUtil.fExcelInsertImage(ws, iRow, 1, path + gvPlanesPVisitas.SelectedDataKey.Value.ToString() + "\\" + gvPlanesPVisitas.SelectedDataKey.Value.ToString() + "_1", img_w, img_h);
                            oUtil.fExcelInsertImage(ws, iRow, 18, path + gvPlanesPVisitas.SelectedDataKey.Value.ToString() + "\\" + gvPlanesPVisitas.SelectedDataKey.Value.ToString() + "_2", img_w, img_h);
                            oUtil.fExcelInsertImage(ws, iRow, 35, path + gvPlanesPVisitas.SelectedDataKey.Value.ToString() + "\\" + gvPlanesPVisitas.SelectedDataKey.Value.ToString() + "_3", img_w, img_h);

                            if (gvPlanesPVisitas.Rows.Count > 1)
                            {
                                gvPlanesPVisitas.SelectedIndex = 1;
                                gvr = gvPlanesPVisitas.Rows[gvPlanesPVisitas.SelectedIndex];
                                iRow += 4;
                                ws.Cells["E" + iRow].Value = Server.HtmlDecode(gvr.Cells[1].Text);
                                oUtil.fExcelInsertImage(ws, iRow, 1, path + gvPlanesPVisitas.SelectedDataKey.Value.ToString() + "\\" + gvPlanesPVisitas.SelectedDataKey.Value.ToString() + "_1", img_w, img_h);
                                oUtil.fExcelInsertImage(ws, iRow, 18, path + gvPlanesPVisitas.SelectedDataKey.Value.ToString() + "\\" + gvPlanesPVisitas.SelectedDataKey.Value.ToString() + "_2", img_w, img_h);
                                oUtil.fExcelInsertImage(ws, iRow, 35, path + gvPlanesPVisitas.SelectedDataKey.Value.ToString() + "\\" + gvPlanesPVisitas.SelectedDataKey.Value.ToString() + "_3", img_w, img_h);
                            }

                            if (b)
                            {
                                gv_Sorting(gvPlanesPVisitas, ViewState["SortExpPlanesPVisitas"].ToString(), ViewState["SortDirPlanesPVisitas"].ToString(), oVar.prDSPlanesPVisitas);
                                gvPlanesPVisitas.SelectedIndex = Convert.ToInt32(ViewState["IndexPlanesPVisitas"].ToString());
                            }
                        }

                        ws.Cells["1:1"].Clear();
                        ws.Row(1).Height = 0;
                        oUtil.fExcelHeaderFooter(ws, "Ficha Plan Parcial", 0, 0, false);
                        oUtil.fExcelSave(pck, file_name);

                        oBasic.SPOk(msgMain, msgPlanesP, "d", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                    }
                }
                else
                    oBasic.SPError(msgMain, msgPlanesP, "d", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, clConstantes.PLANTILLA_NO_EXISTE);
            }
        }
        protected void ddlb_cod_localidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDropDownUPZ();
        }
        #endregion

        #region---PLANESP_Manzanas
        protected void btnPlanesPManzanasAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = "mzn" + btnAccionSource.CommandName;
            oVar.prIndexValue = gvPlanesPManzanas.SelectedIndex;

            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    if (!oPermisos.TienePermisosSP("sp_u_planp_manzana") || gvPlanesPManzanas.Rows.Count == 0)
                        return;
                    fPlanesPManzanasDetalle();
                    fPlanesPManzanasEstadoDetalle(true);
                    oBasic.AlertSection(msgPlanesPManzanas, clConstantes.MSG_U, "warning");
                    oVar.prHayCambiosPlanesPManzanas = false;
                    break;
                case "Agregar":
                    if (!oPermisos.TienePermisosSP("sp_i_planp_manzana"))
                        return;
                    oBasic.fClearControls(vPlanesPManzanasDetalle);
                    fPlanesPManzanasEstadoDetalle(true);
                    oBasic.AlertSection(msgPlanesPManzanas, clConstantes.MSG_I, "info");
                    break;
                case "Eliminar":
                    if (!oPermisos.TienePermisosSP("sp_d_planp_manzana") || gvPlanesPManzanas.Rows.Count == 0)
                        return;
                    oBasic.AlertSection(msgPlanesPManzanas, clConstantes.MSG_D, "danger");
                    return;
            }
            oBasic.FixPanel(divData, "PlanesPManzanas", 2);
        }

        protected void btnPlanesPManzanasCancelar_Click(object sender, EventArgs e)
        {
            oBasic.FixPanel(divData, "PlanesPManzanas", 0);
            oBasic.AlertSection(msgPlanesPManzanas, "", "0");
        }

        protected void btnPlanesPManzanasNavegacion_Click(object sender, EventArgs e)
        {
            int index = 0;
            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    index = Convert.ToInt16(ViewState["IndexPlanesPManzanas"]) - 1;
                    break;
                case "Next":
                    index = Convert.ToInt16(ViewState["IndexPlanesPManzanas"]) + 1;
                    break;
                case "Last":
                    index = ((DataSet)oVar.prDSPlanesPManzanas).Tables[0].Rows.Count - 1;
                    break;
            }
            gvPlanesPManzanas.SelectedIndex = index;
            ViewState["IndexPlanesPManzanas"] = index.ToString();
            fPlanesPManzanasEstadoDetalle(false);
            fPlanesPManzanasDetalle();
        }

        protected void btnPlanesPManzanasVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            int index = 0;
            mvPlanesPManzanas.ActiveViewIndex = cmdArg;
            switch (cmdArg)
            {
                case 0:
                    int iPagina = Convert.ToInt16(ViewState["IndexPlanesPManzanas"]) / gvPlanesPManzanas.PageSize;
                    if (iPagina > 0)
                        index = Convert.ToInt16(ViewState["IndexPlanesPManzanas"]) % gvPlanesPManzanas.PageSize;
                    else
                        index = Convert.ToInt16(ViewState["IndexPlanesPManzanas"]);
                    gvPlanesPManzanas.PageIndex = iPagina;
                    gvPlanesPManzanas.SelectedIndex = index;
                    break;
                case 1:
                    index = Convert.ToInt16(ViewState["IndexPlanesPManzanas"]);
                    fPlanesPManzanasEstadoDetalle(false);
                    fPlanesPManzanasDetalle();
                    break;
            }
            oBasic.FixPanel(divData, "PlanesPManzanas", cmdArg);
        }
        #endregion

        #region---PLANESP_Cesiones
        protected void btnPlanesPCesionesAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = "ces" + btnAccionSource.CommandName;
            oVar.prIndexValue = gvPlanesPCesiones.SelectedIndex;

            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    if (!oPermisos.TienePermisosSP("sp_u_planp_cesion") || gvPlanesPCesiones.Rows.Count == 0)
                        return;
                    fPlanesPCesionesDetalle();
                    fPlanesPCesionesEstadoDetalle(true);
                    oBasic.AlertSection(msgPlanesPCesiones, clConstantes.MSG_U, "warning");
                    oVar.prHayCambiosPlanesPCesiones = false;
                    break;
                case "Agregar":
                    if (!oPermisos.TienePermisosSP("sp_i_planp_cesion"))
                        return;
                    oBasic.fClearControls(vPlanesPCesionesDetalle);
                    fPlanesPCesionesEstadoDetalle(true);
                    oBasic.AlertSection(msgPlanesPCesiones, clConstantes.MSG_I, "info");
                    break;
                case "Eliminar":
                    if (!oPermisos.TienePermisosSP("sp_d_planp_cesion") || gvPlanesPCesiones.Rows.Count == 0)
                        return;
                    oBasic.AlertSection(msgPlanesPCesiones, clConstantes.MSG_D, "danger");
                    return;
            }
            oBasic.FixPanel(divData, "PlanesPCesiones", 2);
        }

        protected void btnPlanesPCesionesCancelar_Click(object sender, EventArgs e)
        {
            oBasic.FixPanel(divData, "PlanesPCesiones", 0);
            oBasic.AlertSection(msgPlanesPCesiones, "", "0");
        }

        protected void btnPlanesPCesionesNavegacion_Click(object sender, EventArgs e)
        {
            int index = 0;
            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    index = Convert.ToInt16(ViewState["IndexPlanesPCesiones"]) - 1;
                    break;
                case "Next":
                    index = Convert.ToInt16(ViewState["IndexPlanesPCesiones"]) + 1;
                    break;
                case "Last":
                    index = ((DataSet)oVar.prDSPlanesPCesiones).Tables[0].Rows.Count - 1;
                    break;
            }
            gvPlanesPCesiones.SelectedIndex = index;
            ViewState["IndexPlanesPCesiones"] = index.ToString();
            fPlanesPCesionesEstadoDetalle(false);
            fPlanesPCesionesDetalle();
        }

        protected void btnPlanesPCesionesVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            int index = 0;
            mvPlanesPCesiones.ActiveViewIndex = cmdArg;
            switch (cmdArg)
            {
                case 0:
                    int iPagina = Convert.ToInt16(ViewState["IndexPlanesPCesiones"]) / gvPlanesPCesiones.PageSize;
                    if (iPagina > 0)
                        index = Convert.ToInt16(ViewState["IndexPlanesPCesiones"]) % gvPlanesPCesiones.PageSize;
                    else
                        index = Convert.ToInt16(ViewState["IndexPlanesPCesiones"]);
                    gvPlanesPCesiones.PageIndex = iPagina;
                    gvPlanesPCesiones.SelectedIndex = index;
                    break;
                case 1:
                    index = Convert.ToInt16(ViewState["IndexPlanesPCesiones"]);
                    fPlanesPCesionesEstadoDetalle(false);
                    fPlanesPCesionesDetalle();
                    break;
            }
            oBasic.FixPanel(divData, "PlanesPCesiones", cmdArg);
        }
        #endregion

        #region---PLANESP_Actos
        protected void btnPlanesPActosAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = "act" + btnAccionSource.CommandName;
            oVar.prIndexValue = gvPlanesPActos.SelectedIndex;

            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    if (!oPermisos.TienePermisosSP("sp_u_planp_acto") || gvPlanesPActos.Rows.Count == 0)
                        return;
                    fPlanesPActosDetalle();
                    fPlanesPActosEstadoDetalle(true);
                    oBasic.AlertSection(msgPlanesPActos, clConstantes.MSG_U, "warning");
                    oVar.prHayCambiosPlanesPActos = false;
                    break;
                case "Agregar":
                    if (!oPermisos.TienePermisosSP("sp_i_planp_acto"))
                        return;
                    oBasic.fClearControls(vPlanesPActosDetalle);
                    fPlanesPActosEstadoDetalle(false);
                    oBasic.AlertSection(msgPlanesPActos, clConstantes.MSG_I, "info");
                    break;
                case "Eliminar":
                    if (!oPermisos.TienePermisosSP("sp_d_planp_acto") || gvPlanesPActos.Rows.Count == 0)
                        return;
                    oBasic.AlertSection(msgPlanesPActos, clConstantes.MSG_D, "danger");
                    return;
            }
            oBasic.FixPanel(divData, "PlanesPActos", 2);
        }

        protected void btnPlanesPActosCancelar_Click(object sender, EventArgs e)
        {
            oBasic.FixPanel(divData, "PlanesPActos", 0);
            oBasic.AlertSection(msgPlanesPActos, "", "0");
        }

        protected void btnPlanesPActosNavegacion_Click(object sender, EventArgs e)
        {
            int index = 0;
            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    index = Convert.ToInt16(ViewState["IndexPlanesPActos"]) - 1;
                    break;
                case "Next":
                    index = Convert.ToInt16(ViewState["IndexPlanesPActos"]) + 1;
                    break;
                case "Last":
                    index = ((DataSet)oVar.prDSPlanesPActos).Tables[0].Rows.Count - 1;
                    break;
            }
            gvPlanesPActos.SelectedIndex = index;
            ViewState["IndexPlanesPActos"] = index.ToString();
            fPlanesPActosEstadoDetalle(false);
            fPlanesPActosDetalle();
        }

        protected void btnPlanesPActosVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            int index = 0;
            mvPlanesPActos.ActiveViewIndex = cmdArg;
            switch (cmdArg)
            {
                case 0:
                    int iPagina = Convert.ToInt16(ViewState["IndexPlanesPActos"]) / gvPlanesPActos.PageSize;
                    if (iPagina > 0)
                        index = Convert.ToInt16(ViewState["IndexPlanesPActos"]) % gvPlanesPActos.PageSize;
                    else
                        index = Convert.ToInt16(ViewState["IndexPlanesPActos"]);
                    gvPlanesPActos.PageIndex = iPagina;
                    gvPlanesPActos.SelectedIndex = index;
                    break;
                case 1:
                    index = Convert.ToInt16(ViewState["IndexPlanesPActos"]);
                    fPlanesPActosEstadoDetalle(false);
                    fPlanesPActosDetalle();
                    break;
            }
            oBasic.FixPanel(divData, "PlanesPActos", cmdArg);
        }

        protected void lb_pdf_planp_acto_Click(object sender, EventArgs e)
        {
            fu_pdf_planp_acto.Visible = true;
            PlanesPActos_Changed();
        }

        protected void lb_pdf_planp_acto_doc_Click(object sender, EventArgs e)
        {
            string rowKey = txt_au_planp_acto.Text.ToString();
            oFile.GetPath(oVar.prPathPlanesPActos + rowKey + ".pdf");
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "verDoc();", true);
        }

        protected void lb_pdf_planp_acto_delete_Click(object sender, EventArgs e)
        {
            string pdf_file = oVar.prPathPlanesPActos + txt_au_planp_acto.Text + ".pdf";
            oBasic.FileDelete(pdf_file);
            lb_pdf_planp_acto_doc.Visible = false;
            lb_pdf_planp_acto_delete.Visible = false;

        }
        #endregion

        #region---PLANESP_Licencias
        protected void btnPlanesPLicenciasAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = "lic" + btnAccionSource.CommandName;
            oVar.prIndexValue = gvPlanesPLicencias.SelectedIndex;

            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    if (!oPermisos.TienePermisosSP("sp_u_planp_licencia") || gvPlanesPLicencias.Rows.Count == 0)
                        return;
                    fPlanesPLicenciasDetalle();
                    fPlanesPLicenciasEstadoDetalle(true);
                    oBasic.AlertSection(msgPlanesPLicencias, clConstantes.MSG_U, "warning");
                    oVar.prHayCambiosPlanesPLicencias = false;
                    break;
                case "Agregar":
                    if (!oPermisos.TienePermisosSP("sp_i_planp_licencia"))
                        return;
                    oBasic.fClearControls(vPlanesPLicenciasDetalle);
                    fPlanesPLicenciasEstadoDetalle(true);
                    oBasic.AlertSection(msgPlanesPLicencias, clConstantes.MSG_I, "info");
                    lb_pdf_planp_licencia.Visible = false;
                    break;
                case "Eliminar":
                    if (!oPermisos.TienePermisosSP("sp_d_planp_licencia") || gvPlanesPLicencias.Rows.Count == 0)
                        return;
                    oBasic.AlertSection(msgPlanesPLicencias, clConstantes.MSG_D, "danger");
                    return;
            }
            oBasic.FixPanel(divData, "PlanesPLicencias", 2);
        }

        protected void btnPlanesPLicenciasCancelar_Click(object sender, EventArgs e)
        {
            oBasic.FixPanel(divData, "PlanesPLicencias", 0);
            oBasic.AlertSection(msgPlanesPLicencias, "", "0");
        }

        protected void btnPlanesPLicenciasNavegacion_Click(object sender, EventArgs e)
        {
            int index = 0;
            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    index = Convert.ToInt16(ViewState["IndexPlanesPLicencias"]) - 1;
                    break;
                case "Next":
                    index = Convert.ToInt16(ViewState["IndexPlanesPLicencias"]) + 1;
                    break;
                case "Last":
                    index = ((DataSet)oVar.prDSPlanesPLicencias).Tables[0].Rows.Count - 1;
                    break;
            }
            gvPlanesPLicencias.SelectedIndex = index;
            ViewState["IndexPlanesPLicencias"] = index.ToString();
            fPlanesPLicenciasEstadoDetalle(false);
            fPlanesPLicenciasDetalle();
        }

        protected void btnPlanesPLicenciasVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            int index = 0;
            mvPlanesPLicencias.ActiveViewIndex = cmdArg;
            switch (cmdArg)
            {
                case 0:
                    int iPagina = Convert.ToInt16(ViewState["IndexPlanesPLicencias"]) / gvPlanesPLicencias.PageSize;
                    if (iPagina > 0)
                        index = Convert.ToInt16(ViewState["IndexPlanesPLicencias"]) % gvPlanesPLicencias.PageSize;
                    else
                        index = Convert.ToInt16(ViewState["IndexPlanesPLicencias"]);
                    gvPlanesPLicencias.PageIndex = iPagina;
                    gvPlanesPLicencias.SelectedIndex = index;
                    break;
                case 1:
                    index = Convert.ToInt16(ViewState["IndexPlanesPLicencias"]);
                    fPlanesPLicenciasEstadoDetalle(false);
                    fPlanesPLicenciasDetalle();
                    break;
            }
            oBasic.FixPanel(divData, "PlanesPLicencias", cmdArg);
        }

        protected void lb_pdf_planp_licencia_Click(object sender, EventArgs e)
        {
            fu_pdf_planp_licencia.Visible = true;
            PlanesPLicencias_Changed();
        }

        protected void lb_pdf_planp_licencia_doc_Click(object sender, EventArgs e)
        {
            string rowKey = txt_au_planp_licencia.Text.ToString();
            oFile.GetPath(oVar.prPathPlanesPLicencias + rowKey + ".pdf");
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "verDoc();", true);
        }

        protected void lb_pdf_planp_licencia_delete_Click(object sender, EventArgs e)
        {
            string pdf_file = oVar.prPathPlanesPLicencias + txt_au_planp_licencia.Text + ".pdf";
            oBasic.FileDelete(pdf_file);
            lb_pdf_planp_licencia_doc.Visible = false;
            lb_pdf_planp_licencia_delete.Visible = false;

        }
        #endregion

        #region---PLANESP_Visitas
        protected void btnPlanesPVisitasAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = "vis" + btnAccionSource.CommandName;
            oVar.prIndexValue = gvPlanesPVisitas.SelectedIndex;

            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    if (!oPermisos.TienePermisosSP("sp_u_planp_visita") || gvPlanesPVisitas.Rows.Count == 0)
                        return;
                    fPlanesPVisitasDetalle();
                    fPlanesPVisitasEstadoDetalle(true);
                    oBasic.AlertSection(msgPlanesPVisitas, clConstantes.MSG_U, "warning");
                    oVar.prHayCambiosPlanesPVisitas = false;
                    break;
                case "Agregar":
                    if (!oPermisos.TienePermisosSP("sp_i_planp_visita"))
                        return;
                    oBasic.fClearControls(vPlanesPVisitasDetalle);
                    fPlanesPVisitasEstadoDetalle(false);
                    oBasic.AlertSection(msgPlanesPVisitas, clConstantes.MSG_I, "info");
                    break;
                case "Eliminar":
                    if (!oPermisos.TienePermisosSP("sp_d_planp_visita") || gvPlanesPVisitas.Rows.Count == 0)
                        return;
                    oBasic.AlertSection(msgPlanesPVisitas, clConstantes.MSG_D, "danger");
                    return;
            }
            oBasic.FixPanel(divData, "PlanesPVisitas", 2);
        }

        protected void btnPlanesPVisitasCancelar_Click(object sender, EventArgs e)
        {
            oBasic.FixPanel(divData, "PlanesPVisitas", 0);
            oBasic.AlertSection(msgPlanesPVisitas, "", "0");
        }

        protected void btnPlanesPVisitasNavegacion_Click(object sender, EventArgs e)
        {
            int index = 0;
            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    index = Convert.ToInt16(ViewState["IndexPlanesPVisitas"]) - 1;
                    break;
                case "Next":
                    index = Convert.ToInt16(ViewState["IndexPlanesPVisitas"]) + 1;
                    break;
                case "Last":
                    index = ((DataSet)oVar.prDSPlanesPVisitas).Tables[0].Rows.Count - 1;
                    break;
            }
            gvPlanesPVisitas.SelectedIndex = index;
            ViewState["IndexPlanesPVisitas"] = index.ToString();
            fPlanesPVisitasEstadoDetalle(false);
            fPlanesPVisitasDetalle();
        }

        protected void btnPlanesPVisitasVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            int index = 0;
            mvPlanesPVisitas.ActiveViewIndex = cmdArg;
            switch (cmdArg)
            {
                case 0:
                    int iPagina = Convert.ToInt16(ViewState["IndexPlanesPVisitas"]) / gvPlanesPVisitas.PageSize;
                    if (iPagina > 0)
                        index = Convert.ToInt16(ViewState["IndexPlanesPVisitas"]) % gvPlanesPVisitas.PageSize;
                    else
                        index = Convert.ToInt16(ViewState["IndexPlanesPVisitas"]);
                    gvPlanesPVisitas.PageIndex = iPagina;
                    gvPlanesPVisitas.SelectedIndex = index;
                    break;
                case 1:
                    index = Convert.ToInt16(ViewState["IndexPlanesPVisitas"]);
                    fPlanesPVisitasEstadoDetalle(false);
                    fPlanesPVisitasDetalle();
                    break;
            }
            oBasic.FixPanel(divData, "PlanesPVisitas", cmdArg);
        }
        #endregion
        #endregion

        #region--GridView

        #region---PLANESP
        protected void gvPlanesP_DataBinding(object sender, EventArgs e)
        {
        }

        protected void gvPlanesP_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvPlanesP, "Select$" + e.Row.RowIndex.ToString()));
            }
        }

        protected void gvPlanesP_SelectedIndexChanged(object sender, EventArgs e)
        {
            gv_SelectedIndexChanged(gvPlanesP);
            oVar.prPlanesPAu = gvPlanesP.Rows.Count == 0 ? "-1" : gvPlanesP.SelectedDataKey["au_planp"].ToString();
            fGetRangoEdificabilidad();
            LoadPlanesAreas();
            LoadPlanesChildGrid(oVar.prPlanesPAu.ToString());
            lblPlanesPSection.Text = gvPlanesP.Rows.Count == 0 ? "" : gvPlanesP.Rows[gvPlanesP.SelectedIndex].Cells[2].Text;
            upPlanesPSection.Update();
        }

        protected void gvPlanesP_Sorting(object sender, GridViewSortEventArgs e)
        {
            gv_Sorting(gvPlanesP, e.SortExpression.ToString(), ViewState["SortDirPlanesP"].ToString(), oVar.prDSPlanesP);
            oVar.prDSPlanesP = oVar.prDataSet;
            LoadPlanesChildGrid(gvPlanesP.SelectedDataKey["au_planp"].ToString());
        }
        #endregion

        #region---PLANESP_Manzanas
        protected void gvPlanesPManzanas_DataBinding(object sender, EventArgs e)
        {
        }

        protected void gvPlanesPManzanas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvPlanesPManzanas, "Select$" + e.Row.RowIndex.ToString()));
            }
        }

        protected void gvPlanesPManzanas_SelectedIndexChanged(object sender, EventArgs e)
        {
            gv_SelectedIndexChanged(gvPlanesPManzanas);
        }

        protected void gvPlanesPManzanas_Sorting(object sender, GridViewSortEventArgs e)
        {
            gv_Sorting(gvPlanesPManzanas, e.SortExpression.ToString(), ViewState["SortDirPlanesPManzanas"].ToString(), oVar.prDSPlanesPManzanas);
            oVar.prDSPlanesPManzanas = oVar.prDataSet;
        }
        #endregion

        #region---PLANESP_Cesiones
        protected void gvPlanesPCesiones_DataBinding(object sender, EventArgs e)
        {
        }

        protected void gvPlanesPCesiones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvPlanesPCesiones, "Select$" + e.Row.RowIndex.ToString()));
            }
        }

        protected void gvPlanesPCesiones_SelectedIndexChanged(object sender, EventArgs e)
        {
            gv_SelectedIndexChanged(gvPlanesPCesiones);
        }

        protected void gvPlanesPCesiones_Sorting(object sender, GridViewSortEventArgs e)
        {
            gv_Sorting(gvPlanesPCesiones, e.SortExpression.ToString(), ViewState["SortDirPlanesPCesiones"].ToString(), oVar.prDSPlanesPCesiones);
            oVar.prDSPlanesPCesiones = oVar.prDataSet;
        }
        #endregion

        #region---PLANESP_Actos
        protected void gvPlanesPActos_DataBinding(object sender, EventArgs e)
        {
        }

        protected void gvPlanesPActos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvPlanesPActos, "Select$" + e.Row.RowIndex.ToString()));
            }
        }

        protected void gvPlanesPActos_SelectedIndexChanged(object sender, EventArgs e)
        {
            gv_SelectedIndexChanged(gvPlanesPActos);
        }

        protected void gvPlanesPActos_Sorting(object sender, GridViewSortEventArgs e)
        {
            gv_Sorting(gvPlanesPActos, e.SortExpression.ToString(), ViewState["SortDirPlanesPActos"].ToString(), oVar.prDSPlanesPActos);
            oVar.prDSPlanesPActos = oVar.prDataSet;
        }

        protected void gvPlanesPActos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "OpenFile")
            {
                gvPlanesPActos.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string rowKey = gvPlanesPActos.DataKeys[rowIndex].Value.ToString();
                oFile.GetPath(oVar.prPathPlanesPActos + rowKey + ".pdf");
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "verDoc();", true);
            }
        }
        #endregion

        #region---PLANESP_Licencias
        protected void gvPlanesPLicencias_DataBinding(object sender, EventArgs e)
        {
        }

        protected void gvPlanesPLicencias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvPlanesPLicencias, "Select$" + e.Row.RowIndex.ToString()));
            }
        }

        protected void gvPlanesPLicencias_SelectedIndexChanged(object sender, EventArgs e)
        {
            gv_SelectedIndexChanged(gvPlanesPLicencias);
        }

        protected void gvPlanesPLicencias_Sorting(object sender, GridViewSortEventArgs e)
        {
            gv_Sorting(gvPlanesPLicencias, e.SortExpression.ToString(), ViewState["SortDirPlanesPLicencias"].ToString(), oVar.prDSPlanesPLicencias);
            oVar.prDSPlanesPLicencias = oVar.prDataSet;
        }

        protected void gvPlanesPLicencias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "OpenFile")
            {
                gvPlanesPLicencias.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string rowKey = gvPlanesPLicencias.DataKeys[rowIndex].Value.ToString();
                oFile.GetPath(oVar.prPathPlanesPLicencias + rowKey + ".pdf");
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "verDoc();", true);
            }
        }
        #endregion

        #region---PLANESP_Visitas
        protected void gvPlanesPVisitas_DataBinding(object sender, EventArgs e)
        {
        }

        protected void gvPlanesPVisitas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvPlanesPVisitas, "Select$" + e.Row.RowIndex.ToString()));
            }
        }

        protected void gvPlanesPVisitas_SelectedIndexChanged(object sender, EventArgs e)
        {
            gv_SelectedIndexChanged(gvPlanesPVisitas);
            oVar.prPlanesPVisitasAu = gvPlanesPVisitas.SelectedDataKey.Value.ToString();
        }

        protected void gvPlanesPVisitas_Sorting(object sender, GridViewSortEventArgs e)
        {
            gv_Sorting(gvPlanesPVisitas, e.SortExpression.ToString(), ViewState["SortDirPlanesPVisitas"].ToString(), oVar.prDSPlanesPVisitas);
            oVar.prDSPlanesPVisitas = oVar.prDataSet;
        }
        #endregion
        #endregion

        #region--Métodos

        private void InitSort(string section, string index, string column, string direction)
        {
            ViewState["IndexPlanesP" + section] = index;
            ViewState["SortExpPlanesP" + section] = column;
            ViewState["SortDirPlanesP" + section] = direction;
        }

        #region---PLANESP
        private void AddPlanes()
        {
            string strResultado = oPlanesP.sp_i_planp(
            #region---params
            oBasic.fInt(txt_cod_sdp),
            txt_nombre_planp.Text,
            txt_direccion_planp.Text,
            ddlb_cod_localidad.SelectedValue,
            ddl_idupz.SelectedValue,
            oBasic.fInt(ddlb_id_categoria_planp),
            oBasic.fInt(ddlb_id_estado_planp),
            oBasic.fInt(ddlb_id_tipo_tratamiento),
            oBasic.fInt(ddlb_id_clasificacion_suelo),
            chk_es_proyecto_asociativo.Checked,
            chk_tiene_carta_intencion.Checked,
            txt_fecha_firma_carta_intencion.Text,
            txt_fecha_iniciacion_viviendas.Text,

            txt_rango_edificabilidad.Text,
            txt_areas_zonas.Text,

            oBasic.fDec(txt_area_bruta),
            oBasic.fDec(txt_area_neta_urbanizable),
            oBasic.fDec(txt_area_base_calculo_cesiones),
            oBasic.fDec(txt_area_util),
            oBasic.fDec(txt_habitantes_vivienda),

            oBasic.fDec(txt_area_af_malla_vial_arterial),
            oBasic.fDec(txt_area_af_rondas),
            oBasic.fDec(txt_area_af_zmpa),
            oBasic.fDec(txt_area_af_espacio_publico),
            oBasic.fDec(txt_area_af_servicios_publicos),
            oBasic.fDec(txt_area_af_manejo_dif),
            txt_af_otro.Text,
            oBasic.fDec(txt_area_af_otro),

            oBasic.fInt(txt_unidades_potencial_VIP),
            oBasic.fInt(txt_unidades_potencial_VIS),
            oBasic.fInt(txt_unidades_potencial_no_VIS),
            oBasic.fInt(txt_unidades_ejecutadas_VIP),
            oBasic.fInt(txt_unidades_ejecutadas_VIS),
            oBasic.fInt(txt_unidades_ejecutadas_no_VIS),

            chk_obligacion_suelo_VIP_en_sitio.Checked,
            chk_obligacion_suelo_VIP_compensacion.Checked,
            chk_obligacion_suelo_VIP_traslado.Checked,

            chk_obligacion_suelo_VIS_en_sitio.Checked,
            chk_obligacion_suelo_VIS_compensacion.Checked,
            chk_obligacion_suelo_VIS_traslado.Checked,

            chk_es_obligacion_construccion_VIP.Checked,
            chk_obligacion_construccion_VIP_en_sitio.Checked,
            chk_obligacion_construccion_VIP_compensacion.Checked,
            chk_obligacion_construccion_VIP_traslado.Checked,
            oBasic.fDec(txt_obligacion_construccion_VIP_area),
            oBasic.fDec(txt_obligacion_construccion_VIP_area_ejecutada),
            oBasic.fDec(txt_porc_obligacion_construccion_VIP),

            chk_es_obligacion_construccion_VIS.Checked,
            chk_obligacion_construccion_VIS_en_sitio.Checked,
            chk_obligacion_construccion_VIS_compensacion.Checked,
            chk_obligacion_construccion_VIS_traslado.Checked,
            oBasic.fDec(txt_obligacion_construccion_VIS_area),
            oBasic.fDec(txt_obligacion_construccion_VIS_area_ejecutada),
            oBasic.fDec(txt_porc_obligacion_construccion_VIS),

            txt_decreto_obligacion.Text,
            txt_articulo_obligacion.Text,

            txt_traslado_acto_proyecto_generador.Text,
            oBasic.fDec(txt_traslado_area),
            txt_traslado_acto_proyecto_receptor.Text,
            txt_traslado_localizacion_receptor.Text,
            chk_traslado_cumple_area_receptor.Checked,
            chk_traslado_cumple_porc_receptor.Checked,
            chk_traslado_es_primera_etapa_receptor.Checked,

            txt_compensacion_licencia.Text,
            chk_compensacion_tiene_certificado_pago.Checked,
            chk_compensacion_cumple_area.Checked,
            txt_obs_modalidad_cumplimiento.Text,

            chk_es_suelo_desarrollo_prioritario.Checked,
            txt_decreto_declaratoria.Text,
            txt_articulo_declaratoria.Text,
            txt_fecha_inicio_declaratoria.Text,
            txt_fecha_fin_declaratoria.Text,
            ddlb_id_estado_declaratoria.SelectedValue,
            txt_observacion_declaratoria.Text,

            txt_observacion.Text,
            ddlb_cod_usu_responsable.SelectedValue
            #endregion
            );

            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                oBasic.SPOk(msgMain, msgPlanesP, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
            else
                oBasic.SPError(msgMain, msgPlanesP, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
        }
        private void DeletePlanes()
        {
            string strResultado = oPlanesP.sp_d_planp(gvPlanesP.SelectedDataKey["au_planp"].ToString());
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                oBasic.fClearControls(vPlanesPDetalle);

                if (Convert.ToInt16(ViewState["IndexPlanesP"]) > 0)
                    ViewState["IndexPlanesP"] = Convert.ToInt16(ViewState["IndexPlanesP"]) - 1;
                else
                    ViewState["IndexPlanesP"] = 0;

                oBasic.SPOk(msgMain, msgPlanesP, "d", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            else
                oBasic.SPError(msgMain, msgPlanesP, "d", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
        }
        private void EditPlanes()
        {
            string strResultado = oPlanesP.sp_u_planp(
            #region---params
            oBasic.fInt(txt_au_planp),
            txt_cod_sdp.Text,
            txt_nombre_planp.Text,
            txt_direccion_planp.Text,
            ddlb_cod_localidad.SelectedValue,
            ddl_idupz.SelectedValue,
            oBasic.fInt(ddlb_id_categoria_planp),
            oBasic.fInt(ddlb_id_estado_planp),
            oBasic.fInt(ddlb_id_tipo_tratamiento),
            oBasic.fInt(ddlb_id_clasificacion_suelo),
            chk_es_proyecto_asociativo.Checked,
            chk_tiene_carta_intencion.Checked,
            txt_fecha_firma_carta_intencion.Text,
            txt_fecha_iniciacion_viviendas.Text,

            txt_rango_edificabilidad.Text,
            txt_areas_zonas.Text,

            oBasic.fDec(txt_area_bruta),
            oBasic.fDec(txt_area_neta_urbanizable),
            oBasic.fDec(txt_area_base_calculo_cesiones),
            oBasic.fDec(txt_area_util),
            oBasic.fDec(txt_habitantes_vivienda),

            oBasic.fDec(txt_area_af_malla_vial_arterial),
            oBasic.fDec(txt_area_af_rondas),
            oBasic.fDec(txt_area_af_zmpa),
            oBasic.fDec(txt_area_af_espacio_publico),
            oBasic.fDec(txt_area_af_servicios_publicos),
            oBasic.fDec(txt_area_af_manejo_dif),
            txt_af_otro.Text,
            oBasic.fDec(txt_area_af_otro),

            oBasic.fInt(txt_unidades_potencial_VIP),
            oBasic.fInt(txt_unidades_potencial_VIS),
            oBasic.fInt(txt_unidades_potencial_no_VIS),
            oBasic.fInt(txt_unidades_ejecutadas_VIP),
            oBasic.fInt(txt_unidades_ejecutadas_VIS),
            oBasic.fInt(txt_unidades_ejecutadas_no_VIS),

            chk_obligacion_suelo_VIP_en_sitio.Checked,
            chk_obligacion_suelo_VIP_compensacion.Checked,
            chk_obligacion_suelo_VIP_traslado.Checked,

            chk_obligacion_suelo_VIS_en_sitio.Checked,
            chk_obligacion_suelo_VIS_compensacion.Checked,
            chk_obligacion_suelo_VIS_traslado.Checked,

            chk_es_obligacion_construccion_VIP.Checked,
            chk_obligacion_construccion_VIP_en_sitio.Checked,
            chk_obligacion_construccion_VIP_compensacion.Checked,
            chk_obligacion_construccion_VIP_traslado.Checked,
            oBasic.fDec(txt_obligacion_construccion_VIP_area),
            oBasic.fDec(txt_obligacion_construccion_VIP_area_ejecutada),
            oBasic.fDec(txt_porc_obligacion_construccion_VIP),

            chk_es_obligacion_construccion_VIS.Checked,
            chk_obligacion_construccion_VIS_en_sitio.Checked,
            chk_obligacion_construccion_VIS_compensacion.Checked,
            chk_obligacion_construccion_VIS_traslado.Checked,
            oBasic.fDec(txt_obligacion_construccion_VIS_area),
            oBasic.fDec(txt_obligacion_construccion_VIS_area_ejecutada),
            oBasic.fDec(txt_porc_obligacion_construccion_VIS),

            txt_decreto_obligacion.Text,
            txt_articulo_obligacion.Text,

            txt_traslado_acto_proyecto_generador.Text,
            oBasic.fDec(txt_traslado_area),
            txt_traslado_acto_proyecto_receptor.Text,
            txt_traslado_localizacion_receptor.Text,
            chk_traslado_cumple_area_receptor.Checked,
            chk_traslado_cumple_porc_receptor.Checked,
            chk_traslado_es_primera_etapa_receptor.Checked,

            txt_compensacion_licencia.Text,
            chk_compensacion_tiene_certificado_pago.Checked,
            chk_compensacion_cumple_area.Checked,
            txt_obs_modalidad_cumplimiento.Text,

            chk_es_suelo_desarrollo_prioritario.Checked,
            txt_decreto_declaratoria.Text,
            txt_articulo_declaratoria.Text,
            txt_fecha_inicio_declaratoria.Text,
            txt_fecha_fin_declaratoria.Text,
            ddlb_id_estado_declaratoria.SelectedValue,
            txt_observacion_declaratoria.Text,

            txt_observacion.Text,
            ddlb_cod_usu_responsable.SelectedValue
            #endregion
            );

            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                oBasic.SPOk(msgMain, msgPlanesP, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
            else
                oBasic.SPError(msgMain, msgPlanesP, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
        }
        private void EnablePlanes(bool b)
        {
            txt_au_planp.Enabled = false;

            chk_es_obligacion_VIS__planp.Enabled = false;
            txt_SP_obligacion_VIS.Enabled = false;
            txt_porc_suelo_util_obligacion_VIS.Enabled = false;
            txt_SE_obligacion_VIS.Enabled = false;

            chk_es_obligacion_VIP__planp.Enabled = false;
            txt_SP_obligacion_VIP.Enabled = false;
            txt_porc_suelo_util_obligacion_VIP.Enabled = false;
            txt_SE_obligacion_VIP.Enabled = false;
        }
        private void GetPlanesAreas()
        {
            txt_areas_zonas.Text = " ";
            for (int i = 1; i <= 23; i++)
            {
                CheckBox chkControl = (CheckBox)divAreasZonas.FindControl("chk_az_" + i.ToString());
                try
                {
                    if (chkControl.Checked)
                    {
                        txt_areas_zonas.Text = txt_areas_zonas.Text + i.ToString() + ", ";
                    }
                }
                catch { }
            }
        }
        private void LoadDropDownUPZ()
        {
            ddl_idupz.Items.Clear();
            ddl_idupz.DataSource = oUPZ.sp_s_upz(ddlb_cod_localidad.SelectedValue);
            ddl_idupz.DataTextField = "nombre_upz";
            ddl_idupz.DataValueField = "cod_upz";

            ddl_idupz.Items.Insert(0, new ListItem("-- Seleccione opción", "0"));
            ddl_idupz.DataBind();
        }
        private void LoadPlanes(string Parametro)
        {
            if (string.IsNullOrEmpty(Parametro))
                Parametro = "%";
            oVar.prDSPlanesP = oPlanesP.sp_s_planesp_nombre(Parametro);
            gvPlanesP.DataSource = ((DataSet)(oVar.prDSPlanesP));
            gvPlanesP.DataBind();

            LoadPlanesOutside();
            LoadPlanesInside();
        }
        private void LoadPlanesAreas()
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
                        txt_areas_zonas_desc.Text = txt_areas_zonas_desc.Text + chkControl.Text + ", ";
                    }
                    else
                        chkControl.Checked = false;
                }
                catch { }
            }
            if (txt_areas_zonas_desc.Text.Length > 0)
                txt_areas_zonas_desc.Text = txt_areas_zonas_desc.Text.Substring(0, txt_areas_zonas_desc.Text.Length - 2);
        }
        private void LoadPlanesDetail()
        {
            int Indice = Convert.ToInt16(ViewState["IndexPlanesP"]);
            DataSet dsTmp = (DataSet)oVar.prDSPlanesP;
            DataRow dRow = dsTmp.Tables[0].Rows[Indice];
            oBasic.LblRegistros(upPlanesPFoot, gvPlanesP.Rows.Count, Indice);

            oBasic.fValueControls(vPlanesPDetalle, dRow);
            LoadDropDownUPZ();
            oBasic.fDetalleDropDown(ddl_idupz, dRow["idupz"].ToString());

            oBasic.fDetalle(chk_es_obligacion_VIS__planp, dRow["es_obligacion_VIS"]);
            oBasic.fDetalle(chk_es_obligacion_VIP__planp, dRow["es_obligacion_VIP"]);

            fGetRangoEdificabilidad();
            LoadPlanesAreas();

            chk_tiene_carta_intencion_CheckedChanged(null, null);
            ddlb_id_tipo_tratamiento_SelectedIndexChanged(null, null);
            chk_obligaciones(null, null);
            chk_obligacion_construccion_VIP_CheckedChanged(null, null);
            chk_obligacion_construccion_VIS_CheckedChanged(null, null);
            chk_es_suelo_desarrollo_prioritario_CheckedChanged(null, null);

            oBasic.fLoadUsuariosFiltro(ddlb_cod_usu_responsable, 10, dRow["cod_usu_responsable"].ToString());
        }
        private void LoadPlanesInside()
        {
            if (gvPlanesP.Rows.Count > 0)
            {
                gvPlanesP.Visible = true;
                gvPlanesP.SelectedIndex = Convert.ToInt16(ViewState["IndexPlanesP"].ToString());
                if (gvPlanesP.Rows.Count < gvPlanesP.SelectedIndex)
                    gvPlanesP.SelectedIndex = 0;
                oVar.prPlanesPAu = gvPlanesP.SelectedDataKey["au_planp"].ToString();
                oBasic.FixPanel(divData, "PlanesP", 0);

                gv_Sorting(gvPlanesP, ViewState["SortExpPlanesP"].ToString(), oUtil.OpDirection(ViewState["SortDirPlanesP"].ToString()), oVar.prDSPlanesP);
                LoadPlanesChildGrid(gvPlanesP.SelectedDataKey["au_planp"].ToString());
            }
            else
            {
                gvPlanesP.Visible = false;
                oBasic.FixPanel(divData, "PlanesP", 3);
            }
        }
        private void LoadPlanesOutside()
        {
            if (hdd_Proyecto_PlanesP_Id.Value != "")
                Session["Proyecto.PlanesP.Id"] = hdd_Proyecto_PlanesP_Id.Value;

            if (Session["Proyecto.PlanesP.Id"] == null || Session["Proyecto.PlanesP.Id"].ToString() == "")
                return;
            hdd_Proyecto_PlanesP_Id.Value = Session["Proyecto.PlanesP.Id"].ToString();
            int p_Id = Convert.ToInt32(Session["Proyecto.PlanesP.Id"].ToString());
            Session["Proyecto.PlanesP.Id"] = null;
            txtBuscar.Visible = false;
            btnBuscar.Visible = false;
            btnVolver.Visible = true;
            oVar.prDSPlanesP = oPlanesP.sp_s_planesp(p_Id);
            gvPlanesP.DataSource = ((DataSet)(oVar.prDSPlanesP));
            gvPlanesP.DataBind();
        }
        private void LoadPlanesChildGrid(string indice)
        {
            fPlanesPManzanasLoadGV(indice);
            fPlanesPCesionesLoadGV(indice);
            fPlanesPActosLoadGV(indice);
            fPlanesPLicenciasLoadGV(indice);
            fPlanesPVisitasLoadGV(indice);
            LoadArchivos(indice);
        }

        private void LoadArchivos(string indice)
        {
            ucFileUpload.FilePath = Path.Combine(oVar.prPathPlanesP.ToString(), "Documentos");
            ucFileUpload.Prefix = tipoArchivo.PDF_PP;
            ucFileUpload.ReferenceID = Convert.ToInt32(indice);
            DataTable dt = oPlanesP.sp_s_planesp(ucFileUpload.ReferenceID).Tables[0];
            ucFileUpload.ArchivoID = dt.Rows.Count > 0 ? Convert.ToInt32("0" + (dt.Rows[0]["idarchivo"] ?? "0").ToString()) : -1;
            ucFileUpload.SectionPermission = cnsSection.PLAN_PARC_DOCUMENTOS;
        }

        protected void chk_tiene_carta_intencion_CheckedChanged(object sender, EventArgs e)
        {
            bool b = false;
            if (chk_tiene_carta_intencion.Checked)
                b = true;
            oBasic.StyleCtl("E", b, txt_fecha_firma_carta_intencion, !b);
            rfv_fecha_firma_carta_intencion.Enabled = b;
            rv_fecha_firma_carta_intencion.Enabled = b;
            if (b)
            {
                try
                {
                    oBasic.fValidarFecha(cal_fecha_firma_carta_intencion, rv_fecha_firma_carta_intencion, "0", 0);
                }
                catch { }
            }
        }
        protected void ddlb_id_tipo_tratamiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool b = false;
            if (ddlb_id_tipo_tratamiento.SelectedValue == "120")
                b = true;
            oBasic.StyleCtl("E", b, txt_area_neta_urbanizable, false);
            if (b)
            {
                try
                {
                    rfv_area_neta_urbanizable.Enabled = true;
                    oBasic.ClassAdd(txt_area_neta_urbanizable, "text-right");
                }
                catch{}
            }
            else
                rfv_area_neta_urbanizable.Enabled = false;
        }
        protected void chk_obligacion_construccion_VIS_CheckedChanged(object sender, EventArgs e)
        {
            bool b = false;
            if (chk_es_obligacion_construccion_VIS.Checked)
                b = true;
            oBasic.StyleCtl("E", b, txt_obligacion_construccion_VIS_area, !b);
            oBasic.StyleCtl("E", b, txt_porc_obligacion_construccion_VIS, !b);
            oBasic.StyleCtl("E", b, chk_obligacion_construccion_VIS_en_sitio, !b);
            oBasic.StyleCtl("E", b, chk_obligacion_construccion_VIS_compensacion, !b);
            oBasic.StyleCtl("E", b, chk_obligacion_construccion_VIS_traslado, !b);
            oBasic.StyleCtl("E", b, txt_obligacion_construccion_VIS_area_ejecutada, !b);
        }
        protected void chk_obligacion_construccion_VIP_CheckedChanged(object sender, EventArgs e)
        {
            bool b = false;
            if (chk_es_obligacion_construccion_VIP.Checked)
                b = true;
            oBasic.StyleCtl("E", b, txt_obligacion_construccion_VIP_area, !b);
            oBasic.StyleCtl("E", b, txt_porc_obligacion_construccion_VIP, !b);
            oBasic.StyleCtl("E", b, chk_obligacion_construccion_VIP_en_sitio, !b);
            oBasic.StyleCtl("E", b, chk_obligacion_construccion_VIP_compensacion, !b);
            oBasic.StyleCtl("E", b, chk_obligacion_construccion_VIP_traslado, !b);
            oBasic.StyleCtl("E", b, txt_obligacion_construccion_VIP_area_ejecutada, !b);
        }
        protected void chk_obligaciones(object sender, EventArgs e)
        {
            bool b = false;
            if (chk_obligacion_suelo_VIS_traslado.Checked || chk_obligacion_construccion_VIS_traslado.Checked || chk_obligacion_suelo_VIP_traslado.Checked || chk_obligacion_construccion_VIP_traslado.Checked)
                b = true;
            oBasic.StyleCtl("E", b, txt_traslado_acto_proyecto_generador, false);
            oBasic.StyleCtl("E", b, txt_traslado_acto_proyecto_receptor, false);
            oBasic.StyleCtl("E", b, txt_traslado_localizacion_receptor, false);
            oBasic.StyleCtl("E", b, txt_traslado_area, false);
            oBasic.StyleCtl("E", b, chk_traslado_cumple_area_receptor, false);
            oBasic.StyleCtl("E", b, chk_traslado_cumple_porc_receptor, false);
            oBasic.StyleCtl("E", b, chk_traslado_es_primera_etapa_receptor, false);

            b = false;
            if (chk_obligacion_suelo_VIS_compensacion.Checked || chk_obligacion_construccion_VIS_compensacion.Checked || chk_obligacion_suelo_VIP_compensacion.Checked || chk_obligacion_construccion_VIP_compensacion.Checked)
                b = true;
            oBasic.StyleCtl("E", b, txt_compensacion_licencia, false);
            oBasic.StyleCtl("E", b, chk_compensacion_tiene_certificado_pago, false);
            oBasic.StyleCtl("E", b, chk_compensacion_cumple_area, false);
        }
        protected void chk_es_suelo_desarrollo_prioritario_CheckedChanged(object sender, EventArgs e)
        {
            bool b = false;
            if (chk_es_suelo_desarrollo_prioritario.Checked)
                b = true;
            oBasic.StyleCtl("E", b, txt_decreto_declaratoria, !b);
            oBasic.StyleCtl("E", b, txt_articulo_declaratoria, !b);
            oBasic.StyleCtl("E", b, txt_fecha_inicio_declaratoria, !b);
            oBasic.StyleCtl("E", b, txt_fecha_fin_declaratoria, !b);
            oBasic.StyleCtl("E", b, ddlb_id_estado_declaratoria, !b);
            oBasic.StyleCtl("E", b, txt_observacion_declaratoria, !b);
        }
        private void fGetRangoEdificabilidad()
        {
            txt_rango_edificabilidad_desc.Text = "";
            for (int i = 243; i <= 254; i++)
            {
                CheckBox chkControl = (CheckBox)divRangoEdificabilidad.FindControl("chk_rango_ed_" + i.ToString());
                try
                {
                    if (txt_rango_edificabilidad.Text.Contains(" " + i.ToString() + ", "))
                    {
                        chkControl.Checked = true;
                        txt_rango_edificabilidad_desc.Text = txt_rango_edificabilidad_desc.Text + chkControl.Text + ", ";
                    }
                    else
                        chkControl.Checked = false;
                }
                catch { }
            }
            if (txt_rango_edificabilidad_desc.Text.Length > 0)
                txt_rango_edificabilidad_desc.Text = txt_rango_edificabilidad_desc.Text.Substring(0, txt_rango_edificabilidad_desc.Text.Length - 2);
        }
        private void fSetRangoEdificabilidad()
        {
            txt_rango_edificabilidad.Text = " ";
            for (int i = 243; i <= 254; i++)
            {
                CheckBox chkControl = (CheckBox)divRangoEdificabilidad.FindControl("chk_rango_ed_" + i.ToString());
                try
                {
                    if (chkControl.Checked)
                    {
                        txt_rango_edificabilidad.Text = txt_rango_edificabilidad.Text + i.ToString() + ", ";
                    }
                }
                catch { }
            }
        }
        #endregion

        #region---PLANESP_Manzanas
        private void fPlanesPManzanasLoadGV(string p_cod_planp)
        {
            if (string.IsNullOrEmpty(p_cod_planp))
                p_cod_planp = "0";
            oVar.prDSPlanesPManzanas = oPlanesPManzanas.sp_s_planesp_manzanas_cod_planp(p_cod_planp);

            gvPlanesPManzanas.DataSource = ((DataSet)(oVar.prDSPlanesPManzanas));
            gvPlanesPManzanas.DataBind();

            if (gvPlanesPManzanas.Rows.Count > 0)
            {
                gvPlanesPManzanas.Visible = true;
                gvPlanesPManzanas.SelectedIndex = Convert.ToInt16(ViewState["IndexPlanesPManzanas"].ToString());
            }
            else
            {
                gvPlanesPManzanas.Visible = false;
            }
            gv_Sorting(gvPlanesPManzanas, ViewState["SortExpPlanesPManzanas"].ToString(), oUtil.OpDirection(ViewState["SortDirPlanesPManzanas"].ToString()), oVar.prDSPlanesPManzanas);
            oVar.prDSPlanesPManzanas = oVar.prDataSet;
            oBasic.FixPanel(divData, "PlanesPManzanas", 0);
        }

        private void fPlanesPManzanasDetalle()
        {
            ddlb_cod_planp_licencia.Items.Clear();
            ddlb_cod_planp_licencia.Items.Add("-- Seleccione opción");
            ddlb_cod_planp_licencia.DataSource = oPlanesPLicencias.sp_s_planesp_licencias_cod_planp(gvPlanesP.SelectedDataKey["au_planp"].ToString());
            ddlb_cod_planp_licencia.DataTextField = "numero_licencia";
            ddlb_cod_planp_licencia.DataValueField = "au_planp_licencia";
            ddlb_cod_planp_licencia.DataBind();

            int Indice = Convert.ToInt16(ViewState["IndexPlanesPManzanas"]);
            DataSet dsTmp = new DataSet();
            dsTmp = (DataSet)oVar.prDSPlanesPManzanas;
            DataRow dRow = dsTmp.Tables[0].Rows[Indice];
            oBasic.LblRegistros(upPlanesPManzanasFoot, gvPlanesPManzanas.Rows.Count, Indice);

            oBasic.fValueControls(vPlanesPManzanasDetalle, dRow);
        }

        private void fPlanesPManzanasEstadoDetalle(bool HabilitarCampos)
        {
            txt_au_planp_manzana.Enabled = false;
            txt_cod_planp__manzana.Enabled = false;

            ddlb_id_uso_manzana_SelectedIndexChanged(null, null);
            chk_es_obligacion_VIS_CheckedChanged(null, null);
            chk_es_obligacion_VIP_CheckedChanged(null, null);
            chk_es_declarado_CheckedChanged(null, null);
        }

        private void fPlanesPManzanasInsert()
        {
            string value_cod_planp_licencia = "0";
            if (ddlb_cod_planp_licencia.SelectedIndex > 0)
                value_cod_planp_licencia = ddlb_cod_planp_licencia.SelectedValue;

            string strResultado = oPlanesPManzanas.sp_i_planp_manzana(
            #region---params
        gvPlanesP.SelectedDataKey["au_planp"].ToString(),
              txt_unidad_gestion.Text,
              txt_manzana.Text,
              oBasic.fDec(txt_area_manzana),
              oBasic.fInt(ddlb_id_uso_manzana),
              oBasic.fPerc(txt_porc_ejecutado),
              txt_fecha_fin.Text,
              oBasic.fInt(txt_UP_VIP__manzana),
              oBasic.fInt(txt_UP_VIS__manzana),
              oBasic.fInt(txt_UP_no_VIS__manzana),
              oBasic.fInt(txt_UE_VIP__manzana),
              oBasic.fInt(txt_UE_VIS__manzana),
              oBasic.fInt(txt_UE_no_VIS__manzana),
              oBasic.fDec(txt_SP_m_VIP),
              oBasic.fDec(txt_SP_m_VIS),
              oBasic.fDec(txt_SP_m_no_VIS),
              oBasic.fDec(txt_SP_m_afectas),
              oBasic.fDec(txt_SP_m_comercio),
              oBasic.fDec(txt_SP_m_comercio_y_servicios),
              oBasic.fDec(txt_SP_m_dotacional),
              oBasic.fDec(txt_SP_m_industria),
              oBasic.fDec(txt_SP_m_industria_y_servicios),
              oBasic.fDec(txt_SP_m_servicios),
              chk_es_obligacion_VIS.Checked,
              oBasic.fPerc(txt_porc_area_obligacion_VIS),
              chk_es_obligacion_VIP.Checked,
              oBasic.fPerc(txt_porc_area_obligacion_VIP),
              chk_es_obligacion_primera_etapa.Checked,
              chk_es_declarado.Checked,
              value_cod_planp_licencia,
              txt_observacion__manzana.Text
            #endregion
      );
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                oBasic.SPOk(msgMain, msgPlanesPManzanas, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
            else
                oBasic.SPError(msgMain, msgPlanesPManzanas, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
        }

        private void fPlanesPManzanasUpdate()
        {
            if (Convert.ToBoolean(oVar.prHayCambiosPlanesPManzanas))
            {
                string value_cod_planp_licencia = "0";
                if (ddlb_cod_planp_licencia.SelectedIndex > 0)
                    value_cod_planp_licencia = ddlb_cod_planp_licencia.SelectedValue;

                string strResultado = oPlanesPManzanas.sp_u_planp_manzana(
                #region---params
                    oBasic.fInt(txt_au_planp_manzana),
                  txt_unidad_gestion.Text,
                  txt_manzana.Text,
                  oBasic.fDec(txt_area_manzana),
                  oBasic.fInt(ddlb_id_uso_manzana),
                  oBasic.fPerc(txt_porc_ejecutado),
                  txt_fecha_fin.Text,
                            oBasic.fInt(txt_UP_VIP__manzana),
                            oBasic.fInt(txt_UP_VIS__manzana),
                            oBasic.fInt(txt_UP_no_VIS__manzana),
                            oBasic.fInt(txt_UE_VIP__manzana),
                            oBasic.fInt(txt_UE_VIS__manzana),
                            oBasic.fInt(txt_UE_no_VIS__manzana),
                            oBasic.fDec(txt_SP_m_VIP),
                            oBasic.fDec(txt_SP_m_VIS),
                            oBasic.fDec(txt_SP_m_no_VIS),
                            oBasic.fDec(txt_SP_m_afectas),
                            oBasic.fDec(txt_SP_m_comercio),
                            oBasic.fDec(txt_SP_m_comercio_y_servicios),
                            oBasic.fDec(txt_SP_m_dotacional),
                            oBasic.fDec(txt_SP_m_industria),
                            oBasic.fDec(txt_SP_m_industria_y_servicios),
                            oBasic.fDec(txt_SP_m_servicios),
                            chk_es_obligacion_VIS.Checked,
                            oBasic.fPerc(txt_porc_area_obligacion_VIS),
                            chk_es_obligacion_VIP.Checked,
                            oBasic.fPerc(txt_porc_area_obligacion_VIP),
                            chk_es_obligacion_primera_etapa.Checked,
                            chk_es_declarado.Checked,
                            value_cod_planp_licencia,
                            txt_observacion__manzana.Text
                #endregion
                );
                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                    oBasic.SPOk(msgMain, msgPlanesPManzanas, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                else
                    oBasic.SPError(msgMain, msgPlanesPManzanas, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
            }
        }

        private void fPlanesPManzanasDelete()
        {
            try
            {
                string strResultado = oPlanesPManzanas.sp_d_planp_manzana(gvPlanesPManzanas.SelectedDataKey.Value.ToString());
                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fPlanesPManzanasDelete:", clConstantes.MSG_OK_D);
                    oBasic.fClearControls(vPlanesPManzanasDetalle);

                    if (Convert.ToInt16(ViewState["IndexPlanesPManzanas"]) > 0)
                        ViewState["IndexPlanesPManzanas"] = Convert.ToInt16(ViewState["IndexPlanesPManzanas"]) - 1;
                    else
                        ViewState["IndexPlanesPManzanas"] = 0;

                    oBasic.SPOk(msgMain, msgPlanesPManzanas, "d", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                    oBasic.SPError(msgMain, msgPlanesPManzanas, "d", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
            }
            catch { }
        }

        protected void ddlb_id_uso_manzana_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool b = false;
            int i;
            try
            {
                i = Convert.ToInt16(ddlb_id_uso_manzana.SelectedItem.Value);
            }
            catch
            {
                return;
            }

            if (i == 283)
                b = true;
            oBasic.EnablePanel(pPlanesPManzanasAreas, b, b);
            oBasic.EnableCtl(txt_area_manzana, !b);

            b = false;
            if (i == 246 || i == 247 || i == 283)
                b = true;
            oBasic.StyleCtl("E", b, chk_es_obligacion_VIS, !b);
            oBasic.StyleCtl("E", b, chk_es_obligacion_VIP, !b);

            chk_es_obligacion_VIS_CheckedChanged(null, null);
            chk_es_obligacion_VIP_CheckedChanged(null, null);

            PlanesPManzanas_Changed();
        }

        protected void chk_es_obligacion_VIS_CheckedChanged(object sender, EventArgs e)
        {
            bool b = false;
            if (chk_es_obligacion_VIS.Checked)
                b = true;
            oBasic.StyleCtl("E", b, txt_porc_area_obligacion_VIS, !b);
            rfv_porc_area_obligacion_VIS.Enabled = b;
            PlanesPManzanas_Changed();
            if (b)
                chk_es_obligacion_VIS.Focus();
        }

        protected void chk_es_obligacion_VIP_CheckedChanged(object sender, EventArgs e)
        {
            bool b = false;
            if (chk_es_obligacion_VIP.Checked)
                b = true;
            oBasic.StyleCtl("E", b, txt_porc_area_obligacion_VIP, !b);
            rfv_porc_area_obligacion_VIP.Enabled = b;
            PlanesPManzanas_Changed();
            if (b)
                chk_es_obligacion_VIP.Focus();
        }

        protected void chk_es_declarado_CheckedChanged(object sender, EventArgs e)
        {
            bool b = false;
            if (chk_es_declarado.Checked)
                b = true;
            oBasic.StyleCtl("E", b, txt_fecha_inicio_declaratoria, !b);
            oBasic.StyleCtl("E", b, txt_fecha_fin_declaratoria, !b);
            PlanesPManzanas_Changed();
        }
        #endregion

        #region---PLANESP_Cesiones
        private void fPlanesPCesionesLoadGV(string p_cod_planp)
        {
            if (string.IsNullOrEmpty(p_cod_planp))
                p_cod_planp = "0";
            oVar.prDSPlanesPCesiones = oPlanesPCesiones.sp_s_planesp_cesiones_cod_planp(p_cod_planp);

            gvPlanesPCesiones.DataSource = ((DataSet)(oVar.prDSPlanesPCesiones));
            gvPlanesPCesiones.DataBind();

            if (gvPlanesPCesiones.Rows.Count > 0)
            {
                gvPlanesPCesiones.Visible = true;
                oBasic.FixPanel(divData, "PlanesPCesiones", 0);
            }
            else
            {
                gvPlanesPCesiones.Visible = false;
            }
            gv_Sorting(gvPlanesPCesiones, ViewState["SortExpPlanesPCesiones"].ToString(), oUtil.OpDirection(ViewState["SortDirPlanesPCesiones"].ToString()), oVar.prDSPlanesPCesiones);
            oVar.prDSPlanesPCesiones = oVar.prDataSet;
            oBasic.FixPanel(divData, "PlanesPCesiones", 0);
        }

        private void fPlanesPCesionesDetalle()
        {
            int Indice = Convert.ToInt16(ViewState["IndexPlanesPCesiones"]);
            DataSet dsTmp = new DataSet();
            dsTmp = (DataSet)oVar.prDSPlanesPCesiones;
            DataRow dRow = dsTmp.Tables[0].Rows[Indice];

            oBasic.fValueControls(vPlanesPCesionesDetalle, dRow);
        }

        private void fPlanesPCesionesEstadoDetalle(bool b)
        {
            txt_au_planp_cesion.Enabled = false;
            txt_cod_planp__cesiones.Enabled = false;
            txt_porc_area.Enabled = false;
        }

        private void fPlanesPCesionesInsert()
        {
            string strResultado = oPlanesPCesiones.sp_i_planp_cesion(
              gvPlanesP.SelectedDataKey["au_planp"].ToString(),
              txt_unidad_gestion__cesiones.Text,
              txt_cesion.Text,
              oBasic.fInt(ddlb_id_tipo_cesion),
              oBasic.fDec(txt_area_cesion),
              oBasic.fPerc(txt_porc_ejecutado__cesiones),
              chk_es_suelo_en_sitio.Checked,
              chk_es_entregado_DADEP.Checked,
              txt_observacion__cesiones.Text
            );
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                oBasic.SPOk(msgMain, msgPlanesPCesiones, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
            else
                oBasic.SPError(msgMain, msgPlanesPCesiones, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
        }

        private void fPlanesPCesionesUpdate()
        {
            if (Convert.ToBoolean(oVar.prHayCambiosPlanesPCesiones))
            {
                string strResultado = oPlanesPCesiones.sp_u_planp_cesion(
                  oBasic.fInt(txt_au_planp_cesion),
                  txt_unidad_gestion__cesiones.Text,
                  txt_cesion.Text,
                  oBasic.fInt(ddlb_id_tipo_cesion),
                  oBasic.fDec(txt_area_cesion),
                  oBasic.fPerc(txt_porc_ejecutado__cesiones),
                  chk_es_suelo_en_sitio.Checked,
                  chk_es_entregado_DADEP.Checked,
                  txt_observacion__cesiones.Text
                );
                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                    oBasic.SPOk(msgMain, msgPlanesPCesiones, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                else
                    oBasic.SPError(msgMain, msgPlanesPCesiones, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
            }
            else
            {
                oBasic.SPOk(msgMain, msgPlanesPCesiones, "", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void fPlanesPCesionesDelete()
        {
            try
            {
                string strResultado = oPlanesPCesiones.sp_d_planp_cesion(gvPlanesPCesiones.SelectedDataKey.Value.ToString());
                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fPlanesPCesionesDelete:", clConstantes.MSG_OK_D);
                    oBasic.fClearControls(vPlanesPCesionesDetalle);

                    if (Convert.ToInt16(ViewState["IndexPlanesPCesiones"]) > 0)
                        ViewState["IndexPlanesPCesiones"] = Convert.ToInt16(ViewState["IndexPlanesPCesiones"]) - 1;
                    else
                        ViewState["IndexPlanesPCesiones"] = 0;

                    oBasic.SPOk(msgMain, msgPlanesPCesiones, "d", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                    oBasic.SPError(msgMain, msgPlanesPCesiones, "d", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
            }
            catch { }
        }
        #endregion

        #region---PLANESP_Actos
        private void fPlanesPActosLoadGV(string p_cod_planp)
        {
            if (string.IsNullOrEmpty(p_cod_planp))
                p_cod_planp = "0";
            oVar.prDSPlanesPActos = oPlanesPActos.sp_s_planesp_actos_cod_planp(p_cod_planp);

            gvPlanesPActos.DataSource = ((DataSet)(oVar.prDSPlanesPActos));
            gvPlanesPActos.DataBind();

            if (gvPlanesPActos.Rows.Count > 0)
            {
                gvPlanesPActos.Visible = true;
                oBasic.FixPanel(divData, "PlanesPActos", 0);
            }
            else
            {
                gvPlanesPActos.Visible = false;
            }
            gv_Sorting(gvPlanesPActos, ViewState["SortExpPlanesPActos"].ToString(), oUtil.OpDirection(ViewState["SortDirPlanesPActos"].ToString()), oVar.prDSPlanesPActos);
            oVar.prDSPlanesPActos = oVar.prDataSet;
            oBasic.FixPanel(divData, "PlanesPActos", 0);
        }

        private void fPlanesPActosDetalle()
        {
            mvPlanesPActos.ActiveViewIndex = 1;
            int Indice = Convert.ToInt16(ViewState["IndexPlanesPActos"]);
            DataSet dsTmp = new DataSet();
            dsTmp = (DataSet)oVar.prDSPlanesPActos;
            DataRow dRow = dsTmp.Tables[0].Rows[Indice];

            oBasic.fValueControls(vPlanesPActosDetalle, dRow);

            if (string.IsNullOrEmpty(dsTmp.Tables[0].Rows[Indice]["time_scan"].ToString()))
            {
                lb_pdf_planp_acto_delete.Visible = false;
                lb_pdf_planp_acto_doc.Visible = false;
            }
            else
            {
                lb_pdf_planp_acto_delete.Visible = true;
                lb_pdf_planp_acto_doc.Visible = true;
            }
        }

        private void fPlanesPActosEstadoDetalle(bool b)
        {
            txt_au_planp_acto.Enabled = false;
            txt_cod_planp__acto.Enabled = false;
            fu_pdf_planp_acto.Visible = false;
            oBasic.StyleCtl("V", b, lb_pdf_planp_acto, false);
        }

        private void fPlanesPActosInsert()
        {
            string strResultado = oPlanesPActos.sp_i_planp_acto(
              gvPlanesP.SelectedDataKey["au_planp"].ToString(),
              oBasic.fInt(ddlb_id_tipo_acto),
              txt_numero_acto.Text,
              txt_fecha_acto.Text,
              txt_vigencia.Text,
              txt_observacion__acto.Text
            );
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                oBasic.SPOk(msgMain, msgPlanesPActos, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
            else
                oBasic.SPError(msgMain, msgPlanesPActos, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
        }

        private void fPlanesPActosUpdate()
        {
            if (Convert.ToBoolean(oVar.prHayCambiosPlanesPActos))
            {
                string strResultado = oPlanesPActos.sp_u_planp_acto(
                  oBasic.fInt(txt_au_planp_acto),
                  oBasic.fInt(ddlb_id_tipo_acto),
                  txt_numero_acto.Text,
                  txt_fecha_acto.Text,
                  txt_vigencia.Text,
                  txt_observacion__acto.Text,
                  fu_pdf_planp_acto.HasFiles
                );

                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    string pdf_file = oVar.prPathPlanesPActos + txt_au_planp_acto.Text + ".pdf";
                    oBasic.LoadPdf(fu_pdf_planp_acto, pdf_file);
                    if (oVar.prError.ToString() != "0")
                    {
                        oBasic.SPError(msgMain, msgPlanesPActos, "fl-u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
                    }
                    else
                    {
                        oBasic.SPOk(msgMain, msgPlanesPActos, "fl-u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                    }

                }
                else
                    oBasic.SPError(msgMain, msgPlanesPActos, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
            }
            else
            {
                oBasic.SPOk(msgMain, msgPlanesPActos, "", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void fPlanesPActosDelete()
        {
            try
            {
                string strResultado = oPlanesPActos.sp_d_planp_acto(gvPlanesPActos.SelectedDataKey.Value.ToString());
                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fPlanesPActosDelete:", clConstantes.MSG_OK_D);
                    oBasic.fClearControls(vPlanesPActosDetalle);

                    if (Convert.ToInt16(ViewState["IndexPlanesPActos"]) > 0)
                        ViewState["IndexPlanesPActos"] = Convert.ToInt16(ViewState["IndexPlanesPActos"]) - 1;
                    else
                        ViewState["IndexPlanesPActos"] = 0;

                    oBasic.SPOk(msgMain, msgPlanesPActos, "d", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                    oBasic.SPError(msgMain, msgPlanesPActos, "d", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
            }
            catch { }
        }
        #endregion

        #region---PLANESP_Licencias
        private void fPlanesPLicenciasLoadGV(string p_cod_planp)
        {
            if (string.IsNullOrEmpty(p_cod_planp))
                p_cod_planp = "0";
            oVar.prDSPlanesPLicencias = oPlanesPLicencias.sp_s_planesp_licencias_cod_planp(p_cod_planp);

            gvPlanesPLicencias.DataSource = ((DataSet)(oVar.prDSPlanesPLicencias));
            gvPlanesPLicencias.DataBind();

            if (gvPlanesPLicencias.Rows.Count > 0)
            {
                gvPlanesPLicencias.Visible = true;
                oBasic.FixPanel(divData, "PlanesPLicencias", 0);
            }
            else
            {
                gvPlanesPLicencias.Visible = false;
            }
            gv_Sorting(gvPlanesPLicencias, ViewState["SortExpPlanesPLicencias"].ToString(), oUtil.OpDirection(ViewState["SortDirPlanesPLicencias"].ToString()), oVar.prDSPlanesPLicencias);
            oVar.prDSPlanesPLicencias = oVar.prDataSet;
            oBasic.FixPanel(divData, "PlanesPLicencias", 0);
        }

        private void fPlanesPLicenciasDetalle()
        {
            mvPlanesPLicencias.ActiveViewIndex = 1;
            int Indice = Convert.ToInt16(ViewState["IndexPlanesPLicencias"]);
            DataSet dsTmp = new DataSet();
            dsTmp = (DataSet)oVar.prDSPlanesPLicencias;
            DataRow dRow = dsTmp.Tables[0].Rows[Indice];

            oBasic.fValueControls(vPlanesPLicenciasDetalle, dRow);
            if (string.IsNullOrEmpty(dsTmp.Tables[0].Rows[Indice]["time_scan"].ToString()))
            {
                lb_pdf_planp_licencia_delete.Visible = false;
                lb_pdf_planp_licencia_doc.Visible = false;
            }
            else
            {
                lb_pdf_planp_licencia_delete.Visible = true;
                lb_pdf_planp_licencia_doc.Visible = true;
            }
        }

        private void fPlanesPLicenciasEstadoDetalle(bool HabilitarCampos)
        {
            txt_au_planp_licencia.Enabled = false;
            txt_cod_planp__licencia.Enabled = false;
            fu_pdf_planp_licencia.Visible = false;
            ddlb_id_tipo_licencia_SelectedIndexChanged(null, null);
        }

        private void fPlanesPLicenciasInsert()
        {
            string strResultado = oPlanesPLicencias.sp_i_planp_licencia(
              gvPlanesP.SelectedDataKey["au_planp"].ToString(),
              oBasic.fInt(ddlb_id_fuente_informacion),
              oBasic.fInt(ddlb_id_tipo_licencia),
              txt_numero_licencia.Text,
              oBasic.fInt(txt_curador),
              txt_fecha_ejecutoria.Text,
              oBasic.fInt(txt_termino_vigencia_meses),
              txt_plano_urbanistico_aprobado.Text,
              txt_nombre_proyecto.Text,
              oBasic.fDec(txt_area_bruta__licencia),
              oBasic.fDec(txt_area_neta),
              oBasic.fDec(txt_area_util__licencia),
              oBasic.fDec(txt_area_cesion_zonas_verdes),
              oBasic.fDec(txt_area_cesion_vias),
              oBasic.fDec(txt_area_cesion_eq_comunal),
              oBasic.fPerc(txt_porc_ejecucion_urbanismo),
              oBasic.fInt(ddlb_id_obligacion_VIS),
              oBasic.fInt(ddlb_id_obligacion_VIP),
              oBasic.fDec(txt_area_terreno_VIS),
              oBasic.fDec(txt_area_terreno_no_VIS),
              oBasic.fDec(txt_area_terreno_VIP),
              oBasic.fDec(txt_area_construida_VIS),
              oBasic.fDec(txt_area_construida_no_VIS),
              oBasic.fDec(txt_area_construida_VIP),
              oBasic.fDec(txt_porc_obligacion_VIS),
              oBasic.fDec(txt_porc_obligacion_VIP),
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
              oBasic.fPerc(txt_porc_ejecucion_construccion),
              chk_cumple_area_obligacion.Checked,
              chk_cumple_porc_area_util.Checked,
              txt_observacion__licencia.Text
            );
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                oBasic.SPOk(msgMain, msgPlanesPLicencias, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
            else
                oBasic.SPError(msgMain, msgPlanesPLicencias, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
        }

        private void fPlanesPLicenciasUpdate()
        {
            if (Convert.ToBoolean(oVar.prHayCambiosPlanesPLicencias))
            {
                string strResultado = oPlanesPLicencias.sp_u_planp_licencia(
                  oBasic.fInt(txt_au_planp_licencia),
                  oBasic.fInt(ddlb_id_fuente_informacion),
                  oBasic.fInt(ddlb_id_tipo_licencia),
                  txt_numero_licencia.Text,
                  oBasic.fInt(txt_curador),
                  txt_fecha_ejecutoria.Text,
                  oBasic.fInt(txt_termino_vigencia_meses),
                  txt_plano_urbanistico_aprobado.Text,
                  txt_nombre_proyecto.Text,
                  oBasic.fDec(txt_area_bruta__licencia),
                  oBasic.fDec(txt_area_neta),
                  oBasic.fDec(txt_area_util__licencia),
                  oBasic.fDec(txt_area_cesion_zonas_verdes),
                  oBasic.fDec(txt_area_cesion_vias),
                  oBasic.fDec(txt_area_cesion_eq_comunal),
                  oBasic.fPerc(txt_porc_ejecucion_urbanismo),
                  oBasic.fInt(ddlb_id_obligacion_VIS),
                  oBasic.fInt(ddlb_id_obligacion_VIP),
                  oBasic.fDec(txt_area_terreno_VIS),
                  oBasic.fDec(txt_area_terreno_no_VIS),
                  oBasic.fDec(txt_area_terreno_VIP),
                  oBasic.fDec(txt_area_construida_VIS),
                  oBasic.fDec(txt_area_construida_no_VIS),
                  oBasic.fDec(txt_area_construida_VIP),
                  oBasic.fDec(txt_porc_obligacion_VIS),
                  oBasic.fDec(txt_porc_obligacion_VIP),
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
                  oBasic.fPerc(txt_porc_ejecucion_construccion),
                  chk_cumple_area_obligacion.Checked,
                  chk_cumple_porc_area_util.Checked,
                  txt_observacion__licencia.Text,
                  fu_pdf_planp_licencia.HasFiles
                );

                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    string pdf_file = oVar.prPathPlanesPLicencias + txt_au_planp_licencia.Text + ".pdf";
                    oBasic.LoadPdf(fu_pdf_planp_licencia, pdf_file);
                    if (oVar.prError.ToString() != "0")
                    {
                        oBasic.SPError(msgMain, msgPlanesPLicencias, "fl-u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
                    }
                    else
                    {
                        oBasic.SPOk(msgMain, msgPlanesPLicencias, "fl-u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                    }

                }
                else
                    oBasic.SPError(msgMain, msgPlanesPLicencias, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
            }
            else
            {
                oBasic.SPOk(msgMain, msgPlanesPLicencias, "", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void fPlanesPLicenciasDelete()
        {
            try
            {
                string strResultado = oPlanesPLicencias.sp_d_planp_licencia(gvPlanesPLicencias.SelectedDataKey["au_planp_licencia"].ToString());
                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fPlanesPLicenciasDelete:", clConstantes.MSG_OK_D);
                    oBasic.fClearControls(vPlanesPLicenciasDetalle);

                    if (Convert.ToInt16(ViewState["IndexPlanesPLicencias"]) > 0)
                        ViewState["IndexPlanesPLicencias"] = Convert.ToInt16(ViewState["IndexPlanesPLicencias"]) - 1;
                    else
                        ViewState["IndexPlanesPLicencias"] = 0;

                    oBasic.SPOk(msgMain, msgPlanesPLicencias, "d", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                    oBasic.SPError(msgMain, msgPlanesPLicencias, "d", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
            }
            catch { }
        }

        protected void ddlb_id_tipo_licencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool b1 = false;
            bool b2 = false;
            int i = 0;
            try
            {
                i = Convert.ToInt16(ddlb_id_tipo_licencia.SelectedItem.Value);
            }
            catch
            { }

            if (i == 85 || i == 186)
                b2 = true;
            if (i == 86 || i == 186)
                b1 = true;

            oBasic.EnablePanel(pPlanesPLicenciasUrb, b1, true);
            oBasic.EnablePanel(pPlanesPLicenciasConst, b2, true);

            PlanesPLicencias_Changed();
        }

        #endregion

        #region---PLANESP_Visitas
        private void fPlanesPVisitasLoadGV(string p_cod_planp)
        {
            if (string.IsNullOrEmpty(p_cod_planp))
                p_cod_planp = "0";
            oVar.prDSPlanesPVisitas = oPlanesPVisitas.sp_s_planesp_visitas_cod_planp(p_cod_planp);

            gvPlanesPVisitas.DataSource = ((DataSet)(oVar.prDSPlanesPVisitas));
            gvPlanesPVisitas.DataBind();

            if (gvPlanesPVisitas.Rows.Count > 0)
            {
                gvPlanesPVisitas.Visible = true;
                gvPlanesPVisitas.SelectedIndex = Convert.ToInt16(ViewState["IndexPlanesPVisitas"].ToString());
                if (gvPlanesPVisitas.Rows.Count < gvPlanesPVisitas.SelectedIndex)
                    gvPlanesPVisitas.SelectedIndex = 0;
                oVar.prPlanesPVisitasAu = gvPlanesPVisitas.SelectedDataKey.Value.ToString();
                oBasic.FixPanel(divData, "PlanesPVisitas", 0);
            }
            else
            {
                gvPlanesPVisitas.Visible = false;
            }
            gv_Sorting(gvPlanesPVisitas, ViewState["SortExpPlanesPVisitas"].ToString(), oUtil.OpDirection(ViewState["SortDirPlanesPVisitas"].ToString()), oVar.prDSPlanesPVisitas);
            oVar.prDSPlanesPVisitas = oVar.prDataSet;
            oBasic.FixPanel(divData, "PlanesPVisitas", 0);
        }

        private void fPlanesPVisitasDetalle()
        {
            mvPlanesPVisitas.ActiveViewIndex = 1;
            int Indice = Convert.ToInt16(ViewState["IndexPlanesPVisitas"]);
            DataSet dsTmp = new DataSet();
            dsTmp = (DataSet)oVar.prDSPlanesPVisitas;
            DataRow dRow = dsTmp.Tables[0].Rows[Indice];

            oBasic.fValueControls(vPlanesPVisitasDetalle, dRow);
        }

        private void fPlanesPVisitasEstadoDetalle(bool b)
        {
            txt_au_planp_visita.Enabled = false;
            txt_cod_planp__visita.Enabled = false;
            oBasic.StyleCtl("V", b, lb_PlanesPVisitasFotos, false);
        }

        private void fPlanesPVisitasInsert()
        {
            string strResultado = oPlanesPVisitas.sp_i_planp_visita(
              gvPlanesP.SelectedDataKey["au_planp"].ToString(),
              txt_fecha_visita.Text,
              txt_observacion__visita.Text
            );
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                oBasic.SPOk(msgMain, msgPlanesPVisitas, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
            else
                oBasic.SPError(msgMain, msgPlanesPVisitas, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
        }

        private void fPlanesPVisitasUpdate()
        {
            if (Convert.ToBoolean(oVar.prHayCambiosPlanesPVisitas))
            {
                string strResultado = oPlanesPVisitas.sp_u_planp_visita(
                  oBasic.fInt(txt_au_planp_visita),
                  txt_fecha_visita.Text,
                  txt_observacion__visita.Text
                );
                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                    oBasic.SPOk(msgMain, msgPlanesPVisitas, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                else
                    oBasic.SPError(msgMain, msgPlanesPVisitas, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
            }
            else
            {
                oBasic.SPOk(msgMain, msgPlanesPVisitas, "", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void fPlanesPVisitasDelete()
        {
            try
            {
                string strResultado = oPlanesPVisitas.sp_d_planp_visita(gvPlanesPVisitas.SelectedDataKey.Value.ToString());
                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fPlanesPVisitasDelete:", clConstantes.MSG_OK_D);
                    oBasic.fClearControls(vPlanesPVisitasDetalle);

                    if (Convert.ToInt16(ViewState["IndexPlanesPVisitas"]) > 0)
                        ViewState["IndexPlanesPVisitas"] = Convert.ToInt16(ViewState["IndexPlanesPVisitas"]) - 1;
                    else
                        ViewState["IndexPlanesPVisitas"] = 0;

                    oBasic.SPOk(msgMain, msgPlanesPVisitas, "d", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                    oBasic.SPError(msgMain, msgPlanesPVisitas, "d", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResultado);
            }
            catch { }
        }
        #endregion
        #endregion

        #region--Generales
        protected void gv_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header)
                return;

            GridView gv = sender as GridView;
            string modulo = gv.ID.Substring(2);
            string sortExpression = ViewState["SortExp" + modulo].ToString();
            string sortDirection = ViewState["SortDir" + modulo].ToString();
            foreach (TableCell tableCell in e.Row.Cells)
            {
                if (!tableCell.HasControls())
                    continue;
                LinkButton lbSort = tableCell.Controls[0] as LinkButton;
                if (lbSort == null)
                    continue;
                if (lbSort.CommandArgument == sortExpression)
                {
                    Image imageSort = new Image();
                    imageSort.ImageAlign = ImageAlign.AbsMiddle;
                    imageSort.Width = 12;
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
                oBasic.LblRegistros(up, gv.Rows.Count, Convert.ToInt32(ViewState["Index" + modulo]));
            }
            catch { }
        }

        private void gv_Sorting(GridView gv, string sortExpression, string sortDirection, object ds)
        {
            string modulo = gv.ID.Substring(2);
            if (ViewState["SortExp" + modulo].ToString() != sortExpression)
                sortDirection = "ASC";
            else
            {
                if (sortDirection == "ASC")
                    sortDirection = "DESC";
                else
                    sortDirection = "ASC";
            }
            ViewState["SortExp" + modulo] = sortExpression;
            ViewState["SortDir" + modulo] = sortDirection;
            gv.SelectedIndex = 0;

            DataView dataView = new DataView(((DataSet)(ds)).Tables[0]);
            dataView.Sort = sortExpression + " " + sortDirection;
            gv.DataSource = dataView;
            gv.DataBind();
            oVar.prDataSet = oUtil.ConvertToDataSet(dataView);
            gv_SelectedIndexChanged(gv);
        }

        private void LoadDropDowns()
        {
            ddlb_cod_localidad.DataSource = oLocalidades.sp_s_localidades();
            ddlb_cod_localidad.DataTextField = "nombre_localidad";
            ddlb_cod_localidad.DataValueField = "cod_localidad";
            ddlb_cod_localidad.DataBind();

            ddlb_id_categoria_planp.DataSource = oIdentidades.sp_s_identidad_id_categoria("32");
            ddlb_id_categoria_planp.DataTextField = "nombre_identidad";
            ddlb_id_categoria_planp.DataValueField = "id_identidad";
            ddlb_id_categoria_planp.DataBind();

            ddlb_id_estado_planp.DataSource = oIdentidades.sp_s_identidad_id_categoria("22");
            ddlb_id_estado_planp.DataTextField = "nombre_identidad";
            ddlb_id_estado_planp.DataValueField = "id_identidad";
            ddlb_id_estado_planp.DataBind();

            ddlb_id_tipo_tratamiento.DataSource = oIdentidades.sp_s_identidad_id_categoria("21");
            ddlb_id_tipo_tratamiento.DataTextField = "nombre_identidad";
            ddlb_id_tipo_tratamiento.DataValueField = "id_identidad";
            ddlb_id_tipo_tratamiento.DataBind();

            ddlb_id_clasificacion_suelo.DataSource = oIdentidades.sp_s_identidad_id_categoria("31");
            ddlb_id_clasificacion_suelo.DataTextField = "nombre_identidad";
            ddlb_id_clasificacion_suelo.DataValueField = "id_identidad";
            ddlb_id_clasificacion_suelo.DataBind();

            ddlb_id_estado_declaratoria.DataSource = oIdentidades.sp_s_identidad_id_categoria("43");
            ddlb_id_estado_declaratoria.DataTextField = "nombre_identidad";
            ddlb_id_estado_declaratoria.DataValueField = "id_identidad";
            ddlb_id_estado_declaratoria.DataBind();

            ddlb_id_uso_manzana.DataSource = oIdentidades.sp_s_identidad_id_categoria("34");
            ddlb_id_uso_manzana.DataTextField = "nombre_identidad";
            ddlb_id_uso_manzana.DataValueField = "id_identidad";
            ddlb_id_uso_manzana.DataBind();

            ddlb_id_tipo_cesion.DataSource = oIdentidades.sp_s_identidad_id_categoria("48");
            ddlb_id_tipo_cesion.DataTextField = "nombre_identidad";
            ddlb_id_tipo_cesion.DataValueField = "id_identidad";
            ddlb_id_tipo_cesion.DataBind();

            ddlb_id_tipo_acto.DataSource = oIdentidades.sp_s_identidad_id_categoria("6");
            ddlb_id_tipo_acto.DataTextField = "nombre_identidad";
            ddlb_id_tipo_acto.DataValueField = "id_identidad";
            ddlb_id_tipo_acto.DataBind();

            ddlb_id_fuente_informacion.DataSource = oIdentidades.sp_s_identidad_id_categoria("15");
            ddlb_id_fuente_informacion.DataTextField = "nombre_identidad";
            ddlb_id_fuente_informacion.DataValueField = "id_identidad";
            ddlb_id_fuente_informacion.DataBind();

            ddlb_id_tipo_licencia.DataSource = oIdentidades.sp_s_identidad_id_categoria("16");
            ddlb_id_tipo_licencia.DataTextField = "nombre_identidad";
            ddlb_id_tipo_licencia.DataValueField = "id_identidad";
            ddlb_id_tipo_licencia.DataBind();

            ddlb_id_obligacion_VIS.DataSource = oIdentidades.sp_s_identidad_id_categoria("17");
            ddlb_id_obligacion_VIS.DataTextField = "nombre_identidad";
            ddlb_id_obligacion_VIS.DataValueField = "id_identidad";
            ddlb_id_obligacion_VIS.DataBind();

            ddlb_id_obligacion_VIP.DataSource = oIdentidades.sp_s_identidad_id_categoria("18");
            ddlb_id_obligacion_VIP.DataTextField = "nombre_identidad";
            ddlb_id_obligacion_VIP.DataValueField = "id_identidad";
            ddlb_id_obligacion_VIP.DataBind();

            oBasic.fLoadUsuariosFiltro(ddlb_cod_usu_responsable, 10);
        }

        private void ValidarSP()
        {

            oBasic.EnableCtl(btnPlanesPAdd, oPermisos.TienePermisosSP("sp_i_planp"));
            oBasic.EnableCtl(btnPlanesPEdit, oPermisos.TienePermisosSP("sp_u_planp"));
            oBasic.EnableCtl(btnPlanesPDel, oPermisos.TienePermisosSP("sp_d_planp"));
            upPlanesPFoot.Update();

            oBasic.EnableCtl(btnPlanesPManzanasAdd, oPermisos.TienePermisosSP("sp_i_planp_manzana"));
            oBasic.EnableCtl(btnPlanesPManzanasEdit, oPermisos.TienePermisosSP("sp_u_planp_manzana"));
            oBasic.EnableCtl(btnPlanesPManzanasDel, oPermisos.TienePermisosSP("sp_d_planp_manzana"));
            upPlanesPManzanasFoot.Update();

            oBasic.EnableCtl(btnPlanesPCesionesAdd, oPermisos.TienePermisosSP("sp_i_planp_cesion"));
            oBasic.EnableCtl(btnPlanesPCesionesEdit, oPermisos.TienePermisosSP("sp_u_planp_cesion"));
            oBasic.EnableCtl(btnPlanesPCesionesDel, oPermisos.TienePermisosSP("sp_d_planp_cesion"));
            upPlanesPCesionesFoot.Update();

            oBasic.EnableCtl(btnPlanesPActosAdd, oPermisos.TienePermisosSP("sp_i_planp_acto"));
            oBasic.EnableCtl(btnPlanesPActosEdit, oPermisos.TienePermisosSP("sp_u_planp_acto"));
            oBasic.EnableCtl(btnPlanesPActosDel, oPermisos.TienePermisosSP("sp_d_planp_acto"));
            upPlanesPActosFoot.Update();

            oBasic.EnableCtl(btnPlanesPLicenciasAdd, oPermisos.TienePermisosSP("sp_i_planp_licencia"));
            oBasic.EnableCtl(btnPlanesPLicenciasEdit, oPermisos.TienePermisosSP("sp_u_planp_licencia"));
            oBasic.EnableCtl(btnPlanesPLicenciasDel, oPermisos.TienePermisosSP("sp_d_planp_licencia"));
            upPlanesPLicenciasFoot.Update();

            oBasic.EnableCtl(btnPlanesPVisitasAdd, oPermisos.TienePermisosSP("sp_i_planp_visita"));
            oBasic.EnableCtl(btnPlanesPVisitasEdit, oPermisos.TienePermisosSP("sp_u_planp_visita"));
            oBasic.EnableCtl(btnPlanesPVisitasDel, oPermisos.TienePermisosSP("sp_d_planp_visita"));
            upPlanesPVisitasFoot.Update();
        }

        #region---CAMBIOS
        protected void PlanesPManzanas_Changed(object sender = null, EventArgs e = null)
        {
            oVar.prHayCambiosPlanesPManzanas = true;
        }

        protected void PlanesPCesiones_Changed(object sender = null, EventArgs e = null)
        {
            oVar.prHayCambiosPlanesPCesiones = true;
        }

        protected void PlanesPActos_Changed(object sender = null, EventArgs e = null)
        {
            oVar.prHayCambiosPlanesPActos = true;
        }

        protected void PlanesPLicencias_Changed(object sender = null, EventArgs e = null)
        {
            oVar.prHayCambiosPlanesPLicencias = true;
        }

        protected void PlanesPVisitas_Changed(object sender = null, EventArgs e = null)
        {
            oVar.prHayCambiosPlanesPVisitas = true;
        }
        #endregion

        #endregion


        protected void ucFileUpload_UserControlException(object sender, Exception e)
        {
            MessageInfo.ShowMessage(e.Message);
        }

        protected void MessageBox_Accept(string key)
        {

        }

        protected void ucFileUpload_ViewDoc(object sender)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "verDoc();", true);
        }
    }
}