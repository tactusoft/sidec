using GLOBAL.DAL;
using GLOBAL.LOG;
using System;
using System.Data;
using System.Web;

namespace SIDec
{
    public class Handler1 : IHttpHandler
    {
        private readonly CONFIGPARAMETROS_DAL oParametros = new CONFIGPARAMETROS_DAL();
        private readonly clLog oLog = new clLog();
        private string _SOURCEPAGE = "Handler1";
        string filepath = "";
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                filepath = context.Request.QueryString["fileFoto"];

                DataTable oDT = oParametros.sp_s_config_parametros().Tables[0];
                int[] posiciones = { 1, 5, 7, 14, 15, 17, 19, 23, 24, 25, 32 }; //au_parametro de las configuraciones de rutas para solo incluir estas

                foreach (int i in posiciones)
                {
                    var basefilepath = oDT.Rows[i - 1][2].ToString();
                    if (filepath.StartsWith(basefilepath))
                    {
                        context.Response.ContentType = "image/png";
                        context.Response.WriteFile(filepath);
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                oLog.RegistrarLogError("Error mostrando archivo : " + e.Message + " : " + filepath, _SOURCEPAGE, "ProcessRequest");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}