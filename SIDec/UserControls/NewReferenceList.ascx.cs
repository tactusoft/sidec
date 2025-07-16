using System;
using System.Data;
using System.Web;

namespace SIDec.UserControls
{

    public partial class NewReferenceList : System.Web.UI.UserControl
    {
        public string Code
        {
            get { return txt_code.Visible? txt_code.Text: ddl_code.SelectedValue; }
        }
        public string Name
        {
            get { return txt_name.Text; }
        }

        public delegate void OnAcceptEventHandler(object sender);
        public event OnAcceptEventHandler Accept;

        protected void Page_Load(object sender, EventArgs e)
        {
            btnAceptar.Focus();
        }

        public void ShowModal(string pTitle, string pColumnCode, string pColumnName, bool hasCode = false, bool requiredCode = true, DataSet dataset = null, string TextField = "nombre_identidad",  string ValueField = "id_identidad")
        {
            lblTitle.Text = pTitle;

            lblCode.Visible = hasCode;
            txt_code.Visible = hasCode && dataset == null;
            txt_code.Text = "";
            txt_name.Text = "";

            if (dataset != null)
            {
                ddl_code.Visible = hasCode;
                ddl_code.DataSource = dataset;
                ddl_code.DataTextField = TextField;
                ddl_code.DataValueField = ValueField;
                ddl_code.DataBind();
                ddl_code.SelectedIndex = 0;
                rfv_code.ControlToValidate = "ddl_code";
            }
            rfv_code.Enabled = requiredCode && hasCode;
            lblCode.Text = pColumnCode;
            lblName.Text = pColumnName;

            btnAceptar.Visible = true;
            MPE.OkControlID = "btnCerrar";
            pnlContenedor.Visible = true;
            btnAceptar.Focus();
            MPE.Show();
        }


        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            MPE.OkControlID = "btnCerrar";
            pnlContenedor.Visible = false;
            btnAceptar.Focus();
            MPE.Hide();
            Accept?.Invoke(this);
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            pnlContenedor.Visible = false;
            MPE.Hide();
        }
        protected void btnCerrar_Click(object sender, EventArgs e)
        {
        }
    }
}