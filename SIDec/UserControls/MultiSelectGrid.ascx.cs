using GLOBAL.DAL;
using System;
using System.Data;
using System.Web.UI.WebControls;
using cnsSection = GLOBAL.CONST.clConstantes.Section;

namespace SIDec.UserControls
{
    public partial class MultiSelectGrid : System.Web.UI.UserControl
    {
        readonly LOCALIDADES_DAL oLocalidades = new LOCALIDADES_DAL();
        readonly UPZ_DAL oUPZ = new UPZ_DAL();
        readonly USUARIOS_DAL oUsuarios = new USUARIOS_DAL();

        public string ControlID
        {
            get { return hddClientID.Value; }
            set { hddClientID.Value = value; }
        }
        public bool Enabled
        {
            get { return Boolean.Parse((Session[ClientID + ".Enabled"] ?? "false").ToString()); }
            set { Session[ClientID + ".Enabled"] = value; }
        }
        public string ChildName
        {
            set
            {
                lblchildName.Text = value;

                lblchildName.Visible = ddl_child.Visible = !(value == "");
                gvMultiSelectGrid.Columns[1].Visible = !(value == "");
                ddl_parent.AutoPostBack = !(value == "");
                rfv_childvalue.Enabled = !(value == "");
            }
        }
        public DataTable Data
        {
            get {
                DataTable dt = (DataTable)Session[ClientID + ".Data"];
                RenameColumns(dt);
                return dt; }
            set
            {
                DataTable dt = value;
                RenameColumns(dt);
                Session[ClientID + ".Data"] = dt;
            }
        }

        
        public string ParentName
        {
            set { lblParentName.Text = value; }
        }
        public string Title
        {
            set { lblTitle.Text = value; }
        }
        public int TypeReference
        {
            get { return Int32.Parse((Session[ClientID + ".TypeReference"] ?? "0").ToString()); }
            set { Session[ClientID + ".TypeReference"] = value; }
        }
        public new string ToString()
        {
            string values = "";
            string parent = "";
            string child = "";
            if (Data == null)
                return null;

            foreach (DataRow dr in Data.Rows)
            {
                if(parent != dr["nombreParent"].ToString())
                {
                    values += (parent == "" ? "" : (", " + parent + (child==""?"":" (" + child + ")")).Replace("(, ", "("));
                    parent = dr["nombreParent"].ToString();
                    child = "";
                }
                if(lblchildName.Text != "")
                    child += ", " + dr["nombreChild"].ToString().Trim();
            }
            values += (parent == "" ? "" : (", " + parent + (child == "" ? "" : " (" + child + ")")).Replace("(, ", "("));
            return (values+" ").Substring(1).Trim();
        }



        public delegate void OnAcceptEventHandler(object sender);
        public event OnAcceptEventHandler Accept;
        public delegate void OnRemoveEventHandler(object sender);
        public event OnRemoveEventHandler Remove;


        protected void Page_Load(object sender, EventArgs e)
        {
            btnAceptar.Focus();
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            MPE.Show();
            DataTable dt = Data;
            RenameColumns(dt);
            DataRow dr = dt.NewRow();
            bool noExist = true;

            foreach(DataRow item in dt.Rows)
            {
                noExist = item["id"].ToString() == (lblchildName.Text == "" ? ddl_parent.SelectedValue : ddl_child.SelectedValue) ? false : noExist;
            }

            if (noExist)
            {
                dr["id"] = lblchildName.Text == "" ? ddl_parent.SelectedValue : ddl_child.SelectedValue;
                dr["nombreParent"] = ddl_parent.SelectedItem.Text;
                if(lblchildName.Text != "")
                    dr["nombreChild"] = ddl_child.SelectedItem.Text;
                dt.Rows.Add(dr);

                dt.DefaultView.Sort = "nombreParent" + (lblchildName.Text != "" ? ", nombreChild" : "");
                dt = dt.DefaultView.ToTable();
            }
             
            Data = dt;

            gvMultiSelectGrid.DataSource = dt;
            gvMultiSelectGrid.DataBind();

            MPE.OkControlID = "btnCerrar";
            pnlContenedor.Visible = true;
            btnAceptar.Focus();
            Accept?.Invoke(this);
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            pnlContenedor.Visible = false;
            MPE.Hide();
        }
        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            Session.Remove(Request.RawUrl + "KeyMsgBox");
        }
        protected void ddl_parent_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadChildDropDowns();
            MPE.Show();
        }
        protected void gvMultiSelectGrid_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int rowIndex;
            MPE.Show();
            if (int.TryParse(e.CommandArgument.ToString(), out rowIndex))
            {
                if (rowIndex >= gvMultiSelectGrid.Rows.Count)
                    rowIndex = 0;
                
                switch (e.CommandName)
                {
                    case "_Delete":
                        DataTable dt = Data;
                        dt.Rows.RemoveAt(rowIndex);
                        Data = dt;
                        gvMultiSelectGrid.DataSource = dt;
                        gvMultiSelectGrid.DataBind();
                        break;
                    default:
                        return;
                }
            }
            Remove?.Invoke(this);
        }



        private void LoadParentDropDowns()
        {
            ddl_parent.Items.Clear();
            switch (TypeReference)
            {
                case 1: //
                    ddl_parent.DataSource = oLocalidades.sp_s_localidades();
                    ddl_parent.DataTextField = "nombre_localidad";
                    ddl_parent.DataValueField = "cod_localidad";
                    ddl_parent.Items.Insert(0, new ListItem("-- Seleccione opción", ""));
                    ddl_parent.DataBind();
                    break;
                case 2: //
                    ddl_parent.DataSource = oUsuarios.sp_s_usuarios_acceso_opcion((cnsSection.PROYECTO_ESTRATEGICO).ToString()).Tables[0];
                    ddl_parent.DataTextField = "nombre_completo";
                    ddl_parent.DataValueField = "cod_usuario";
                    ddl_parent.Items.Insert(0, new ListItem("-- Seleccione opción", ""));
                    ddl_parent.DataBind();
                    break;
                default: break;
            }
            LoadChildDropDowns();
        }
        private void LoadChildDropDowns()
        {
            ddl_child.Items.Clear();
            switch (TypeReference)
            {
                case 1: //
                    ddl_child.DataSource = oUPZ.sp_s_upz(ddl_parent.SelectedValue);
                    ddl_child.DataTextField = "nombre_upz";
                    ddl_child.DataValueField = "cod_upz";
                    ddl_child.Items.Insert(0, new ListItem("-- Seleccione opción", ""));
                    ddl_child.DataBind();
                    break;
                default: break;
            }
            upChild.Update();
        }
        private void RenameColumns(DataTable dt)
        {
            switch (TypeReference)
            {
                case 1: //
                    if (dt.Columns["idupz"] != null) dt.Columns["idupz"].ColumnName = "id";
                    if (dt.Columns["localidad"] != null) dt.Columns["localidad"].ColumnName = "nombreParent";
                    if (dt.Columns["upz"] != null) dt.Columns["upz"].ColumnName = "nombreChild";
                    break;
                case 2: //
                    if (dt.Columns["cod_usuario"] != null) dt.Columns["cod_usuario"].ColumnName = "id";
                    if (dt.Columns["nombre_completo"] != null) dt.Columns["nombre_completo"].ColumnName = "nombreParent";
                    break;
                default: break;
            }
        }
        public void ShowModal()
        {
            MPE.Show();
            LoadParentDropDowns();

            gvMultiSelectGrid.Columns[0].HeaderText = lblParentName.Text;
            gvMultiSelectGrid.Columns[1].HeaderText = lblchildName.Text;
            gvMultiSelectGrid.DataSource = Data;
            gvMultiSelectGrid.DataBind();

            btnAceptar.Visible = true;
            MPE.OkControlID = "btnCerrar";
            ViewControls();
            pnlContenedor.Visible = true;
            btnAceptar.Focus();
        }
        private void ViewControls()
        {
            gvMultiSelectGrid.Columns[2].Visible = Enabled;
            pnlBody.Visible = Enabled;
        }


    }
}