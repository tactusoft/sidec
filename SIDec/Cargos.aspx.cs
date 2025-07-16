using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using GLOBAL.DAL;
using GLOBAL.VAR;
using GLOBAL.CONST;
using GLOBAL.LOG;
using GLOBAL.UTIL;
using GLOBAL.PERMISOS;

namespace SIDec
{
  public partial class Cargos : System.Web.UI.Page
  {
    #region OBJETOS
    CARGOS_DAL oCargos = new CARGOS_DAL();
    //PERFILES_DAL oPerfiles = new PERFILES_DAL();

    clGlobalVar oVar = new clGlobalVar();
    clUtil oUtil = new clUtil();
    clLog oLog = new clLog();

    clPermisos oPermisos = new clPermisos();

    #endregion

    #region---CONSTANTES

    private const string _SOURCEPAGE = "Cargos";

    private const string _MSGCONTADORREGISTROS = "Registro {0} de {1}";

    private const string _DIVMSGCargos = "DivMsgCargos";

    #endregion

    bool bPageChanged = false;

    #region---EVENTOS DE CONTROL

    protected void Page_Load(object sender, EventArgs e)
    {
      Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "SetCursor('" + txtBuscar.ClientID + "');", true);
      btnConfirmarCargos.Attributes.Add("onclick", "document.getElementById('" + btnConfirmarCargos.ClientID + "').disabled=true;" + ClientScript.GetPostBackEventReference(btnConfirmarCargos, "Click") + "; return false;");

      fValidarSP();

      if (!IsPostBack)
      {
        ViewState["CriterioBuscar"] = "";
        ViewState["AccionFinal"] = "";
        ViewState["RealIndexCargos"] = "0";

        txtBuscar.Focus();

        oVar.prDSCargos = oCargos.sp_s_cargos();

        this.ViewState["SortExpression"] = "chip";
        this.ViewState["SortDirection"] = "ASC";

        fCargosLoadGV("");
        fBotonAccionFinalEstado(false);
        fBotonesNavegacionEstado(false);
      }
    }


    #region------BUTTONS

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
      ViewState["CriterioBuscar"] = txtBuscar.Text;
      fCargosLoadGV(ViewState["CriterioBuscar"].ToString());
      fActivarVistaGrid();
    }

    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
      switch (ViewState["AccionFinal"].ToString())
      {
        case "Editar":
          fCargosUpdate();
          break;
        case "Agregar":
          fCargosInsert();
          break;
        case "Eliminar":
        case "":
          fCargosDelete();
          break;
      }

      fCargosLoadGV("");
      mvCargos.ActiveViewIndex = 0;
      fBotonAccionFinalEstado(false);
    }

    protected void btnCargosAccion_Click(object sender, EventArgs e)
    {
      bool bActivarConfirmar = false;
      if (gvCargos.Rows.Count > 0)
        bActivarConfirmar = true;

      LinkButton btnAccionSource = (LinkButton)sender;
      ViewState["AccionFinal"] = btnAccionSource.CommandName;

      hfEvtGVCargos.Value = btnAccionSource.CommandArgument;

      switch (btnAccionSource.CommandName)
      {
        case "Editar":
          if (gvCargos.Rows.Count > 0)
          {
            fCargosDetalle();
            fCargosEstadoDetalle(true);
          }
          break;
        case "Agregar":
          bActivarConfirmar = true;
          fCargosLimpiarDetalle();
          fCargosEstadoDetalle(true);
          mvCargos.ActiveViewIndex = 1;
          break;
        case "Eliminar":
          if (gvCargos.Rows.Count > 0)
            fCargosDetalle();
          break;
      }
      fBotonAccionFinalEstado(bActivarConfirmar);
      upCargosFoot.Update();
    }

    protected void btnCargosCancelar_Click(object sender, EventArgs e)
    {
      btnCargosVista_Click(sender, e);
    }

    protected void btnCargosNavegacion_Click(object sender, EventArgs e)
    {
      int iIndex = 0;

      switch (((LinkButton)sender).CommandName)
      {
        case "Back":
          iIndex = Convert.ToInt16(ViewState["RealIndexCargos"]) - 1;
          break;
        case "Next":
          iIndex = Convert.ToInt16(ViewState["RealIndexCargos"]) + 1;
          break;
        case "Last":
          iIndex = ((DataSet)oVar.prDSCargosFiltro).Tables[0].Rows.Count - 1;
          break;
      }
      ViewState["RealIndexCargos"] = iIndex.ToString();
      fCargosEstadoDetalle(false);
      fCargosDetalle();
    }

    protected void btnCargosVista_Click(object sender, EventArgs e)
    {
      int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);
      int iIndex = 0;

      if (cmdArg == 0)
        fActivarVistaGrid();
      else
        mvCargos.ActiveViewIndex = cmdArg;

      switch (cmdArg)
      {
        case 0:
          int iPagina = Convert.ToInt16(ViewState["RealIndexCargos"]) / gvCargos.PageSize;

          if (iPagina > 0)
            iIndex = Convert.ToInt16(ViewState["RealIndexCargos"]) % gvCargos.PageSize;
          else
            iIndex = Convert.ToInt16(ViewState["RealIndexCargos"]);

          //fCargosLoadGV(gvCargosDec.SelectedDataKey.Value.ToString());
          gvCargos.PageIndex = iPagina;
          gvCargos.SelectedIndex = iIndex;
          break;
        case 1:
          fCargosEstadoDetalle(false);

          if (gvCargos.Rows.Count > 0)
            fCargosDetalle();

          break;
      }
      upCargosFoot.Update();
    }


    #endregion

    #region------GRIDVIEW  

    protected void gvCargos_DataBinding(object sender, EventArgs e)
    {
      fActivarVistaGrid();
    }

    protected void gvCargos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
      bPageChanged = true;
      gvCargos.PageIndex = e.NewPageIndex;
      fCargosLoadGV(ViewState["CriterioBuscar"].ToString());
      ViewState["RealIndexCargos"] = ((gvCargos.PageSize * gvCargos.PageIndex) + gvCargos.PageIndex - 1).ToString();
    }

    protected void gvCargos_RowCreated(object sender, GridViewRowEventArgs e)
    {
      if (e.Row.RowType != DataControlRowType.Header) return;

      string sortExpression = this.ViewState["SortExpression"].ToString();
      string sortDirection = this.ViewState["SortDirection"].ToString();

      foreach (TableCell tableCell in e.Row.Cells)
      {
        if (!tableCell.HasControls()) continue;

        LinkButton lbSort = tableCell.Controls[0] as LinkButton;

        if (lbSort == null) continue;

        if (lbSort.CommandArgument == sortExpression)
        {
          Image imageSort = new Image();
          imageSort.ImageAlign = ImageAlign.AbsMiddle;
          imageSort.Width = 10;

          if (sortDirection == "ASC") imageSort.ImageUrl = "~/Images/icon/up.png";
          else imageSort.ImageUrl = "~/images/icon/down.png";


          imageSort.Style.Add("margin-left", "15px");
          tableCell.Controls.Add(imageSort);
        }
      }
    }

    protected void gvCargos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if (e.Row.RowType == DataControlRowType.DataRow)
      {
        e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvCargos, "Select$" + e.Row.RowIndex.ToString()));
      }
    }

    protected void gvCargos_SelectedIndexChanged(object sender, EventArgs e)
    {
      ViewState["RealIndexCargos"] = ((gvCargos.PageIndex * gvCargos.PageSize) + gvCargos.SelectedIndex).ToString();

      if (SIDec.Properties.Settings.Default.Properties["DetalleOnClickCargos"] != null)
      {
        if (SIDec.Properties.Settings.Default.DetalleOnClickCargos)
        {
          fCargosDetalle();
          fCargosEstadoDetalle(false);
        }
      }
      fCuentaRegistros(gvCargos.SelectedIndex);
    }

    protected void gvCargos_Sorting(object sender, GridViewSortEventArgs e)
    {
      string sortExpression = this.ViewState["SortExpression"].ToString();
      string sortDirection = this.ViewState["SortDirection"].ToString();

      if (sortExpression == e.SortExpression)
      {
        this.ViewState["SortDirection"] = (sortDirection == "ASC") ? "DESC" : "ASC";
      }
      else
      {
        this.ViewState["SortExpression"] = e.SortExpression;
        this.ViewState["SortDirection"] = "ASC";
      }

      DataView sortedView = new DataView(((DataSet)(oVar.prDSCargosFiltro)).Tables[0]);
      sortedView.Sort = e.SortExpression + " " + sortDirection;
      Session["objects"] = sortedView;
      gvCargos.DataSource = sortedView;
      gvCargos.DataBind();

      //Almacenar el nuevo Dataset ordenado -nuevos Index-
      oVar.prDSCargosFiltro = oUtil.ConvertToDataSet(sortedView);
    }

    #endregion

    #region------MULTIVIEW

    protected void mvCargos_ActiveViewChanged(object sender, EventArgs e)
    {
      fBotonesNavegacionEstado(Convert.ToBoolean(mvCargos.ActiveViewIndex));
    }

    #endregion

    #endregion

    #region---METODOS

    #region------Cargos

    private void fCargosDelete()
    {
      string strResultado = oCargos.sp_d_cargo(txtAuCargo.Text);

      if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
      {
        oLog.RegistrarLogInfo(_SOURCEPAGE, "fCargosDelete:", clConstantes.MSG_OK_D);
        fCargosLimpiarDetalle();
        oVar.prDSCargos = oCargos.sp_s_cargos();

        if (Convert.ToInt16(ViewState["RealIndexCargos"]) > 0)
          ViewState["RealIndexCargos"] = Convert.ToInt16(ViewState["RealIndexCargos"]) - 1;
        else
          ViewState["RealIndexCargos"] = 0;

        fMensajeCRUD(clConstantes.MSG_OK_D, (int)clConstantes.NivelMensaje.Exitoso);
      }
      else
      {
        oLog.RegistrarLogInfo(_SOURCEPAGE, "fCargosDelete:", clConstantes.MSG_ERR_D);
        fMensajeCRUD(clConstantes.MSG_ERR_D, (int)clConstantes.NivelMensaje.Error);
      }
    }

    private void fCargosLoadGV(string Parametro)
    {
      DataSet odsCloneCargos = ((DataSet)(oVar.prDSCargos)).Clone();
      string strQuery = "";

      if (!string.IsNullOrEmpty(Parametro))
      {
        strQuery = string.Format("nombre_cargo LIKE '%{0}%'", Parametro);

        DataRow[] oDr = ((DataSet)oVar.prDSCargos).Tables[0].Select(strQuery);
        foreach (DataRow row in oDr)
        {
          odsCloneCargos.Tables[0].ImportRow(row);
        }
        gvCargos.DataSource = odsCloneCargos;
        gvCargos.DataBind();
        oVar.prDSCargosFiltro = odsCloneCargos;
      }
      else
      {
        gvCargos.DataSource = ((DataSet)(oVar.prDSCargos));
        gvCargos.DataBind();
        oVar.prDSCargosFiltro = (DataSet)(oVar.prDSCargos);
      }

      if (gvCargos.Rows.Count > 0)
      {
        gvCargos.SelectedIndex = 0;
        btnCargosVG.Enabled = true;
      }
     

      mvCargos.ActiveViewIndex = 0;
      fBotonAccionFinalEstado(false);


      upCargosBtnVistas.Update();
      upCargos.Update();
      fCuentaRegistros(0);
    }

    /// <summary>
    /// Cargar detalles del Usuario seleccionado
    /// </summary>  
    private void fCargosDetalle()
    {
      mvCargos.ActiveViewIndex = 1;
      int Indice = Convert.ToInt16(ViewState["RealIndexCargos"]);

      DataSet dsTmp = new DataSet();
      dsTmp = (DataSet)oVar.prDSCargosFiltro;
      int iTotalRegistros = dsTmp.Tables[0].Rows.Count;

      txtAuCargo.Text = dsTmp.Tables[0].Rows[Indice]["au_cargo"].ToString();
      txtCargo.Text = dsTmp.Tables[0].Rows[Indice]["nombre_cargo"].ToString();

      fCuentaRegistros(Indice);
    }

    /// <summary>
    /// Asignar el estado de los campos y botones de la Vista de Detalle
    /// </summary>
    /// <param name="HabilitarCampos"></param>
    /// <param name="VerBotonConfirmacion"></param>
    private void fCargosEstadoDetalle(bool HabilitarCampos)
    {
      txtAuCargo.Enabled = false;
      txtCargo.Enabled = HabilitarCampos;
    }

    private void fCargosInsert()
    {
      string strResultado = oCargos.sp_i_cargo(txtCargo.Text);
      if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
      {
        oLog.RegistrarLogInfo(_SOURCEPAGE, "fCargosInsert:", clConstantes.MSG_OK_I);
        fCargosLimpiarDetalle();
        oVar.prDSCargos = oCargos.sp_s_cargos();
        fMensajeCRUD(clConstantes.MSG_OK_I, (int)clConstantes.NivelMensaje.Exitoso);
      }
      else
      {
        oLog.RegistrarLogInfo(_SOURCEPAGE, "fCargosInsert:", clConstantes.MSG_ERR_I);
        fMensajeCRUD(clConstantes.MSG_ERR_I, (int)clConstantes.NivelMensaje.Error);
      }
    }

    private void fCargosLimpiarDetalle()
    {
      txtAuCargo.Text = "";
      txtCargo.Text = "";
    }

    private void fCargosUpdate()
    {
      string strResultado = oCargos.sp_u_cargo(txtAuCargo.Text, txtCargo.Text);
      if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
      {
        oLog.RegistrarLogInfo(_SOURCEPAGE, "fCargosUpdate:", clConstantes.MSG_OK_U);
        fCargosLimpiarDetalle();
        oVar.prDSCargos = oCargos.sp_s_cargos();
        fMensajeCRUD(clConstantes.MSG_OK_U, (int)clConstantes.NivelMensaje.Exitoso);
      }
      else
      {
        oLog.RegistrarLogInfo(_SOURCEPAGE, "fCargosUpdate:", clConstantes.MSG_ERR_U);
        fMensajeCRUD(clConstantes.MSG_ERR_U, (int)clConstantes.NivelMensaje.Error);
      }
    }

    #endregion

    #region------GENERALES
    
    /// <summary>
    /// Activar Vista de Grid
    /// </summary>  
    private void fActivarVistaGrid()
    {
      mvCargos.ActiveViewIndex = 0;
      fBotonAccionFinalEstado(false);
      upCargos.Update();
    }

    /// <summary>
    /// Establecer si se muestra o no los Botones de Confirmación y Cancelar Accion.
    /// </summary>    
    /// <param name="bVer"></param>
    private void fBotonAccionFinalEstado(bool bVer)
    {
      if (bVer)
      {
        btnCargosAccionFinal.Style.Add("visibility", "visible");
        btnCargosCancelar.Style.Add("visibility", "visible");
      }
      else
      {
        btnCargosAccionFinal.Style.Add("visibility", "hidden");
        btnCargosCancelar.Style.Add("visibility", "hidden");
      }
    }

    /// <summary>
    /// Asignar el estado de visivilidad al Div que contiene los controles de navegacion
    /// </summary>
    /// <param name="bVer"></param>
    private void fBotonesNavegacionEstado(bool bVer)
    {
      if (bVer)
        divCargosNavegacion.Style.Add("visibility", "visible");
      else
        divCargosNavegacion.Style.Add("visibility", "hidden");
    }

    /// <summary>
    /// Actualizar Mensaje de Navegacion de Registros
    /// </summary>    
    /// <param name="RealIndex"></param>
    private void fCuentaRegistros(int RealIndex)
    {
      if (((DataSet)oVar.prDSCargosFiltro).Tables[0].Rows.Count > 0)
      {
        if (bPageChanged && gvCargos.AllowPaging)
        {
          bPageChanged = false;
          lblCargosCuenta.Text = string.Format(_MSGCONTADORREGISTROS, ((gvCargos.PageSize * gvCargos.PageIndex) + gvCargos.PageIndex).ToString(), ((DataSet)oVar.prDSCargosFiltro).Tables[0].Rows.Count.ToString());
        }
        else
          lblCargosCuenta.Text = string.Format(_MSGCONTADORREGISTROS, (RealIndex + 1).ToString(), ((DataSet)oVar.prDSCargosFiltro).Tables[0].Rows.Count.ToString());
      }
      else
        lblCargosCuenta.Text = string.Format(_MSGCONTADORREGISTROS, "0", "0");

      if (RealIndex + 1 >= ((DataSet)oVar.prDSCargosFiltro).Tables[0].Rows.Count)
      {
        btnFirstCargos.Enabled = true;
        btnBackCargos.Enabled = true;
        btnNextCargos.Enabled = false;
        btnLastCargos.Enabled = false;
      }
      else if (RealIndex - 1 < 0)
      {
        btnFirstCargos.Enabled = false;
        btnBackCargos.Enabled = false;
        btnNextCargos.Enabled = true;
        btnLastCargos.Enabled = true;
      }
      else
      {
        btnFirstCargos.Enabled = true;
        btnBackCargos.Enabled = true;
        btnNextCargos.Enabled = true;
        btnLastCargos.Enabled = true;
      }

      upCargosFoot.Update();
    }
   
    private void fMensajeCRUD(string Mensaje, int NivelMensaje)
    {
      //Por el UP
      ScriptManager.RegisterStartupScript(upCargos, upCargos.GetType(), Guid.NewGuid().ToString(), "MensajeCRUD('" + Mensaje + "'," + NivelMensaje + ");", true);
    }

    private void fValidarSP()
    {
      
      (this.Master as Authentic).EstadoBoton(oPermisos.TienePermisosSP("sp_d_cargo"), btnCargosDel.ID);
      (this.Master as Authentic).EstadoBoton(oPermisos.TienePermisosSP("sp_i_cargo"), btnCargosAdd.ID);
      (this.Master as Authentic).EstadoBoton(oPermisos.TienePermisosSP("sp_u_cargo"), btnCargosEdit.ID);
    }


    #endregion

    #endregion


  }
}