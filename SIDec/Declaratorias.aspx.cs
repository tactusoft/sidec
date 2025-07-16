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
	public partial class Declaratorias : System.Web.UI.Page
	{
		#region--------------------------------------------------------------------VARIABLES
		DECLARATORIAS_DAL oDeclaratorias = new DECLARATORIAS_DAL();

		clGlobalVar oVar = new clGlobalVar();
		clFile oFile = new clFile();
		clPermisos oPermisos = new clPermisos();
		clBasic oBasic = new clBasic();

		private const string _SOURCEPAGE = "Declaratorias";
		#endregion

		#region--------------------------------------------------------------------PAGE
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				//oVar.prDSDeclaratorias = oDeclaratorias.sp_s_declaratorias();

				ViewState["IndexDeclaratorias"] = "0";
				ViewState["SortExpDeclaratorias"] = "fecha_resolucion_declaratoria";
				ViewState["SortDirDeclaratorias"] = "ASC";

				fDeclaratoriasLoadGV();
			}
		}
		#endregion

		#region--------------------------------------------------------------------BUTTONS
		#endregion

		#region--------------------------------------------------------------------GRIDVIEW
		protected void gvDeclaratorias_DataBinding(object sender, EventArgs e)
		{
		}

		protected void gvDeclaratorias_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvDeclaratorias, "Select$" + e.Row.RowIndex.ToString()));
			}
		}

		protected void gvDeclaratorias_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (e.CommandName == "OpenFile")
			{
				gvDeclaratorias.SelectedIndex = Convert.ToInt32(e.CommandArgument);
				int rowIndex = Convert.ToInt32(e.CommandArgument);
				string rowKey = gvDeclaratorias.DataKeys[rowIndex].Value.ToString();
				oFile.GetPath(oVar.prPathDeclaratorias + rowKey + ".pdf");
				ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "verDoc();", true);
			}
		}
		#endregion

		#region---METODOS
		#region------Declaratorias
		private void fDeclaratoriasLoadGV()
		{
			oVar.prDSDeclaratorias = oDeclaratorias.sp_s_declaratorias();
			gvDeclaratorias.DataSource = ((DataSet)(oVar.prDSDeclaratorias));
			gvDeclaratorias.DataBind();

			if (gvDeclaratorias.Rows.Count > 0)
			{
				gvDeclaratorias.Visible = true;
				gvDeclaratorias.SelectedIndex = Convert.ToInt16(ViewState["IndexDeclaratorias"].ToString());
				if (gvDeclaratorias.Rows.Count < gvDeclaratorias.SelectedIndex)
					gvDeclaratorias.SelectedIndex = 0;
				oVar.prDeclaratoriasAu = gvDeclaratorias.SelectedDataKey.Value.ToString();
				//oBasic.FixPanel(divData, "Declaratorias", 0);
			}
			else
			{
				gvDeclaratorias.Visible = false;
				//oBasic.FixPanel(divData, "Declaratorias", 3);
			}
		}
		#endregion
		#endregion

		#region--------------------------------------------------------------------GENERALES
		protected void gv_RowCreated(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType != DataControlRowType.Header)
				return;

			GridView gv = sender as GridView;
			string modulo = gv.ID.Substring(2);
			string sortExpression = ViewState["SortExp" + modulo].ToString();
			string sortDirection = ViewState["SortDir" + modulo].ToString();
			foreach (TableCell tableCell in e.Row.Cells)
			{
				if (!tableCell.HasControls())
					continue;
				LinkButton lbSort = tableCell.Controls[0] as LinkButton;
				if (lbSort == null)
					continue;
				if (lbSort.CommandArgument == sortExpression)
				{
					Image imageSort = new Image();
					imageSort.ImageAlign = ImageAlign.AbsMiddle;
					imageSort.Width = 12;
					imageSort.Style.Add("margin-left", "3px");
					if (sortDirection == "ASC")
						imageSort.ImageUrl = "~/Images/icon/up.png";
					else
						imageSort.ImageUrl = "~/Images/icon/down.png";
					tableCell.Controls.Add(imageSort);
				}
			}
		}

		private void gv_SelectedIndexChanged(GridView gv)
		{
			string modulo = gv.ID.Substring(2);
			ViewState["Index" + modulo] = ((gv.PageIndex * gv.PageSize) + gv.SelectedIndex).ToString();
			try
			{
				UpdatePanel up = (UpdatePanel)divData.FindControl("up" + modulo + "Foot");
				oBasic.LblRegistros(up, gv.Rows.Count, Convert.ToInt32(ViewState["Index" + modulo]));
			}
			catch { }

			//if (SIDec.Properties.Settings.Default.Properties["DetalleOnClick" + modulo] != null)
			//{
			// *************** pendiente ajustar ***************
			//if (SIDec.Properties.Settings.Default.DetalleOnClickDeclaratorias)
			//{
			//    //fDeclaratoriasDetalle();
			//    string s = "f" + modulo + "Detalle";
			//    //fDeclaratoriasEstadoDetalle(false);
			//    s = "f" + modulo + "EstadoDetalle";
			//}
			//}
		}
		#endregion
	}
}