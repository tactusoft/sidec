using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIDec
{
  public partial class Popup : System.Web.UI.MasterPage
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void EstadoBoton(bool Habilitado, string lbBotonID)
    {
      LinkButton lbContent = (LinkButton)fGetControl(ContentPlaceHolder1, lbBotonID);
      if (lbContent != null)
      {
        lbContent.Enabled = Habilitado;
        if (Habilitado)
        {
          lbContent.Style.Remove("color");
          lbContent.Style.Add("cursor", "pointer");
        }
        else
        {
          lbContent.Style.Add("color", "#aaa");
          lbContent.Style.Add("cursor", "not-allowed");
        }
      }
    }

    private static Control fGetControl(Control RootControl, string ControlIdBuscar)
    {
      if (RootControl.ID == ControlIdBuscar)
        return RootControl;
      foreach (Control Ctl in RootControl.Controls)
      {
        Control FoundCtl = fGetControl(Ctl, ControlIdBuscar);
        if (FoundCtl != null)
          return FoundCtl;
      }
      return null;
    }

  }
}