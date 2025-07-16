using AjaxControlToolkit;
using GLOBAL.CONST;
using GLOBAL.DAL;
using GLOBAL.LOG;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Image = System.Web.UI.WebControls.Image;

namespace SIDec
{
    public class clBasic 
    {
        private readonly USUARIOS_DAL oUsuarios = new USUARIOS_DAL();

        private readonly clLog oLog = new clLog();
        private readonly clGlobalVar oVar = new clGlobalVar();
        private readonly clUtil oUtil = new clUtil();
        private const string _SOURCEPAGE = "Basic";

        #region-------------------------------------------------------------------- conversiones
        public string fPerc(WebControl aspControl)
        {
            if (aspControl.GetType().Name == "TextBox")
            {
                TextBox ctlT = aspControl as TextBox;
                try
                {
                    return "(per)" + (Convert.ToDecimal(ctlT.Text.Replace("%", "").Trim()) / 100).ToString();
                }
                catch
                {
                    return null;
                }
            }
            else
                return null;
        }
        public string fDateTime(WebControl aspControl)
        {
            if (aspControl.GetType().Name == "TextBox")
            {
                TextBox ctlT = aspControl as TextBox;
                try
                {
                    return "" + DateTime.Parse(ctlT.Text).ToString("yyyy/MM/dd HH:mm:ss");
                }
                catch
                {
                    return null;
                }
            }
            else
                return null;
        }
        public string fDateTime(string value)
        {
                try
                {
                    return "" + DateTime.Parse(value).ToString("yyyy/MM/dd HH:mm:ss");
                }
                catch
                {
                    return null;
                }
        }
        public string fDec(WebControl aspControl)
        {
            if (aspControl.GetType().Name == "TextBox")
            {
                TextBox ctlT = aspControl as TextBox;
                try
                {
                    return "(dec)" + decimal.Parse(ctlT.Text).ToString();
                }
                catch
                {
                    return null;
                }
            }
            else
                return null;
        }
        public string fDec(string value)
        {
            try
            {
                return "(dec)" + decimal.Parse(value).ToString();
            }
            catch
            {
                return null;
            }
        }
        public string fInt(WebControl aspControl)
        {
            if (aspControl.GetType().Name == "TextBox")
            {
                TextBox ctlT = aspControl as TextBox;
                try
                {
                    return "(int)" + int.Parse(ctlT.Text, NumberStyles.AllowThousands).ToString();
                }
                catch
                {
                    return null;
                }
            }
            else if (aspControl.GetType().Name == "DropDownList")
            {
                DropDownList ddlb = aspControl as DropDownList;
                try
                {
                    return "(int)" + int.Parse(ddlb.SelectedValue).ToString();
                }
                catch
                {
                    return null;
                }
            }
            else
                return null;
        }
        public string fInt(HiddenField ctlT)
        {
            try
            {
                return "(int)" + int.Parse(ctlT.Value, NumberStyles.AllowThousands).ToString();
            }
            catch
            {
                return null;
            }
        }
        public string fInt2(WebControl aspControl)
        {
            if (aspControl.GetType().Name == "TextBox")
            {
                TextBox ctlT = aspControl as TextBox;
                try
                {
                    return int.Parse(ctlT.Text, NumberStyles.AllowThousands).ToString();
                }
                catch
                {
                    return null;
                }
            }
            else if (aspControl.GetType().Name == "DropDownList")
            {
                DropDownList ddlb = aspControl as DropDownList;
                try
                {
                    return int.Parse(ddlb.SelectedValue).ToString();
                }
                catch
                {
                    return null;
                }
            }
            else
                return null;
        }
        #endregion

        #region-------------------------------------------------------------------- estilos controles
        //oculta/muestra (V) o habilita/deshabilita (E) controles web y label y elimina (true or false) valores cuando esta deshabilitado
        public void StyleCtlLbl(string type, bool bHabilitado, WebControl aspControl, Label lblControl, bool bEliminar)
        {
            if (type == "V")
            {
                lblControl.Visible = bHabilitado;
                aspControl.Visible = bHabilitado;
                if (aspControl.GetType().Name == "TextBox")
                {
                    TextBox ctlT = aspControl as TextBox;
                    if (bEliminar)
                        ctlT.Text = "";
                }
                else if (aspControl.GetType().Name == "DropDownList")
                {
                    DropDownList ctlD = aspControl as DropDownList;
                    if (bEliminar)
                        ctlD.SelectedIndex = -1;
                }
                else if (aspControl.GetType().Name == "CheckBox")
                {
                    CheckBox ctlC = aspControl as CheckBox;
                    ctlC.Checked = false;
                }
            }
            else if (type == "E")
            {
                lblControl.Enabled = bHabilitado;
                aspControl.Enabled = bHabilitado;
                if (bHabilitado)
                {
                    lblControl.CssClass = lblControl.CssClass.Replace(" lblDis", "");
                    aspControl.CssClass = aspControl.CssClass.Replace(" txtDis", "");
                }
                else
                {
                    lblControl.CssClass = lblControl.CssClass.Insert(lblControl.CssClass.Length, " lblDis");
                    aspControl.CssClass = aspControl.CssClass.Insert(aspControl.CssClass.Length, " txtDis");
                    if (aspControl.GetType().Name == "TextBox")
                    {
                        TextBox ctlT = aspControl as TextBox;
                        if (bEliminar)
                            ctlT.Text = "";
                    }
                    else if (aspControl.GetType().Name == "DropDownList")
                    {
                        DropDownList ctlD = aspControl as DropDownList;
                        if (bEliminar)
                            ctlD.SelectedIndex = -1;
                    }
                    else if (aspControl.GetType().Name == "CheckBox")
                    {
                        CheckBox ctlC = aspControl as CheckBox;
                        if (bEliminar)
                            ctlC.Checked = false;
                    }
                }
            }
        }
        public void StyleCtl(string type, bool bHabilitado, WebControl aspControl, bool bEliminar)
        {
            if (type == "V")
            {
                aspControl.Visible = bHabilitado;
                if (aspControl.GetType().Name == "TextBox")
                {
                    TextBox ctlT = aspControl as TextBox;
                    if (bEliminar)
                        ctlT.Text = "";
                }
                else if (aspControl.GetType().Name == "DropDownList")
                {
                    DropDownList ctlD = aspControl as DropDownList;
                    if (bEliminar)
                        ctlD.SelectedIndex = -1;
                }
            }
            else if (type == "E")
            {
                aspControl.Enabled = bHabilitado;
                if (bHabilitado)
                {
                    aspControl.CssClass = aspControl.CssClass.Replace(" txtDis", "");
                }
                else
                {
                    aspControl.CssClass = aspControl.CssClass.Insert(aspControl.CssClass.Length, " txtDis");
                    if (aspControl.GetType().Name == "TextBox")
                    {
                        TextBox ctlT = aspControl as TextBox;
                        if (bEliminar)
                            ctlT.Text = "";
                    }
                    else if (aspControl.GetType().Name == "DropDownList")
                    {
                        DropDownList ctlD = aspControl as DropDownList;
                        if (bEliminar)
                            ctlD.SelectedIndex = -1;
                    }
                    else if (aspControl.GetType().Name == "CheckBox")
                    {
                        CheckBox ctlC = aspControl as CheckBox;
                        if (bEliminar)
                            ctlC.Checked = false;
                    }
                }
            }
        }
        public void StyleButton(LinkButton lb)
        {
            if (lb.Enabled)
            {
                lb.Attributes.Remove("disable");
                ClassRemove(lb, "disabled");
            }
            else
            {
                lb.Attributes.Add("disabled", null);
                ClassAdd(lb, "disabled");
            }
        }
        public void ActiveNav(MultiView mv, LinkButton lbOld, LinkButton lbNew, int newIndex)
        {
            ClassRemove(lbOld, "active");
            ClassAdd(lbNew, "active");
            mv.ActiveViewIndex = newIndex;
        }
        public void ClassSet(Object obj, int option, string cls = "") // 1:add 2:remove 3:delete
        {
            WebControl ctl = null;
            HtmlGenericControl hctl = null;
            UpdatePanel up = null;
            if (obj is WebControl control)
                ctl = control;
            else if (obj is UpdatePanel panel)
                up = panel;
            else if (obj is HtmlGenericControl hgcontrol) 
                hctl = hgcontrol;
            else
                return;

            if (ctl != null)
            {
                if (option == 1)
                {
                    ClassAdd(ctl, cls);
                }
                else if (option == 2)
                {
                    ClassRemove(ctl, cls);
                }
                else if (option == 3)
                {
                    if (!string.IsNullOrEmpty(cls))
                        ctl.CssClass = cls;
                    else
                        ctl.CssClass = null;
                }
            }
            else if (up != null)
            {
                if (option == 1)
                {
                    up.Attributes["class"] = (up.Attributes["class"] + " " + cls).Trim();
                }
                else if (option == 2)
                {
                    up.Attributes["class"] = fRemoveString(up.Attributes["class"], cls).Trim();
                }
                else if (option == 3)
                {
                    up.Attributes.Remove("class");
                    if (!string.IsNullOrEmpty(cls))
                        up.Attributes["class"] = cls.Trim();
                }
            }
            else if (hctl != null)
            {
                if (option == 1)
                {
                    hctl.Attributes["class"] = (hctl.Attributes["class"] + " " + cls).Trim();
                }
                else if (option == 2)
                {
                    hctl.Attributes["class"] = fRemoveString(hctl.Attributes["class"], cls).Trim();
                }
                else if (option == 3)
                {
                    hctl.Attributes.Remove("class");
                    if (!string.IsNullOrEmpty(cls))
                        hctl.Attributes["class"] = cls.Trim();
                }
            }
        }
        public void ClassAdd(WebControl ctl, string cls)
        {
            ClassRemove(ctl, cls);
            if (ctl.CssClass.Length == 0)
                ctl.CssClass = cls;
            else
                //ctl.Attributes.Add("class", cls);
                ctl.CssClass = ctl.CssClass.Insert(ctl.CssClass.Length, " " + cls).Trim();

        }
        public void ClassRemove(WebControl ctl, string cls)
        {
            ctl.CssClass = fRemoveString(ctl.CssClass, cls);
        }
        public void EnablePanel(Panel panel, bool enabled, bool visible)
        {
            if (panel != null)
            {
                foreach (Control ctl in panel.Controls)
                {
                    if (ctl is LinkButton || ctl is TextBox || ctl is DropDownList || ctl is CheckBox || ctl is Panel || ctl is CheckBoxList)
                    {
                        WebControl wctl = (WebControl)ctl;
                        wctl.Enabled = enabled;
                        VisibleCtl(wctl, visible);
                    }
                }
                panel.Enabled = enabled;
                VisibleCtl(panel, visible);
            }
        }
        public void EnableControls(Control control, bool enabled, bool visible)
        {
            if (control != null)
            {
                foreach (Control ctl in control.Controls)
                {
                    if (ctl.Controls.Count > 0)
                        EnableControls(ctl, enabled, visible);
                    if (ctl is LinkButton || ctl is TextBox || ctl is DropDownList || ctl is CheckBox || ctl is Panel || ctl is CheckBoxList)
                    {
                        WebControl wctl = (WebControl)ctl;
                        wctl.Enabled = enabled;
                        VisibleCtl(wctl, visible);
                    }
                }
            }
        }
        public void EnableCtl(WebControl ctl, bool enabled, bool visible = true)
        {
            if (ctl == null)
                return;
            ctl.Enabled = enabled;
            if (enabled)
                ClassRemove(ctl, "disabled");
            else
                ClassAdd(ctl, "disabled");
            VisibleCtl(ctl, visible);
        }
        public void EnableButton(bool Habilitado, LinkButton lb)
        {
            if (lb != null)
            {
                if (Habilitado)
                    lb.Enabled = true;
                else
                    lb.Enabled = false;
                StyleButton(lb);
            }
        }
        public void VisibleCtl(WebControl ctl, bool visible)
        {
            if (visible)
            {
                ctl.Style.Remove("visibility");
                ClassRemove(ctl, "d-none");
            }
            else
            {
                ctl.Style.Add("visibility", "hidden");
                ClassAdd(ctl, "d-none");
            }
        }
        public void LblRegistros(UpdatePanel up, int rows, int index)
        {
            string modulo = up.ID.ToString().Replace("up", "").Replace("Foot", "");
            Label lbl = oUtil.FindControlRecursive(up, "lbl" + modulo + "Cuenta") as Label;
            LinkButton lb1 = oUtil.FindControlRecursive(up, "btn" + modulo + "NavFirst") as LinkButton;
            LinkButton lb2 = oUtil.FindControlRecursive(up, "btn" + modulo + "NavBack") as LinkButton;
            LinkButton lb3 = oUtil.FindControlRecursive(up, "btn" + modulo + "NavNext") as LinkButton;
            LinkButton lb4 = oUtil.FindControlRecursive(up, "btn" + modulo + "NavLast") as LinkButton;

            if (rows > 0)
                lbl.Text = string.Format(clConstantes.MSG_CONTADOR, (index + 1).ToString(), rows.ToString());
            else
            {
                lbl.Text = string.Format(clConstantes.MSG_CONTADOR, "0", "0");
                EnableCtl(lb1, false);
                EnableCtl(lb2, false);
                EnableCtl(lb3, false);
                EnableCtl(lb4, false);
            }

            if (rows == 1)
            {
                EnableCtl(lb1, false);
                EnableCtl(lb2, false);
                EnableCtl(lb3, false);
                EnableCtl(lb4, false);
            }
            else if (index + 1 >= rows)
            {
                EnableCtl(lb1, true);
                EnableCtl(lb2, true);
                EnableCtl(lb3, false);
                EnableCtl(lb4, false);
            }
            else if (index - 1 < 0)
            {
                EnableCtl(lb1, false);
                EnableCtl(lb2, false);
                EnableCtl(lb3, true);
                EnableCtl(lb4, true);
            }
            else
            {
                EnableCtl(lb1, true);
                EnableCtl(lb2, true);
                EnableCtl(lb3, true);
                EnableCtl(lb4, true);
            }
            up.Update();
        }
        public void FixPanel(HtmlControl divData, string modulo, int option, bool pList = false, bool pAdd = true, bool pEdit = true, bool pDelete = true)
        {
            UpdatePanel up = (UpdatePanel)divData.FindControl("up" + modulo);
            if (up == null) return;

            HtmlGenericControl div = (HtmlGenericControl)divData.FindControl("msg" + modulo);
            div = div ?? new HtmlGenericControl();
            Panel pView = (Panel)up.FindControl("p" + modulo + "View");
            pView = pView ?? new Panel();
            Panel pNavegacion = (Panel)up.FindControl("p" + modulo + "Navegacion");
            pNavegacion = pNavegacion ?? new Panel();
            Panel pExecAction = (Panel)up.FindControl("p" + modulo + "ExecAction");
            pExecAction = pExecAction ?? new Panel();
            Panel pAction = (Panel)up.FindControl("p" + modulo + "Action");
            pAction = pAction ?? new Panel();
            Label lblCuenta = (Label)up.FindControl("lbl" + modulo + "Cuenta");
            lblCuenta = lblCuenta ?? new Label();
            LinkButton lbVG = (LinkButton)up.FindControl("btn" + modulo + "VG");
            lbVG = lbVG ?? new LinkButton();
            LinkButton lbVD = (LinkButton)up.FindControl("btn" + modulo + "VD");
            lbVD = lbVD ?? new LinkButton();
            MultiView mv = (MultiView)up.FindControl("mv" + modulo);
            mv = mv ?? new MultiView();
            GridView gv = (GridView)up.FindControl("gv" + modulo);
            View v = (View)up.FindControl("v" + modulo);
            v = v ?? new View();


            LinkButton lbList = (LinkButton)pAction.FindControl("btn" + modulo + "List");
            lbList = lbList ?? new LinkButton();
            lbList.Visible = pList;
            LinkButton lbAdd = (LinkButton)pAction.FindControl("btn" + modulo + "Add");
            lbAdd = lbAdd ?? new LinkButton();
            lbAdd.Visible = pAdd;
            LinkButton lbEdit = (LinkButton)pAction.FindControl("btn" + modulo + "Edit");
            lbEdit = lbEdit ?? new LinkButton();
            lbEdit.Visible = pEdit;
            LinkButton lbDel = (LinkButton)pAction.FindControl("btn" + modulo + "Del");
            lbDel = lbDel ?? new LinkButton();
            lbDel.Visible = pDelete;
            bool hasActions = pList || pAdd || pEdit || pDelete;

            switch (option)
            {
                case 0: //vista tabla
                    mv.ActiveViewIndex = 0;
                    AlertSection(div, "", "0");
                    EnablePanel(pView, true, true);
                    ToggleActive(lbVG);
                    EnableCtl(lblCuenta, true, true);
                    EnablePanel(pNavegacion, false, false);
                    EnablePanel(pExecAction, false, false);
                    EnablePanel(pAction, true, hasActions);
                    break;
                case 1: //vista form
                    mv.ActiveViewIndex = 1;
                    EnablePanel(pView, true, true);
                    ToggleActive(lbVD);
                    EnableCtl(lblCuenta, true, true);
                    EnablePanel(pNavegacion, true, true);
                    EnablePanel(pExecAction, false, false); 
                    EnablePanel(pAction, true, hasActions);
                    break;
                case 2: //edición
                    mv.ActiveViewIndex = 1;
                    EnablePanel(pView, false, false);
                    ToggleActive(lbVD);
                    EnableCtl(lblCuenta, false, false);
                    EnablePanel(pNavegacion, false, false);
                    EnablePanel(pExecAction, true, true);
                    EnablePanel(pAction, false, false);
                    break;
                case 3: //no data
                    mv.ActiveViewIndex = 0;
                    AlertSection(div, "", "0");
                    EnablePanel(pView, false, false);
                    ToggleActive(lbVG);
                    EnableCtl(lblCuenta, false, false);
                    EnablePanel(pNavegacion, false, false);
                    EnablePanel(pExecAction, false, false);
                    EnablePanel(pAction, false, false);
                    break;

            }
            if (gv==null || gv.Rows.Count == 0)
                AddControl(v, "div", clConstantes.TABLE_NO_RECORDS, "alert alert-warning m-1");
            if (up.UpdateMode == UpdatePanelUpdateMode.Conditional)
                up.Update();
        }
        public void ToggleActive(LinkButton lb)
        {
            if (lb != null && lb.Parent != null)
            {
                foreach (Control ctl in lb.Parent.Controls)
                {
                    if (ctl is LinkButton)
                    {
                        WebControl wctl = (WebControl)ctl;
                        if (wctl == lb)
                            ClassAdd(lb, "active");
                        else
                            ClassRemove(wctl, "active");
                    }
                }
            }
        }
        public void AddControl(object obj, string type, string text, string cls = "")
        {
            WebControl ctl = null;
            HtmlGenericControl hctl = null;
            UpdatePanel up = null;
            View view = null;

            if (obj is WebControl control)
                ctl = control;
            else if (obj is UpdatePanel panel)
                up = panel;
            else if (obj is View _view)
                view = _view;
            else if (obj is HtmlGenericControl hgcontrol)
                hctl = hgcontrol;
            else
                return;

            HtmlGenericControl div = new HtmlGenericControl(type);
            if (ctl != null)
                ctl.Controls.Add(div);
            else if (up != null)
                up.Controls.Add(div);
            else if (view != null)
                view.Controls.Add(div);
            else if (hctl != null)
                hctl.Controls.Add(div);

            div.InnerText = text;
            if (cls != "")
                ClassSet(div, 1, cls);
        }
        #endregion

        #region-------------------------------------------------------------------- alerts
        public bool AlertUserControl(HtmlGenericControl divSection, HtmlGenericControl divSectionMain, string strResult, string strPage, string strSection, string typeAction)
        {
            if (strResult.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                string strMsgOk = GetMessageOk(typeAction);
                oLog.RegistrarLogInfo(strPage, strSection, strMsgOk);
                if (divSectionMain != null) AlertMain(divSectionMain, strMsgOk, "success");
                if (divSection != null) AlertSection(divSection, null, "0"); 

                return true;
            }
            else
            {
                SPError(divSectionMain, divSection, typeAction, strPage, strSection, strResult);
                return false;
            }
        }
        public void AlertMain(HtmlGenericControl div, string msg, string type) //UpdatePanel up
        {
            HtmlGenericControl span = (HtmlGenericControl)div.FindControl("msgMainText");
            ClassSet(div, 3);
            if (type == "0")
            {
                span.InnerText = null;
                ClassSet(div, 1, "d-none");
            }
            else
            {
                span.InnerText = msg;
                ClassSet(div, 1, "alert alert-" + type + " alert-dismissible fade show p-2");
            }
            //up.Update();
        }
        public void AlertSection(HtmlGenericControl div, string msg, string type)
        {
            if (type == "0")
            {
                div.InnerText = null;
                ClassSet(div, 1, "d-none");
            }
            else
            {
                div.InnerText = msg;
                ClassSet(div, 3);
                ClassSet(div, 1, "alert alert-" + type + " alert-dismissible fade show");
            }
        }
        #endregion

        #region-------------------------------------------------------------------- SP execution
        private string GetMessageOk(string type)
        {
            switch (type)
            {
                case "i":
                    return clConstantes.MSG_OK_I;
                case "u":
                    return clConstantes.MSG_OK_U;
                case "d":
                    return clConstantes.MSG_OK_D;
                case "x":
                    return clConstantes.MSG_OK_FILE_NO_DATA;
                case "":
                    return clConstantes.MSG_SIN_CAMBIOS;
                case "fl-i":
                    return clConstantes.MSG_OK_FILELOAD_I;
                case "fl-u":
                    return clConstantes.MSG_OK_FILELOAD_U;
                case "fl-d":
                    return clConstantes.MSG_OK_FILELOAD_D;
            }
            return "";
        }
        private string GetMessageError(string type)
        {
            switch (type)
            {
                case "i":
                    return clConstantes.MSG_ERR_I;
                case "u":
                    return clConstantes.MSG_ERR_U;
                case "d":
                    return clConstantes.MSG_ERR_D;
                case "fl-i":
                    return clConstantes.MSG_ERR_FILELOAD_I;
                case "fl-u":
                    return clConstantes.MSG_ERR_FILELOAD_U;
                case "fl-d":
                    return clConstantes.MSG_ERR_FILELOAD_D;
            }
            return "";
        }
        public void SPOk(HtmlGenericControl msgMain, HtmlGenericControl msgSection, string type, string page, string module)
        {
            string c = GetMessageOk(type);

            if (type == "")
            {
                AlertMain(msgMain, c, "info");
            }
            else
            {
                oLog.RegistrarLogInfo(page, module, c);
                AlertMain(msgMain, c, "success");
            }
            if (msgSection != null)
                AlertSection(msgSection, null, "0");
        }
        public void SPError(HtmlGenericControl msgMain, HtmlGenericControl msgSection, string type, string page, string module, string error)
        {
            string c = GetMessageError(type);
            oLog.RegistrarLogError(error, page, module);
            if (msgMain != null) AlertMain(msgMain, c, "danger");
            if (msgSection != null) AlertSection(msgSection, null, "0");

        }
        #endregion

        #region-------------------------------------------------------------------- folders and files
        public bool fFolderExists(string path)
        {
            if (Directory.Exists(path))
                return true;
            else
                return false;
        }
        public void FileDelete(string path_file)
        {
            try
            {
                File.Move(path_file, path_file.Substring(0, path_file.Length - 4) + "_" + (DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf"));
            }
            catch
            { }
        }
        public string fFolderPath(string file_path)
        {
            string folder_path;
            try
            {
                folder_path = System.IO.Path.GetDirectoryName(file_path);
            }
            catch
            {
                folder_path = "";
            }
            return folder_path;
        }
        public void LoadPdf(FileUpload file_upload, string file_pdf)
        {
            if (file_upload.HasFile)
            {
                if (File.Exists(file_pdf))
                    File.Move(file_pdf, file_pdf.Substring(0, file_pdf.Length - 4) + "_" + (DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf"));

                if (System.IO.Path.GetExtension(file_upload.FileName).ToLower() == ".pdf" && file_upload.PostedFile.ContentLength <= clConstantes.MAX_PDF_SIZE)
                {
                    if (!fFolderExists(fFolderPath(file_pdf)))
                    {
                        try
                        {
                            System.IO.Directory.CreateDirectory(fFolderPath(file_pdf));
                        }
                        catch (Exception MyError)
                        {
                            oLog.RegistrarLogError(MyError, _SOURCEPAGE, "LoadPdf");
                            oVar.prError = clConstantes.FILE_ERR_LOAD;
                        }
                    }


                    try
                    {
                        file_upload.PostedFile.SaveAs(file_pdf);
                        oVar.prError = "0";
                    }
                    catch (Exception MyError)
                    {
                        oLog.RegistrarLogError(MyError, _SOURCEPAGE, "LoadPdf");
                        oVar.prError = clConstantes.FILE_ERR_LOAD;
                    }
                }
                else
                {
                    oLog.RegistrarLogError("Error subiendo Documento" + System.IO.Path.GetExtension(file_upload.FileName) + ":::" + file_upload.PostedFile.ContentLength, _SOURCEPAGE, "LoadPdf");
                    oVar.prError = clConstantes.FILE_NO_PDF;
                }
            }
            else
            {
                oVar.prError = "0";
            }
        }
        #endregion

        #region-------------------------------------------------------------------- Varios
        public bool IsIntType(Type type)
        {
            if (
                type == typeof(Int16) ||
                type == typeof(Int32) ||
                type == typeof(Int64) ||
                type == typeof(UInt16) ||
                type == typeof(UInt32) ||
                type == typeof(UInt64)
                )
                return true;
            else
                return false;
        }
        public bool IsDecType(Type type)
        {
            if (
                type == typeof(Decimal) ||
                type == typeof(Double) ||
                type == typeof(Single)
                )
                return true;
            else
                return false;
        }
        public string fRemoveString(string s, string r)
        {
            if (string.IsNullOrEmpty(s))
                return null;
            if (s == r)
            {
                s = "";
            }
            else if (s.StartsWith(r + " "))
            {
                s = s.Replace(r + " ", "");
            }
            else if (s.EndsWith(" " + r))
            {
                s = s.Replace(" " + r, "");
            }
            else
                s = s.Replace(" " + r + " ", " ");
            s = s.Trim();
            return s;
        }
        public void fValidarFecha(AjaxControlToolkit.CalendarExtender cal, RangeValidator rv, string min_fecha, int meses)
        {
            if (min_fecha == "0" || String.IsNullOrEmpty(min_fecha))
                min_fecha = "2008-01-01";

            DateTime fecha = DateTime.ParseExact(min_fecha, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            cal.StartDate = fecha;
            cal.EndDate = DateTime.Today.AddMonths(meses).AddDays(1).AddSeconds(-1);

            rv.MinimumValue = min_fecha;
            rv.MaximumValue = DateTime.Now.Date.AddMonths(meses).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            rv.ErrorMessage = string.Format(clConstantes.FECHA_RANGO_ERR, fecha.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.Now.Date.AddMonths(meses).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            rv.CssClass = "invalid-feedback";
            rv.Display = ValidatorDisplay.Dynamic;
            rv.Type = ValidationDataType.Date;
            rv.SetFocusOnError = rv.SetFocusOnError.Equals(true);
        }
        public void fValidarFecha_old(string control, AjaxControlToolkit.CalendarExtender cal, RangeValidator rv, string min_fecha, int meses)
        {
            if (min_fecha == "0" || String.IsNullOrEmpty(min_fecha))
                min_fecha = "2008-01-01";

            DateTime fecha = DateTime.ParseExact(min_fecha, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            cal.StartDate = fecha;
            cal.EndDate = DateTime.Today.AddMonths(meses).AddDays(1).AddSeconds(-1);

            rv.MinimumValue = min_fecha;
            rv.MaximumValue = DateTime.Now.Date.AddMonths(meses).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            rv.ErrorMessage = string.Format(clConstantes.FECHA_RANGO_ERR_old, control, fecha.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.Now.Date.AddMonths(meses).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            rv.CssClass = "valg";
            rv.Display = ValidatorDisplay.Dynamic;
            rv.Type = ValidationDataType.Date;
            rv.SetFocusOnError = rv.SetFocusOnError.Equals(true);
        }
        public void fValidarLongitud(string control, RegularExpressionValidator rev, int max)
        {
            rev.ValidationExpression = "^[\\s\\S]{0," + max + "}$";
            rev.ErrorMessage = "{ " + control + ": Máximo " + max + " carácteres}";
            rev.CssClass = "valg";
            rev.Display = ValidatorDisplay.Dynamic;
            rev.SetFocusOnError = rev.SetFocusOnError.Equals(true);
        }
        public void fDetalle(WebControl aspControl, object column, string typetext = "")
        {
            string valor = column.ToString();

            if (aspControl.GetType().Name == "TextBox")
            {
                TextBox ctlT = aspControl as TextBox;

                if (typetext.StartsWith("N") || typetext.StartsWith("P"))
                {
                    if (decimal.TryParse(valor, out decimal dec))
                        ctlT.Text = dec.ToString(typetext, CultureInfo.CreateSpecificCulture("es-CO")).Replace("%", "").Trim();

                    AttributeAdd(ctlT, "onkeypress", "return SoloDecimal(event);");
                    AttributeAdd(ctlT, "onchange", "FormatDecimal(this, " + (typetext + "0").Substring(1, 1) + ");");

                }
                else if (string.IsNullOrEmpty(valor))
                    ctlT.Text = "";
                else if (fCheckDate(valor))
                    ctlT.Text = Convert.ToDateTime(valor).ToString("yyyy-MM-dd");  //oUtil.ConvertToFechaDetalle(valor);
                else
                    ctlT.Text = valor;
            }
            else if (aspControl.GetType().Name == "DropDownList")
            {
                DropDownList ctlD = aspControl as DropDownList;
                ctlD.SelectedIndex = -1;
                if (!string.IsNullOrEmpty(valor))
                    ctlD.Items.FindByValue(valor).Selected = true;
            }
            else if (aspControl.GetType().Name == "CheckBox")
            {
                CheckBox ctlC = aspControl as CheckBox;
                if (!string.IsNullOrEmpty(valor))
                    ctlC.Checked = Convert.ToBoolean(Convert.ToInt16(valor));
                else
                    ctlC.Checked = false;
            }
        }
        public void fDetalleFecha(TextBox textbox, string fecha)
        {
            if (!string.IsNullOrEmpty(fecha))
                textbox.Text = oUtil.ConvertToFechaDetalle(fecha);
            else
                textbox.Text = "";
        }
        public bool fCheckDate(String date)
        {
            try
            {
                DateTime dt = DateTime.ParseExact(date, "d/MM/yyyy hh:mm:ss tt", new CultureInfo("es-CO"));
                return true;
            }
            catch
            {
                try
                {
                    DateTime dt = DateTime.ParseExact(date, "dd/MM/yyyy hh:mm:ss tt", new CultureInfo("es-CO"));
                    return true;
                }
                catch
                {
                    return false;
                }

            };
        }
        public void fDetalleDropDown(DropDownList dropdown, string valor)
        {
            dropdown.SelectedIndex = -1;
            if (!string.IsNullOrEmpty(valor) && dropdown.Items.FindByValue(valor)!= null)
                dropdown.Items.FindByValue(valor).Selected = true;
        }
        public void fLoadUsuariosFiltro(DropDownList ddl_usuario, int filtro, string cod_usuario = "-1")
        {
            int cod_usu;
            if (string.IsNullOrEmpty(cod_usuario))
                cod_usu = -1;
            else
                cod_usu = Convert.ToInt32(cod_usuario);

            ddl_usuario.Items.Clear();
            ddl_usuario.DataSource = oUsuarios.sp_s_usuarios_filtro(filtro, cod_usu);
            ddl_usuario.DataTextField = "nombre_completo";
            ddl_usuario.DataValueField = "cod_usuario";
            ddl_usuario.DataBind();

            if (ddl_usuario.Items.FindByValue(cod_usu.ToString()) != null)
                ddl_usuario.Items.FindByValue(cod_usu.ToString()).Selected = true;
            else
                ddl_usuario.Items.Insert(0, new ListItem("--Seleccione opción", ""));
        }
        public bool fCheckUsuarioRegistro(string cod_usu)
        {
            if (cod_usu == oVar.prUserCod.ToString())
                return true;
            else
                return false;
        }
        public string RandomPassword()
        {
            int LONG_PASS = 10;
            string contraseña = string.Empty;
            string[] letras = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                                ".", "!", "#", "$", "%", "&", "_", "-", "?", "*", "+", "=", ":"};
            Random EleccionAleatoria = new Random();

            while (contraseña.Length < LONG_PASS)
            {
                int LetraAleatoria = EleccionAleatoria.Next(0, 75);
                int NumeroAleatorio = EleccionAleatoria.Next(0, 9);

                if (LetraAleatoria < letras.Length)
                {
                    contraseña += letras[LetraAleatoria];
                }
                else
                {
                    contraseña += NumeroAleatorio.ToString();
                }
            }
            return contraseña;
        }
        #endregion

        #region-------------------------------------------------------------------- dropdowns
        public void ddlbAnos(DropDownList ddl, string selected)
        {
            int min = 2002;
            int max = DateTime.Now.Year;
            for (int i = max; i >= min; i--)
            {
                ddl.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            if (selected == "l")
                ddl.SelectedIndex = ddl.Items.Count - 1;
            else if (selected == "f")
                ddl.SelectedIndex = 0;
            else
                ddl.SelectedValue = selected;
        }
        public void ddlbPorc10(DropDownList ddl, string selected)
        {
            int min = 0;
            int max = 100;
            for (int i = min; i <= max; i += 10)
            {
                ddl.Items.Add(new ListItem(i.ToString() + "%", i.ToString()));
            }

            if (selected == "l")
                ddl.SelectedIndex = ddl.Items.Count - 1;
            else if (selected == "f")
                ddl.SelectedIndex = 0;
            else
                ddl.SelectedValue = selected;
        }
        #endregion

        #region-------------------------------------------------------------------- loop controls
        public void fEnableControls(Control ctrl)
        {
            foreach (Control item in ctrl.Controls)
            {
                if (item is AjaxControlToolkit.TabPanel)
                {
                    TabPanel tp = item as TabPanel;
                    foreach (Control tpControls in tp.Controls)
                    {
                        fEnableControls(tpControls);
                    }
                }
                else if (item.Controls.Count > 1)
                    fEnableControls(item);
                else
                {
                    WebControl wctl = item as WebControl;
                    wctl.Enabled = true;
                }
            }
        }
        public void fClearControls(Control ctrl)
        {
            foreach (Control item in ctrl.Controls)
            {
                if (item is AjaxControlToolkit.TabPanel)
                {
                    TabPanel tp = item as TabPanel;
                    foreach (Control tpControls in tp.Controls)
                    {
                        fClearControls(tpControls);
                    }
                }
                else
                {
                    if (item.GetType() == typeof(TextBox))
                    {
                        TextBox txt = item as TextBox;
                        txt.Text = string.Empty;
                    }
                    else if (item.GetType() == typeof(DropDownList))
                    {
                        DropDownList dl = item as DropDownList;
                        dl.SelectedIndex = -1;
                    }
                    else if (item.GetType() == typeof(CheckBox))
                    {
                        CheckBox chk = item as CheckBox;
                        chk.Checked = false;
                    }
                    else if (item.GetType() == typeof(AsyncFileUpload))
                    {
                        AsyncFileUpload afu = item as AsyncFileUpload;
                        if(afu.HasFile)
                            afu.ClearAllFilesFromPersistedStore();
                    }

                    if (item.Controls.Count > 1)
                        fClearControls(item);
                }
            }
        }
        public void fEditControls(Control ctrl, bool enable)
        {
            foreach (Control item in ctrl.Controls)
            {
                if (item is AjaxControlToolkit.TabPanel)
                {
                    TabPanel tp = item as TabPanel;
                    foreach (Control tpControls in tp.Controls)
                    {
                        fEditControls(tpControls, enable);
                    }
                }
                else if (item.Controls.Count >= 1)
                    fEditControls(item, enable);
                else
                {
                    if (item.GetType() == typeof(TextBox))
                    {
                        TextBox txt = item as TextBox;
                        txt.Enabled = enable;
                        Label lbl = (Label)ctrl.FindControl(txt.ID.Replace("txt", "lbl"));
                        if (lbl != null)
                        {
                            txt.Visible = enable;
                            lbl.Visible = !enable;
                        }
                    }
                    else if (item.GetType() == typeof(DropDownList))
                    {
                        DropDownList dl = item as DropDownList;
                        dl.Enabled = enable;
                        
                        Label lbl = (Label)ctrl.FindControl(dl.ID.Replace("ddl", "lbl"));
                        if (lbl != null)
                        {
                            dl.Visible = enable;
                            lbl.Visible = !enable;
                        }
                    }
                    else if (item.GetType() == typeof(CheckBox))
                    {
                        CheckBox chk = item as CheckBox;
                        chk.Enabled = enable;
                    }
                    else if (item.GetType() == typeof(RadioButton))
                    {
                        RadioButton chk = item as RadioButton;
                        chk.Enabled = enable;
                    }
                }
            }
        }
        public void fValueTextBox(TextBox txt, string tipo, string valorColumn) {
            switch (tipo.ToLower())
            {
                case "date":
                    txt.Text = oUtil.ConvertToFechaDetalle(valorColumn);
                    break;
                case "int":
                    txt.Text = int.Parse(valorColumn).ToString("N0", CultureInfo.CreateSpecificCulture("es-CO"));
                    txt.Attributes.Add("MaxLength", "15");
                    AttributeAdd(txt, "onkeypress", "return SoloEntero(event);");
                    AttributeAdd(txt, "onchange", "FormatDecimal(this, 0);");
                    ClassAdd(txt, "text-right");
                    break;
                case "porc":
                    txt.Text = decimal.Parse(valorColumn).ToString("P2", CultureInfo.CreateSpecificCulture("es-CO")).Replace("%", "").Trim();
                    txt.Attributes.Add("MaxLength", "6");
                    AttributeAdd(txt, "onkeypress", "return SoloDecimal(event);");
                    AttributeAdd(txt, "onchange", "FormatDecimal(this, 2);");
                    ClassAdd(txt, "text-right");
                    break;
                case "dec":
                    txt.Text = decimal.Parse(valorColumn).ToString("N2", CultureInfo.CreateSpecificCulture("es-CO"));
                    txt.Attributes.Add("MaxLength", "20");
                    AttributeAdd(txt, "onkeypress", "return SoloDecimal(event);");
                    AttributeAdd(txt, "onchange", "FormatDecimal(this, 2);");
                    ClassAdd(txt, "text-right");
                    break;
                default:
                    txt.Text = valorColumn;
                    break;
            }
        }
        public void fValueControls(Control ctrl, DataRow dRow, string FileImages = "")
        {
            foreach (Control item in ctrl.Controls)
            {
                if (item is AjaxControlToolkit.TabPanel)
                {
                    TabPanel tp = item as TabPanel;
                    foreach (Control tpControls in tp.Controls)
                    {
                        fValueControls(tpControls, dRow, FileImages);
                    }
                }
                else if (item.Controls.Count >= 1)
                    fValueControls(item, dRow, FileImages);
                else
                {
                    if (!string.IsNullOrEmpty(item.ID) && (item is TextBox || item is Label || item is DropDownList || item is CheckBox || item is HiddenField || item is ListBox || item is RadioButtonList || item is Image))
                    {
                        try
                        {
                            string nameColumn = item.ID.ToString().Substring(item.ID.ToString().IndexOf("_") + 1);
                            string valorColumn = "";
                            Type type_column = null;
                            try
                            {
                                valorColumn = dRow[dRow.Table.Columns[nameColumn].Ordinal].ToString();
                                type_column = dRow.Table.Columns[nameColumn].DataType;
                            }
                            catch
                            {
                                valorColumn = dRow[dRow.Table.Columns[nameColumn.Remove(nameColumn.IndexOf("__"))].Ordinal].ToString();
                                type_column = dRow.Table.Columns[nameColumn.Remove(nameColumn.IndexOf("__"))].DataType;
                            }
                            bool isInt = false;
                            bool isDec = false;
                            bool isDate = false;
                            decimal numValue = 0;

                            if (IsIntType(type_column))
                            {
                                isInt = true;
                                try
                                {
                                    numValue = int.Parse(valorColumn);
                                }
                                catch { }
                            }
                            else if (IsDecType(type_column))
                            {
                                isDec = true;
                                try
                                {
                                    numValue = decimal.Parse(valorColumn);
                                }
                                catch { }
                            }
                            else if (type_column == typeof(DateTime))
                                isDate = true;


                            if (item.GetType() == typeof(TextBox))
                            {
                                TextBox txt = item as TextBox;
                                if (isDate)
                                {
                                    txt.Text = oUtil.ConvertToFechaDetalle(valorColumn);
                                }
                                else if (isInt)
                                {
                                    txt.Text = numValue.ToString("N0", CultureInfo.CreateSpecificCulture("es-CO"));
                                    txt.Attributes.Add("MaxLength", "15");
                                    AttributeAdd(txt, "onkeypress", "return SoloEntero(event);");
                                    AttributeAdd(txt, "onchange", "FormatDecimal(this, 0);");

                                    ClassAdd(txt, "text-right");
                                }
                                else if (isDec && nameColumn.StartsWith("porc_"))
                                {
                                    txt.Text = numValue.ToString("P2", CultureInfo.CreateSpecificCulture("es-CO")).Replace("%", "").Trim();
                                    txt.Attributes.Add("MaxLength", "6");
                                    AttributeAdd(txt, "onkeypress", "return SoloDecimal(event);");
                                    AttributeAdd(txt, "onchange", "FormatDecimal(this, 2);");
                                    ClassAdd(txt, "text-right");
                                }
                                else if (isDec)
                                {
                                    txt.Text = numValue.ToString("N2", CultureInfo.CreateSpecificCulture("es-CO"));
                                    txt.Attributes.Add("MaxLength", "20");
                                    AttributeAdd(txt, "onkeypress", "return SoloDecimal(event);");
                                    AttributeAdd(txt, "onchange", "FormatDecimal(this, 2);");
                                    ClassAdd(txt, "text-right");
                                }
                                else
                                    txt.Text = valorColumn;
                            }
                            else if (item.GetType() == typeof(Label))
                            {
                                Label dl = item as Label;
                                dl.Text = valorColumn;
                            }
                            else if (item.GetType() == typeof(DropDownList))
                            {
                                DropDownList dl = item as DropDownList;
                                dl.SelectedIndex = -1;
                                if (!string.IsNullOrEmpty(valorColumn))
                                    dl.Items.FindByValue(valorColumn).Selected = true;
                            }
                            else if (item.GetType() == typeof(RadioButtonList))
                            {
                                RadioButtonList dl = item as RadioButtonList;
                                if (!string.IsNullOrEmpty(valorColumn))
                                    dl.Items.FindByValue(valorColumn).Selected = true;
                            }
                            else if (item.GetType() == typeof(CheckBox))
                            {
                                CheckBox chk = item as CheckBox;
                                chk.Checked = Convert.ToBoolean(Convert.ToInt16(valorColumn));
                            }
                            else if (item.GetType() == typeof(HiddenField))
                            {
                                HiddenField hdd = item as HiddenField;
                                hdd.Value = valorColumn;
                            }
                            else if (item.GetType() == typeof(Image))
                            {
                                Image img = item as Image;
                                if (valorColumn != null && valorColumn.ToString().Length > 2)
                                {
                                    img.ImageUrl = ConfigurationManager.AppSettings["UrlImages"].ToString() + FileImages + valorColumn;
                                    img.Visible = true;
                                }
                                else
                                    img.Visible = false;
                            }
                        }
                        catch { }
                    }
                }
            }

        }
        public void AttributeAdd(TextBox txt, string key, string value)
        {
            if ((txt.Attributes[key]??"").IndexOf(value) < 0)
                txt.Attributes.Add(key, (txt.Attributes[key] ?? "") + value +" ");
        }
        #endregion
    }
}
