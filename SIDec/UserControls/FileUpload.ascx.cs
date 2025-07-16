using GLOBAL.DAL;
using GLOBAL.LOG;
using GLOBAL.PERMISOS;
using GLOBAL.UTIL;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using cnsAction = GLOBAL.CONST.clConstantes.Accion;
using tipoArchivo = GLOBAL.CONST.clConstantes.TipoArchivo;

namespace SIDec.UserControls
{
    public partial class FileUpload : UserControl
    {
        private readonly ARCHIVO_DAL oArchivo = new ARCHIVO_DAL();

        private readonly clBasic oBasic = new clBasic();
        private readonly clFile oFile = new clFile();
        private readonly clLog oLog = new clLog();
        private readonly clPermisos oPermisos = new clPermisos();

        private const string _SOURCEPAGE = "FileUpload";
        private const string EXCEPTION_EXTENSION = "Extensión inválida";

        public delegate void OnViewDocEventHandler(object sender);
        public event OnViewDocEventHandler ViewDoc;

        public delegate void OnUserControlExceptionEventHandler(object sender, Exception ex);
        public event OnUserControlExceptionEventHandler UserControlException;



        #region Propiedades
        /// <summary>
        /// Identificador del registro del archivo
        /// </summary>
        public int ArchivoID
        {
            get
            {
                return (Int32.TryParse(hddArchivoPrimary.Value, out int id) ? id : 0);
            }
            set
            {
                hddArchivoPrimary.Value = value.ToString();
                hdd_idarchivo.Value = hddArchivoPrimary.Value;
                LoadGrid();
            }
        }
        /// <summary>
        /// Código del usuario responsable del registro
        /// </summary>
        public string CodUsuario
        {
            get
            {
                return (Session[ControlID + ".FileUpload.CodUsuario"] ?? "").ToString();
            }
            set
            {
                Session[ControlID + ".FileUpload.CodUsuario"] = value;
            }
        }
        /// <summary>
        /// Identificador del instancia del control para evitar solapar las variables de sesión
        /// </summary>
        public string ControlID
        {
            get
            {
                return (hddId.Value);
            }
            set
            {
                hddId.Value = value;
            }
        }
        public bool Enabled
        {
            get
            {
                return (Session[ControlID + ".FileUpload.Enabled"] ?? "0").ToString() == "1";
            }
            set
            {
                 Session[ControlID + ".FileUpload.Enabled"] = value ? "1" : "0";
            }
        }
        public string Extensions
        {
            get
            {
                return (Session[ControlID + ".FileUpload.Extensions"] ?? "*").ToString();
            }
            set
            {
                Session[ControlID + ".FileUpload.Extensions"] = value.ToLower();
            }
        }
        /// <summary>
        /// Ruta de la carpeta donde se guardan los archivos de las evidencias de las visitas
        /// </summary>
        public string FilePath
        {
            get
            {
                return Session[ControlID + ".FileUpload.FilePath"].ToString();
            }
            set
            {
                Session[ControlID + ".FileUpload.FilePath"] = value;
            }
        }
        public bool Multiple
        {
            get
            {
                return (Session[ControlID + ".FileUpload.Multiple"] ?? "0").ToString() == "1";
            }
            set
            {
                Session[ControlID + ".FileUpload.Multiple"] = value ? "1" : "0";
            }
        }
        /// <summary>
        /// Prefijo con el que se almacena los archivos de las evidencias
        /// </summary>
        public tipoArchivo Prefix
        {
            get
            {
                return (tipoArchivo)(Session[ControlID + ".LoadFile.Prefix"]?? tipoArchivo.ND);
            }
            set
            {
                Session[ControlID + ".LoadFile.Prefix"] = value;
            }
        }
        /// <summary>
        /// Id del registro de la tabla con la que se relacionan los archivos
        /// </summary>
        public int ReferenceID
        {
            get
            {
                return (Int32.TryParse(hddReferenceID.Value, out int id) ? id : 0);
            }
            set
            {
                hddReferenceID.Value = value.ToString();
            }
        }
        /// <summary>
        /// Objeto de permiso a verificars
        /// </summary>
        public string SectionPermission
        {
            get
            {
                return (Session[ControlID + ".LoadFile.SectionPermission"]??"").ToString();
            }
            set
            {
                Session[ControlID + ".LoadFile.SectionPermission"] = value;
            }
        }
        /// <summary>
        /// Indica si se debe validar un usuario responsable
        /// </summary>
        public bool ValidateResponsible
        {
            get
            {
                return (Session[ControlID + ".FileUpload.ValidateResponsible"] ?? "0").ToString() == "1";
            }
            set
            {
                Session[ControlID + ".FileUpload.ValidateResponsible"] = value ? "1" : "0";
            }
        }
        /// <summary>
        /// Indicador para saber si se debe tener un usuario responsable 
        /// </summary>
        public bool RequeridedResponsible
        {
            get
            {
                return (Session[ControlID + ".FileUpload.RequeridedResponsible"] ?? "1").ToString() == "1";
            }
            set
            {
                Session[ControlID + ".FileUpload.RequeridedResponsible"] = value ? "1" : "0";
            }
        }
        #endregion



        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Enctype = "multipart/form-data"; 
            RegisterScript();
            if (!IsPostBack)
            {
                LoadGrid();
            }
            ViewControls();
            if (fuLoad.HasFile)
                lbl_nombre.Text = fuLoad.FileName;
        }
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            if (hdd_idarchivo.Value != "0")
            {
                MessageBox1.ShowConfirmation("EDIT", "¿Está seguro de actualizar la información?", type: "warning");
            }
            else
                MessageBox1.ShowConfirmation("ADD", "¿Está seguro de continuar con la acción solicitada?");
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
             if (!ValidateAccess(SectionPermission, cnsAction.INSERTAR, ValidateResponsible, RequeridedResponsible)) return;
            ViewAdd();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }
        protected void btnFileUploadAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;

            switch (btnAccionSource.CommandName)
            {
                case "Listar":
                    LoadGrid();
                    break;
                case "Editar":
                    if (!ValidateAccess(SectionPermission, cnsAction.EDITAR, ValidateResponsible, RequeridedResponsible)) return;
                    ViewEdit();
                    break;
                case "Agregar":
                    if (!ValidateAccess(SectionPermission, cnsAction.INSERTAR, ValidateResponsible, RequeridedResponsible)) return;
                    ViewAdd();
                    break;
                case "Eliminar":
                    if (!ValidateAccess(SectionPermission, cnsAction.ELIMINAR, ValidateResponsible, RequeridedResponsible)) return;
                    ViewDelete();
                    break;
            }
        }
        protected void FileUploadComplete(object sender, EventArgs e)
        {
            bool isValidExtension = Extensions == "*";
            string extension = Path.GetExtension(fuLoad.FileName).Substring(1);
            foreach (string type in Extensions.Split(','))
            {
                isValidExtension = (isValidExtension || type == extension);
            }
            
            if (!isValidExtension)
            {
                lbl_nombre.Text = EXCEPTION_EXTENSION;
                lbl_nombre.ForeColor = System.Drawing.Color.Red;

                fuLoad.ClearAllFilesFromPersistedStore();
                UserControlException?.Invoke(this, new Exception(EXCEPTION_EXTENSION));
            }
            else
            {
                lbl_nombre.ForeColor = System.Drawing.Color.Black;
                lbl_nombre.Text = fuLoad.FileName;
            }
        }
        protected void gvArchivos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvArchivos, "Select$" + e.Row.RowIndex.ToString()));

                string ruta = gvArchivos.DataKeys[e.Row.DataItemIndex % gvArchivos.PageSize]["ruta"].ToString();
                string ext = Path.GetExtension(ruta).Substring(1);

                (e.Row.Cells[3].FindControl("btnPdf") as LinkButton).Text = ext.ToLower() == "pdf" ?
                                                                                "<i class='fas fa-file-pdf'></i>"
                                                                                : (ext.ToLower() == "jpg" || ext.ToLower() == "png" || ext.ToLower() == "jpeg") ?
                                                                                        "<i class='fas fa-file-image'></i>"
                                                                                        : "<i class='fas fa-file'></i>";
            }
        }
        protected void gvArchivos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                if (rowIndex >= gvArchivos.Rows.Count)
                    rowIndex = 0;
                hdd_idarchivo.Value = rowIndex >= 0 ? gvArchivos.DataKeys[rowIndex]["idarchivo"].ToString() : "0";

                switch (e.CommandName)
                {
                    case "_Detail":
                        ViewDetail();
                        break;
                    case "_OpenFile":
                        ViewFile(gvArchivos.DataKeys[rowIndex]["ruta"].ToString());
                        break;
                    default:
                        return;
                }
            }
        }
        protected void lbPdf_Click(object sender, EventArgs e)
        {
            ViewFile(hdd_ruta.Value);
        }
        protected void MessageBox_Accept(string key)
        {
            string strResult = "";
            switch (key)
            {
                case "ADD":
                case "EDIT":
                    strResult = Save();
                    break;
                case "Delete":
                    strResult = Delete();
                    break;
                default:
                    break;
            }
            LoadGrid();
        }
        #endregion



        #region Métodos Privados
        private string Delete()
        {
            hdd_idarchivo.Value = hdd_idarchivo.Value == "" ? "0" : hdd_idarchivo.Value;

            string strResult = oArchivo.sp_d_archivo(hdd_idarchivo.Value);
            if (oBasic.AlertUserControl(null, null, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "d"))
            {
                LoadGrid();
            }
            return strResult;
        }
        private void LoadDetail()
        {
            pnlDetail.Visible = true;
            pnlGrid.Visible = false;
            lbl_nombre.Text = "No hay archivo adjunto";
            lbPdf.Visible = false;

            DataSet dSet = oArchivo.sp_s_archivos_listar_hijos(hdd_idarchivo.Value, ((int)Prefix).ToString());
            if (dSet != null && dSet.Tables.Count > 0 && dSet.Tables[0].Rows.Count > 0)
            {
                lbPdf.Visible = true;
                DataRow dRow = dSet.Tables[0].Rows[0];

                oBasic.fValueControls(pnlDetail, dRow);
                string ruta = hdd_ruta.Value.ToLower();
                string ext = Path.GetExtension(ruta).Substring(1);

                lbPdf.Text = ext == "pdf" ?
                                "<i class='fas fa-file-pdf'></i>"
                                : (ext == "jpg" || ext == "png" || ext == "jpeg") ?
                                        "<i class='fas fa-file-image'></i>"
                                        : "<i class='fas fa-file'></i>";

                if (lbl_fec_auditoria_archivo.Text.Trim().Length > 0)
                {
                    lbl_fec_auditoria_archivo.Text = "Modificado el: " + lbl_fec_auditoria_archivo.Text;
                    lbl_fec_auditoria_archivo.ToolTip = lbl_fec_auditoria_archivo.Text;
                }
            }
            oBasic.EnableControls(pnlDetail, Enabled, true);

            lbPdf.Enabled = true;
            oBasic.FixPanel(divData, "FileUpload", Enabled ? 2 : 0, pList: Multiple, pEdit: !(hdd_idarchivo.Value == "0"), pDelete: !(hdd_idarchivo.Value == "0"));
        }
        public void LoadGrid()
        {
            pnlDetail.Visible = false;
            pnlGrid.Visible = true;
            //Enabled = false;
            if (!Multiple)
            {
                LoadDetail();
                return;
            }
            if (Prefix != tipoArchivo.ND)
            {
                DataSet ds = oArchivo.sp_s_archivos_listar_hijos(ArchivoID.ToString(), ((int)Prefix).ToString());
                gvArchivos.DataSource = ds;
                gvArchivos.DataBind();
            }
            oBasic.FixPanel(divData, "FileUpload", 0, pList: false, pAdd: false, pEdit: false, pDelete: false);
        }
        private void RegisterScript()
        {
            if (Enabled)
            {
                lbLoad.Attributes.Add("onclick", "$(\"input[ID*='" + fuLoad.ClientID + "']\").click();return false;");
                fuLoad.Attributes.Add("onchange", "infoFile_" + ControlID.Replace('.', '_') + "();");

                string key = ControlID + ".LoadFileEvent";
                StringBuilder scriptSlider = new StringBuilder();
                scriptSlider.Append("<script type='text/javascript'> ");
                scriptSlider.Append("   function infoFile_" + ControlID.Replace('.', '_') + "() {  ");
                scriptSlider.Append("       var name = $(\"input[ID*='" + fuLoad.ClientID + "']\").val(); ");
                scriptSlider.Append("       $('#" + lbPdf.ClientID + "').hide(); ");
                scriptSlider.Append("       name = name.substring(name.lastIndexOf('\\\\') + 1); ");
                scriptSlider.Append("       var ext = name.substring(name.lastIndexOf('.') + 1).toLowerCase(); ");
 
                if (Extensions != "*")
                {
                    string extensions = "";
                    string msgExtensions = "";
                    foreach (string extension in Extensions.Split(','))
                    {
                        extensions += " && ext !== '" + extension.Trim() + "'";
                        msgExtensions += ", " + extension.Trim();
                    }
                    scriptSlider.Append("       if(" + extensions.Substring(3) + "){");
                    scriptSlider.Append("          $('#" + txtInfo.ClientID + "').val(''); ");
                    scriptSlider.Append("          $('#" + lbl_nombre.ClientID + "').html(''); ");
                    scriptSlider.Append("           $('#" + lblError.ClientID + "').html('Extensión inválida, solo se permite «" + msgExtensions.Substring(2) + "»'); ");
                    scriptSlider.Append("           $('#" + lblError.ClientID + "').attr('title','Extensión inválida, solo se permite «" + msgExtensions.Substring(2) + "»');} ");
                    scriptSlider.Append("       else ");
                }

                scriptSlider.Append("       {   $('#" + lblError.ClientID + "').html(''); ");
                scriptSlider.Append("           $('#" + lblError.ClientID + "').attr('title', ''); ");
                scriptSlider.Append("           $('#" + lbl_nombre.ClientID + "').html(name); ");
                scriptSlider.Append("          $('#" + txtInfo.ClientID + "').val(name); }");
                scriptSlider.Append("          Page_ClientValidate('vgFileUpload');");
                scriptSlider.Append("} </script> ");

                if (!Page.ClientScript.IsStartupScriptRegistered(key))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), key, scriptSlider.ToString(), false);
                }
            }
        }
        private string Save()
        {
            string filepath = SaveFilePhysical();
            string strResult = SaveFileDataBase(filepath);
            LoadGrid();
            return strResult;
        }
        private string SaveFileDataBase(string filepath)
        {
            string strResult;
            hdd_idarchivo.Value = hdd_idarchivo.Value == "" ? "0" : hdd_idarchivo.Value;
            if (hdd_idarchivo.Value == "0")
                strResult = oArchivo.sp_i_archivo(ArchivoID, Convert.ToInt32(Prefix), Prefix.ToString(), fuLoad.FileName, filepath, txt_descripcion.Text);
            else
                strResult = oArchivo.sp_u_archivo(Convert.ToInt32(hdd_idarchivo.Value), ArchivoID, (int)Prefix, Prefix.ToString(), fuLoad.FileName, filepath, txt_descripcion.Text);

            if (oBasic.AlertUserControl(null, null, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name + ".archivo", "i"))
            {
                if (ArchivoID == 0)
                {
                    ArchivoID = Convert.ToInt32(strResult.Split(':')[1]);
                    strResult = oArchivo.sp_u_archivo_referencia(hdd_idarchivo.Value, ReferenceID.ToString(), ((int)Prefix));
                    oBasic.AlertUserControl(null, null, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name + ".referencia", "i");
                }
            }

            return strResult;
        }
        private string SaveFilePhysical()
        {
            string fileName = "";
            bool isValidExtension = Extensions == "*";
            if (fuLoad.HasFile)
            {
                string extension = Path.GetExtension(fuLoad.FileName).Substring(1).ToLower();
                foreach (string type in Extensions.Split(','))
                {
                    isValidExtension = (isValidExtension || type == extension);
                }

                if (isValidExtension)
                {
                    fileName = Prefix.ToString() + "_";
                    fileName += ReferenceID == 0 ? "" : ReferenceID + "_";
                    fileName += Guid.NewGuid() + Path.GetExtension(fuLoad.FileName).ToLower();

                    bool Resultado = oFile.fVerificarPath(FilePath);
                    if (Resultado)
                    {
                        fileName = Path.Combine(FilePath, fileName);
                        try
                        {
                            fuLoad.SaveAs(fileName);
                        }
                        catch (Exception e)
                        {
                            oLog.RegistrarLogError("Error subiendo archivo " + e.Message + ":" + fileName + ":::" + extension, _SOURCEPAGE, ControlID);
                        }
                    }
                    else
                        oLog.RegistrarLogError("Error creando ruta " + FilePath, _SOURCEPAGE, ControlID);
                }
                else
                {
                    UserControlException?.Invoke(this, new Exception(EXCEPTION_EXTENSION));
                }
            }
            else
            {
                if(hdd_idarchivo.Value == "0")
                    UserControlException?.Invoke(this, new Exception("Seleccione un archivo para continuar."));
            }

            return fileName;
        }
        private bool ValidateAccess(string section, string action, bool validateResponsible = false, bool requeridedResponsible = true)
        {
            string message = oPermisos.ValidateAccess(section, action, CodUsuario, validateResponsible, requeridedResponsible);

            if (message == "")
                return true;

            MessageInfo.ShowMessage(message);
            return false;
        }
        /*private bool ValidateAccess(string section, string action, bool validateResponsible = false, bool requeridedResponsible = false)
        {
            string cod_usu_responsable = (Session["Proyecto.cod_usu_responsable"] ?? 0).ToString();
            if (cod_usu_responsable == "0" && requeridedResponsible)
            {
                MessageInfo.ShowMessage("Esta acción requiere que se asigne un responsable");
                return false;
            }
            cod_usu_responsable = (!validateResponsible && cod_usu_responsable == "0" ? oVar.prUserCod : cod_usu_responsable).ToString();
            if (!oPermisos.TienePermisosAccion(section, action))
            {
                MessageInfo.ShowMessage("No cuenta con permisos suficientes para realizar esta acción");
                return false;
            }
            else if (!oPermisos.TienePermisosAccion(section, action, cod_usu_responsable, oVar.prUserCod.ToString()) && validateResponsible)
            {
                MessageInfo.ShowMessage("Para realizar esta acción comuníquese con el usuario responsable");
                return false;
            }
            return true;
        }*/
        private void ViewAdd()
        {
            pnlDetail.Visible = true;
            pnlGrid.Visible = false;
            oBasic.fClearControls(pnlDetail);
            hdd_idarchivo.Value = "0";
            lbl_nombre.Text = "No hay archivo adjunto";
            lbPdf.Visible = false;
            Enabled = true;
            oBasic.EnableControls(pnlDetail, true, true);
            oBasic.FixPanel(divData, "FileUpload", 2);
            ViewControls();
        }
        private void ViewControls()
        {
            txt_descripcion.Enabled = Enabled;
            lbLoad.Visible = Enabled;
            rfv_InfoFile.Enabled = Enabled && hdd_idarchivo.Value == "0";
            if (Enabled)
                RegisterScript();
        }
        private void ViewDelete()
        {
            Enabled = false;
            ViewControls();
            LoadDetail();
            MessageBox1.ShowConfirmation("Delete", "¿Está seguro de eliminar este documento?", type: "danger");
        }
        private void ViewDetail()
        {
            Enabled = false;
            ViewControls();
            LoadDetail();
        }
        private void ViewEdit()
        {
            Enabled = true;
            oBasic.fClearControls(pnlDetail);
            ViewControls();
            LoadDetail();
        }
        private void ViewFile(string ruta)
        {
            if (!ValidateAccess(SectionPermission, cnsAction.CONSULTAR, false, false)) return;

            string ext = Path.GetExtension(ruta).Substring(1);

            oFile.GetPath(ruta);
            ViewDoc?.Invoke(this);
        }
        #endregion

    }
}