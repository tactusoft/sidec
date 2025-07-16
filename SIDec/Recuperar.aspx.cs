using GLOBAL.UTIL;
using System;

namespace SIDec.Account
{
    public partial class Recuperar : System.Web.UI.Page
    {
        clUtil oUtil = new clUtil();

        protected void Page_Load(object sender, EventArgs e)
        {
            Regresar.NavigateUrl = "Login";
        }

        protected void btnRecuperar_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                string NewPW = oUtil.CreaPW();
            }
        }
    }
}