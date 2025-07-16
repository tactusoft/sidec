using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using GLOBAL.VAR;
using GLOBAL.UTIL;
using GLOBAL.LOG;
using GLOBAL.DAL;
using System.Drawing;
using System.Runtime.Serialization;

namespace SIDec
{
  public partial class Fotos : System.Web.UI.Page
  {
    IDENTIDADES_DAL oIdentidades = new IDENTIDADES_DAL();
    clGlobalVar oVar = new clGlobalVar();
    clFile oFile = new clFile();
    clLog oLog = new clLog();
    clUtil oUtil = new clUtil();
     
    private const string _SOURCEPAGE = "Fotos";
    string[] ListaFotos;

    string[] infoProp = new string[2];

    string estiloEditar = "";
    string estiloBorrar = "";

    private string _ESTILODESHABILITADO = "btnDeshabilitado";

    System.Web.UI.WebControls.Image imgSource;
    System.Web.UI.WebControls.Image imgDestino;

    System.Web.UI.WebControls.LinkButton btnEditar;
    System.Web.UI.WebControls.LinkButton btnBorrar;
    System.Web.UI.WebControls.Label lblTipoFoto;
    System.Web.UI.WebControls.Label lblObservacion;
    
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        ViewState["Indice"] = "";

        ddlbTipoFoto.DataSource = oIdentidades.sp_s_identidad_id_categoria("30");
        ddlbTipoFoto.DataTextField = "nombre_identidad";
        ddlbTipoFoto.DataValueField = "id_identidad";
        ddlbTipoFoto.DataBind();

        if (Session["sVerFotos"] == null || string.IsNullOrEmpty(Session["sVerFotos"].ToString()) || Session["sVerFotos"].ToString() != "VF")
        {
          mvFotos.ActiveViewIndex = 0;
          Page.ClientScript.RegisterStartupScript(this.GetType(), "Cerrar", "window.close();", true);
        }
        else
          fLoadFotos();
      }
    }

    protected void btnAsignarFoto_Click(object sender, EventArgs e)
    {
      LinkButton btnSource = (LinkButton)sender;
      ViewState["Indice"] = btnSource.CommandName.Substring(1, 1);

      string imagCtrlName = "Image" + ViewState["Indice"].ToString();

      if (fConfirmarReemplazar(((System.Web.UI.WebControls.Image)divFotoContainer.FindControl(imagCtrlName)).ImageUrl))
        mpeReemplazar.Show();
    }

    protected void btnSiBorrar_Click(object sender, EventArgs e)
    {
      imgSource = (System.Web.UI.WebControls.Image)divFotoContainer.FindControl("Image" + ViewState["Indice"].ToString());
      lblTipoFoto = (System.Web.UI.WebControls.Label)divFotoContainer.FindControl("lblTF" + ViewState["Indice"].ToString());
      lblObservacion = (System.Web.UI.WebControls.Label)divFotoContainer.FindControl("lblObs" + ViewState["Indice"].ToString());

      string fullPathDoc = oVar.prPathPrediosVisitas.ToString() + "//" + oVar.prVisitaAu.ToString() + "//" + oVar.prVisitaAu.ToString() + "_" + ViewState["Indice"].ToString() + Path.GetExtension(imgSource.ImageUrl);
      if (oFile.fBorrarFile(fullPathDoc))
      {
        lblTipoFoto.Text = "";
        lblObservacion.Text = "";
        imgSource.ImageUrl = "";
        fEstiloBotones(false);
      }
    }
    
    protected void btnEliminarFoto_Click(object sender, EventArgs e)
    {
      LinkButton btnSource = (LinkButton)sender;
      ViewState["Indice"] = btnSource.CommandName.Substring(1, 1);
            
      imgSource = (System.Web.UI.WebControls.Image)divFotoContainer.FindControl("Image" + ViewState["Indice"].ToString());

      if (fConfirmarBorrar(imgSource.ImageUrl))
        mpeBorrar.Show();
    }

    protected void btnSiReemplazar_Click(object sender, EventArgs e)
    {
      fVistaFotos(false, true);
    }

    protected void btnNoBorrar_Click(object sender, EventArgs e)
    {

    }

    protected void btnNoReemplazar_Click(object sender, EventArgs e)
    {
      
    }

    protected void lbCancelar_Click(object sender, EventArgs e)
    {
      fVistaFotos(true);
    }

    protected void lbSubir_Click(object sender, EventArgs e)
    {
      string fullPathDoc = "";
      string fullPathImgTmp = "";
      bool SetFilePropOk = false;
      if (FileUpload1.Visible)    //Subir foto
      {
        if (FileUpload1.HasFile)
        {
          Bitmap fotoSeleccionada = new Bitmap(FileUpload1.FileContent);
          fullPathDoc = oVar.prPathPrediosVisitas.ToString() + "//" + oVar.prVisitaAu.ToString() + "//" + oVar.prVisitaAu.ToString() + "_" + ViewState["Indice"].ToString() + Path.GetExtension(this.FileUpload1.FileName);
          fullPathImgTmp = oVar.prTmpImg.ToString() + "//" + oVar.prVisitaAu.ToString() + "_" + ViewState["Indice"].ToString() + ".tmp";

          bool Resultado = oFile.fVerificarPath(oVar.prPathPrediosVisitas.ToString() + "//" + oVar.prVisitaAu.ToString());
          if (Resultado)
          {
            oFile.fBorrarFile(fullPathDoc);

            // Crear bitmap en memoria del archivo seleccionado
            fotoSeleccionada = new Bitmap(FileUpload1.FileContent);
            SetFilePropOk = oFile.fSetFileProp(fotoSeleccionada, fullPathImgTmp, fullPathDoc, ddlbTipoFoto.SelectedItem.ToString(), txtDescripcionFoto.Text);            
          }
          else
            oLog.RegistrarLogError("Error subiendo Foto " + System.IO.Path.GetExtension(FileUpload1.FileName) + ":::" + FileUpload1.PostedFile.ContentLength, _SOURCEPAGE, "lbSubir_Click");
        }
      }
      else    //Editar info de la foto
      {
        imgSource = (System.Web.UI.WebControls.Image)divFotoContainer.FindControl("Image" + ViewState["Indice"].ToString());        
        fullPathDoc = oVar.prPathPrediosVisitas.ToString() + "//" + oVar.prVisitaAu.ToString() + "//" + oVar.prVisitaAu.ToString() + "_" + ViewState["Indice"].ToString() + Path.GetExtension(imgSource.ImageUrl);
        fullPathImgTmp = oVar.prTmpImg.ToString() + "//" + oVar.prVisitaAu.ToString() + "_" + ViewState["Indice"].ToString() + ".tmp";
        string fullPathImgTmpOrigen = oVar.prPathPrediosVisitas.ToString() + "//" + oVar.prVisitaAu.ToString() + "//" + oVar.prVisitaAu.ToString() + "_" + ViewState["Indice"].ToString() + ".tmp";

        if (oFile.fCopiarFile(fullPathDoc, fullPathImgTmpOrigen))
        {  
          Bitmap fotoSeleccionada = new Bitmap(fullPathImgTmpOrigen);
          oFile.fBorrarFile(fullPathDoc);
          if(oFile.fSetFileProp(fotoSeleccionada, fullPathImgTmp, fullPathDoc, ddlbTipoFoto.SelectedItem.ToString(), txtDescripcionFoto.Text))
          {
            SetFilePropOk = true;
            fotoSeleccionada.Dispose();
            oFile.fBorrarFile(fullPathImgTmpOrigen);
          }
        }
      }

      if (SetFilePropOk)
      {        
        fVistaFotos(true);
        fLoadFotos();

        ddlbTipoFoto.SelectedIndex = -1;
        txtDescripcionFoto.Text = "";
      }
    }
    
    private bool fConfirmarBorrar(string imgUrl)
    {
      if (string.IsNullOrEmpty(imgUrl))
        return false;
      return true;
    }
    
    private bool fConfirmarReemplazar(string imgUrl)
    {
      if (string.IsNullOrEmpty(imgUrl))
      {
        fVistaFotos(false, true);
        return false;
      }
      return true;
    }
    
    private string[] fGetListaFotos()
    {
      return oFile.fGetArchivoFotos(oVar.prVisitaAu.ToString());
    }
    
    private void fLoadFotos()
    {
        ListaFotos = fGetListaFotos();
        string nombreFoto = "";

        if (ListaFotos != null && ListaFotos.Length > 0)
        {
            foreach (string Foto in ListaFotos)
            {
                nombreFoto = Path.GetFileName(Foto).Replace(Path.GetExtension(Foto), "");
                ViewState["Indice"] = nombreFoto.Substring(nombreFoto.LastIndexOf("_") + 1);

                imgDestino = (System.Web.UI.WebControls.Image)divFotoContainer.FindControl("Image" + ViewState["Indice"].ToString());
                lblTipoFoto = (System.Web.UI.WebControls.Label)divFotoContainer.FindControl("lblTF" + ViewState["Indice"].ToString());
                lblObservacion = (System.Web.UI.WebControls.Label)divFotoContainer.FindControl("lblObs" + ViewState["Indice"].ToString());          

                if (imgDestino != null)
                {
                    fEstiloBotones(true);
                    btnEditar.CommandArgument = Foto;

                    imgDestino.ImageUrl = "~/Handler1.ashx?fileFoto=" + Foto;

                    infoProp = oFile.fGetFileProp(Foto);
                    if (infoProp != null)
                    {
                        lblTipoFoto.Text = infoProp[0].ToString();
                        lblObservacion.Text = infoProp[1].ToString();
                    }
                }
                else
                fEstiloBotones(false);
            }
        }
    }
   
    protected void btnEditar_Click(object sender, EventArgs e)
    {
      LinkButton btnSource = (LinkButton)sender;
      ViewState["Indice"] = btnSource.CommandName.Substring(1, 1);

      fVistaFotos(false);

      infoProp = oFile.fGetFileProp(btnSource.CommandArgument);
      if (infoProp != null)
      {
        ddlbTipoFoto.SelectedIndex = -1;

        if (!string.IsNullOrEmpty(infoProp[0].ToString()))
          ddlbTipoFoto.Items.FindByText(infoProp[0].ToString().Replace("\0", "")).Selected = true;

        txtDescripcionFoto.Text = infoProp[1].ToString().Replace("\0", "");
      }
    }

    private void fVistaFotos(bool VistaFotos, bool SubirArchivo = false)
    {
      //divFotoContainer.Visible = VistaFotos;
      divFotoContainer.Visible = true;
      divUF.Visible = !VistaFotos;
      FileUpload1.Visible = SubirArchivo;
    }

    private void fEstiloBotones(bool Habilitar)
    {
      btnEditar = (System.Web.UI.WebControls.LinkButton)divFotoContainer.FindControl("btnEditar" + ViewState["Indice"].ToString());
      btnBorrar = (System.Web.UI.WebControls.LinkButton)divFotoContainer.FindControl("btnEliminar" + ViewState["Indice"].ToString());

      estiloEditar = btnEditar.CssClass;
      estiloBorrar = btnBorrar.CssClass;

      if (Habilitar)
      {
        btnEditar.CssClass = estiloEditar.Contains(_ESTILODESHABILITADO) ? estiloEditar.Replace(_ESTILODESHABILITADO, "") : estiloEditar;
        btnBorrar.CssClass = estiloBorrar.Contains(_ESTILODESHABILITADO) ? estiloBorrar.Replace(_ESTILODESHABILITADO, "") : estiloBorrar;
      }
      else
      {
        btnEditar.CssClass = estiloEditar.Contains(_ESTILODESHABILITADO) ? estiloEditar : estiloEditar + " " + _ESTILODESHABILITADO;
        btnBorrar.CssClass = estiloEditar.Contains(_ESTILODESHABILITADO) ? estiloBorrar : estiloBorrar + " " + _ESTILODESHABILITADO;
      }

    }
  }
}