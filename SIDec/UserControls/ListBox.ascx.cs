using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIDec.UserControls
{
    public partial class ListBox : UserControl
    {
        public string Label
        {
            set { lblBoxTest.Text = value; }
        }

        public string ControlID
        {
            get
            {
                return (hddId.Value);
            }
            set
            {
                hddId.Value = value;
            }
        }
        public bool Enabled
        {
            get
            {
                return lstBoxTest.Enabled; 
            }
            set
            {
                lblBoxTest.Visible = true;
                lstBoxTest.Visible = value;
                txtBoxTest.Visible = !value;
                rfv_BoxTest.Enabled = rfv_BoxTest.ValidationGroup.Trim() != "" && value;
            }
        }
        public string ValidationGroup
        {
            set
            {
                rfv_BoxTest.ValidationGroup = value;
                rfv_BoxTest.Enabled = value.Trim() != "";
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterScripts();
        }

        private void RegisterScripts()
        {
            string key = ControlID+".sumoSelect";
            StringBuilder scriptGantt = new StringBuilder();
            scriptGantt.Append(" <script type='text/javascript'> ");
            scriptGantt.Append("    $(document).ready(function() { ");
            scriptGantt.Append("        $('#" + lstBoxTest.ClientID + "').SumoSelect({ placeholder: '--Seleccione opción',csvDispCount: 100}); ");
            scriptGantt.Append("    }); ");
            scriptGantt.Append(" </script> ");

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), key, scriptGantt.ToString(), false);
        }

        public new string ToString()
        {
            string selectedText = string.Empty;
            foreach (ListItem li in lstBoxTest.Items)
            {
                if (li.Selected == true)
                {
                    selectedText += "," + li.Text;
                }
            }
            return selectedText == string.Empty ? "" : selectedText.Substring(1);
        }

        public string GetSelectedValues()
        {
            string selectedValues = string.Empty;
            foreach (ListItem li in lstBoxTest.Items)
            {
                if (li.Selected == true)
                {
                    selectedValues += ","+li.Value;
                }
            }
            return selectedValues == string.Empty ? "" : selectedValues.Substring(1);
        }

        public void SetSelectedValues(List<string> selectedValues)
        {
            lstBoxTest.ClearSelection();
            if (selectedValues != null)
            {
                foreach (string identidad in selectedValues)
                {
                    foreach (ListItem li in lstBoxTest.Items)
                    {
                        if (li.Value == identidad)
                        {
                            li.Selected = true;
                        }
                    }
                }
            }

            txtBoxTest.Text = ToString();
        }

        public void LoadListBox(DataSet ds, string DataText = "nombre_identidad", string DataValue = "id_identidad")
        {
            lstBoxTest.DataSource = ds;
            lstBoxTest.DataTextField = DataText;
            lstBoxTest.DataValueField = DataValue;
            lstBoxTest.DataBind();
            RegisterScripts();
        }
    }
}