using AjaxControlToolkit;
using GLOBAL.CONST;
using GLOBAL.DAL;
using GLOBAL.LOG;
using GLOBAL.PERMISOS;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using SIDec.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Color = GLOBAL.CONST.clConstantes.Color;
using tipoArchivo = GLOBAL.CONST.clConstantes.TipoArchivo;

namespace SIDec.UserControls.Particular
{
    public partial class ProjectBankHeaderPE : UserControl
    {
        readonly ARCHIVO_DAL oArchivo = new ARCHIVO_DAL();
        readonly BANCOPROYECTOS_DAL oBanco = new BANCOPROYECTOS_DAL();
        readonly BANCOACTIVIDADES_DAL oActividades = new BANCOACTIVIDADES_DAL();
        readonly BancoEntidades_DAL oBancoEntidades = new BancoEntidades_DAL();
        readonly BancoTratamientos_DAL oBancoTratamientos = new BancoTratamientos_DAL();
        readonly BancoUPL_DAL oBancoUpl = new BancoUPL_DAL();
        readonly IDENTIDADES_DAL oIdentidades = new IDENTIDADES_DAL();
        readonly UPZ_DAL oUPZ = new UPZ_DAL();
        readonly UPL_DAL oUPL = new UPL_DAL();
        readonly USUARIOS_DAL oUsuarios = new USUARIOS_DAL();

        readonly clBasic oBasic = new clBasic();
        readonly clGlobalVar oVar = new clGlobalVar();
        readonly clFile oFile = new clFile();
        readonly clLog oLog = new clLog();
        private readonly clPermisos oPermisos = new clPermisos();


        private const string _SOURCEPAGE = "ProjectBankHeader";

        #region Propiedades
        public int CodUsuResponsable
        {
            get
            {
                return Convert.ToInt32((Session[ControlID + ".cod_usu_responsable"] ?? 0).ToString());
            }
            set
            {
                Session[ControlID + ".cod_usu_responsable"] = value;
            }
        }
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
        public int IdBanco
        {
            get
            {
                return (Int32.TryParse(hddIdBanco.Value, out int id) ? id : 0);
            }
            set
            {
                hddIdBanco.Value = value.ToString();
                ucRepresentative.ReferenceID = value;
            }
        }
        public string NombreProyecto { get { return txt_nombre.Text; } }
        public bool Enabled
        {
            get
            {
                return hddEnabled.Value == "1";
            }
            set
            {
                hddEnabled.Value = value ? "1" : "0";
            }
        }
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
                LoadDropDowns();
                HideShowFileUpload(false);
            }
        }
        protected void afuImage_UploadedComplete(object sender, AsyncFileUploadEventArgs e)
        {
            ttNoImage.Visible = ttFileRequired.Visible = false;
            string contentType = afuImage.ContentType;
            if (!(contentType == "image/jpeg" || contentType == "image/gif"
                        || contentType == "image/png"))
            {
                ttNoImage.Visible = true;
            }
            upImage.Update();
        }
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            if (FileValidation())
            {
                if (hdd_IdArchivoList.Value == "0")
                    MessageBox1.ShowConfirmation("FILELOAD", "¿Está seguro que desea cargar la imagen?");
                else
                    MessageBox1.ShowConfirmation("FILELOAD", "Se actualizará la imagen ¿Esta seguro que desea continuar?", type: "warning");
            }
            else if (hdd_IdArchivoList.Value != "0")
            {
                MessageBox1.ShowConfirmation("EDIT", "Se conserva el archivo actual y solo se actualizará la información ¿Desea continuar?", type: "warning");
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            HideShowFileUpload(false);
        }
        protected void btnLocalidadUpz_Click(object sender, EventArgs e)
        {
            ucMsUpz.ShowModal();
        }
        protected void btnPBImagesAdd_Click(object sender, EventArgs e)
        {
            if (hddIdBanco.Value == "0")
            {
                MessageInfo.ShowMessage("Se debe crear el proyecto para poder adicionar las imágenes");
                return;
            }
            HideShowFileUpload(true);
        }
        protected void btnPBSubHeader_Click(object sender, EventArgs e)
        {
            int oldIndex = mvPBSubHeader.ActiveViewIndex;
            int newIndex = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            string oldID = "lblPBSubHeader_" + oldIndex.ToString();
            string newID = "lblPBSubHeader_" + newIndex.ToString();
            LinkButton lbOld = (LinkButton)ulPBSubHeader.FindControl(oldID);
            LinkButton lbNew = (LinkButton)ulPBSubHeader.FindControl(newID);
            oBasic.ActiveNav(mvPBSubHeader, lbOld, lbNew, newIndex);
        }
        protected void btnresponsable_Click(object sender, EventArgs e)
        {
            ucMsResponsable.ShowModal();
        }
        protected void gvPBImages_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
            {
                if (rowIndex >= gvPBImages.Rows.Count)
                    rowIndex = 0;

                switch (e.CommandName)
                {
                    case "Select":
                        break;
                    case "_Delete":
                        HideShowFileUpload(false);
                        hdd_IdArchivoList.Value = rowIndex >= 0 ? gvPBImages.DataKeys[rowIndex]["idarchivo"].ToString() : "-1";
                        MessageBox1.ShowConfirmation("DELETE", "¿Esta seguro que desea eliminar la imagen «" + gvPBImages.DataKeys[rowIndex]["nombre"].ToString() + "»?", type: "danger");
                        break;
                    case "_Edit":
                        HideShowFileUpload(true);
                        hdd_IdArchivoList.Value = rowIndex >= 0 ? gvPBImages.DataKeys[rowIndex]["idarchivo"].ToString() : "-1";
                        lblPreviousImage.Text = rowIndex >= 0 ? gvPBImages.DataKeys[rowIndex]["nombre"].ToString() : "";
                        txt_DescripcionImagen.Text = rowIndex >= 0 ? gvPBImages.DataKeys[rowIndex]["descripcion"].ToString() : "";
                        break;
                    default:
                        return;
                }
            }
        }
        protected void MessageBox_Accept(string key)
        {
            try
            {
                string strResult = "";
                switch (key)
                {
                    case "FILELOAD":
                        string pathFile = SaveImageFile();
                        strResult = pathFile == "" ? "999999:Error al guardar el archivo" : SaveImageDataBase(pathFile);
                        ttNoImage.Visible = ttFileRequired.Visible = false;
                        upImage.Update();
                        break;
                    case "EDIT":
                        strResult = SaveImageDataBase(null);
                        ttNoImage.Visible = ttFileRequired.Visible = false;
                        upImage.Update();
                        break;
                    case "DELETE":
                        strResult = oArchivo.sp_d_archivo(hdd_IdArchivoList.Value);
                        oBasic.AlertUserControl(null, null, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "d");
                        break;
                    default:
                        break;
                }
                LoadImageList();
                HideShowFileUpload(false);
                gvPBImages.HeaderRow.Focus();
            }
            catch (Exception)
            {

            }
        }
        protected void ucMsResponsable_Accept(object sender)
        {
            GetUsersColaborator();
        }
        protected void ucMsResponsable_Remove(object sender)
        {
            GetUsersColaborator();
        }
        protected void ucMsUpz_Accept(object sender)
        {
            txtlocalidadupz.Text = btnLocalidadUpz.Text = btnLocalidadUpz.ToolTip = txtlocalidadupz.ToolTip = ucMsUpz.ToString();
            upPBHeader.Update();
        }
        protected void ucMsUpz_Remove(object sender)
        {
            txtlocalidadupz.Text = btnLocalidadUpz.Text = btnLocalidadUpz.ToolTip = txtlocalidadupz.ToolTip = ucMsUpz.ToString();
            upPBHeader.Update();
        }
        #endregion

        #region Métodos Públicos
        public string Delete()
        {
            Int32.TryParse(hddIdBanco.Value, out int idBanco);
            return (idBanco == 0 ? "99999:No se identificó el proyecto a eliminar" : Delete(oBasic.fInt(hddIdBanco)));
        }
        public void Initialize()
        {
            cefec_inicio_construccion.StartDate = ce_fec_inicio_ventas.StartDate = new DateTime(2000, 1, 1);
            rv_fec_inicio_construccion.MinimumValue = rv_fec_inicio_ventas.MinimumValue = (new DateTime(2000, 1, 1)).ToString("yyyy-MM-dd");
            cefec_inicio_construccion.EndDate = ce_fec_inicio_ventas.EndDate = DateTime.Today.AddYears(50);
            rv_fec_inicio_construccion.MaximumValue = rv_fec_inicio_ventas.MaximumValue = (DateTime.Today.AddYears(50)).ToString("yyyy-MM-dd");
        }
        private void ClearControls()
        {
            oBasic.fValueTextBox(txte_area_bruta, "dec", "0");
            oBasic.fValueTextBox(txte_area_neta, "dec", "0");
            oBasic.fValueTextBox(txte_area_suelo_util, "dec", "0");
            oBasic.fValueTextBox(txte_cesion_parque, "dec", "0");
            oBasic.fValueTextBox(txte_cesion_equipamiento, "dec", "0");
            oBasic.fValueTextBox(txte_viviendas_vip, "int", "0");
            oBasic.fValueTextBox(txte_viviendas_vis, "int", "0");
            oBasic.fValueTextBox(txte_viviendas_novip, "int", "0");
            oBasic.fValueTextBox(txte_poblacion_beneficiaria, "int", "0");
        }
        public void LoadDetail()
        {            
            ClearControls();
            if (this.IdBanco != 0)
            {
                DataSet dSet = oBanco.sp_s_banco_consultar(IdBanco, 0);
                DataRow dRow = dSet.Tables[0].Rows[0];
                oBasic.fValueControls(pnlPBDetail, dRow, "");
                oBasic.fValueControls(pnlPBEdition, dRow, "");
                LoadGanttChart();
                LoadSliderImages();
                LoadImageList();
                LoadActor();
                LoadDropDownResponsibleUser(dRow);
                pnlAnexos.Visible = true;
                dvObservacionesPA.Visible = (dRow["idProyecto"] ?? "").ToString() != "";
            }
            else
            {
                oBasic.fClearControls(pnlPBEdition);
                oBasic.fClearControls(pnlAnexos);
                pnlAnexos.Visible = false;
                ddlEntidad.SetSelectedValues(null);
                ddlTratamiento.SetSelectedValues(null);
                ddlUpl.SetSelectedValues(null);
                gvPBImages.DataSource = null;
                gvPBImages.DataBind();
                LoadDropDownResponsibleUser(null);
                ucRepresentative.ActorID = 0;
                ucRepresentative.LoadGrid();
            }
            LoadLocalidadUpz();
            LoadResponsable();
            LoadDropDownEntidad();
            LoadDropDownTratamiento();
            LoadDropDownUPL();

            ViewControls();
            upPBHeader.Update();
        }
        public void LoadChart() {
            LoadGanttChart();
            upPBHeader.Update();
        }
        public string Save()
        {
            Int32.TryParse(hddIdBanco.Value, out int idBanco);
            string result = (idBanco == 0 ? Add() : Edit());
            SaveBancoEntidades();
            SaveBancoTratamientos();
            SaveBancoUpl();
            SaveLocalidadesUpz();
            SaveColaboradores();
            return result;
        }
        #endregion

        #region Métodos Privados
        private string Add()
        {
            string strResult = oBanco.sp_i_banco(
                oBasic.fInt(ddl_idtipo_proyecto),
                null,
                txte_nombre.Text,
                oBasic.fInt(ddl_idestado_proyecto),
                txte_descripcion.Text,
                null,
                "0",
                "0",
                null,
                oBasic.fInt(ddl_idinstrumento),
                oBasic.fInt(ddl_idvinculacion),
                oBasic.fDec(txte_poblacion_beneficiaria),
                oBasic.fDec(txte_area_bruta),
                oBasic.fDec(txte_area_neta),
                oBasic.fDec(txte_area_suelo_util),
                oBasic.fInt(txte_viviendas_vip),
                oBasic.fInt(txte_viviendas_vis),
                oBasic.fInt(txte_viviendas_novip),
                oBasic.fDec(txte_cesion_parque),
                oBasic.fDec(txte_cesion_equipamiento),
                oBasic.fDateTime(txte_fec_inicio_ventas),
                oBasic.fDateTime(txte_fec_inicio_construccion),
                null,
                oBasic.fInt(ddl_cod_usu_responsable));

            oBasic.AlertUserControl(null, null, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "u");

            if (strResult.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                IdBanco = Int32.Parse((strResult.Split(':'))[1]);

            return strResult;
        }
        private void ClearContents(Control control)
        {
            for (var i = 0; i < Session.Keys.Count; i++)
            {
                if (Session.Keys[i].Contains(control.ClientID))
                {
                    Session.Remove(Session.Keys[i]);
                    break;
                }
            }
        }
        private string Delete(string pIdBanco)
        {
            string strResult = oBanco.sp_d_banco(pIdBanco);
            oBasic.AlertUserControl(null, null, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "d");
            return strResult;
        }
        private string Edit()
        {
            string strResult = oBanco.sp_u_banco(
            oBasic.fInt(hddIdBanco),
            oBasic.fInt(ddl_idtipo_proyecto),
            null,   
            txte_nombre.Text,
            oBasic.fInt(ddl_idestado_proyecto),
            txte_descripcion.Text,
            null,
            "0",
            "0",
            null,
            oBasic.fInt(ddl_idinstrumento),
            oBasic.fInt(ddl_idvinculacion),
            oBasic.fDec(txte_poblacion_beneficiaria),
            oBasic.fDec(txte_area_bruta),
            oBasic.fDec(txte_area_neta),
            oBasic.fDec(txte_area_suelo_util),
            oBasic.fInt(txte_viviendas_vip),
            oBasic.fInt(txte_viviendas_vis),
            oBasic.fInt(txte_viviendas_novip),
            oBasic.fDec(txte_cesion_parque),
            oBasic.fDec(txte_cesion_equipamiento),
            oBasic.fDateTime(txte_fec_inicio_ventas),
            oBasic.fDateTime(txte_fec_inicio_construccion),
            oBasic.fInt(ddl_cod_usu_responsable));

            oBasic.AlertUserControl(null, null, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "u");
            return strResult;
        }
        private bool FileValidation()
        {
            ttNoImage.Visible = ttFileRequired.Visible = false;
            if (afuImage.HasFile)
            {
                string contentType = afuImage.ContentType;
                if (contentType == "image/jpeg" || contentType == "image/gif" || contentType == "image/png")
                {
                    return true;
                }
                else
                {
                    ClearContents(afuImage);
                    ttNoImage.Visible = true;
                }
            }
            else
            {
                if (hdd_IdArchivoList.Value == "0")
                {
                    ttFileRequired.Visible = true;
                }
            }
            upImage.Update();
            return false;
        }
        private void GetUsersColaborator()
        {
            int countUsers = (ucMsResponsable.ToString() + "") == "" ? 0 : ((ucMsResponsable.ToString() + "").Split(',')).Length;
            lblnumusersedit.Text = lblnumusers.Text = lblnumusers.ToolTip = lblnumusersedit.ToolTip = countUsers.ToString() + " Reg";
            btnresponsable.ToolTip = txtresponsable.ToolTip = countUsers == 0 ? "Usuarios colaboradores" : ucMsResponsable.ToString();
            upPBHeader.Update();
        }
        private void HideShowFileUpload(bool _visible)
        {
            dvImageActions.Visible = _visible;
            afuImage.Visible = _visible;
            gvPBImages.Visible = !_visible;
            ClearContents(afuImage);
            hdd_IdArchivoList.Value = "0";
            lblPreviousImage.Text = "";
            txt_DescripcionImagen.Text = "";
        }
        private void LoadActor()
        {
            hdd_idactor.Value = hdd_idactor.Value == string.Empty ? "0" : hdd_idactor.Value;
            ucRepresentative.ActorID = Int32.Parse(hdd_idactor.Value );
            ucRepresentative.LoadGrid();
            txtRepresentante.Text = ucRepresentative.GetActors();
            txteRepresentante.Text = ucRepresentative.GetActors();
        }
        private void LoadDropDowns()
        {
            LoadDropDownIdentidad(ddl_idtipo_proyecto, "72", "1, 3");
            LoadDropDownIdentidad(ddl_idestado_proyecto, "55");
            LoadDropDownIdentidad(ddl_idinstrumento, "51", ""); 
            LoadDropDownIdentidad(ddl_idvinculacion, "74");
        }
        private void LoadDropDownIdentidad(DropDownList ddl, string id, string filterIgnore = "")
        {
            DataView dv = oIdentidades.sp_s_identidad_id_categoria(id).Tables[0].DefaultView;
            dv.RowFilter = filterIgnore == "" ? "" : "IsNull(opcion_identidad, 0) not in (" + filterIgnore + ")";
            ddl.DataSource = dv;
            ddl.DataTextField = "nombre_identidad";
            ddl.DataValueField = "id_identidad";
            ddl.DataBind();
        }
        private void LoadDropDownEntidad()
        {
            DataSet ds = oIdentidades.sp_s_identidad_id_categoria("75");
            ddlEntidad.LoadListBox(ds);
            string entidades = string.Empty;

            DataSet dsEntidad = oBancoEntidades.sp_s_banco_entidades(hddIdBanco.Value);
            if (dsEntidad.Tables.Count > 0)
            {
                DataTable dt = dsEntidad.Tables[0];
                List<string> idEntidades = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    idEntidades.Add(dr["identidad"].ToString());
                    entidades += ", " + dr["entidad"].ToString();
                }
                ddlEntidad.SetSelectedValues(idEntidades);
                txtEntidades.Text = entidades == string.Empty ? "" : entidades.Substring(1);
            }
        }
        private void LoadDropDownTratamiento()
        {
            DataSet ds = oIdentidades.sp_s_identidad_id_categoria("21");
            ddlTratamiento.LoadListBox(ds);
            string tratamientos = string.Empty;

            DataSet dsTratamiento = oBancoTratamientos.sp_s_banco_tratamientos(hddIdBanco.Value);
            if (dsTratamiento.Tables.Count > 0)
            {
                DataTable dt = dsTratamiento.Tables[0];
                List<string> idTratamientos = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    idTratamientos.Add(dr["idtratamiento"].ToString());
                    tratamientos += ", " + dr["tratamiento"].ToString();
                }
                ddlTratamiento.SetSelectedValues(idTratamientos);
                txtTratamientos.Text = tratamientos == string.Empty ? "" : tratamientos.Substring(1);
            }
        }
        private void LoadDropDownUPL()
        {
            DataSet ds = oUPL.sp_s_upl();
            ddlUpl.LoadListBox(ds, "nombre", "idupl");
            string upls = string.Empty;

            DataSet dsUPL = oBancoUpl.sp_s_banco_upl(hddIdBanco.Value);
            if (dsUPL.Tables.Count > 0)
            {
                DataTable dt = dsUPL.Tables[0];
                List<string> idUPLs = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    idUPLs.Add(dr["idupl"].ToString());
                    upls += ", " + dr["upl"].ToString();
                }
                ddlUpl.SetSelectedValues(idUPLs);
                txtUpl.Text = upls == string.Empty ? "" : upls.Substring(1);
            }
        }
        private void LoadDropDownResponsibleUser(DataRow dRow)
        {
            DataSet ds = oUsuarios.sp_s_usuarios_filtro(12, Convert.ToInt32(oVar.prUserCod.ToString()));
            DataSet ds2 = ds.Clone();
            DataRow[] oDr = ds.Tables[0].Select(" cod_usuario in (" + (dRow != null ? "0" + (dRow["cod_usu_responsable"] ?? "0").ToString() : "0") + ", " + oVar.prUserCod.ToString() + ")");

            foreach (DataRow row in oDr)
            {
                ds2.Tables[0].ImportRow(row);
            }
            ddl_cod_usu_responsable.Items.Clear();
            ddl_cod_usu_responsable.DataSource = ds2;
            ddl_cod_usu_responsable.DataTextField = "nombre_completo";
            ddl_cod_usu_responsable.DataValueField = "cod_usuario";
            ddl_cod_usu_responsable.Items.Insert(0, new ListItem("--Seleccione opción", "0"));
            ddl_cod_usu_responsable.DataBind();


            ListItem li = ddl_cod_usu_responsable.Items.FindByValue(dRow != null ? (dRow["cod_usu_responsable"] ?? "0").ToString() : "0");
            ddl_cod_usu_responsable.SelectedValue = li != null ? li.Value : "0";
        }
        private void LoadGanttChart()
        {
            if (!Enabled)
            {
                string key = "Gantt";
                if (GanttChartDIV != null)
                {
                    DataSet dSet = oActividades.sp_s_bancoactividades_listar(IdBanco, p_activo: true);
                    DataTable dTable = dSet.Tables[0];

                    if (dTable.Rows.Count > 0)
                    {
                        StringBuilder scriptGantt = new StringBuilder();
                        scriptGantt.Append(" <script type='text/javascript'>");
                        scriptGantt.Append(" var g = new JSGantt.GanttChart('g', document.getElementById('" + GanttChartDIV.ClientID + "'), 'month'); ");
                        scriptGantt.Append(" g.setShowRes(0); ");
                        scriptGantt.Append(" g.setShowDur(0); ");
                        scriptGantt.Append(" g.setShowComp(0);");
                        scriptGantt.Append(" g.setCaptionType('None'); ");
                        scriptGantt.Append(" g.setShowStartDate(1); ");
                        scriptGantt.Append(" g.setShowEndDate(1); ");

                        foreach (DataRow dr in dTable.Rows)
                        {
                            string idActividad = dr["idbanco_actividad"].ToString();
                            string nombre = dr["nombre"].ToString();
                            string fec_inicio = null, fec_culminacion = null, fec_finalizacion = null;
                            if (DateTime.TryParse(dr["fec_inicio"].ToString(), out DateTime dfecha_inicio))
                            {
                                fec_inicio = dfecha_inicio.ToShortDateString();
                            }
                            if (DateTime.TryParse(dr["fec_culminacion"].ToString(), out DateTime dfecha_culminacion))
                            {
                                fec_culminacion = dfecha_culminacion.ToShortDateString();
                            }
                            if (DateTime.TryParse(dr["fec_finalizacion"].ToString(), out DateTime dfecha_finalizacion))
                            {
                                fec_finalizacion = dfecha_finalizacion.ToShortDateString();
                            }
                            int.TryParse(dr["semaforo"].ToString(), out int semaforo);

                            if (fec_inicio != null && fec_culminacion != null)
                            {
                                string colorlinea = semaforo > 0 ? Color.VERDE :
                                                    semaforo < 0 ? Color.ROJO : Color.AMARILLO;

                                scriptGantt.Append(" g.AddTaskItem(new JSGantt.TaskItem(" + idActividad + ", '" + nombre + "', '" + fec_inicio + "', '" + fec_culminacion + "', '" + colorlinea + "', '', 0, '', 0, 0, 0, 1)); ");
                            }
                        }

                        scriptGantt.Append(" g.Draw(); ");
                        scriptGantt.Append(" g.DrawDependencies(); ");
                        scriptGantt.Append("</script>");

                        if (!Page.ClientScript.IsStartupScriptRegistered(key))
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), key, scriptGantt.ToString(), false);
                        }
                    }
                }
            }
        }
        private void LoadImageList()
        {
            HideShowFileUpload(false);

            gvPBImages.DataSource = ((DataSet)oArchivo.sp_s_archivos_listar_hijos(hdd_idArchivo.Value, ((int)tipoArchivo.IMG_BP).ToString()));
            gvPBImages.DataBind();
        }
        private void LoadLocalidadUpz()
        {
            ucMsUpz.Data = oUPZ.sp_s_banco_upz(IdBanco.ToString()).Tables[0];

            ucMsUpz.Title = "Información de la Localidad y UPZ";
            ucMsUpz.ParentName = "Localidad";
            ucMsUpz.ChildName = "UPZ";
            ucMsUpz.TypeReference = 1;
            ucMsUpz.Enabled = Enabled;
            txtlocalidadupz.Text = btnLocalidadUpz.Text = btnLocalidadUpz.ToolTip = txtlocalidadupz.ToolTip = ucMsUpz.ToString();
        }
        private void LoadResponsable()
        {
            ucMsResponsable.Data = oBanco.sp_s_banco_colaboradores(IdBanco.ToString()).Tables[0];

            ucMsResponsable.Title = "Colaboradores del proyectos";
            ucMsResponsable.ParentName = "Usuarios";
            ucMsResponsable.ChildName = "";
            ucMsResponsable.TypeReference = 2;
            bool isAadmin = oPermisos.TienePermisosAccion(clConstantes.Section.PROYECTO_ESTRATEGICO, clConstantes.Accion.INSERTAR, oVar.prUserCod.ToString());
            ucMsResponsable.Enabled = Enabled && (ddl_cod_usu_responsable.SelectedValue == oVar.prUserCod.ToString() || isAadmin);
            GetUsersColaborator();
        }
        private void LoadSliderImages()
        {
            if (!Enabled)
            {
                DataSet ds = ((DataSet)oArchivo.sp_s_archivos_listar_hijos(hdd_idArchivo.Value, ((int)tipoArchivo.IMG_BP).ToString()));
                List<Images> lstImages = new List<Images>();
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        Images item = new Images(dr["ruta"].ToString());
                        lstImages.Add(item);
                    }
                }
                siCarousel.SetSlides(lstImages);
            }
        }
        private void SaveColaboradores() //TODO: VERIFICAR EL GUARDADO DE ERRORES
        {
            DataTable dt = ucMsResponsable.Data;
            if (dt.Rows.Count > 0)
            {
                string strResult = oBanco.sp_d_banco_usuario_colaborador(IdBanco);
                if (strResult.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        strResult = oBanco.sp_i_banco_usuario_colaborador(IdBanco, dr["id"].ToString());
                    }
                }
            }
        }
        private void SaveBancoEntidades()
        {
            string idEntidades = ddlEntidad.GetSelectedValues();
            if (idEntidades.Split(',').Length > 0)
            {
                string strResult = oBancoEntidades.sp_d_banco_entidades(IdBanco);
                oBasic.AlertUserControl(null, null, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name + ".delete", "d");
                foreach (string idEntidad in idEntidades.Split(','))
                {
                    if (idEntidad.Trim() != "")
                    {
                        strResult = oBancoEntidades.sp_iu_banco_entidades(IdBanco, idEntidad);
                        oBasic.AlertUserControl(null, null, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name + ".insert", "i");
                    }
                }
            }
        }
        private void SaveLocalidadesUpz()//TODO: VERIFICAR EL GUARDADO DE ERRORES
        {
            DataTable dt = ucMsUpz.Data;
            if (dt.Rows.Count > 0)
            {
                string strResult = oUPZ.sp_d_banco_upz(IdBanco);
                if (strResult.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        strResult = oUPZ.sp_iu_banco_upz(IdBanco, dr["id"].ToString());
                    }
                }
            }
        }
        private void SaveBancoTratamientos()
        {
            string idTratamientos = ddlTratamiento.GetSelectedValues();
            if (idTratamientos.Split(',').Length > 0)
            {
                string strResult = oBancoTratamientos.sp_d_banco_tratamientos(IdBanco);
                oBasic.AlertUserControl(null, null, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name + ".delete", "d");
                foreach (string idTratamiento in idTratamientos.Split(','))
                {
                    if (idTratamiento.Trim() != "")
                    {
                        strResult = oBancoTratamientos.sp_iu_banco_tratamientos(IdBanco, idTratamiento);
                        oBasic.AlertUserControl(null, null, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name + ".insert", "i");
                    }
                }
            }
        }
        private void SaveBancoUpl()
        {
            string idUpls = ddlUpl.GetSelectedValues();
            if (idUpls.Split(',').Length > 0)
            {
                string strResult = oBancoUpl.sp_d_banco_upl(IdBanco);
                oBasic.AlertUserControl(null, null, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name + ".delete", "d");
                foreach (string idUpl in idUpls.Split(','))
                {
                    if (idUpl.Trim() != "")
                    {
                        strResult = oBancoUpl.sp_iu_banco_upl(IdBanco, idUpl);
                        oBasic.AlertUserControl(null, null, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name + ".insert", "i");
                    }
                }
            }
        }
        private string SaveImageDataBase(string pathFile)
        {
            string strResult;
            hdd_idArchivo.Value = hdd_idArchivo.Value == "" ? "0" : hdd_idArchivo.Value;
            if (hdd_IdArchivoList.Value == "0")
            {
                strResult = oArchivo.sp_i_archivo(int.Parse(hdd_idArchivo.Value), (int)tipoArchivo.IMG_BP, tipoArchivo.IMG_BP.ToString(), afuImage.FileName, pathFile, txt_DescripcionImagen.Text);
                if (oBasic.AlertUserControl(null, null, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name+".archivo", "i"))
                {
                    if (hdd_idArchivo.Value == "0")
                    {
                        hdd_idArchivo.Value = strResult.Split(':')[1];
                        strResult = oArchivo.sp_u_archivo_referencia(hdd_idArchivo.Value, hddIdBanco.Value, 1);
                        oBasic.AlertUserControl(null, null, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name+".referencia", "i");
                    }
                }
            }
            else
            {
                strResult = oArchivo.sp_u_archivo(Convert.ToInt32(hdd_IdArchivoList.Value), Convert.ToInt32(hdd_idArchivo.Value),
                                                (int)tipoArchivo.IMG_BP, tipoArchivo.IMG_BP.ToString(), afuImage.FileName, pathFile, txt_DescripcionImagen.Text);
                oBasic.AlertUserControl(null, null, strResult, _SOURCEPAGE, System.Reflection.MethodBase.GetCurrentMethod().Name, "d");
            }

            return strResult;
        }
        private string SaveImageFile()
        {
            ttNoImage.Visible = ttFileRequired.Visible = false;
            string pathFile = "";
            if (afuImage.HasFile)
            {
                string contentType = afuImage.ContentType;
                if (contentType == "image/jpeg" || contentType == "image/gif" || contentType == "image/png")
                {
                    string fileName = "BP_" + Guid.NewGuid() + Path.GetExtension(afuImage.FileName).ToLower();

                    pathFile = Path.Combine(oVar.prPathPrediosVisitas.ToString(), "BancoProyectos");

                    bool Resultado = oFile.fVerificarPath(pathFile);
                    if (Resultado)
                    {
                        try
                        {
                            pathFile = Path.Combine(pathFile, fileName);
                            oFile.fBorrarFile(pathFile);
                            afuImage.SaveAs(pathFile);
                        }
                        catch (Exception e)
                        {
                            oLog.RegistrarLogError("Error subiendo imagen " + e.Message + ":" + afuImage.FileName + ":::" + afuImage.PostedFile.ContentLength, _SOURCEPAGE, "btnAccept_Click");
                        }
                    }
                    else
                        oLog.RegistrarLogError("Error subiendo imagen " + afuImage.FileName + ":::" + afuImage.PostedFile.ContentLength, _SOURCEPAGE, "btnAccept_Click");
                }
                else
                {
                    ttNoImage.Visible = true;
                }
            }
            else
            {
                ttFileRequired.Visible = false;
            }
            upImage.Update();
            return pathFile;
        }
        private void ViewControls()
        {
            hdd_idProyecto.Value = hdd_idProyecto.Value == "" ? "0" : hdd_idProyecto.Value;
            pnlPBDetail.Visible = !Enabled;
            pnlPBSliderImages.Visible = !Enabled;
            pnlPBEdition.Visible = Enabled;
            pnlAnexos.Visible = Enabled && IdBanco > 0;
            oBasic.fEditControls(pnlPBEdition, Enabled && hdd_idProyecto.Value == "0");
            ddlEntidad.Enabled = true;
            ddlTratamiento.Enabled = true;
            ddlUpl.Enabled = true;
            txte_codigo.Enabled = false;
            rfv_idtipo_proyecto.Enabled = rfv_nombre.Enabled = rfv_idestado_proyecto.Enabled = rev_fec_inicio_ventas.Enabled = rv_fec_inicio_ventas.Enabled
                = rev_fec_inicio_construccion.Enabled = rv_fec_inicio_construccion.Enabled = Enabled && hdd_idProyecto.Value == "0";
            lblPBSubHeader_1.Visible = Enabled && hdd_idProyecto.Value == "0";
            dvRepresentante.Visible = !(Enabled && hdd_idProyecto.Value == "0");
            bool isAadmin = oPermisos.TienePermisosAccion(clConstantes.Section.PROYECTO_ESTRATEGICO, clConstantes.Accion.INSERTAR, oVar.prUserCod.ToString());
            ddl_cod_usu_responsable.Enabled = Enabled && (ddl_cod_usu_responsable.SelectedValue == oVar.prUserCod.ToString() || ddl_cod_usu_responsable.SelectedValue == "0" || isAadmin);
        }
        #endregion

    }
}