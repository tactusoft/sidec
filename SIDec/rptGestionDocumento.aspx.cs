using GLOBAL.DAL;
using GLOBAL.PERMISOS;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using cnsAction = GLOBAL.CONST.clConstantes.Accion;
using cnsSection = GLOBAL.CONST.clConstantes.Section;

namespace SIDec
{
    public partial class rptGestionDocumento : Page
    {
        private readonly DOCUMENTOS_DAL oDocumentos = new DOCUMENTOS_DAL();
        private readonly USUARIOS_DAL oUsuarios = new USUARIOS_DAL();

        private readonly clBasic oBasic = new clBasic();
        private readonly clPermisos oPermisos = new clPermisos();
        private readonly clUtil oUtil = new clUtil();
        private readonly clGlobalVar oVar = new clGlobalVar();
        private DataSet oDSReporte = new DataSet();

        private string FechaInicialFiltro
        {
            get { return (Session["rptGestionDocumento.FechaInicialFiltro"] ?? "").ToString(); }
            set { Session["rptGestionDocumento.FechaInicialFiltro"] = value; }
        }
        private string FechaFinalFiltro
        {
            get { return (Session["rptGestionDocumento.FechaFinalFiltro"] ?? "").ToString(); }
            set { Session["rptGestionDocumento.FechaFinalFiltro"] = value; }
        }
        private string UsuarioFiltro
        {
            get { return (Session["rptGestionDocumento.UsuarioFiltro"] ?? "").ToString(); }
            set { Session["rptGestionDocumento.UsuarioFiltro"] = value; }
        }
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
                LoadUsersList();
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            LoadReport();
            LoadGrid();
        }
        protected void btnExportar_Click(object sender, EventArgs e)
        {
            Export();
        }
        protected void btnReporteSection_Click(object sender, EventArgs e)
        {
            int oldIndex = mvReporteSection.ActiveViewIndex;
            int newIndex = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            string oldID = "lbReporteSection_" + oldIndex.ToString();
            string newID = "lbReporteSection_" + newIndex.ToString();
            LinkButton lbOld = (LinkButton)ulReporteSection.FindControl(oldID);
            LinkButton lbNew = (LinkButton)ulReporteSection.FindControl(newID);
            oBasic.ActiveNav(mvReporteSection, lbOld, lbNew, newIndex);
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

            List<string> nombre_hoja = new List<string>{"detalle","resumen"}; // nombres hojas
            List<GridViewRow> gvr = new List<GridViewRow>{gvDetalle.HeaderRow,gvConsolidado.HeaderRow}; // títulos de columna

            oUtil.fExcelExportDS("Gestión Documentos", (DataSet)oVar.prDS_rpt_gestion_documentos, nombre_hoja, gvr);
        }
        private void Initialize()
        {
            txt_fecha_inicial.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
            txt_fecha_final.Text = DateTime.Now.ToString("yyyy-MM-dd");
            cefecha_inicial.StartDate = ce_fecha_final.StartDate = new DateTime(2015, 1, 1);
            rv_fecha_inicial.MinimumValue = rv_fecha_final.MinimumValue = (new DateTime(2015, 1, 1)).ToString("yyyy-MM-dd");
            cefecha_inicial.EndDate = ce_fecha_final.EndDate = DateTime.Today;
            rv_fecha_inicial.MaximumValue = rv_fecha_final.MaximumValue = (DateTime.Today).ToString("yyyy-MM-dd");
        }
        private void LoadUsersList()
        {
            ddlb_cod_usuario.Enabled = true;
            if (oPermisos.TienePermisosAccion(cnsSection.REPORTE_GESTION_DOCUMENTOS, cnsAction.CONSULTAR, "", oVar.prUserCod.ToString()))
            {
                DataTable dt = oUsuarios.sp_s_usuarios_filtro(4, Convert.ToInt32(oVar.prUserCod.ToString())).Tables[0];
                ddlb_cod_usuario.DataSource = dt;
                ddlb_cod_usuario.DataTextField = "nombre_completo";
                ddlb_cod_usuario.DataValueField = "cod_usuario";
                ddlb_cod_usuario.DataBind();
                if (dt.Rows.Count > 1)
                    ddlb_cod_usuario.Items.Insert(0, new ListItem("Todos", ""));
            }
            else
            {
                ddlb_cod_usuario.Items.Insert(0, new ListItem(oVar.prUserName.ToString(), oVar.prUserCod.ToString()));
                ddlb_cod_usuario.Enabled = false;
            }

            ddlb_cod_usuario.Items.FindByText(oVar.prUserName.ToString()).Selected = true;
        }
        private void LoadGrid()
        {
            pnlGrids.Visible = true;

            if (oDSReporte.Tables[0].Rows.Count > 0)
            {
                DataTable dtDetail = oDSReporte.Tables[0].AsEnumerable().Take(100).CopyToDataTable();
                DataTable dtGrouped = oDSReporte.Tables[1].AsEnumerable().Take(100).CopyToDataTable();
                gvConsolidado.DataSource = dtGrouped;
                gvDetalle.DataSource = dtDetail;
                lbl_total_consolidado.Text = "Resumen: " + oDSReporte.Tables[1].Rows.Count.ToString() + " Registros. " + (oDSReporte.Tables[1].Rows.Count > 100 ? "(Se presentan los 100 primeros)" : "");
                lbl_total_detalle.Text = "Detalle: " + oDSReporte.Tables[0].Rows.Count.ToString() + " Registros. " + (oDSReporte.Tables[0].Rows.Count > 100 ? "(Se presentan los 100 primeros)" : "");
            }
            else
            {
                gvDetalle.DataSource = null;
                gvConsolidado.DataSource = null;
                lbl_total_detalle.Text = "No se encontraron registros asosiados";
                lbl_total_consolidado.Text = "No se encontraron registros asosiados";
            }
            gvDetalle.DataBind();
            gvConsolidado.DataBind();
        }
        private void LoadReport()
        {
            if (FechaInicialFiltro == txt_fecha_inicial.Text && FechaFinalFiltro == txt_fecha_final.Text && UsuarioFiltro == ddlb_cod_usuario.SelectedValue)
            {
                oDSReporte = (DataSet)oVar.prDS_rpt_gestion_documentos;
                return;
            }
            FechaInicialFiltro = txt_fecha_inicial.Text;
            FechaFinalFiltro = txt_fecha_final.Text;
            UsuarioFiltro = ddlb_cod_usuario.SelectedValue;
            oDSReporte = oDocumentos.sp_rpt_gestion_documentos(txt_fecha_inicial.Text, txt_fecha_final.Text, ddlb_cod_usuario.SelectedValue);
            oVar.prDS_rpt_gestion_documentos = oDSReporte;
        }
        #endregion
    }
}