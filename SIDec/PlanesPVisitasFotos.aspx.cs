using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using GLOBAL.VAR;
using GLOBAL.UTIL;
using GLOBAL.LOG;
using GLOBAL.DAL;
using System.Drawing;
using GLOBAL.PERMISOS;

namespace SIDec
{
  public partial class PlanesPVisitasFotos : System.Web.UI.Page
  {
    IDENTIDADES_DAL oIdentidades = new IDENTIDADES_DAL();
    clGlobalVar oVar = new clGlobalVar();
    clFile oFile = new clFile();
    clLog oLog = new clLog();
    clUtil oUtil = new clUtil();
    clPermisos oPermisos = new clPermisos();
    clBasic oBasic = new clBasic();

    private const int _numFotos = 3; // cantidad de fotos

    private const string _SOURCEPAGE = "PlanesPVisitasFotos";
    string[] ListaFotos;
    string[] infoProp = new string[2];

    LinkButton lbControl;
    System.Web.UI.WebControls.Image imgSource;
    System.Web.UI.WebControls.Image imgDestino;
    Label lblObservacion;

    #region---PAGE
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        ViewState["Indice"] = "";
        //if (Session["PlanesPVisitasVerFotos"] == null || string.IsNullOrEmpty(Session["PlanesPVisitasVerFotos"].ToString()) || Session["PlanesPVisitasVerFotos"].ToString() != "PV_VF")
        //{
        //  mvFotos.ActiveViewIndex = 0;
        //  Page.ClientScript.RegisterStartupScript(this.GetType(), "Cerrar", "window.close();", true);
        //}
        //else
          fLoadFotos();
      }
    }

    private void fLoadFotos()
    {
      fValidarSP();
      divFotoEdicion.Visible = false;
      FileUpload1.Visible = false;
      txtDescripcionFoto.Text = "";
      ListaFotos = oFile.fGetArchivoPlanesPVisitasFotos(oVar.prPlanesPVisitasAu.ToString());
      string nombreFoto = "";
      if (ListaFotos != null && ListaFotos.Length > 0)
      {
        foreach (string Foto in ListaFotos)
        {
          if (Foto.LastIndexOf("_") > 0)
          {
            int indexFoto = 0;
            nombreFoto = Path.GetFileName(Foto).Replace(Path.GetExtension(Foto), "");
            ViewState["Indice"] = nombreFoto.Substring(nombreFoto.LastIndexOf('_') + 1);
            indexFoto = Convert.ToInt16(ViewState["Indice"].ToString());
            imgDestino = (System.Web.UI.WebControls.Image)divFotoContainer.FindControl("Image" + indexFoto.ToString());
            lblObservacion = (System.Web.UI.WebControls.Label)divFotoContainer.FindControl("lblObs" + indexFoto.ToString());
            if (imgDestino != null)
            {
              imgDestino.ImageUrl = "~/Handler1.ashx?fileFoto=" + Foto;
              infoProp = oFile.fGetFileProp(Foto);
              if (infoProp != null)
              {
                lblObservacion.Text = infoProp[1].ToString();
              }
            }
          }
        }
      }
      fSetStyleControls();
    }
    #endregion

    #region---fotos
    protected void btnFotoAccion_Click(object sender, EventArgs e)
    {
      LinkButton btnSource = (LinkButton)sender;
      string sAccion = btnSource.CommandName.Substring(0, 1); ;
      ViewState["Indice"] = btnSource.CommandName.Substring(1, 1);
      divFotoEdicion.Visible = false;
      FileUpload1.Visible = false;
      if (sAccion == "A")
      {
        divFotoEdicion.Visible = true;
        FileUpload1.Visible = true;
        string imagCtrlName = "Image" + ViewState["Indice"].ToString();
      }
      else if (sAccion == "E")
      {
        divFotoEdicion.Visible = true;
        infoProp = oFile.fGetFileProp(fPathFile(2, ViewState["Indice"].ToString()));
        if (infoProp != null)
        {
          txtDescripcionFoto.Text = infoProp[1].ToString().Replace("\0", "");
          txtDescripcionFoto.Focus();
        }
      }
      else if (sAccion == "D")
      {
        imgSource = (System.Web.UI.WebControls.Image)divFotoContainer.FindControl("Image" + ViewState["Indice"].ToString());
        if (!string.IsNullOrEmpty(imgSource.ImageUrl))
          mpeEliminar.Show();
      }
    }

    protected void lbCancelar_Click(object sender, EventArgs e)
    {
      divFotoEdicion.Visible = false;
      FileUpload1.Visible = false;
    }

    protected void lbSubir_Click(object sender, EventArgs e)
    {
      string pathFile = "";
      string pathFileTmp = "";
      bool SetFilePropOk = false;
      if (FileUpload1.Visible)    //Subir foto
      {
        if (FileUpload1.HasFile)
        {
          try
          {
            Bitmap bitmap = new Bitmap(FileUpload1.FileContent);
          }
          catch
          {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Solo se admiten archivos en formato JPG o PNG')", true);
            return;
          }
          Bitmap fotoSeleccionada = new Bitmap(FileUpload1.FileContent);
          pathFile = fPathFile(1, ViewState["Indice"].ToString());
          pathFileTmp = oVar.prTmpImg.ToString() + "\\" + oVar.prPlanesPVisitasAu.ToString() + "_" + ViewState["Indice"].ToString() + ".tmp";

          bool Resultado = oFile.fVerificarPath(oVar.prPathPlanesPVisitas.ToString() + "\\" + oVar.prPlanesPVisitasAu.ToString());
          if (Resultado)
          {
            oFile.fBorrarFile(pathFile);
            fotoSeleccionada = new Bitmap(FileUpload1.FileContent);
            SetFilePropOk = oFile.fSetFileProp(fotoSeleccionada, pathFileTmp, pathFile, "", txtDescripcionFoto.Text);
            if (SetFilePropOk)
              fLoadFotos();
          }
          else
            oLog.RegistrarLogError("Error subiendo Foto " + System.IO.Path.GetExtension(FileUpload1.FileName) + ":::" + FileUpload1.PostedFile.ContentLength, _SOURCEPAGE, "lbSubir_Click");
        }
      }
      else    //Editar info de la foto
      {
        pathFile = fPathFile(2, ViewState["Indice"].ToString());
        pathFileTmp = pathFile.Substring(0, pathFile.Length - Path.GetExtension(pathFile).Length) + ".tmp";
        if (oFile.fCopiarFile(pathFile, pathFileTmp))
        {
          oFile.fBorrarFile(pathFile);
          if (oFile.fSetFileProp1(pathFile, pathFileTmp, "", txtDescripcionFoto.Text))
          {
            oFile.fBorrarFile(pathFileTmp);
            lblObservacion = (System.Web.UI.WebControls.Label)divFotoContainer.FindControl("lblObs" + ViewState["Indice"].ToString());
            lblObservacion.Text = txtDescripcionFoto.Text;
            txtDescripcionFoto.Text = "";
            divFotoEdicion.Visible = false;
            FileUpload1.Visible = false;
          }
        }
      }
      fSetStyleControls();
    }

    protected void btnSiEliminar_Click(object sender, EventArgs e)
    {
      lblObservacion = (System.Web.UI.WebControls.Label)divFotoContainer.FindControl("lblObs" + ViewState["Indice"].ToString());
      string pathFile = fPathFile(2, ViewState["Indice"].ToString());
      if (oFile.fBorrarFile(pathFile))
      {
        lblObservacion.Text = "";
        imgSource.ImageUrl = "";
        fSetStyleControls();
      }
    }
    #endregion

    #region---varios
    private string fPathFile(int iOpcion, string sIndex)
    {
      string pathFile = oVar.prPathPlanesPVisitas.ToString() + "\\" + oVar.prPlanesPVisitasAu.ToString() + "\\" + oVar.prPlanesPVisitasAu.ToString() + "_" + sIndex;

      if (iOpcion == 1) //cargar
        pathFile = pathFile + Path.GetExtension(this.FileUpload1.FileName).ToLower();
      else if (iOpcion == 2) //eliminar - editar
      {
        imgSource = (System.Web.UI.WebControls.Image)divFotoContainer.FindControl("Image" + ViewState["Indice"]);
        pathFile = pathFile + Path.GetExtension(imgSource.ImageUrl);
      }
      return pathFile;
    }

    private void fSetStyleControls()
    {
      bool b = oPermisos.TienePermisosSP("sp_u_planp_visita");
      if (!b)
        return;

      for (int i = 1; i <= _numFotos; i++)
      {
        imgSource = (System.Web.UI.WebControls.Image)divFotoContainer.FindControl("Image" + i);
        if (string.IsNullOrEmpty(imgSource.ImageUrl))
        {
          lbControl = oUtil.FindControlRecursive(this.Master, "btnFotoAdd" + i) as LinkButton;
          lbControl.Enabled = true;
          oBasic.StyleButton(lbControl);
          lbControl = oUtil.FindControlRecursive(this.Master, "btnFotoDel" + i) as LinkButton;
          lbControl.Enabled = false;
          oBasic.StyleButton(lbControl);
          lbControl = oUtil.FindControlRecursive(this.Master, "btnFotoEdit" + i) as LinkButton;
          lbControl.Enabled = false;
          oBasic.StyleButton(lbControl);
        }
        else
        {
          lbControl = oUtil.FindControlRecursive(this.Master, "btnFotoAdd" + i) as LinkButton;
          lbControl.Enabled = false;
          oBasic.StyleButton(lbControl);
          lbControl = oUtil.FindControlRecursive(this.Master, "btnFotoDel" + i) as LinkButton;
          lbControl.Enabled = true;
          oBasic.StyleButton(lbControl);
          lbControl = oUtil.FindControlRecursive(this.Master, "btnFotoEdit" + i) as LinkButton;
          lbControl.Enabled = true;
          oBasic.StyleButton(lbControl);
        }
      }
    }

    private void fValidarSP()
    {
      bool b = oPermisos.TienePermisosSP("sp_u_planp_visita");
      for (int i = 1; i <= _numFotos; i++)
      {
        lbControl = oUtil.FindControlRecursive(this.Master, "btnFotoEdit" + i) as LinkButton;
        oBasic.EnableCtl(lbControl, b);
        lbControl = oUtil.FindControlRecursive(this.Master, "btnFotoDel" + i) as LinkButton;
        oBasic.EnableCtl(lbControl, b);
        lbControl = oUtil.FindControlRecursive(this.Master, "btnFotoAdd" + i) as LinkButton;
        oBasic.EnableCtl(lbControl, b);
      }
    }
    #endregion
  }
}