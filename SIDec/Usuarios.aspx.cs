using GLOBAL.DAL;
using GLOBAL.LOG;
using GLOBAL.PERMISOS;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using cnsAction = GLOBAL.CONST.clConstantes.Accion;
using cnsSection = GLOBAL.CONST.clConstantes.Section;

namespace SIDec
{
    public partial class Usuarios : Page
    {
        private readonly USUARIOS_DAL oUsuarios = new USUARIOS_DAL();
        private readonly CARGOS_DAL oCargos = new CARGOS_DAL();
        private readonly PERFILES_DAL oPerfiles = new PERFILES_DAL();
        private readonly IDENTIDADES_DAL oIdentidades = new IDENTIDADES_DAL();

        private readonly clGlobalVar oVar = new clGlobalVar();
        private readonly clUtil oUtil = new clUtil();
        private readonly clLog oLog = new clLog();
        private readonly clPermisos oPermisos = new clPermisos();
        private readonly clBasic oBasic = new clBasic();

        private const string _SOURCEPAGE = "User";

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterScript();

            if (!IsPostBack)
            {
                Initialize();
                LoadDropDowns();
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
            if (txt_cod_usuario.Text != "0" && txt_cod_usuario.Text.Trim() != "")
                MessageBox1.ShowConfirmation("Editar", "¿Está seguro de actualizar la información?", type: "warning");
            else
                MessageBox1.ShowConfirmation("Agregar", "¿Está seguro de continuar con la acción solicitada?");
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ViewState["User.CriterioBuscar"] = txtBuscar.Text.Trim();
            LoadGrid();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LoadGrid();
            gridSelect();
        }
        protected void btnUserAccion_Click(object sender, EventArgs e)
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
                case "Reset":
                    if (!ValidateAccess(cnsSection.ADMINISTRACION_USUARIO, cnsAction.EDITAR)) return;

                    string usuario = txt_usuario.Text.ToLower();
                    MessageBox1.ShowConfirmation("Reset", "¿Está seguro de reiniciar la clave del usuario " + usuario + "?", type: "warning");
                    return;
                default:
                    return;
            }
            //ViewControls();
        }
        protected void btnUserNavegation_Click(object sender, EventArgs e)
        {
            int index = 0;
            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    index = Convert.ToInt16(ViewState["IndexUser"]) - 1;
                    break;
                case "Next":
                    index = Convert.ToInt16(ViewState["IndexUser"]) + 1;
                    break;
                case "Last":
                    index = ((DataSet)oVar.prDSUsuarios).Tables[0].Rows.Count - 1;
                    break;
            }
            gvUser.PageIndex = (index - (index % gvUser.PageSize)) / gvUser.PageSize;
            gvUser.DataSource = ((DataSet)oVar.prDSUsuarios).Tables[0];
            gvUser.DataBind();
            gvUser.SelectedIndex = index % gvUser.PageSize;
            ViewState["IndexUser"] = index.ToString();

            txt_cod_usuario.Text = index >= 0 ? gvUser.DataKeys[gvUser.SelectedIndex]["cod_usuario"].ToString() : "0";

            LoadDetail();
            gv_SelectedIndexChanged(gvUser);
        }
        protected void chk_f_activos_CheckedChanged(object sender, EventArgs e)
        {
            ViewState["User.CriterioBuscar"] = txtBuscar.Text.Trim();
            LoadGrid();
        }
        protected void gvUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUser.PageIndex = e.NewPageIndex;
            LoadGrid();
            if (gvUser.SelectedIndex > gvUser.Rows.Count)
                gvUser.SelectedIndex = gvUser.Rows.Count - 1;
            ViewState["IndexUser"] = ((gvUser.PageSize * gvUser.PageIndex) + gvUser.SelectedIndex);
        }
        protected void gvUser_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            oBasic.AlertMain(msgMain, "", "0");
            upMsgMain.Update();

            if (e.CommandName != "_Detail" && e.CommandName != "_Edit" && e.CommandName != "_Reset")
                return;

            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                if (rowIndex > -1)
                {
                    if (rowIndex >= gvUser.Rows.Count)
                        rowIndex = 0;
                    gvUser.SelectedIndex = rowIndex;
                    gv_SelectedIndexChanged(gvUser);
                    txt_cod_usuario.Text = rowIndex >= 0 ? gvUser.DataKeys[rowIndex]["cod_usuario"].ToString() : "0";

                    switch (e.CommandName)
                    {
                        case "_Detail":
                            ViewDetail();
                            break;
                        case "_Edit":
                            ViewEdit();
                            break;
                        case "_Reset":
                            ViewDetail();
                            string usuario = gvUser.DataKeys[rowIndex]["usuario"].ToString();
                            MessageBox1.ShowConfirmation("Reset", "¿Está seguro de reiniciar la clave del usuario " + usuario.ToLower() + "?", type: "warning");
                            break;
                        default:
                            return;
                    }
                }
            }
        }
        protected void gvUser_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvUser, "Select$" + e.Row.RowIndex.ToString()));
            }
        }
        protected void gvUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            gv_SelectedIndexChanged(gvUser);
        }
        protected void gvUser_Sorting(object sender, GridViewSortEventArgs e)
        {
            gv_Sorting(gvUser, e.SortExpression.ToString(), oVar.prDSUsuarios);
        }
        protected void MessageBox_Accept(string key)
        {
            try
            {
                switch (key)
                {
                    case "Editar":
                        Edit();
                        break;
                    case "Agregar":
                        Add();
                        break;
                    case "Reset":
                        ResetPassword();
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
            string password = oBasic.RandomPassword();
            string strResult = oUsuarios.sp_i_usuario(txt_usuario.Text.ToLower(), txt_nombre_usuario.Text, txt_apellido_usuario.Text,
                                                        oBasic.fInt(ddlb_cod_cargo_usuario), oBasic.fInt(ddlb_id_area_entidad), txt_matricula_usuario.Text,
                                                        txt_correo_usuario.Text, password, chk_habilitado.Checked,
                                                        chk_asigna_usuario_predios.Checked, chk_edita_actos.Checked, chk_edita_documentos.Checked,
                                                        chk_elimina_documentos.Checked, chk_recibe_prestamos.Checked, chk_revisa_gestion.Checked);

            if (oBasic.AlertUserControl(msgUser, msgMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name + ".usuario", "i"))
            {
                string idsPerfiles = "";
                txt_cod_usuario.Text = (strResult.Split(':'))[1];
                foreach (ListItem item in cbl_cod_perfil.Items)
                {
                    if (item.Selected)
                        idsPerfiles += "," + item.Value;
                }
                strResult = oUsuarios.sp_iu_usuarioperfil(oBasic.fInt(txt_cod_usuario), idsPerfiles);
                if (oBasic.AlertUserControl(msgUser, msgMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name + ".Perfiles", "i"))
                {
                    oBasic.SPOk(msgMain, msgUser, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name); 
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), " <script type='text/javascript'>  navigator.clipboard.writeText('" + password + "');  </script> ", false);
                    MessageInfo.ShowMessage("El usuario fue creado correctamente. <br/> «" + password + "» copiado en el portapapeles.");
                    LoadGrid();
                }
                else
                {
                    oBasic.SPError(msgUser, msgMain, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResult);
                    MessageInfo.ShowMessage("No fue posible actualizar el perfil del usuario.", type: "danger");
                }
            }
            else
            {
                oBasic.SPError(msgUser, msgMain, "i", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResult);
                MessageInfo.ShowMessage("No fue posible actualizar el perfil del usuario.", type: "danger");
            }
            upMsgMain.Update();
        }
        private void Edit()
        {
            string strResult = oUsuarios.sp_u_usuario(oBasic.fInt2(txt_cod_usuario), txt_usuario.Text.ToLower(), txt_nombre_usuario.Text,
                                                        txt_apellido_usuario.Text, oBasic.fInt(ddlb_cod_cargo_usuario), oBasic.fInt(ddlb_id_area_entidad),
                                                        txt_matricula_usuario.Text, txt_correo_usuario.Text, chk_habilitado.Checked,
                                                        chk_asigna_usuario_predios.Checked, chk_edita_actos.Checked, chk_edita_documentos.Checked,
                                                        chk_elimina_documentos.Checked, chk_recibe_prestamos.Checked, chk_revisa_gestion.Checked);

            if (oBasic.AlertUserControl(msgUser, msgMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name + ".usuario", "u"))
            {
                string idsPerfiles = "";
                foreach (ListItem item in cbl_cod_perfil.Items)
                {
                    if (item.Selected)
                        idsPerfiles += "," + item.Value;
                }
                strResult = oUsuarios.sp_iu_usuarioperfil(oBasic.fInt(txt_cod_usuario), idsPerfiles);
                if (oBasic.AlertUserControl(msgUser, msgMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name + ".Perfiles", "u"))
                {
                    oBasic.SPOk(msgMain, msgUser, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                    LoadGrid();
                }
                else
                {
                    oBasic.SPError(msgUser, msgMain, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResult);
                    MessageInfo.ShowMessage("No fue posible actualizar el perfil del usuario.", type: "danger");
                }
            }
            else
            {
                oBasic.SPError(msgUser, msgMain, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResult);
                MessageInfo.ShowMessage("No fue posible actualizar el perfil del usuario.", type: "danger");
            }
            upMsgMain.Update();
        }
        private bool Enabled()
        {
            return ViewState["User.Enabled"].ToString() == "1";
        }
        private void gridSelect()
        {
            int index = Convert.ToInt16(ViewState["IndexUser"]);
            gvUser.PageIndex = (index - (index % gvUser.PageSize)) / gvUser.PageSize;
            gvUser.DataSource = ((DataSet)oVar.prDSUsuarios).Tables[0];
            gvUser.DataBind();
            gvUser.SelectedIndex = index % gvUser.PageSize;
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
            if (gvUser.SelectedIndex > gvUser.Rows.Count)
                gvUser.SelectedIndex = gvUser.Rows.Count - 1;
            ViewState["Index" + modulo] = ((gv.PageIndex * gv.PageSize) + gv.SelectedIndex).ToString();

            try
            {
                UpdatePanel up = (UpdatePanel)divData.FindControl("up" + modulo + "Foot");
                oBasic.LblRegistros(up, ((DataSet)oVar.prDSUsuarios).Tables[0].Rows.Count, Convert.ToInt32(ViewState["Index" + modulo] ?? "0"));
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
            oVar.prDSUsuarios = oUtil.ConvertToDataSet(dataView);
        }
        private void Initialize()
        {
            ViewState["User.Enabled"] = "0";
            ViewState["User.CriterioBuscar"] = "";
            ViewState["AccionFinal"] = "";

            ViewState["IndexUser"] = "0";
            ViewState["SortExpUser"] = "usuario";
            ViewState["SortDirUser"] = "ASC";
        }
        private void LoadDetail()
        {
            string cod_usuario = txt_cod_usuario.Text.Trim() == "" ? "0" : txt_cod_usuario.Text;
            oBasic.fClearControls(pnlDetail);

            txt_cod_usuario.Text = cod_usuario;

            cbl_cod_perfil.ClearSelection();
            if (txt_cod_usuario.Text != "0")
            {
                DataSet dSet = oUsuarios.sp_s_usuario(oBasic.fInt2(txt_cod_usuario));
                if (dSet != null && dSet.Tables.Count > 0 && dSet.Tables[0].Rows.Count > 0)
                {
                    DataRow dRow = dSet.Tables[0].Rows[0];
                    oBasic.fValueControls(pnlDetail, dRow);
                    string idperfiles = dRow["idsperfil"].ToString();
                    foreach (string idperfil in idperfiles.Split(','))
                    {
                        foreach (ListItem item in cbl_cod_perfil.Items)
                        {
                            if (item.Value == idperfil)
                                item.Selected = true;
                        }
                    }
                    mvUser.ActiveViewIndex = 1;
                }
            }

            pnlUserNavegation.Visible = divUserNavegation.Visible = true;
            oBasic.EnableControls(pnlDetail, Enabled(), true);
            txt_cod_usuario.Enabled = false;
            oBasic.FixPanel(divData, "User", Enabled() ? 2 : 1, pList: true);
            oBasic.EnableCtl(lblUserCuenta, true, true);
            int Indice = Convert.ToInt16(ViewState["IndexUser"]);
            oBasic.LblRegistros(upUserFoot, ((DataSet)oVar.prDSUsuarios).Tables[0].Rows.Count, Indice);
        }
        private void LoadDropDowns()
        {
            ddlb_cod_cargo_usuario.DataSource = oCargos.sp_s_cargos();
            ddlb_cod_cargo_usuario.DataTextField = "nombre_cargo";
            ddlb_cod_cargo_usuario.DataValueField = "au_cargo";
            ddlb_cod_cargo_usuario.DataBind();

            cbl_cod_perfil.DataSource = oPerfiles.sp_s_perfiles();
            cbl_cod_perfil.DataTextField = "nombre_perfil";
            cbl_cod_perfil.DataValueField = "cod_perfil";
            cbl_cod_perfil.DataBind();

            ddlb_id_area_entidad.DataSource = oIdentidades.sp_s_identidad_id_categoria_op("14", "1");
            ddlb_id_area_entidad.DataTextField = "nombre_identidad";
            ddlb_id_area_entidad.DataValueField = "id_identidad";
            ddlb_id_area_entidad.DataBind();
        }
        private void LoadGrid()
        {
            if (!ValidateAccess(cnsSection.ADMINISTRACION_USUARIO, cnsAction.CONSULTAR)) return;
            oVar.prDSUsuarios = oUsuarios.sp_s_usuarios((chk_f_activos.Checked ? 1 : 0), ViewState["User.CriterioBuscar"].ToString());

            gvUser.DataSource = ((DataSet)(oVar.prDSUsuarios));
            gvUser.DataBind();

            if (gvUser.Rows.Count == 0)
            {
                lblUserCuenta.Text = "";
            }
            else
            {
                gvUser.SelectedIndex = gvUser.SelectedIndex == -1 ? 0 : gvUser.SelectedIndex;
            }
            gv_Sorting(gvUser, "", oVar.prDSUsuarios);
            gv_SelectedIndexChanged(gvUser);
            oBasic.FixPanel(divData, "User", 0, pAdd: false, pEdit: false, pDelete: false);
            mvUser.ActiveViewIndex = 0;
            pnlUserNavegation.Visible = divUserNavegation.Visible = false;
            upUser.Update();
            upUserNavegation.Update();
        }
        private void RegisterScript()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), Guid.NewGuid().ToString(), "SetCursor('" + txtBuscar.ClientID + "');", true);

            string key = "validateCodPerfiles";
            StringBuilder scriptSlider = new StringBuilder();
            scriptSlider.Append("<script type='text/javascript'> ");
            scriptSlider.Append("   function ValidateCodPerfil(source, args)  ");
            scriptSlider.Append("   {   var chkListModules = document.getElementById('" + cbl_cod_perfil.ClientID + "'); ");
            scriptSlider.Append("       var chkListinputs = chkListModules.getElementsByTagName('input'); ");
            scriptSlider.Append("       for (var i = 0; i < chkListinputs.length; i++) ");
            scriptSlider.Append("       {   if (chkListinputs[i].checked)  ");
            scriptSlider.Append("           {   args.IsValid = true; ");
            scriptSlider.Append("               return;}    } ");
            scriptSlider.Append("           args.IsValid = false; ");
            scriptSlider.Append("   } ");
            scriptSlider.Append(" </script> ");

            if (!Page.ClientScript.IsStartupScriptRegistered(key))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), key, scriptSlider.ToString(), false);
            }
        }
        protected void ResetPassword()
        {
            string password = oBasic.RandomPassword();

            string strResult = oUsuarios.sp_u_usuario_resetpassword(oBasic.fInt2(txt_cod_usuario), password);
            if (oBasic.AlertUserControl(msgUser, msgMain, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "i"))
            {
                LoadGrid();
                oBasic.SPOk(msgMain, msgUser, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), " <script type='text/javascript'>  navigator.clipboard.writeText('" + password + "');  </script> ", false);
                MessageInfo.ShowMessage("El password del usuario fue reinicidado. <br/> «" + password + "» copiada en el portapapeles.");
            }
            else
            {
                oBasic.SPError(msgUser, msgMain, "u", _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, strResult);
                MessageInfo.ShowMessage("No fue posible reiniciar el password del usuario.", type: "danger");
            }
        }
        private bool ValidateAccess(string section, string action)
        {
            string message = oPermisos.ValidateAccess(section, action, oVar.prUserCod.ToString(), false, false);

            if (message == "")
                return true;

            MessageInfo.ShowMessage(message);
            return false;
        }
        private void ViewAdd()
        {
            if (!ValidateAccess(cnsSection.ADMINISTRACION_USUARIO, cnsAction.EDITAR)) return;

            ViewState["User.Enabled"] = "1";
            txt_cod_usuario.Text = "0";
            LoadDetail();
            txt_cod_usuario.Text = "";
        }
        private void ViewDetail()
        {
            if (!ValidateAccess(cnsSection.ADMINISTRACION_USUARIO, cnsAction.CONSULTAR)) return;

            ViewState["User.Enabled"] = "0";
            LoadDetail();
        }
        private void ViewEdit()
        {
            if (!ValidateAccess(cnsSection.ADMINISTRACION_USUARIO, cnsAction.EDITAR)) return;

            ViewState["User.Enabled"] = "1";
            LoadDetail();
        }
        #endregion

    }
}