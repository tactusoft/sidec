using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SIDec.UserControls
{

    public partial class MessageBox : System.Web.UI.UserControl
    {

        clBasic oBasic = new clBasic();
        private static string Key { get; set; }
        public delegate void OnYesEventHandler(object sender, string e);
        public event OnYesEventHandler Yes;
        public delegate void OnNoEventHandler(object sender, string e);
        public event OnNoEventHandler No;

        public delegate void OnAcceptEventHandler(string e);
        public event OnAcceptEventHandler Accept;
        public delegate void OnCancelEventHandler(object sender, string e);
        public event OnCancelEventHandler Cancel;


        protected void Page_Load(object sender, EventArgs e)
        {
            btnAceptar.Focus();
        }
        /// <summary>
        /// Modal para presentar mensajes de información o alerta
        /// </summary>
        /// <param name="pMessage">Mensaje a mostrar</param>
        /// <param name="title">Valor del título del modal - por defecto «Confirmar acción»</param>
        /// <param name="type">info (default), warning, danger</param>
        public void ShowMessage(string pMessage, string title = null, string type = "info")
        {
            lblTexto.Text = pMessage;
            lblTitle.Text = title ?? "Validación"; //"Confirmar acción";
            btnCancelar.Visible = false;
            MPE.OkControlID = "btnAceptar";
            SetStyle(type);
            pnlContenedor.Visible = true;
            btnAceptar.Focus();
            MPE.Show();
        }
        public void ShowConfirmation(string pKey, string pMessage = null, string title = null, string type = "info", bool letHTML = false)
        {
            Session[HttpUtility.UrlDecode(Request.RawUrl + "KeyMsgBox")] = pKey;
            lblTexto.Text = letHTML? pMessage : HttpUtility.HtmlEncode(pMessage) ?? lblTexto.Text;
            lblTitle.Text = title ?? "Confirmar acción";
            btnCancelar.Visible = true;
            btnAceptar.Visible = true;
            MPE.OkControlID = "btnCerrar";
            SetStyle(type);
            pnlContenedor.Visible = true;
            btnAceptar.Focus();
            MPE.Show();
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            if (Session[HttpUtility.UrlDecode(Request.RawUrl + "KeyMsgBox")] != null)
                Key = Session[HttpUtility.UrlDecode(Request.RawUrl + "KeyMsgBox")].ToString();
            Session.Remove(Request.RawUrl + "KeyMsgBox");
            pnlContenedor.Visible = false;
            MPE.Hide();
            if (Yes != null)
            {
                Yes(this, Key);
            }
            else
            {
                Accept?.Invoke( Key);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (Session[Request.RawUrl + "KeyMsgBox"] != null)
                Key = Session[Request.RawUrl + "KeyMsgBox"].ToString();
            Session.Remove(Request.RawUrl + "KeyMsgBox");
            pnlContenedor.Visible = false;
            MPE.Hide();
            if (No != null)
            {
                No(this, Key);
            }
            else
            {
                Cancel?.Invoke(this, Key);
            }
        }
        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            Session.Remove(Request.RawUrl + "KeyMsgBox");
        }
        private void SetStyle(string type) 
        {
            oBasic.ClassSet(divHeader, 3, "modal-header modal-bg-" + type);
            oBasic.ClassSet(btnAceptar, 3, "btn btn-outline-" + type);
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<i class='fas fa-check'></i>&nbsp&nbsp;";
            Label lbl = new Label { Text = "Aceptar" };
            btnAceptar.Controls.Clear();
            btnAceptar.Controls.Add(html);
            btnAceptar.Controls.Add(lbl);
        }
    }
}