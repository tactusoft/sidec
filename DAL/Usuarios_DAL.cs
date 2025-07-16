using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class USUARIOS_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "USUARIOS_DAL";
		private const string TABLA_USUARIOS = "Usuarios";

		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clDB oDB = new clDB();

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataTable oDataTable;
		private DataSet oDataSet;

		public USUARIOS_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}
		public DataSet sp_s_usuarios_filtro(int p_filtro, int p_usuario_cargado)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_filtro", p_filtro);
				oDB.MySQLAddParameter(MySqlCmd, "p_usuario_cargado", p_usuario_cargado);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_USUARIOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public DataSet sp_s_usuarios_acceso_opcion(string p_opcion)
        {
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_opcion", p_opcion);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_USUARIOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		public DataSet sp_s_usuario(string p_idusuario)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idusuario", p_idusuario);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_USUARIOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public DataSet sp_s_usuarios(int p_habilitado, string p_nombre)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_habilitado", p_habilitado);
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre", p_nombre);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_USUARIOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public DataSet sp_s_usuario_usuario(string p_usuario, string p_password)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_usuario", p_usuario);
				oDB.MySQLAddParameter(MySqlCmd, "p_password", p_password, "pass");

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_USUARIOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public string sp_i_usuario(
				string p_usuario,
				string p_nombre_usuario,
				string p_apellido_usuario,
				string p_cod_cargo_usuario,
				string p_id_area_entidad,
				string p_matricula_usuario,
				string p_correo_usuario,
				string p_password,
				bool p_habilitado,
				bool p_asigna_usuario_predios,
				bool p_edita_actos,
				bool p_edita_documentos,
				bool p_elimina_documentos,
				bool p_recibe_prestamos,
				bool p_revisa_gestion)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_usuario", p_usuario);
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre_usuario", p_nombre_usuario);
				oDB.MySQLAddParameter(MySqlCmd, "p_apellido_usuario", p_apellido_usuario);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_cargo_usuario", p_cod_cargo_usuario);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_area_entidad", p_id_area_entidad);
				oDB.MySQLAddParameter(MySqlCmd, "p_matricula_usuario", p_matricula_usuario);
				oDB.MySQLAddParameter(MySqlCmd, "p_correo_usuario", p_correo_usuario);
				oDB.MySQLAddParameter(MySqlCmd, "p_password", p_password, "pass");
				oDB.MySQLAddParameter(MySqlCmd, "p_habilitado", p_habilitado);
				oDB.MySQLAddParameter(MySqlCmd, "p_asigna_usuario_predios", p_asigna_usuario_predios);
				oDB.MySQLAddParameter(MySqlCmd, "p_edita_actos", p_edita_actos);
				oDB.MySQLAddParameter(MySqlCmd, "p_edita_documentos", p_edita_documentos);
				oDB.MySQLAddParameter(MySqlCmd, "p_elimina_documentos", p_elimina_documentos);
				oDB.MySQLAddParameter(MySqlCmd, "p_recibe_prestamos", p_recibe_prestamos);
				oDB.MySQLAddParameter(MySqlCmd, "p_revisa_gestion", p_revisa_gestion);

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "ERR12";
			}
		}

		public string sp_iu_usuarioperfil(
				string p_idusuario,
				string p_idsperfil)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idusuario", p_idusuario);
				oDB.MySQLAddParameter(MySqlCmd, "p_idsperfil", p_idsperfil);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "ERR12";
			}
		}

		public string sp_u_usuario(
				string p_cod_usuario,
				string p_usuario,
				string p_nombre_usuario,
				string p_apellido_usuario,
				string p_cod_cargo_usuario,
				string p_id_area_entidad,
				string p_matricula_usuario,
				string p_correo_usuario,
				bool p_habilitado,
				bool p_asigna_usuario_predios,
				bool p_edita_actos,
				bool p_edita_documentos,
				bool p_elimina_documentos,
				bool p_recibe_prestamos,
				bool p_revisa_gestion)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usuario", p_cod_usuario);
				oDB.MySQLAddParameter(MySqlCmd, "p_usuario", p_usuario);
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre_usuario", p_nombre_usuario);
				oDB.MySQLAddParameter(MySqlCmd, "p_apellido_usuario", p_apellido_usuario);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_cargo_usuario", p_cod_cargo_usuario);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_area_entidad", p_id_area_entidad);
				oDB.MySQLAddParameter(MySqlCmd, "p_matricula_usuario", p_matricula_usuario);
				oDB.MySQLAddParameter(MySqlCmd, "p_correo_usuario", p_correo_usuario);
				oDB.MySQLAddParameter(MySqlCmd, "p_habilitado", p_habilitado);
				oDB.MySQLAddParameter(MySqlCmd, "p_asigna_usuario_predios", p_asigna_usuario_predios);
				oDB.MySQLAddParameter(MySqlCmd, "p_edita_actos", p_edita_actos);
				oDB.MySQLAddParameter(MySqlCmd, "p_edita_documentos", p_edita_documentos);
				oDB.MySQLAddParameter(MySqlCmd, "p_elimina_documentos", p_elimina_documentos);
				oDB.MySQLAddParameter(MySqlCmd, "p_recibe_prestamos", p_recibe_prestamos);
				oDB.MySQLAddParameter(MySqlCmd, "p_revisa_gestion", p_revisa_gestion);

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "ERR12";
			}
		}

		public string sp_d_usuario(string p_cod_usuario)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usuario", p_cod_usuario);

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "ERR12";
			}
		}

		public string sp_u_usuario_password(
				string p_usuario,
				string p_password,
				string p_old_password,
				bool p_reset)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_usuario", p_usuario);
				oDB.MySQLAddParameter(MySqlCmd, "p_password", p_password, "pass");
				oDB.MySQLAddParameter(MySqlCmd, "p_old_password", p_old_password, "pass");
				oDB.MySQLAddParameter(MySqlCmd, "p_reset", p_reset);

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "ERR12";
			}
		}
		public string sp_u_usuario_resetpassword(string p_cod_usuario, string password)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usuario", p_cod_usuario);
				oDB.MySQLAddParameter(MySqlCmd, "p_password", password, "pass");

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "ERR12";
			}
		}
		public DataSet sp_rpt_gestion_usuarios(
				string p_fecha_ini,
				string p_fecha_fin,
				string p_cod_usu)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_ini", p_fecha_ini);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_fin", p_fecha_fin);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", p_cod_usu);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_USUARIOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		#region-----DISPOSE
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}
		~USUARIOS_DAL()
		{
			Dispose(false);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				MySqlConn.Dispose();
				MySqlConn = null;

				MySqlDA.Dispose();
				MySqlDA = null;

				oDataSet.Dispose();
				oDataSet = null;

				oDataTable.Dispose();
				oDataTable = null;
			}
		}
		#endregion
	}
}