using GLOBAL.DAL;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIDec
{
    public partial class rptPlanesP : Page
    {
        private readonly IDENTIDADES_DAL oIdentidades = new IDENTIDADES_DAL();
        private readonly clGlobalVar oVar = new clGlobalVar();
        private readonly clUtil oUtil = new clUtil();
        private readonly PLANESP_DAL oPlanesP = new PLANESP_DAL();
        private readonly clBasic oBasic = new clBasic();

        private DataSet oDSReporte = new DataSet();
        private const string _SOURCEPAGE = "Reporte Planes parciales";


        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnExportar);

            if (!IsPostBack)
            {
                oVar.prItemReportePlanesP = 0;
                LoadDropDowns();
            }
        }
        protected void btnEjecutar_Click(object sender, EventArgs e)
        {
            LoadGrid(oVar.prItemReportePlanesP);
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            GridView gv = (GridView)upReportes.FindControl("gvReporte_" + oVar.prItemReportePlanesP.ToString());

            if (gv.Rows.Count == 0)
            {
                oBasic.SPOk(msgMain, null, "x", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return;
            }

            List<string> nombre_hoja = new List<string>(); ////////// nombres hojas
            List<string> origen_hoja = new List<string>(); ////////// celdas iniciales
            string template = "";
            string title = "";
            bool autofit = true;

            if (oVar.prItemReportePlanesP == 0)
            {
                oVar.prDS_rpt_planesp = oVar.prDS_rpt_planesp_general;
                nombre_hoja.Add("detalle");
                origen_hoja.Add("A3");
                template = oVar.prPathFormatosOrigen.ToString() + oVar.prPathPlanesPGeneral.ToString();
                title = "Planes Parciales General";
            }
            else if (oVar.prItemReportePlanesP == 1)
            {
                oVar.prDS_rpt_planesp = oVar.prDS_rpt_planesp_cesiones;
                nombre_hoja.Add("detalle");
                origen_hoja.Add("A3");
                template = oVar.prPathFormatosOrigen.ToString() + oVar.prPathPlanesPCesiones.ToString();
                title = "Planes Parciales Cesiones";
            }
            else if (oVar.prItemReportePlanesP == 2)
            {
                oVar.prDS_rpt_planesp = oVar.prDS_rpt_planesp_indicadores;
                nombre_hoja.Add("detalle");
                nombre_hoja.Add("anual");
                nombre_hoja.Add("resumen");
                origen_hoja.Add("A3");
                origen_hoja.Add("A3");
                origen_hoja.Add("A3");
                template = oVar.prPathFormatosOrigen.ToString() + oVar.prPathPlanesPIndicadores.ToString();
                title = "Planes Parciales Indicadores";
                autofit = false;
            }
            oUtil.fExcelExportDSTemplate(title, (DataSet)oVar.prDS_rpt_planesp, nombre_hoja, origen_hoja, template, autofit);
        }
        protected void btnReporte_Click(object sender, EventArgs e)
        {
            int oldIndex = mvReportes.ActiveViewIndex;
            int newIndex = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            string oldID = "lbReporte_" + oldIndex.ToString();
            string newID = "lbReporte_" + newIndex.ToString();
            LinkButton lbOld = (LinkButton)ulReportes.FindControl(oldID);
            LinkButton lbNew = (LinkButton)ulReportes.FindControl(newID);
            oBasic.ActiveNav(mvReportes, lbOld, lbNew, newIndex);
            oVar.prItemReportePlanesP = newIndex;

            if (newIndex == 2)
                oBasic.EnablePanel(pParam2, false, false);
            else
                oBasic.EnablePanel(pParam2, true, true);
        }
        #endregion

        #region Métodos
        private void LoadGrid(int index)
        {
            if (index == 0)
            {
                oDSReporte = oPlanesP.sp_rpt_planesp_general(
                  oBasic.fInt(ddlb_id_tipo_tratamiento),
                  oBasic.fInt(ddlb_rango_1),
                  oBasic.fInt(ddlb_rango_2),
                  oBasic.fInt(ddlb_ano_1),
                  oBasic.fInt(ddlb_ano_2));
                oVar.prDS_rpt_planesp_general = oDSReporte;
                lbl_total_0.Text = "Registros: " + oDSReporte.Tables[0].Rows.Count.ToString();
            }
            else if (index == 1)
            {
                oDSReporte = oPlanesP.sp_rpt_planesp_cesiones(
                  oBasic.fInt(ddlb_id_tipo_tratamiento),
                  oBasic.fInt(ddlb_rango_1),
                  oBasic.fInt(ddlb_rango_2),
                  oBasic.fInt(ddlb_ano_1),
                  oBasic.fInt(ddlb_ano_2));
                oVar.prDS_rpt_planesp_cesiones = oDSReporte;
                lbl_total_1.Text = "Registros: " + oDSReporte.Tables[0].Rows.Count.ToString();
            }
            else if (index == 2)
            {
                oDSReporte = oPlanesP.sp_rpt_planesp_indicadores(
                  oBasic.fInt(ddlb_id_tipo_tratamiento),
                  oBasic.fInt(ddlb_ano_1),
                  oBasic.fInt(ddlb_ano_2));
                oVar.prDS_rpt_planesp_indicadores = oDSReporte;
                lbl_total_2.Text = "Registros: " + oDSReporte.Tables[0].Rows.Count.ToString() +
                   " / " + oDSReporte.Tables[1].Rows.Count.ToString() +
                   " / " + oDSReporte.Tables[2].Rows.Count.ToString();
            }
            else
                return;

            GridView gv = (GridView)upReportes.FindControl("gvReporte_" + index.ToString());
            gv.DataSource = oDSReporte.Tables[0];
            gv.DataBind();

            try
            {
                GridView gv1 = (GridView)upReportes.FindControl("gvReporte_" + index.ToString() + "_1");
                gv1.DataSource = oDSReporte.Tables[1];
                gv1.DataBind();

                try
                {
                    GridView gv2 = (GridView)upReportes.FindControl("gvReporte_" + index.ToString() + "_2");
                    gv2.DataSource = oDSReporte.Tables[2];
                    gv2.DataBind();
                }
                catch { }
            }
            catch { }

        }
        private void LoadDropDowns()
        {
            ddlb_id_tipo_tratamiento.DataSource = oIdentidades.sp_s_identidad_id_categoria("21");
            ddlb_id_tipo_tratamiento.DataTextField = "nombre_identidad";
            ddlb_id_tipo_tratamiento.DataValueField = "id_identidad";
            ddlb_id_tipo_tratamiento.DataBind();

            oBasic.ddlbAnos(ddlb_ano_1, "l");
            oBasic.ddlbAnos(ddlb_ano_2, "f");
            oBasic.ddlbPorc10(ddlb_rango_1, "f");
            oBasic.ddlbPorc10(ddlb_rango_2, "l");
        }
        #endregion
    }
}