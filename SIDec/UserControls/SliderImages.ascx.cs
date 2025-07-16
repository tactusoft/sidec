using SIDec.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace SIDec.UserControls
{
    public partial class SliderImages : System.Web.UI.UserControl
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {           
            //RegisterScript();
        }

        private void RegisterScript()
        {
            string key = "Slider";
            StringBuilder scriptSlider = new StringBuilder();
            scriptSlider.Append(" <script type='text/javascript'> ");
            scriptSlider.Append("     jQuery(document).ready(function() { ");
            scriptSlider.Append("         $('.jcarousel').jcarousel({ scroll: 1, transitions: true });  ");
            scriptSlider.Append("         $('.jcarousel-control-prev').jcarouselControl({ target: '-=1'  }); ");
            scriptSlider.Append("         $('.jcarousel-control-next').jcarouselControl({ target: '+=1' }); ");
            scriptSlider.Append("      }); ");
            scriptSlider.Append(" </script> ");

            if (!Page.ClientScript.IsStartupScriptRegistered(key))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), key, scriptSlider.ToString(), false);
            }
        }

        public void SetSlides(List<Images> items)
        {
            this.rptCarousel.DataSource = items;
            this.rptCarousel.DataBind();
            RegisterScript();
        }
    }
}