using GLOBAL.CONST;
using GLOBAL.DAL;
using GLOBAL.LOG;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using System;
using System.Data;
using System.Web.UI;

namespace GLOBAL.AUTENTICACION
{
    public class clAutentica : Page
    {
        private const string _TABLAUSUARIOS = "Usuarios";
        private int CodAutorizacion = -10;

        USUARIOS_DAL oUsuario = new USUARIOS_DAL();
        clGlobalVar oVar = new clGlobalVar();

        clLog oLog = new clLog();
        clConstantes oConst = new clConstantes();
        clUtil oUtil = new clUtil();

        DataSet dsIP = new DataSet();
        DataSet dsUsuario = new DataSet();
        DataSet dsPerfil = new DataSet();

        public int Autenticacion(string p_cod_usuario, string p_pass)
        {
            dsUsuario = oUsuario.sp_s_usuario_usuario(p_cod_usuario, p_pass);
            if (dsUsuario.Tables[_TABLAUSUARIOS].Rows.Count == 0)  //No existe
            {
                CodAutorizacion = (int)clConstantes.Autenticacion.NoExisteUsuario;
            }
            else //Existe 
            {        
                CodAutorizacion = (int)clConstantes.Autenticacion.Autenticado;
                oVar.prUserCod = dsUsuario.Tables[_TABLAUSUARIOS].Rows[0]["cod_usuario"].ToString();
                oVar.prUser = dsUsuario.Tables[_TABLAUSUARIOS].Rows[0]["usuario"].ToString();
                oVar.prUserName = dsUsuario.Tables[_TABLAUSUARIOS].Rows[0]["nombre_completo"].ToString();
                oVar.prUserCargo = dsUsuario.Tables[_TABLAUSUARIOS].Rows[0]["nombre_cargo"].ToString();
                oVar.prUserCambiarPW = Convert.ToBoolean(dsUsuario.Tables[_TABLAUSUARIOS].Rows[0]["cambiar_pw"]);

                oVar.prUserRevisaGestion = dsUsuario.Tables[_TABLAUSUARIOS].Rows[0]["revisa_gestion"].ToString();
                oVar.prUserEditaActos = dsUsuario.Tables[_TABLAUSUARIOS].Rows[0]["edita_actos"].ToString();
                oVar.prUserEditaDocumentos = dsUsuario.Tables[_TABLAUSUARIOS].Rows[0]["edita_documentos"].ToString();
                oVar.prUserEliminaDocumentos = dsUsuario.Tables[_TABLAUSUARIOS].Rows[0]["elimina_documentos"].ToString();
                oVar.prUserAsignaUsuarioPredios = dsUsuario.Tables[_TABLAUSUARIOS].Rows[0]["asigna_usuario_predios"].ToString();
                oVar.prUserRecibePrestamos = dsUsuario.Tables[_TABLAUSUARIOS].Rows[0]["recibe_prestamos"].ToString();
            }
            return CodAutorizacion;
        }
    }
}