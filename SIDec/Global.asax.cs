using GLOBAL.DAL;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Routing;

namespace SIDec
{
    public class Global : System.Web.HttpApplication
    {
        CONFIGPARAMETROS_DAL oParametros = new CONFIGPARAMETROS_DAL();

        clGlobalVar oVar = new clGlobalVar();
        clUtil oUtil = new clUtil();

        NameValueCollection oDestinoLog = ConfigurationManager.GetSection("GLOBAL/Log") as NameValueCollection;

        protected void Application_Start(object sender, EventArgs e)
        {
            // Código que se ejecuta al iniciar la aplicación      
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Cargar valores default de la aplicacion
            fLoadAppDefaults();

            ////Verificar la existencia de la carpeta para Log.
            fCheckLogFolder();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //Cargar valores default para la sesion
            fLoadSessionDefaults();
        }

        private void fCheckLogFolder()
        {
            string path = HttpContext.Current.Server.MapPath("~/" + oDestinoLog.Get("LogFolderName").ToString());
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private void fLoadSessionDefaults()
        {
            oVar.prPCInfo = Environment.MachineName + "/" + Environment.UserName;
            oUtil.setIPSesion();
            oVar.prUser = "";
        }

        private void fLoadAppDefaults()
        {
            string strLogPath = "~/" + oDestinoLog.Get("LogFolderName").ToString() + "/";

            oVar.prLogErrorDestino = Convert.ToInt16(oDestinoLog.Get("DestinoErrorLog").ToString());
            oVar.prLogError = HttpContext.Current.Server.MapPath(strLogPath + oDestinoLog.Get("LogErrorFileName").ToString());

            oVar.prLogInfoDestino = Convert.ToInt16(oDestinoLog.Get("DestinoInfoLog").ToString());
            oVar.prLogInfo = HttpContext.Current.Server.MapPath(strLogPath + oDestinoLog.Get("LogInfoFileName").ToString());

            oVar.prTmpImg = HttpContext.Current.Server.MapPath("imgtmp");
            oVar.prSidecFolder = HttpContext.Current.Server.MapPath("sidec_folder");

            DataSet oDS = new DataSet();

            oDS = oParametros.sp_s_config_parametros();
            oVar.prPathFormatosOrigen = oVar.prPathFormatosDestino = oDS.Tables[0].Rows[0][2].ToString();
            oVar.prPathComisionIntersectorial = oDS.Tables[0].Rows[1][2].ToString();
            oVar.prDocFormatoFO35 = oDS.Tables[0].Rows[2][2].ToString();
            oVar.prDocFormatoFO379 = oDS.Tables[0].Rows[3][2].ToString();
            oVar.prPathDocumentos = oDS.Tables[0].Rows[4][2].ToString();
            oVar.prDocAyuda = oDS.Tables[0].Rows[5][2].ToString();
            oVar.prPathPrediosVisitas = oDS.Tables[0].Rows[6][2].ToString();
            oVar.prPathPlantillaVisitas = oDS.Tables[0].Rows[7][2].ToString();
            oVar.prPathPlantillaConceptos = oDS.Tables[0].Rows[8][2].ToString();
            oVar.prPathPlantillaVisitasOld = oDS.Tables[0].Rows[12][2].ToString();
            oVar.prPathDeclaratorias = oDS.Tables[0].Rows[13][2].ToString();
            oVar.prPathPlanesP = oDS.Tables[0].Rows[14][2].ToString();
            oVar.prPathPlanesPDoc1 = oDS.Tables[0].Rows[15][2].ToString();
            oVar.prPathPlanesPVisitas = oDS.Tables[0].Rows[16][2].ToString();
            oVar.prPathPlanesPGeneral = oDS.Tables[0].Rows[17][2].ToString();
            oVar.prPathPlanesPlanos = oDS.Tables[0].Rows[18][2].ToString();
            oVar.prPathPlantillaCartaTerminos = oDS.Tables[0].Rows[19][2].ToString();
            oVar.prPathPlantillaGestionDocumental = oDS.Tables[0].Rows[20][2].ToString();
            oVar.prPathPlantillaEstadoPredios = oDS.Tables[0].Rows[21][2].ToString();
            oVar.prPathPlanesPActos = oDS.Tables[0].Rows[22][2].ToString();
            oVar.prPathPlanesPLicencias = oDS.Tables[0].Rows[23][2].ToString();
            oVar.prPathResoluciones = oDS.Tables[0].Rows[24][2].ToString();
            oVar.prPathPlanesPCesiones = oDS.Tables[0].Rows[25][2].ToString();
            oVar.prPathPlanesPIndicadores = oDS.Tables[0].Rows[26][2].ToString();

            oVar.prDescPredios = oDS.Tables[0].Rows[27][2].ToString();
            oVar.prDescPlanesP = oDS.Tables[0].Rows[28][2].ToString();
            oVar.prDescProyectos = oDS.Tables[0].Rows[29][2].ToString();
            oVar.prDescGeosidec = oDS.Tables[0].Rows[30][2].ToString();
            oVar.prPathDocumentosProyectos = oDS.Tables[0].Rows[31][2].ToString();
        }
    }
}