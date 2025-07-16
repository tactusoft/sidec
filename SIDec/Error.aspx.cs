using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIDec
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //btnPreviousPage.NavigateUrl = "Login";
        }

        //static string prevPage = String.Empty;

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!IsPostBack)
        //    {
        //        prevPage = Request.UrlReferrer.ToString();
        //    }
        //}

        //protected void btnPreviousPage_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect(prevPage);
        //}

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    btnPreviousPage.Attributes.Add("onClick", "javascript:history.back(); return false;");
        //}

        //protected void btnPreviousPage_Click(object sender, EventArgs e)
        //{

        //}
    }
}