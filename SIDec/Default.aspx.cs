using GLOBAL.VAR;
using System;

namespace SIDec
{
    public partial class Default : System.Web.UI.Page
  {
    clGlobalVar oVar = new clGlobalVar();

        protected void Page_Load(object sender, EventArgs e)
        {
            desc_predios.InnerText = oVar.prDescPredios.ToString();
            desc_planesp.InnerText = oVar.prDescPlanesP.ToString();
            desc_proyectos.InnerText = oVar.prDescProyectos.ToString();
            desc_geosidec.InnerText = oVar.prDescGeosidec.ToString();
        }
  }
}