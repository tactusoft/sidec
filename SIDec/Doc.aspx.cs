using System;
using System.Net;
using System.IO;
using GLOBAL.VAR;

namespace SIDec
{
    public partial class Doc : System.Web.UI.Page
    {
        clGlobalVar oVar = new clGlobalVar();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (oVar.prFullPathDoc != null)
            {
                string FilePath = oVar.prFullPathDoc.ToString().Replace("\\\\", "/").Replace("\\", "/");
                if (File.Exists(FilePath))
                {
                    string name = FilePath.Substring(FilePath.LastIndexOf("/") + 1);
                    string extension = FilePath.Substring(FilePath.LastIndexOf(".") + 1).ToLower();
                    switch (extension)
                    {
                        case "pdf": Response.ContentType = "application/pdf"; break;
                        case "jpeg":
                        case "jpg": Response.ContentType = "image/jpeg"; break;
                        case "png": Response.ContentType = "image/png"; break;
                        default: Response.ContentType = "multipart/form-data";
                            Response.AddHeader("Content-Disposition", "attachment; filename= " + name); break;
                    }
                    if (File.Exists(FilePath))
                    {
                        WebClient client = new WebClient();
                        byte[] buffer = client.DownloadData(oVar.prFullPathDoc.ToString());
                        Response.AddHeader("content-length", buffer.Length.ToString());
                        Response.BinaryWrite(buffer);
                    }
                }
            }
        }
    }
}