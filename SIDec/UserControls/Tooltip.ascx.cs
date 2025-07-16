using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIDec.UserControls
{
    public partial class Tooltip : System.Web.UI.UserControl
    {

        public string width
        {
            set
            {
                sToolTip.Style.Remove("width");
                sToolTip.Style.Add("width", value);
            }
        }

        public string ErrorMessage
        {
            set
            {
                lblErrorMessage.Text = value;
            }
        }

        public string ToolTip
        {
            set
            {
                lblToolTip.Text = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}