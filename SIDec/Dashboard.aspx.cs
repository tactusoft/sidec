using GLOBAL.DAL;
using SigesTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIDec
{
    public partial class Dashboard : Page
    {
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
                switch ((Session["Indicador.Explore"] ?? "0").ToString())
                {
                    case "0":
                        SetIndicador();
                        break;
                    case "1":
                        pnlConsolidate.Visible = false;
                        pnlLevel2.Visible = true;
                        ind_2.Code = (Session["Indicador.Code"] ?? "").ToString();
                        ind_2.Detail = (Session["Indicador.Detail"] ?? "").ToString();
                        ind_2.IndexAxis = (char)(Session["Indicador.IndexAxis"] ?? "");
                        ind_2.Type = (Session["Indicador.Type"] ?? "").ToString();
                        SetIndicador(false);
                        break;
                    default:
                        LoadGrid();
                        break;
                }
                Session["Indicador.Explore"] = "0";
                
            }
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (!DateTime.TryParse(txt_mes_visualizacion.Text + "-01", out DateTime date)) return;
            ind_2.Code = Session["Indicador.Code"].ToString();
            ind_2.DateFilter = date;
            ind_2.LoadExcel();
        }
        protected void btnMinimize_Click(object sender, EventArgs e)
        {
            Session["Indicador.mes_visualización"] = txt_mes_visualizacion.Text;
            Session["Indicador.Detail"] = 0;
            Session["Indicador.Explore"] = "0";
            Response.Redirect("dashboard");
        }        
        protected void gvDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string code = Session["Indicador.Code"].ToString();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Verifica si la columna "color" existe en el DataRowView
                var dataItem = e.Row.DataItem as DataRowView;
                if (dataItem != null && dataItem.Row.Table.Columns.Contains("ordenBack"))
                {
                    // Obtén el valor de la columna "color"
                    bool colorbackValue = (Convert.ToInt32((dataItem["ordenBack"] ?? "0").ToString()) % 2 == 1);

                    e.Row.BackColor = colorbackValue ? System.Drawing.Color.LightCyan : System.Drawing.Color.White;
                }
                if (dataItem != null && dataItem.Row.Table.Columns.Contains("ordenBack2"))
                {
                    // Obtén el valor de la columna "color"
                    bool colorbackValue = (Convert.ToInt32((dataItem["ordenBack2"] ?? "0").ToString()) == 1);
                    if (colorbackValue)
                        e.Row.Style["border-top"] = "2px solid black";
                }
                if (dataItem != null && dataItem.Row.Table.Columns.Contains("color"))
                {
                    // Obtén el valor de la columna "color"
                    string colorValue = dataItem["color"].ToString();

                    // Verifica que el colorValue tiene un valor válido antes de aplicar el color
                    if (!string.IsNullOrEmpty(colorValue))
                    {
                        // Convierte el valor RGBA a Color
                        var color = UserControls.Indicador.Graphics.ConvertRgbaToColor(colorValue);

                        switch (code)
                        {
                            case "1":
                                e.Row.Cells[3].BackColor = color;
                                break;
                        }
                    }
                }
            }

            int MAX_COLUMN = int.MaxValue;
            switch (code)
            {
                case "1":
                    MAX_COLUMN = 11;
                    break;
                case "2":
                    MAX_COLUMN = 9;
                    break;
            }
            HideColumns(e.Row, MAX_COLUMN);
        }
        protected void txt_mes_visualizacion_TextChanged(object sender, EventArgs e)
        {
            Session["Indicador.mes_visualización"] = txt_mes_visualizacion.Text;
            Response.Redirect("dashboard");
        }
        #endregion


        #region Métodos
        private void HideColumns(GridViewRow row,  int maxVisibleColumn)
        {
            for(int i = maxVisibleColumn; i< row.Cells.Count; i++)
                row.Cells[i].Visible = false;
        }
        private void Initialize()
        {
            ind_2.Reload();
            string text_session = (Session["Indicador.mes_visualización"] ?? "").ToString();
            txt_mes_visualizacion.Text = text_session == "" ? DateTime.Now.ToString("yyyy-MM") : text_session;
            Session["Indicador.mes_visualización"] = null;
            if (Session["Indicador.Referencias"] == null)
                using (Indicador_DAL dal = new Indicador_DAL())
                {
                    Session["Indicador.Referencias"] = dal.GetIndicadores_Referencia();
                }
        }
        private void LoadGrid()
        {
            pnlConsolidate.Visible = false;
            pnlDetail.Visible = true;

            string v_code = Session["Indicador.Code"].ToString();
            if (!DateTime.TryParse(txt_mes_visualizacion.Text + "-01", out DateTime date))
                return;
         
            string p_anio = date.Year.ToString();
            string p_mes = date.Month.ToString();

            using (Indicador_DAL dal = new Indicador_DAL())
            {
                DataSet ds = dal.GetIndicadorDetalle(v_code, p_anio, p_mes);
                if (ds != null && ds.Tables.Count > 0) {
                    gvDetail.DataSource = ds;
                    gvDetail.DataBind();
                }
            }

            List<IndicadorReferenciaTO> references = (List<IndicadorReferenciaTO>)Session["Indicador.Referencias"];
            if (references != null)
            {
                var reference = references.Where(rg => rg.IdReferencia.ToString() == v_code).FirstOrDefault();
                if (reference != null)
                {
                    lbl_ind.Text = reference.Nombre;
                    lbl_ind.ToolTip = reference.Nombre;
                }
            }

            RegisterScript();
        }
        private void RegisterScript()
        {
            string key = "LoadGridDashboard";
            StringBuilder scriptSlider = new StringBuilder();
            scriptSlider.Append("<script type='text/javascript'> ");
            scriptSlider.Append("   $(document).ready(gridviewScrollIndicador());  ");
            scriptSlider.Append(" </script> ");

            if (!Page.ClientScript.IsStartupScriptRegistered(key))
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), key, scriptSlider.ToString(), false);
        }
        private void SetIndicador(bool full = true)
        {
            if (!DateTime.TryParse(txt_mes_visualizacion.Text + "-01", out DateTime date))
                return;
            
            string section = "";
            if (full)
            {
                ind_1_1.DateFilter = ind_1_2.DateFilter = date;

                section += ind_1_1.GetChartDataReport();
                section += getPrueba();
                section += ind_1_2.GetChartDataReport();
            }
            else
            {
                ind_2.DateFilter = date;
                section += ind_2.GetChartDataReport();
            }

            if (section == string.Empty)
            {
                return;
            }

            string key = "script_Indicadores" + DateTime.Now.ToString("yyMMdd_HHmmss");


            if (!Page.ClientScript.IsStartupScriptRegistered(Page.GetType(), key))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), key, section, false);
            }
        }

        private string getPrueba()
        {
            return "";
                //"   <script>" +
                //"       Sys.Application.add_load(function() { " +
                //"           var ctx = document.getElementById('myChart').getContext('2d'); " +
                //"           var myChart = new Chart(ctx, {" +
                //"               type: 'bar'," +
                //"               data: {" +
                //"                   labels:['Proyecto 1', 'Proyecto 2', 'Global']," +
                //"                   datasets: [{" +
                //"                       label: 'Área VIP'," +
                //"                       data:[3000, 4000, 7000]," +
                //"                       backgroundColor: 'rgba(255, 00, 00)'," +
                //"                       stack: 'Área'" +
                //"                   }," +
                //"                   {" +
                //"                       label: 'Área VIS'," +
                //"                       data:[2000, 3000, 5000]," +
                //"                       backgroundColor: 'rgba(00, 255, 00)'," +
                //"                       stack: 'Área'" +
                //"                   }," +
                //"                   {" +
                //"                       label: 'Área No VIS'," +
                //"                       data:[1000, 1500, 2500]," +
                //"                       backgroundColor: 'rgba(00, 00, 255)'," +
                //"                       stack: 'Área'" +
                //"                   }," +
                //"                   {" +
                //"                       label: 'Viviendas construidas (%)'," +
                //"                       data:[80, 90, 85]," +
                //"                       type: 'line'," +
                //"                       fill: false," +
                //"                       borderColor: 'rgba(0, 0, 0)'," +
                //"                       yAxisID: 'yLine' " +
                //"                   }]" +
                //"               }," +
                //"               options:{" +
                //"                   scales:{" +
                //"                       x:{" +
                //"                           stacked: true" +
                //"                       }," +
                //"                       y:{" +
                //"                           stacked: true," +
                //"                           beginAtZero: true," +
                //"                               title:{" +
                //"                                   display: true," +
                //"                                   text: 'Área (m2)'" +
                //"                               }" +
                //"                           }," +
                //"                           yLine: { " +
                //"                           beginAtZero: true," +
                //"                           position: 'right', " +
                //"                           ticks: {" +
                //"                               callback: function(value) {" +
                //"                                   return value + '%'; " +
                //"                               }" +
                //"                           }," +
                //"                           title: {" +
                //"                               display: true," +
                //"                               text: 'Viviendas construidas (%)' " +
                //"                           }" +
                //"                       }" +
                //"                   }" +    
                //"               }" +
                //"           });" +
                //"   });" +
                //"</script>";
        }
        #endregion
    }
}