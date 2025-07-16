using System;
using System.Data;
using System.Web;

namespace SIDec.UserControls
{
    public partial class TextBoxGrid : System.Web.UI.UserControl
    {
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

        public new string ToString()
        {
            string values = "";
            if (Session[ClientID + ".Grid.Values"] == null)
                return null;

            DataTable dt = (DataTable)Session[ClientID + ".Grid.Values"];
            foreach (DataRow dr in dt.Rows)
            {
                values += "; " + dr["value"].ToString();
            }

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

        public void ShowModal(string pTitle, string pColumnName, string pValues = null)
        {
            lblTitle.Text = pTitle;
            lblColumnName.Text = pColumnName;

            Session[ClientID + ".Grid.Values"] = null;

            DataTable dt = new DataTable("dtValues");
            if (dt.Columns.Count == 0)
                dt.Columns.Add("value");

            foreach(string value in (pValues??"").Split(';'))
            {
                if(value.Trim() != string.Empty)
                {
                    DataRow dr = dt.NewRow();
                    dr["value"] = value.Trim();
                    dt.Rows.Add(dr);
                }
            }
            Session[ClientID + ".Grid.Values"] = dt;
            gvTextBoxGrid.DataSource = dt;
            gvTextBoxGrid.DataBind();

            btnAceptar.Visible = true;
            MPE.OkControlID = "btnCerrar";
            ViewControls();
            pnlContenedor.Visible = true;
            btnAceptar.Focus();
            MPE.Show();
        }


        private void ViewControls()
        {
            gvTextBoxGrid.Columns[1].Visible = Enabled;
            pnlBody.Visible = Enabled;
        }


        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)(Session[ClientID + ".Grid.Values"] ?? new DataTable("dtValues"));
            if (dt.Columns.Count == 0)
                dt.Columns.Add("value");

            foreach (string value in (txt_value.Text.Trim() ?? "").Split(';'))
            {
                if (value.Trim() != string.Empty)
                {
                    DataRow dr = dt.NewRow();
                    dr["value"] = value.Trim();
                    dt.Rows.Add(dr);
                }
            }

            Session[ClientID + ".Grid.Values"] = dt;

            gvTextBoxGrid.DataSource = dt;
            gvTextBoxGrid.DataBind();

            MPE.OkControlID = "btnCerrar";
            pnlContenedor.Visible = true;
            btnAceptar.Focus();
            txt_value.Text = "";
            MPE.Show();
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

        protected void gvTextBoxGrid_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int rowIndex;
            if (int.TryParse(e.CommandArgument.ToString(), out rowIndex))
            {
                if (rowIndex >= gvTextBoxGrid.Rows.Count)
                    rowIndex = 0;
                
                switch (e.CommandName)
                {
                    case "_Delete":
                        DataTable dt = (DataTable)Session[ClientID + ".Grid.Values"];
                        dt.Rows[rowIndex].Delete();
                        Session[ClientID + ".Grid.Values"] = dt;
                        gvTextBoxGrid.DataSource = dt;
                        gvTextBoxGrid.DataBind();
                        break;
                    default:
                        return;
                }
            }
            MPE.Show();
            Remove?.Invoke(this);
        }
    }
}