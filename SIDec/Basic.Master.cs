using System;
using System.Web.UI;
using GLOBAL.LOG;

namespace SIDec
{  
    public partial class Basic : System.Web.UI.MasterPage
    {
        clLog oLog = new clLog();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void Mensaje(string Mensaje, int NivelMensaje)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "MensajeWeb('" + Mensaje + "'," + NivelMensaje + ");", true);
        }
    }
}