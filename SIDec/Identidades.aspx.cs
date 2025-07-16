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
    public partial class Identidades : System.Web.UI.Page
    {
        #region OBJETOS
        IDENTIDADESCATEGORIAS_DAL oIdentidadesCat = new IDENTIDADESCATEGORIAS_DAL();
        IDENTIDADES_DAL oIdentidades = new IDENTIDADES_DAL();

        clGlobalVar oVar = new clGlobalVar();
        clUtil oUtil = new clUtil();
        clLog oLog = new clLog();

        clPermisos oPermisos = new clPermisos();
        #endregion

        #region---CONSTANTES
        private const string _SOURCEPAGE = "Identidades";

        private const string _MSGCONTADORREGISTROS = "Registro {0} de {1}";

        private const string _DIVMSGIDENTIDADES = "DivMsgIdentidades";
        #endregion

        bool bPageChanged = false;

        #region---EVENTOS DE CONTROL
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "SetCursor('" + txtBuscar.ClientID + "');", true);
            btnConfirmarIdentidades.Attributes.Add("onclick", "document.getElementById('" + btnConfirmarIdentidades.ClientID + "').disabled=true;" + ClientScript.GetPostBackEventReference(btnConfirmarIdentidades, "Click") + "; return false;");
            btnConfirmarIdentidadesCat.Attributes.Add("onclick", "document.getElementById('" + btnConfirmarIdentidadesCat.ClientID + "').disabled=true;" + ClientScript.GetPostBackEventReference(btnConfirmarIdentidadesCat, "Click") + "; return false;");

            fValidarSP();

            if (!IsPostBack)
            {
                ViewState["CriterioBuscar"] = "";
                ViewState["AccionFinal"] = "";
                ViewState["RealIndexIdentidadesCat"] = "0";
                ViewState["RealIndexIdentidades"] = "0";

                txtBuscar.Focus();

                oVar.prDSIdCategorias = oIdentidadesCat.sp_s_IdentidadesCategorias();
                oVar.prDSIdentidades = oIdentidades.sp_s_identidades();

                this.ViewState["SortExpression"] = "chip";
                this.ViewState["SortDirection"] = "ASC";

                fIdentidadesCatLoadGV(ViewState["CriterioBuscar"].ToString());
                fLoadDropDowns();
                fBotonesAccionesOcultar();

                fBotonesNavegacionEstado(divIdentidadesCatNavegacion, false);
                fBotonesNavegacionEstado(divIdentidadesNavegacion, false);
            }
        }

        #region------BUTTONS
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ViewState["CriterioBuscar"] = txtBuscar.Text;
            fIdentidadesCatLoadGV(ViewState["CriterioBuscar"].ToString());
            mvIdentidadesCat.ActiveViewIndex = 0;
        }

        protected void btnConfirmarIdentidadesCat_Click(object sender, EventArgs e)
        {
            switch (ViewState["AccionFinal"].ToString())
            {
                case "Editar":
                    fIdentidadesCatUpdate();
                    break;
                case "Agregar":
                    fIdentidadesCatInsert();
                    break;        
            }
            fIdentidadesCatLoadGV(ViewState["CriterioBuscar"].ToString());
        }

        protected void btnConfirmarIdentidades_Click(object sender, EventArgs e)
        {
            switch (ViewState["AccionFinal"].ToString())
            {
                case "Editar":
                    fIdentidadesUpdate();
                    break;
                case "Agregar":
                    fIdentidadesInsert();
                    break;
                case "Eliminar":
                case "":
                    fIdentidadesDelete();
                    break;
            }
            fIdentidadesLoadGV(gvIdentidadesCat.SelectedDataKey.Value.ToString());
        }

        #region----------CATEGORIAS
        protected void btnIdentidadesCatAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = btnAccionSource.CommandName;

            hfEvtGVIdentidadesCat.Value = btnAccionSource.CommandArgument;

            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    if (gvIdentidadesCat.Rows.Count > 0)
                    {
                    fIdentidadesCatDetalle();
                    fIdentidadesCatEstadoDetalle(true);
                    }
                    break;
                case "Agregar":
                    fIdentidadesCatLimpiarDetalle();
                    fIdentidadesCatEstadoDetalle(true);
                    mvIdentidadesCat.ActiveViewIndex = 1;
                    break;
                case "Eliminar":
                    if (gvIdentidadesCat.Rows.Count > 0)
                    fIdentidadesCatDetalle();
                    break;
            }
            fBotonAccionFinalEstado(btnIdentidadesCatAccionFinal, btnIdentidadesCatCancelar, true);
            upIdentidadesCatFoot.Update();
        }

        protected void btnIdentidadesCatCancelar_Click(object sender, EventArgs e)
        {
            btnIdentidadesCatVista_Click(sender, e);
        }

        protected void btnIdentidadesCatNavegacion_Click(object sender, EventArgs e)
        {
            int iIndex = 0;

            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    iIndex = Convert.ToInt16(ViewState["RealIndexIdentidadesCat"]) - 1;
                    break;
                case "Next":
                    iIndex = Convert.ToInt16(ViewState["RealIndexIdentidadesCat"]) + 1;
                    break;
                case "Last":
                    iIndex = ((DataSet)oVar.prDSIdCategoriasFiltro).Tables[0].Rows.Count - 1;
                    break;
            }
            ViewState["RealIndexIdentidadesCat"] = iIndex.ToString();

            fIdentidadesCatEstadoDetalle(false);
            fIdentidadesCatDetalle();
            fIdentidadesLoadGV(txtIdCategoria.Text);      
        }
    
        protected void btnIdentidadesCatVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            int iIndex = 0;

            //if (cmdArg == 0)
            //  mvIdentidadesCat.ActiveViewIndex = 0;
            //else
            //  mvIdentidadesCat.ActiveViewIndex = cmdArg;

            if (cmdArg == 0)
                fActivarVistaGrid(mvIdentidadesCat, btnIdentidadesCatAccionFinal, btnIdentidadesCatCancelar);
            else
                mvIdentidadesCat.ActiveViewIndex = cmdArg;

            switch (cmdArg)
            {
                case 0:
                    int iPagina = Convert.ToInt16(ViewState["RealIndexIdentidadesCat"]) / gvIdentidadesCat.PageSize;
                    if (iPagina > 0)
                        iIndex = Convert.ToInt16(ViewState["RealIndexIdentidadesCat"]) % gvIdentidadesCat.PageSize;
                    else
                        iIndex = Convert.ToInt16(ViewState["RealIndexIdentidadesCat"]);
                    fIdentidadesCatLoadGV(ViewState["CriterioBuscar"].ToString());
                    gvIdentidadesCat.PageIndex = iPagina;
                    gvIdentidadesCat.SelectedIndex = iIndex;
                    break;
                case 1:
                    fIdentidadesCatEstadoDetalle(false);
                    if (gvIdentidadesCat.Rows.Count > 0)
                        fIdentidadesCatDetalle();
                    break;
            }
            upIdentidadesCatFoot.Update();
        }  
        #endregion

        #region----------IDENTIDADES
        protected void btnIdentidadesAccion_Click(object sender, EventArgs e)
        {
            LinkButton btnAccionSource = (LinkButton)sender;
            ViewState["AccionFinal"] = btnAccionSource.CommandName;

            hfEvtGVIdentidades.Value = btnAccionSource.CommandArgument;

            switch (btnAccionSource.CommandName)
            {
                case "Editar":
                    if (gvIdentidades.Rows.Count > 0)
                    {
                    fIdentidadesDetalle();
                    fIdentidadesEstadoDetalle(true);
                    }
                    break;
                case "Agregar":
                    fIdentidadesLimpiarDetalle();
                    fIdentidadesEstadoDetalle(true);
                    mvIdentidades.ActiveViewIndex = 1;
                    ddlbCategoria.Items.FindByValue(gvIdentidadesCat.SelectedDataKey.Value.ToString()).Selected = true;
                    ddlbCategoria.Enabled = false;
                    break;
                case "Eliminar":
                    if (gvIdentidades.Rows.Count > 0)
                    fIdentidadesDetalle();
                    break;
            }
            fBotonAccionFinalEstado(btnIdentidadesAccionFinal, btnIdentidadesCancelar, true);
            upIdentidadesFoot.Update();
        }

        protected void btnIdentidadesCancelar_Click(object sender, EventArgs e)
        {
            btnIdentidadesVista_Click(sender, e);
        }

        protected void btnIdentidadesNavegacion_Click(object sender, EventArgs e)
        {
            int iIndex = 0;

            switch (((LinkButton)sender).CommandName)
            {
                case "Back":
                    iIndex = Convert.ToInt16(ViewState["RealIndexIdentidades"]) - 1;
                    break;
                case "Next":
                    iIndex = Convert.ToInt16(ViewState["RealIndexIdentidades"]) + 1;
                    break;
                case "Last":
                    iIndex = ((DataSet)oVar.prDSIdentidadesFiltro).Tables[0].Rows.Count - 1;
                    break;
            }
            ViewState["RealIndexIdentidades"] = iIndex.ToString();
            fIdentidadesEstadoDetalle(false);
            fIdentidadesDetalle();
        }

        protected void btnIdentidadesVista_Click(object sender, EventArgs e)
        {
            int cmdArg = Convert.ToInt16(((LinkButton)sender).CommandArgument);
            int iIndex = 0;

            if (cmdArg == 0)
            fActivarVistaGrid(mvIdentidades, btnIdentidadesAccionFinal, btnIdentidadesCancelar);
            else
            mvIdentidades.ActiveViewIndex = cmdArg;

            switch (cmdArg)
            {
                case 0:
                    int iPagina = Convert.ToInt16(ViewState["RealIndexIdentidades"]) / gvIdentidades.PageSize;

                    if (iPagina > 0)
                        iIndex = Convert.ToInt16(ViewState["RealIndexIdentidades"]) % gvIdentidades.PageSize;
                    else
                        iIndex = Convert.ToInt16(ViewState["RealIndexIdentidades"]);

                    fIdentidadesLoadGV(gvIdentidadesCat.SelectedDataKey.Value.ToString());
                    gvIdentidades.PageIndex = iPagina;
                    gvIdentidades.SelectedIndex = iIndex;
                    break;
                case 1:
                    fIdentidadesEstadoDetalle(false);

                    if (gvIdentidades.Rows.Count > 0)
                        fIdentidadesDetalle();

                    break;
            }
            upIdentidadesFoot.Update();
        }
        #endregion
        #endregion

        #region------GRIDVIEW  
        #region----------CATEGORIAS
        protected void gvIdentidadesCat_DataBinding(object sender, EventArgs e)
        {
            //mvIdentidadesCat.ActiveViewIndex = 0;
            fActivarVistaGrid(mvIdentidadesCat, btnIdentidadesCatAccionFinal, btnIdentidadesCatCancelar);
        }

        protected void gvIdentidadesCat_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvIdentidadesCat.PageIndex = e.NewPageIndex;
            fIdentidadesCatLoadGV(ViewState["CriterioBuscar"].ToString());
            }

        protected void gvIdentidadesCat_RowCreated(object sender, GridViewRowEventArgs e)
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
    
        protected void gvIdentidadesCat_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvIdentidadesCat, "Select$" + e.Row.RowIndex.ToString()));
            }
        }


        protected void gvIdentidadesCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["RealIndexIdentidadesCat"] = ((gvIdentidadesCat.PageIndex * gvIdentidadesCat.PageSize) + gvIdentidadesCat.SelectedIndex).ToString();

            if (SIDec.Properties.Settings.Default.Properties["DetalleOnClickIdentidadesCategoria"] != null)
            {
            if (SIDec.Properties.Settings.Default.DetalleOnClickIdentidadesCategoria)
            {
                fIdentidadesCatDetalle();
                fIdentidadesCatEstadoDetalle(false);
            }
            }
            fCuentaRegistros(lblIdentidadesCatCuenta, gvIdentidadesCat, (DataSet)oVar.prDSIdCategoriasFiltro, btnFirstIdentidadesCat, btnBackIdentidadesCat, btnNextIdentidadesCat, btnLastIdentidadesCat, upIdentidadesCatFoot, gvIdentidadesCat.SelectedIndex);
            fIdentidadesLoadGV(gvIdentidadesCat.SelectedDataKey.Value.ToString());      
        }

        protected void gvIdentidadesCat_Sorting(object sender, GridViewSortEventArgs e)
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

            DataView sortedView = new DataView(((DataSet)(oVar.prDSIdCategoriasFiltro)).Tables[0]);
            sortedView.Sort = e.SortExpression + " " + sortDirection;
            Session["objects"] = sortedView;
            gvIdentidadesCat.DataSource = sortedView;
            gvIdentidadesCat.DataBind();

            //Almacenar el nuevo Dataset ordenado -nuevos Index-
            oVar.prDSIdCategoriasFiltro = oUtil.ConvertToDataSet(sortedView);
        }
        #endregion

        #region----------IDENTIDADES
        protected void gvIdentidades_DataBinding(object sender, EventArgs e)
        {
            fActivarVistaGrid(mvIdentidades, btnIdentidadesAccionFinal, btnIdentidadesCancelar);
        }

        protected void gvIdentidades_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvIdentidades.PageIndex = e.NewPageIndex;
            fIdentidadesLoadGV(ViewState["CriterioBuscar"].ToString());
        }

        protected void gvIdentidades_RowCreated(object sender, GridViewRowEventArgs e)
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
                else imageSort.ImageUrl = "~/Images/icon/down.png";


                imageSort.Style.Add("margin-left", "15px");
                tableCell.Controls.Add(imageSort);
            }
            }
        }

        protected void gvIdentidades_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvIdentidades, "Select$" + e.Row.RowIndex.ToString()));
            }
        }

        protected void gvIdentidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["RealIndexIdentidades"] = ((gvIdentidades.PageIndex * gvIdentidades.PageSize) + gvIdentidades.SelectedIndex).ToString();

            if (SIDec.Properties.Settings.Default.Properties["DetalleOnClickIdentidades"] != null)
            {
            if (SIDec.Properties.Settings.Default.DetalleOnClickIdentidades)
            {
                fIdentidadesDetalle();
                fIdentidadesEstadoDetalle(false);
            }
            }
            fCuentaRegistros(lblIdentidadesCuenta, gvIdentidades, (DataSet)oVar.prDSIdentidadesFiltro, btnFirstIdentidades, btnBackIdentidades, btnNextIdentidades, btnLastIdentidades, upIdentidadesFoot, gvIdentidades.SelectedIndex);
        }

        protected void gvIdentidades_Sorting(object sender, GridViewSortEventArgs e)
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

            DataView sortedView = new DataView(((DataSet)(oVar.prDSIdentidadesFiltro)).Tables[0]);
            sortedView.Sort = e.SortExpression + " " + sortDirection;
            Session["objects"] = sortedView;
            gvIdentidades.DataSource = sortedView;
            gvIdentidades.DataBind();

            //Almacenar el nuevo Dataset ordenado -nuevos Index-
            oVar.prDSIdentidadesFiltro = oUtil.ConvertToDataSet(sortedView);
        }    
        #endregion
        #endregion

        #region------MULTIVIEW
        protected void mvIdentidadesCat_ActiveViewChanged(object sender, EventArgs e)
        {
            fBotonesNavegacionEstado(divIdentidadesCatNavegacion, Convert.ToBoolean(mvIdentidadesCat.ActiveViewIndex));
        }
    
        protected void mvIdentidades_ActiveViewChanged(object sender, EventArgs e)
        {
            fBotonesNavegacionEstado(divIdentidadesNavegacion, Convert.ToBoolean(mvIdentidades.ActiveViewIndex));
        }
        #endregion
        #endregion

        #region---METODOS
        #region------CATEGORIAS
        private void fIdentidadesCatDetalle()
        {
            mvIdentidadesCat.ActiveViewIndex = 1;
            int Indice = Convert.ToInt16(ViewState["RealIndexIdentidadesCat"]);

            DataSet dsTmp = new DataSet();
            dsTmp = (DataSet)oVar.prDSIdCategoriasFiltro;
            int iTotalRegistros = dsTmp.Tables[0].Rows.Count;

            txtIdCategoria.Text = dsTmp.Tables[0].Rows[Indice]["id_categoria_identidad"].ToString();
            txtCategoria.Text = dsTmp.Tables[0].Rows[Indice]["categoria_identidad"].ToString();
            txtDescripcionCat.Text = dsTmp.Tables[0].Rows[Indice]["descripcion_categoria_identidad"].ToString();

            fCuentaRegistros(lblIdentidadesCatCuenta, gvIdentidadesCat, (DataSet)oVar.prDSIdCategoriasFiltro, btnFirstIdentidadesCat, btnBackIdentidadesCat, btnNextIdentidadesCat, btnLastIdentidadesCat, upIdentidadesCatFoot, Indice);
        }

        private void fIdentidadesCatEstadoDetalle(bool HabilitarCampos)
        {
            txtIdCategoria.Enabled = false;
            txtCategoria.Enabled = HabilitarCampos;
            txtDescripcionCat.Enabled = HabilitarCampos;
        }

        private void fIdentidadesCatInsert()
        {
            string strResultado = oIdentidadesCat.sp_i_identidad_categoria(txtCategoria.Text, txtDescripcionCat.Text);
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
            oLog.RegistrarLogInfo(_SOURCEPAGE, "fIdentidadesCatInsert:", clConstantes.MSG_OK_I);
            fIdentidadesCatLimpiarDetalle();
            oVar.prDSIdentidadesCat= oIdentidadesCat.sp_s_IdentidadesCategorias();
            fMensajeCRUD(clConstantes.MSG_OK_I, (int)clConstantes.NivelMensaje.Exitoso);
            }
            else
            {
            oLog.RegistrarLogInfo(_SOURCEPAGE, "fIdentidadesCatInsert:", clConstantes.MSG_ERR_I);
            fMensajeCRUD(clConstantes.MSG_ERR_I, (int)clConstantes.NivelMensaje.Error);
            }
        }

        private void fIdentidadesCatLimpiarDetalle()
        {
            txtIdCategoria.Text = "";
            txtCategoria.Text = "";
            txtDescripcionCat.Text = "";
        }

        private void fIdentidadesCatLoadGV(string Parametro)
        {
            DataSet odsCloneIdentidadesCat = ((DataSet)(oVar.prDSIdCategorias)).Clone();
            string strQuery = "";

            if (!string.IsNullOrEmpty(Parametro))
            {
                strQuery = string.Format("categoria_identidad LIKE '%{0}%'", Parametro);

                DataRow[] oDr = ((DataSet)oVar.prDSIdCategorias).Tables[0].Select(strQuery);
                foreach (DataRow row in oDr)
                {
                    odsCloneIdentidadesCat.Tables[0].ImportRow(row);
                }
                gvIdentidadesCat.DataSource = odsCloneIdentidadesCat;
                gvIdentidadesCat.DataBind();
                oVar.prDSIdCategoriasFiltro = odsCloneIdentidadesCat;
            }
            else
            {
                oVar.prDSIdCategorias = oIdentidadesCat.sp_s_IdentidadesCategorias();
                gvIdentidadesCat.DataSource = oVar.prDSIdCategorias;
                gvIdentidadesCat.DataBind();        
                oVar.prDSIdCategoriasFiltro = (DataSet)(oVar.prDSIdCategorias);
            }

            if (gvIdentidadesCat.Rows.Count > 0)
            {
                gvIdentidadesCat.SelectedIndex = 0;
                btnIdentidadesCatVG.Enabled = true;
            }

            mvIdentidadesCat.ActiveViewIndex = 0;
            fBotonAccionFinalEstado(btnIdentidadesCatAccionFinal, btnIdentidadesCatCancelar, false);
            fIdentidadesLoadGV(gvIdentidadesCat.SelectedDataKey.Value.ToString());

            upIdentidadesCatBtnVistas.Update();
            upIdentidadesCat.Update();
            fCuentaRegistros(lblIdentidadesCatCuenta, gvIdentidadesCat, (DataSet)oVar.prDSIdCategoriasFiltro, btnFirstIdentidadesCat, btnBackIdentidadesCat, btnNextIdentidadesCat, btnLastIdentidadesCat, upIdentidadesCatFoot, 0);
        }
            
        private void fIdentidadesCatUpdate()
        {
            string strResultado = oIdentidadesCat.sp_u_identidad_categoria(txtIdCategoria.Text, txtCategoria.Text, txtDescripcionCat.Text);
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
            oLog.RegistrarLogInfo(_SOURCEPAGE, "fIdentidadesCatUpdate:", clConstantes.MSG_OK_U);
            fIdentidadesCatLimpiarDetalle();
            oVar.prDSIdentidadesCat = oIdentidadesCat.sp_s_IdentidadesCategorias();
            fMensajeCRUD(clConstantes.MSG_OK_U, (int)clConstantes.NivelMensaje.Exitoso);
            }
            else
            {
            oLog.RegistrarLogInfo(_SOURCEPAGE, "fIdentidadesCatUpdate:", clConstantes.MSG_ERR_U);
            fMensajeCRUD(clConstantes.MSG_ERR_U, (int)clConstantes.NivelMensaje.Error);
            }
        }
        #endregion

        #region------IDENTIDADES
        private void fIdentidadesDelete()
        {
            string strResultado = oIdentidades.sp_d_identidad(txtIdIdentidad.Text);

            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
            oLog.RegistrarLogInfo(_SOURCEPAGE, "fIdentidadesDelete:", clConstantes.MSG_OK_D);
            fIdentidadesLimpiarDetalle();
            oVar.prDSIdentidades = oIdentidades.sp_s_identidades();

            if (Convert.ToInt16(ViewState["RealIndexIdentidades"]) > 0)
                ViewState["RealIndexIdentidades"] = Convert.ToInt16(ViewState["RealIndexIdentidades"]) - 1;
            else
                ViewState["RealIndexIdentidades"] = 0;

            fMensajeCRUD(clConstantes.MSG_OK_D, (int)clConstantes.NivelMensaje.Exitoso);
            }
            else
            {
            oLog.RegistrarLogInfo(_SOURCEPAGE, "fIdentidadesDelete:", clConstantes.MSG_ERR_D);
            fMensajeCRUD(clConstantes.MSG_ERR_D, (int)clConstantes.NivelMensaje.Error);
            }
        }

        private void fIdentidadesLoadGV(string Parametro)
        {
            DataSet odsCloneIdentidades = ((DataSet)(oVar.prDSIdentidades)).Clone();
            string strQuery = "";

            if (!string.IsNullOrEmpty(Parametro))
            {
            strQuery = string.Format("id_categoria_identidad = '{0}'", Parametro);

            DataRow[] oDr = ((DataSet)oVar.prDSIdentidades).Tables[0].Select(strQuery);
            foreach (DataRow row in oDr)
            {
                odsCloneIdentidades.Tables[0].ImportRow(row);
            }
            gvIdentidades.DataSource = odsCloneIdentidades;
            gvIdentidades.DataBind();
            oVar.prDSIdentidadesFiltro = odsCloneIdentidades;
            }
            else
            {
            gvIdentidades.DataSource = ((DataSet)(oVar.prDSIdentidades));
            gvIdentidades.DataBind();
            oVar.prDSIdentidadesFiltro = (DataSet)(oVar.prDSIdentidades);
            }

            if (gvIdentidades.Rows.Count > 0)
            {
            gvIdentidades.SelectedIndex = 0;
            btnIdentidadesVG.Enabled = true;
            }

            mvIdentidades.ActiveViewIndex = 0;
            fBotonAccionFinalEstado(btnIdentidadesAccionFinal, btnIdentidadesCancelar, false);


            upIdentidadesBtnVistas.Update();
            upIdentidades.Update();
            fCuentaRegistros(lblIdentidadesCuenta, gvIdentidades, (DataSet)oVar.prDSIdentidadesFiltro, btnFirstIdentidades, btnBackIdentidades, btnNextIdentidades, btnLastIdentidades, upIdentidadesFoot, 0);
        }
 
        private void fIdentidadesDetalle()
        {
            mvIdentidades.ActiveViewIndex = 1;
            int Indice = Convert.ToInt16(ViewState["RealIndexIdentidades"]);

            DataSet dsTmp = new DataSet();
            dsTmp = (DataSet)oVar.prDSIdentidadesFiltro;
            int iTotalRegistros = dsTmp.Tables[0].Rows.Count;

            txtIdIdentidad.Text = dsTmp.Tables[0].Rows[Indice]["id_identidad"].ToString();
            txtIdentidad.Text = dsTmp.Tables[0].Rows[Indice]["nombre_identidad"].ToString();
            txtDescripcionIdentidad.Text = dsTmp.Tables[0].Rows[Indice]["descripcion_identidad"].ToString();

            ddlbCategoria.SelectedIndex = -1;
            if (!string.IsNullOrEmpty(dsTmp.Tables[0].Rows[Indice]["id_categoria_identidad"].ToString()))
            ddlbCategoria.Items.FindByValue(dsTmp.Tables[0].Rows[Indice]["id_categoria_identidad"].ToString()).Selected = true;

            txtOrdenIdentidad.Text = dsTmp.Tables[0].Rows[Indice]["orden_identidad"].ToString();
            chkbHabilitado.Checked = Convert.ToBoolean(Convert.ToInt16(dsTmp.Tables[0].Rows[Indice]["habilitado"].ToString()));                                    
            txtNombreIdentidad2.Text = dsTmp.Tables[0].Rows[Indice]["nombre_identidad2"].ToString();
      
            fCuentaRegistros(lblIdentidadesCuenta, gvIdentidades, (DataSet)oVar.prDSIdentidadesFiltro, btnFirstIdentidades, btnBackIdentidades, btnNextIdentidades, btnLastIdentidades, upIdentidadesFoot, Indice);

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "defaultddlbIdentidades();", true);
        }

        private void fIdentidadesEstadoDetalle(bool HabilitarCampos)
        {
            txtIdIdentidad.Enabled = false;
            txtIdentidad.Enabled = HabilitarCampos;
            txtDescripcionIdentidad.Enabled = HabilitarCampos;
            ddlbCategoria.Enabled = HabilitarCampos;
            txtOrdenIdentidad.Enabled = HabilitarCampos;
            chkbHabilitado.Enabled = HabilitarCampos;
            txtNombreIdentidad2.Enabled = HabilitarCampos;
        }

        private void fIdentidadesInsert()
        {
            string strResultado = oIdentidades.sp_i_identidad(ddlbCategoria.SelectedValue, txtIdentidad.Text, txtDescripcionIdentidad.Text, txtOrdenIdentidad.Text, txtNombreIdentidad2.Text, chkbHabilitado.Checked);
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
            oLog.RegistrarLogInfo(_SOURCEPAGE, "fIdentidadesInsert:", clConstantes.MSG_OK_I);
            fIdentidadesLimpiarDetalle();
            oVar.prDSIdentidades = oIdentidades.sp_s_identidades();
            fMensajeCRUD(clConstantes.MSG_OK_I, (int)clConstantes.NivelMensaje.Exitoso);
            }
            else
            {
            oLog.RegistrarLogInfo(_SOURCEPAGE, "fIdentidadesInsert:", clConstantes.MSG_ERR_I);
            fMensajeCRUD(clConstantes.MSG_ERR_I, (int)clConstantes.NivelMensaje.Error);
            }
        }

        private void fIdentidadesLimpiarDetalle()
        {
            txtIdIdentidad.Text = "";
            txtIdentidad.Text = "";
            txtDescripcionIdentidad.Text = "";      
            ddlbCategoria.SelectedIndex = -1;
            txtOrdenIdentidad.Text = "";
            chkbHabilitado.Enabled = false;
            txtNombreIdentidad2.Text = "";
        }

        private void fIdentidadesUpdate()
        {
            string strResultado = oIdentidades.sp_u_identidad(txtIdIdentidad.Text, ddlbCategoria.SelectedValue, txtIdentidad.Text, txtDescripcionIdentidad.Text, txtOrdenIdentidad.Text, txtNombreIdentidad2.Text, chkbHabilitado.Checked);
            if (strResultado.Substring(0, 5) == clConstantes.DB_ACTION_OK)
            {
            oLog.RegistrarLogInfo(_SOURCEPAGE, "fIdentidadesUpdate:", clConstantes.MSG_OK_U);
            fIdentidadesLimpiarDetalle();
            oVar.prDSIdentidades = oIdentidades.sp_s_identidades();
            fMensajeCRUD(clConstantes.MSG_OK_U, (int)clConstantes.NivelMensaje.Exitoso);
            }
            else
            {
            oLog.RegistrarLogInfo(_SOURCEPAGE, "fIdentidadesUpdate:", clConstantes.MSG_ERR_U);
            fMensajeCRUD(clConstantes.MSG_ERR_U, (int)clConstantes.NivelMensaje.Error);
            }
        }
        #endregion
    
        #region------GENERALES
        private void fActivarVistaGrid(MultiView mvSource, LinkButton btnOkSource, LinkButton btnCancelSource)
        {
            mvSource.ActiveViewIndex = 0;
            fBotonAccionFinalEstado(btnOkSource, btnCancelSource, false);
        }

        private void fBotonAccionFinalEstado(LinkButton btnOkSource, LinkButton btnCancelSource, bool bVer)
        {
            if (bVer)
            {
            btnOkSource.Style.Add("visibility", "visible");
            btnCancelSource.Style.Add("visibility", "visible");
            }
            else
            {
            btnOkSource.Style.Add("visibility", "hidden");
            btnCancelSource.Style.Add("visibility", "hidden");
            }
        }

        private void fBotonesAccionesOcultar()
        {
            fBotonAccionFinalEstado(btnIdentidadesAccionFinal, btnIdentidadesCancelar, false);
        }

        private void fBotonesNavegacionEstado(Panel panelSource, bool bVer)
        {
            if (bVer)
            panelSource.Style.Add("visibility", "visible");
            else
            panelSource.Style.Add("visibility", "hidden");
        }

        private void fCuentaRegistros(Label lblCuenta, GridView gvDatos, DataSet dsFiltro, LinkButton Primero, LinkButton Anterior, LinkButton Siguiente, LinkButton Ultimo, UpdatePanel upNavegacion, int RealIndex)
        {

            if (dsFiltro.Tables[0].Rows.Count > 0)
            {
            if (bPageChanged && gvDatos.AllowPaging)
            {
                bPageChanged = false;
                lblCuenta.Text = string.Format(_MSGCONTADORREGISTROS, ((gvDatos.PageSize * gvDatos.PageIndex) + gvDatos.PageIndex).ToString(), dsFiltro.Tables[0].Rows.Count.ToString());
            }
            else
                lblCuenta.Text = string.Format(_MSGCONTADORREGISTROS, (RealIndex + 1).ToString(), dsFiltro.Tables[0].Rows.Count.ToString());
            }
            else
            lblCuenta.Text = string.Format(_MSGCONTADORREGISTROS, "0", "0");

            if (RealIndex + 1 >= dsFiltro.Tables[0].Rows.Count)
            {
            Primero.Enabled = true;
            Anterior.Enabled = true;
            Siguiente.Enabled = false;
            Ultimo.Enabled = false;
            }
            else if (RealIndex - 1 < 0)
            {
            Primero.Enabled = false;
            Anterior.Enabled = false;
            Siguiente.Enabled = true;
            Ultimo.Enabled = true;
            }
            else
            {
            Primero.Enabled = true;
            Anterior.Enabled = true;
            Siguiente.Enabled = true;
            Ultimo.Enabled = true;
            }

            upNavegacion.Update();
        }

        private void fLoadDropDowns()
        {
            ddlbCategoria.DataSource = oIdentidadesCat.sp_s_IdentidadesCategorias();
            ddlbCategoria.DataTextField = "categoria_identidad";
            ddlbCategoria.DataValueField = "id_categoria_identidad";
            ddlbCategoria.DataBind();
        }

        private void fMensajeCRUD(string Mensaje, int NivelMensaje)
        {
            //Por el UP
            ScriptManager.RegisterStartupScript(upIdentidades, upIdentidades.GetType(), Guid.NewGuid().ToString(), "MensajeCRUD('" + Mensaje + "'," + NivelMensaje + ",'" + _DIVMSGIDENTIDADES + "');", true);
        }

        private void fValidarSP()
        {
            (this.Master as Authentic).EstadoBoton(oPermisos.TienePermisosSP("sp_d_identidad"), btnIdentidadesDel.ID);
            (this.Master as Authentic).EstadoBoton(oPermisos.TienePermisosSP("sp_i_identidad"), btnIdentidadesAdd.ID);
            (this.Master as Authentic).EstadoBoton(oPermisos.TienePermisosSP("sp_u_identidad"), btnIdentidadesEdit.ID);
        }
        #endregion
        #endregion
    }
}