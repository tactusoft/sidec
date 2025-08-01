using GLOBAL.CARTATERMINOSTEMPLATE;
using GLOBAL.CONCEPTOTEMPLATE;
using GLOBAL.CONST;
using GLOBAL.DAL;
using GLOBAL.LOG;
using GLOBAL.PERMISOS;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using GLOBAL.VISITATEMPLATE;
using OfficeOpenXml;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cnsSection = GLOBAL.CONST.clConstantes.Section;
using tipoArchivo = GLOBAL.CONST.clConstantes.TipoArchivo;

namespace SIDec
{
    public partial class Predios : System.Web.UI.Page
    {
        #region--------------------------------------------------------------------VARIABLES
        #region OBJETOS
        readonly ACTOSADMIN_DAL oActosAdm = new ACTOSADMIN_DAL();
        readonly AFECTACIONES_DAL oAfectaciones = new AFECTACIONES_DAL();
        readonly BARRIOS_DAL oBarrios = new BARRIOS_DAL();
        readonly CONCEPTOS_DAL oConceptos = new CONCEPTOS_DAL();
        readonly DOCUMENTOS_DAL oDocumentos = new DOCUMENTOS_DAL();
        readonly IDENTIDADES_DAL oIdentidades = new IDENTIDADES_DAL();
        readonly LICENCIAS_DAL oLicencias = new LICENCIAS_DAL();
        readonly PD_OBSERVACIONES_DAL oObservaciones = new PD_OBSERVACIONES_DAL();
        readonly PREDIOS_DAL oPredios = new PREDIOS_DAL();
        readonly PREDIOSDECLARADOS_DAL oPrediosDeclarados = new PREDIOSDECLARADOS_DAL();
        readonly PREDIOSPROPIETARIOS_DAL oPrediosPropietarios = new PREDIOSPROPIETARIOS_DAL();
        readonly PRESTAMOS_DAL oPrestamos = new PRESTAMOS_DAL();
        readonly PROPIETARIOS_DAL oPropietarios = new PROPIETARIOS_DAL();
        readonly USUARIOS_DAL oUsuarios = new USUARIOS_DAL();
        readonly VISITAS_DAL oVisitas = new VISITAS_DAL();

        readonly clGlobalVar oVar = new clGlobalVar();
        readonly clUtil oUtil = new clUtil();
        readonly clLog oLog = new clLog();
        readonly clPermisos oPermisos = new clPermisos();
        readonly clFile oFile = new clFile();
        readonly clVisitaTemplate oVisitaTemplate = new clVisitaTemplate();
        readonly clVisitaTemplateOld oVisitaTemplateOld = new clVisitaTemplateOld();
        readonly clConceptoTemplate oConceptoTemplate = new clConceptoTemplate();
        readonly clCartaTerminosTemplate oCartaTerminosTemplate = new clCartaTerminosTemplate();
        readonly clBasic oBasic = new clBasic();
        #endregion

        #region---CONSTANTES
        private const string _SOURCEPAGE = "Predios";

        private const string _PATHFORMATOSERR = "No existe directorio de formatos: {0}.";
        private const string _FILENOEXISTE = "No existe el formato: {0}.";

        private const string _MSGCONTADORREGISTROS = "Registro {0} de {1}";

        private const string _DIVMSGPREDIOS = "DivMsgPredios";
        private const string _DIVMSGPREDIOSDEC = "DivMsgPrediosDec";
        private const string _DIVMSGDOCUMENTOS = "DivMsgDocumentos";
        private const string _DIVMSGPRESTAMOS = "DivMsgPrestamos";
        private const string _DIVMSGPREDIOSPROPIETARIOS = "DivMsgPrediosPropietarios";
        private const string _DIVMSGPROPIETARIOS = "DivMsgPropietarios";
        private const string _DIVMSGVISITAS = "DivMsgVisitas";
        private const string _DIVMSGLICENCIAS = "DivMsgLicencias";
        private const string _DIVMSGCONCEPTOS = "DivMsgConceptos";
        private const string _DIVMSGACTOSADM = "DivMsgActosAdm";

        //concepto técnico
        private const string _CONCEPTO_39 = "Evaluar los factores técnicos, urbanísticos y arquitectónicos del predio en estudio, para determinar si cumplen o no con las calidades exigidas dentro de la Declaratoria " +
              "de {0}, Acuerdo No. {1} de {2}, las cuales son acordes al Plan de Ordenamiento Territorial, el Plan Nacional de Desarrollo, la Ley No. 388 de 1997, y demás normas urbanísticas aplicables en " +
              "la materia para Vivienda de Interés Social y Vivienda de Interés Prioritario.";

        //Evaluación técnica previa a la enajenación forzosa en pública subasta
        private const string _CONCEPTO_40 = "Evaluar las condiciones del predio en estudio, para determinar si vencido el término del artículo 52 de la Ley 388 de 1997, conserva las características exigidas en el " +
              "artículo {0} del Acuerdo Distrital No. {1} del {2} y las demás normas urbanísticas aplicables en la materia, que permitan el desarrollo proyectos de Vivienda de Interés Social y Prioritario.";

        //Alcance a evaluación técnica previa a la enajenación forzosa en pública subasta
        private const string _CONCEPTO_38 = "Ampliar los factores técnicos, urbanísticos y arquitectónicos de la evaluación técnica previa a la enajenación forzosa en pública subasta de fecha {0} de {1} de {2}, " +
               "en desarrollo del seguimiento a los predios declarados de {3}.";

        private const string _CONCEPTO_SD_1 = "Informe técnico de visita de campo del {0}, realizado al predio objeto de estudio por la Subdirección de Gestión de Suelo de la Secretaría Distrital de Hábitat.";
        private const string _CONCEPTO_SD_2 = "Certificado catastral del predio objeto de estudio, expedido el {0}, a través de {1}.";
        private const string _CONCEPTO_SD_3 = "Matricula inmobiliaria del predio objeto de estudio, expedida el {0}, a través de la {1}.";
        private const string _CONCEPTO_SD_4 = "Consulta electrónica del {0}, realizada a través del módulo de “Consultas electrónicas Consulta Lic. de Urbanismo y Construcción”, de la Ventanilla Única de Construcción – VUC.";
        private const string _CONCEPTO_SD_5 = "Manual de procesos y procedimientos “Seguimiento al cumplimiento de la declaratoria de desarrollo o construcción prioritaria” PM02-PR06.";
        private readonly string[] _CONCEPTO_SD = new string[5] { _CONCEPTO_SD_1, _CONCEPTO_SD_2, _CONCEPTO_SD_3, _CONCEPTO_SD_4, _CONCEPTO_SD_5 };

        //**********************************************
        private const string _TIPOVISITA_ACTA = "62";
        private const string _TIPOVISITA_INFORMETECNICO = "64";
        #endregion

        string _FORMATOSORIGEN;
        string _FORMATOSDESTINO;
        string _FORMATOF035;
        string _FORMATOF0379;
        string[] ArchivosFoto;

        bool bPageChanged = false;
        bool HayCambiosPredios = false;
        bool HayCambiosPrediosDec = false;
        bool HayCambiosDocumentos = false;
        bool HayCambiosPropietarios = false;
        bool HayCambiosPrediosPropietarios = false;
        bool HayCambiosObservaciones = false;
        bool HayCambiosVisitas = false;
        bool esPermitido = false;

        private DataSet dsColaborators; 
        #endregion

        #region--------------------------------------------------------------------PAGE
        protected void Page_Load(object sender, EventArgs e)
        {
            _FORMATOSORIGEN = oVar.prPathFormatosOrigen.ToString();
            _FORMATOSDESTINO = oVar.prPathFormatosDestino.ToString();
            _FORMATOF035 = oVar.prDocFormatoFO35.ToString();
            _FORMATOF0379 = oVar.prDocFormatoFO379.ToString();

            Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "SetCursor('" + txtBuscador.ClientID + "');", true);

            btnConfirmarPredios.Attributes.Add("onclick", "document.getElementById('" + btnConfirmarPredios.ClientID + "').disabled=true;" + ClientScript.GetPostBackEventReference(btnConfirmarPredios, "Click") + "; return false;");
            btnConfirmarDocumentos.Attributes.Add("onclick", "document.getElementById('" + btnConfirmarDocumentos.ClientID + "').disabled=true;" + ClientScript.GetPostBackEventReference(btnConfirmarDocumentos, "Click") + "; return false;");
            btnConfirmarPrestamos.Attributes.Add("onclick", "document.getElementById('" + btnConfirmarPrestamos.ClientID + "').disabled=true;" + ClientScript.GetPostBackEventReference(btnConfirmarPrestamos, "Click") + "; return false;");
            btnConfirmarPrediosPropietarios.Attributes.Add("onclick", "document.getElementById('" + btnConfirmarPrediosPropietarios.ClientID + "').disabled=true;" + ClientScript.GetPostBackEventReference(btnConfirmarPrediosPropietarios, "Click") + "; return false;");
            btnConfirmarPropietarios.Attributes.Add("onclick", "document.getElementById('" + btnConfirmarPropietarios.ClientID + "').disabled=true;" + ClientScript.GetPostBackEventReference(btnConfirmarPropietarios, "Click") + "; return false;");
            btnConfirmarObservaciones.Attributes.Add("onclick", "document.getElementById('" + btnConfirmarObservaciones.ClientID + "').disabled=true;" + ClientScript.GetPostBackEventReference(btnConfirmarObservaciones, "Click") + "; return false;");
            btnConfirmarVisitas.Attributes.Add("onclick", "document.getElementById('" + btnConfirmarVisitas.ClientID + "').disabled=true;" + ClientScript.GetPostBackEventReference(btnConfirmarVisitas, "Click") + "; return false;");
            btnConfirmarLicencias.Attributes.Add("onclick", "document.getElementById('" + btnConfirmarLicencias.ClientID + "').disabled=true;" + ClientScript.GetPostBackEventReference(btnConfirmarLicencias, "Click") + "; return false;");
            btnConfirmarConceptos.Attributes.Add("onclick", "document.getElementById('" + btnConfirmarConceptos.ClientID + "').disabled=true;" + ClientScript.GetPostBackEventReference(btnConfirmarConceptos, "Click") + "; return false;");
            btnConfirmarActosAdm.Attributes.Add("onclick", "document.getElementById('" + btnConfirmarActosAdm.ClientID + "').disabled=true;" + ClientScript.GetPostBackEventReference(btnConfirmarActosAdm, "Click") + "; return false;");

            if (!IsPostBack)
            {
                this.Page.Form.Enctype = "multipart/form-data";

                ViewState["CriterioBuscar"] = "";
                ViewState["AccionFinal"] = "";
                ViewState["RealIndexPredios"] = "0";
                ViewState["RealIndexPrediosDec"] = "0";
                ViewState["RealIndexDocumentos"] = "0";
                ViewState["RealIndexPrestamos"] = "0";
                ViewState["RealIndexPrediosPropietarios"] = "0";
                ViewState["RealIndexPropietarios"] = "0";
                ViewState["RealIndexObservaciones"] = "0";
                ViewState["RealIndexVisitas"] = "0";
                ViewState["RealIndexLicencias"] = "0";
                ViewState["RealIndexConceptos"] = "0";
                ViewState["RealIndexActosAdm"] = "0";
                ViewState["IndexFoto"] = "0";

                txtBuscador.Focus();

                this.ViewState["SortExpression"] = "chip";
                this.ViewState["SortDirection"] = "ASC";

                fPrediosLoadGV(ViewState["CriterioBuscar"].ToString());

                fLoadDropDowns();
                fBotonesAccionesOcultar();

                oVar.prDSBarriosInfo = oBarrios.sp_s_barrios_info();
                //---------------------------------------------------------------------------------------------------------------------------------------------------
                //Para cada Item de la pagina
                fBotonesNavegacionEstado(divPrediosNavegacion, false);
                fBotonesNavegacionEstado(divPrediosDecNavegacion, false);
                fBotonesNavegacionEstado(divDocumentosNavegacion, false);
                fBotonesNavegacionEstado(divPrestamosNavegacion, false);
                fBotonesNavegacionEstado(divPrediosPropietariosNavegacion, false);
                fBotonesNavegacionEstado(divPropietariosNavegacion, false);
                fBotonesNavegacionEstado(divObservacionesNavegacion, false);
                fBotonesNavegacionEstado(divVisitasNavegacion, false);
                fBotonesNavegacionEstado(divLicenciasNavegacion, false);
                fBotonesNavegacionEstado(divConceptosNavegacion, false);
                fBotonesNavegacionEstado(divActosAdmNavegacion, false);
                //---------------------------------------------------------------------------------------------------------------------------------------------------

                fPrediosLoadGVExterno();
            }
        }

        protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e) //altura del container
        {
            upConceptos.Update();

            if (((AjaxControlToolkit.TabContainer)sender).ActiveTab.ID.ToString().Contains("Visita"))
                Session["PrediosVisitasVerFotos"] = "PV_VF";
            else
                Session["PrediosVisitasVerFotos"] = "";

            if (((AjaxControlToolkit.TabContainer)sender).ActiveTab.ID.ToString().Contains("tpDocumentos"))
                TabContainer1.Height = 440;
            else if (((AjaxControlToolkit.TabContainer)sender).ActiveTab.ID.ToString().Contains("tpPrestamos"))
                TabContainer1.Height = 360;
            else if (((AjaxControlToolkit.TabContainer)sender).ActiveTab.ID.ToString().Contains("tpAfectaciones"))
                TabContainer1.Height = 320;
            else if (((AjaxControlToolkit.TabContainer)sender).ActiveTab.ID.ToString().Contains("tpPropietarios"))
                TabContainer1.Height = 600;
            else if (((AjaxControlToolkit.TabContainer)sender).ActiveTab.ID.ToString().Contains("tpObservaciones"))
                TabContainer1.Height = 360;
            else if (((AjaxControlToolkit.TabContainer)sender).ActiveTab.ID.ToString().Contains("tpVisitas"))
            {
                TabContainer1.Height = 670;
                TabContainerVisitas.Height = 350;
            }
            else if (((AjaxControlToolkit.TabContainer)sender).ActiveTab.ID.ToString().Contains("tpLicencias"))
                TabContainer1.Height = 490;
            else if (((AjaxControlToolkit.TabContainer)sender).ActiveTab.ID.ToString().Contains("tpConceptos"))
            {
                TabContainer1.Height = 680;
                TabContainerConceptos.Height = 360;
            }
            else if (((AjaxControlToolkit.TabContainer)sender).ActiveTab.ID.ToString().Contains("tpActAdmin"))
                TabContainer1.Height = 440;
            else if (((AjaxControlToolkit.TabContainer)sender).ActiveTab.ID.ToString().Contains("tpInteresados")
                || (((AjaxControlToolkit.TabContainer)sender).ActiveTab.ID.ToString().Contains("tpFichaPredial")))
                TabContainer1.Height = 300;
            else if (((AjaxControlToolkit.TabContainer)sender).ActiveTab.ID.ToString().Contains("tbCartas"))
                TabContainer1.Height = 590;
        }

        protected void TabContainerVisitas_ActiveTabChanged(object sender, EventArgs e)
        {

        }

        protected void TabContainerConceptos_ActiveTabChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region--------------------------------------------------------------------BUTTONS
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ViewState["CriterioBuscar"] = txtBuscador.Text.Trim();
            fPrediosLoadGV(ViewState["CriterioBuscar"].ToString());
            fActivarVistaGrid(mvPredios, btnPrediosAccionFinal, btnPrediosCancelar);
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Session["Retorno.Proyecto.Page"] = "PP";
            Response.Redirect(Session["Retorno.Predios.Origen"].ToString());
        }
        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            bool VistaGrid = true;

            //PREDIOS
            if (ViewState["AccionFinal"].ToString().Substring(0, 3) == "pre")
            {
                switch (ViewState["AccionFinal"].ToString().Substring(3))
                {
                    case "Editar":
                        fPrediosUpdate();
                        break;
                }
                fPrediosLoadGV(ViewState["CriterioBuscar"].ToString());
                oUtil.fSetIndex2(gvPredios, Convert.ToInt16(ViewState["RealIndexPredios"]));
            }

            //PREDIOS DECLARADOS
            else if (ViewState["AccionFinal"].ToString().Substring(0, 3) == "dec")
            {
                switch (ViewState["AccionFinal"].ToString().Substring(3))
                {
                    case "Editar":
                        fPrediosDecUpdate();
                        break;
                }
                fPrediosDecLoadGV(txt_chip_PrediosDec.Text);
                oUtil.fSetIndex2(gvPrediosDec, Convert.ToInt16(ViewState["RealIndexPrediosDec"]));
            }

            //DOCUMENTOS
            else if (ViewState["AccionFinal"].ToString().Substring(0, 3) == "doc")
            {

        //if (fValidarFolios())
        //{
        switch (ViewState["AccionFinal"].ToString().Substring(3))
        {
          case "Editar":
            if (fValidarFolios())
            {
              fDocumentosUpdate();
              fDocumentosLimpiarDetalle();
            }
            break;
          case "Agregar":
            if (fValidarFolios())
            {
              fDocumentosInsert();
              fDocumentosLimpiarDetalle();
            }
            break;
          case "Eliminar":
            fDocumentosDelete();
            break;
          case "Reordenar":
            fDocumentosReorder();
            break;
          case "MoveUp":
            fDocumentosMove(-1);
            break;
          case "MoveDown":
            fDocumentosMove(1);
            break;
        }
        fDocumentosLoadGV(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
        oUtil.fSetIndex2(gvDocumentos, Convert.ToInt16(ViewState["RealIndexDocumentos"]));
      }

            //PRESTAMOS
            else if (ViewState["AccionFinal"].ToString().Substring(0, 3) == "prs")
            {
                switch (ViewState["AccionFinal"].ToString().Substring(3))
                {
                    case "Editar":
                        fPrestamosUpdate();
                        break;
                    case "Agregar":
                        fPrestamosInsert();
                        break;
                    case "Agregar2":
                        fPrestamosInsertLote();
                        break;
                }
                fPrestamosLoadGV(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                oUtil.fSetIndex2(gvPrestamos, Convert.ToInt16(ViewState["RealIndexPrestamos"]));
            }

            //PREDIOS PROPIETARIOS
            else if (ViewState["AccionFinal"].ToString().Substring(0, 3) == "prp")
            {
                switch (ViewState["AccionFinal"].ToString().Substring(3))
                {
                    case "Editar":
                        fPrediosPropietariosUpdate();
                        break;
                    case "Agregar":
                        fPrediosPropietariosInsert();
                        break;
                    case "Eliminar":
                        fPrediosPropietariosDelete();
                        break;
                }
                fPrediosPropietariosLoadGV(gvPrediosDec.SelectedRow.Cells[1].Text);
                oUtil.fSetIndex2(gvPrediosPropietarios, Convert.ToInt16(ViewState["RealIndexPrediosPropietarios"]));
            }

            //PROPIETARIOS
            else if (ViewState["AccionFinal"].ToString().Substring(0, 3) == "pro")
            {
                switch (ViewState["AccionFinal"].ToString().Substring(3))
                {
                    case "Editar":
                        fPropietariosUpdate();
                        break;
                    case "Agregar":
                        fPropietariosInsert();
                        break;
                    case "Eliminar":
                        fPropietariosDelete();
                        break;
                }
                fPrediosPropietariosLoadGV(gvPrediosDec.SelectedRow.Cells[1].Text);
                oUtil.fSetIndex2(gvPropietarios, Convert.ToInt16(ViewState["RealIndexPropietarios"]));
            }

            //OBSERVACIONES
            else if (ViewState["AccionFinal"].ToString().Substring(0, 3) == "obs")
            {
                switch (ViewState["AccionFinal"].ToString().Substring(3))
                {
                    case "Editar":
                        fObservacionesUpdate();
                        break;
                    case "Agregar":
                        fObservacionesInsert();
                        break;
                    case "Eliminar":
                        fObservacionesDelete();
                        break;
                }
                fObservacionesLoadGV(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                oUtil.fSetIndex2(gvObservaciones, Convert.ToInt16(ViewState["RealIndexObservaciones"]));
            }

            //VISITAS
            else if (ViewState["AccionFinal"].ToString().Substring(0, 3) == "vis")
            {
                switch (ViewState["AccionFinal"].ToString().Substring(3))
                {
                    case "Editar":
                        fVisitasUpdate();
                        break;
                    case "Agregar":
                        fVisitasInsert();
                        break;
                    case "Eliminar":
                        fVisitasDelete();
                        break;
                }
                fVisitasLoadGV(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                oUtil.fSetIndex2(gvVisitas, Convert.ToInt16(ViewState["RealIndexVisitas"]));
            }

            //LICENCIAS
            else if (ViewState["AccionFinal"].ToString().Substring(0, 3) == "lic")
            {
                switch (ViewState["AccionFinal"].ToString().Substring(3))
                {
                    case "Editar":
                        fLicenciasUpdate();
                        break;
                    case "Agregar":
                        fLicenciasInsert();
                        break;
                    case "Eliminar":
                        fLicenciasDelete();
                        break;
                }
                fLicenciasLoadGV(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                oUtil.fSetIndex2(gvLicencias, Convert.ToInt16(ViewState["RealIndexLicencias"]));
            }

            //CONCEPTOS
            else if (ViewState["AccionFinal"].ToString().Substring(0, 3) == "con")
            {
                switch (ViewState["AccionFinal"].ToString().Substring(3))
                {
                    case "Editar":
                        fConceptosUpdate();
                        break;
                    case "Agregar":
                        fConceptosInsert();
                        break;
                    case "Eliminar":
                        fConceptosDelete();
                        break;
                }
                fConceptosLoadGV(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                oUtil.fSetIndex2(gvConceptos, Convert.ToInt16(ViewState["RealIndexConceptos"]));
            }

            //ACTOS ADMINISTRATIVOS
            else if (ViewState["AccionFinal"].ToString().Substring(0, 3) == "adm")
            {
                switch (ViewState["AccionFinal"].ToString().Substring(3))
                {
                    case "Editar":
                        fActosAdmUpdate();
                        break;
                    case "Agregar":
                        fActosAdmInsert();
                        break;
                    case "Eliminar":
                        fActosAdmDelete();
                        break;
                }
                fActosAdmLoadGV(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                fPrediosDecLoadGV(gvPredios.SelectedDataKey.Value.ToString());
                ViewState["index_predios_dec"] = gvPrediosDec.SelectedIndex;
                gvPrediosDec.SelectedIndex = Convert.ToInt16(ViewState["index_predios_dec"].ToString());
                oUtil.fSetIndex2(gvActosAdm, Convert.ToInt16(ViewState["RealIndexActosAdm"]));
            }

            if (VistaGrid)
            {
                mvPredios.ActiveViewIndex = 0;
                mvPrediosDec.ActiveViewIndex = 0;
                mvPrestamos.ActiveViewIndex = 0;
                mvPrediosPropietarios.ActiveViewIndex = 0;
                mvPropietarios.ActiveViewIndex = 0;
                mvObservaciones.ActiveViewIndex = 0;
                mvVisitas.ActiveViewIndex = 0;
                mvLicencias.ActiveViewIndex = 0;
                mvConceptos.ActiveViewIndex = 0;
                mvActosAdm.ActiveViewIndex = 0;

                fBotonesAccionesOcultar();
            }
        }

        #region--------PREDIOS
        protected void btnPrediosAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            fBotonAccionFinalEstado(btnPrediosAccionFinal, btnPrediosCancelar, true);
            ViewState["AccionFinal"] = "pre" + btnAccionSource.CommandName;
            txt_chip.Focus();

            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    fPrediosDetalle();
                    fPrediosEstadoDetalle(true);
                    hfEvtGVPredios.Value = btnAccionSource.CommandArgument;
                    break;
                case "Agregar":
                    fPrediosLimpiarDetalle();
                    fPrediosEstadoDetalle(true);
                    mvPredios.ActiveViewIndex = 1;
                    break;
                case "Eliminar":
                    fPrediosDetalle();
                    break;
            }
            upPrediosFoot.Update();
        }

        protected void btnPrediosCancelar_Click(object sender, EventArgs e)
        {
            btnPrediosVista_Click(sender, e);
        }

        protected void btnPrediosNavegacion_Click(object sender, EventArgs e)
        {
            int iIndex = 0;

            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    iIndex = Convert.ToInt16(ViewState["RealIndexPredios"]) - 1;
                    break;
                case "Next":
                    iIndex = Convert.ToInt16(ViewState["RealIndexPredios"]) + 1;
                    break;
                case "Last":
                    iIndex = ((DataSet)oVar.prDSPrediosFiltro).Tables[0].Rows.Count - 1;
                    break;
            }
            ViewState["RealIndexPredios"] = iIndex.ToString();
            fPrediosEstadoDetalle(false);
            fPrediosDetalle();
            fPrediosDecLoadGV(((DataSet)oVar.prDSPrediosFiltro).Tables[0].Rows[iIndex][0].ToString());
        }

        protected void btnPrediosVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);

            if (cmdArg == 0)
                fActivarVistaGrid(mvPredios, btnPrediosAccionFinal, btnPrediosCancelar);
            else
                mvPredios.ActiveViewIndex = cmdArg;

            switch (cmdArg)
            {
                case 0:
                    int iPagina = Convert.ToInt16(ViewState["RealIndexPredios"]) / gvPredios.PageSize;
                    int iIndex;
                    if (iPagina > 0)
                        iIndex = Convert.ToInt16(ViewState["RealIndexPredios"]) % gvPredios.PageSize;
                    else
                        iIndex = Convert.ToInt16(ViewState["RealIndexPredios"]);

                    fPrediosLoadGV(ViewState["CriterioBuscar"].ToString());
                    gvPredios.PageIndex = iPagina;
                    gvPredios.SelectedIndex = iIndex;
                    break;
                case 1:
                    fPrediosEstadoDetalle(false);
                    fPrediosDetalle();
                    break;
            }
            gv_SelectedIndexChanged(gvPredios);
            upPrediosFoot.Update();
        }
        #endregion

        #region--------PREDIOS DECLARADOS
        protected void btnPrediosDecAccion_Click(object sender, EventArgs e)
        {
            bool bActivarConfirmar = false;
            if (gvPrediosDec.Rows.Count > 0)
                bActivarConfirmar = true;

            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = "dec" + btnAccionSource.CommandName;
            hfEvtGVPrediosDec.Value = btnAccionSource.CommandArgument;
            txt_chip_PrediosDec.Focus();
            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    fPrediosDecDetalle();
                    fPrediosDecEstadoDetalle(true);
                    hfEvtGVPrediosDec.Value = btnAccionSource.CommandArgument;
                    break;
            }
            fBotonAccionFinalEstado(btnPrediosDecAccionFinal, btnPrediosDecCancelar, bActivarConfirmar);
            upPrediosDecFoot.Update();
        }

        protected void btnPrediosDecCancelar_Click(object sender, EventArgs e)
        {
            btnPrediosDecVista_Click(sender, e);
        }

        protected void btnPrediosDecNavegacion_Click(object sender, EventArgs e)
        {
            int iIndex = 0;

            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    iIndex = Convert.ToInt16(ViewState["RealIndexPrediosDec"]) - 1;
                    break;
                case "Next":
                    iIndex = Convert.ToInt16(ViewState["RealIndexPrediosDec"]) + 1;
                    break;
                case "Last":
                    iIndex = ((DataSet)oVar.prDSPrediosDecFiltro).Tables[0].Rows.Count - 1;
                    break;
            }
            ViewState["RealIndexPrediosDec"] = iIndex.ToString();
            fPrediosDecEstadoDetalle(false);
            fPrediosDecDetalle();
        }

        protected void btnPrediosDecVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);

            if (cmdArg == 0)
                fActivarVistaGrid(mvPrediosDec, btnPrediosDecAccionFinal, btnPrediosDecCancelar);
            else
                mvPrediosDec.ActiveViewIndex = cmdArg;

            switch (cmdArg)
            {
                case 0:
                    int iPagina = Convert.ToInt16(ViewState["RealIndexPrediosDec"]) / gvPrediosDec.PageSize;

                    int iIndex;
                    if (iPagina > 0)
                        iIndex = Convert.ToInt16(ViewState["RealIndexPrediosDec"]) % gvPrediosDec.PageSize;
                    else
                        iIndex = Convert.ToInt16(ViewState["RealIndexPrediosDec"]);

                    fPrediosDecLoadGV(gvPredios.SelectedDataKey.Value.ToString());
                    gvPrediosDec.PageIndex = iPagina;
                    gvPrediosDec.SelectedIndex = iIndex;
                    break;
                case 1:
                    fPrediosDecEstadoDetalle(false);
                    fPrediosDecDetalle();
                    break;
            }
            gv_SelectedIndexChanged(gvPrediosDec);
            upPrediosDecFoot.Update();
        }

        protected void btnPrediosDecFormato_Click(object sender, EventArgs e)
        {
            if (gvPrediosDec.Rows.Count > 0)
            {
                if (!Directory.Exists(_FORMATOSORIGEN))
                    fMensajeCRUD(_PATHFORMATOSERR, (int)clConstantes.NivelMensaje.Error, _DIVMSGPREDIOSDEC, upPrediosDec);
                else
                {
                    if (File.Exists(_FORMATOSORIGEN + @"\" + _FORMATOF035))
                    {
                        string NewName = _FORMATOF035.Replace(".xlsx", "");

                        //Archivo Excel del cual crear la copia:
                        FileInfo templateFile = new FileInfo(_FORMATOSORIGEN + @"\" + _FORMATOF035);

                        string fileName = NewName + "-" + gvPredios.Rows[gvPredios.SelectedIndex].Cells[0].Text + ".xlsx";

                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (ExcelPackage pck = new ExcelPackage(templateFile, true))
                        {
                            //Abrir primera Hoja del Archivo Excel que se crea'
                            ExcelWorksheet ws = pck.Workbook.Worksheets[0];

                            ws.Cells["B13"].Value = ws.Cells["B13"].Value + " " +
                                            gvPrediosDec.Rows[gvPrediosDec.SelectedIndex].Cells[2].Text + "-" +
                                            gvPrediosDec.Rows[gvPrediosDec.SelectedIndex].Cells[8].Text.Substring(6, 4) + "/    CHIP " +
                                            gvPredios.Rows[gvPredios.SelectedIndex].Cells[0].Text;
                            
                            Response.Clear();
                            Response.ClearHeaders();
                            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.BinaryWrite(pck.GetAsByteArray());
                            Response.End();

                            fMensajeCRUD("Archivo Creado", (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGPREDIOSDEC, upPrediosDec);
                        }
                    }
                    else
                        fMensajeCRUD(string.Format(_FILENOEXISTE, _FORMATOSORIGEN + "/" + _FORMATOF035), (int)clConstantes.NivelMensaje.Error, _DIVMSGPREDIOSDEC, upPrediosDec);
                }
            }
        }

        protected void btnPrediosDecFormato1_Click(object sender, EventArgs e)
        {
            Exception rtaCreacion;
            string pathPlantillaSource = string.Format("{0}{1}", oVar.prPathFormatosOrigen, oVar.prPathPlantillaCartaTerminos);

            DataSet dsSource = oPrediosDeclarados.sp_s_predios_dec_carta_terminos(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
            int indexTable = dsSource.Tables.Count - 1;
            int numRows = dsSource.Tables[indexTable].Rows.Count;
            string[] fileKey = new string[numRows];
            string[] pathPlantillaEdit = new string[numRows];
            string[] file_name = new string[numRows];
            if (numRows == 1)
            {
                fileKey[0] = oVar.prUser.ToString() + oUtil.CrearUniqueName();
                file_name[0] = "carta_terminos_" + gvPredios.SelectedRow.Cells[0].Text + ".docx";
                pathPlantillaEdit[0] = string.Format("{0}{1}{2}", oVar.prPathFormatosOrigen, fileKey[0], Path.GetExtension(pathPlantillaSource));
                File.Copy(pathPlantillaSource, pathPlantillaEdit[0], true);
                try
                {
                    HttpResponse response = HttpContext.Current.Response;
                    response.ClearContent();
                    response.Clear();
                    response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    response.AddHeader("Content-Disposition", "attachment; filename=" + file_name[0] + ";");
                    response.TransmitFile(pathPlantillaEdit[0]);
                    response.Flush();
                    response.End();
                }
                catch
                {
                    //
                }
                finally
                {
                    File.Delete(pathPlantillaEdit[0]);
                    fMensajeCRUD("Documento creado.", (int)clConstantes.NivelMensaje.Exitoso, "DivMsgPrediosDec", upPrediosDec);
                }
            }
            else
            {
                string temp_file = oUtil.CrearUniqueName();
                string temp_folder = oVar.prPathFormatosOrigen + oVar.prUser.ToString() + temp_file;
                Directory.CreateDirectory(temp_folder);
                for (int indexRow = 0; indexRow < numRows; indexRow++)
                {
                    pathPlantillaEdit[indexRow] = string.Format("{0}\\{1}{2}", temp_folder, indexRow.ToString(), Path.GetExtension(pathPlantillaSource));
                    File.Copy(pathPlantillaSource, pathPlantillaEdit[indexRow], true);
                    rtaCreacion = oCartaTerminosTemplate.fCrearTemplate(pathPlantillaEdit[indexRow], dsSource, indexTable, indexRow);
                    if (rtaCreacion != null)
                    {
                        oLog.RegistrarLogError(rtaCreacion, _SOURCEPAGE, "btnPrediosDecFormato1_Click");
                        fMensajeCRUD("No fue posible crear el documento.", (int)clConstantes.NivelMensaje.Error, "DivMsgPrediosDec", upPrediosDec);
                    }
                    file_name[indexRow] = "carta_terminos_" + gvPredios.SelectedRow.Cells[0].Text + "_" + indexRow.ToString() + ".docx";
                }

                string zip_file = oVar.prPathFormatosOrigen.ToString() + temp_file + ".zip";
                ZipFile.CreateFromDirectory(temp_folder, zip_file);
                file_name[0] = "carta_terminos_" + gvPredios.SelectedRow.Cells[0].Text + ".zip";

                try
                {
                    HttpResponse response = HttpContext.Current.Response;
                    response.ClearContent();
                    response.Clear();
                    Response.ContentType = "application/zip";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + file_name[0] + ";");
                    response.TransmitFile(zip_file);
                    response.Flush();
                    response.Close();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                catch (Exception MyErr)
                {
                    if (MyErr.HResult != -2146233040)
                        oLog.RegistrarLogError(MyErr, _SOURCEPAGE, "fGuardarPlantilla");
                }
                Directory.Delete(temp_folder, true);
                File.Delete(zip_file);
            }
            upPrediosDec.Update();
        }

        #endregion

        #region--------DOCUMENTOS
        protected void btnDocumentosAccion_Click(object sender, EventArgs e)
        {
            bool bActivarConfirmar = false;
            if (gvDocumentos.Rows.Count > 0)
                bActivarConfirmar = true;

            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = "doc" + btnAccionSource.CommandName;


            switch (btnAccionSource.CommandName)
            {
                case "Eliminar":
                    mpeDocumentos.Show();
                    return;
                case "Reordenar":
                    mpeDocumentosReordenar.Show();
                    return;
                case "MoveUp":
                    btnConfirmar_Click(null, null);
                    return;
                case "MoveDown":
                    btnConfirmar_Click(null, null);
                    return;
            }

            hfEvtGVDocumentos.Value = btnAccionSource.CommandArgument;

            txt_tipo_documento.Focus();

            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    fDocumentosDetalle();
                    fDocumentosEstadoDetalle(true, (int)clConstantes.AccionRol.MODIFICAR);
                    oVar.prPrediosDocumentos_FolioInicial = txt_folio_inicial_documento.Text;
                    break;
                case "Agregar":
                    bActivarConfirmar = true;
                    fDocumentosLimpiarDetalle();
                    fDocumentosEstadoDetalle(true, (int)clConstantes.AccionRol.CREAR);
                    mvDocumentos.ActiveViewIndex = 1;
                    txt_numero_carpeta.Text = gvPrediosDec.Rows[0].Cells[5].Text;
                    ddlb_cod_usu.Items.FindByValue(oVar.prUserCod.ToString()).Selected = true;
                    try
                    {
                        Int32 fi = Convert.ToInt32(gvDocumentos.Rows[gvDocumentos.Rows.Count - 1].Cells[9].Text);
                        txt_folio_inicial_documento.Text = Convert.ToString(fi + 1);
                    }
                    catch { }
                    break;
            }
            oUtil.fEstiloEstadoControl(vDocumentosDetalle);
            fBotonAccionFinalEstado(btnDocumentosAccionFinal, btnDocumentosCancelar, bActivarConfirmar);
            upDocumentosFoot.Update();
        }

        protected void btnDocumentosCancelar_Click(object sender, EventArgs e)
        {
            btnDocumentosVista_Click(sender, e);
        }

        protected void btnDocumentosNavegacion_Click(object sender, EventArgs e)
        {
            int iIndex = 0;
            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    iIndex = Convert.ToInt16(ViewState["RealIndexDocumentos"]) - 1;
                    break;
                case "Next":
                    iIndex = Convert.ToInt16(ViewState["RealIndexDocumentos"]) + 1;
                    break;
                case "Last":
                    iIndex = ((DataSet)oVar.prDSDocumentosFiltro).Tables[0].Rows.Count - 1;
                    break;
            }
            ViewState["RealIndexDocumentos"] = iIndex.ToString();
            fDocumentosEstadoDetalle(false, (int)clConstantes.AccionRol.CONSULTAR);
            fDocumentosDetalle();
        }

        protected void btnDocumentosVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            int iIndex;

            if (cmdArg == 0)
                fActivarVistaGrid(mvDocumentos, btnDocumentosAccionFinal, btnDocumentosCancelar);
            else
                mvDocumentos.ActiveViewIndex = cmdArg;

            switch (cmdArg)
            {
                case 0:
                    int iPagina = Convert.ToInt16(ViewState["RealIndexDocumentos"]) / gvDocumentos.PageSize;

                    if (iPagina > 0)
                        iIndex = Convert.ToInt16(ViewState["RealIndexDocumentos"]) % gvDocumentos.PageSize;
                    else
                        iIndex = Convert.ToInt16(ViewState["RealIndexDocumentos"]);

                    fDocumentosLoadGV(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                    gvDocumentos.PageIndex = iPagina;
                    gvDocumentos.SelectedIndex = iIndex;
                    break;
                case 1:
                    fDocumentosEstadoDetalle(false, (int)clConstantes.AccionRol.CONSULTAR);

                    if (gvDocumentos.Rows.Count > 0)
                        fDocumentosDetalle();

                    break;
            }
            gv_SelectedIndexChanged(gvDocumentos);
            upDocumentosFoot.Update();
        }

        protected void btnDocumentosFormato_Click(object sender, EventArgs e)
        {
            if (gvDocumentos.Rows.Count > 0)
            {
                if (!Directory.Exists(_FORMATOSORIGEN) || !Directory.Exists(_FORMATOSDESTINO))
                    fMensajeCRUD(clConstantes.MSG_ERR_FILE_CREADO, (int)clConstantes.NivelMensaje.Error, _DIVMSGDOCUMENTOS, upDocumentos);
                else
                {
                    if (File.Exists(_FORMATOSORIGEN + @"\" + _FORMATOF0379))
                    {
                        string NewName = _FORMATOF0379.Replace(".xlsx", "");

                        //Archivo Excel del cual crear la copia:
                        FileInfo templateFile = new FileInfo(_FORMATOSORIGEN + @"\" + _FORMATOF0379);

                        string fileName = NewName + "-" + gvPredios.Rows[gvPredios.SelectedIndex].Cells[0].Text + ".xlsx";

                        using (ExcelPackage pck = new ExcelPackage(templateFile, true))
                        {
                            int TotalRegistros = gvDocumentos.Rows.Count;
                            double LimiteImprimir = 0;
                            double iConsecutivo = 0;

                            //Filas que no se imprimen
                            int FilasFijas = 10;
                            int FilaImprimir = FilasFijas;
                            //Abrir primera Hoja del Archivo Excel que se crea'
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            ExcelWorksheet ws = pck.Workbook.Worksheets[0];

                            ws.Cells["C8"].Value = gvPredios.Rows[gvPredios.SelectedIndex].Cells[0].Text;

                            foreach (GridViewRow row in gvDocumentos.Rows)
                            {
                                ws.Cells["A" + FilaImprimir.ToString()].Value = Server.HtmlDecode(row.Cells[3].Text);
                                ws.Cells["B" + FilaImprimir.ToString()].Value = Server.HtmlDecode(row.Cells[4].Text);
                                ws.Cells["C" + FilaImprimir.ToString()].Value = Server.HtmlDecode(row.Cells[5].Text.Replace("&nbsp;", "N/A"));
                                ws.Cells["D" + FilaImprimir.ToString()].Value = Server.HtmlDecode(row.Cells[6].Text.Replace("&nbsp;", "N/A"));
                                ws.Cells["E" + FilaImprimir.ToString()].Value = Server.HtmlDecode(row.Cells[7].Text);
                                ws.Cells["F" + FilaImprimir.ToString()].Value = Server.HtmlDecode(row.Cells[8].Text);
                                ws.Cells["G" + FilaImprimir.ToString()].Value = Server.HtmlDecode(row.Cells[9].Text);
                                ws.Cells["H" + FilaImprimir.ToString()].Value = Server.HtmlDecode(row.Cells[11].Text);
                                ws.Cells["I" + FilaImprimir.ToString()].Value = Server.HtmlDecode(row.Cells[10].Text);

                                FilaImprimir++;
                                iConsecutivo = Convert.ToInt16(row.Cells[3].Text.Replace("&nbsp;", ""));
                            }

                            //Buscar la centena mas cercana
                            LimiteImprimir = Math.Ceiling(iConsecutivo / 100) * 100;
                            iConsecutivo++;
                            for (int inc = FilaImprimir; iConsecutivo <= LimiteImprimir; inc++)
                            {
                                ws.Cells["A" + inc.ToString()].Value = iConsecutivo.ToString();
                                iConsecutivo++;
                            }

                            var Rango = ws.Cells["A10:I" + (LimiteImprimir + FilasFijas - 1).ToString()];

                            Rango.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            Rango.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            Rango.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            Rango.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            Response.Clear();
                            Response.ClearHeaders();
                            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.BinaryWrite(pck.GetAsByteArray());
                            Response.End();

                        }
                        fMensajeCRUD(clConstantes.MSG_OK_FILE_CREADO, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGDOCUMENTOS, upDocumentos);
                    }
                    else
                        fMensajeCRUD(clConstantes.MSG_ERR_FILE_CREADO, (int)clConstantes.NivelMensaje.Error, _DIVMSGDOCUMENTOS, upDocumentos);
                }
            }
        }

        protected void lbSubirDoc_Click(object sender, EventArgs e)
        {
            FileUpload1.Visible = true;
        }
        #endregion

        #region--------PRESTAMOS
        protected void btnPrestamosAccion_Click(object sender, EventArgs e)
        {
            bool bActivarConfirmar = false;
            if (gvPrestamos.Rows.Count > 0)
                bActivarConfirmar = true;

            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = "prs" + btnAccionSource.CommandName;

            hfEvtGVPrestamos.Value = btnAccionSource.CommandArgument;

            esPermitido = true;

            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    if (gvPrestamos.Rows.Count > 0)
                    {
                        if (oBasic.fCheckUsuarioRegistro(gvPrestamos.SelectedDataKey["cod_usu_entrega_prestamo"].ToString()))
                        {
                            fPrestamosDetalle();
                            fPrestamosEstadoDetalle(true, (int)clConstantes.AccionRol.MODIFICAR);
                            if (ddlb_cod_usu_recibe_prestamo.Text == "")
                            {
                                ddlb_cod_usu_recibe_prestamo.Text = oVar.prUserCod.ToString();
                                ddlb_cod_usu_recibe_prestamo.Items.FindByValue(oVar.prUserCod.ToString()).Selected = true;
                            }
                            if (string.IsNullOrEmpty(txt_fecha_devolucion_prestamo.Text))
                                txt_fecha_devolucion_prestamo.Text = DateTime.Now.ToString("yyyy-MM-dd");
                            txt_fecha_devolucion_prestamo.Enabled = true;
                            ddlb_cod_usu_recibe_prestamo.Enabled = false;

                            rfv_fecha_devolucion_prestamo.Enabled = true;
                            rfv_cod_usu_recibe_prestamo.Enabled = ddlb_cod_usu_recibe_prestamo.Enabled;
                            oBasic.fValidarFecha_old("fecha devolución", cal_fecha_devolucion_prestamo, rv_fecha_devolucion_prestamo, txt_fecha_entrega_prestamo.Text, 0);
                        }
                        else
                        {
                            esPermitido = false;
                            oBasic.EnableButton(false, btnPrestamosEdit);
                        }
                    }
                    break;
                case "Agregar":
                    if (gvPrestamos.Rows.Count == 0 || gvPrestamos.SelectedDataKey["TotalNulos"].ToString() == "0")
                    {
                        bActivarConfirmar = true;
                        fPrestamosLimpiarDetalle();
                        fPrestamosEstadoDetalle(true, (int)clConstantes.AccionRol.CREAR);
                        mvPrestamos.ActiveViewIndex = 1;
                        txt_fecha_entrega_prestamo.Text = DateTime.Now.ToString("yyyy-MM-dd");
                        try
                        {
                            txt_folios_prestamo.Text = gvDocumentos.Rows[gvDocumentos.Rows.Count - 1].Cells[9].Text.ToString();
                        }
                        catch { }
                        ddlb_cod_usu_solicita_prestamo.Items.Clear();
                        ddlb_cod_usu_entrega_prestamo.Items.FindByValue(oVar.prUserCod.ToString()).Selected = true;
                        txt_cod_predio_declarado_prestamo.Text = gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString();
                        txt_fecha_devolucion_prestamo.Enabled = false;
                        ddlb_cod_usu_recibe_prestamo.Enabled = false;
                        rfv_fecha_devolucion_prestamo.Enabled = false;
                        rfv_cod_usu_recibe_prestamo.Enabled = false;

                        if (gvPrestamos.Rows.Count > 0)
                        {
                            DataSet dsTmp = (DataSet)oVar.prDSPrestamosUltimo;
                            string fecha = oUtil.ConvertToFechaDetalle(dsTmp.Tables[0].Rows[0]["fecha_devolucion_prestamo"].ToString());
                            oBasic.fValidarFecha_old("fecha entrega", cal_fecha_entrega_prestamo, rv_fecha_entrega_prestamo, fecha, 0);
                        }
                        else
                        {
                            oBasic.fValidarFecha_old("fecha entrega", cal_fecha_entrega_prestamo, rv_fecha_entrega_prestamo, "0", 0);
                        }
                    }
                    else
                    {
                        esPermitido = false;
                        oBasic.EnableButton(false, btnPrestamosAdd);
                    }
                    break;
                case "Agregar2":
                    bActivarConfirmar = true;
                    fPrestamosLimpiarDetalle2();
                    ddlb_id_tipo_prestamo.Focus();
                    ddlb_cod_usu_entrega_prestamo2.Items.FindByValue(oVar.prUserCod.ToString()).Selected = true;
                    ddlb_cod_usu_recibe_prestamo2.Items.FindByValue(oVar.prUserCod.ToString()).Selected = true;
                    break;
            }

            if (esPermitido)
            {
                ddlb_id_area_solicita_prestamo.Focus();
                oUtil.fEstiloEstadoControl(vPrestamosDetalle);
                fBotonAccionFinalEstado(btnPrestamosAccionFinal, btnPrestamosCancelar, bActivarConfirmar);
                upPrestamosFoot.Update();
            }
            else
            {
                fMensajeCRUD(clConstantes.MSG_ERR_PERMISO, (int)clConstantes.NivelMensaje.Alerta, _DIVMSGPRESTAMOS, upPrestamos);
                upPrestamosFoot.Update();
            }

        }

        protected void btnPrestamosCancelar_Click(object sender, EventArgs e)
        {
            btnPrestamosVista_Click(sender, e);
        }

        protected void btnPrestamosNavegacion_Click(object sender, EventArgs e)
        {
            int iIndex = 0;

            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    iIndex = Convert.ToInt16(ViewState["RealIndexPrestamos"]) - 1;
                    break;
                case "Next":
                    iIndex = Convert.ToInt16(ViewState["RealIndexPrestamos"]) + 1;
                    break;
                case "Last":
                    iIndex = ((DataSet)oVar.prDSPrestamos).Tables[0].Rows.Count - 1;
                    break;
            }
            ViewState["RealIndexPrestamos"] = iIndex.ToString();
            fPrestamosEstadoDetalle(false, (int)clConstantes.AccionRol.CONSULTAR);
            fPrestamosDetalle();
        }

        protected void btnPrestamosVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);

            if (cmdArg == 0)
                fActivarVistaGrid(mvPrestamos, btnPrestamosAccionFinal, btnPrestamosCancelar);
            else
                mvPrestamos.ActiveViewIndex = cmdArg;

            switch (cmdArg)
            {
                case 0:
                    int iPagina = Convert.ToInt16(ViewState["RealIndexPrestamos"]) / gvPrestamos.PageSize;

                    int iIndex;
                    if (iPagina > 0)
                        iIndex = Convert.ToInt16(ViewState["RealIndexPrestamos"]) % gvPrestamos.PageSize;
                    else
                        iIndex = Convert.ToInt16(ViewState["RealIndexPrestamos"]);

                    fPrestamosLoadGV(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                    gvPrestamos.PageIndex = iPagina;
                    gvPrestamos.SelectedIndex = iIndex;
                    break;
                case 1:
                    fPrestamosEstadoDetalle(false, (int)clConstantes.AccionRol.CONSULTAR);

                    if (gvPrestamos.Rows.Count > 0)
                        fPrestamosDetalle();

                    break;
            }
            gv_SelectedIndexChanged(gvPrestamos);
            upPrestamosFoot.Update();
        }

        protected void lb_cod_predio_declarado_Click(object sender, EventArgs e)
        {
            foreach (ListItem li in lbx_cod_predio_declarado.Items)
            {
                if (li.Selected)
                {
                    ListItem item = new ListItem(li.Value);
                    if (!lbx_cod_predio_declarado_add2.Items.Contains(item))
                    {
                        lbx_cod_predio_declarado_add.Items.Add(li.Text);
                        lbx_cod_predio_declarado_add2.Items.Add(li.Value);
                    }
                }
            }
            lbx_cod_predio_declarado.ClearSelection();
        }

        protected void lb_cod_predio_declarado_clear_Click(object sender, EventArgs e)
        {
            int j = lbx_cod_predio_declarado_add.Items.Count - 1;
            for (int i = j; i >= 0; i--)
            {
                if (lbx_cod_predio_declarado_add.Items[i].Selected)
                {
                    lbx_cod_predio_declarado_add.Items.RemoveAt(i);
                    lbx_cod_predio_declarado_add2.Items.RemoveAt(i);
                }
            }

        }
        #endregion

        #region--------PREDIOS PROPIETARIOS
        protected void btnPrediosPropietariosAccion_Click(object sender, EventArgs e)
        {
            bool bActivarConfirmar = false;
            if (gvPrediosPropietarios.Rows.Count > 0)
                bActivarConfirmar = true;

            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = "prp" + btnAccionSource.CommandName;

            if (btnAccionSource.CommandName == "Eliminar")
            {
                mpePrediosPropietarios.Show();
                return;
            }

            hfEvtGVPrediosPropietarios.Value = btnAccionSource.CommandArgument;

            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    if (gvPrediosPropietarios.Rows.Count > 0)
                    {
                        fPrediosPropietariosDetalle();
                        fPrediosPropietariosEstadoDetalle(true);
                    }
                    break;
                case "Agregar":
                    bActivarConfirmar = true;
                    fPrediosPropietariosLimpiarDetalle();
                    fPrediosPropietariosEstadoDetalle(true);
                    mvPrediosPropietarios.ActiveViewIndex = 1;
                    txt_fecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    txt_chip__predio_propietario.Text = gvPrediosDec.SelectedRow.Cells[1].Text;
                    break;
            }
            txt_fuente_propietario.Focus();
            fBotonAccionFinalEstado(btnPrediosPropietariosAccionFinal, btnPrediosPropietariosCancelar, bActivarConfirmar);
            upPrediosPropietariosFoot.Update();
        }

        protected void btnPrediosPropietariosCancelar_Click(object sender, EventArgs e)
        {
            btnPrediosPropietariosVista_Click(sender, e);
        }

        protected void btnPrediosPropietariosNavegacion_Click(object sender, EventArgs e)
        {
            int iIndex = 0;

            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    iIndex = Convert.ToInt16(ViewState["RealIndexPrediosPropietarios"]) - 1;
                    break;
                case "Next":
                    iIndex = Convert.ToInt16(ViewState["RealIndexPrediosPropietarios"]) + 1;
                    break;
                case "Last":
                    iIndex = ((DataSet)oVar.prDSPrediosPropietariosFiltro).Tables[0].Rows.Count - 1;
                    break;
            }
            ViewState["RealIndexPrediosPropietarios"] = iIndex.ToString();
            fPrediosPropietariosEstadoDetalle(false);
            fPrediosPropietariosDetalle();
        }

        protected void btnPrediosPropietariosVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);

            if (cmdArg == 0)
                fActivarVistaGrid(mvPrediosPropietarios, btnPrediosPropietariosAccionFinal, btnPrediosPropietariosCancelar);
            else
                mvPrediosPropietarios.ActiveViewIndex = cmdArg;

            switch (cmdArg)
            {
                case 0:
                    int iPagina = Convert.ToInt16(ViewState["RealIndexPrediosPropietarios"]) / gvPrediosPropietarios.PageSize;

                    int iIndex;
                    if (iPagina > 0)
                        iIndex = Convert.ToInt16(ViewState["RealIndexPrediosPropietarios"]) % gvPrediosPropietarios.PageSize;
                    else
                        iIndex = Convert.ToInt16(ViewState["RealIndexPrediosPropietarios"]);

                    fPrediosPropietariosLoadGV(gvPrediosDec.SelectedRow.Cells[1].Text);
                    gvPrediosPropietarios.PageIndex = iPagina;
                    gvPrediosPropietarios.SelectedIndex = iIndex;
                    break;
                case 1:
                    fPrediosPropietariosEstadoDetalle(false);

                    if (gvPrediosPropietarios.Rows.Count > 0)
                        fPrediosPropietariosDetalle();

                    break;
            }
            gv_SelectedIndexChanged(gvPrediosPropietarios);
            upPrediosPropietariosFoot.Update();
        }
        #endregion

        #region--------PROPIETARIOS
        protected void btnPropietariosAccion_Click(object sender, EventArgs e)
        {
            bool bActivarConfirmar = false;
            if (gvPropietarios.Rows.Count > 0)
                bActivarConfirmar = true;

            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = "pro" + btnAccionSource.CommandName;

            if (btnAccionSource.CommandName == "Eliminar")
            {
                mpePropietarios.Show();
                return;
            }

            hfEvtGVPropietarios.Value = btnAccionSource.CommandArgument;

            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    if (gvPropietarios.Rows.Count > 0)
                    {
                        fPropietariosDetalle();
                        fPropietariosEstadoDetalle(true);
                    }
                    break;
                case "Agregar":
                    bActivarConfirmar = true;
                    fPropietariosLimpiarDetalle();
                    fPropietariosEstadoDetalle(true);
                    mvPropietarios.ActiveViewIndex = 1;
                    break;
            }
            txt_nombre_propietario.Focus();
            fBotonAccionFinalEstado(btnPropietariosAccionFinal, btnPropietariosCancelar, bActivarConfirmar);
            upPropietariosFoot.Update();
        }

        protected void btnPropietariosCancelar_Click(object sender, EventArgs e)
        {
            btnPropietariosVista_Click(sender, e);
        }

        protected void btnPropietariosNavegacion_Click(object sender, EventArgs e)
        {
            int iIndex = 0;

            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    iIndex = Convert.ToInt16(ViewState["RealIndexPropietarios"]) - 1;
                    break;
                case "Next":
                    iIndex = Convert.ToInt16(ViewState["RealIndexPropietarios"]) + 1;
                    break;
                case "Last":
                    iIndex = ((DataSet)oVar.prDSPropietariosFiltro).Tables[0].Rows.Count - 1;
                    break;
            }
            ViewState["RealIndexPropietarios"] = iIndex.ToString();
            fPropietariosEstadoDetalle(false);
            fPropietariosDetalle();
        }

        protected void btnPropietariosVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);

            if (cmdArg == 0)
                fActivarVistaGrid(mvPropietarios, btnPropietariosAccionFinal, btnPropietariosCancelar);
            else
                mvPropietarios.ActiveViewIndex = cmdArg;

            switch (cmdArg)
            {
                case 0:
                    int iPagina = Convert.ToInt16(ViewState["RealIndexPropietarios"]) / gvPropietarios.PageSize;

                    int iIndex;
                    if (iPagina > 0)
                        iIndex = Convert.ToInt16(ViewState["RealIndexPropietarios"]) % gvPropietarios.PageSize;
                    else
                        iIndex = Convert.ToInt16(ViewState["RealIndexPropietarios"]);

                    fPropietariosLoadGV(gvPrediosDec.SelectedRow.Cells[1].Text);
                    gvPropietarios.PageIndex = iPagina;
                    gvPropietarios.SelectedIndex = iIndex;
                    break;
                case 1:
                    fPropietariosEstadoDetalle(false);

                    if (gvPropietarios.Rows.Count > 0)
                        fPropietariosDetalle();

                    break;
            }
            gv_SelectedIndexChanged(gvPropietarios);
            upPropietariosFoot.Update();
        }
        #endregion

        #region--------OBSERVACIONES
        protected void btnObservacionesAccion_Click(object sender, EventArgs e)
        {
            bool bActivarConfirmar = false;
            if (gvObservaciones.Rows.Count > 0)
                bActivarConfirmar = true;

            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = "obs" + btnAccionSource.CommandName;

            if (btnAccionSource.CommandName == "Eliminar")
            {
                mpeObservaciones.Show();
                return;
            }

            hfEvtGVObservaciones.Value = btnAccionSource.CommandArgument;

            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    if (gvObservaciones.Rows.Count > 0)
                    {
                        fObservacionesDetalle();
                        fObservacionesEstadoDetalle(true);
                    }
                    break;
                case "Agregar":
                    bActivarConfirmar = true;
                    fObservacionesLimpiarDetalle();
                    fObservacionesEstadoDetalle(true);
                    ddlb_cod_usu__observacion.Items.FindByValue(oVar.prUserCod.ToString()).Selected = true;
                    mvObservaciones.ActiveViewIndex = 1;
                    txt_fecha_observacion.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    break;
            }
            txt_fecha_observacion.Focus();
            fBotonAccionFinalEstado(btnObservacionesAccionFinal, btnObservacionesCancelar, bActivarConfirmar);
            upObservacionesFoot.Update();
        }

        protected void btnObservacionesCancelar_Click(object sender, EventArgs e)
        {
            btnObservacionesVista_Click(sender, e);
        }

        protected void btnObservacionesNavegacion_Click(object sender, EventArgs e)
        {
            int iIndex = 0;

            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    iIndex = Convert.ToInt16(ViewState["RealIndexObservaciones"]) - 1;
                    break;
                case "Next":
                    iIndex = Convert.ToInt16(ViewState["RealIndexObservaciones"]) + 1;
                    break;
                case "Last":
                    iIndex = ((DataSet)oVar.prDSObservacionesFiltro).Tables[0].Rows.Count - 1;
                    break;
            }
            ViewState["RealIndexObservaciones"] = iIndex.ToString();
            fObservacionesEstadoDetalle(true);
            fObservacionesDetalle();
        }

        protected void btnObservacionesVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);

            if (cmdArg == 0)
                fActivarVistaGrid(mvObservaciones, btnObservacionesAccionFinal, btnObservacionesCancelar);
            else
                mvObservaciones.ActiveViewIndex = cmdArg;

            switch (cmdArg)
            {
                case 0:
                    int iPagina = Convert.ToInt16(ViewState["RealIndexObservaciones"]) / gvObservaciones.PageSize;

                    int iIndex;
                    if (iPagina > 0)
                        iIndex = Convert.ToInt16(ViewState["RealIndexObservaciones"]) % gvObservaciones.PageSize;
                    else
                        iIndex = Convert.ToInt16(ViewState["RealIndexObservaciones"]);

                    fObservacionesLoadGV(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                    gvObservaciones.PageIndex = iPagina;
                    gvObservaciones.SelectedIndex = iIndex;
                    break;
                case 1:
                    fObservacionesEstadoDetalle(true);

                    if (gvObservaciones.Rows.Count > 0)
                        fObservacionesDetalle();

                    break;
            }
            gv_SelectedIndexChanged(gvObservaciones);
            upObservacionesFoot.Update();
        }
        #endregion

        #region--------VISITAS
        protected void btnVisitasAccion_Click(object sender, EventArgs e)
        {
            bool bActivarConfirmar = false;
            if (gvVisitas.Rows.Count > 0)
                bActivarConfirmar = true;

            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = "vis" + btnAccionSource.CommandName;

            if (btnAccionSource.CommandName == "Eliminar")
            {
                mpeVisitas.Show();
                return;
            }

            hfEvtGVVisitas.Value = btnAccionSource.CommandArgument;

            ddlb_id_tipo_visita.Focus();
            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    if (gvVisitas.Rows.Count > 0)
                    {
                        fVisitasDetalle();
                        if (ddlb_id_tipo_visita.SelectedValue.ToString() == _TIPOVISITA_INFORMETECNICO || ddlb_id_tipo_visita.SelectedValue.ToString() == _TIPOVISITA_ACTA)
                        {
                            TabContainerVisitas.Enabled = true;
                            fVisitasEstadoDetalle(true, true);
                        }
                        else
                        {
                            TabContainerVisitas.Enabled = false;
                            fVisitasEstadoDetalle(true, false);
                        }
                    }
                    break;
                case "Agregar":
                    bActivarConfirmar = true;
                    fVisitasLimpiarDetalle();
                    fVisitasEstadoDetalle(true, false);
                    mvVisitas.ActiveViewIndex = 1;
                    ddlb_cod_usu_visita.Items.FindByValue(oVar.prUserCod.ToString()).Selected = true;
                    break;
            }
            oUtil.fEstiloEstadoControl(vVisitasDetalle);
            fBotonAccionFinalEstado(btnVisitasAccionFinal, btnVisitasCancelar, bActivarConfirmar);
            upVisitasFoot.Update();
        }

        protected void btnVisitasCancelar_Click(object sender, EventArgs e)
        {
            btnVisitasVista_Click(sender, e);
        }

        protected void btnVisitasNavegacion_Click(object sender, EventArgs e)
        {
            int iIndex = 0;

            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    iIndex = Convert.ToInt16(ViewState["RealIndexVisitas"]) - 1;
                    break;
                case "Next":
                    iIndex = Convert.ToInt16(ViewState["RealIndexVisitas"]) + 1;
                    break;
                case "Last":
                    iIndex = ((DataSet)oVar.prDSVisitasFiltro).Tables[0].Rows.Count - 1;
                    break;
            }
            ViewState["RealIndexVisitas"] = iIndex.ToString();
            fVisitasEstadoDetalle(false, false);
            fVisitasDetalle();
        }

        protected void btnVisitasVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            int iIndex;

            if (cmdArg == 0)
                fActivarVistaGrid(mvVisitas, btnVisitasAccionFinal, btnVisitasCancelar);
            else
                mvVisitas.ActiveViewIndex = cmdArg;

            switch (cmdArg)
            {
                case 0:
                    int iPagina = Convert.ToInt16(ViewState["RealIndexVisitas"]) / gvVisitas.PageSize;

                    if (iPagina > 0)
                        iIndex = Convert.ToInt16(ViewState["RealIndexVisitas"]) % gvVisitas.PageSize;
                    else
                        iIndex = Convert.ToInt16(ViewState["RealIndexVisitas"]);

                    fVisitasLoadGV(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                    gvVisitas.PageIndex = iPagina;
                    gvVisitas.SelectedIndex = iIndex;
                    break;
                case 1:
                    fVisitasEstadoDetalle(false, false);

                    if (gvVisitas.Rows.Count > 0)
                        fVisitasDetalle();

                    break;
            }
            gv_SelectedIndexChanged(gvVisitas);
            upVisitasFoot.Update();
        }

        protected void btnCrearDocVisita_Click(object sender, EventArgs e)
        {
            fPreparaPlantilla();
        }

        protected void btnCrearDocVisitaOld_Click(object sender, EventArgs e)
        {
            fPreparaPlantillaOld();
        }

        protected void btnFotoNavegacion_Click(object sender, EventArgs e)
        {
            int iIndex = Convert.ToInt16(ViewState["IndexFoto"].ToString());

            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    iIndex--;
                    break;
                case "Next":
                    iIndex++;
                    break;

            }
            ViewState["IndexFoto"] = iIndex.ToString();
            fVerFotos();
        }

        protected void btnVerFotos_Click(object sender, EventArgs e)
        {
            fVerFotos();
        }
        #endregion

        #region--------LICENCIAS
        protected void btnLicenciasAccion_Click(object sender, EventArgs e)
        {
            bool bActivarConfirmar = false;
            if (gvLicencias.Rows.Count > 0)
                bActivarConfirmar = true;

            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = "lic" + btnAccionSource.CommandName;

            if (btnAccionSource.CommandName == "Eliminar")
            {
                mpeLicencias.Show();
                return;
            }

            hfEvtGVLicencias.Value = btnAccionSource.CommandArgument;

            ddlb_id_tipo_visita.Focus();
            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    if (gvLicencias.Rows.Count > 0)
                    {
                        fLicenciasDetalle();
                        fLicenciasEstadoDetalle(true);
                        oVar.prHayCambiosLicencias = false;
                    }
                    break;
                case "Agregar":
                    bActivarConfirmar = true;
                    fLicenciasLimpiarDetalle();
                    fLicenciasEstadoDetalle(true);
                    mvLicencias.ActiveViewIndex = 1;
                    break;
            }

            fBotonAccionFinalEstado(btnLicenciasAccionFinal, btnLicenciasCancelar, bActivarConfirmar);
            upLicenciasFoot.Update();
        }

        protected void btnLicenciasCancelar_Click(object sender, EventArgs e)
        {
            btnLicenciasVista_Click(sender, e);
        }

        protected void btnLicenciasNavegacion_Click(object sender, EventArgs e)
        {
            int iIndex = 0;

            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    iIndex = Convert.ToInt16(ViewState["RealIndexLicencias"]) - 1;
                    break;
                case "Next":
                    iIndex = Convert.ToInt16(ViewState["RealIndexLicencias"]) + 1;
                    break;
                case "Last":
                    iIndex = ((DataSet)oVar.prDSLicenciasFiltro).Tables[0].Rows.Count - 1;
                    break;
            }
            ViewState["RealIndexLicencias"] = iIndex.ToString();
            fLicenciasEstadoDetalle(false);
            fLicenciasDetalle();
        }

        protected void btnLicenciasVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            int iIndex;

            if (cmdArg == 0)
                fActivarVistaGrid(mvLicencias, btnLicenciasAccionFinal, btnLicenciasCancelar);
            else
                mvLicencias.ActiveViewIndex = cmdArg;

            switch (cmdArg)
            {
                case 0:
                    int iPagina = Convert.ToInt16(ViewState["RealIndexLicencias"]) / gvLicencias.PageSize;

                    if (iPagina > 0)
                        iIndex = Convert.ToInt16(ViewState["RealIndexLicencias"]) % gvLicencias.PageSize;
                    else
                        iIndex = Convert.ToInt16(ViewState["RealIndexLicencias"]);

                    fLicenciasLoadGV(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                    gvLicencias.PageIndex = iPagina;
                    gvLicencias.SelectedIndex = iIndex;
                    break;
                case 1:
                    fLicenciasEstadoDetalle(false);

                    if (gvLicencias.Rows.Count > 0)
                        fLicenciasDetalle();

                    break;
            }
            gv_SelectedIndexChanged(gvLicencias);
            upLicenciasFoot.Update();
        }
        #endregion

        #region--------CONCEPTOS
        protected void btnConceptosAccion_Click(object sender, EventArgs e)
        {
            bool bActivarConfirmar = false;
            if (gvConceptos.Rows.Count > 0)
                bActivarConfirmar = true;

            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = "con" + btnAccionSource.CommandName;

            if (btnAccionSource.CommandName == "Eliminar")
            {
                mpeConceptos.Show();
                return;
            }

            hfEvtGVConceptos.Value = btnAccionSource.CommandArgument;
            ddlb_id_tipo_concepto.Focus();

            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    if (gvConceptos.Rows.Count > 0)
                    {
                        fConceptosDetalle();
                        fConceptosEstadoDetalle(true, false);
                        oVar.prHayCambiosConceptos = false;
                    }
                    break;
                case "Agregar":
                    bActivarConfirmar = true;
                    fConceptosLimpiarDetalle();
                    fConceptosEstadoDetalle(true, true);
                    mvConceptos.ActiveViewIndex = 1;
                    ddlb_cod_usu_concepto.Items.FindByValue(oVar.prUserCod.ToString()).Selected = true;
                    break;
            }
            fBotonAccionFinalEstado(btnConceptosAccionFinal, btnConceptosCancelar, bActivarConfirmar);
            upConceptosFoot.Update();
        }

        protected void btnConceptosCancelar_Click(object sender, EventArgs e)
        {
            btnConceptosVista_Click(sender, e);
        }

        protected void btnConceptosNavegacion_Click(object sender, EventArgs e)
        {
            int iIndex = 0;

            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    iIndex = Convert.ToInt16(ViewState["RealIndexConceptos"]) - 1;
                    break;
                case "Next":
                    iIndex = Convert.ToInt16(ViewState["RealIndexConceptos"]) + 1;
                    break;
                case "Last":
                    iIndex = ((DataSet)oVar.prDSConceptosFiltro).Tables[0].Rows.Count - 1;
                    break;
            }
            ViewState["RealIndexConceptos"] = iIndex.ToString();
            fConceptosEstadoDetalle(false, false);
            fConceptosDetalle();
        }

        protected void btnConceptosVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            int iIndex;

            if (cmdArg == 0)
                fActivarVistaGrid(mvConceptos, btnConceptosAccionFinal, btnConceptosCancelar);
            else
                mvConceptos.ActiveViewIndex = cmdArg;

            switch (cmdArg)
            {
                case 0:
                    int iPagina = Convert.ToInt16(ViewState["RealIndexConceptos"]) / gvConceptos.PageSize;

                    if (iPagina > 0)
                        iIndex = Convert.ToInt16(ViewState["RealIndexConceptos"]) % gvConceptos.PageSize;
                    else
                        iIndex = Convert.ToInt16(ViewState["RealIndexConceptos"]);

                    fConceptosLoadGV(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                    gvConceptos.PageIndex = iPagina;
                    gvConceptos.SelectedIndex = iIndex;
                    break;
                case 1:
                    fConceptosEstadoDetalle(false, false);

                    if (gvConceptos.Rows.Count > 0)
                        fConceptosDetalle();

                    break;
            }
            gv_SelectedIndexChanged(gvConceptos);
            upConceptosFoot.Update();
        }

        protected void btnCrearDocConcepto_Click(object sender, EventArgs e)
        {
            fPreparaPlantillaConcepto();
        }
        #endregion

        #region--------ACTOS ADMINISTRATIVOS
        protected void btnActosAdmAccion_Click(object sender, EventArgs e)
        {
            bool bActivarConfirmar = false;
            if (gvActosAdm.Rows.Count > 0)
                bActivarConfirmar = true;

            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = "adm" + btnAccionSource.CommandName;

            if (btnAccionSource.CommandName == "Eliminar")
            {
                if (gvActosAdm.SelectedRow.Cells[10].Text == "1")
                    fMensajeCRUD(clConstantes.MSG_ERR_PERMISO, (int)clConstantes.NivelMensaje.Alerta, _DIVMSGACTOSADM, upActosAdm);
                else
                    mpeActosAdm.Show();
                return;
            }

            hfEvtGVActosAdm.Value = btnAccionSource.CommandArgument;

            ddlb_id_tipo_acto.Focus();

            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    if (gvActosAdm.Rows.Count > 0)
                    {
                        fActosAdmDetalle();
                        fActosAdmEstadoDetalle(false);
                        oVar.prHayCambiosActosAdm = false;
                    }
                    break;
                case "Agregar":
                    bActivarConfirmar = true;
                    fActosAdmLimpiarDetalle();
                    fActosAdmEstadoDetalle(true);
                    break;
            }
            fBotonAccionFinalEstado(btnActosAdmAccionFinal, btnActosAdmCancelar, bActivarConfirmar);
            upActosAdmFoot.Update();
        }

        protected void btnActosAdmCancelar_Click(object sender, EventArgs e)
        {
            btnActosAdmVista_Click(sender, e);
        }

        protected void btnActosAdmNavegacion_Click(object sender, EventArgs e)
        {
            int iIndex = 0;

            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    iIndex = Convert.ToInt16(ViewState["RealIndexActosAdm"]) - 1;
                    break;
                case "Next":
                    iIndex = Convert.ToInt16(ViewState["RealIndexActosAdm"]) + 1;
                    break;
                case "Last":
                    iIndex = ((DataSet)oVar.prDSActosAdmFiltro).Tables[0].Rows.Count - 1;
                    break;
            }
            ViewState["RealIndexActosAdm"] = iIndex.ToString();
            fActosAdmEstadoDetalle(false);
            fActosAdmDetalle();
        }

        protected void btnActosAdmVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            int iIndex;

            if (cmdArg == 0)
                fActivarVistaGrid(mvActosAdm, btnActosAdmAccionFinal, btnActosAdmCancelar);
            else
                mvActosAdm.ActiveViewIndex = cmdArg;

            switch (cmdArg)
            {
                case 0:
                    int iPagina = Convert.ToInt16(ViewState["RealIndexActosAdm"]) / gvActosAdm.PageSize;

                    if (iPagina > 0)
                        iIndex = Convert.ToInt16(ViewState["RealIndexActosAdm"]) % gvActosAdm.PageSize;
                    else
                        iIndex = Convert.ToInt16(ViewState["RealIndexActosAdm"]);

                    fActosAdmLoadGV(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                    gvActosAdm.PageIndex = iPagina;
                    gvActosAdm.SelectedIndex = iIndex;
                    break;
                case 1:
                    fActosAdmEstadoDetalle(false);
                    if (gvActosAdm.Rows.Count > 0)
                        fActosAdmDetalle();

                    break;
            }
            gv_SelectedIndexChanged(gvActosAdm);
            upActosAdmFoot.Update();
        }

        
        #endregion
        #endregion

        #region--------------------------------------------------------------------GRIDVIEW
        #region--------PREDIOS
        protected void gvPredios_DataBinding(object sender, EventArgs e)
        {
            fActivarVistaGrid(mvPredios, btnPrediosAccionFinal, btnPrediosCancelar);
        }

        protected void gvPredios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            bPageChanged = true;
            gvPredios.PageIndex = e.NewPageIndex;
            fPrediosLoadGV(ViewState["CriterioBuscar"].ToString());
            ViewState["RealIndexPredios"] = ((gvPredios.PageSize * gvPredios.PageIndex) + gvPredios.PageIndex - 1).ToString();
        }

        protected void gvPredios_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header) return;

            string sortExpression = this.ViewState["SortExpression"].ToString();
            string sortDirection = this.ViewState["SortDirection"].ToString();

            foreach (System.Web.UI.WebControls.TableCell tableCell in e.Row.Cells)
            {
                if (!tableCell.HasControls()) continue;

                if (!(tableCell.Controls[0] is LinkButton lbSort)) continue;

                if (lbSort.CommandArgument == sortExpression)
                {
                    Image imageSort = new Image
                    {
                        ImageAlign = ImageAlign.AbsMiddle,
                        Width = 10
                    };

                    if (sortDirection == "ASC") imageSort.ImageUrl = "~/Images/icon/up.png";
                    else imageSort.ImageUrl = "~/Images/icon/down.png";


                    imageSort.Style.Add("margin-left", "15px");
                    tableCell.Controls.Add(imageSort);
                }
            }
        }

        protected void gvPredios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "OpenMap")
            {
                int iRow = Convert.ToInt32(e.CommandArgument);
                gvPredios.SelectRow(iRow);

                string cx = gvPredios.DataKeys[iRow][1].ToString();
                string cy = gvPredios.DataKeys[iRow][2].ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "loadMapa(" + cy + "," + cx + ");", true);
            }
        }

        protected void gvPredios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvPredios, "Select$" + e.Row.RowIndex.ToString()));
            }
        }

        protected void gvPredios_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["RealIndexPredios"] = ((gvPredios.PageIndex * gvPredios.PageSize) + gvPredios.SelectedIndex).ToString();

            if (SIDec.Properties.Settings.Default.Properties["DetalleOnClickPredios"] != null)
            {
                if (SIDec.Properties.Settings.Default.DetalleOnClickPredios)
                {
                    fPrediosDetalle();
                    fPrediosEstadoDetalle(false);
                }
            }
            fPrediosDecLoadGV(gvPredios.SelectedDataKey.Value.ToString());
            oVar.prUserResponsablePredioDec = Convert.ToInt16(gvPrediosDec.SelectedDataKey["cod_usu_responsable"].ToString());
            fCuentaRegistros(lblPrediosCuenta, gvPredios, (DataSet)oVar.prDSPrediosFiltro, btnFirstPredios, btnBackPredios, btnNextPredios, btnLastPredios, upPrediosFoot, gvPredios.SelectedIndex);
            upAfectaciones.Update();
        }

        protected void gvPredios_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = this.ViewState["SortExpression"].ToString();
            string sortDirection = this.ViewState["SortDirection"].ToString();

            if (sortExpression == e.SortExpression)
            {
                this.ViewState["SortDirection"] = (sortDirection == "ASC") ? "DESC" : "ASC";
            }
            else
            {
                this.ViewState["SortExpression"] = e.SortExpression;
                this.ViewState["SortDirection"] = "ASC";
            }

            DataView sortedView = new DataView(((DataSet)(oVar.prDSPrediosFiltro)).Tables[0])
            {
                Sort = e.SortExpression + " " + sortDirection
            };
            Session["objects"] = sortedView;
            gvPredios.DataSource = sortedView;
            gvPredios.DataBind();

            //Almacenar el nuevo Dataset ordenado -nuevos Index-
            oVar.prDSPrediosFiltro = oUtil.ConvertToDataSet(sortedView);
            gvPredios_SelectedIndexChanged(sender, e);
        }
        #endregion

        #region--------PREDIOS DECLARADOS
        protected void gvPrediosDec_DataBinding(object sender, EventArgs e)
        {
            fActivarVistaGrid(mvPrediosDec, btnPrediosDecAccionFinal, btnPrediosDecCancelar);
        }

        protected void gvPrediosDec_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPrediosDec.PageIndex = e.NewPageIndex;
            fPrediosDecLoadGV(ViewState["CriterioBuscar"].ToString());
        }

        protected void gvPrediosDec_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header) return;

            string sortExpression = this.ViewState["SortExpression"].ToString();
            string sortDirection = this.ViewState["SortDirection"].ToString();

            foreach (System.Web.UI.WebControls.TableCell tableCell in e.Row.Cells)
            {
                if (!tableCell.HasControls()) continue;

                if (!(tableCell.Controls[0] is LinkButton lbSort)) continue;

                if (lbSort.CommandArgument == sortExpression)
                {
                    Image imageSort = new Image
                    {
                        ImageAlign = ImageAlign.AbsMiddle,
                        Width = 10
                    };

                    if (sortDirection == "ASC") imageSort.ImageUrl = "~/Images/icon/up.png";
                    else imageSort.ImageUrl = "~/Images/icon/down.png";


                    imageSort.Style.Add("margin-left", "15px");
                    tableCell.Controls.Add(imageSort);
                }
            }
        }

        protected void gvPrediosDec_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvPrediosDec, "Select$" + e.Row.RowIndex.ToString()));
            }
        }

        protected void gvPrediosDec_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["RealIndexPrediosDec"] = ((gvPrediosDec.PageIndex * gvPrediosDec.PageSize) + gvPrediosDec.SelectedIndex).ToString();

            if (SIDec.Properties.Settings.Default.Properties["DetalleOnClickPrediosDec"] != null)
            {
                if (SIDec.Properties.Settings.Default.DetalleOnClickPrediosDec)
                {
                    fPrediosDecDetalle();
                    fPrediosDecEstadoDetalle(false);
                }
            }
            oVar.prUserResponsablePredioDec = Convert.ToInt16(gvPrediosDec.SelectedDataKey["cod_usu_responsable"].ToString());
            fCuentaRegistros(lblPrediosDecCuenta, gvPrediosDec, (DataSet)oVar.prDSPrediosDecFiltro, btnFirstPrediosDec, btnBackPrediosDec, btnNextPrediosDec, btnLastPrediosDec, upPrediosDecFoot, gvPrediosDec.SelectedIndex);

            string cod_predio_declarado = gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString();
            string idarchivo = gvPrediosDec.SelectedDataKey.Values["idarchivo"].ToString();
            string cod_usu_responsable = IsHelperUser()? oVar.prUserCod.ToString() :gvPrediosDec.SelectedDataKey.Values["cod_usu_responsable"].ToString();

            fDocumentosLoadGV(cod_predio_declarado);
            fPrestamosLoadGV(cod_predio_declarado);
            fPrediosPropietariosLoadGV(gvPrediosDec.SelectedRow.Cells[1].Text);
            fObservacionesLoadGV(cod_predio_declarado);
            fVisitasLoadGV(cod_predio_declarado);
            fLicenciasLoadGV(cod_predio_declarado);
            fConceptosLoadGV(cod_predio_declarado);
            fActosAdmLoadGV(cod_predio_declarado);
            fFichaPredialGV(cod_predio_declarado, idarchivo, cod_usu_responsable);
            LoadInteresados(cod_predio_declarado, cod_usu_responsable);
            LoadCartas(cod_predio_declarado, cod_usu_responsable);
        }

        protected void gvPrediosDec_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = this.ViewState["SortExpression"].ToString();
            string sortDirection = this.ViewState["SortDirection"].ToString();

            if (sortExpression == e.SortExpression)
            {
                this.ViewState["SortDirection"] = (sortDirection == "ASC") ? "DESC" : "ASC";
            }
            else
            {
                this.ViewState["SortExpression"] = e.SortExpression;
                this.ViewState["SortDirection"] = "ASC";
            }

            DataView sortedView = new DataView(((DataSet)(oVar.prDSPrediosDecFiltro)).Tables[0])
            {
                Sort = e.SortExpression + " " + sortDirection
            };
            Session["objects"] = sortedView;
            gvPrediosDec.DataSource = sortedView;
            gvPrediosDec.DataBind();

            //Almacenar el nuevo Dataset ordenado -nuevos Index-
            oVar.prDSPrediosDecFiltro = oUtil.ConvertToDataSet(sortedView);
            gvPrediosDec_SelectedIndexChanged(sender, e);
        }
        #endregion

        #region--------DOCUMENTOS
        protected void gvDocumentos_DataBinding(object sender, EventArgs e)
        {
            fActivarVistaGrid(mvDocumentos, btnDocumentosAccionFinal, btnDocumentosCancelar);
        }

        protected void gvDocumentos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDocumentos.PageIndex = e.NewPageIndex;
            fDocumentosLoadGV(ViewState["CriterioBuscar"].ToString());
        }

        protected void gvDocumentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "OpenDoc")
            {
                gvDocumentos.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvDocumentos.Rows[Convert.ToInt32(e.CommandArgument)];
                oFile.GetFullPathDoc(gvPrediosDec.SelectedRow.Cells[0].Text, gvDocumentos.DataKeys[row.DataItemIndex].Value.ToString());
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "verDoc();", true);
            }
        }

        protected void gvDocumentos_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header) return;

            string sortExpression = this.ViewState["SortExpression"].ToString();
            string sortDirection = this.ViewState["SortDirection"].ToString();

            foreach (System.Web.UI.WebControls.TableCell tableCell in e.Row.Cells)
            {
                if (!tableCell.HasControls()) continue;

                if (!(tableCell.Controls[0] is LinkButton lbSort)) continue;

                if (lbSort.CommandArgument == sortExpression)
                {
                    Image imageSort = new Image
                    {
                        ImageAlign = ImageAlign.AbsMiddle,
                        Width = 10
                    };

                    if (sortDirection == "ASC") imageSort.ImageUrl = "~/Images/icon/up.png";
                    else imageSort.ImageUrl = "~/Images/icon/down.png";


                    imageSort.Style.Add("margin-left", "15px");
                    tableCell.Controls.Add(imageSort);
                }
            }
        }

        protected void gvDocumentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvDocumentos, "Select$" + e.Row.RowIndex.ToString()));
            }
        }

        protected void gvDocumentos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["RealIndexDocumentos"] = ((gvDocumentos.PageIndex * gvDocumentos.PageSize) + gvDocumentos.SelectedIndex).ToString();

            if (SIDec.Properties.Settings.Default.Properties["DetalleOnClickDocumentos"] != null)
            {
                if (SIDec.Properties.Settings.Default.DetalleOnClickDocumentos)
                {
                    fDocumentosDetalle();
                    fDocumentosEstadoDetalle(false, (int)clConstantes.AccionRol.CONSULTAR);
                }
            }
            fDocumentosValidarPermisos();
            fCuentaRegistros(lblDocumentosCuenta, gvDocumentos, (DataSet)oVar.prDSDocumentosFiltro, btnFirstDocumentos, btnBackDocumentos, btnNextDocumentos, btnLastDocumentos, upDocumentosFoot, gvDocumentos.SelectedIndex);
        }

        protected void gvDocumentos_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = this.ViewState["SortExpression"].ToString();
            string sortDirection = this.ViewState["SortDirection"].ToString();

            if (sortExpression == e.SortExpression)
            {
                this.ViewState["SortDirection"] = (sortDirection == "ASC") ? "DESC" : "ASC";
            }
            else
            {
                this.ViewState["SortExpression"] = e.SortExpression;
                this.ViewState["SortDirection"] = "ASC";
            }

            DataView sortedView = new DataView(((DataSet)(oVar.prDSDocumentosFiltro)).Tables[0])
            {
                Sort = e.SortExpression + " " + sortDirection
            };
            Session["objects"] = sortedView;
            gvDocumentos.DataSource = sortedView;
            gvDocumentos.DataBind();

            //Almacenar el nuevo Dataset ordenado -nuevos Index-
            oVar.prDSDocumentosFiltro = oUtil.ConvertToDataSet(sortedView);
        }
        #endregion

        #region--------PRESTAMOS
        protected void gvPrestamos_DataBinding(object sender, EventArgs e)
        {
            fActivarVistaGrid(mvPrestamos, btnPrestamosAccionFinal, btnPrestamosCancelar);
        }

        protected void gvPrestamos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPrestamos.PageIndex = e.NewPageIndex;
            fPrestamosLoadGV(ViewState["CriterioBuscar"].ToString());
        }

        protected void gvPrestamos_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header) return;

            string sortExpression = this.ViewState["SortExpression"].ToString();
            string sortDirection = this.ViewState["SortDirection"].ToString();

            foreach (TableCell tableCell in e.Row.Cells)
            {
                if (!tableCell.HasControls()) continue;

                if (!(tableCell.Controls[0] is LinkButton lbSort)) continue;

                if (lbSort.CommandArgument == sortExpression)
                {
                    Image imageSort = new Image
                    {
                        ImageAlign = ImageAlign.AbsMiddle,
                        Width = 10
                    };

                    if (sortDirection == "ASC") imageSort.ImageUrl = "~/Images/icon/up.png";
                    else imageSort.ImageUrl = "~/Images/icon/down.png";


                    imageSort.Style.Add("margin-left", "15px");
                    tableCell.Controls.Add(imageSort);
                }
            }
        }

        protected void gvPrestamos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvPrestamos, "Select$" + e.Row.RowIndex.ToString()));
            }
        }

        protected void gvPrestamos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["RealIndexPrestamos"] = ((gvPrestamos.PageIndex * gvPrestamos.PageSize) + gvPrestamos.SelectedIndex).ToString();
            if (SIDec.Properties.Settings.Default.Properties["DetalleOnClickPrestamos"] != null)
            {
                if (SIDec.Properties.Settings.Default.DetalleOnClickPrestamos)
                {
                    fPrestamosDetalle();
                    fPrestamosEstadoDetalle(false, (int)clConstantes.AccionRol.CONSULTAR);
                }
            }
            fPrestamosValidarPermisos();
            fCuentaRegistros(lblPrestamosCuenta, gvPrestamos, (DataSet)oVar.prDSPrestamos, btnFirstPrestamos, btnBackPrestamos, btnNextPrestamos, btnLastPrestamos, upPrestamosFoot, gvPrestamos.SelectedIndex);
        }

        protected void gvPrestamos_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = this.ViewState["SortExpression"].ToString();
            string sortDirection = this.ViewState["SortDirection"].ToString();

            if (sortExpression == e.SortExpression)
            {
                this.ViewState["SortDirection"] = (sortDirection == "ASC") ? "DESC" : "ASC";
            }
            else
            {
                this.ViewState["SortExpression"] = e.SortExpression;
                this.ViewState["SortDirection"] = "ASC";
            }

            DataView sortedView = new DataView(((DataSet)(oVar.prDSPrestamos)).Tables[0])
            {
                Sort = e.SortExpression + " " + sortDirection
            };
            Session["objects"] = sortedView;
            gvPrestamos.DataSource = sortedView;
            gvPrestamos.DataBind();

            //Almacenar el nuevo Dataset ordenado -nuevos Index-
            oVar.prDSPrestamos = oUtil.ConvertToDataSet(sortedView);
        }
        #endregion

        #region--------PREDIOS PROPIETARIOS
        protected void gvPrediosPropietarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvPrediosPropietarios, "Select$" + e.Row.RowIndex.ToString()));
            }
        }

        protected void gvPrediosPropietarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["RealIndexPrediosPropietarios"] = ((gvPrediosPropietarios.PageIndex * gvPrediosPropietarios.PageSize) + gvPrediosPropietarios.SelectedIndex).ToString();

            if (SIDec.Properties.Settings.Default.Properties["DetalleOnClickPrediosPropietarios"] != null)
            {
                if (SIDec.Properties.Settings.Default.DetalleOnClickPrediosPropietarios)
                {
                    fPrediosPropietariosDetalle();
                    fPrediosPropietariosEstadoDetalle(false);
                }
            }

            fPrediosPropietariosValidarPermisos();
            fPropietariosLoadGV(gvPrediosPropietarios.SelectedDataKey.Value.ToString());
            fCuentaRegistros(lblPrediosPropietariosCuenta, gvPrediosPropietarios, (DataSet)oVar.prDSPrediosPropietariosFiltro, btnFirstPrediosPropietarios, btnBackPrediosPropietarios, btnNextPrediosPropietarios, btnLastPrediosPropietarios, upPrediosPropietariosFoot, gvPrediosPropietarios.SelectedIndex);
        }
        #endregion

        #region--------PROPIETARIOS
        protected void gvPropietarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvPropietarios, "Select$" + e.Row.RowIndex.ToString()));
            }
        }

        protected void gvPropietarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["RealIndexPropietarios"] = ((gvPropietarios.PageIndex * gvPropietarios.PageSize) + gvPropietarios.SelectedIndex).ToString();

            if (SIDec.Properties.Settings.Default.Properties["DetalleOnClickPropietarios"] != null)
            {
                if (SIDec.Properties.Settings.Default.DetalleOnClickPropietarios)
                {
                    fPropietariosDetalle();
                    fPropietariosEstadoDetalle(false);
                }
            }
            fPropietariosValidarPermisos();
            fCuentaRegistros(lblPropietariosCuenta, gvPropietarios, (DataSet)oVar.prDSPropietariosFiltro, btnFirstPropietarios, btnBackPropietarios, btnNextPropietarios, btnLastPropietarios, upPropietariosFoot, gvPropietarios.SelectedIndex);
        }
        #endregion

        #region--------OBSERVACIONES
        protected void gvObservaciones_DataBinding(object sender, EventArgs e)
        {
            fActivarVistaGrid(mvObservaciones, btnObservacionesAccionFinal, btnObservacionesCancelar);
        }

        protected void gvObservaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvObservaciones.PageIndex = e.NewPageIndex;
            fObservacionesLoadGV(ViewState["CriterioBuscar"].ToString());
            oVar.prVisitaAu = gvObservaciones.SelectedDataKey;
        }

        protected void gvObservaciones_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header) return;

            string sortExpression = this.ViewState["SortExpression"].ToString();
            string sortDirection = this.ViewState["SortDirection"].ToString();

            foreach (System.Web.UI.WebControls.TableCell tableCell in e.Row.Cells)
            {
                if (!tableCell.HasControls()) continue;

                if (!(tableCell.Controls[0] is LinkButton lbSort)) continue;

                if (lbSort.CommandArgument == sortExpression)
                {
                    Image imageSort = new Image
                    {
                        ImageAlign = ImageAlign.AbsMiddle,
                        Width = 10
                    };

                    if (sortDirection == "ASC") imageSort.ImageUrl = "~/Images/icon/up.png";
                    else imageSort.ImageUrl = "~/Images/icon/down.png";


                    imageSort.Style.Add("margin-left", "15px");
                    tableCell.Controls.Add(imageSort);
                }
            }
        }

        protected void gvObservaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvObservaciones, "Select$" + e.Row.RowIndex.ToString()));
            }
        }

        protected void gvObservaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["RealIndexObservaciones"] = ((gvObservaciones.PageIndex * gvObservaciones.PageSize) + gvObservaciones.SelectedIndex).ToString();

            if (SIDec.Properties.Settings.Default.Properties["DetalleOnClickObservaciones"] != null)
            {
                if (SIDec.Properties.Settings.Default.DetalleOnClickObservaciones)
                {
                    fObservacionesEstadoDetalle(false);
                    fObservacionesDetalle();
                }
            }
            fObservacionesValidarPermisos();
            fCuentaRegistros(lblObservacionesCuenta, gvObservaciones, (DataSet)oVar.prDSObservacionesFiltro, btnFirstObservaciones, btnBackObservaciones, btnNextObservaciones, btnLastObservaciones, upObservacionesFoot, gvObservaciones.SelectedIndex);
        }

        protected void gvObservaciones_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = this.ViewState["SortExpression"].ToString();
            string sortDirection = this.ViewState["SortDirection"].ToString();

            if (sortExpression == e.SortExpression)
            {
                this.ViewState["SortDirection"] = (sortDirection == "ASC") ? "DESC" : "ASC";
            }
            else
            {
                this.ViewState["SortExpression"] = e.SortExpression;
                this.ViewState["SortDirection"] = "ASC";
            }

            DataView sortedView = new DataView(((DataSet)oVar.prDSObservacionesFiltro).Tables[0])
            {
                Sort = e.SortExpression + " " + sortDirection
            };
            Session["objects"] = sortedView;
            gvObservaciones.DataSource = sortedView;
            gvObservaciones.DataBind();

            //Almacenar el nuevo Dataset ordenado -nuevos Index-
            oVar.prDSObservacionesFiltro = oUtil.ConvertToDataSet(sortedView);
        }
        #endregion

        #region--------VISITAS
        protected void gvVisitas_DataBinding(object sender, EventArgs e)
        {
            fActivarVistaGrid(mvVisitas, btnVisitasAccionFinal, btnVisitasCancelar);
        }

        protected void gvVisitas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvVisitas.PageIndex = e.NewPageIndex;
            fVisitasLoadGV(ViewState["CriterioBuscar"].ToString());
            oVar.prVisitaAu = gvVisitas.SelectedDataKey;
        }

        protected void gvVisitas_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header) return;

            string sortExpression = this.ViewState["SortExpression"].ToString();
            string sortDirection = this.ViewState["SortDirection"].ToString();

            foreach (System.Web.UI.WebControls.TableCell tableCell in e.Row.Cells)
            {
                if (!tableCell.HasControls()) continue;

                if (!(tableCell.Controls[0] is LinkButton lbSort)) continue;

                if (lbSort.CommandArgument == sortExpression)
                {
                    Image imageSort = new Image
                    {
                        ImageAlign = ImageAlign.AbsMiddle,
                        Width = 10
                    };

                    if (sortDirection == "ASC") imageSort.ImageUrl = "~/Images/icon/up.png";
                    else imageSort.ImageUrl = "~/Images/icon/down.png";


                    imageSort.Style.Add("margin-left", "15px");
                    tableCell.Controls.Add(imageSort);
                }
            }
        }

        protected void gvVisitas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvVisitas, "Select$" + e.Row.RowIndex.ToString()));
            }
        }

        protected void gvVisitas_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["RealIndexVisitas"] = ((gvVisitas.PageIndex * gvVisitas.PageSize) + gvVisitas.SelectedIndex).ToString();

            if (SIDec.Properties.Settings.Default.Properties["DetalleOnClickVisitas"] != null)
            {
                if (SIDec.Properties.Settings.Default.DetalleOnClickVisitas)
                {
                    fVisitasEstadoDetalle(false, false);
                    fVisitasDetalle();
                }
            }
            fVisitasValidarPermisos();
            oVar.prVisitaAu = gvVisitas.SelectedDataKey.Value;
            fCuentaRegistros(lblVisitasCuenta, gvVisitas, (DataSet)oVar.prDSVisitasFiltro, btnFirstVisitas, btnBackVisitas, btnNextVisitas, btnLastVisitas, upVisitasFoot, gvVisitas.SelectedIndex);
        }

        protected void gvVisitas_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = this.ViewState["SortExpression"].ToString();
            string sortDirection = this.ViewState["SortDirection"].ToString();

            if (sortExpression == e.SortExpression)
            {
                this.ViewState["SortDirection"] = (sortDirection == "ASC") ? "DESC" : "ASC";
            }
            else
            {
                this.ViewState["SortExpression"] = e.SortExpression;
                this.ViewState["SortDirection"] = "ASC";
            }

            DataView sortedView = new DataView(((DataSet)(oVar.prDSVisitasFiltro)).Tables[0])
            {
                Sort = e.SortExpression + " " + sortDirection
            };
            Session["objects"] = sortedView;
            gvVisitas.DataSource = sortedView;
            gvVisitas.DataBind();

            //Almacenar el nuevo Dataset ordenado -nuevos Index-
            oVar.prDSVisitasFiltro = oUtil.ConvertToDataSet(sortedView);
        }
        #endregion

        #region--------LICENCIAS
        protected void gvLicencias_DataBinding(object sender, EventArgs e)
        {
            fActivarVistaGrid(mvLicencias, btnLicenciasAccionFinal, btnLicenciasCancelar);
        }

        protected void gvLicencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLicencias.PageIndex = e.NewPageIndex;
            fLicenciasLoadGV(ViewState["CriterioBuscar"].ToString());
        }

        protected void gvLicencias_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header) return;

            string sortExpression = this.ViewState["SortExpression"].ToString();
            string sortDirection = this.ViewState["SortDirection"].ToString();

            foreach (System.Web.UI.WebControls.TableCell tableCell in e.Row.Cells)
            {
                if (!tableCell.HasControls()) continue;

                if (!(tableCell.Controls[0] is LinkButton lbSort)) continue;

                if (lbSort.CommandArgument == sortExpression)
                {
                    Image imageSort = new Image
                    {
                        ImageAlign = ImageAlign.AbsMiddle,
                        Width = 10
                    };

                    if (sortDirection == "ASC") imageSort.ImageUrl = "~/Images/icon/up.png";
                    else imageSort.ImageUrl = "~/Images/icon/down.png";


                    imageSort.Style.Add("margin-left", "15px");
                    tableCell.Controls.Add(imageSort);
                }
            }
        }

        protected void gvLicencias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvLicencias, "Select$" + e.Row.RowIndex.ToString()));
            }
        }

        protected void gvLicencias_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["RealIndexLicencias"] = ((gvLicencias.PageIndex * gvLicencias.PageSize) + gvLicencias.SelectedIndex).ToString();

            if (SIDec.Properties.Settings.Default.Properties["DetalleOnClickLicencias"] != null)
            {
                if (SIDec.Properties.Settings.Default.DetalleOnClickLicencias)
                {
                    fLicenciasEstadoDetalle(false);
                    fLicenciasDetalle();
                }
            }
            fLicenciasValidarPermisos();
            fCuentaRegistros(lblLicenciasCuenta, gvLicencias, (DataSet)oVar.prDSLicenciasFiltro, btnFirstLicencias, btnBackLicencias, btnNextLicencias, btnLastLicencias, upLicenciasFoot, gvLicencias.SelectedIndex);
        }

        protected void gvLicencias_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = this.ViewState["SortExpression"].ToString();
            string sortDirection = this.ViewState["SortDirection"].ToString();

            if (sortExpression == e.SortExpression)
            {
                this.ViewState["SortDirection"] = (sortDirection == "ASC") ? "DESC" : "ASC";
            }
            else
            {
                this.ViewState["SortExpression"] = e.SortExpression;
                this.ViewState["SortDirection"] = "ASC";
            }

            DataView sortedView = new DataView(((DataSet)(oVar.prDSLicenciasFiltro)).Tables[0])
            {
                Sort = e.SortExpression + " " + sortDirection
            };
            Session["objects"] = sortedView;
            gvLicencias.DataSource = sortedView;
            gvLicencias.DataBind();

            //Almacenar el nuevo Dataset ordenado -nuevos Index-
            oVar.prDSLicenciasFiltro = oUtil.ConvertToDataSet(sortedView);
        }
        #endregion

        #region--------CONCEPTOS
        protected void gvConceptos_DataBinding(object sender, EventArgs e)
        {
            fActivarVistaGrid(mvConceptos, btnConceptosAccionFinal, btnConceptosCancelar);
        }

        protected void gvConceptos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvConceptos.PageIndex = e.NewPageIndex;
            fConceptosLoadGV(ViewState["CriterioBuscar"].ToString());
        }

        protected void gvConceptos_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header) return;

            string sortExpression = this.ViewState["SortExpression"].ToString();
            string sortDirection = this.ViewState["SortDirection"].ToString();

            foreach (System.Web.UI.WebControls.TableCell tableCell in e.Row.Cells)
            {
                if (!tableCell.HasControls()) continue;

                if (!(tableCell.Controls[0] is LinkButton lbSort)) continue;

                if (lbSort.CommandArgument == sortExpression)
                {
                    Image imageSort = new Image
                    {
                        ImageAlign = ImageAlign.AbsMiddle,
                        Width = 10
                    };

                    if (sortDirection == "ASC") imageSort.ImageUrl = "~/Images/icon/up.png";
                    else imageSort.ImageUrl = "~/Images/icon/down.png";


                    imageSort.Style.Add("margin-left", "15px");
                    tableCell.Controls.Add(imageSort);
                }
            }
        }

        protected void gvConceptos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvConceptos, "Select$" + e.Row.RowIndex.ToString()));
            }
        }

        protected void gvConceptos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["RealIndexConceptos"] = ((gvConceptos.PageIndex * gvConceptos.PageSize) + gvConceptos.SelectedIndex).ToString();

            if (SIDec.Properties.Settings.Default.Properties["DetalleOnClickConceptos"] != null)
            {
                if (SIDec.Properties.Settings.Default.DetalleOnClickConceptos)
                {
                    fConceptosDetalle();
                    fConceptosEstadoDetalle(false, false);
                }
            }
            fConceptosValidarPermisos();
            fCuentaRegistros(lblConceptosCuenta, gvConceptos, (DataSet)oVar.prDSConceptosFiltro, btnFirstConceptos, btnBackConceptos, btnNextConceptos, btnLastConceptos, upConceptosFoot, gvConceptos.SelectedIndex);
        }

        protected void gvConceptos_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = this.ViewState["SortExpression"].ToString();
            string sortDirection = this.ViewState["SortDirection"].ToString();

            if (sortExpression == e.SortExpression)
            {
                this.ViewState["SortDirection"] = (sortDirection == "ASC") ? "DESC" : "ASC";
            }
            else
            {
                this.ViewState["SortExpression"] = e.SortExpression;
                this.ViewState["SortDirection"] = "ASC";
            }

            DataView sortedView = new DataView(((DataSet)(oVar.prDSConceptosFiltro)).Tables[0])
            {
                Sort = e.SortExpression + " " + sortDirection
            };
            Session["objects"] = sortedView;
            gvConceptos.DataSource = sortedView;
            gvConceptos.DataBind();

            //Almacenar el nuevo Dataset ordenado -nuevos Index-
            oVar.prDSConceptosFiltro = oUtil.ConvertToDataSet(sortedView);
        }
        #endregion

        #region--------ACTOS ADMINISTRATIVOS
        protected void gvActosAdm_DataBinding(object sender, EventArgs e)
        {
            fActivarVistaGrid(mvActosAdm, btnActosAdmAccionFinal, btnActosAdmCancelar);
        }

        protected void gvActosAdm_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvActosAdm.PageIndex = e.NewPageIndex;
            fActosAdmLoadGV(ViewState["CriterioBuscar"].ToString());
        }

        protected void gvActosAdm_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "OpenDoc")
            {
                gvActosAdm.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                if (oFile.fVerificarFile(oFile.GetPathResoluciones(gvActosAdm.SelectedRow.Cells[2].Text, gvActosAdm.SelectedRow.Cells[3].Text)))
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "verDoc();", true);
                else
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Alert", "alert('No existe el archivo solicitado')", true);
            }
        }

        protected void gvActosAdm_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header) return;

            string sortExpression = this.ViewState["SortExpression"].ToString();
            string sortDirection = this.ViewState["SortDirection"].ToString();

            foreach (System.Web.UI.WebControls.TableCell tableCell in e.Row.Cells)
            {
                if (!tableCell.HasControls()) continue;

                if (!(tableCell.Controls[0] is LinkButton lbSort)) continue;

                if (lbSort.CommandArgument == sortExpression)
                {
                    Image imageSort = new Image
                    {
                        ImageAlign = ImageAlign.AbsMiddle,
                        Width = 10
                    };

                    if (sortDirection == "ASC") imageSort.ImageUrl = "~/Images/icon/up.png";
                    else imageSort.ImageUrl = "~/Images/icon/down.png";


                    imageSort.Style.Add("margin-left", "15px");
                    tableCell.Controls.Add(imageSort);
                }
            }
        }

        protected void gvActosAdm_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvActosAdm, "Select$" + e.Row.RowIndex.ToString()));
            }
        }

        protected void gvActosAdm_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["RealIndexActosAdm"] = ((gvActosAdm.PageIndex * gvActosAdm.PageSize) + gvActosAdm.SelectedIndex).ToString();

            if (SIDec.Properties.Settings.Default.Properties["DetalleOnClickActosAdm"] != null)
            {
                if (SIDec.Properties.Settings.Default.DetalleOnClickActosAdm)
                {
                    fActosAdmDetalle();
                    fActosAdmEstadoDetalle(false);
                }
            }
            fActosAdmValidarPermisos();
            fCuentaRegistros(lblActosAdmCuenta, gvActosAdm, (DataSet)oVar.prDSActosAdmFiltro, btnFirstActosAdm, btnBackActosAdm, btnNextActosAdm, btnLastActosAdm, upActosAdmFoot, gvActosAdm.SelectedIndex);
        }

        protected void gvActosAdm_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = this.ViewState["SortExpression"].ToString();
            string sortDirection = this.ViewState["SortDirection"].ToString();

            if (sortExpression == e.SortExpression)
            {
                this.ViewState["SortDirection"] = (sortDirection == "ASC") ? "DESC" : "ASC";
            }
            else
            {
                this.ViewState["SortExpression"] = e.SortExpression;
                this.ViewState["SortDirection"] = "ASC";
            }

            DataView sortedView = new DataView(((DataSet)(oVar.prDSActosAdmFiltro)).Tables[0])
            {
                Sort = e.SortExpression + " " + sortDirection
            };
            Session["objects"] = sortedView;
            gvActosAdm.DataSource = sortedView;
            gvActosAdm.DataBind();

            //Almacenar el nuevo Dataset ordenado -nuevos Index-
            oVar.prDSActosAdmFiltro = oUtil.ConvertToDataSet(sortedView);
        }
        #endregion
        #endregion

        #region--------------------------------------------------------------------MULTIVIEW
        protected void mvPredios_ActiveViewChanged(object sender, EventArgs e)
        {
            fBotonesNavegacionEstado(divPrediosNavegacion, Convert.ToBoolean(mvPredios.ActiveViewIndex));
        }

        protected void mvPrediosDec_ActiveViewChanged(object sender, EventArgs e)
        {
            fBotonesNavegacionEstado(divPrediosDecNavegacion, Convert.ToBoolean(mvPrediosDec.ActiveViewIndex));
        }

        protected void mvDocumentos_ActiveViewChanged(object sender, EventArgs e)
        {
            fBotonesNavegacionEstado(divDocumentosNavegacion, Convert.ToBoolean(mvDocumentos.ActiveViewIndex));
        }

        protected void mvPrestamos_ActiveViewChanged(object sender, EventArgs e)
        {
            fBotonesNavegacionEstado(divPrestamosNavegacion, Convert.ToBoolean(mvPrestamos.ActiveViewIndex));
            if (mvPrestamos.ActiveViewIndex == 1)
            {
                btnPrestamosAccionFinal.ValidationGroup = "vgPrestamos";
                btnPrestamosAccionFinal.OnClientClick = "if(Page_ClientValidate('vgPrestamos')) return ShowModalPopup('puPrestamos');";
            }
            else if (mvPrestamos.ActiveViewIndex == 2)
            {
                btnPrestamosAccionFinal.ValidationGroup = "vgPrestamos2";
                btnPrestamosAccionFinal.OnClientClick = "if(Page_ClientValidate('vgPrestamos2')) return ShowModalPopup('puPrestamos');";
            }
        }

        protected void mvPrediosPropietarios_ActiveViewChanged(object sender, EventArgs e)
        {
            fBotonesNavegacionEstado(divPrediosPropietariosNavegacion, Convert.ToBoolean(mvPrediosPropietarios.ActiveViewIndex));
        }

        protected void mvPropietarios_ActiveViewChanged(object sender, EventArgs e)
        {
            fBotonesNavegacionEstado(divPropietariosNavegacion, Convert.ToBoolean(mvPropietarios.ActiveViewIndex));
        }

        protected void mvObservaciones_ActiveViewChanged(object sender, EventArgs e)
        {
            fBotonesNavegacionEstado(divObservacionesNavegacion, Convert.ToBoolean(mvObservaciones.ActiveViewIndex));
        }

        protected void mvVisitas_ActiveViewChanged(object sender, EventArgs e)
        {
            fBotonesNavegacionEstado(divVisitasNavegacion, Convert.ToBoolean(mvVisitas.ActiveViewIndex));
        }

        protected void mvLicencias_ActiveViewChanged(object sender, EventArgs e)
        {
            fBotonesNavegacionEstado(divLicenciasNavegacion, Convert.ToBoolean(mvLicencias.ActiveViewIndex));
        }

        protected void mvConceptos_ActiveViewChanged(object sender, EventArgs e)
        {
            fBotonesNavegacionEstado(divConceptosNavegacion, Convert.ToBoolean(mvConceptos.ActiveViewIndex));
        }

        protected void mvActosAdm_ActiveViewChanged(object sender, EventArgs e)
        {
            fBotonesNavegacionEstado(divActosAdmNavegacion, Convert.ToBoolean(mvActosAdm.ActiveViewIndex));
        }
        #endregion

        #region--------------------------------------------------------------------METODOS
        #region------PREDIOS
        private void fPrediosLoadGV(string Parametro)
        {
            if (string.IsNullOrEmpty(Parametro))
                oVar.prDSPredios = null;
            else
                oVar.prDSPredios = oPredios.sp_s_predios(Parametro);

            fPrediosLoadGVInterno();
        }

        private void fPrediosLoadGVExterno()
        {
            if (hddProyectoPrediosChip.Value != "")
                Session["Proyecto.Predios.chip"] = hddProyectoPrediosChip.Value;

            if (Session["Proyecto.Predios.chip"] == null || Session["Proyecto.Predios.chip"].ToString() == "")
                return;
            hddProyectoPrediosChip.Value = Session["Proyecto.Predios.chip"].ToString();
            ViewState["CriterioBuscar"] = Session["Proyecto.Predios.chip"].ToString();
            Session["Proyecto.Predios.chip"] = null;
            btnBuscar.Visible = false;
            btnVolver.Visible = true;
            oVar.prDSPredios = oPredios.sp_s_predio_chip(hddProyectoPrediosChip.Value);
            gvPredios.DataSource = ((DataSet)(oVar.prDSPredios));
            gvPredios.DataBind();
            fPrediosLoadGV(ViewState["CriterioBuscar"].ToString());
            fActivarVistaGrid(mvPredios, btnPrediosAccionFinal, btnPrediosCancelar);
            txtBuscador.Attributes.Add("style", "display:none");
        }


        private void fPrediosLoadGVInterno()
        {
            gvPredios.DataSource = ((DataSet)(oVar.prDSPredios));
            gvPredios.DataBind();
            oVar.prDSPrediosFiltro = (DataSet)(oVar.prDSPredios);

            if (gvPredios.Rows.Count > 0)
            {
                gvPredios.SelectedIndex = 0;
                btnPrediosVD.Enabled = true;
                fPrediosDecLoadGV(gvPredios.SelectedDataKey.Value.ToString());
                gvPrediosDec.Visible = true;
                TabContainer1.Visible = true;
                btnPrediosEdit.Visible = true;
                btnPrediosDecEdit.Visible = true;
                btnPrediosDecFormato.Visible = true;
                btnPrediosDecFormato1.Visible = true;
                btnCartaExcel.Visible = true;
            }
            else
            {
                btnPrediosEdit.Visible = false;
                btnPrediosDecEdit.Visible = false;
                btnPrediosDecFormato.Visible = false;
                btnPrediosDecFormato1.Visible = false;
                btnCartaExcel.Visible = false;
                gvPrediosDec.Visible = false;
                TabContainer1.Visible = false;

                gvPrediosDec.DataSource = null;
                gvDocumentos.DataSource = null;
                gvPrestamos.DataSource = null;
                gvPrediosPropietarios.DataSource = null;
                gvObservaciones.DataSource = null;
                gvVisitas.DataSource = null;
                gvLicencias.DataSource = null;
                gvConceptos.DataSource = null;
                gvActosAdm.DataSource = null;

                gvPrediosDec.DataBind();
                gvDocumentos.DataBind();
                gvPrestamos.DataBind();
                gvPrediosPropietarios.DataBind();
                gvObservaciones.DataBind();
                gvVisitas.DataBind();
                gvLicencias.DataBind();
                gvConceptos.DataBind();
                gvActosAdm.DataBind();

                lblPrediosCuenta.Text = "";
                lblPrediosDecCuenta.Text = "";
                lblDocumentosCuenta.Text = "";
                lblPrestamosCuenta.Text = "";
                lblPrediosPropietariosCuenta.Text = "";
                lblObservacionesCuenta.Text = "";
                lblVisitasCuenta.Text = "";
                lblLicenciasCuenta.Text = "";
                lblConceptosCuenta.Text = "";
                lblActosAdmCuenta.Text = "";

                upPrediosFoot.Update();
                upPrediosDecFoot.Update();
                upDocumentosFoot.Update();
                upPrestamosFoot.Update();
                upPrediosPropietariosFoot.Update();
                upObservacionesFoot.Update();
                upVisitasFoot.Update();
                upLicenciasFoot.Update();
                upConceptosFoot.Update();
                upActosAdmFoot.Update();

                fAfectacionesLimpiarDetalle();
            }
            upPrediosDec.Update();
            upTab.Update();
            fCuentaRegistros(lblPrediosCuenta, gvPredios, (DataSet)oVar.prDSPrediosFiltro, btnFirstPredios, btnBackPredios, btnNextPredios, btnLastPredios, upPrediosFoot, 0);
            upPrediosFoot.Update();
        }

        private void fPrediosDetalle()
        {
            mvPredios.ActiveViewIndex = 1;
            int Indice = Convert.ToInt16(ViewState["RealIndexPredios"]);

            DataSet dsTmp = (DataSet)oVar.prDSPrediosFiltro;

            txt_chip.Text = dsTmp.Tables[0].Rows[Indice]["chip"].ToString();
            txt_matricula.Text = dsTmp.Tables[0].Rows[Indice]["matricula"].ToString();
            txt_cod_lote.Text = dsTmp.Tables[0].Rows[Indice]["cod_lote"].ToString();
            txt_direccion.Text = dsTmp.Tables[0].Rows[Indice]["direccion"].ToString();

            txt_area_terreno_UAECD.Text = dsTmp.Tables[0].Rows[Indice]["area_terreno_UAECD"].ToString();
            txt_area_terreno_folio.Text = dsTmp.Tables[0].Rows[Indice]["area_terreno_folio"].ToString();
            txt_area_construccion.Text = dsTmp.Tables[0].Rows[Indice]["area_construccion"].ToString();

            fCuentaRegistros(lblPrediosCuenta, gvPredios, (DataSet)oVar.prDSPrediosFiltro, btnFirstPredios, btnBackPredios, btnNextPredios, btnLastPredios, upPrediosFoot, Indice);
        }

        private void fPrediosEstadoDetalle(bool HabilitarCampos)
        {
            txt_chip.Enabled = false;
            txt_matricula.Enabled = false;
            txt_cod_lote.Enabled = false;
            txt_direccion.Enabled = false;
            txt_area_terreno_UAECD.Enabled = HabilitarCampos;
            txt_area_terreno_folio.Enabled = HabilitarCampos;
            txt_area_construccion.Enabled = HabilitarCampos;
        }

        private void fPrediosLimpiarDetalle()
        {
            txt_chip.Text = "";
            txt_matricula.Text = "";
            txt_cod_lote.Text = "";
            txt_direccion.Text = "";

            txt_area_terreno_UAECD.Text = "";
            txt_area_terreno_folio.Text = "";
            txt_area_construccion.Text = "";
        }

        private void fPrediosUpdate()
        {
            if (HayCambiosPredios == false)
            {
                fMensajeCRUD(clConstantes.MSG_SIN_CAMBIOS, (int)clConstantes.NivelMensaje.Alerta, _DIVMSGPREDIOS, upPredios);
                fPrediosLimpiarDetalle();
            }
            else
            {
                string strResultado = oPredios.sp_u_predio(
                  txt_chip.Text,
                  txt_area_terreno_UAECD.Text,
                  txt_area_terreno_folio.Text,
                  txt_area_construccion.Text);
                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fPrediosUpdate:", clConstantes.MSG_OK_U);
                    fPrediosLimpiarDetalle();
                    oVar.prDSPredios = oPredios.sp_s_predios(ViewState["CriterioBuscar"].ToString());
                    fMensajeCRUD(clConstantes.MSG_OK_U, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGPREDIOS, upPredios);
                }
                else
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fPrediosUpdate:", clConstantes.MSG_ERR_U);
                    fMensajeCRUD(clConstantes.MSG_ERR_U, (int)clConstantes.NivelMensaje.Error, _DIVMSGPREDIOS, upPredios);
                }
            }
        }
        #endregion

        #region------PREDIOS DECLARADOS
        private void fPrediosDecLoadGV(string Parametro)
        {
            oVar.prDSPrediosDec = oPrediosDeclarados.sp_s_predios_dec_chip(Parametro);
            DataSet odsClonePrediosDec = ((DataSet)(oVar.prDSPrediosDec)).Clone();
            string strQuery;

            if (!string.IsNullOrEmpty(Parametro))
            {
                strQuery = string.Format("chip = '{0}'", Parametro);
                DataRow[] oDr = ((DataSet)oVar.prDSPrediosDec).Tables[0].Select(strQuery);
                foreach (DataRow row in oDr)
                {
                    odsClonePrediosDec.Tables[0].ImportRow(row);
                }
                gvPrediosDec.DataSource = odsClonePrediosDec;
                gvPrediosDec.DataBind();
                oVar.prDSPrediosDecFiltro = odsClonePrediosDec;
            }
            else
            {
                gvPrediosDec.DataSource = ((DataSet)(oVar.prDSPrediosDec));
                gvPrediosDec.DataBind();
                oVar.prDSPrediosDecFiltro = (DataSet)(oVar.prDSPrediosDec);
            }

            if (gvPrediosDec.Rows.Count > 0)
            {
                gvPrediosDec.SelectedIndex = 0;
                btnPrediosDecVG.Enabled = true;

                oVar.prUserResponsablePredioDec = Convert.ToInt16(gvPrediosDec.SelectedDataKey["cod_usu_responsable"].ToString());

                string cod_predio_declarado = gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString();
                string idarchivo = gvPrediosDec.SelectedDataKey.Values["idarchivo"].ToString();
                string cod_usu_responsable = IsHelperUser() ? oVar.prUserCod.ToString() : gvPrediosDec.SelectedDataKey.Values["cod_usu_responsable"].ToString();

                fDocumentosLoadGV(cod_predio_declarado);
                fPrestamosLoadGV(cod_predio_declarado);
                fAfectacionesLoad(gvPrediosDec.SelectedRow.Cells[1].Text);
                fPrediosPropietariosLoadGV(gvPrediosDec.SelectedRow.Cells[1].Text);
                fObservacionesLoadGV(cod_predio_declarado);
                fVisitasLoadGV(cod_predio_declarado);
                fLicenciasLoadGV(cod_predio_declarado);
                fConceptosLoadGV(cod_predio_declarado);
                fActosAdmLoadGV(cod_predio_declarado);
                fFichaPredialGV(cod_predio_declarado, idarchivo, cod_usu_responsable);
                LoadInteresados(cod_predio_declarado, cod_usu_responsable);
                LoadCartas(cod_predio_declarado, cod_usu_responsable);
            }
            else
            {
                gvPrediosDec.DataSource = null;
                gvPrediosDec.DataBind();
            }

            fPrediosDecValidarPermisos();
            upPrediosDecBtnVistas.Update();
            upPrediosDec.Update();
            fCuentaRegistros(lblPrediosDecCuenta, gvPrediosDec, (DataSet)oVar.prDSPrediosDecFiltro, btnFirstPrediosDec, btnBackPrediosDec, btnNextPrediosDec, btnLastPrediosDec, upPrediosDecFoot, 0);
        }

        private void fPrediosDecDetalle()
        {
            mvPrediosDec.ActiveViewIndex = 1;
            int Indice = Convert.ToInt16(ViewState["RealIndexPrediosDec"]);

            DataSet dsTmp = (DataSet)oVar.prDSPrediosDecFiltro;

            txt_chip_PrediosDec.Text = dsTmp.Tables[0].Rows[Indice]["chip"].ToString();
            txt_resolucion_declaratoria.Text = dsTmp.Tables[0].Rows[Indice]["resolucion_declaratoria"].ToString();
            txt_numero_caja.Text = dsTmp.Tables[0].Rows[Indice]["numero_caja"].ToString();
            txt_numero_carpetas.Text = dsTmp.Tables[0].Rows[Indice]["numero_carpetas"].ToString();
            txt_posicion_carpeta.Text = dsTmp.Tables[0].Rows[Indice]["posicion_carpeta"].ToString();
            chk_recibe_carta_terminos.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["recibe_carta_terminos"].ToString()));
            oBasic.fLoadUsuariosFiltro(ddlb_cod_usu_responsable, 2, dsTmp.Tables[0].Rows[Indice]["cod_usu_responsable"].ToString());
            txt_obs_predio_declarado.Text = dsTmp.Tables[0].Rows[Indice]["obs_predio_declarado"].ToString();

            fCuentaRegistros(lblPrediosDecCuenta, gvPrediosDec, (DataSet)oVar.prDSPrediosDecFiltro, btnFirstPrediosDec, btnBackPrediosDec, btnNextPrediosDec, btnLastPrediosDec, upPrediosDecFoot, Indice);
        }

        private void fPrediosDecEstadoDetalle(bool HabilitarCampos)
        {
            txt_chip_PrediosDec.Enabled = false;
            txt_resolucion_declaratoria.Enabled = false;
            txt_numero_caja.Enabled = HabilitarCampos;
            txt_numero_carpetas.Enabled = HabilitarCampos;
            txt_posicion_carpeta.Enabled = HabilitarCampos;
            chk_recibe_carta_terminos.Enabled = HabilitarCampos;
            if (oVar.prUserAsignaUsuarioPredios.ToString() == "1")
            {
                ddlb_cod_usu_responsable.Enabled = HabilitarCampos;
                oBasic.StyleCtlLbl("E", true, ddlb_cod_usu_responsable, lbl_cod_usu_responsable, false);
            }
            else
            {
                ddlb_cod_usu_responsable.Enabled = false;
                oBasic.StyleCtlLbl("E", false, ddlb_cod_usu_responsable, lbl_cod_usu_responsable, false);
            }
            txt_obs_predio_declarado.Enabled = HabilitarCampos;
        }

        private void fPrediosDecLimpiarDetalle()
        {
        }

        private void fPrediosDecUpdate()
        {
            if (HayCambiosPrediosDec == false)
            {
                fMensajeCRUD(clConstantes.MSG_SIN_CAMBIOS, (int)clConstantes.NivelMensaje.Alerta, _DIVMSGPREDIOSDEC, upPrediosDec);
                fPrediosDecLimpiarDetalle();
            }
            else
            {
                string strResultado = oPrediosDeclarados.sp_u_predio_declarado(
                  gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString(),
                  oBasic.fInt2(txt_numero_caja),
                  oBasic.fInt2(txt_numero_carpetas),
                  oBasic.fInt2(txt_posicion_carpeta),
                  chk_recibe_carta_terminos.Checked,
                  oBasic.fInt2(ddlb_cod_usu_responsable),
                  txt_obs_predio_declarado.Text);
                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fPrediosDecUpdate:", clConstantes.MSG_OK_U);
                    fPrediosDecLimpiarDetalle();
                    oVar.prDSPrediosDec = oPrediosDeclarados.sp_s_predios_dec_chip(txt_chip_PrediosDec.Text);
                    fMensajeCRUD(clConstantes.MSG_OK_U, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGPREDIOSDEC, upPrediosDec);
                }
                else
                {
                    oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fPrediosDecUpdate");
                    fMensajeCRUD(clConstantes.MSG_ERR_U, (int)clConstantes.NivelMensaje.Error, _DIVMSGPREDIOSDEC, upPrediosDec);
                }
            }
        }

        private void fPrediosDecValidarPermisos()
        {
            oBasic.EnableButton(false, btnPrediosDecEdit);

            if (oVar.prUserResponsablePredioDec.ToString() == oVar.prUserCod.ToString() || oVar.prUserAsignaUsuarioPredios.ToString() == "1" || IsHelperUser())
            {
                if (oPermisos.TienePermisosSP("sp_u_predio_declarado"))
                    oBasic.EnableButton(true, btnPrediosDecEdit);
            }
        }
        #endregion

        #region------DOCUMENTOS
        private void fDocumentosLoadGV(string Parametro)
        {
            oVar.prDSDocumentos = oDocumentos.sp_s_documentos_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
            DataSet odsCloneDocumentos = ((DataSet)(oVar.prDSDocumentos)).Clone();

            if (!string.IsNullOrEmpty(Parametro))
            {
                string strQuery = string.Format("cod_predio_declarado = '{0}'", Parametro);
                DataRow[] oDr = ((DataSet)oVar.prDSDocumentos).Tables[0].Select(strQuery);
                foreach (DataRow row in oDr)
                {
                    odsCloneDocumentos.Tables[0].ImportRow(row);
                }
                gvDocumentos.DataSource = odsCloneDocumentos;
                gvDocumentos.DataBind();
                oVar.prDSDocumentosFiltro = odsCloneDocumentos;
            }
            else
            {
                gvDocumentos.DataSource = ((DataSet)(oVar.prDSDocumentos));
                gvDocumentos.DataBind();
                oVar.prDSDocumentosFiltro = (DataSet)(oVar.prDSDocumentos);
            }

            if (gvDocumentos.Rows.Count > 0)
            {
                gvDocumentos.SelectedIndex = 0;
                btnDocumentosVG.Enabled = true;
            }

            fDocumentosValidarPermisos();
            mvDocumentos.ActiveViewIndex = 0;
            fBotonAccionFinalEstado(btnDocumentosAccionFinal, btnDocumentosCancelar, false);
            upDocumentosBtnVistas.Update();
            upDocumentos.Update();
            fCuentaRegistros(lblDocumentosCuenta, gvDocumentos, (DataSet)oVar.prDSDocumentosFiltro, btnFirstDocumentos, btnBackDocumentos, btnNextDocumentos, btnLastDocumentos, upDocumentosFoot, 0);
        }

        private void fDocumentosLimpiarDetalle()
        {
            txt_au_documento.Text = "";
            txt_numero_carpeta.Text = "";
            txt_orden_documento.Text = "";
            txt_tipo_documento.Text = "";
            txt_radicado_documento.Text = "N/A";
            txt_fecha_radicado_documento.Text = "";
            txt_folio_inicial_documento.Text = "";
            txt_folios_documento.Text = "";
            txt_folio_final_documento.Text = "";
            txt_obs_documento.Text = "Documento en buen estado.";
            ddlb_cod_usu.SelectedIndex = -1;
        }

        private void fDocumentosDetalle()
        {
            if (gvDocumentos.Rows.Count > 0)
            {
                mvDocumentos.ActiveViewIndex = 1;
                int Indice = Convert.ToInt16(ViewState["RealIndexDocumentos"]);

                DataSet dsTmp = (DataSet)oVar.prDSDocumentosFiltro;

                txt_au_documento.Text = dsTmp.Tables[0].Rows[Indice]["au_documento"].ToString();
                txt_numero_carpeta.Text = dsTmp.Tables[0].Rows[Indice]["numero_carpeta"].ToString();
                txt_orden_documento.Text = dsTmp.Tables[0].Rows[Indice]["orden_documento"].ToString();
                txt_tipo_documento.Text = dsTmp.Tables[0].Rows[Indice]["tipo_documento"].ToString();
                txt_radicado_documento.Text = dsTmp.Tables[0].Rows[Indice]["radicado_documento"].ToString();
                oBasic.fDetalleFecha(txt_fecha_radicado_documento, dsTmp.Tables[0].Rows[Indice]["fecha_radicado_documento"].ToString());
                txt_folio_inicial_documento.Text = dsTmp.Tables[0].Rows[Indice]["folio_inicial_documento"].ToString();
                txt_folios_documento.Text = dsTmp.Tables[0].Rows[Indice]["folios_documento"].ToString();
                txt_folio_final_documento.Text = dsTmp.Tables[0].Rows[Indice]["folio_final_documento"].ToString();
                txt_obs_documento.Text = dsTmp.Tables[0].Rows[Indice]["obs_documento"].ToString();
                oBasic.fLoadUsuariosFiltro(ddlb_cod_usu, 4, dsTmp.Tables[0].Rows[Indice]["cod_usu"].ToString());

                fCuentaRegistros(lblDocumentosCuenta, gvDocumentos, (DataSet)oVar.prDSDocumentosFiltro, btnFirstDocumentos, btnBackDocumentos, btnNextDocumentos, btnLastDocumentos, upDocumentosFoot, Indice);
            }
        }

        private void fDocumentosEstadoDetalle(bool HabilitarCampos, int CodAccion)
        {
            txt_au_documento.Enabled = false;
            txt_orden_documento.Enabled = false;
            txt_tipo_documento.Enabled = HabilitarCampos;
            txt_radicado_documento.Enabled = HabilitarCampos;
            txt_fecha_radicado_documento.Enabled = HabilitarCampos;

            if (CodAccion == (int)clConstantes.AccionRol.MODIFICAR)
                txt_numero_carpeta.Enabled = true;
            else
                txt_numero_carpeta.Enabled = false;

            lbSubirDoc.Visible = HabilitarCampos;
            txt_folio_inicial_documento.Enabled = HabilitarCampos;
            txt_folios_documento.Enabled = HabilitarCampos;
            txt_folio_final_documento.Enabled = false;
            txt_obs_documento.Enabled = HabilitarCampos;
            ddlb_cod_usu.Enabled = false;
            lbSubirDoc.Visible = HabilitarCampos;
            FileUpload1.Visible = false;
        }

        private void fDocumentosInsert()
        {
            string strResultado = oDocumentos.sp_i_documento(
            gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString(),
            txt_tipo_documento.Text,
            txt_radicado_documento.Text,
            txt_fecha_radicado_documento.Text,
            oBasic.fInt2(txt_folio_inicial_documento),
            oBasic.fInt2(txt_folios_documento),
            txt_obs_documento.Text,
            oBasic.fInt2(ddlb_cod_usu),
            FileUpload1.HasFile);
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                string au = strResultado.Substring(6);
                string pathFile = oVar.prPathDocumentos.ToString() + "\\" + gvPrediosDec.SelectedRow.Cells[0].Text + "\\" + au + ".pdf";
                oBasic.LoadPdf(FileUpload1, pathFile);
                if (oVar.prError.ToString() == clConstantes.FILE_ERR_LOAD)
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fDocumentosInsert:", clConstantes.MSG_ERR_FILELOAD);
                    fMensajeCRUD(clConstantes.MSG_ERR_FILELOAD, (int)clConstantes.NivelMensaje.Error, _DIVMSGDOCUMENTOS, upDocumentos);
                }
                else
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fDocumentosInsert:", clConstantes.DB_ACTION_OK);
                    oVar.prDSDocumentos = oDocumentos.sp_s_documentos_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                    fMensajeCRUD(clConstantes.MSG_OK_I, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGDOCUMENTOS, upDocumentos);
                }
            }
            else
            {
                oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fDocumentosInsert");
                fMensajeCRUD(clConstantes.MSG_ERR_I, (int)clConstantes.NivelMensaje.Error, _DIVMSGDOCUMENTOS, upDocumentos);
            }
        }

        private void fDocumentosUpdate()
        {
            oVar.prError = "0";
            if (FileUpload1.HasFile)
            {
                string pathFile = oVar.prPathDocumentos.ToString() + "\\" + gvPrediosDec.SelectedRow.Cells[0].Text + "\\" + txt_au_documento.Text + ".pdf";
                oBasic.LoadPdf(FileUpload1, pathFile);
            }

            if (oVar.prError.ToString() == clConstantes.FILE_ERR_LOAD)
            {
                oLog.RegistrarLogInfo(_SOURCEPAGE, "fDocumentosUpdate:", clConstantes.MSG_ERR_FILELOAD);
                fMensajeCRUD(clConstantes.MSG_ERR_FILELOAD, (int)clConstantes.NivelMensaje.Error, _DIVMSGDOCUMENTOS, upDocumentos);
            }
            else if (HayCambiosDocumentos || FileUpload1.HasFile)
            {
                string strResultado = oDocumentos.sp_u_documento(
                    oBasic.fInt2(txt_au_documento),
                    oBasic.fInt2(txt_numero_carpeta),
                    txt_tipo_documento.Text,
                    txt_radicado_documento.Text,
                    txt_fecha_radicado_documento.Text,
                    oBasic.fInt2(txt_folio_inicial_documento),
                    oBasic.fInt2(txt_folios_documento),
                    txt_obs_documento.Text,
                    oVar.prUserCod.ToString(),
                    FileUpload1.HasFile);

                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_FILELOAD)
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fDocumentosUpdate:", clConstantes.MSG_OK_FILELOAD);
                    oVar.prDSDocumentos = oDocumentos.sp_s_documentos_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                    fMensajeCRUD(clConstantes.MSG_OK_FILELOAD, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGDOCUMENTOS, upDocumentos);
                }
                else if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fDocumentosUpdate:", clConstantes.MSG_OK_U);
                    oVar.prDSDocumentos = oDocumentos.sp_s_documentos_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                    fMensajeCRUD(clConstantes.MSG_OK_U, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGDOCUMENTOS, upDocumentos);
                }
                else if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_ERR_DATOS)
                {
                    oVar.prDSDocumentos = oDocumentos.sp_s_documentos_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                    fMensajeCRUD(clConstantes.MSG_ERR_DATOS, (int)clConstantes.NivelMensaje.Alerta, _DIVMSGDOCUMENTOS, upDocumentos);
                }
                else
                {
                    oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fDocumentosUpdate");
                    fMensajeCRUD(clConstantes.MSG_ERR_U, (int)clConstantes.NivelMensaje.Error, _DIVMSGDOCUMENTOS, upDocumentos);
                }
            }
            else
            {
                fMensajeCRUD(clConstantes.MSG_SIN_CAMBIOS, (int)clConstantes.NivelMensaje.Alerta, _DIVMSGDOCUMENTOS, upDocumentos);
                fDocumentosLimpiarDetalle();
            }
        }

        private void fDocumentosDelete()
        {
            string strResultado = oDocumentos.sp_d_documento(gvDocumentos.SelectedDataKey.Value.ToString());

            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                string pathFile = oVar.prPathDocumentos.ToString() + "\\" + gvPrediosDec.SelectedRow.Cells[0].Text + "\\" + gvDocumentos.SelectedDataKey.Value.ToString() + ".pdf";
                oBasic.FileDelete(pathFile);
                oLog.RegistrarLogInfo(_SOURCEPAGE, "fDocumentosDelete:", clConstantes.MSG_OK_D);
                fDocumentosLimpiarDetalle();

                if (Convert.ToInt16(ViewState["RealIndexDocumentos"]) > 0)
                    ViewState["RealIndexDocumentos"] = Convert.ToInt16(ViewState["RealIndexDocumentos"]) - 1;
                else
                    ViewState["RealIndexDocumentos"] = 0;

                fMensajeCRUD(clConstantes.MSG_OK_D, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGDOCUMENTOS, upDocumentos);
            }
            else if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_ERR_PERMISO)
            {
                oLog.RegistrarLogInfo(_SOURCEPAGE, "fDocumentosDelete:", clConstantes.MSG_ERR_PERMISO);
                fMensajeCRUD(clConstantes.MSG_ERR_PERMISO, (int)clConstantes.NivelMensaje.Error, _DIVMSGDOCUMENTOS, upDocumentos);
            }
            else
            {
                oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fDocumentosDelete");
                fMensajeCRUD(clConstantes.MSG_ERR_D, (int)clConstantes.NivelMensaje.Error, _DIVMSGDOCUMENTOS, upDocumentos);
            }
        }

        private void fDocumentosReorder()
        {
            string strResultado = "";
            if (rbDocumentosOpcion.SelectedIndex == 0)
            {
                strResultado = oDocumentos.sp_u_documentos_reorder(
                    "1",
                    gvDocumentos.SelectedDataKey.Value.ToString(),
                    oBasic.fInt2(txt_folio_inicial_nuevo)
                );
            }
            else if (rbDocumentosOpcion.SelectedIndex == 1)
            {
                if (Convert.ToInt16(txt_folio_inicial_nuevo.Text) <= Convert.ToInt16(gvDocumentos.Rows[gvDocumentos.SelectedIndex].Cells[8].Text))
                {
                    strResultado = clConstantes.DB_ACTION_ERR_ORDER;
                    oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fDocumentosReorder");
                    fMensajeCRUD(clConstantes.MSG_ERR_ORDEN, (int)clConstantes.NivelMensaje.Error, _DIVMSGDOCUMENTOS, upDocumentos);
                    return;
                }
                else
                {
                    strResultado = oDocumentos.sp_u_documentos_reorder(
                        "2",
                        gvDocumentos.SelectedDataKey.Value.ToString(),
                        oBasic.fInt2(txt_folio_inicial_nuevo)
                    );
                }
            }
            else if (rbDocumentosOpcion.SelectedIndex == 2)
            {
                strResultado = oDocumentos.sp_u_documentos_reorder(
                    "3",
                    gvDocumentos.SelectedDataKey.Value.ToString(),
                    "0"
                );
            }

            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                oLog.RegistrarLogInfo(_SOURCEPAGE, "fDocumentosReorder:", clConstantes.DB_ACTION_OK);
                oVar.prDSDocumentos = oDocumentos.sp_s_documentos_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                fMensajeCRUD(clConstantes.MSG_OK_U, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGDOCUMENTOS, upDocumentos);
                string i = strResultado.Substring(6);
                ViewState["RealIndexDocumentos"] = (Convert.ToInt16(ViewState["RealIndexDocumentos"]) - Convert.ToInt16(i) - 1).ToString();
            }
            else
            {
                oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fDocumentosReorder");
                fMensajeCRUD(clConstantes.MSG_ERR_U, (int)clConstantes.NivelMensaje.Error, _DIVMSGDOCUMENTOS, upDocumentos);
            }
        }

        private void fDocumentosMove(int parametro)
        {
            int iCurrentIndex = gvDocumentos.SelectedIndex;
            int iMoveIndex = iCurrentIndex + parametro;
            if (iMoveIndex < 0)
                return;

            string sAutonum;
            string sFolio;
            if (parametro == -1)
            {
                sAutonum = gvDocumentos.DataKeys[iCurrentIndex].Value.ToString();
                sFolio = gvDocumentos.Rows[iMoveIndex].Cells[8].Text.ToString();
            }
            else
            {
                sAutonum = gvDocumentos.DataKeys[iMoveIndex].Value.ToString();
                sFolio = gvDocumentos.Rows[iCurrentIndex].Cells[8].Text.ToString();
            }

            string strResultado = oDocumentos.sp_u_documentos_reorder(
                "1",
                sAutonum,
                sFolio
            );
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                oLog.RegistrarLogInfo(_SOURCEPAGE, "fDocumentosReorder:", clConstantes.DB_ACTION_OK);
                oVar.prDSDocumentos = oDocumentos.sp_s_documentos_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                ViewState["RealIndexDocumentos"] = (Convert.ToInt16(ViewState["RealIndexDocumentos"]) + parametro).ToString();
            }
            else
            {
                oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fDocumentosReorder");
                fMensajeCRUD(clConstantes.MSG_ERR_U, (int)clConstantes.NivelMensaje.Error, _DIVMSGDOCUMENTOS, upDocumentos);
            }
        }

        private void fDocumentosValidarPermisos()
        {
            oBasic.EnableButton(false, btnDocumentosAdd);
            oBasic.EnableButton(false, btnDocumentosEdit);
            oBasic.EnableButton(false, btnDocumentosDel);
            oBasic.EnableButton(false, btnDocumentosMoveUp);
            oBasic.EnableButton(false, btnDocumentosMoveDown);
            oBasic.EnableButton(false, btnDocumentosReorder);

            if (oVar.prUserResponsablePredioDec.ToString() != oVar.prUserCod.ToString() && !IsHelperUser())
            {
                if (Convert.ToBoolean(Convert.ToInt16(oVar.prUserEditaDocumentos.ToString())))
                {
                    if (oPermisos.TienePermisosSP("sp_i_documento"))
                        oBasic.EnableButton(true, btnDocumentosAdd);

                    if (oPermisos.TienePermisosSP("sp_u_documento"))
                        oBasic.EnableButton(true, btnDocumentosEdit);

                    if (oPermisos.TienePermisosSP("sp_d_documento") && Convert.ToBoolean(Convert.ToInt16(oVar.prUserEliminaDocumentos.ToString())))
                        oBasic.EnableButton(true, btnDocumentosDel);

                    if (oPermisos.TienePermisosSP("sp_u_documentos_ajustar"))
                    {
                        oBasic.EnableButton(true, btnDocumentosMoveUp);
                        oBasic.EnableButton(true, btnDocumentosMoveDown);
                    }
                    if (oPermisos.TienePermisosSP("sp_u_documentos_reorder"))
                        oBasic.EnableButton(true, btnDocumentosReorder);
                }
            }
            else
            {
                if (oPermisos.TienePermisosSP("sp_i_documento"))
                    oBasic.EnableButton(true, btnDocumentosAdd);

                if (gvDocumentos.Rows.Count > 0)
                {
                    if (oPermisos.TienePermisosSP("sp_u_documento"))
                        oBasic.EnableButton(true, btnDocumentosEdit);
                    if (oPermisos.TienePermisosSP("sp_d_documento"))
                        oBasic.EnableButton(true, btnDocumentosDel);
                }
            }

        }

        public bool IsHelperUser()
        {
            bool HelperUser = false;
            if (dsColaborators == null)
            {
                dsColaborators = oPrediosDeclarados.sp_s_predio_dec_colaboradores(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
            }
            if(dsColaborators != null) { 
                string strQuery = string.Format("cod_usuario = '{0}'", oVar.prUserCod.ToString());
                DataRow[] oDr = ((DataSet)dsColaborators).Tables[0].Select(strQuery);

                HelperUser = (oDr.Length > 0);
            }
            return HelperUser;
        }

        protected void Documentos_folios_TextChanged(object sender, EventArgs e)
        {
            txt_folio_final_documento.Text = (Convert.ToInt32(txt_folio_inicial_documento.Text) + Convert.ToInt32(txt_folios_documento.Text)).ToString();
            HayCambiosDocumentos = true;
        }
        #endregion

        #region------PRESTAMOS
        private void fPrestamosLoadGV(string p_cod_predio_declarado)
        {
            if (string.IsNullOrEmpty(p_cod_predio_declarado))
                p_cod_predio_declarado = "0";

            oVar.prDSPrestamosUltimo = oPrestamos.sp_s_prestamos_cod_predio_ultimo(p_cod_predio_declarado);
            oVar.prDSPrestamos = oPrestamos.sp_s_prestamos_cod_predio(p_cod_predio_declarado);
            gvPrestamos.DataSource = ((DataSet)(oVar.prDSPrestamos));
            gvPrestamos.DataBind();

            if (gvPrestamos.Rows.Count > 0)
            {
                gvPrestamos.SelectedIndex = 0;
                btnPrestamosVG.Enabled = true;
            }

            oBasic.StyleCtlLbl("V", false, ddlb_id_area_solicita_prestamo2, lbl_id_area_solicita_prestamo2, true);
            oBasic.StyleCtlLbl("V", false, ddlb_cod_usu_solicita_prestamo2, lbl_cod_usu_solicita_prestamo2, true);
            oBasic.StyleCtlLbl("V", false, txt_memorando_interno2, lbl_memorando_interno2, true);
            oBasic.StyleCtlLbl("V", false, txt_fecha_entrega_prestamo2, lbl_fecha_entrega_prestamo2, true);
            oBasic.StyleCtlLbl("V", false, ddlb_cod_usu_entrega_prestamo2, lbl_cod_usu_entrega_prestamo2, true);
            oBasic.StyleCtlLbl("V", false, txt_fecha_devolucion_prestamo2, lbl_fecha_devolucion_prestamo2, true);
            oBasic.StyleCtlLbl("V", false, ddlb_cod_usu_recibe_prestamo2, lbl_cod_usu_recibe_prestamo2, true);
            oBasic.StyleCtlLbl("V", false, txt_obs_prestamo2, lbl_obs_prestamo2, true);

            mvPrestamos.ActiveViewIndex = 0;
            fBotonAccionFinalEstado(btnPrestamosAccionFinal, btnPrestamosCancelar, false);

            fPrestamosValidarPermisos();
            upPrestamosBtnVistas.Update();
            upPrestamos.Update();
            fCuentaRegistros(lblPrestamosCuenta, gvPrestamos, (DataSet)oVar.prDSPrestamos, btnFirstPrestamos, btnBackPrestamos, btnNextPrestamos, btnLastPrestamos, upPrestamosFoot, 0);
        }

        private void fPrestamosLimpiarDetalle()
        {
            txt_au_prestamo.Text = "";
            txt_cod_predio_declarado_prestamo.Text = "";
            ddlb_id_area_solicita_prestamo.SelectedIndex = -1;
            txt_memorando_interno.Text = "";
            ddlb_cod_usu_solicita_prestamo.SelectedIndex = -1;
            txt_fecha_entrega_prestamo.Text = "";
            ddlb_cod_usu_entrega_prestamo.SelectedIndex = -1;
            txt_folios_prestamo.Text = "";
            txt_fecha_devolucion_prestamo.Text = "";
            ddlb_cod_usu_recibe_prestamo.SelectedIndex = -1;
            txt_obs_prestamo.Text = "";
        }

        private void fPrestamosLimpiarDetalle2()
        {
            mvPrestamos.ActiveViewIndex = 2;
            ddlb_id_tipo_prestamo.SelectedIndex = -1;
            ddlb_id_area_solicita_prestamo2.SelectedIndex = -1;
            txt_memorando_interno2.Text = "";
            ddlb_cod_usu_solicita_prestamo2.SelectedIndex = -1;
            txt_fecha_entrega_prestamo2.Text = "";
            ddlb_cod_usu_entrega_prestamo2.SelectedIndex = -1;
            txt_fecha_devolucion_prestamo2.Text = "";
            ddlb_cod_usu_recibe_prestamo2.SelectedIndex = -1;
            txt_obs_prestamo2.Text = "";
            txt_chip_filtro.Text = "";
            lbx_cod_predio_declarado.Items.Clear();
            lbx_cod_predio_declarado_add.Items.Clear();
            lbx_cod_predio_declarado_add2.Items.Clear();
            lbx_cod_predio_declarado_add2.Visible = false;
        }

        private void fPrestamosDetalle()
        {
            mvPrestamos.ActiveViewIndex = 1;
            int Indice = Convert.ToInt16(ViewState["RealIndexPrestamos"]);

            DataSet dsTmp = (DataSet)oVar.prDSPrestamos;

            txt_au_prestamo.Text = dsTmp.Tables[0].Rows[Indice]["au_prestamo"].ToString();
            txt_cod_predio_declarado_prestamo.Text = dsTmp.Tables[0].Rows[Indice]["cod_predio_declarado"].ToString();
            oBasic.fDetalleDropDown(ddlb_id_area_solicita_prestamo, dsTmp.Tables[0].Rows[Indice]["id_area_solicita_prestamo"].ToString());
            oBasic.fLoadUsuariosFiltro(ddlb_cod_usu_solicita_prestamo, 1, dsTmp.Tables[0].Rows[Indice]["cod_usu_solicita_prestamo"].ToString());
            txt_memorando_interno.Text = dsTmp.Tables[0].Rows[Indice]["memorando_interno"].ToString();
            oBasic.fDetalleFecha(txt_fecha_entrega_prestamo, dsTmp.Tables[0].Rows[Indice]["fecha_entrega_prestamo"].ToString());
            oBasic.fLoadUsuariosFiltro(ddlb_cod_usu_entrega_prestamo, 1, dsTmp.Tables[0].Rows[Indice]["cod_usu_entrega_prestamo"].ToString());
            txt_folios_prestamo.Text = dsTmp.Tables[0].Rows[Indice]["folios_prestamo"].ToString();
            oBasic.fDetalleFecha(txt_fecha_devolucion_prestamo, dsTmp.Tables[0].Rows[Indice]["fecha_devolucion_prestamo"].ToString());
            oBasic.fLoadUsuariosFiltro(ddlb_cod_usu_recibe_prestamo, 1, dsTmp.Tables[0].Rows[Indice]["cod_usu_recibe_prestamo"].ToString());
            txt_obs_prestamo.Text = dsTmp.Tables[0].Rows[Indice]["obs_prestamo"].ToString();

            fCuentaRegistros(lblPrestamosCuenta, gvPrestamos, (DataSet)oVar.prDSPrestamos, btnFirstPrestamos, btnBackPrestamos, btnNextPrestamos, btnLastPrestamos, upPrestamosFoot, Indice);
        }

        private void fPrestamosEstadoDetalle(bool HabilitarCampos, int CodAccion)
        {
            bool HabilitarCampo = false;
            if (CodAccion == (int)clConstantes.AccionRol.CREAR)
                HabilitarCampo = true;

            txt_au_prestamo.Enabled = false;
            txt_cod_predio_declarado_prestamo.Enabled = false;
            ddlb_id_area_solicita_prestamo.Enabled = HabilitarCampo;
            ddlb_cod_usu_solicita_prestamo.Enabled = HabilitarCampo;
            txt_fecha_entrega_prestamo.Enabled = HabilitarCampo;
            ddlb_cod_usu_entrega_prestamo.Enabled = false;
            txt_folios_prestamo.Enabled = HabilitarCampo;
            txt_fecha_devolucion_prestamo.Enabled = HabilitarCampos;
            ddlb_cod_usu_recibe_prestamo.Enabled = HabilitarCampos;
            txt_obs_prestamo.Enabled = HabilitarCampos;
        }

        private void fPrestamosInsert()
        {
            string strResultado = oPrestamos.sp_i_prestamo(
              oBasic.fInt2(txt_cod_predio_declarado_prestamo),
              ddlb_id_area_solicita_prestamo.SelectedValue,
              ddlb_cod_usu_solicita_prestamo.SelectedValue,
              txt_memorando_interno.Text,
              txt_fecha_entrega_prestamo.Text,
              ddlb_cod_usu_entrega_prestamo.Text,
              txt_folios_prestamo.Text,
              txt_fecha_devolucion_prestamo.Text,
              ddlb_cod_usu_recibe_prestamo.Text,
              txt_obs_prestamo.Text
            );
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                oLog.RegistrarLogInfo(_SOURCEPAGE, "fPrestamosInsert:", clConstantes.MSG_OK_I);
                fPrestamosLimpiarDetalle();
                oVar.prDSPrestamos = oPrestamos.sp_s_prestamos_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                fMensajeCRUD(clConstantes.MSG_OK_I, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGPRESTAMOS, upPrestamos);
            }
            else if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_ERR_DATOS)
            {
                fMensajeCRUD(clConstantes.MSG_ERR_DATOS, (int)clConstantes.NivelMensaje.Error, _DIVMSGPRESTAMOS, upPrestamos);
            }
            else
            {
                oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fPrestamosInsert");
                fMensajeCRUD(clConstantes.MSG_ERR_I, (int)clConstantes.NivelMensaje.Error, _DIVMSGPRESTAMOS, upPrestamos);
            }
        }

        private void fPrestamosInsertLote()
        {
            if (lbx_cod_predio_declarado_add2.Items.Count > 0)
            {
                string txt_cod_predios_declarados_prestamo = "";
                for (int i = 0; i < lbx_cod_predio_declarado_add2.Items.Count; i++)
                {
                    txt_cod_predios_declarados_prestamo = txt_cod_predios_declarados_prestamo + lbx_cod_predio_declarado_add2.Items[i].Value + ", ";
                }

                string strResultado = "";
                if (ddlb_id_tipo_prestamo.SelectedValue == "291")
                {
                    strResultado = oPrestamos.sp_i_prestamo_lote(
                      "1",
                      txt_cod_predios_declarados_prestamo,
                      ddlb_id_area_solicita_prestamo2.Text,
                      ddlb_cod_usu_solicita_prestamo2.Text,
                      txt_memorando_interno2.Text,
                      txt_fecha_entrega_prestamo2.Text,
                      ddlb_cod_usu_entrega_prestamo2.Text,
                      "",
                      "",
                      txt_obs_prestamo2.Text
                    );
                }
                else if (ddlb_id_tipo_prestamo.SelectedValue == "292")
                {
                    strResultado = oPrestamos.sp_i_prestamo_lote(
                      "2",
                      txt_cod_predios_declarados_prestamo,
                      null,
                      null,
                      "",
                      "",
                      "",
                      txt_fecha_devolucion_prestamo2.Text,
                      ddlb_cod_usu_recibe_prestamo2.SelectedValue,
                      txt_obs_prestamo2.Text
                    );
                }
                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fPrestamosInsertLote:", clConstantes.MSG_OK_I);
                    fPrestamosLimpiarDetalle();
                    oVar.prDSPrestamos = oPrestamos.sp_s_prestamos_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                    fMensajeCRUD(clConstantes.MSG_OK_I, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGPRESTAMOS, upPrestamos);
                }
                else if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_ERR_PARCIAL)
                {
                    oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fPrestamosInsertLote - Parcial");
                    string msg = clConstantes.MSG_ERR_PARCIAL + ". Error en fecha: " + strResultado.Substring(7);
                    fMensajeCRUD(msg, (int)clConstantes.NivelMensaje.Alerta, _DIVMSGPRESTAMOS, upPrestamos);
                }
                else
                {
                    oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fPrestamosInsertLote");
                    fMensajeCRUD(clConstantes.MSG_ERR_I, (int)clConstantes.NivelMensaje.Error, _DIVMSGPRESTAMOS, upPrestamos);
                }
            }
        }

        private void fPrestamosUpdate()
        {
            if (oPermisos.TienePermisosSP("sp_u_prestamo"))
            {
                string strResultado = oPrestamos.sp_u_prestamo(
                  oBasic.fInt2(txt_au_prestamo),
                  txt_memorando_interno.Text,
                  txt_fecha_devolucion_prestamo.Text,
                  ddlb_cod_usu_recibe_prestamo.SelectedValue,
                  txt_obs_prestamo.Text
                );
                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fPrestamosUpdate:", clConstantes.MSG_OK_U);
                    fPrestamosLimpiarDetalle();
                    oVar.prDSPrestamos = oPrestamos.sp_s_prestamos_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                    fMensajeCRUD(clConstantes.MSG_OK_U, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGPRESTAMOS, upPrestamos);
                }
                else if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_ERR_DATOS)
                {
                    fMensajeCRUD(clConstantes.MSG_ERR_DATOS, (int)clConstantes.NivelMensaje.Error, _DIVMSGPRESTAMOS, upPrestamos);
                }
                else
                {
                    oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fPrestamosUpdate");
                    fMensajeCRUD(clConstantes.MSG_ERR_U, (int)clConstantes.NivelMensaje.Error, _DIVMSGPRESTAMOS, upPrestamos);
                }
            }
            else
            {
                fMensajeCRUD(clConstantes.DB_ACTION_ERR_PERMISO, (int)clConstantes.NivelMensaje.Alerta, _DIVMSGPRESTAMOS, upPrestamos);
                fPrestamosLimpiarDetalle();
            }
        }

        private void fPrestamosValidarPermisos()
        {
            oBasic.EnableButton(false, btnPrestamosEdit);
            oBasic.EnableButton(false, btnPrestamosAdd);
            oBasic.EnableButton(false, btnPrestamosAdd2);

            if (oPermisos.TienePermisosSP("sp_i_prestamo_lote"))
                oBasic.EnableButton(true, btnPrestamosAdd2);

            if (gvPrestamos.Rows.Count > 0)
            {
                if (Convert.ToInt16(gvPrestamos.SelectedDataKey["TotalNulos"].ToString()) == 0 && oPermisos.TienePermisosSP("sp_i_prestamo"))
                    oBasic.EnableButton(true, btnPrestamosAdd);
                else if (gvPrestamos.SelectedDataKey["cod_usu_entrega_prestamo"].ToString() == oVar.prUserCod.ToString())
                    oBasic.EnableButton(true, btnPrestamosEdit);
                else if (Convert.ToBoolean(Convert.ToInt16(oVar.prUserRecibePrestamos.ToString())))
                    oBasic.EnableButton(true, btnPrestamosEdit);
            }
            else if (oPermisos.TienePermisosSP("sp_i_prestamo"))
                oBasic.EnableButton(true, btnPrestamosAdd);
        }

        protected void ddlb_id_area_solicita_prestamo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlb_cod_usu_solicita_prestamo.Items.Clear();
            if (ddlb_id_area_solicita_prestamo.SelectedValue == "288")
            {
                ddlb_cod_usu_solicita_prestamo.Items.Insert(0, new ListItem("ND", "0"));
                oBasic.StyleCtlLbl("E", false, ddlb_cod_usu_solicita_prestamo, lbl_cod_usu_solicita_prestamo, false);
                txt_obs_prestamo.Text = "Transferencia de archivo.";
            }
            else
            {
                int filtro = 5;
                if (ddlb_id_area_solicita_prestamo.SelectedValue == "82")
                    filtro = 6;
                oBasic.fLoadUsuariosFiltro(ddlb_cod_usu_solicita_prestamo, filtro);

                oBasic.StyleCtlLbl("E", true, ddlb_cod_usu_solicita_prestamo, lbl_cod_usu_solicita_prestamo, false);
                txt_obs_prestamo.Text = "";
            }
        }

        protected void ddlb_id_area_solicita_prestamo2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlb_cod_usu_solicita_prestamo2.Items.Clear();
            if (ddlb_id_area_solicita_prestamo2.SelectedValue == "288")
            {
                ddlb_cod_usu_solicita_prestamo2.Items.Insert(0, new ListItem("ND", "0"));
                oBasic.StyleCtlLbl("E", false, ddlb_cod_usu_solicita_prestamo, lbl_cod_usu_solicita_prestamo, false);
                txt_obs_prestamo2.Text = "Transferencia de archivo.";
            }
            else
            {
                int filtro = 5;
                if (ddlb_id_area_solicita_prestamo2.SelectedValue == "82")
                    filtro = 6;
                oBasic.fLoadUsuariosFiltro(ddlb_cod_usu_solicita_prestamo2, filtro);

                oBasic.StyleCtlLbl("E", true, ddlb_cod_usu_solicita_prestamo2, lbl_cod_usu_solicita_prestamo2, false);
                txt_obs_prestamo2.Text = "";

            }
        }

        protected void ddlb_id_tipo_prestamo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                oBasic.StyleCtlLbl("V", false, ddlb_id_area_solicita_prestamo2, lbl_id_area_solicita_prestamo2, true);
                oBasic.StyleCtlLbl("V", false, ddlb_cod_usu_solicita_prestamo2, lbl_cod_usu_solicita_prestamo2, true);
                oBasic.StyleCtlLbl("V", false, txt_memorando_interno2, lbl_memorando_interno2, true);
                oBasic.StyleCtlLbl("V", false, txt_fecha_entrega_prestamo2, lbl_fecha_entrega_prestamo2, true);
                oBasic.StyleCtlLbl("V", false, ddlb_cod_usu_entrega_prestamo2, lbl_cod_usu_entrega_prestamo2, true);
                oBasic.StyleCtlLbl("V", false, txt_fecha_devolucion_prestamo2, lbl_fecha_devolucion_prestamo2, true);
                oBasic.StyleCtlLbl("V", false, ddlb_cod_usu_recibe_prestamo2, lbl_cod_usu_recibe_prestamo2, true);
                oBasic.StyleCtlLbl("V", false, txt_obs_prestamo2, lbl_obs_prestamo2, true);

                lbx_cod_predio_declarado_add2.Enabled = false;
                lbx_cod_predio_declarado_add2.Visible = false;
                lbx_cod_predio_declarado.Items.Clear();
                lbx_cod_predio_declarado_add.Items.Clear();
                lbx_cod_predio_declarado_add2.Items.Clear();
                int i = Convert.ToInt16(ddlb_id_tipo_prestamo.SelectedValue);
                if (i == 291) // ||
                {
                    oBasic.StyleCtlLbl("V", true, ddlb_id_area_solicita_prestamo2, lbl_id_area_solicita_prestamo2, true);
                    oBasic.StyleCtlLbl("V", true, ddlb_cod_usu_solicita_prestamo2, lbl_cod_usu_solicita_prestamo2, true);
                    oBasic.StyleCtlLbl("V", true, txt_memorando_interno2, lbl_memorando_interno2, true);
                    oBasic.StyleCtlLbl("V", true, txt_fecha_entrega_prestamo2, lbl_fecha_entrega_prestamo2, true);
                    oBasic.StyleCtlLbl("V", true, ddlb_cod_usu_entrega_prestamo2, lbl_cod_usu_entrega_prestamo2, true);
                    oBasic.StyleCtlLbl("V", true, txt_obs_prestamo2, lbl_obs_prestamo2, true);

                    lbx_cod_predio_declarado.DataSource = oPrediosDeclarados.sp_s_predios_dec("2", txt_chip_filtro.Text);
                    lbx_cod_predio_declarado.DataTextField = "chip_resolucion";
                    lbx_cod_predio_declarado.DataValueField = "cod_predio_declarado";
                    lbx_cod_predio_declarado.DataBind();

                    oBasic.StyleCtlLbl("E", false, ddlb_cod_usu_entrega_prestamo2, lbl_cod_usu_entrega_prestamo2, true);
                    ddlb_cod_usu_entrega_prestamo2.SelectedValue = oVar.prUserCod.ToString();
                    txt_fecha_entrega_prestamo2.Text = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else if (i == 292) // ||
                {
                    oBasic.StyleCtlLbl("V", true, txt_fecha_devolucion_prestamo2, lbl_fecha_devolucion_prestamo2, true);
                    oBasic.StyleCtlLbl("V", true, ddlb_cod_usu_recibe_prestamo2, lbl_cod_usu_recibe_prestamo2, true);
                    oBasic.StyleCtlLbl("V", true, txt_obs_prestamo2, lbl_obs_prestamo2, true);

                    lbx_cod_predio_declarado.DataSource = oPrediosDeclarados.sp_s_predios_dec("3", txt_chip_filtro.Text);
                    lbx_cod_predio_declarado.DataTextField = "chip_resolucion";
                    lbx_cod_predio_declarado.DataValueField = "cod_predio_declarado";
                    lbx_cod_predio_declarado.DataBind();

                    oBasic.StyleCtlLbl("E", false, ddlb_cod_usu_recibe_prestamo2, lbl_cod_usu_recibe_prestamo2, true);
                    ddlb_cod_usu_recibe_prestamo2.SelectedValue = oVar.prUserCod.ToString();
                    txt_fecha_devolucion_prestamo2.Text = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            catch { }
        }

        protected void txt_chip_filtro_TextChanged(object sender, EventArgs e)
        {
            lbx_cod_predio_declarado.Items.Clear();
            if (ddlb_id_tipo_prestamo.SelectedValue == "291")
            {
                lbx_cod_predio_declarado.DataSource = oPrediosDeclarados.sp_s_predios_dec("2", txt_chip_filtro.Text);
                lbx_cod_predio_declarado.DataTextField = "chip_resolucion";
                lbx_cod_predio_declarado.DataValueField = "cod_predio_declarado";
                lbx_cod_predio_declarado.DataBind();
            }
            else if (ddlb_id_tipo_prestamo.SelectedValue == "292")
            {
                lbx_cod_predio_declarado.DataSource = oPrediosDeclarados.sp_s_predios_dec("3", txt_chip_filtro.Text);
                lbx_cod_predio_declarado.DataTextField = "chip_resolucion";
                lbx_cod_predio_declarado.DataValueField = "cod_predio_declarado";
                lbx_cod_predio_declarado.DataBind();
            }
        }
        #endregion

        #region------AFECTACIONES
        private void fAfectacionesLoad(string Parametro)
        {
            fAfectacionesLimpiarDetalle();

            DataSet oDS = oAfectaciones.sp_s_afectaciones_chip(Parametro);
            int iRow = 0;

            if (oDS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow oRow in oDS.Tables[0].Rows)
                {
                    switch (oDS.Tables[0].Rows[iRow]["id_afectacion"].ToString().ToLower())
                    {
                        case "161":
                            txtAA161.Text = oDS.Tables[0].Rows[iRow]["Alta"].ToString();
                            txtAM161.Text = oDS.Tables[0].Rows[iRow]["Media"].ToString();
                            txtAB161.Text = oDS.Tables[0].Rows[iRow]["Baja"].ToString();
                            chk161.Checked = (!string.IsNullOrEmpty(txtAA161.Text) || !string.IsNullOrEmpty(txtAM161.Text) || !string.IsNullOrEmpty(txtAB161.Text));
                            break;

                        case "162":
                            txtAA162.Text = oDS.Tables[0].Rows[iRow]["Alta"].ToString();
                            txtAM162.Text = oDS.Tables[0].Rows[iRow]["Media"].ToString();
                            txtAB162.Text = oDS.Tables[0].Rows[iRow]["Baja"].ToString();
                            chk162.Checked = (!string.IsNullOrEmpty(txtAA162.Text) || !string.IsNullOrEmpty(txtAM162.Text) || !string.IsNullOrEmpty(txtAB162.Text));
                            break;

                        case "165":
                            txtAA165.Text = "4A: " + oDS.Tables[0].Rows[iRow]["4A"].ToString();
                            txtAM165.Text = "4B: " + oDS.Tables[0].Rows[iRow]["4B"].ToString();
                            txtAB165.Text = "4C: " + oDS.Tables[0].Rows[iRow]["4C"].ToString();
                            chk165.Checked = (!string.IsNullOrEmpty(oDS.Tables[0].Rows[iRow]["4A"].ToString()) || !string.IsNullOrEmpty(oDS.Tables[0].Rows[iRow]["4B"].ToString()) || !string.IsNullOrEmpty(oDS.Tables[0].Rows[iRow]["4C"].ToString()));
                            break;

                        case "163":
                            chk163.Checked = true;
                            break;

                        case "164":
                            chk164.Checked = true;
                            break;

                        case "166":
                            chk166.Checked = true;
                            txtA166.Text = oDS.Tables[0].Rows[iRow]["No Aplica"].ToString();
                            break;

                        case "167":
                            chk167.Checked = true;
                            txtObs167.Text = oDS.Tables[0].Rows[iRow]["obs_afectacion"].ToString();
                            break;

                        case "168":
                            chk168.Checked = true;
                            txtA168.Text = oDS.Tables[0].Rows[iRow]["No Aplica"].ToString();
                            txtObs168.Text = oDS.Tables[0].Rows[iRow]["obs_afectacion"].ToString();
                            break;

                        case "169":
                            chk169.Checked = true;
                            txtA169.Text = oDS.Tables[0].Rows[iRow]["No Aplica"].ToString();
                            break;

                        case "170":
                            chk170.Checked = true;
                            txtA170.Text = oDS.Tables[0].Rows[iRow]["No Aplica"].ToString();
                            break;

                        case "171":
                            chk171.Checked = true;
                            txtA171.Text = oDS.Tables[0].Rows[iRow]["No Aplica"].ToString();
                            txtObs171.Text = oDS.Tables[0].Rows[iRow]["obs_afectacion"].ToString();
                            break;

                        case "172":
                            chk172.Checked = true;
                            txtA172.Text = oDS.Tables[0].Rows[iRow]["No Aplica"].ToString();
                            break;

                        case "173":
                            chk173.Checked = true;
                            break;

                        case "174":
                            chk174.Checked = true;
                            txtA174.Text = oDS.Tables[0].Rows[iRow]["No Aplica"].ToString();
                            txtObs174.Text = oDS.Tables[0].Rows[iRow]["obs_afectacion"].ToString();
                            break;

                        case "175":
                            chk175.Checked = true;
                            txtA175.Text = oDS.Tables[0].Rows[iRow]["No Aplica"].ToString();
                            txtObs175.Text = oDS.Tables[0].Rows[iRow]["obs_afectacion"].ToString();
                            break;

                    }
                    iRow++;
                }
            }
        }

        private void fAfectacionesLimpiarDetalle()
        {
            chk161.Checked = false;
            txtAA161.Text = "";
            txtAM161.Text = "";
            txtAB161.Text = "";

            chk162.Checked = false;
            txtAA162.Text = "";
            txtAM162.Text = "";
            txtAB162.Text = "";

            chk163.Checked = false;

            chk164.Checked = false;

            chk165.Checked = false;
            txtAA165.Text = "";
            txtAM165.Text = "";
            txtAB165.Text = "";

            chk166.Checked = false;
            txtA166.Text = "";

            chk167.Checked = false;
            txtObs167.Text = "";

            chk168.Checked = false;
            txtA168.Text = "";
            txtObs168.Text = "";

            chk169.Checked = false;
            txtA169.Text = "";

            chk170.Checked = false;
            txtA170.Text = "";

            chk171.Checked = false;
            txtA171.Text = "";
            txtObs171.Text = "";

            chk172.Checked = false;
            txtA172.Text = "";

            chk173.Checked = false;

            chk174.Checked = false;
            txtA174.Text = "";
            txtObs174.Text = "";

            chk175.Checked = false;
            txtA175.Text = "";
            txtObs175.Text = "";
        }
        #endregion

        #region------PREDIOS PROPIETARIOS
        private void fPrediosPropietariosLoadGV(string Parametro)
        {
            oVar.prDSPrediosPropietarios = oPrediosPropietarios.sp_s_predios_propietarios_chip(Parametro);

            gvPrediosPropietarios.DataSource = ((DataSet)(oVar.prDSPrediosPropietarios));
            gvPrediosPropietarios.DataBind();
            oVar.prDSPrediosPropietariosFiltro = (DataSet)(oVar.prDSPrediosPropietarios);

            if (gvPrediosPropietarios.Rows.Count > 0)
            {
                gvPrediosPropietarios.SelectedIndex = 0;
                btnPrediosPropietariosVG.Enabled = true;
                fPropietariosLoadGV(gvPrediosPropietarios.SelectedDataKey.Value.ToString());
            }

            mvPrediosPropietarios.ActiveViewIndex = 0;
            fBotonAccionFinalEstado(btnPrediosPropietariosAccionFinal, btnPrediosPropietariosCancelar, false);

            fPrediosPropietariosValidarPermisos();
            upPrediosPropietariosBtnVistas.Update();
            upPrediosPropietarios.Update();
            fCuentaRegistros(lblPrediosPropietariosCuenta, gvPrediosPropietarios, (DataSet)oVar.prDSPrediosPropietariosFiltro, btnFirstPrediosPropietarios, btnBackPrediosPropietarios, btnNextPrediosPropietarios, btnLastPrediosPropietarios, upPrediosPropietariosFoot, 0);
        }

        private void fPrediosPropietariosLimpiarDetalle()
        {
            txt_au_predio_propietario.Text = "";
            txt_chip__predio_propietario.Text = "";
            txt_fecha.Text = "";
            txt_fuente_propietario.Text = "";
            txt_observacion__predio_propietario.Text = "";
        }

        private void fPrediosPropietariosDetalle()
        {
            mvPrediosPropietarios.ActiveViewIndex = 1;
            int Indice = Convert.ToInt16(ViewState["RealIndexPrediosPropietarios"]);

            DataSet dsTmp = (DataSet)oVar.prDSPrediosPropietariosFiltro;

            txt_au_predio_propietario.Text = dsTmp.Tables[0].Rows[Indice]["au_predio_propietario"].ToString();
            txt_chip__predio_propietario.Text = dsTmp.Tables[0].Rows[Indice]["chip"].ToString();
            txt_fuente_propietario.Text = dsTmp.Tables[0].Rows[Indice]["fuente_propietario"].ToString();
            oBasic.fDetalleFecha(txt_fecha, dsTmp.Tables[0].Rows[Indice]["fecha"].ToString());
            txt_observacion__predio_propietario.Text = dsTmp.Tables[0].Rows[Indice]["observacion"].ToString();

            fCuentaRegistros(lblPrediosPropietariosCuenta, gvPrediosPropietarios, (DataSet)oVar.prDSPrediosPropietariosFiltro, btnFirstPrediosPropietarios, btnBackPrediosPropietarios, btnNextPrediosPropietarios, btnLastPrediosPropietarios, upPrediosPropietariosFoot, Indice);
        }

        private void fPrediosPropietariosEstadoDetalle(bool HabilitarCampos)
        {
            txt_au_predio_propietario.Enabled = false;
            txt_chip__predio_propietario.Enabled = false;
            txt_fuente_propietario.Enabled = HabilitarCampos;
            txt_fecha.Enabled = HabilitarCampos;
            txt_observacion__predio_propietario.Enabled = HabilitarCampos;
        }

        private void fPrediosPropietariosInsert()
        {
            string strResultado = oPrediosPropietarios.sp_i_predios_propietario(
              txt_chip__predio_propietario.Text,
              txt_fuente_propietario.Text,
              txt_fecha.Text,
              txt_observacion__predio_propietario.Text
            );
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                oLog.RegistrarLogInfo(_SOURCEPAGE, "fPrediosPropietariosInsert:", clConstantes.MSG_OK_I);
                fPrediosPropietariosLimpiarDetalle();
                oVar.prDSPrediosPropietarios = oPrediosPropietarios.sp_s_predios_propietarios_chip(gvPrediosDec.SelectedRow.Cells[1].Text);
                fMensajeCRUD(clConstantes.MSG_OK_I, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGPREDIOSPROPIETARIOS, upPrediosPropietarios);
            }
            else
            {
                oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fPrediosPropietariosInsert");
                fMensajeCRUD(clConstantes.MSG_ERR_I, (int)clConstantes.NivelMensaje.Error, _DIVMSGPREDIOSPROPIETARIOS, upPrediosPropietarios);
            }
        }

        private void fPrediosPropietariosUpdate()
        {
            if (HayCambiosPrediosPropietarios)
            {
                string strResultado = oPrediosPropietarios.sp_u_predios_propietario(
                  oBasic.fInt2(txt_au_predio_propietario),
                  txt_fuente_propietario.Text,
                  txt_fecha.Text,
                  txt_observacion__predio_propietario.Text
                );
                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fPrediosPropietariosUpdate:", clConstantes.MSG_OK_U);
                    fPrediosPropietariosLimpiarDetalle();
                    oVar.prDSPrediosPropietarios = oPrediosPropietarios.sp_s_predios_propietarios_chip(gvPrediosDec.SelectedRow.Cells[1].Text);
                    fMensajeCRUD(clConstantes.MSG_OK_U, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGPREDIOSPROPIETARIOS, upPrediosPropietarios);
                }
                else
                {
                    oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fPrediosPropietariosUpdate");
                    fMensajeCRUD(clConstantes.MSG_ERR_U, (int)clConstantes.NivelMensaje.Error, _DIVMSGPREDIOSPROPIETARIOS, upPrediosPropietarios);
                }
            }
            else
            {
                fMensajeCRUD(clConstantes.MSG_SIN_CAMBIOS, (int)clConstantes.NivelMensaje.Alerta, _DIVMSGPREDIOSPROPIETARIOS, upPrediosPropietarios);
                fPrediosPropietariosLimpiarDetalle();
            }
        }

        private void fPrediosPropietariosDelete()
        {
            string strResultado = oPrediosPropietarios.sp_d_predios_propietario(gvPrediosPropietarios.SelectedDataKey.Value.ToString());

            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                oLog.RegistrarLogInfo(_SOURCEPAGE, "fPrediosPropietariosDelete:", clConstantes.MSG_OK_D);
                fPrediosPropietariosLimpiarDetalle();
                oVar.prDSPrediosPropietarios = oPrediosPropietarios.sp_s_predios_propietarios_chip(txt_chip__predio_propietario.Text);

                if (Convert.ToInt16(ViewState["RealIndexPrediosPropietarios"]) > 0)
                    ViewState["RealIndexPrediosPropietarios"] = Convert.ToInt16(ViewState["RealIndexPrediosPropietarios"]) - 1;
                else
                    ViewState["RealIndexPrediosPropietarios"] = 0;

                fMensajeCRUD(clConstantes.MSG_OK_D, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGPREDIOSPROPIETARIOS, upPrediosPropietarios);

                fPropietariosLoadGV(gvPrediosPropietarios.SelectedDataKey.Value.ToString());
            }
            else
            {
                oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fPrediosPropietariosDelete");
                fMensajeCRUD(clConstantes.MSG_ERR_D, (int)clConstantes.NivelMensaje.Error, _DIVMSGPREDIOSPROPIETARIOS, upPrediosPropietarios);
            }
        }

        private void fPrediosPropietariosValidarPermisos()
        {
            oBasic.EnableButton(false, btnPrediosPropietariosAdd);
            oBasic.EnableButton(false, btnPrediosPropietariosEdit);
            oBasic.EnableButton(false, btnPrediosPropietariosDel);

            if (oVar.prUserCod.ToString() != oVar.prUserResponsablePredioDec.ToString() && !IsHelperUser())
                return;

            if (oPermisos.TienePermisosSP("sp_i_predios_propietario"))
                oBasic.EnableButton(true, btnPrediosPropietariosAdd);

            if (gvPrediosPropietarios.Rows.Count > 0)
            {
                if (oPermisos.TienePermisosSP("sp_u_predios_propietario"))
                    oBasic.EnableButton(true, btnPrediosPropietariosEdit);
                if (oPermisos.TienePermisosSP("sp_d_predios_propietario"))
                    oBasic.EnableButton(true, btnPrediosPropietariosDel);
            }
        }
        #endregion

        #region------PROPIETARIOS
        private void fPropietariosLoadGV(string Parametro)
        {
            oVar.prDSPropietarios = oPropietarios.sp_s_propietarios_cod_predio_propietario(Parametro);

            gvPropietarios.DataSource = ((DataSet)(oVar.prDSPropietarios));
            gvPropietarios.DataBind();
            oVar.prDSPropietariosFiltro = (DataSet)(oVar.prDSPropietarios);

            if (gvPropietarios.Rows.Count > 0)
            {
                gvPropietarios.SelectedIndex = 0;
                btnPropietariosVG.Enabled = true;
            }

            mvPropietarios.ActiveViewIndex = 0;
            fBotonAccionFinalEstado(btnPropietariosAccionFinal, btnPropietariosCancelar, false);

            fPropietariosValidarPermisos();
            upPropietariosBtnVistas.Update();
            upPropietarios.Update();
            fCuentaRegistros(lblPropietariosCuenta, gvPropietarios, (DataSet)oVar.prDSPropietariosFiltro, btnFirstPropietarios, btnBackPropietarios, btnNextPropietarios, btnLastPropietarios, upPropietariosFoot, 0);
        }

        private void fPropietariosLimpiarDetalle()
        {
            txt_au_propietario.Text = "";
            txt_cod_predio_propietario.Text = "";
            txt_nombre_propietario.Text = "";
            ddlb_id_tipo_doc_propietario.SelectedIndex = -1;
            txt_num_doc_propietario.Text = "";
            txt_direccion_propietario.Text = "";
            txt_telefono_propietario.Text = "";
            txt_correo_propietario.Text = "";
            txt_nombre_representante.Text = "";
            ddlb_id_tipo_doc_representante.SelectedIndex = -1;
            txt_num_doc_representante.Text = "";
            txt_direccion_representante.Text = "";
            txt_telefono_representante.Text = "";
            txt_correo_representante.Text = "";
            txt_observacion__propietario.Text = "";
        }

        private void fPropietariosDetalle()
        {
            mvPropietarios.ActiveViewIndex = 1;
            int Indice = Convert.ToInt16(ViewState["RealIndexPropietarios"]);

            DataSet dsTmp = (DataSet)oVar.prDSPropietariosFiltro;

            txt_au_propietario.Text = dsTmp.Tables[0].Rows[Indice]["au_propietario"].ToString();
            txt_cod_predio_propietario.Text = dsTmp.Tables[0].Rows[Indice]["cod_predio_propietario"].ToString();
            txt_nombre_propietario.Text = dsTmp.Tables[0].Rows[Indice]["nombre_propietario"].ToString();
            oBasic.fDetalleDropDown(ddlb_id_tipo_doc_propietario, dsTmp.Tables[0].Rows[Indice]["id_tipo_doc_propietario"].ToString());
            txt_num_doc_propietario.Text = dsTmp.Tables[0].Rows[Indice]["num_doc_propietario"].ToString();
            txt_direccion_propietario.Text = dsTmp.Tables[0].Rows[Indice]["direccion_propietario"].ToString();
            txt_telefono_propietario.Text = dsTmp.Tables[0].Rows[Indice]["telefono_propietario"].ToString();
            txt_correo_propietario.Text = dsTmp.Tables[0].Rows[Indice]["correo_propietario"].ToString();
            txt_nombre_representante.Text = dsTmp.Tables[0].Rows[Indice]["nombre_representante"].ToString();
            oBasic.fDetalleDropDown(ddlb_id_tipo_doc_representante, dsTmp.Tables[0].Rows[Indice]["id_tipo_doc_representante"].ToString());
            txt_num_doc_representante.Text = dsTmp.Tables[0].Rows[Indice]["num_doc_representante"].ToString();
            txt_direccion_representante.Text = dsTmp.Tables[0].Rows[Indice]["direccion_representante"].ToString();
            txt_telefono_representante.Text = dsTmp.Tables[0].Rows[Indice]["telefono_representante"].ToString();
            txt_correo_representante.Text = dsTmp.Tables[0].Rows[Indice]["correo_representante"].ToString();
            txt_observacion__propietario.Text = dsTmp.Tables[0].Rows[Indice]["observacion"].ToString();

            fCuentaRegistros(lblPropietariosCuenta, gvPropietarios, (DataSet)oVar.prDSPropietariosFiltro, btnFirstPropietarios, btnBackPropietarios, btnNextPropietarios, btnLastPropietarios, upPropietariosFoot, Indice);
        }

        private void fPropietariosEstadoDetalle(bool HabilitarCampos)
        {
            txt_au_propietario.Enabled = false;
            txt_cod_predio_propietario.Enabled = false;
            txt_nombre_propietario.Enabled = HabilitarCampos;
            ddlb_id_tipo_doc_propietario.Enabled = HabilitarCampos;
            txt_num_doc_propietario.Enabled = HabilitarCampos;
            txt_direccion_propietario.Enabled = HabilitarCampos;
            txt_telefono_propietario.Enabled = HabilitarCampos;
            txt_correo_propietario.Enabled = HabilitarCampos;
            txt_nombre_representante.Enabled = HabilitarCampos;
            ddlb_id_tipo_doc_representante.Enabled = HabilitarCampos;
            txt_num_doc_representante.Enabled = HabilitarCampos;
            txt_direccion_representante.Enabled = HabilitarCampos;
            txt_telefono_representante.Enabled = HabilitarCampos;
            txt_correo_representante.Enabled = HabilitarCampos;
            txt_observacion__propietario.Enabled = HabilitarCampos;
        }

        private void fPropietariosInsert()
        {
            string strResultado = oPropietarios.sp_i_propietario(
                gvPrediosPropietarios.SelectedDataKey.Value.ToString(),
                txt_nombre_propietario.Text,
                oBasic.fInt2(ddlb_id_tipo_doc_propietario),
                txt_num_doc_propietario.Text,
                txt_direccion_propietario.Text,
                txt_telefono_propietario.Text,
                txt_correo_propietario.Text,
                txt_nombre_representante.Text,
                oBasic.fInt2(ddlb_id_tipo_doc_representante),
                txt_num_doc_representante.Text,
                txt_direccion_representante.Text,
                txt_telefono_representante.Text,
                txt_correo_representante.Text,
                txt_observacion__propietario.Text
            );
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                oLog.RegistrarLogInfo(_SOURCEPAGE, "fPropietariosInsert:", clConstantes.MSG_OK_I);
                fPropietariosLimpiarDetalle();
                oVar.prDSPropietarios = oPropietarios.sp_s_propietarios_cod_predio_propietario(oBasic.fInt2(txt_cod_predio_propietario));
                fMensajeCRUD(clConstantes.MSG_OK_I, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGPROPIETARIOS, upPropietarios);
            }
            else
            {
                oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fPropietariosInsert");
                fMensajeCRUD(clConstantes.MSG_ERR_I, (int)clConstantes.NivelMensaje.Error, _DIVMSGPROPIETARIOS, upPropietarios);
            }
        }

        private void fPropietariosUpdate()
        {
            if (HayCambiosPropietarios)
            {
                string strResultado = oPropietarios.sp_u_propietario(
                  oBasic.fInt2(txt_au_propietario),
                  txt_nombre_propietario.Text,
                  oBasic.fInt2(ddlb_id_tipo_doc_propietario),
                  txt_num_doc_propietario.Text,
                  txt_direccion_propietario.Text,
                  txt_telefono_propietario.Text,
                  txt_correo_propietario.Text,
                  txt_nombre_representante.Text,
                  oBasic.fInt2(ddlb_id_tipo_doc_representante),
                  txt_num_doc_representante.Text,
                  txt_direccion_representante.Text,
                  txt_telefono_representante.Text,
                  txt_correo_representante.Text,
                  txt_observacion__propietario.Text
                );
                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fPropietariosUpdate:", clConstantes.MSG_OK_U);
                    fPropietariosLimpiarDetalle();
                    oVar.prDSPropietarios = oPropietarios.sp_s_propietarios_cod_predio_propietario(gvPrediosDec.SelectedRow.Cells[1].Text);
                    fMensajeCRUD(clConstantes.MSG_OK_U, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGPROPIETARIOS, upPropietarios);
                }
                else
                {
                    oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fPropietariosUpdate");
                    fMensajeCRUD(clConstantes.MSG_ERR_U, (int)clConstantes.NivelMensaje.Error, _DIVMSGPROPIETARIOS, upPropietarios);
                }
            }
            else
            {
                fMensajeCRUD(clConstantes.MSG_SIN_CAMBIOS, (int)clConstantes.NivelMensaje.Alerta, _DIVMSGPROPIETARIOS, upPropietarios);
                fPropietariosLimpiarDetalle();
            }
        }

        private void fPropietariosDelete()
        {
            string strResultado = oPropietarios.sp_d_propietario(gvPropietarios.SelectedDataKey.Value.ToString());

            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                oLog.RegistrarLogInfo(_SOURCEPAGE, "fPropietariosDelete:", clConstantes.MSG_OK_D);
                fPropietariosLimpiarDetalle();
                oVar.prDSPropietarios = oPropietarios.sp_s_propietarios_cod_predio_propietario(oBasic.fInt2(txt_cod_predio_propietario));

                if (Convert.ToInt16(ViewState["RealIndexPropietarios"]) > 0)
                    ViewState["RealIndexPropietarios"] = Convert.ToInt16(ViewState["RealIndexPropietarios"]) - 1;
                else
                    ViewState["RealIndexPropietarios"] = 0;

                fMensajeCRUD(clConstantes.MSG_OK_D, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGPROPIETARIOS, upPropietarios);
            }
            else
            {
                oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fPropietariosDelete");
                fMensajeCRUD(clConstantes.MSG_ERR_D, (int)clConstantes.NivelMensaje.Error, _DIVMSGPROPIETARIOS, upPropietarios);
            }
        }

        private void fPropietariosValidarPermisos()
        {
            oBasic.EnableButton(false, btnPropietariosAdd);
            oBasic.EnableButton(false, btnPropietariosEdit);
            oBasic.EnableButton(false, btnPropietariosDel);

            if (oVar.prUserCod.ToString() != oVar.prUserResponsablePredioDec.ToString() && !IsHelperUser())
                return;

            if (oPermisos.TienePermisosSP("sp_i_propietario"))
                oBasic.EnableButton(true, btnPropietariosAdd);

            if (gvPropietarios.Rows.Count > 0)
            {
                if (oPermisos.TienePermisosSP("sp_u_propietario"))
                    oBasic.EnableButton(true, btnPropietariosEdit);
                if (oPermisos.TienePermisosSP("sp_d_propietario"))
                    oBasic.EnableButton(true, btnPropietariosDel);
            }
        }
        #endregion

        #region------OBSERVACIONES
        private void fObservacionesLoadGV(string Parametro)
        {
            oVar.prDSObservaciones = oObservaciones.sp_s_pd_observaciones_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
            DataSet odsCloneObservaciones = ((DataSet)(oVar.prDSObservaciones)).Clone();

            if (!string.IsNullOrEmpty(Parametro))
            {
                string strQuery = string.Format("cod_predio_declarado = '{0}'", Parametro);
                DataRow[] oDr = ((DataSet)oVar.prDSObservaciones).Tables[0].Select(strQuery);
                foreach (DataRow row in oDr)
                {
                    odsCloneObservaciones.Tables[0].ImportRow(row);
                }
                gvObservaciones.DataSource = odsCloneObservaciones;
                gvObservaciones.DataBind();
                oVar.prDSObservacionesFiltro = odsCloneObservaciones;
            }
            else
            {
                gvObservaciones.DataSource = ((DataSet)(oVar.prDSObservaciones));
                gvObservaciones.DataBind();
                oVar.prDSObservacionesFiltro = (DataSet)(oVar.prDSObservaciones);
            }

            if (gvObservaciones.Rows.Count > 0)
            {
                gvObservaciones.SelectedIndex = 0;
                btnObservacionesVG.Enabled = true;
            }

            gvObservaciones_SelectedIndexChanged(null, null);
            fObservacionesValidarPermisos();
            mvObservaciones.ActiveViewIndex = 0;
            fBotonAccionFinalEstado(btnObservacionesAccionFinal, btnObservacionesCancelar, false);
            upObservacionesBtnVistas.Update();
            upObservaciones.Update();
            fCuentaRegistros(lblObservacionesCuenta, gvObservaciones, (DataSet)oVar.prDSObservacionesFiltro, btnFirstObservaciones, btnBackObservaciones, btnNextObservaciones, btnLastObservaciones, upObservacionesFoot, 0);
        }

        private void fObservacionesLimpiarDetalle()
        {
            txt_au_pd_observacion.Text = "";
            txt_fecha_observacion.Text = "";
            txt_observacion.Text = "";
            ddlb_id_estado_predio_obs.SelectedIndex = -1;
            ddlb_cod_usu__observacion.SelectedIndex = -1;
        }

        private void fObservacionesDetalle()
        {
            if (gvObservaciones.Rows.Count > 0)
            {
                mvObservaciones.ActiveViewIndex = 1;
                int Indice = Convert.ToInt16(ViewState["RealIndexObservaciones"]);

                DataSet dsTmp = (DataSet)oVar.prDSObservacionesFiltro;

                txt_au_pd_observacion.Text = dsTmp.Tables[0].Rows[Indice]["au_pd_observacion"].ToString();
                oBasic.fDetalleFecha(txt_fecha_observacion, dsTmp.Tables[0].Rows[Indice]["fecha_observacion"].ToString());
                txt_observacion.Text = dsTmp.Tables[0].Rows[Indice]["observacion"].ToString();
                oBasic.fDetalleDropDown(ddlb_id_estado_predio_obs, dsTmp.Tables[0].Rows[Indice]["id_estado_predio_obs"].ToString());
                oBasic.fLoadUsuariosFiltro(ddlb_cod_usu__observacion, 4, dsTmp.Tables[0].Rows[Indice]["cod_usu"].ToString());

                fCuentaRegistros(lblObservacionesCuenta, gvObservaciones, (DataSet)oVar.prDSObservacionesFiltro, btnFirstObservaciones, btnBackObservaciones, btnNextObservaciones, btnLastObservaciones, upObservacionesFoot, Indice);
            }
        }

        private void fObservacionesEstadoDetalle(bool HabilitarCampos)
        {
            txt_au_pd_observacion.Enabled = false;
            txt_fecha_observacion.Enabled = false;
            txt_observacion.Enabled = HabilitarCampos;
            ddlb_id_estado_predio_obs.Enabled = HabilitarCampos;
            ddlb_cod_usu__observacion.Enabled = false;
        }

        private void fObservacionesInsert()
        {
            string strResultado = oObservaciones.sp_i_pd_observacion(
              gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString(),
              txt_fecha_observacion.Text,
              txt_observacion.Text,
              oBasic.fInt2(ddlb_id_estado_predio_obs));
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                oLog.RegistrarLogInfo(_SOURCEPAGE, "fObservacionesInsert:", clConstantes.DB_ACTION_OK);
                oVar.prDSObservaciones = oObservaciones.sp_s_pd_observaciones_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                fMensajeCRUD(clConstantes.MSG_OK_I, (int)clConstantes.NivelMensaje.Exitoso, "DivMsgObservaciones", upObservaciones);
            }
            else
            {
                oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fObservacionesInsert");
                fMensajeCRUD(clConstantes.MSG_ERR_I, (int)clConstantes.NivelMensaje.Error, "DivMsgObservaciones", upObservaciones);
            }
        }

        private void fObservacionesUpdate()
        {
            if (HayCambiosObservaciones)
            {
                string strResultado = oObservaciones.sp_u_pd_observacion(
                  oBasic.fInt2(txt_au_pd_observacion),
                  txt_fecha_observacion.Text,
                  txt_observacion.Text,
                  oBasic.fInt2(ddlb_id_estado_predio_obs));
                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fObservacionesUpdate:", clConstantes.MSG_OK_U);
                    oVar.prDSObservaciones = oObservaciones.sp_s_pd_observaciones_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                    fMensajeCRUD(clConstantes.MSG_OK_U, (int)clConstantes.NivelMensaje.Exitoso, "DivMsgObservaciones", upObservaciones);
                }
                else if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_ERR_DATOS)
                {
                    oVar.prDSObservaciones = oObservaciones.sp_s_pd_observaciones_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                    fMensajeCRUD(clConstantes.MSG_ERR_DATOS, (int)clConstantes.NivelMensaje.Alerta, "DivMsgObservaciones", upObservaciones);
                }
                else
                {
                    oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fObservacionesUpdate");
                    fMensajeCRUD(clConstantes.MSG_ERR_U, (int)clConstantes.NivelMensaje.Error, "DivMsgObservaciones", upObservaciones);
                }
            }
            else
            {
                fMensajeCRUD(clConstantes.MSG_SIN_CAMBIOS, (int)clConstantes.NivelMensaje.Alerta, "DivMsgObservaciones", upObservaciones);
                fObservacionesLimpiarDetalle();
            }
        }

        private void fObservacionesDelete()
        {
            string strResultado = oObservaciones.sp_d_pd_observacion(gvObservaciones.SelectedDataKey.Value.ToString());

            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                oLog.RegistrarLogInfo(_SOURCEPAGE, "fObservacionesDelete:", clConstantes.MSG_OK_D);
                fObservacionesLimpiarDetalle();

                if (Convert.ToInt16(ViewState["RealIndexObservaciones"]) > 0)
                    ViewState["RealIndexObservaciones"] = Convert.ToInt16(ViewState["RealIndexObservaciones"]) - 1;
                else
                    ViewState["RealIndexObservaciones"] = 0;

                fMensajeCRUD(clConstantes.MSG_OK_D, (int)clConstantes.NivelMensaje.Exitoso, "DivMsgObservaciones", upObservaciones);
            }
            else
            {
                oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fObservacionesDelete");
                fMensajeCRUD(clConstantes.MSG_ERR_D, (int)clConstantes.NivelMensaje.Error, "DivMsgObservaciones", upObservaciones);
            }
        }

        private void fObservacionesValidarPermisos()
        {
            oBasic.EnableButton(false, btnObservacionesEdit);
            oBasic.EnableButton(false, btnObservacionesAdd);
            oBasic.EnableButton(false, btnObservacionesDel);

            if (oVar.prUserCod.ToString() != oVar.prUserResponsablePredioDec.ToString() && !IsHelperUser())
                return;

            if (oPermisos.TienePermisosSP("sp_i_pd_observacion"))
                oBasic.EnableButton(true, btnObservacionesAdd);

            if (gvObservaciones.Rows.Count > 0)
            {
                if (gvObservaciones.SelectedDataKey["cod_usu"].ToString() == oVar.prUserCod.ToString())
                {
                    if (oPermisos.TienePermisosSP("sp_u_pd_observacion"))
                        oBasic.EnableButton(true, btnObservacionesEdit);
                    if (oPermisos.TienePermisosSP("sp_d_pd_observacion"))
                        oBasic.EnableButton(true, btnObservacionesDel);
                }
            }
        }
        #endregion

        #region------VISITAS
        private void fVisitasLoadGV(string Parametro)
        {
            oVar.prDSVisitas = oVisitas.sp_s_visitas_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
            DataSet odsCloneVisitas = ((DataSet)(oVar.prDSVisitas)).Clone();

            if (!string.IsNullOrEmpty(Parametro))
            {
                string strQuery = string.Format("cod_predio_declarado = '{0}'", Parametro);
                DataRow[] oDr = ((DataSet)oVar.prDSVisitas).Tables[0].Select(strQuery);
                foreach (DataRow row in oDr)
                {
                    odsCloneVisitas.Tables[0].ImportRow(row);
                }
                gvVisitas.DataSource = odsCloneVisitas;
                gvVisitas.DataBind();
                oVar.prDSVisitasFiltro = odsCloneVisitas;
            }
            else
            {
                gvVisitas.DataSource = ((DataSet)(oVar.prDSVisitas));
                gvVisitas.DataBind();
                oVar.prDSVisitasFiltro = (DataSet)(oVar.prDSVisitas);
            }

            if (gvVisitas.Rows.Count > 0)
            {
                gvVisitas.SelectedIndex = 0;
                btnVisitasVG.Enabled = true;
                oVar.prVisitaAu = gvVisitas.SelectedDataKey.Value.ToString();
            }

            mvVisitas.ActiveViewIndex = 0;
            fBotonAccionFinalEstado(btnVisitasAccionFinal, btnVisitasCancelar, false);

            fVisitasValidarPermisos();
            upVisitasBtnVistas.Update();
            upVisitas.Update();
            fCuentaRegistros(lblVisitasCuenta, gvVisitas, (DataSet)oVar.prDSVisitasFiltro, btnFirstVisitas, btnBackVisitas, btnNextVisitas, btnLastVisitas, upVisitasFoot, 0);
        }

        private void fVisitasLimpiarDetalle()
        {
            txt_au_visita.Text = "";
            ddlb_id_tipo_visita.SelectedIndex = -1;
            ddlb_cod_usu_visita.SelectedIndex = -1;
            txt_fecha_visita.Text = "";
            ddlb_id_estado_visita.SelectedIndex = -1;

            chk_lic_construccion.Checked = false;
            chk_lic_urbanismo.Checked = false;
            chk_lic_sin_valla.Checked = false;
            ddlb_id_ocupacion_visita.SelectedIndex = -1;
            chk_act_viv_unifamiliar.Checked = false;
            chk_act_viv_multifamiliar.Checked = false;
            chk_act_parqueadero.Checked = false;
            chk_act_comercio.Checked = false;
            txt_act_otro.Text = "";
            chk_ent_viv_unifamiliar.Checked = false;
            chk_ent_viv_multifamiliar.Checked = false;
            chk_ent_comercio.Checked = false;
            chk_ent_industria.Checked = false;
            txt_ent_otro.Text = "";
            chk_acc_via_vehicular.Checked = false;
            chk_acc_via_peatonal.Checked = false;
            chk_acc_ninguna.Checked = false;
            ddlb_id_pendiente_lote.SelectedIndex = -1;
            ddlb_id_pendiente_ladera.SelectedIndex = -1;
            chk_cob_vivienda.Checked = false;
            chk_cob_pastos.Checked = false;
            chk_cob_rastrojo.Checked = false;
            chk_cob_bosque.Checked = false;
            chk_cob_sin_cobertura.Checked = false;
            txt_cob_otro.Text = "";
            chk_inest_fisuras.Checked = false;
            chk_inest_fracturas.Checked = false;
            chk_inest_escarpe.Checked = false;
            chk_inest_corona.Checked = false;
            chk_inest_depositos.Checked = false;
            chk_inest_ninguna.Checked = false;
            txt_inest_otro.Text = "";
            chk_agua_interna.Checked = false;
            chk_agua_limitrofe.Checked = false;
            chk_agua_amortiguacion.Checked = false;
            chk_agua_obras.Checked = false;
            txt_agua_otro.Text = "";
            chk_geom_depositos.Checked = false;
            chk_geom_llenos.Checked = false;
            chk_geom_escarpes.Checked = false;
            chk_geom_llanuras.Checked = false;
            txt_geom_otro.Text = "";

            ddlb_id_uso_lote.SelectedIndex = -1;
            txt_uso_entorno.Text = "";
            ddlb_id_estado_vias_internas.SelectedIndex = -1;
            ddlb_id_estado_vias_perimetrales.SelectedIndex = -1;
            chk_tiene_servidumbres.Checked = false;
            txt_construccion_existente_lote.Text = "";
            ddlb_id_estado_construccion_existente_lote.SelectedIndex = -1;
            txt_uso_construccion_lote.Text = "";
            txt_construccion_existente_entorno.Text = "";
            txt_estado_consolidacion_entorno.Text = "";
            chk_existe_vivienda_lote.Checked = false;
            txt_obs_vivienda_lote.Text = "";
            chk_existe_vivienda_entorno.Checked = false;
            txt_obs_vivienda_entorno.Text = "";
            txt_nombre_urbanizacion_lote.Text = "";
            ddlb_id_pendiente_topografia.SelectedIndex = -1;

            txt_obs_visita.Text = "";
            ddlb_cod_usu_visita.SelectedIndex = -1;
        }

        private void fVisitasDetalle()
        {
            mvVisitas.ActiveViewIndex = 1;
            int Indice = Convert.ToInt16(ViewState["RealIndexVisitas"]);

            DataSet dsTmp = (DataSet)oVar.prDSVisitasFiltro;

            txt_au_visita.Text = dsTmp.Tables[0].Rows[Indice]["au_visita"].ToString();
            oVar.prVisitaAu = txt_au_visita.Text;
            oBasic.fDetalleDropDown(ddlb_id_tipo_visita, dsTmp.Tables[0].Rows[Indice]["id_tipo_visita"].ToString());
            oBasic.fLoadUsuariosFiltro(ddlb_cod_usu_visita, 3, dsTmp.Tables[0].Rows[Indice]["cod_usu_visita"].ToString());
            oBasic.fDetalleFecha(txt_fecha_visita, dsTmp.Tables[0].Rows[Indice]["fecha_visita"].ToString());
            oBasic.fDetalleDropDown(ddlb_id_estado_visita, dsTmp.Tables[0].Rows[Indice]["id_estado_visita"].ToString());

            chk_lic_construccion.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["lic_construccion"].ToString()));
            chk_lic_urbanismo.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["lic_urbanismo"].ToString()));
            chk_lic_sin_valla.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["lic_sin_valla"].ToString()));
            oBasic.fDetalleDropDown(ddlb_id_ocupacion_visita, dsTmp.Tables[0].Rows[Indice]["id_ocupacion_visita"].ToString());

            chk_act_viv_unifamiliar.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["act_viv_unifamiliar"].ToString()));
            chk_act_viv_multifamiliar.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["act_viv_multifamiliar"].ToString()));
            chk_act_parqueadero.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["act_parqueadero"].ToString()));
            chk_act_comercio.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["act_comercio"].ToString()));
            txt_act_otro.Text = dsTmp.Tables[0].Rows[Indice]["act_otro"].ToString();

            chk_ent_viv_unifamiliar.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["ent_viv_unifamiliar"].ToString()));
            chk_ent_viv_multifamiliar.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["ent_viv_multifamiliar"].ToString()));
            chk_ent_comercio.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["ent_comercio"].ToString()));
            chk_ent_industria.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["ent_industria"].ToString()));
            txt_ent_otro.Text = dsTmp.Tables[0].Rows[Indice]["ent_otro"].ToString();

            chk_acc_via_vehicular.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["acc_via_vehicular"].ToString()));
            chk_acc_via_peatonal.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["acc_via_peatonal"].ToString()));
            chk_acc_ninguna.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["acc_ninguna"].ToString()));
            oBasic.fDetalleDropDown(ddlb_id_pendiente_lote, dsTmp.Tables[0].Rows[Indice]["id_pendiente_lote"].ToString());
            oBasic.fDetalleDropDown(ddlb_id_pendiente_ladera, dsTmp.Tables[0].Rows[Indice]["id_pendiente_ladera"].ToString());

            chk_cob_vivienda.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["cob_vivienda"].ToString()));
            chk_cob_pastos.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["cob_pastos"].ToString()));
            chk_cob_rastrojo.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["cob_rastrojo"].ToString()));
            chk_cob_bosque.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["cob_bosque"].ToString()));
            chk_cob_sin_cobertura.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["cob_sin_cobertura"].ToString()));
            txt_cob_otro.Text = dsTmp.Tables[0].Rows[Indice]["cob_otro"].ToString();

            chk_inest_fisuras.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["inest_fisuras"].ToString()));
            chk_inest_fracturas.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["inest_fracturas"].ToString()));
            chk_inest_escarpe.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["inest_escarpe"].ToString()));
            chk_inest_corona.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["inest_corona"].ToString()));
            chk_inest_depositos.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["inest_depositos"].ToString()));
            chk_inest_ninguna.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["inest_ninguna"].ToString()));
            txt_inest_otro.Text = dsTmp.Tables[0].Rows[Indice]["inest_otro"].ToString();

            chk_agua_interna.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["agua_interna"].ToString()));
            chk_agua_limitrofe.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["agua_limitrofe"].ToString()));
            chk_agua_amortiguacion.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["agua_amortiguacion"].ToString()));
            chk_agua_obras.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["agua_obras"].ToString()));
            txt_agua_otro.Text = dsTmp.Tables[0].Rows[Indice]["agua_otro"].ToString();

            chk_geom_depositos.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["geom_depositos"].ToString()));
            chk_geom_llenos.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["geom_llenos"].ToString()));
            chk_geom_escarpes.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["geom_escarpes"].ToString()));
            chk_geom_llanuras.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["geom_llanuras"].ToString()));
            txt_geom_otro.Text = dsTmp.Tables[0].Rows[Indice]["geom_otro"].ToString();

            oBasic.fDetalleDropDown(ddlb_id_uso_lote, dsTmp.Tables[0].Rows[Indice]["id_uso_lote"].ToString());
            txt_uso_entorno.Text = dsTmp.Tables[0].Rows[Indice]["uso_entorno"].ToString();
            oBasic.fDetalleDropDown(ddlb_id_estado_vias_internas, dsTmp.Tables[0].Rows[Indice]["id_estado_vias_internas"].ToString());
            oBasic.fDetalleDropDown(ddlb_id_estado_vias_perimetrales, dsTmp.Tables[0].Rows[Indice]["id_estado_vias_perimetrales"].ToString());
            chk_tiene_servidumbres.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["tiene_servidumbres"].ToString()));
            txt_construccion_existente_lote.Text = dsTmp.Tables[0].Rows[Indice]["construccion_existente_lote"].ToString();
            oBasic.fDetalleDropDown(ddlb_id_estado_construccion_existente_lote, dsTmp.Tables[0].Rows[Indice]["id_estado_construccion_existente_lote"].ToString());
            txt_uso_construccion_lote.Text = dsTmp.Tables[0].Rows[Indice]["uso_construccion_lote"].ToString();
            txt_construccion_existente_entorno.Text = dsTmp.Tables[0].Rows[Indice]["construccion_existente_entorno"].ToString();
            txt_estado_consolidacion_entorno.Text = dsTmp.Tables[0].Rows[Indice]["estado_consolidacion_entorno"].ToString();
            chk_existe_vivienda_lote.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["existe_vivienda_lote"].ToString()));
            txt_obs_vivienda_lote.Text = dsTmp.Tables[0].Rows[Indice]["obs_vivienda_lote"].ToString();
            chk_existe_vivienda_entorno.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["existe_vivienda_entorno"].ToString()));
            txt_obs_vivienda_entorno.Text = dsTmp.Tables[0].Rows[Indice]["obs_vivienda_entorno"].ToString();
            txt_nombre_urbanizacion_lote.Text = dsTmp.Tables[0].Rows[Indice]["nombre_urbanizacion_lote"].ToString();
            oBasic.fDetalleDropDown(ddlb_id_pendiente_topografia, dsTmp.Tables[0].Rows[Indice]["id_pendiente_topografia"].ToString());
            txt_obs_visita.Text = dsTmp.Tables[0].Rows[Indice]["obs_visita"].ToString();

            fCuentaRegistros(lblVisitasCuenta, gvVisitas, (DataSet)oVar.prDSVisitasFiltro, btnFirstVisitas, btnBackVisitas, btnNextVisitas, btnLastVisitas, upVisitasFoot, Indice);
        }

        private void fVisitasEstadoDetalle(bool HabilitarCampos, bool TipoVisitaInfoTecnico)
        {
            ddlb_id_tipo_visita.Enabled = HabilitarCampos;
            ddlb_cod_usu_visita.Enabled = HabilitarCampos;
            txt_fecha_visita.Enabled = HabilitarCampos;
            ddlb_id_estado_visita.Enabled = HabilitarCampos;
            txt_nombre_urbanizacion_lote.Enabled = HabilitarCampos;
            txt_obs_visita.Enabled = HabilitarCampos;

            chk_lic_construccion.Enabled = TipoVisitaInfoTecnico;
            chk_lic_urbanismo.Enabled = TipoVisitaInfoTecnico;
            chk_lic_sin_valla.Enabled = TipoVisitaInfoTecnico;
            ddlb_id_ocupacion_visita.Enabled = TipoVisitaInfoTecnico;
            chk_act_viv_unifamiliar.Enabled = TipoVisitaInfoTecnico;
            chk_act_viv_multifamiliar.Enabled = TipoVisitaInfoTecnico;
            chk_act_parqueadero.Enabled = TipoVisitaInfoTecnico;
            chk_act_comercio.Enabled = TipoVisitaInfoTecnico;
            txt_act_otro.Enabled = TipoVisitaInfoTecnico;
            chk_ent_viv_unifamiliar.Enabled = TipoVisitaInfoTecnico;
            chk_ent_viv_multifamiliar.Enabled = TipoVisitaInfoTecnico;
            chk_ent_comercio.Enabled = TipoVisitaInfoTecnico;
            chk_ent_industria.Enabled = TipoVisitaInfoTecnico;
            txt_ent_otro.Enabled = TipoVisitaInfoTecnico;
            chk_acc_via_vehicular.Enabled = TipoVisitaInfoTecnico;
            chk_acc_via_peatonal.Enabled = TipoVisitaInfoTecnico;
            chk_acc_ninguna.Enabled = TipoVisitaInfoTecnico;
            ddlb_id_pendiente_lote.Enabled = TipoVisitaInfoTecnico;
            ddlb_id_pendiente_ladera.Enabled = TipoVisitaInfoTecnico;
            chk_cob_vivienda.Enabled = TipoVisitaInfoTecnico;
            chk_cob_pastos.Enabled = TipoVisitaInfoTecnico;
            chk_cob_rastrojo.Enabled = TipoVisitaInfoTecnico;
            chk_cob_bosque.Enabled = TipoVisitaInfoTecnico;
            chk_cob_sin_cobertura.Enabled = TipoVisitaInfoTecnico;
            txt_cob_otro.Enabled = TipoVisitaInfoTecnico;
            chk_inest_fisuras.Enabled = TipoVisitaInfoTecnico;
            chk_inest_fracturas.Enabled = TipoVisitaInfoTecnico;
            chk_inest_escarpe.Enabled = TipoVisitaInfoTecnico;
            chk_inest_corona.Enabled = TipoVisitaInfoTecnico;
            chk_inest_depositos.Enabled = TipoVisitaInfoTecnico;
            chk_inest_ninguna.Enabled = TipoVisitaInfoTecnico;
            txt_inest_otro.Enabled = TipoVisitaInfoTecnico;
            chk_agua_interna.Enabled = TipoVisitaInfoTecnico;
            chk_agua_limitrofe.Enabled = TipoVisitaInfoTecnico;
            chk_agua_amortiguacion.Enabled = TipoVisitaInfoTecnico;
            chk_agua_obras.Enabled = TipoVisitaInfoTecnico;
            txt_agua_otro.Enabled = TipoVisitaInfoTecnico;
            chk_geom_depositos.Enabled = TipoVisitaInfoTecnico;
            chk_geom_llenos.Enabled = TipoVisitaInfoTecnico;
            chk_geom_escarpes.Enabled = TipoVisitaInfoTecnico;
            chk_geom_llanuras.Enabled = TipoVisitaInfoTecnico;
            txt_geom_otro.Enabled = TipoVisitaInfoTecnico;
            ddlb_id_uso_lote.Enabled = TipoVisitaInfoTecnico;
            txt_uso_entorno.Enabled = TipoVisitaInfoTecnico;
            ddlb_id_estado_vias_internas.Enabled = TipoVisitaInfoTecnico;
            ddlb_id_estado_vias_perimetrales.Enabled = TipoVisitaInfoTecnico;
            chk_tiene_servidumbres.Enabled = TipoVisitaInfoTecnico;
            txt_construccion_existente_lote.Enabled = TipoVisitaInfoTecnico;
            ddlb_id_estado_construccion_existente_lote.Enabled = TipoVisitaInfoTecnico;
            txt_uso_construccion_lote.Enabled = TipoVisitaInfoTecnico;
            txt_construccion_existente_entorno.Enabled = TipoVisitaInfoTecnico;
            txt_estado_consolidacion_entorno.Enabled = TipoVisitaInfoTecnico;
            chk_existe_vivienda_lote.Enabled = TipoVisitaInfoTecnico;
            txt_obs_vivienda_lote.Enabled = TipoVisitaInfoTecnico;
            chk_existe_vivienda_entorno.Enabled = TipoVisitaInfoTecnico;
            txt_obs_vivienda_entorno.Enabled = TipoVisitaInfoTecnico;
            ddlb_id_pendiente_topografia.Enabled = TipoVisitaInfoTecnico;
        }

        private void fVisitasInsert()
        {
            string strResultado = oVisitas.sp_i_visita(
              gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString(),
              ddlb_id_tipo_visita.SelectedValue,
              ddlb_cod_usu_visita.SelectedValue,
              txt_fecha_visita.Text,
              ddlb_id_estado_visita.SelectedValue,
              chk_lic_construccion.Checked.ToString(),
              chk_lic_urbanismo.Checked.ToString(),
              chk_lic_sin_valla.Checked.ToString(),
              ddlb_id_ocupacion_visita.SelectedValue,
              chk_act_viv_unifamiliar.Checked.ToString(),
              chk_act_viv_multifamiliar.Checked.ToString(),
              chk_act_parqueadero.Checked.ToString(),
              chk_act_comercio.Checked.ToString(),
              txt_act_otro.Text,
              chk_ent_viv_unifamiliar.Checked.ToString(),
              chk_ent_viv_multifamiliar.Checked.ToString(),
              chk_ent_comercio.Checked.ToString(),
              chk_ent_industria.Checked.ToString(),
              txt_ent_otro.Text,
              chk_acc_via_vehicular.Checked.ToString(),
              chk_acc_via_peatonal.Checked.ToString(),
              chk_acc_ninguna.Checked.ToString(),
              ddlb_id_pendiente_lote.SelectedValue,
              ddlb_id_pendiente_ladera.SelectedValue,
              chk_cob_vivienda.Checked.ToString(),
              chk_cob_pastos.Checked.ToString(),
              chk_cob_rastrojo.Checked.ToString(),
              chk_cob_bosque.Checked.ToString(),
              chk_cob_sin_cobertura.Checked.ToString(),
              txt_cob_otro.Text,
              chk_inest_fisuras.Checked.ToString(),
              chk_inest_fracturas.Checked.ToString(),
              chk_inest_escarpe.Checked.ToString(),
              chk_inest_corona.Checked.ToString(),
              chk_inest_depositos.Checked.ToString(),
              chk_inest_ninguna.Checked.ToString(),
              txt_inest_otro.Text,
              chk_agua_interna.Checked.ToString(),
              chk_agua_limitrofe.Checked.ToString(),
              chk_agua_amortiguacion.Checked.ToString(),
              chk_agua_obras.Checked.ToString(),
              txt_agua_otro.Text,
              chk_geom_depositos.Checked.ToString(),
              chk_geom_llenos.Checked.ToString(),
              chk_geom_escarpes.Checked.ToString(),
              chk_geom_llanuras.Checked.ToString(),
              txt_geom_otro.Text,
              ddlb_id_uso_lote.SelectedValue,
              txt_uso_entorno.Text,
              ddlb_id_estado_vias_internas.SelectedValue,
              ddlb_id_estado_vias_perimetrales.SelectedValue,
              chk_tiene_servidumbres.Checked.ToString(),
              txt_construccion_existente_lote.Text,
              ddlb_id_estado_construccion_existente_lote.SelectedValue,
              txt_uso_construccion_lote.Text,
              txt_construccion_existente_entorno.Text,
              txt_estado_consolidacion_entorno.Text,
              chk_existe_vivienda_lote.Checked.ToString(),
              txt_obs_vivienda_lote.Text,
              chk_existe_vivienda_entorno.Checked.ToString(),
              txt_obs_vivienda_entorno.Text,
              txt_nombre_urbanizacion_lote.Text,
              ddlb_id_pendiente_topografia.SelectedValue,
              txt_obs_visita.Text
            );
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                oLog.RegistrarLogInfo(_SOURCEPAGE, "fVisitasInsert:", clConstantes.MSG_OK_I);
                fVisitasLimpiarDetalle();
                oVar.prDSVisitas = oVisitas.sp_s_visitas_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                fMensajeCRUD(clConstantes.MSG_OK_I, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGVISITAS, upVisitas);
            }
            else
            {
                oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fVisitasInsert");
                fMensajeCRUD(clConstantes.MSG_ERR_I, (int)clConstantes.NivelMensaje.Error, _DIVMSGVISITAS, upVisitas);
            }
        }

        private void fVisitasUpdate()
        {
            if (HayCambiosVisitas)
            {
                string strResultado = oVisitas.sp_u_visita(
                  oBasic.fInt2(txt_au_visita),
                  ddlb_id_tipo_visita.SelectedValue,
                  ddlb_cod_usu_visita.SelectedValue,
                  txt_fecha_visita.Text,
                  ddlb_id_estado_visita.SelectedValue,
                  chk_lic_construccion.Checked.ToString(),
                  chk_lic_urbanismo.Checked.ToString(),
                  chk_lic_sin_valla.Checked.ToString(),
                  ddlb_id_ocupacion_visita.SelectedValue,
                  chk_act_viv_unifamiliar.Checked.ToString(),
                  chk_act_viv_multifamiliar.Checked.ToString(),
                  chk_act_parqueadero.Checked.ToString(),
                  chk_act_comercio.Checked.ToString(),
                  txt_act_otro.Text,
                  chk_ent_viv_unifamiliar.Checked.ToString(),
                  chk_ent_viv_multifamiliar.Checked.ToString(),
                  chk_ent_comercio.Checked.ToString(),
                  chk_ent_industria.Checked.ToString(),
                  txt_ent_otro.Text,
                  chk_acc_via_vehicular.Checked.ToString(),
                  chk_acc_via_peatonal.Checked.ToString(),
                  chk_acc_ninguna.Checked.ToString(),
                  ddlb_id_pendiente_lote.SelectedValue,
                  ddlb_id_pendiente_ladera.SelectedValue,
                  chk_cob_vivienda.Checked.ToString(),
                  chk_cob_pastos.Checked.ToString(),
                  chk_cob_rastrojo.Checked.ToString(),
                  chk_cob_bosque.Checked.ToString(),
                  chk_cob_sin_cobertura.Checked.ToString(),
                  txt_cob_otro.Text,
                  chk_inest_fisuras.Checked.ToString(),
                  chk_inest_fracturas.Checked.ToString(),
                  chk_inest_escarpe.Checked.ToString(),
                  chk_inest_corona.Checked.ToString(),
                  chk_inest_depositos.Checked.ToString(),
                  chk_inest_ninguna.Checked.ToString(),
                  txt_inest_otro.Text,
                  chk_agua_interna.Checked.ToString(),
                  chk_agua_limitrofe.Checked.ToString(),
                  chk_agua_amortiguacion.Checked.ToString(),
                  chk_agua_obras.Checked.ToString(),
                  txt_agua_otro.Text,
                  chk_geom_depositos.Checked.ToString(),
                  chk_geom_llenos.Checked.ToString(),
                  chk_geom_escarpes.Checked.ToString(),
                  chk_geom_llanuras.Checked.ToString(),
                  txt_geom_otro.Text,
                  ddlb_id_uso_lote.SelectedValue,
                  txt_uso_entorno.Text,
                  ddlb_id_estado_vias_internas.SelectedValue,
                  ddlb_id_estado_vias_perimetrales.SelectedValue,
                  chk_tiene_servidumbres.Checked.ToString(),
                  txt_construccion_existente_lote.Text,
                  ddlb_id_estado_construccion_existente_lote.SelectedValue,
                  txt_uso_construccion_lote.Text,
                  txt_construccion_existente_entorno.Text,
                  txt_estado_consolidacion_entorno.Text,
                  chk_existe_vivienda_lote.Checked.ToString(),
                  txt_obs_vivienda_lote.Text,
                  chk_existe_vivienda_entorno.Checked.ToString(),
                  txt_obs_vivienda_entorno.Text,
                  txt_nombre_urbanizacion_lote.Text,
                  ddlb_id_pendiente_topografia.SelectedValue,
                  txt_obs_visita.Text
                );
                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fVisitasUpdate:", clConstantes.MSG_OK_U);
                    fVisitasLimpiarDetalle();
                    oVar.prDSVisitas = oVisitas.sp_s_visitas_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                    fMensajeCRUD(clConstantes.MSG_OK_U, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGVISITAS, upVisitas);
                }
                else
                {
                    oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fVisitasUpdate");
                    fMensajeCRUD(clConstantes.MSG_ERR_U, (int)clConstantes.NivelMensaje.Error, _DIVMSGVISITAS, upVisitas);
                }
            }
            else
            {
                fMensajeCRUD(clConstantes.MSG_SIN_CAMBIOS, (int)clConstantes.NivelMensaje.Alerta, _DIVMSGVISITAS, upVisitas);
                fVisitasLimpiarDetalle();
            }
        }

        private void fVisitasDelete()
        {
            string strResultado = oVisitas.sp_d_visita(gvVisitas.SelectedDataKey.Value.ToString());

            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                oLog.RegistrarLogInfo(_SOURCEPAGE, "fVisitasDelete:", clConstantes.MSG_OK_D);
                fVisitasLimpiarDetalle();
                oVar.prDSVisitas = oVisitas.sp_s_visitas_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());

                if (Convert.ToInt16(ViewState["RealIndexVisitas"]) > 0)
                    ViewState["RealIndexVisitas"] = Convert.ToInt16(ViewState["RealIndexVisitas"]) - 1;
                else
                    ViewState["RealIndexVisitas"] = 0;

                fMensajeCRUD(clConstantes.MSG_OK_D, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGVISITAS, upVisitas);
            }
            else
            {
                oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fVisitasDelete");
                fMensajeCRUD(clConstantes.MSG_ERR_D, (int)clConstantes.NivelMensaje.Error, _DIVMSGVISITAS, upVisitas);
            }
        }

        private void fVisitasValidarPermisos()
        {
            oBasic.EnableButton(false, btnVisitasAdd);
            oBasic.EnableButton(false, btnVisitasEdit);
            oBasic.EnableButton(false, btnVisitasDel);

            if (oVar.prUserCod.ToString() != oVar.prUserResponsablePredioDec.ToString() && !IsHelperUser())
                return;

            if (oPermisos.TienePermisosSP("sp_i_visita"))
                oBasic.EnableButton(true, btnVisitasAdd);

            if (gvVisitas.Rows.Count > 0)
            {
                if (oPermisos.TienePermisosSP("sp_u_visita"))
                    oBasic.EnableButton(true, btnVisitasEdit);
                if (oPermisos.TienePermisosSP("sp_d_visita"))
                    oBasic.EnableButton(true, btnVisitasDel);
            }
        }

        private void fVerFotos()
        {
            fGetArchivoFotos();

            if (ArchivosFoto == null)
            {
                fMensajeCRUD("Visita no tiene Fotos", (int)clConstantes.NivelMensaje.Alerta, _DIVMSGVISITAS, upVisitas);
                upVisitas.Update();
            }
            else
            {
                mpeVerFoto.Show();
                int IndexFoto = Convert.ToInt16(ViewState["IndexFoto"].ToString());

                ViewState["IndexFoto"] = IndexFoto.ToString();

                if (IndexFoto < 0)
                {
                    ViewState["IndexFoto"] = "0";
                    btnBackFoto.Enabled = false;
                }


                if (IndexFoto > ArchivosFoto.Length)
                {
                    ViewState["IndexFoto"] = (ArchivosFoto.Rank - 1).ToString();
                    btnNextFoto.Enabled = false;
                }

                imgFoto.ImageUrl = "~/Handler1.ashx?fileFoto=" + ArchivosFoto[IndexFoto].ToString();
            }

        }

        private void fGetArchivoFotos()
        {
            ArchivosFoto = oFile.fGetArchivoFotos(gvVisitas.SelectedDataKey.Value.ToString());
        }

        private void fGuardarPlantilla(string pathSource, string nameFile)
        {
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            response.AddHeader("Content-Disposition", "attachment; filename=" + nameFile + ";");
            response.TransmitFile(pathSource);
            response.Flush();
            response.End();
        }

        private void fPreparaPlantilla()
        {
            Exception rtaCreacion;
            string pathPlantillaSource = oVar.prPathPlantillaVisitas.ToString();
            string fileKey = oVar.prUser.ToString() + oUtil.CrearUniqueName();
            string pathPlantillaEdit = string.Format("{0}\\{1}{2}", Path.GetDirectoryName(pathPlantillaSource), fileKey, Path.GetExtension(pathPlantillaSource));
            ArchivosFoto = oFile.fGetArchivoFotos(gvVisitas.SelectedDataKey.Value.ToString());

            try
            {
                System.IO.File.Copy(pathPlantillaSource, pathPlantillaEdit, true);
                if (string.IsNullOrEmpty(txt_au_visita.Text))
                    rtaCreacion = oVisitaTemplate.fCrearTemplate(pathPlantillaEdit, oVisitas.sp_s_visita_plantilla(gvVisitas.SelectedDataKey.Value.ToString()));
                else
                    rtaCreacion = oVisitaTemplate.fCrearTemplate(pathPlantillaEdit, oVisitas.sp_s_visita_plantilla(txt_au_visita.Text));

                if (rtaCreacion == null)
                {
                    fMensajeCRUD("Documento de visita creado.", (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGVISITAS, upVisitas);
                    string file_name = gvPredios.SelectedRow.Cells[0].Text;
                    fGuardarPlantilla(pathPlantillaEdit, "Visita_" + file_name + ".docx");
                }
                else
                {
                    oLog.RegistrarLogError(rtaCreacion, _SOURCEPAGE, "fCrearPlantilla");
                    fMensajeCRUD("No fue posible crear documento de visita.", (int)clConstantes.NivelMensaje.Error, _DIVMSGVISITAS, upVisitas);
                }
            }
            catch (Exception MyErr)
            {
                oLog.RegistrarLogError(MyErr, _SOURCEPAGE, "fPreparaPlantilla");
            }
            finally
            {
                File.Delete(pathPlantillaEdit);
                upVisitasBtnVistas.Update();
                upVisitas.Update();
            }
        }

        private void fPreparaPlantillaOld()
        {
            Exception rtaCreacion;
            string pathPlantillaSource = oVar.prPathPlantillaVisitasOld.ToString();
            string fileKey = oVar.prUser.ToString() + oUtil.CrearUniqueName();
            string pathPlantillaEdit = string.Format("{0}\\{1}{2}", Path.GetDirectoryName(pathPlantillaSource), fileKey, Path.GetExtension(pathPlantillaSource));
            ArchivosFoto = oFile.fGetArchivoFotos(gvVisitas.SelectedDataKey.Value.ToString());

            try
            {
                System.IO.File.Copy(pathPlantillaSource, pathPlantillaEdit, true);

                if (string.IsNullOrEmpty(txt_au_visita.Text))
                    rtaCreacion = oVisitaTemplateOld.fCrearTemplate(pathPlantillaEdit, oVisitas.sp_s_visita_plantilla(gvVisitas.SelectedDataKey.Value.ToString()));
                else
                    rtaCreacion = oVisitaTemplateOld.fCrearTemplate(pathPlantillaEdit, oVisitas.sp_s_visita_plantilla(txt_au_visita.Text));

                if (rtaCreacion == null)
                {
                    fMensajeCRUD("Documento de visita creado.", (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGVISITAS, upVisitas);
                    string file_name = gvPredios.SelectedRow.Cells[0].Text;
                    fGuardarPlantilla(pathPlantillaEdit, "Visita_" + file_name + ".docx");
                }
                else
                {
                    oLog.RegistrarLogError(rtaCreacion, _SOURCEPAGE, "fCrearPlantilla");
                    fMensajeCRUD("No fue posible crear documento de visita.", (int)clConstantes.NivelMensaje.Error, _DIVMSGVISITAS, upVisitas);
                }
            }
            catch (Exception MyErr)
            {
                oLog.RegistrarLogError(MyErr, _SOURCEPAGE, "fPreparaPlantilla");
            }
            finally
            {
                File.Delete(pathPlantillaEdit);
                upVisitasBtnVistas.Update();
                upVisitas.Update();
            }
        }

        protected void ddlb_id_tipo_visita_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlb_id_tipo_visita.SelectedValue.ToString() == _TIPOVISITA_INFORMETECNICO || ddlb_id_tipo_visita.SelectedValue.ToString() == _TIPOVISITA_ACTA)
            {
                TabContainerVisitas.Enabled = true;
                fVisitasEstadoDetalle(true, true);
            }
            else
            {
                TabContainerVisitas.Enabled = false;
                fVisitasEstadoDetalle(true, false);
            }
            oUtil.fEstiloEstadoControl(vVisitasDetalle);
        }
        #endregion

        #region------LICENCIAS
        private void fLicenciasLoadGV(string Parametro)
        {
            oVar.prDSLicencias = oLicencias.sp_s_licencias_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
            DataSet odsCloneLicencias = ((DataSet)(oVar.prDSLicencias)).Clone();

            if (!string.IsNullOrEmpty(Parametro))
            {
                string strQuery = string.Format("cod_predio_declarado = '{0}'", Parametro);
                DataRow[] oDr = ((DataSet)oVar.prDSLicencias).Tables[0].Select(strQuery);
                foreach (DataRow row in oDr)
                {
                    odsCloneLicencias.Tables[0].ImportRow(row);
                }
                gvLicencias.DataSource = odsCloneLicencias;
                gvLicencias.DataBind();
                oVar.prDSLicenciasFiltro = odsCloneLicencias;
            }
            else
            {
                gvLicencias.DataSource = ((DataSet)(oVar.prDSLicencias));
                gvLicencias.DataBind();
                oVar.prDSLicenciasFiltro = (DataSet)(oVar.prDSLicencias);
            }

            if (gvLicencias.Rows.Count > 0)
            {
                gvLicencias.SelectedIndex = 0;
                btnLicenciasVG.Enabled = true;
            }

            mvLicencias.ActiveViewIndex = 0;
            fBotonAccionFinalEstado(btnLicenciasAccionFinal, btnLicenciasCancelar, false);

            fLicenciasValidarPermisos();
            upLicenciasBtnVistas.Update();
            upLicencias.Update();
            fCuentaRegistros(lblLicenciasCuenta, gvLicencias, (DataSet)oVar.prDSLicenciasFiltro, btnFirstLicencias, btnBackLicencias, btnNextLicencias, btnLastLicencias, upLicenciasFoot, 0);
        }

        private void fLicenciasLimpiarDetalle()
        {
            ddlb_id_fuente_informacion.SelectedIndex = -1;
            ddlb_id_tipo_licencia.SelectedIndex = -1;
            txt_curador.Text = "";
            txt_numero_licencia.Text = "";
            txt_fecha_ejecutoria.Text = "";
            txt_termino_vigencia_meses.Text = "";
            txt_nombre_proyecto.Text = "";
            txt_plano_urbanistico_aprobado.Text = "";
            txt_area_bruta.Text = "";
            txt_area_neta.Text = "";
            txt_area_util.Text = "";
            txt_area_cesion_zonas_verdes.Text = "";
            txt_area_cesion_vias.Text = "";
            txt_area_cesion_eq_comunal.Text = "";
            txt_porcentaje_ejecucion_urbanismo.Text = "";
            ddlb_id_obligacion_VIS.SelectedIndex = -1;
            ddlb_id_obligacion_VIP.SelectedIndex = -1;
            txt_area_terreno_VIS.Text = "";
            txt_area_terreno_no_VIS.Text = "";
            txt_area_terreno_VIP.Text = "";
            txt_area_construida_VIS.Text = "";
            txt_area_construida_no_VIS.Text = "";
            txt_area_construida_VIP.Text = "";
            txt_porcentaje_obligacion_VIS.Text = "";
            txt_porcentaje_obligacion_VIP.Text = "";
            txt_unidades_vivienda_VIS.Text = "";
            txt_unidades_vivienda_no_VIS.Text = "";
            txt_unidades_vivienda_VIP.Text = "";
            txt_area_comercio.Text = "";
            txt_area_oficina.Text = "";
            txt_area_institucional.Text = "";
            txt_area_industria.Text = "";
            txt_area_lote.Text = "";
            txt_area_sotano.Text = "";
            txt_area_semisotano.Text = "";
            txt_area_primer_piso.Text = "";
            txt_area_pisos_restantes.Text = "";
            txt_area_construida_total.Text = "";
            txt_area_libre_primer_piso.Text = "";
            txt_porcentaje_ejecucion_construccion.Text = "";
        }

        private void fLicenciasDetalle()
        {
            mvLicencias.ActiveViewIndex = 1;
            int Indice = Convert.ToInt16(ViewState["RealIndexLicencias"]);

            DataSet dsTmp = (DataSet)oVar.prDSLicenciasFiltro;

            txt_au_licencia.Text = dsTmp.Tables[0].Rows[Indice]["au_licencia"].ToString();
            oBasic.fDetalleDropDown(ddlb_id_fuente_informacion, dsTmp.Tables[0].Rows[Indice]["id_fuente_informacion"].ToString());
            oBasic.fDetalleDropDown(ddlb_id_tipo_licencia, dsTmp.Tables[0].Rows[Indice]["id_tipo_licencia"].ToString());
            txt_numero_licencia.Text = dsTmp.Tables[0].Rows[Indice]["numero_licencia"].ToString();
            txt_curador.Text = dsTmp.Tables[0].Rows[Indice]["curador"].ToString();
            oBasic.fDetalleFecha(txt_fecha_ejecutoria, dsTmp.Tables[0].Rows[Indice]["fecha_ejecutoria"].ToString());
            txt_termino_vigencia_meses.Text = dsTmp.Tables[0].Rows[Indice]["termino_vigencia_meses"].ToString();
            txt_nombre_proyecto.Text = dsTmp.Tables[0].Rows[Indice]["nombre_proyecto"].ToString();
            txt_plano_urbanistico_aprobado.Text = dsTmp.Tables[0].Rows[Indice]["plano_urbanistico_aprobado"].ToString();
            txt_area_bruta.Text = dsTmp.Tables[0].Rows[Indice]["area_bruta"].ToString();
            txt_area_neta.Text = dsTmp.Tables[0].Rows[Indice]["area_neta"].ToString();
            txt_area_util.Text = dsTmp.Tables[0].Rows[Indice]["area_util"].ToString();
            txt_area_cesion_zonas_verdes.Text = dsTmp.Tables[0].Rows[Indice]["area_cesion_zonas_verdes"].ToString();
            txt_area_cesion_vias.Text = dsTmp.Tables[0].Rows[Indice]["area_cesion_vias"].ToString();
            txt_area_cesion_eq_comunal.Text = dsTmp.Tables[0].Rows[Indice]["area_cesion_eq_comunal"].ToString();
            txt_porcentaje_ejecucion_urbanismo.Text = dsTmp.Tables[0].Rows[Indice]["porcentaje_ejecucion_urbanismo"].ToString();
            oBasic.fDetalleDropDown(ddlb_id_obligacion_VIS, dsTmp.Tables[0].Rows[Indice]["id_obligacion_VIS"].ToString());
            oBasic.fDetalleDropDown(ddlb_id_obligacion_VIP, dsTmp.Tables[0].Rows[Indice]["id_obligacion_VIP"].ToString());
            txt_area_terreno_VIS.Text = dsTmp.Tables[0].Rows[Indice]["area_terreno_VIS"].ToString();
            txt_area_terreno_no_VIS.Text = dsTmp.Tables[0].Rows[Indice]["area_terreno_no_VIS"].ToString();
            txt_area_terreno_VIP.Text = dsTmp.Tables[0].Rows[Indice]["area_terreno_VIP"].ToString();
            txt_area_construida_VIS.Text = dsTmp.Tables[0].Rows[Indice]["area_construida_VIS"].ToString();
            txt_area_construida_no_VIS.Text = dsTmp.Tables[0].Rows[Indice]["area_construida_no_VIS"].ToString();
            txt_area_construida_VIP.Text = dsTmp.Tables[0].Rows[Indice]["area_construida_VIP"].ToString();
            txt_porcentaje_obligacion_VIS.Text = dsTmp.Tables[0].Rows[Indice]["porcentaje_obligacion_VIS"].ToString();
            txt_porcentaje_obligacion_VIP.Text = dsTmp.Tables[0].Rows[Indice]["porcentaje_obligacion_VIP"].ToString();
            txt_unidades_vivienda_VIS.Text = dsTmp.Tables[0].Rows[Indice]["unidades_vivienda_VIS"].ToString();
            txt_unidades_vivienda_no_VIS.Text = dsTmp.Tables[0].Rows[Indice]["unidades_vivienda_no_VIS"].ToString();
            txt_unidades_vivienda_VIP.Text = dsTmp.Tables[0].Rows[Indice]["unidades_vivienda_VIP"].ToString();
            txt_area_comercio.Text = dsTmp.Tables[0].Rows[Indice]["area_comercio"].ToString();
            txt_area_oficina.Text = dsTmp.Tables[0].Rows[Indice]["area_oficina"].ToString();
            txt_area_institucional.Text = dsTmp.Tables[0].Rows[Indice]["area_institucional"].ToString();
            txt_area_industria.Text = dsTmp.Tables[0].Rows[Indice]["area_industria"].ToString();
            txt_area_lote.Text = dsTmp.Tables[0].Rows[Indice]["area_lote"].ToString();
            txt_area_sotano.Text = dsTmp.Tables[0].Rows[Indice]["area_sotano"].ToString();
            txt_area_semisotano.Text = dsTmp.Tables[0].Rows[Indice]["area_semisotano"].ToString();
            txt_area_primer_piso.Text = dsTmp.Tables[0].Rows[Indice]["area_primer_piso"].ToString();
            txt_area_pisos_restantes.Text = dsTmp.Tables[0].Rows[Indice]["area_pisos_restantes"].ToString();
            txt_area_construida_total.Text = dsTmp.Tables[0].Rows[Indice]["area_construida_total"].ToString();
            txt_area_libre_primer_piso.Text = dsTmp.Tables[0].Rows[Indice]["area_libre_primer_piso"].ToString();
            txt_porcentaje_ejecucion_construccion.Text = dsTmp.Tables[0].Rows[Indice]["porcentaje_ejecucion_construccion"].ToString();

            fCuentaRegistros(lblLicenciasCuenta, gvLicencias, (DataSet)oVar.prDSLicenciasFiltro, btnFirstLicencias, btnBackLicencias, btnNextLicencias, btnLastLicencias, upLicenciasFoot, Indice);
        }

        private void fLicenciasEstadoDetalle(bool HabilitarCampos)
        {
            ddlb_id_fuente_informacion.Enabled = HabilitarCampos;
            ddlb_id_tipo_licencia.Enabled = HabilitarCampos;
            txt_numero_licencia.Enabled = HabilitarCampos;
            txt_curador.Enabled = HabilitarCampos;
            txt_fecha_ejecutoria.Enabled = HabilitarCampos;
            txt_termino_vigencia_meses.Enabled = HabilitarCampos;
            txt_nombre_proyecto.Enabled = HabilitarCampos;
            txt_plano_urbanistico_aprobado.Enabled = HabilitarCampos;
            txt_area_bruta.Enabled = HabilitarCampos;
            txt_area_neta.Enabled = HabilitarCampos;
            txt_area_util.Enabled = HabilitarCampos;
            txt_area_cesion_zonas_verdes.Enabled = HabilitarCampos;
            txt_area_cesion_vias.Enabled = HabilitarCampos;
            txt_area_cesion_eq_comunal.Enabled = HabilitarCampos;
            txt_porcentaje_ejecucion_urbanismo.Enabled = HabilitarCampos;
            ddlb_id_obligacion_VIS.Enabled = HabilitarCampos;
            ddlb_id_obligacion_VIP.Enabled = HabilitarCampos;
            txt_area_terreno_VIS.Enabled = HabilitarCampos;
            txt_area_terreno_no_VIS.Enabled = HabilitarCampos;
            txt_area_terreno_VIP.Enabled = HabilitarCampos;
            txt_area_construida_VIS.Enabled = HabilitarCampos;
            txt_area_construida_no_VIS.Enabled = HabilitarCampos;
            txt_area_construida_VIP.Enabled = HabilitarCampos;
            txt_porcentaje_obligacion_VIS.Enabled = HabilitarCampos;
            txt_porcentaje_obligacion_VIP.Enabled = HabilitarCampos;
            txt_unidades_vivienda_VIS.Enabled = HabilitarCampos;
            txt_unidades_vivienda_no_VIS.Enabled = HabilitarCampos;
            txt_unidades_vivienda_VIP.Enabled = HabilitarCampos;
            txt_area_comercio.Enabled = HabilitarCampos;
            txt_area_oficina.Enabled = HabilitarCampos;
            txt_area_institucional.Enabled = HabilitarCampos;
            txt_area_industria.Enabled = HabilitarCampos;
            txt_area_lote.Enabled = HabilitarCampos;
            txt_area_sotano.Enabled = HabilitarCampos;
            txt_area_semisotano.Enabled = HabilitarCampos;
            txt_area_primer_piso.Enabled = HabilitarCampos;
            txt_area_pisos_restantes.Enabled = HabilitarCampos;
            txt_area_construida_total.Enabled = false;
            txt_area_libre_primer_piso.Enabled = false;
            txt_porcentaje_ejecucion_construccion.Enabled = HabilitarCampos;

            ddlb_id_tipo_licencia_SelectedIndexChanged(null, null);

            oUtil.fEstiloEstadoControl(vLicenciasDetalle);
        }

        private void fLicenciasInsert()
        {
            string strResultado = oLicencias.sp_i_licencia(
              gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString(),
              ddlb_id_fuente_informacion.SelectedValue,
              ddlb_id_tipo_licencia.SelectedValue,
              txt_numero_licencia.Text,
              txt_curador.Text,
              txt_fecha_ejecutoria.Text,
              txt_termino_vigencia_meses.Text,
              txt_nombre_proyecto.Text,
              txt_plano_urbanistico_aprobado.Text,
              txt_area_bruta.Text,
              txt_area_neta.Text,
              txt_area_util.Text,
              txt_area_cesion_zonas_verdes.Text,
              txt_area_cesion_vias.Text,
              txt_area_cesion_eq_comunal.Text,
              txt_porcentaje_ejecucion_urbanismo.Text,
              ddlb_id_obligacion_VIS.SelectedValue,
              ddlb_id_obligacion_VIP.SelectedValue,
              txt_area_terreno_VIS.Text,
              txt_area_terreno_no_VIS.Text,
              txt_area_terreno_VIP.Text,
              txt_area_construida_VIS.Text,
              txt_area_construida_no_VIS.Text,
              txt_area_construida_VIP.Text,
              txt_porcentaje_obligacion_VIS.Text,
              txt_porcentaje_obligacion_VIP.Text,
              txt_unidades_vivienda_VIS.Text,
              txt_unidades_vivienda_no_VIS.Text,
              txt_unidades_vivienda_VIP.Text,
              txt_area_comercio.Text,
              txt_area_oficina.Text,
              txt_area_institucional.Text,
              txt_area_industria.Text,
              txt_area_lote.Text,
              txt_area_sotano.Text,
              txt_area_semisotano.Text,
              txt_area_primer_piso.Text,
              txt_area_pisos_restantes.Text,
              txt_area_construida_total.Text,
              txt_area_libre_primer_piso.Text,
              txt_porcentaje_ejecucion_construccion.Text
            );
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                oLog.RegistrarLogInfo(_SOURCEPAGE, "fLicenciasInsert:", clConstantes.MSG_OK_I);
                fLicenciasLimpiarDetalle();
                oVar.prDSLicencias = oLicencias.sp_s_licencias_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                fMensajeCRUD(clConstantes.MSG_OK_I, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGLICENCIAS, upLicencias);
            }
            else
            {
                oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fLicenciasInsert");
                fMensajeCRUD(clConstantes.MSG_ERR_I, (int)clConstantes.NivelMensaje.Error, _DIVMSGLICENCIAS, upLicencias);
            }
        }

        private void fLicenciasUpdate()
        {
            if (Convert.ToBoolean(oVar.prHayCambiosLicencias))
            {
                string strResultado = oLicencias.sp_u_licencia(
                  oBasic.fInt2(txt_au_licencia),
                  ddlb_id_fuente_informacion.SelectedValue,
                  ddlb_id_tipo_licencia.SelectedValue,
                  txt_numero_licencia.Text,
                  txt_curador.Text,
                  txt_fecha_ejecutoria.Text,
                  txt_termino_vigencia_meses.Text,
                  txt_nombre_proyecto.Text,
                  txt_plano_urbanistico_aprobado.Text,
                  txt_area_bruta.Text,
                  txt_area_neta.Text,
                  txt_area_util.Text,
                  txt_area_cesion_zonas_verdes.Text,
                  txt_area_cesion_vias.Text,
                  txt_area_cesion_eq_comunal.Text,
                  txt_porcentaje_ejecucion_urbanismo.Text,
                  ddlb_id_obligacion_VIS.SelectedValue,
                  ddlb_id_obligacion_VIP.SelectedValue,
                  txt_area_terreno_VIS.Text,
                  txt_area_terreno_no_VIS.Text,
                  txt_area_terreno_VIP.Text,
                  txt_area_construida_VIS.Text,
                  txt_area_construida_no_VIS.Text,
                  txt_area_construida_VIP.Text,
                  txt_porcentaje_obligacion_VIS.Text,
                  txt_porcentaje_obligacion_VIP.Text,
                  txt_unidades_vivienda_VIS.Text,
                  txt_unidades_vivienda_no_VIS.Text,
                  txt_unidades_vivienda_VIP.Text,
                  txt_area_comercio.Text,
                  txt_area_oficina.Text,
                  txt_area_institucional.Text,
                  txt_area_industria.Text,
                  txt_area_lote.Text,
                  txt_area_sotano.Text,
                  txt_area_semisotano.Text,
                  txt_area_primer_piso.Text,
                  txt_area_pisos_restantes.Text,
                  txt_area_construida_total.Text,
                  txt_area_libre_primer_piso.Text,
                  txt_porcentaje_ejecucion_construccion.Text
                );
                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fLicenciasUpdate:", clConstantes.MSG_OK_U);
                    fLicenciasLimpiarDetalle();
                    oVar.prDSLicencias = oLicencias.sp_s_licencias_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                    fMensajeCRUD(clConstantes.MSG_OK_U, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGLICENCIAS, upLicencias);
                }
                else
                {
                    oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fLicenciasUpdate");
                    fMensajeCRUD(clConstantes.MSG_ERR_U, (int)clConstantes.NivelMensaje.Error, _DIVMSGLICENCIAS, upLicencias);
                }
            }
            else
            {
                fMensajeCRUD(clConstantes.MSG_SIN_CAMBIOS, (int)clConstantes.NivelMensaje.Alerta, _DIVMSGLICENCIAS, upLicencias);
                fLicenciasLimpiarDetalle();
            }
        }

        private void fLicenciasDelete()
        {
            string strResultado = oLicencias.sp_d_licencia(gvLicencias.SelectedDataKey.Value.ToString());

            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                oLog.RegistrarLogInfo(_SOURCEPAGE, "fLicenciasDelete:", clConstantes.MSG_OK_D);
                fLicenciasLimpiarDetalle();
                oVar.prDSLicencias = oLicencias.sp_s_licencias_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());

                if (Convert.ToInt16(ViewState["RealIndexLicencias"]) > 0)
                    ViewState["RealIndexLicencias"] = Convert.ToInt16(ViewState["RealIndexLicencias"]) - 1;
                else
                    ViewState["RealIndexLicencias"] = 0;

                fMensajeCRUD(clConstantes.MSG_OK_D, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGLICENCIAS, upLicencias);
            }
            else
            {
                oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fLicenciasDelete");
                fMensajeCRUD(clConstantes.MSG_ERR_D, (int)clConstantes.NivelMensaje.Error, _DIVMSGLICENCIAS, upLicencias);
            }
        }

        private void fLicenciasValidarPermisos()
        {
            oBasic.EnableButton(false, btnLicenciasAdd);
            oBasic.EnableButton(false, btnLicenciasEdit);
            oBasic.EnableButton(false, btnLicenciasDel);

            if (oVar.prUserCod.ToString() != oVar.prUserResponsablePredioDec.ToString() && !IsHelperUser())
                return;

            if (oPermisos.TienePermisosSP("sp_i_licencia"))
                oBasic.EnableButton(true, btnLicenciasAdd);

            if (gvLicencias.Rows.Count > 0)
            {
                if (oPermisos.TienePermisosSP("sp_u_licencia"))
                    oBasic.EnableButton(true, btnLicenciasEdit);
                if (oPermisos.TienePermisosSP("sp_d_licencia"))
                    oBasic.EnableButton(true, btnLicenciasDel);
            }
        }

        protected void ddlb_id_tipo_licencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool b1 = false;
            bool b2 = false;
            switch (ddlb_id_tipo_licencia.SelectedValue.ToString())
            {
                case "85": //Construcción
                    b1 = false;
                    b2 = true;
                    break;
                case "86": //Urbanismo
                    b1 = true;
                    b2 = false;
                    break;
                case "186": //Construcción y Urbanismo
                    b1 = true;
                    b2 = true;
                    break;
            }
            oBasic.StyleCtlLbl("E", b1, txt_plano_urbanistico_aprobado, lbl_plano_urbanistico_aprobado, !b1);
            oBasic.StyleCtlLbl("E", b1, txt_area_bruta, lbl_area_bruta, !b1);
            oBasic.StyleCtlLbl("E", b1, txt_area_neta, lbl_area_neta, !b1);
            oBasic.StyleCtlLbl("E", b1, txt_area_util, lbl_area_util, !b1);
            oBasic.StyleCtlLbl("E", b1, txt_area_cesion_zonas_verdes, lbl_area_cesion_zonas_verdes, !b1);
            oBasic.StyleCtlLbl("E", b1, txt_area_cesion_vias, lbl_area_cesion_vias, !b1);
            oBasic.StyleCtlLbl("E", b1, txt_area_cesion_eq_comunal, lbl_area_cesion_eq_comunal, !b1);
            oBasic.StyleCtlLbl("E", b1, txt_porcentaje_ejecucion_urbanismo, lbl_porcentaje_ejecucion_urbanismo, !b1);
            oBasic.StyleCtlLbl("E", b2, ddlb_id_obligacion_VIS, lbl_id_obligacion_VIS, !b2);
            oBasic.StyleCtlLbl("E", b2, ddlb_id_obligacion_VIP, lbl_id_obligacion_VIP, !b2);
            oBasic.StyleCtlLbl("E", b2, txt_area_terreno_VIS, lbl_area_terreno_VIS, !b2);
            oBasic.StyleCtlLbl("E", b2, txt_area_terreno_no_VIS, lbl_area_terreno_no_VIS, !b2);
            oBasic.StyleCtlLbl("E", b2, txt_area_terreno_VIP, lbl_area_terreno_VIP, !b2);
            oBasic.StyleCtl("E", b2, txt_area_construida_VIS, !b2);
            oBasic.StyleCtl("E", b2, txt_area_construida_no_VIS, !b2);
            oBasic.StyleCtl("E", b2, txt_area_construida_VIP, !b2);
            oBasic.StyleCtl("E", b2, txt_porcentaje_obligacion_VIS, !b2);
            oBasic.StyleCtl("E", b2, txt_porcentaje_obligacion_VIP, !b2);
            oBasic.StyleCtl("E", b2, txt_unidades_vivienda_VIS, !b2);
            oBasic.StyleCtl("E", b2, txt_unidades_vivienda_no_VIS, !b2);
            oBasic.StyleCtl("E", b2, txt_unidades_vivienda_VIP, !b2);
            oBasic.StyleCtlLbl("E", b2, txt_area_comercio, lbl_area_comercio, !b2);
            oBasic.StyleCtlLbl("E", b2, txt_area_oficina, lbl_area_oficina, !b2);
            oBasic.StyleCtlLbl("E", b2, txt_area_institucional, lbl_area_institucional, !b2);
            oBasic.StyleCtlLbl("E", b2, txt_area_industria, lbl_area_industria, !b2);
            oBasic.StyleCtlLbl("E", b2, txt_area_lote, lbl_area_lote, !b2);
            oBasic.StyleCtlLbl("E", b2, txt_area_sotano, lbl_area_sotano, !b2);
            oBasic.StyleCtlLbl("E", b2, txt_area_semisotano, lbl_area_semisotano, !b2);
            oBasic.StyleCtlLbl("E", b2, txt_area_primer_piso, lbl_area_primer_piso, !b2);
            oBasic.StyleCtlLbl("E", b2, txt_area_pisos_restantes, lbl_area_pisos_restantes, !b2);
            oBasic.StyleCtlLbl("E", b2, txt_area_construida_total, lbl_area_construida_total, !b2);
            oBasic.StyleCtlLbl("E", b2, txt_area_libre_primer_piso, lbl_area_libre_primer_piso, !b2);
            oBasic.StyleCtlLbl("E", b2, txt_porcentaje_ejecucion_construccion, lbl_porcentaje_ejecucion_construccion, !b2);
        }
        #endregion

        #region------CONCEPTOS
        private void fConceptosLoadGV(string Parametro)
        {
            oVar.prDSConceptos = oConceptos.sp_s_conceptos_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
            DataSet odsCloneConceptos = ((DataSet)(oVar.prDSConceptos)).Clone();
            string strQuery;

            if (!string.IsNullOrEmpty(Parametro))
            {
                strQuery = string.Format("cod_predio_declarado = '{0}'", Parametro);
                DataRow[] oDr = ((DataSet)oVar.prDSConceptos).Tables[0].Select(strQuery);
                foreach (DataRow row in oDr)
                {
                    odsCloneConceptos.Tables[0].ImportRow(row);
                }
                gvConceptos.DataSource = odsCloneConceptos;
                gvConceptos.DataBind();
                oVar.prDSConceptosFiltro = odsCloneConceptos;
            }
            else
            {
                gvConceptos.DataSource = ((DataSet)(oVar.prDSConceptos));
                gvConceptos.DataBind();
                oVar.prDSConceptosFiltro = (DataSet)(oVar.prDSConceptos);
            }

            if (gvConceptos.Rows.Count > 0)
            {
                gvConceptos.SelectedIndex = 0;
                btnConceptosVG.Enabled = true;
            }

            mvConceptos.ActiveViewIndex = 0;
            fBotonAccionFinalEstado(btnConceptosAccionFinal, btnConceptosCancelar, false);

            fConceptosValidarPermisos();
            upConceptosBtnVistas.Update();
            upConceptos.Update();
            fCuentaRegistros(lblConceptosCuenta, gvConceptos, (DataSet)oVar.prDSConceptosFiltro, btnFirstConceptos, btnBackConceptos, btnNextConceptos, btnLastConceptos, upConceptosFoot, 0);
        }

        private void fConceptosLimpiarDetalle()
        {
            txt_au_concepto.Text = "";
            ddlb_id_tipo_concepto.SelectedIndex = -1;
            txt_fecha_concepto.Text = "";
            ddlb_id_estado_concepto.SelectedIndex = -1;
            txt_objeto.Text = "";
            txt_antecedentes.Text = "";
            txt_argumentos.Text = "";
            chk_sd_1.Checked = false;
            txt_sd_1_fecha.Text = "";
            txt_sd_1_t.Text = "";
            chk_sd_2.Checked = false;
            txt_sd_2_fecha.Text = "";
            ddlb_id_origen_certificado_catastral.SelectedIndex = -1;
            txt_sd_2_t.Text = "";
            chk_sd_3.Checked = false;
            txt_sd_3_fecha.Text = "";
            ddlb_id_origen_matricula.SelectedIndex = -1;
            txt_sd_3_t.Text = "";
            chk_sd_4.Checked = false;
            txt_sd_4_fecha.Text = "";
            txt_sd_4_t.Text = "";
            chk_sd_5.Checked = false;
            txt_sd_5_t.Text = "";
            txt_sd_otros.Text = "";
            txt_soportes.Text = "";
            txt_consideraciones.Text = "";
            txt_conclusiones.Text = "";
            txt_obs_concepto.Text = "";
            ddlb_cod_usu_concepto.SelectedIndex = -1;
            ddlb_cod_usu_concepto_revisa.SelectedIndex = -1;
            ddlb_cod_usu_concepto_aprueba.SelectedIndex = -1;
        }

        private void fConceptosDetalle()
        {
            mvConceptos.ActiveViewIndex = 1;
            int Indice = Convert.ToInt16(ViewState["RealIndexConceptos"]);

            DataSet dsTmp = (DataSet)oVar.prDSConceptosFiltro;

            txt_au_concepto.Text = dsTmp.Tables[0].Rows[Indice]["au_concepto"].ToString();
            oBasic.fDetalleDropDown(ddlb_id_tipo_concepto, dsTmp.Tables[0].Rows[Indice]["id_tipo_concepto"].ToString());
            oBasic.fLoadUsuariosFiltro(ddlb_cod_usu_concepto, 7, dsTmp.Tables[0].Rows[Indice]["cod_usu_concepto"].ToString());
            oBasic.fDetalleFecha(txt_fecha_concepto, dsTmp.Tables[0].Rows[Indice]["fecha_concepto"].ToString());
            oBasic.fDetalleDropDown(ddlb_id_estado_concepto, dsTmp.Tables[0].Rows[Indice]["id_estado_concepto"].ToString());

            txt_objeto.Text = dsTmp.Tables[0].Rows[Indice]["objeto"].ToString();
            txt_antecedentes.Text = dsTmp.Tables[0].Rows[Indice]["antecedentes"].ToString();
            txt_argumentos.Text = dsTmp.Tables[0].Rows[Indice]["argumentos"].ToString();

            chk_sd_1.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["sd_1"].ToString()));
            oBasic.fDetalleFecha(txt_sd_1_fecha, dsTmp.Tables[0].Rows[Indice]["sd_1_fecha"].ToString());
            txt_sd_1_t.Text = dsTmp.Tables[0].Rows[Indice]["sd_1_t"].ToString();

            chk_sd_2.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["sd_2"].ToString()));
            oBasic.fDetalleFecha(txt_sd_2_fecha, dsTmp.Tables[0].Rows[Indice]["sd_2_fecha"].ToString());
            oBasic.fDetalleDropDown(ddlb_id_origen_certificado_catastral, dsTmp.Tables[0].Rows[Indice]["id_origen_certificado_catastral"].ToString());
            txt_sd_2_t.Text = dsTmp.Tables[0].Rows[Indice]["sd_2_t"].ToString();

            chk_sd_3.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["sd_3"].ToString()));
            oBasic.fDetalleFecha(txt_sd_3_fecha, dsTmp.Tables[0].Rows[Indice]["sd_3_fecha"].ToString());
            oBasic.fDetalleDropDown(ddlb_id_origen_matricula, dsTmp.Tables[0].Rows[Indice]["id_origen_matricula"].ToString());
            txt_sd_3_t.Text = dsTmp.Tables[0].Rows[Indice]["sd_3_t"].ToString();

            chk_sd_4.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["sd_4"].ToString()));
            oBasic.fDetalleFecha(txt_sd_4_fecha, dsTmp.Tables[0].Rows[Indice]["sd_4_fecha"].ToString());
            txt_sd_4_t.Text = dsTmp.Tables[0].Rows[Indice]["sd_4_t"].ToString();

            chk_sd_5.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["sd_5"].ToString()));
            txt_sd_5_t.Text = dsTmp.Tables[0].Rows[Indice]["sd_5_t"].ToString();

            txt_sd_otros.Text = dsTmp.Tables[0].Rows[Indice]["sd_otros"].ToString();
            txt_soportes.Text = dsTmp.Tables[0].Rows[Indice]["soportes"].ToString();

            txt_consideraciones.Text = dsTmp.Tables[0].Rows[Indice]["consideraciones"].ToString();
            txt_conclusiones.Text = dsTmp.Tables[0].Rows[Indice]["conclusiones"].ToString();
            txt_obs_concepto.Text = dsTmp.Tables[0].Rows[Indice]["obs_concepto"].ToString();

            hf_tipo_declaratoria.Value = dsTmp.Tables[0].Rows[Indice]["tipo_declaratoria"].ToString();
            hf_resolucion_declaratoria.Value = dsTmp.Tables[0].Rows[Indice]["resolucion_declaratoria"].ToString();
            hf_ano_resolucion_declaratoria.Value = dsTmp.Tables[0].Rows[Indice]["ano_resolucion_declaratoria"].ToString();

            oBasic.fLoadUsuariosFiltro(ddlb_cod_usu_concepto_revisa, 8, dsTmp.Tables[0].Rows[Indice]["cod_usu_concepto_revisa"].ToString());
            oBasic.fLoadUsuariosFiltro(ddlb_cod_usu_concepto_aprueba, 9, dsTmp.Tables[0].Rows[Indice]["cod_usu_concepto_aprueba"].ToString());

            sd_Changed(chk_sd_1, null);
            sd_Changed(chk_sd_2, null);
            sd_Changed(chk_sd_3, null);
            sd_Changed(chk_sd_4, null);
            sd_Changed(chk_sd_5, null);

            txt_fecha_concepto_TextChanged(null, null);

            fCuentaRegistros(lblConceptosCuenta, gvConceptos, (DataSet)oVar.prDSConceptosFiltro, btnFirstConceptos, btnBackConceptos, btnNextConceptos, btnLastConceptos, upConceptosFoot, Indice);
        }

        private void fConceptosEstadoDetalle(bool HabilitarCampos, bool Creacion)
        {
            txt_au_concepto.Enabled = HabilitarCampos;
            ddlb_id_tipo_concepto.Enabled = HabilitarCampos;
            ddlb_cod_usu_concepto.Enabled = HabilitarCampos;
            txt_fecha_concepto.Enabled = HabilitarCampos;
            ddlb_id_estado_concepto.Enabled = HabilitarCampos;

            txt_objeto.Enabled = HabilitarCampos;
            txt_antecedentes.Enabled = HabilitarCampos;
            txt_argumentos.Enabled = HabilitarCampos;

            chk_sd_1.Enabled = HabilitarCampos;
            txt_sd_1_fecha.Enabled = HabilitarCampos;
            txt_sd_1_t.Enabled = HabilitarCampos;

            chk_sd_2.Enabled = HabilitarCampos;
            txt_sd_2_fecha.Enabled = HabilitarCampos;
            ddlb_id_origen_certificado_catastral.Enabled = HabilitarCampos;
            txt_sd_2_t.Enabled = HabilitarCampos;

            chk_sd_3.Enabled = HabilitarCampos;
            txt_sd_3_fecha.Enabled = HabilitarCampos;
            ddlb_id_origen_matricula.Enabled = HabilitarCampos;
            txt_sd_3_t.Enabled = HabilitarCampos;

            chk_sd_4.Enabled = HabilitarCampos;
            txt_sd_4_fecha.Enabled = HabilitarCampos;
            txt_sd_4_t.Enabled = HabilitarCampos;

            chk_sd_5.Enabled = HabilitarCampos;
            txt_sd_5_t.Enabled = HabilitarCampos;
            txt_sd_otros.Enabled = HabilitarCampos;

            txt_soportes.Enabled = HabilitarCampos;
            txt_consideraciones.Enabled = HabilitarCampos;
            txt_conclusiones.Enabled = HabilitarCampos;
            txt_obs_concepto.Enabled = HabilitarCampos;

            ddlb_cod_usu_concepto_revisa.Enabled = HabilitarCampos;
            ddlb_cod_usu_concepto_aprueba.Enabled = HabilitarCampos;

            oBasic.fValidarLongitud("Observaciones", rev_obs_concepto, 500);

            if (Creacion) //Creacion de concepto
            {
                string strConclusion = "";
                int iOrden = 1;
                int iOk = 0;

                string s1 = "lo que constituye una de las condiciones técnicas y urbanísticas señaladas en el literal b) del numeral 4. LINEAMIENTOS O POLÍTICAS DE OPERACIÓN del Procedimiento de " +
                      "seguimiento al cumplimiento de la declaratoria de desarrollo o construcción prioritaria PM02-PR06, que determina la no pertinencia de enajenación forzosa";

                string s = "El análisis técnico y los soportes documentales que reposan en el expediente permiten establecer que el predio cuenta con un área de terreno inferior a 500m2 " +
                  "(área predio:{1}m2), " + s1 + ", " + "toda vez que el potencial edificatorio en predios con estas áreas, es inferior a 50 unidades de vivienda, debido a que la aplicación de los " +
                  "índices de ocupación y construcción, así como las normas volumétricas, establecen que el potencial edificatorio resultante, con destinación específica para VIP y/o VIS, " +
                  "no absorbe los costos directos e indirectos que se requieren para el desarrollo de un proyecto inmobiliario.";
                if (gvPredios.SelectedRow.Cells[8].Text.ToString() != "&nbsp;" && (Convert.ToDecimal(gvPredios.SelectedRow.Cells[8].Text) < 500))
                {
                    strConclusion = iOrden.ToString() + ". " + string.Format(s, gvPredios.SelectedRow.Cells[0].Text, gvPredios.SelectedRow.Cells[7].Text) + System.Environment.NewLine + System.Environment.NewLine;
                    iOrden++;
                    iOk = 1;
                }

                DataSet oDSConclusiones = oAfectaciones.sp_s_afectaciones_chip(gvPredios.SelectedRow.Cells[0].Text);
                int iRow = 0;

                foreach (DataRow oRow in oDSConclusiones.Tables[0].Rows)
                {
                    if (oDSConclusiones.Tables[0].Rows[iRow][1].ToString() == "161" && oDSConclusiones.Tables[0].Rows[iRow][9].ToString() != "")
                    {
                        s = "El análisis de los soportes documentales que reposan en el expediente permite determinar que el predio presenta condición de amenaza alta por inundación, " + s1 + ".";
                        strConclusion = strConclusion + iOrden.ToString() + ". " + s + System.Environment.NewLine + System.Environment.NewLine;
                        iOrden++;
                        iOk = 1;
                    }
                    else if (oDSConclusiones.Tables[0].Rows[iRow][1].ToString() == "162" && oDSConclusiones.Tables[0].Rows[iRow][9].ToString() != "")
                    {
                        s = "El análisis de los soportes documentales que reposan en el expediente permite determinar que el predio presenta condición de amenaza alta por remoción en masa, " + s1 + ".";
                        strConclusion = strConclusion + iOrden.ToString() + ". " + s + System.Environment.NewLine + System.Environment.NewLine;
                        iOrden++;
                        iOk = 1;
                    }
                    else if (oDSConclusiones.Tables[0].Rows[iRow][1].ToString() == "164")
                    {
                        s = "El análisis técnico y los soportes documentales que reposan en el expediente permiten establecer que el predio se encuentra localizado en el área de influencia de " +
                          "Bien de Interés Cultural del ámbito Nacional (XXXXXX), " + s1 + ", dado que existe incertidumbre normativa respecto al potencial en este tipo de predios en los cuales " +
                          "su desarrollo está sujeto a que los valores patrimoniales del bien de Interés Cultural Nacional se conserven. toda vez que depende de la formulación de un PEMP.";
                        strConclusion = strConclusion + iOrden.ToString() + ". " + s + System.Environment.NewLine + System.Environment.NewLine;
                        iOrden++;
                    }
                    else if (oDSConclusiones.Tables[0].Rows[iRow][1].ToString() == "165")
                    {
                        s = "El análisis de los soportes documentales que reposan en el expediente permite determinar que el predio es un predio con densidad restringida de acuerdo con XXXXX, " +
                          "por encontrarse ubicado en XXXXXXX , donde solo se permite XXXXX, " + s1 + ", dado que existe incertidumbre normativa respecto al potencial en este tipo de predios en los cuales " +
                          "su desarrollo está sujeto a que los valores patrimoniales del bien de Interés Cultural Nacional se conserven. toda vez que depende de la formulación de un PEMP.";
                        strConclusion = strConclusion + iOrden.ToString() + ". " + s + System.Environment.NewLine + System.Environment.NewLine;
                        iOrden++;
                        iOk = 1;
                    }
                    else if (oDSConclusiones.Tables[0].Rows[iRow][1].ToString() == "166")
                    {
                        s = "El análisis de los soportes documentales que reposan en el expediente permite determinar que el predio se encuentra en área de influencia aeroportuaria, " + s1 +
                          ", toda vez que de acuerdo con el artículo 1 del Decreto Distrital 765 de 1999, corresponde al área en la que es necesario restringir algunos usos, " +
                          "en especial el residencial, así como incentivar a aparición de otros usos que apoyen las actividades del aeropuerto o que sean compatibles con ellas.";
                        strConclusion = strConclusion + iOrden.ToString() + ". " + s + System.Environment.NewLine + System.Environment.NewLine;
                        iOrden++;
                        iOk = 1;
                    }
                    else if (oDSConclusiones.Tables[0].Rows[iRow][1].ToString() == "170")
                    {
                        s = "El análisis de los soportes documentales que reposan en el expediente permite determinar que el predio se localiza en zona de recuperación morfológica, " + s1 +
                          ", toda vez que estos predios deben realizar un manejo especial para la recomposición geomorfológica de su suelo y su incorporación al desarrollo urbano.";
                        strConclusion = strConclusion + iOrden.ToString() + ". " + s + System.Environment.NewLine + System.Environment.NewLine;
                        iOrden++;
                        iOk = 1;
                    }
                    else if (oDSConclusiones.Tables[0].Rows[iRow][1].ToString() == "174")
                    {
                        s = "El análisis de los soportes documentales que reposan en el expediente permite determinar que el predio se localiza en zona de protección, " + s1 +
                          ", toda vez que constituyen elementos de la estructura ecológica principal, áreas del relleno sanitario Doña Juana, áreas incluidas en las categorías de protección en suelo rural.";
                        strConclusion = strConclusion + iOrden.ToString() + ". " + s + System.Environment.NewLine + System.Environment.NewLine;
                        iOrden++;
                        iOk = 1;
                    }
                    iRow++;
                }
                if (iOk == 1)
                {
                    s = "Por lo anterior de conformidad con el mismo procedimiento PM02-PR06 se remite el expediente a la Subsecretaría Jurídica para que emita el correspondiente acto administrativo " +
                      "mediante el cual se ordena NO dar inicio al proceso de enajenación forzosa.";
                    txt_conclusiones.Text = strConclusion + s + System.Environment.NewLine;
                }
            }
        }

        private void fConceptosInsert()
        {
            string strResultado = oConceptos.sp_i_concepto(
              gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString(),
              ddlb_id_tipo_concepto.SelectedValue,
              ddlb_cod_usu_concepto.SelectedValue,
              txt_fecha_concepto.Text,
              ddlb_id_estado_concepto.SelectedValue,
              txt_objeto.Text,
              txt_antecedentes.Text,
              txt_argumentos.Text,
              chk_sd_1.Checked.ToString(),
              txt_sd_1_fecha.Text,
              txt_sd_1_t.Text,
              chk_sd_2.Checked.ToString(),
              txt_sd_2_fecha.Text,
              ddlb_id_origen_certificado_catastral.SelectedValue,
              txt_sd_2_t.Text,
              chk_sd_3.Checked.ToString(),
              txt_sd_3_fecha.Text,
              ddlb_id_origen_matricula.SelectedValue,
              txt_sd_3_t.Text,
              chk_sd_4.Checked.ToString(),
              txt_sd_4_fecha.Text,
              txt_sd_4_t.Text,
              chk_sd_5.Checked.ToString(),
              txt_sd_5_t.Text,
              txt_sd_otros.Text,
              txt_soportes.Text,
              txt_consideraciones.Text,
              txt_conclusiones.Text,
              txt_obs_concepto.Text,
              ddlb_cod_usu_concepto_revisa.SelectedValue,
              ddlb_cod_usu_concepto_aprueba.SelectedValue);
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                oLog.RegistrarLogInfo(_SOURCEPAGE, "fConceptosInsert:", clConstantes.MSG_OK_I);
                fConceptosLimpiarDetalle();
                oVar.prDSConceptos = oConceptos.sp_s_conceptos_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                fMensajeCRUD(clConstantes.MSG_OK_I, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGCONCEPTOS, upConceptos);
            }
            else
            {
                oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fConceptosInsert");
                fMensajeCRUD(clConstantes.MSG_ERR_I, (int)clConstantes.NivelMensaje.Error, _DIVMSGCONCEPTOS, upConceptos);
            }
        }

        private void fConceptosUpdate()
        {
            if (Convert.ToBoolean(oVar.prHayCambiosConceptos))
            {
                string strResultado = oConceptos.sp_u_concepto(
                  txt_au_concepto.Text,
                  ddlb_id_tipo_concepto.SelectedValue,
                  ddlb_cod_usu_concepto.SelectedValue,
                  txt_fecha_concepto.Text,
                  ddlb_id_estado_concepto.SelectedValue,
                  txt_objeto.Text,
                  txt_antecedentes.Text,
                  txt_argumentos.Text,
                  chk_sd_1.Checked.ToString(),
                  txt_sd_1_fecha.Text,
                  txt_sd_1_t.Text,
                  chk_sd_2.Checked.ToString(),
                  txt_sd_2_fecha.Text,
                  ddlb_id_origen_certificado_catastral.SelectedValue,
                  txt_sd_2_t.Text,
                  chk_sd_3.Checked.ToString(),
                  txt_sd_3_fecha.Text,
                  ddlb_id_origen_matricula.SelectedValue,
                  txt_sd_3_t.Text,
                  chk_sd_4.Checked.ToString(),
                  txt_sd_4_fecha.Text,
                  txt_sd_4_t.Text,
                  chk_sd_5.Checked.ToString(),
                  txt_sd_5_t.Text,
                  txt_sd_otros.Text,
                  txt_soportes.Text,
                  txt_consideraciones.Text,
                  txt_conclusiones.Text,
                  txt_obs_concepto.Text,
                  ddlb_cod_usu_concepto_revisa.SelectedValue,
                  ddlb_cod_usu_concepto_aprueba.SelectedValue);
                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fConceptosUpdate:", clConstantes.MSG_OK_U);
                    fConceptosLimpiarDetalle();
                    oVar.prDSConceptos = oConceptos.sp_s_conceptos_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                    fMensajeCRUD(clConstantes.MSG_OK_U, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGCONCEPTOS, upConceptos);
                }
                else
                {
                    oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fConceptosUpdate");
                    fMensajeCRUD(clConstantes.MSG_ERR_U, (int)clConstantes.NivelMensaje.Error, _DIVMSGCONCEPTOS, upConceptos);
                }
            }
            else
            {
                fMensajeCRUD(clConstantes.MSG_SIN_CAMBIOS, (int)clConstantes.NivelMensaje.Alerta, _DIVMSGCONCEPTOS, upConceptos);
                fConceptosLimpiarDetalle();
            }
        }

        private void fConceptosDelete()
        {
            string strResultado = oConceptos.sp_d_concepto(gvConceptos.SelectedDataKey.Value.ToString());

            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                oLog.RegistrarLogInfo(_SOURCEPAGE, "fConceptosDelete:", clConstantes.MSG_OK_D);
                fConceptosLimpiarDetalle();
                oVar.prDSConceptos = oConceptos.sp_s_conceptos_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());

                if (Convert.ToInt16(ViewState["RealIndexConceptos"]) > 0)
                    ViewState["RealIndexConceptos"] = Convert.ToInt16(ViewState["RealIndexConceptos"]) - 1;
                else
                    ViewState["RealIndexConceptos"] = 0;

                fMensajeCRUD(clConstantes.MSG_OK_D, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGCONCEPTOS, upConceptos);
            }
            else
            {
                oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fConceptosDelete");
                fMensajeCRUD(clConstantes.MSG_ERR_D, (int)clConstantes.NivelMensaje.Error, _DIVMSGCONCEPTOS, upConceptos);
            }
        }

        private void fConceptosValidarPermisos()
        {
            oBasic.EnableButton(false, btnConceptosAdd);
            oBasic.EnableButton(false, btnConceptosEdit);
            oBasic.EnableButton(false, btnConceptosDel);

            if (oVar.prUserCod.ToString() != oVar.prUserResponsablePredioDec.ToString() && !IsHelperUser())
                return;

            if (oPermisos.TienePermisosSP("sp_i_concepto"))
                oBasic.EnableButton(true, btnConceptosAdd);

            if (gvConceptos.Rows.Count > 0)
            {
                if (oPermisos.TienePermisosSP("sp_u_concepto"))
                    oBasic.EnableButton(true, btnConceptosEdit);
                if (oPermisos.TienePermisosSP("sp_d_concepto"))
                    oBasic.EnableButton(true, btnConceptosDel);
            }
        }

        private void fPreparaPlantillaConcepto()
        {
            Exception rtaCreacion;
            string pathPlantillaSource = oVar.prPathPlantillaConceptos.ToString();
            string fileKey = oVar.prUser.ToString() + oUtil.CrearUniqueName();
            string pathPlantillaEdit = string.Format("{0}\\{1}{2}", Path.GetDirectoryName(pathPlantillaSource), fileKey, Path.GetExtension(pathPlantillaSource));

            try
            {
                System.IO.File.Copy(pathPlantillaSource, pathPlantillaEdit, true);

                if (string.IsNullOrEmpty(txt_au_concepto.Text))
                    rtaCreacion = oConceptoTemplate.fCrearTemplate(pathPlantillaEdit, oConceptos.sp_s_concepto_plantilla(gvConceptos.SelectedDataKey.Value.ToString()));
                else
                    rtaCreacion = oConceptoTemplate.fCrearTemplate(pathPlantillaEdit, oConceptos.sp_s_concepto_plantilla(txt_au_concepto.Text));

                if (rtaCreacion == null)
                {
                    fMensajeCRUD("Documento de concepto creado.", (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGCONCEPTOS, upConceptos);
                    string file_name = gvPredios.SelectedRow.Cells[0].Text;
                    fGuardarPlantilla(pathPlantillaEdit, "Concepto_" + file_name + ".docx");
                }
                else
                {
                    oLog.RegistrarLogError(rtaCreacion, _SOURCEPAGE, "fCrearPlantilla");
                    fMensajeCRUD("No fue posible crear documento de concepto.", (int)clConstantes.NivelMensaje.Error, _DIVMSGCONCEPTOS, upConceptos);
                }
            }
            catch (Exception MyErr)
            {
                oLog.RegistrarLogError(MyErr, _SOURCEPAGE, "fPreparaPlantilla");
            }
            finally
            {
                File.Delete(pathPlantillaEdit);
                upConceptosBtnVistas.Update();
                upConceptos.Update();
            }
        }

        protected void ddlb_id_tipo_concepto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlb_id_tipo_concepto.SelectedValue.ToString() == "39") //concepto técnico
                txt_objeto.Text = string.Format(_CONCEPTO_39, hf_tipo_declaratoria.Value, hf_resolucion_declaratoria.Value, hf_ano_resolucion_declaratoria.Value);
            else if (ddlb_id_tipo_concepto.SelectedValue.ToString() == "40") //Evaluación técnica previa a la enajenación forzosa en pública subasta
                txt_objeto.Text = string.Format(_CONCEPTO_40, "XXX", "XXX", "XXX");
            else if (ddlb_id_tipo_concepto.SelectedValue.ToString() == "38") //Alcance a evaluación técnica previa a la enajenación forzosa en pública subasta
                txt_objeto.Text = string.Format(_CONCEPTO_38, "XXX", "XXX", "XXX", "XXX");
            else
                txt_objeto.Text = "";
            oVar.prHayCambiosConceptos = true;
        }

        protected void sd_Changed(object sender, EventArgs e)
        {
            WebControl ctl = sender as WebControl;
            string id;
            if (ctl.ID.ToString() == "ddlb_id_origen_certificado_catastral")
                id = "2";
            else if (ctl.ID.ToString() == "ddlb_id_origen_matricula")
                id = "3";
            else
                id = ctl.ID.ToString().Substring(7, 1);

            DropDownList ddlb = null;
            Label lbl = null;
            if (id == "2")
            {
                ddlb = (System.Web.UI.WebControls.DropDownList)tpConceptos3.FindControl("ddlb_id_origen_certificado_catastral");
                lbl = (System.Web.UI.WebControls.Label)tpConceptos3.FindControl("lbl_id_origen_certificado_catastral");
            }
            else if (id == "3")
            {
                ddlb = (System.Web.UI.WebControls.DropDownList)tpConceptos3.FindControl("ddlb_id_origen_matricula");
                lbl = (System.Web.UI.WebControls.Label)tpConceptos3.FindControl("lbl_id_origen_matricula");
            }

            CheckBox chk = (System.Web.UI.WebControls.CheckBox)tpConceptos3.FindControl("chk_sd_" + id);
            TextBox txt_f = (System.Web.UI.WebControls.TextBox)tpConceptos3.FindControl("txt_sd_" + id + "_fecha");
            string txt_obs_sd;
            txt_obs_sd = _CONCEPTO_SD[Convert.ToInt16(id) - 1];
            TextBox txt_obs = (System.Web.UI.WebControls.TextBox)tpConceptos3.FindControl("txt_sd_" + id + "_t");

            if (chk.Checked == false)
            {
                txt_obs.Text = string.Empty;
                txt_obs.Enabled = false;
                txt_obs.CssClass = txt_obs.CssClass.Insert(txt_obs.CssClass.Length, " txtDis");
                if (id == "2" || id == "3")
                {
                    ddlb.SelectedIndex = -1;
                    ddlb.Enabled = false;
                    ddlb.CssClass = ddlb.CssClass.Insert(ddlb.CssClass.Length, " txtDis");
                    lbl.CssClass = lbl.CssClass.Insert(lbl.CssClass.Length, " lblDis");
                }
                if (id != "5")
                {
                    txt_f.Text = string.Empty;
                    txt_f.Enabled = false;
                    txt_f.CssClass = txt_f.CssClass.Insert(txt_f.CssClass.Length, " txtDis");
                }
            }
            else if (chk.Checked == true)
            {
                txt_obs.Enabled = true;
                txt_obs.CssClass = txt_obs.CssClass.Replace(" txtDis", "");
                if (id != "5")
                {
                    txt_f.Enabled = true;
                    txt_f.CssClass = txt_f.CssClass.Replace(" txtDis", "");
                    if (id == "2" || id == "3")
                    {
                        ddlb.Enabled = true;
                        ddlb.CssClass = ddlb.CssClass.Replace(" txtDis", "");
                        lbl.CssClass = lbl.CssClass.Replace(" lblDis", "");
                        if (txt_f.Text != string.Empty)
                        {
                            txt_obs.Text = string.Format(txt_obs_sd, Convert.ToDateTime(txt_f.Text).ToString("dd 'de' MMMM 'de' yyyy", CultureInfo.CreateSpecificCulture("es-ES")), ddlb.SelectedItem.ToString());
                        }
                    }
                    else
                    {
                        try
                        {
                            txt_obs.Text = string.Format(txt_obs_sd, Convert.ToDateTime(txt_f.Text).ToString("dd 'de' MMMM 'de' yyyy", CultureInfo.CreateSpecificCulture("es-ES")));
                        }
                        catch { }
                    }

                }
                else
                    txt_obs.Text = txt_obs_sd;
            }
        }

        protected void txt_fecha_concepto_TextChanged(object sender, EventArgs e)
        {
            try
            {
                oBasic.fValidarFecha_old("fecha concepto", cal_fecha_concepto, rv_fecha_concepto, "0", 0);

                string tmp_id_estado_concepto = ddlb_id_estado_concepto.SelectedValue.ToString();
                ddlb_id_estado_concepto.Items.Clear();
                if (Convert.ToDateTime(txt_fecha_concepto.Text) >= Convert.ToDateTime("2019-04-01"))
                    ddlb_id_estado_concepto.DataSource = oIdentidades.sp_s_identidad_id_categoria_op("10", "1");
                else
                    ddlb_id_estado_concepto.DataSource = oIdentidades.sp_s_identidad_id_categoria_op("10", "0");
                ddlb_id_estado_concepto.DataTextField = "nombre_identidad";
                ddlb_id_estado_concepto.DataValueField = "id_identidad";
                ddlb_id_estado_concepto.DataBind();
                oBasic.fDetalleDropDown(ddlb_id_estado_concepto, tmp_id_estado_concepto);
                oVar.prHayCambiosConceptos = true;
            }
            catch { }
        }
        #endregion

        #region------ACTOS ADMINISTRATIVOS
        private void fActosAdmLoadGV(string Parametro)
        {
            oVar.prDSActosAdm = oActosAdm.sp_s_actos_administrativos_cod_predio(Parametro);
            DataSet odsCloneActosAdm = ((DataSet)(oVar.prDSActosAdm)).Clone();

            if (!string.IsNullOrEmpty(Parametro))
            {
                string strQuery = string.Format("cod_predio_declarado = '{0}'", Parametro);
                DataRow[] oDr = ((DataSet)oVar.prDSActosAdm).Tables[0].Select(strQuery);
                foreach (DataRow row in oDr)
                {
                    odsCloneActosAdm.Tables[0].ImportRow(row);
                }
                gvActosAdm.DataSource = odsCloneActosAdm;
                gvActosAdm.DataBind();
                oVar.prDSActosAdmFiltro = odsCloneActosAdm;
            }
            else
            {
                gvActosAdm.DataSource = ((DataSet)(oVar.prDSActosAdm));
                gvActosAdm.DataBind();
                oVar.prDSActosAdmFiltro = (DataSet)(oVar.prDSActosAdm);
            }
            if (gvActosAdm.Rows.Count > 0)
            {
                gvActosAdm.SelectedIndex = 0;
                btnActosAdmVG.Enabled = true;
            }

            mvActosAdm.ActiveViewIndex = 0;
            fBotonAccionFinalEstado(btnActosAdmAccionFinal, btnActosAdmCancelar, false);

            fActosAdmValidarPermisos();
            upActosAdmBtnVistas.Update();
            upActosAdm.Update();
            fCuentaRegistros(lblActosAdmCuenta, gvActosAdm, (DataSet)oVar.prDSActosAdmFiltro, btnFirstActosAdm, btnBackActosAdm, btnNextActosAdm, btnLastActosAdm, upActosAdmFoot, 0);
        }

        private void fActosAdmLimpiarDetalle()
        {
            oBasic.fClearControls(vActosAdmDetalle);
        }

        private void fActosAdmDetalle()
        {
            mvActosAdm.ActiveViewIndex = 1;
            int Indice = Convert.ToInt16(ViewState["RealIndexActosAdm"]);

            DataSet dsTmp = (DataSet)oVar.prDSActosAdmFiltro;
            DataRow dRow = dsTmp.Tables[0].Rows[Indice];

            oBasic.fValueControls(vActosAdmDetalle, dRow);

            fCuentaRegistros(lblActosAdmCuenta, gvActosAdm, (DataSet)oVar.prDSActosAdmFiltro, btnFirstActosAdm, btnBackActosAdm, btnNextActosAdm, btnLastActosAdm, upActosAdmFoot, Indice);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "defaultddlbActosAdm();", true);
        }

        private void fActosAdmEstadoDetalle(bool HabilitarCampos)
        {
            mvActosAdm.ActiveViewIndex = 1;
            txt_au_acto_administrativo.Enabled = false;
            txt_fecha_vencimiento_acto.Enabled = false;

            ddlb_id_estado_predio_declarado_SelectedIndexChanged(null, null);

            oBasic.fValidarFecha_old("fecha acto", cal_fecha_acto, rv_fecha_acto, "0", 0);
            oBasic.fValidarFecha_old("fecha notificación", cal_fecha_notificacion_acto, rv_fecha_notificacion_acto, txt_fecha_acto.Text, 0);
            oBasic.fValidarFecha_old("fecha ejecutoria", cal_fecha_ejecutoria_acto, rv_fecha_ejecutoria_acto, string.IsNullOrEmpty(txt_fecha_notificacion_acto.Text) ? txt_fecha_acto.Text : txt_fecha_notificacion_acto.Text, 0);

            txt_fecha_notificacion_acto.Focus();
            bool b = false;
            if (HabilitarCampos == true || gvActosAdm.SelectedRow.Cells[12].Text != "1")
            {
                b = true;
                ddlb_id_tipo_acto.Focus();
            }
            oBasic.StyleCtlLbl("E", b, txt_numero_acto, lblnumeroacto, false);
            oBasic.StyleCtlLbl("E", b, txt_fecha_acto, lblfechaacto, false);
            oBasic.StyleCtlLbl("E", b, ddlb_id_tipo_acto, lblidtipoacto, false);
            oBasic.StyleCtlLbl("E", b, ddlb_id_estado_predio_declarado, lblidestadoprediodeclarado, false);
        }

        private void fActosAdmInsert()
        {
            txt_suspension_meses_TextChanged(null, null);
            string strResultado = oActosAdm.sp_i_acto_administrativo(
              gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString(),
              oBasic.fInt2(ddlb_id_tipo_acto),
              txt_numero_acto.Text,
              txt_fecha_acto.Text,
              oBasic.fInt2(ddlb_id_estado_predio_declarado),
              oBasic.fInt2(txt_suspension_meses),
              oBasic.fInt2(txt_suspension_dias),
              oBasic.fInt2(ddlb_id_causal_acto),
              txt_fecha_notificacion_acto.Text,
              txt_fecha_ejecutoria_acto.Text,
              txt_fecha_vencimiento_acto.Text,
              txt_obs_acto.Text);
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                oLog.RegistrarLogInfo(_SOURCEPAGE, "fActosAdmInsert:", clConstantes.MSG_OK_I);
                fActosAdmLimpiarDetalle();
                oVar.prDSActosAdm = oActosAdm.sp_s_actos_administrativos_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                fMensajeCRUD(clConstantes.MSG_OK_I, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGACTOSADM, upActosAdm);
            }
            else
            {
                oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fActosAdmInsert");
                fMensajeCRUD(clConstantes.MSG_ERR_I, (int)clConstantes.NivelMensaje.Error, _DIVMSGACTOSADM, upActosAdm);
            }
        }

        private void fActosAdmUpdate()
        {
            if (Convert.ToBoolean(oVar.prHayCambiosActosAdm))
            {
                txt_suspension_meses_TextChanged(null, null);
                string strResultado = oActosAdm.sp_u_acto_administrativo(
                  oBasic.fInt2(txt_au_acto_administrativo),
                  oBasic.fInt2(ddlb_id_tipo_acto),
                  txt_numero_acto.Text,
                  txt_fecha_acto.Text,
                  oBasic.fInt2(ddlb_id_estado_predio_declarado),
                  oBasic.fInt2(txt_suspension_meses),
                  oBasic.fInt2(txt_suspension_dias),
                  oBasic.fInt2(ddlb_id_causal_acto),
                  txt_fecha_notificacion_acto.Text,
                  txt_fecha_ejecutoria_acto.Text,
                  txt_fecha_vencimiento_acto.Text,
                  txt_obs_acto.Text);
                if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
                {
                    oLog.RegistrarLogInfo(_SOURCEPAGE, "fActosAdmUpdate:", clConstantes.MSG_OK_U);
                    fActosAdmLimpiarDetalle();
                    oVar.prDSActosAdm = oActosAdm.sp_s_actos_administrativos_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());
                    fMensajeCRUD(clConstantes.MSG_OK_U, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGACTOSADM, upActosAdm);
                    fPrediosDecLoadGV(gvPredios.SelectedDataKey.Value.ToString());
                }
                else
                {
                    oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fActosAdmUpdate");
                    fMensajeCRUD(clConstantes.MSG_ERR_U, (int)clConstantes.NivelMensaje.Error, _DIVMSGACTOSADM, upActosAdm);
                }
            }
            else
            {
                fMensajeCRUD(clConstantes.MSG_SIN_CAMBIOS, (int)clConstantes.NivelMensaje.Alerta, _DIVMSGACTOSADM, upActosAdm);
                fActosAdmLimpiarDetalle();
            }
        }

        private void fActosAdmDelete()
        {
            string strResultado = oActosAdm.sp_d_acto_administrativo(gvActosAdm.SelectedDataKey.Value.ToString());

            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
                oLog.RegistrarLogInfo(_SOURCEPAGE, "fActosAdmDelete:", clConstantes.MSG_OK_D);
                fActosAdmLimpiarDetalle();
                oVar.prDSActosAdm = oActosAdm.sp_s_actos_administrativos_cod_predio(gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString());

                if (Convert.ToInt16(ViewState["RealIndexActosAdm"]) > 0)
                    ViewState["RealIndexActosAdm"] = Convert.ToInt16(ViewState["RealIndexActosAdm"]) - 1;
                else
                    ViewState["RealIndexActosAdm"] = 0;

                fMensajeCRUD(clConstantes.MSG_OK_D, (int)clConstantes.NivelMensaje.Exitoso, _DIVMSGACTOSADM, upActosAdm);
            }
            else
            {
                oLog.RegistrarLogError(strResultado, _SOURCEPAGE, "fActosAdmDelete");
                fMensajeCRUD(clConstantes.MSG_ERR_D, (int)clConstantes.NivelMensaje.Error, _DIVMSGACTOSADM, upActosAdm);
            }
        }

        private void fActosAdmValidarPermisos()
        {
            oBasic.EnableButton(false, btnActosAdmAdd);
            oBasic.EnableButton(false, btnActosAdmEdit);
            oBasic.EnableButton(false, btnActosAdmDel);

            if (!Convert.ToBoolean(Convert.ToByte(oVar.prUserEditaActos.ToString())))
                if (oVar.prUserCod.ToString() != oVar.prUserResponsablePredioDec.ToString() && !IsHelperUser())
                    return;

            if (oPermisos.TienePermisosSP("sp_i_acto_administrativo"))
                oBasic.EnableButton(true, btnActosAdmAdd);

            if (gvActosAdm.Rows.Count > 0)
            {
                if (oPermisos.TienePermisosSP("sp_u_acto_administrativo"))
                    oBasic.EnableButton(true, btnActosAdmEdit);
                if (oPermisos.TienePermisosSP("sp_d_acto_administrativo"))
                    oBasic.EnableButton(true, btnActosAdmDel);
            }
        }

        protected void ddlb_id_estado_predio_declarado_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool b = false;
            try
            {
                int i = Convert.ToInt16(ddlb_id_estado_predio_declarado.SelectedItem.Value);
                if (i == 46 || i == 49 || i == 50)
                    b = true;
                else
                {
                    txt_suspension_meses.Text = "";
                    txt_suspension_dias.Text = "";
                }
            }
            catch { }
            rfv_suspension_meses.Enabled = b;
            rfv_suspension_dias.Enabled = b;
            rv_suspension_meses.Enabled = b;
            rv_suspension_dias.Enabled = b;
            divActosSuspension.Visible = b;
            oVar.prHayCambiosActosAdm = true;
            ddlb_id_estado_predio_declarado.Focus();
        }

        protected void txt_suspension_meses_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime date = Convert.ToDateTime(txt_fecha_ejecutoria_acto.Text);
                date = date.AddMonths(Convert.ToInt32(txt_suspension_meses.Text));
                date = date.AddDays(Convert.ToInt32(txt_suspension_dias.Text));
                txt_fecha_vencimiento_acto.Text = date.ToString("yyyy-MM-dd");
                oVar.prHayCambiosActosAdm = true;
            }
            catch { }
        }

        #endregion

        private void fFichaPredialGV(string pcod_predio_declarado, string idarchivo, string cod_usu_responsable)
        {
            ucFileUpload.FilePath = Path.Combine(oVar.prPathDeclaratorias.ToString(), "Fichas");
            ucFileUpload.Prefix = tipoArchivo.PDF_PD;
            ucFileUpload.ReferenceID = Convert.ToInt32(pcod_predio_declarado);
            ucFileUpload.ArchivoID = idarchivo != "" ? Convert.ToInt32("0" + (idarchivo ?? "0").ToString()) : 0;
            ucFileUpload.SectionPermission = cnsSection.PRED_DECL_DOCUMENTOS_EXTRAS;
            ucFileUpload.CodUsuario = cod_usu_responsable;
            ucFileUpload.ValidateResponsible = true;
        }
        private void LoadInteresados(string pcod_predio_declarado, string cod_usu_responsable)
        {
            ucInteresado.ReferenceID = Convert.ToInt32(pcod_predio_declarado);
            ucInteresado.ResponsibleUserCode = cod_usu_responsable;
            ucInteresado.LoadGrid();
        }
        private void LoadCartas(string pcod_predio_declarado, string cod_usu_responsable)
        {
            ucCarta.ReferenceID = Convert.ToInt32(pcod_predio_declarado);
            ucCarta.ResponsibleUserCode = cod_usu_responsable;
            ucCarta.LoadGrid();
        }

        #endregion

        #region--------------------------------------------------------------------GENERALES
        private void fActivarVistaGrid(MultiView mvSource, LinkButton btnOkSource, LinkButton btnCancelSource)
        {
            mvSource.ActiveViewIndex = 0;
            fBotonAccionFinalEstado(btnOkSource, btnCancelSource, false);
            upPredios.Update();
        }

        private void fBotonAccionFinalEstado(LinkButton btnOkSource, LinkButton btnCancelSource, bool bVer)
        {
            if (bVer)
            {
                btnOkSource.Style.Add("visibility", "visible");
                btnCancelSource.Style.Add("visibility", "visible");
            }
            else
            {
                btnOkSource.Style.Add("visibility", "hidden");
                btnCancelSource.Style.Add("visibility", "hidden");
            }
        }

        private void fBotonesAccionesOcultar()
        {
            fBotonAccionFinalEstado(btnPrediosAccionFinal, btnPrediosCancelar, false);
            fBotonAccionFinalEstado(btnPrediosDecAccionFinal, btnPrediosDecCancelar, false);
            fBotonAccionFinalEstado(btnDocumentosAccionFinal, btnDocumentosCancelar, false);
            fBotonAccionFinalEstado(btnPrestamosAccionFinal, btnPrestamosCancelar, false);
            fBotonAccionFinalEstado(btnPrediosPropietariosAccionFinal, btnPrediosPropietariosCancelar, false);
            fBotonAccionFinalEstado(btnPropietariosAccionFinal, btnPropietariosCancelar, false);
            fBotonAccionFinalEstado(btnVisitasAccionFinal, btnVisitasCancelar, false);
            fBotonAccionFinalEstado(btnLicenciasAccionFinal, btnLicenciasCancelar, false);
            fBotonAccionFinalEstado(btnConceptosAccionFinal, btnConceptosCancelar, false);
            fBotonAccionFinalEstado(btnActosAdmAccionFinal, btnActosAdmCancelar, false);
        }

        private void fBotonesNavegacionEstado(Panel panelSource, bool bVer)
        {
            if (bVer)
                panelSource.Style.Add("visibility", "visible");
            else
                panelSource.Style.Add("visibility", "hidden");
        }

        private void fCuentaRegistros(Label lblCuenta, GridView gvDatos, DataSet dsFiltro, LinkButton Primero, LinkButton Anterior, LinkButton Siguiente, LinkButton Ultimo, UpdatePanel upNavegacion, int RealIndex)
        {
            if (dsFiltro == null)
            {
                lblCuenta.Text = string.Format(_MSGCONTADORREGISTROS, "0", "0");
                Primero.Enabled = false;
                Anterior.Enabled = false;
                Siguiente.Enabled = false;
                Ultimo.Enabled = false;
            }
            else
            {
                if (dsFiltro.Tables[0].Rows.Count > 0)
                {
                    if (bPageChanged && gvDatos.AllowPaging)
                    {
                        bPageChanged = false;
                        lblCuenta.Text = string.Format(_MSGCONTADORREGISTROS, ((gvDatos.PageSize * gvDatos.PageIndex) + gvDatos.PageIndex).ToString(), dsFiltro.Tables[0].Rows.Count.ToString());
                    }
                    else
                        lblCuenta.Text = string.Format(_MSGCONTADORREGISTROS, (RealIndex + 1).ToString(), dsFiltro.Tables[0].Rows.Count.ToString());
                }
                else
                    lblCuenta.Text = string.Format(_MSGCONTADORREGISTROS, "0", "0");

                if (RealIndex + 1 >= dsFiltro.Tables[0].Rows.Count)
                {
                    Primero.Enabled = true;
                    Anterior.Enabled = true;
                    Siguiente.Enabled = false;
                    Ultimo.Enabled = false;
                }
                else if (RealIndex - 1 < 0)
                {
                    Primero.Enabled = false;
                    Anterior.Enabled = false;
                    Siguiente.Enabled = true;
                    Ultimo.Enabled = true;
                }
                else
                {
                    Primero.Enabled = true;
                    Anterior.Enabled = true;
                    Siguiente.Enabled = true;
                    Ultimo.Enabled = true;
                }
            }
            oBasic.StyleButton(Primero);
            oBasic.StyleButton(Anterior);
            oBasic.StyleButton(Siguiente);
            oBasic.StyleButton(Ultimo);
            upNavegacion.Update();
        }

        private void fLoadDropDowns()
        {
            ////*****IDENTIDAD FILTRADA

            ddlb_id_area_solicita_prestamo.DataSource = oIdentidades.sp_s_identidad_id_categoria("14");
            ddlb_id_area_solicita_prestamo.DataTextField = "nombre_identidad";
            ddlb_id_area_solicita_prestamo.DataValueField = "id_identidad";
            ddlb_id_area_solicita_prestamo.DataBind();

            ddlb_id_area_solicita_prestamo2.DataSource = oIdentidades.sp_s_identidad_id_categoria("14");
            ddlb_id_area_solicita_prestamo2.DataTextField = "nombre_identidad";
            ddlb_id_area_solicita_prestamo2.DataValueField = "id_identidad";
            ddlb_id_area_solicita_prestamo2.DataBind();

            ddlb_id_tipo_prestamo.DataSource = oIdentidades.sp_s_identidad_id_categoria("41");
            ddlb_id_tipo_prestamo.DataTextField = "nombre_identidad";
            ddlb_id_tipo_prestamo.DataValueField = "id_identidad";
            ddlb_id_tipo_prestamo.DataBind();

            ddlb_id_estado_predio_declarado.DataSource = oIdentidades.sp_s_identidad_id_categoria("9");
            ddlb_id_estado_predio_declarado.DataTextField = "nombre_identidad";
            ddlb_id_estado_predio_declarado.DataValueField = "id_identidad";
            ddlb_id_estado_predio_declarado.DataBind();

            ddlb_id_estado_concepto.DataSource = oIdentidades.sp_s_identidad_id_categoria_op("10", "0");
            ddlb_id_estado_concepto.DataTextField = "nombre_identidad";
            ddlb_id_estado_concepto.DataValueField = "id_identidad";
            ddlb_id_estado_concepto.DataBind();

            ddlb_id_estado_construccion_existente_lote.DataSource = oIdentidades.sp_s_identidad_id_categoria("27");
            ddlb_id_estado_construccion_existente_lote.DataTextField = "nombre_identidad";
            ddlb_id_estado_construccion_existente_lote.DataValueField = "id_identidad";
            ddlb_id_estado_construccion_existente_lote.DataBind();

            ddlb_id_estado_vias_internas.DataSource = oIdentidades.sp_s_identidad_id_categoria("27");
            ddlb_id_estado_vias_internas.DataTextField = "nombre_identidad";
            ddlb_id_estado_vias_internas.DataValueField = "id_identidad";
            ddlb_id_estado_vias_internas.DataBind();

            ddlb_id_estado_vias_perimetrales.DataSource = oIdentidades.sp_s_identidad_id_categoria("27");
            ddlb_id_estado_vias_perimetrales.DataTextField = "nombre_identidad";
            ddlb_id_estado_vias_perimetrales.DataValueField = "id_identidad";
            ddlb_id_estado_vias_perimetrales.DataBind();

            ddlb_id_estado_visita.DataSource = oIdentidades.sp_s_identidad_id_categoria("12");
            ddlb_id_estado_visita.DataTextField = "nombre_identidad";
            ddlb_id_estado_visita.DataValueField = "id_identidad";
            ddlb_id_estado_visita.DataBind();

            ddlb_id_fuente_informacion.DataSource = oIdentidades.sp_s_identidad_id_categoria("15");
            ddlb_id_fuente_informacion.DataTextField = "nombre_identidad";
            ddlb_id_fuente_informacion.DataValueField = "id_identidad";
            ddlb_id_fuente_informacion.DataBind();

            ddlb_id_obligacion_VIS.DataSource = oIdentidades.sp_s_identidad_id_categoria("17");
            ddlb_id_obligacion_VIS.DataTextField = "nombre_identidad";
            ddlb_id_obligacion_VIS.DataValueField = "id_identidad";
            ddlb_id_obligacion_VIS.DataBind();

            ddlb_id_obligacion_VIP.DataSource = oIdentidades.sp_s_identidad_id_categoria("18");
            ddlb_id_obligacion_VIP.DataTextField = "nombre_identidad";
            ddlb_id_obligacion_VIP.DataValueField = "id_identidad";
            ddlb_id_obligacion_VIP.DataBind();

            ddlb_id_origen_certificado_catastral.DataSource = oIdentidades.sp_s_identidad_id_categoria("38");
            ddlb_id_origen_certificado_catastral.DataTextField = "nombre_identidad";
            ddlb_id_origen_certificado_catastral.DataValueField = "id_identidad";
            ddlb_id_origen_certificado_catastral.DataBind();

            ddlb_id_origen_matricula.DataSource = oIdentidades.sp_s_identidad_id_categoria("39");
            ddlb_id_origen_matricula.DataTextField = "nombre_identidad";
            ddlb_id_origen_matricula.DataValueField = "id_identidad";
            ddlb_id_origen_matricula.DataBind();

            ddlb_id_pendiente_topografia.DataSource = oIdentidades.sp_s_identidad_id_categoria("28");
            ddlb_id_pendiente_topografia.DataTextField = "nombre_identidad";
            ddlb_id_pendiente_topografia.DataValueField = "id_identidad";
            ddlb_id_pendiente_topografia.DataBind();

            ddlb_id_tipo_acto.DataSource = oIdentidades.sp_s_identidad_id_categoria("6");
            ddlb_id_tipo_acto.DataTextField = "nombre_identidad";
            ddlb_id_tipo_acto.DataValueField = "id_identidad";
            ddlb_id_tipo_acto.DataBind();

            ddlb_id_causal_acto.DataSource = oIdentidades.sp_s_identidad_id_categoria("49");
            ddlb_id_causal_acto.DataTextField = "nombre_identidad";
            ddlb_id_causal_acto.DataValueField = "id_identidad";
            ddlb_id_causal_acto.DataBind();

            ddlb_id_tipo_concepto.DataSource = oIdentidades.sp_s_identidad_id_categoria("8");
            ddlb_id_tipo_concepto.DataTextField = "nombre_identidad";
            ddlb_id_tipo_concepto.DataValueField = "id_identidad";
            ddlb_id_tipo_concepto.DataBind();

            ddlb_id_tipo_doc_propietario.DataSource = oIdentidades.sp_s_identidad_id_categoria("4");
            ddlb_id_tipo_doc_propietario.DataTextField = "nombre_identidad";
            ddlb_id_tipo_doc_propietario.DataValueField = "id_identidad";
            ddlb_id_tipo_doc_propietario.DataBind();

            ddlb_id_tipo_doc_representante.DataSource = oIdentidades.sp_s_identidad_id_categoria("4");
            ddlb_id_tipo_doc_representante.DataTextField = "nombre_identidad";
            ddlb_id_tipo_doc_representante.DataValueField = "id_identidad";
            ddlb_id_tipo_doc_representante.DataBind();

            ddlb_id_tipo_licencia.DataSource = oIdentidades.sp_s_identidad_id_categoria("16");
            ddlb_id_tipo_licencia.DataTextField = "nombre_identidad";
            ddlb_id_tipo_licencia.DataValueField = "id_identidad";
            ddlb_id_tipo_licencia.DataBind();

            ddlb_id_tipo_visita.DataSource = oIdentidades.sp_s_identidad_id_categoria("11");
            ddlb_id_tipo_visita.DataTextField = "nombre_identidad";
            ddlb_id_tipo_visita.DataValueField = "id_identidad";
            ddlb_id_tipo_visita.DataBind();

            ddlb_id_ocupacion_visita.DataSource = oIdentidades.sp_s_identidad_id_categoria("36");
            ddlb_id_ocupacion_visita.DataTextField = "nombre_identidad";
            ddlb_id_ocupacion_visita.DataValueField = "id_identidad";
            ddlb_id_ocupacion_visita.DataBind();

            ddlb_id_uso_lote.DataSource = oIdentidades.sp_s_identidad_id_categoria("29");
            ddlb_id_uso_lote.DataTextField = "nombre_identidad";
            ddlb_id_uso_lote.DataValueField = "id_identidad";
            ddlb_id_uso_lote.DataBind();

            ddlb_id_pendiente_lote.DataSource = oIdentidades.sp_s_identidad_id_categoria("37");
            ddlb_id_pendiente_lote.DataTextField = "nombre_identidad";
            ddlb_id_pendiente_lote.DataValueField = "id_identidad";
            ddlb_id_pendiente_lote.DataBind();

            ddlb_id_pendiente_ladera.DataSource = oIdentidades.sp_s_identidad_id_categoria("37");
            ddlb_id_pendiente_ladera.DataTextField = "nombre_identidad";
            ddlb_id_pendiente_ladera.DataValueField = "id_identidad";
            ddlb_id_pendiente_ladera.DataBind();

            ddlb_id_estado_predio_obs.DataSource = oIdentidades.sp_s_identidad_id_categoria("42");
            ddlb_id_estado_predio_obs.DataTextField = "nombre_identidad";
            ddlb_id_estado_predio_obs.DataValueField = "id_identidad";
            ddlb_id_estado_predio_obs.DataBind();

            oBasic.fLoadUsuariosFiltro(ddlb_cod_usu_entrega_prestamo, 4);
            oBasic.fLoadUsuariosFiltro(ddlb_cod_usu_entrega_prestamo2, 4);
            oBasic.fLoadUsuariosFiltro(ddlb_cod_usu_recibe_prestamo, 4);
            oBasic.fLoadUsuariosFiltro(ddlb_cod_usu_recibe_prestamo2, 4);

            ddlb_cod_usu_concepto.DataSource = oUsuarios.sp_s_usuarios_filtro(7, -1);
            ddlb_cod_usu_concepto.DataTextField = "nombre_completo";
            ddlb_cod_usu_concepto.DataValueField = "cod_usuario";
            ddlb_cod_usu_concepto.DataBind();

            ddlb_cod_usu_concepto_revisa.DataSource = oUsuarios.sp_s_usuarios_filtro(8, -1);
            ddlb_cod_usu_concepto_revisa.DataTextField = "nombre_completo";
            ddlb_cod_usu_concepto_revisa.DataValueField = "cod_usuario";
            ddlb_cod_usu_concepto_revisa.DataBind();

            ddlb_cod_usu_concepto_aprueba.DataSource = oUsuarios.sp_s_usuarios_filtro(9, -1);
            ddlb_cod_usu_concepto_aprueba.DataTextField = "nombre_completo";
            ddlb_cod_usu_concepto_aprueba.DataValueField = "cod_usuario";
            ddlb_cod_usu_concepto_aprueba.DataBind();

            ddlb_cod_usu_visita.DataSource = oUsuarios.sp_s_usuarios_filtro(3, 0);
            ddlb_cod_usu_visita.DataTextField = "nombre_completo";
            ddlb_cod_usu_visita.DataValueField = "cod_usuario";
            ddlb_cod_usu_visita.DataBind();

            oBasic.fLoadUsuariosFiltro(ddlb_cod_usu, 4);

            ddlb_cod_usu__observacion.DataSource = oUsuarios.sp_s_usuarios_filtro(4, -1);
            ddlb_cod_usu__observacion.DataTextField = "nombre_completo";
            ddlb_cod_usu__observacion.DataValueField = "cod_usuario";
            ddlb_cod_usu__observacion.DataBind();

            oBasic.fLoadUsuariosFiltro(ddlb_cod_usu_responsable, 2);
        }

        private void fMensajeCRUD(string Mensaje, int NivelMensaje, string divMsg, UpdatePanel upControl)
        {
            //Por el UP
            ScriptManager.RegisterStartupScript(upControl, upControl.GetType(), Guid.NewGuid().ToString(), "MensajeCRUD('" + Mensaje + "'," + NivelMensaje + ",'" + divMsg + "');", true);
        }

        private bool fValidarFolios()
        {
            bool FoliosValidos = true;
            string strResultado = oDocumentos.sp_v_documento(
              gvPrediosDec.SelectedDataKey.Values["cod_predio_declarado"].ToString(),
              oBasic.fInt2(txt_au_documento),
              oBasic.fInt2(txt_folio_inicial_documento),
              oBasic.fInt2(txt_folios_documento));
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_ERR_DATOS)
            {
                FoliosValidos = false;
                lblMsgErr.Text = "Los datos ingresados de folio inicial y número de folios se cruzan con datos ya existentes";
                mpeFolios.Show();
            }
            return FoliosValidos;
        }

        private void fCuentaRegistros(GridView gv, int Index)
        {
            string modulo = gv.ID.Substring(2);
            UpdatePanel up = oUtil.FindControlRecursive(this.Master, "up" + modulo + "Foot") as UpdatePanel;
            Label lbl = oUtil.FindControlRecursive(this.Master, "lbl" + modulo + "Cuenta") as Label;
            LinkButton lb1 = oUtil.FindControlRecursive(this.Master, "btnFirst" + modulo) as LinkButton;
            LinkButton lb2 = oUtil.FindControlRecursive(this.Master, "btnBack" + modulo) as LinkButton;
            LinkButton lb3 = oUtil.FindControlRecursive(this.Master, "btnNext" + modulo) as LinkButton;
            LinkButton lb4 = oUtil.FindControlRecursive(this.Master, "btnLast" + modulo) as LinkButton;

            if (gv.Rows.Count > 0)
                lbl.Text = string.Format(_MSGCONTADORREGISTROS, (Index + 1).ToString(), gv.Rows.Count.ToString());
            else
            {
                lbl.Text = string.Format(_MSGCONTADORREGISTROS, "0", "0");
                lb1.Enabled = false;
                lb2.Enabled = false;
                lb3.Enabled = false;
                lb4.Enabled = false;
            }

            if (Index + 1 >= gv.Rows.Count)
            {
                lb1.Enabled = true;
                lb2.Enabled = true;
                lb3.Enabled = false;
                lb4.Enabled = false;
            }
            else if (Index - 1 < 0)
            {
                lb1.Enabled = false;
                lb2.Enabled = false;
                lb3.Enabled = true;
                lb4.Enabled = true;
            }
            else
            {
                lb1.Enabled = true;
                lb2.Enabled = true;
                lb3.Enabled = true;
                lb4.Enabled = true;
            }
            oBasic.StyleButton(lb1);
            oBasic.StyleButton(lb2);
            oBasic.StyleButton(lb3);
            oBasic.StyleButton(lb4);
            up.Update();
        }

        private void gv_SelectedIndexChanged(GridView gv)
        {
            string modulo = gv.ID.Substring(2);
            ViewState["RealIndex" + modulo] = ((gv.PageIndex * gv.PageSize) + gv.SelectedIndex).ToString();
            if (gv.Rows.Count > 0)
                fCuentaRegistros(gv, gv.SelectedIndex);
        }

        #region---CAMBIOS
        protected void Predios_TextChanged(object sender, EventArgs e)
        {
            HayCambiosPredios = true;
        }

        protected void PrediosDec_TextChanged(object sender, EventArgs e)
        {
            HayCambiosPrediosDec = true;
        }

        protected void Documentos_TextChanged(object sender, EventArgs e)
        {
            HayCambiosDocumentos = true;
        }

    protected void Prestamos_TextChanged(object sender, EventArgs e)
    {
    }

        protected void Propietarios_TextChanged(object sender, EventArgs e)
        {
            HayCambiosPropietarios = true;
        }

        protected void PrediosPropietarios_TextChanged(object sender, EventArgs e)
        {
            HayCambiosPrediosPropietarios = true;
        }

        protected void Visitas_TextChanged(object sender, EventArgs e)
        {
            HayCambiosVisitas = true;
        }

        protected void Observaciones_TextChanged(object sender, EventArgs e)
        {
            HayCambiosObservaciones = true;
        }

        protected void Licencias_TextChanged(object sender, EventArgs e)
        {
            oVar.prHayCambiosLicencias = true;
        }

        protected void Conceptos_TextChanged(object sender, EventArgs e)
        {
            oVar.prHayCambiosConceptos = true;
        }

        protected void ActosAdm_TextChanged(object sender, EventArgs e)
        {
            oVar.prHayCambiosActosAdm = true;
        }

        #endregion

        #endregion

        protected void ucFileUpload_UserControlException(object sender, Exception e)
        {

        }

        protected void ucFileUpload_ViewDoc(object sender)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "verDoc();", true);
        }

        protected void btnCartaExcel_Click(object sender, EventArgs e)
        {
            ucCarta.LoadReport();
        }
    }
}