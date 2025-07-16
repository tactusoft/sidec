using GLOBAL.CONST;
using GLOBAL.DAL;
using GLOBAL.PERMISOS;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using cnsAction = GLOBAL.CONST.clConstantes.Accion;
using cnsSection = GLOBAL.CONST.clConstantes.Section;

namespace SIDec
{
    public partial class Perfiles : Page
    {
        private readonly PERMISOS_DAL oPermisos = new PERMISOS_DAL();
        private readonly PERFILES_DAL oPerfiles = new PERFILES_DAL();

        private readonly clGlobalVar oVar = new clGlobalVar();
        private readonly clUtil oUtil = new clUtil();
        private readonly clPermisos clsPermisos = new clPermisos();
        private readonly clBasic oBasic = new clBasic();

        private const string _SOURCEPAGE = "Profile";

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterScript();

            if (!IsPostBack)
            {
                Initialize();
                LoadGrid();

                txtBuscar.Focus();
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ViewAdd();
        }
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            if (txt_cod_perfil.Text != "0" && txt_cod_perfil.Text.Trim() != "")
                MessageBox1.ShowConfirmation("Editar", "¿Está seguro de actualizar la información?", type: "warning");
            else
                MessageBox1.ShowConfirmation("Agregar", "¿Está seguro de continuar con la acción solicitada?");
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ViewState["Profile.CriterioBuscar"] = txtBuscar.Text.Trim();
            LoadGrid();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LoadGrid();
            gridSelect();
        }
        protected void btnProfileAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            oBasic.AlertMain(msgMain, "", "0");
            upMsgMain.Update();

            switch (btnAccionSource.CommandName)
            {
                case "Agregar":
                    ViewAdd();
                    break;
                case "Listar":
                    LoadGrid();
                    gridSelect();
                    break;
                case "Editar":
                    ViewEdit();
                    break;
                case "Delete":
                    if (!ValidateAccess(cnsSection.ADMINISTRACION_PERFIL, cnsAction.EDITAR)) return;

                    string perfil = txt_nombre_perfil.Text.ToLower();
                    MessageBox1.ShowConfirmation("Delete", "¿Está seguro de eliminar el registro?", type: "warning");
                    return;
                default:
                    return;
            }
        }
        protected void btnProfileNavegation_Click(object sender, EventArgs e)
        {
            int index = 0;
            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    index = Convert.ToInt16(ViewState["IndexProfile"]) - 1;
                    break;
                case "Next":
                    index = Convert.ToInt16(ViewState["IndexProfile"]) + 1;
                    break;
                case "Last":
                    index = ((DataSet)oVar.prDSPerfiles).Tables[0].Rows.Count - 1;
                    break;
            }
            gvProfile.PageIndex = (index - (index % gvProfile.PageSize)) / gvProfile.PageSize;
            gvProfile.DataSource = ((DataSet)oVar.prDSPerfiles).Tables[0];
            gvProfile.DataBind();
            gvProfile.SelectedIndex = index % gvProfile.PageSize;
            ViewState["IndexProfile"] = index.ToString();

            txt_cod_perfil.Text = index >= 0 ? gvProfile.DataKeys[gvProfile.SelectedIndex]["cod_perfil"].ToString() : "0";

            LoadDetail();
            gv_SelectedIndexChanged(gvProfile);
        }
        protected void chk_f_activos_CheckedChanged(object sender, EventArgs e)
        {
            ViewState["Profile.CriterioBuscar"] = txtBuscar.Text.Trim();
            LoadGrid();
        }
        protected void gvPermissions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataKey dk = gvPermissions.DataKeys[e.Row.DataItemIndex];
                int permission = int.Parse(dk["consultar"].ToString()) + int.Parse(dk["insertar"].ToString()) + int.Parse(dk["modificar"].ToString()) + int.Parse(dk["eliminar"].ToString());
                string tipo = dk["tipo_permiso"].ToString();

                if (permission > 0)
                {
                    e.Row.BackColor = System.Drawing.Color.LightBlue;
                    e.Row.Font.Italic = true;
                }

                ((DropDownList)e.Row.Cells[2].FindControl("ddlRead")).Font.Bold = int.Parse(dk["consultar"].ToString()) > 0;
                ((DropDownList)e.Row.Cells[2].FindControl("ddlCreate")).Font.Bold = int.Parse(dk["insertar"].ToString()) > 0;
                ((DropDownList)e.Row.Cells[2].FindControl("ddlUpdate")).Font.Bold = int.Parse(dk["modificar"].ToString()) > 0;
                ((DropDownList)e.Row.Cells[2].FindControl("ddlDelete")).Font.Bold = int.Parse(dk["eliminar"].ToString()) > 0;

                if (tipo == "SP")
                {
                    ((DropDownList)e.Row.Cells[2].FindControl("ddlCreate")).Visible = false;
                    ((DropDownList)e.Row.Cells[2].FindControl("ddlUpdate")).Visible = false;
                    ((DropDownList)e.Row.Cells[2].FindControl("ddlDelete")).Visible = false;
                }
            }
        }
        protected void gvProfile_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProfile.PageIndex = e.NewPageIndex;
            LoadGrid();
            if (gvProfile.SelectedIndex > gvProfile.Rows.Count)
                gvProfile.SelectedIndex = gvProfile.Rows.Count - 1;
            ViewState["IndexProfile"] = ((gvProfile.PageSize * gvProfile.PageIndex) + gvProfile.SelectedIndex);
        }
        protected void gvProfile_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            oBasic.AlertMain(msgMain, "", "0");
            upMsgMain.Update();

            if (e.CommandName != "_Detail" && e.CommandName != "_Edit" && e.CommandName != "_Delete")
                return;

            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                if (rowIndex > -1)
                {
                    if (rowIndex >= gvProfile.Rows.Count)
                        rowIndex = 0;
                    gvProfile.SelectedIndex = rowIndex;
                    gv_SelectedIndexChanged(gvProfile);
                    txt_cod_perfil.Text = rowIndex >= 0 ? gvProfile.DataKeys[rowIndex]["cod_perfil"].ToString() : "0";

                    switch (e.CommandName)
                    {
                        case "_Detail":
                            ViewDetail();
                            break;
                        case "_Edit":
                            ViewEdit();
                            break;
                        case "_Delete":
                            if (!ValidateAccess(cnsSection.ADMINISTRACION_PERFIL, cnsAction.ELIMINAR)) return;
                            ViewDetail();
                            MessageBox1.ShowConfirmation("Delete", "¿Está seguro de eliminar el registro?", type: "warning");
                            break;
                        default:
                            return;
                    }
                }
            }
        }
        protected void gvProfile_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvProfile, "Select$" + e.Row.RowIndex.ToString()));
            }
        }
        protected void gvProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            gv_SelectedIndexChanged(gvProfile);
        }
        protected void gvProfile_Sorting(object sender, GridViewSortEventArgs e)
        {
            gv_Sorting(gvProfile, e.SortExpression.ToString(), oVar.prDSPerfiles);
        }
        protected void MessageBox_Accept(string key)
        {
            try
            {
                //string strResult = "";
                switch (key)
                {
                    case "Editar":
                        Edit();
                        break;
                    case "Agregar":
                        Add();
                        break;
                    case "Delete":
                        Delete();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion



        #region Methods
        private void Add()
        {
            string strResult = oPerfiles.sp_i_perfil(txt_nombre_perfil.Text, txt_desc_perfil.Text);

            if (oBasic.AlertUserControl(msgProfile, msgMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name + ".perfil", "i"))
            {
                txt_cod_perfil.Text = (strResult.Split(':'))[1];
                SavePemissions("i");
            }
            else
            {
                oBasic.SPError(msgProfile, msgMain, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResult);
                MessageInfo.ShowMessage("No fue posible actualizar el perfil.", type: "danger");
            }
            upMsgMain.Update();
        }
        private void Delete()
        {
            string strResult = oPerfiles.sp_d_perfil(txt_cod_perfil.Text);

            if (oBasic.AlertUserControl(msgProfile, msgMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "d"))
            {
                oBasic.SPOk(msgMain, msgProfile, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                LoadGrid();
            }
            else
            {
                oBasic.SPError(msgProfile, msgMain, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResult);
                MessageInfo.ShowMessage("No fue posible eliminar el perfil.", type: "danger");
            }
            upMsgMain.Update();
        }
        private void Edit()
        {
            string strResult = oPerfiles.sp_u_perfil(txt_cod_perfil.Text, txt_nombre_perfil.Text, txt_desc_perfil.Text);
            
            if (oBasic.AlertUserControl(msgProfile, msgMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name + ".perfil", "u"))
            {
                SavePemissions("u");
            }
            else
            {
                oBasic.SPError(msgProfile, msgMain, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResult);
                MessageInfo.ShowMessage("No fue posible actualizar el perfil.", type: "danger");
            }
            upMsgMain.Update();
        }
        private bool Enabled()
        {
            return ViewState["Profile.Enabled"].ToString() == "1";
        }
        private void gridSelect()
        {
            int index = Convert.ToInt16(ViewState["IndexProfile"]);
            gvProfile.PageIndex = (index - (index % gvProfile.PageSize)) / gvProfile.PageSize;
            gvProfile.DataSource = ((DataSet)oVar.prDSPerfiles).Tables[0];
            gvProfile.DataBind();
            gvProfile.SelectedIndex = index % gvProfile.PageSize;
        }
        protected void gv_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header)
                return;

            GridView gv = sender as GridView;
            string modulo = gv.ID.Substring(2);

            string sortExpression = (ViewState["SortExp" + modulo] ?? "").ToString().Split(',')[0];
            string sortDirection = (ViewState["SortDir" + modulo] ?? "").ToString().Split(',')[0];
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
            if (gvProfile.SelectedIndex > gvProfile.Rows.Count)
                gvProfile.SelectedIndex = gvProfile.Rows.Count - 1;
            ViewState["Index" + modulo] = ((gv.PageIndex * gv.PageSize) + gv.SelectedIndex).ToString();

            try
            {
                UpdatePanel up = (UpdatePanel)divData.FindControl("up" + modulo + "Foot");
                oBasic.LblRegistros(up, ((DataSet)oVar.prDSPerfiles).Tables[0].Rows.Count, Convert.ToInt32(ViewState["Index" + modulo] ?? "0"));
            }
            catch { }
        }
        private void gv_Sorting(GridView gv, string sortExpression, object ds)
        {
            string modulo = gv.ID.Substring(2);
            string expression = "", direction = "", sort = "";
            int indexselected = gv.SelectedIndex;


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

            gv.DataSource = dataView;
            gv.SelectedIndex = indexselected;
            gv.DataBind();
            gv_SelectedIndexChanged(gv);
            oVar.prDSPerfiles = oUtil.ConvertToDataSet(dataView);
        }
        private void Initialize()
        {
            ViewState["Profile.Enabled"] = "0";
            ViewState["Profile.CriterioBuscar"] = "";
            ViewState["AccionFinal"] = "";

            ViewState["IndexProfile"] = "0";
            ViewState["SortExpProfile"] = "nombre_perfil";
            ViewState["SortDirProfile"] = "ASC";
        }
        private void LoadDetail()
        {
            string cod_perfil = txt_cod_perfil.Text.Trim() == "" ? "0" : txt_cod_perfil.Text;
            oBasic.fClearControls(pnlDetail);
            oBasic.fClearControls(gvPermissions);

            txt_cod_perfil.Text = cod_perfil;
            if (txt_cod_perfil.Text != "0")
            {
                DataSet dSet = oPerfiles.sp_s_perfil(txt_cod_perfil.Text);
                if (dSet != null && dSet.Tables.Count > 0 && dSet.Tables[0].Rows.Count > 0)
                {
                    DataRow dRow = dSet.Tables[0].Rows[0];
                    oBasic.fValueControls(pnlDetail, dRow);
                }
                mvProfile.ActiveViewIndex = 1;
            }
            gvPermissions.DataSource = oPermisos.sp_s_permisos_listar(txt_cod_perfil.Text);
            gvPermissions.DataBind();

            pnlProfileNavegation.Visible = divProfileNavegation.Visible = true;
            oBasic.EnableControls(pnlDetail, Enabled(), true);
            txt_cod_perfil.Enabled = false;
            oBasic.FixPanel(divData, "Profile", Enabled() ? 2 : 1, pList: true);
            gvPermissions.Enabled = Enabled();
            oBasic.EnableCtl(lblProfileCuenta, true, true);
            int Indice = Convert.ToInt16(ViewState["IndexProfile"]);
            oBasic.LblRegistros(upProfileFoot, ((DataSet)oVar.prDSPerfiles).Tables[0].Rows.Count, Indice);
        }
        private void LoadGrid()
        {
            if (!ValidateAccess(cnsSection.ADMINISTRACION_PERFIL, cnsAction.CONSULTAR)) return;
            oVar.prDSPerfiles = oPerfiles.sp_s_perfiles(ViewState["Profile.CriterioBuscar"].ToString());

            gvProfile.DataSource = ((DataSet)(oVar.prDSPerfiles));
            gvProfile.DataBind();

            if (gvProfile.Rows.Count == 0)
            {
                lblProfileCuenta.Text = "";
            }
            else
            {
                gvProfile.SelectedIndex = gvProfile.SelectedIndex == -1 ? 0 : gvProfile.SelectedIndex;
            }
            gv_Sorting(gvProfile, "", oVar.prDSPerfiles);
            gv_SelectedIndexChanged(gvProfile);
            oBasic.FixPanel(divData, "Profile", 0, pAdd: false, pEdit: false, pDelete: false);
            mvProfile.ActiveViewIndex = 0;
            pnlProfileNavegation.Visible = divProfileNavegation.Visible = false;
            upProfile.Update();
            upProfileNavegation.Update();
        }
        private void RegisterScript()
        {
            //Page.ClientScript.RegisterStartupScript(GetType(), Guid.NewGuid().ToString(), "SetCursor('" + txtBuscar.ClientID + "');", true);

            //string key = "validateCodPerfiles";
            //StringBuilder scriptSlider = new StringBuilder();
            //scriptSlider.Append("<script type='text/javascript'> ");
            //scriptSlider.Append("   $('.toggle').click(function () { ");
            //scriptSlider.Append("       var lbl = $(this).children('id*=\"lblMenu\"'); alert(lbl.val());");
            //scriptSlider.Append("       $(this).children('div').each(function(lbl){ ");
            //scriptSlider.Append("           var classes = ['far fa-circle', 'fas fa-dot-circle', 'fas fa-circle']; ");
            //scriptSlider.Append("           this.className = classes[($.inArray(this.className, classes) + 1)% classes.length]; ");
            //scriptSlider.Append("           lbl.val(($.inArray(this.className, classes) + 1)% classes.length); ");
            //scriptSlider.Append("       }); ");
            //scriptSlider.Append("   }); ");
            //scriptSlider.Append(" </script> ");

            //if (!Page.ClientScript.IsStartupScriptRegistered(key))
            //{
            //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), key, scriptSlider.ToString(), false);
            //}
        }
        private void SavePemissions(string tipo)
        {
            string strResult = "";
            string strError = "";
            foreach (GridViewRow dr in gvPermissions.Rows)
            {
                strResult = clConstantes.DB_ACTION_OK;
                string tipo_permiso = gvPermissions.DataKeys[dr.RowIndex]["tipo_permiso"].ToString();
                string objeto_permiso = gvPermissions.DataKeys[dr.RowIndex]["objeto_permiso"].ToString();

                string consultar = gvPermissions.DataKeys[dr.RowIndex]["consultar"].ToString();
                string insertar = gvPermissions.DataKeys[dr.RowIndex]["insertar"].ToString();
                string modificar = gvPermissions.DataKeys[dr.RowIndex]["modificar"].ToString();
                string eliminar = gvPermissions.DataKeys[dr.RowIndex]["eliminar"].ToString();

                string consultar_sel = ((DropDownList)dr.Cells[2].FindControl("ddlRead") ).SelectedValue;
                string insertar_sel = ((DropDownList)dr.Cells[2].FindControl("ddlCreate")).SelectedValue;
                string modificar_sel = ((DropDownList)dr.Cells[2].FindControl("ddlUpdate")).SelectedValue;
                string eliminar_sel=  ((DropDownList)dr.Cells[2].FindControl("ddlDelete")).SelectedValue;

                if (consultar != consultar_sel || insertar != insertar_sel || modificar != modificar_sel || eliminar != eliminar_sel)
                    strResult = oPermisos.sp_iud_permiso(oBasic.fInt(txt_cod_perfil), tipo_permiso, objeto_permiso, 
                                                            consultar_sel, insertar_sel, modificar_sel, eliminar_sel);

                if (strResult.Substring(0, 5) != clConstantes.DB_ACTION_OK)
                    strError = strResult;
            }

            if (strError == "")
            {
                oBasic.SPOk(msgMain, msgProfile, tipo, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                LoadGrid();
            }
            else
            {
                oBasic.SPError(msgProfile, msgMain, tipo, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResult);
                MessageInfo.ShowMessage("Se presentó un problema con el registro de los permisos.", type: "danger");
            }
        }
        private bool ValidateAccess(string section, string action)
        {
            string message = clsPermisos.ValidateAccess(section, action, oVar.prUserCod.ToString(), false, false);

            if (message == "")
                return true;

            MessageInfo.ShowMessage(message);
            return false;
        }
        private void ViewAdd()
        {
            if (!ValidateAccess(cnsSection.ADMINISTRACION_PERFIL, cnsAction.EDITAR)) return;

            ViewState["Profile.Enabled"] = "1";
            txt_cod_perfil.Text = "0";
            LoadDetail();
            txt_cod_perfil.Text = "";
        }
        private void ViewDetail()
        {
            if (!ValidateAccess(cnsSection.ADMINISTRACION_PERFIL, cnsAction.CONSULTAR)) return;

            ViewState["Profile.Enabled"] = "0";
            LoadDetail();
        }
        private void ViewEdit()
        {
            if (!ValidateAccess(cnsSection.ADMINISTRACION_PERFIL, cnsAction.EDITAR)) return;

            ViewState["Profile.Enabled"] = "1";
            LoadDetail();
        }
        #endregion

    }
}