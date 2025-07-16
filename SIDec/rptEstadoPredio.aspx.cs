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
    public partial class rptEstadoPredio : Page
    {
        private readonly DECLARATORIAS_DAL oDeclaratorias = new DECLARATORIAS_DAL();
        private readonly IDENTIDADES_DAL oIdentidades = new IDENTIDADES_DAL();
        private readonly PREDIOSDECLARADOS_DAL oPrediosDeclarados = new PREDIOSDECLARADOS_DAL();
        private readonly USUARIOS_DAL oUsuarios = new USUARIOS_DAL();

        private readonly clBasic oBasic = new clBasic();
        private readonly clPermisos oPermisos = new clPermisos();
        private readonly clUtil oUtil = new clUtil();
        private readonly clGlobalVar oVar = new clGlobalVar();
        private DataSet oDSReporte = new DataSet();

        private string CodDecFiltro
        {
            get { return (Session["rptEstadoPredio.CodDecFiltro"] ?? "-1").ToString(); }
            set { Session["rptEstadoPredio.CodDecFiltro"] = value; }
        }
        private string EstadoFiltro
        {
            get { return (Session["rptEstadoPredio.EstadoFiltro"] ?? "-1").ToString(); }
            set { Session["rptEstadoPredio.EstadoFiltro"] = value; }
        }
        private string EstadoPredioFiltro
        {
            get { return (Session["rptEstadoPredio.EstadoPredioFiltro"] ?? "-1").ToString(); }
            set { Session["rptEstadoPredio.EstadoPredioFiltro"] = value; }
        }
        private string UsuarioFiltro
        {
            get { return (Session["rptEstadoPredio.UsuarioFiltro"] ?? "-1").ToString(); }
            set { Session["rptEstadoPredio.UsuarioFiltro"] = value; }
        }
        private string TiempoFiltro
        {
            get { return (Session["rptEstadoPredio.TiempoFiltro"] ?? "-1").ToString(); }
            set { Session["rptEstadoPredio.TiempoFiltro"] = value; }
        }
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
                LoadDropDowns();
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

            List<string> nombre_hoja = new List<string>{"detalle"}; // nombres hojas
            List<string> origen_hoja = new List<string>{"A2"}; // celdas iniciales

            DataSet nds = ((DataSet)oVar.prDS_rpt_estado_predios).Copy();
            DataColumnCollection dcc = nds.Tables[0].Columns;
            nds.Tables[0].Columns.Remove(dcc[7]); // columnas omitir

            string template = oVar.prPathFormatosOrigen.ToString() + oVar.prPathPlantillaEstadoPredios.ToString();
            oUtil.fExcelExportDSTemplate("Estado Predios", nds, nombre_hoja, origen_hoja, template);
        }
        private void Initialize()
        {
            oDSReporte = null;
            ddl_id_estado_predio_declarado2.SelectedIndex = 1;
        }
        private void LoadDropDowns()
        {
            ddl_cod_declaratoria.DataSource = oDeclaratorias.sp_s_declaratorias();
            ddl_cod_declaratoria.DataTextField = "resolucion";
            ddl_cod_declaratoria.DataValueField = "cod_declaratoria";
            ddl_cod_declaratoria.DataBind();

            LoadUsuario();
            LoadEstado();
        }

        private void LoadEstado()
        {
            ddl_id_estado_predio_declarado.Items.Clear();
            string value = ddl_id_estado_predio_declarado2.SelectedValue;
            ddl_id_estado_predio_declarado.DataSource = oIdentidades.sp_s_identidad_id_categoria_op("9", value);
            ddl_id_estado_predio_declarado.DataTextField = "nombre_identidad";
            ddl_id_estado_predio_declarado.DataValueField = "id_identidad";
            ddl_id_estado_predio_declarado.DataBind();
            ddl_id_estado_predio_declarado.Items.Insert(0, new ListItem("Todos", ""));
        }

        private void LoadUsuario()
        {
            ddl_cod_usuario.Enabled = true;
            if (oPermisos.TienePermisosAccion(cnsSection.REPORTE_GESTION_DOCUMENTOS, cnsAction.CONSULTAR, "", oVar.prUserCod.ToString()))
            {
                DataTable dt = oUsuarios.sp_s_usuarios_filtro(11, -1).Tables[0];
                ddl_cod_usuario.DataSource = dt;
                ddl_cod_usuario.DataTextField = "nombre_completo";
                ddl_cod_usuario.DataValueField = "cod_usuario";
                ddl_cod_usuario.DataBind();
                if (dt.Rows.Count > 1)
                    ddl_cod_usuario.Items.Insert(0, new ListItem("Todos", ""));
            }
            else
            {
                ddl_cod_usuario.Items.Insert(0, new ListItem(oVar.prUserName.ToString(), oVar.prUserCod.ToString()));
                ddl_cod_usuario.Enabled = false;
            }

            ddl_cod_usuario.SelectedIndex = 0;
        }
        private void LoadGrid()
        {
            pnlGrids.Visible = true;

            if (oDSReporte != null && oDSReporte.Tables[0].Rows.Count > 0)
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
            if (CodDecFiltro == ddl_cod_declaratoria.SelectedValue && EstadoFiltro == ddl_id_estado_predio_declarado2.SelectedValue 
                && EstadoPredioFiltro == ddl_id_estado_predio_declarado.SelectedValue && UsuarioFiltro == ddl_cod_usuario.SelectedValue
                && TiempoFiltro == ddl_id_tiempo_cumplimiento.SelectedValue)
            {
                oDSReporte = (DataSet)oVar.prDS_rpt_estado_predios;
                return;
            }
            CodDecFiltro = ddl_cod_declaratoria.SelectedValue;
            EstadoFiltro = ddl_id_estado_predio_declarado2.SelectedValue;
            EstadoPredioFiltro = ddl_id_estado_predio_declarado.SelectedValue;
            UsuarioFiltro = ddl_cod_usuario.SelectedValue;
            TiempoFiltro = ddl_id_tiempo_cumplimiento.SelectedValue;

            oDSReporte = oPrediosDeclarados.sp_rpt_estado_predios(
                oBasic.fInt(ddl_cod_declaratoria),
                oBasic.fInt(ddl_cod_usuario),
                oBasic.fInt(ddl_id_estado_predio_declarado),
                oBasic.fInt(ddl_id_estado_predio_declarado2),
                oBasic.fInt(ddl_id_tiempo_cumplimiento));
            oVar.prDS_rpt_estado_predios = oDSReporte;
        }
        #endregion

        protected void ddl_id_estado_predio_declarado2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadEstado();
            upBuscar.Update();
        }
    }
}