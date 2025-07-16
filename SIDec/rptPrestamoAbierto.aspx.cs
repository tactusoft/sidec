using GLOBAL.DAL;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIDec
{
    public partial class rptPrestamoAbierto : Page
    {
        private readonly PRESTAMOS_DAL oPrestamos = new PRESTAMOS_DAL();

        private readonly clUtil oUtil = new clUtil();
        private readonly clGlobalVar oVar = new clGlobalVar();
        private DataSet oDSReporte = new DataSet();

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadReport();
                LoadGrid();
            }
        }
        protected void btnExportar_Click(object sender, EventArgs e)
        {
            Export();
        }
        #endregion

        #region Métodos
        private void Export()
        {
            LoadReport();
            if (oDSReporte.Tables[0].Rows.Count == 0)
            {
                MessageInfo.ShowMessage("No se encontraron registros asociados. Verifique los filtros ingresados.");

                Session["ReloadXFU"] = "1";
                (this.Master as AuthenticNew).fReload();
                return;
            }

            List<string> nombre_hoja = new List<string> { "detalle" }; //nombres hojas
            List<GridViewRow> gvr = new List<GridViewRow> { gvDetalle.HeaderRow }; // títulos de columna

            oUtil.fExcelExportDS("Préstamos Abiertos", (DataSet)oVar.prDS_rpt_prestamos_abiertos, nombre_hoja, gvr);
        }
        private void LoadGrid()
        {
            if (oDSReporte.Tables[0].Rows.Count > 0)
            {
                DataTable dtDetail = oDSReporte.Tables[0].AsEnumerable().Take(100).CopyToDataTable();
                gvDetalle.DataSource = dtDetail;
                lbl_total_detalle.Text = "Detalle: " + oDSReporte.Tables[0].Rows.Count.ToString() + " Registros. " + (oDSReporte.Tables[0].Rows.Count > 100 ? "(Se presentan los 100 primeros)" : "");
            }
            else
            {
                gvDetalle.DataSource = null;
                lbl_total_detalle.Text = "No se encontraron registros asosiados";
            }
            gvDetalle.DataBind();
        }
        private void LoadReport()
        {
            oDSReporte = oPrestamos.sp_rpt_prestamos_abiertos();
            oVar.prDS_rpt_prestamos_abiertos = oDSReporte;
        }
        #endregion
    }
}