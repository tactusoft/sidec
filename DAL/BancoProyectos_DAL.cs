using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class BANCOPROYECTOS_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "BANCOPROYECTOS_DAL";
		private const string TABLA_BANCO_PROYECTOS = "BancoProyectos";

		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clDB oDB = new clDB();

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataTable oDataTable;
		private DataSet oDataSet;

		public BANCOPROYECTOS_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}

		public DataSet sp_s_banco_listar(string p_filtro, string p_usuario, string p_isPA)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                oDB.MySQLAddParameter(MySqlCmd, "p_filtro", p_filtro);
				oDB.MySQLAddParameter(MySqlCmd, "p_usuario", p_usuario);
				oDB.MySQLAddParameter(MySqlCmd, "p_isPA", p_isPA);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_BANCO_PROYECTOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		public DataSet sp_s_banco_consultar(int p_idbanco, int p_idproyecto)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                oDB.MySQLAddParameter(MySqlCmd, "p_idbanco", p_idbanco);
				oDB.MySQLAddParameter(MySqlCmd, "p_idproyecto", p_idproyecto);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_BANCO_PROYECTOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		#region sp_i_banco
		public string sp_i_banco(
			string p_idtipo_proyecto,
			string p_idprioridad,
			string p_nombre,
			string p_idestado_proyecto,
			string p_descripcion,
			string p_fec_radicacion,
			string p_idlocalidad,
			string p_idupz,
			string p_idupl,
			string p_idinstrumento,
			string p_idvinculacion,
			string p_poblacion_beneficiaria,
			string p_area_bruta,
			string p_area_neta,
			string p_area_suelo_util,
			string p_viviendas_vip,
			string p_viviendas_vis,
			string p_viviendas_novip,
			string p_cesion_parque,
			string p_cesion_equipamiento,
			string p_fec_inicio_ventas,
			string p_fec_inicio_construccion,
			string p_idproyecto,
			string p_cod_usu_responsable
		)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                oDB.MySQLAddParameter(MySqlCmd, "p_idtipo_proyecto", p_idtipo_proyecto);
				oDB.MySQLAddParameter(MySqlCmd, "p_idprioridad", p_idprioridad);
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre", p_nombre);
				oDB.MySQLAddParameter(MySqlCmd, "p_idestado_proyecto", p_idestado_proyecto);
				oDB.MySQLAddParameter(MySqlCmd, "p_descripcion", p_descripcion);
				
				oDB.MySQLAddParameter(MySqlCmd, "p_fec_radicacion", p_fec_radicacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_idlocalidad", p_idlocalidad);
				oDB.MySQLAddParameter(MySqlCmd, "p_idupz", p_idupz == "0" ? null : p_idupz);
				oDB.MySQLAddParameter(MySqlCmd, "p_idupl", p_idupl);				
				oDB.MySQLAddParameter(MySqlCmd, "p_idinstrumento", p_idinstrumento);

				oDB.MySQLAddParameter(MySqlCmd, "p_idvinculacion", p_idvinculacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_poblacion_beneficiaria", p_poblacion_beneficiaria);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_bruta", p_area_bruta);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_neta", p_area_neta);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_suelo_util", p_area_suelo_util);

				oDB.MySQLAddParameter(MySqlCmd, "p_viviendas_vip", p_viviendas_vip);
				oDB.MySQLAddParameter(MySqlCmd, "p_viviendas_vis", p_viviendas_vis);
				oDB.MySQLAddParameter(MySqlCmd, "p_viviendas_novip", p_viviendas_novip);
				oDB.MySQLAddParameter(MySqlCmd, "p_cesion_parque", p_cesion_parque);
				oDB.MySQLAddParameter(MySqlCmd, "p_cesion_equipamiento", p_cesion_equipamiento);

				oDB.MySQLAddParameter(MySqlCmd, "p_fec_inicio_ventas", p_fec_inicio_ventas);
				oDB.MySQLAddParameter(MySqlCmd, "p_fec_inicio_construccion", p_fec_inicio_construccion);
				oDB.MySQLAddParameter(MySqlCmd, "p_idproyecto", p_idproyecto);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu_responsable", p_cod_usu_responsable);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "Error:" + Error.Message;
			}
		}
		#endregion

		public string sp_d_banco(string p_idbanco)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                oDB.MySQLAddParameter(MySqlCmd, "p_idbanco", p_idbanco);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "Error:" + Error.Message;
			}
		}

		public string sp_u_banco_reactivar(string p_idbanco)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                oDB.MySQLAddParameter(MySqlCmd, "p_idbanco", p_idbanco);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "Error:" + Error.Message;
			}
		}

		public DataSet sp_s_banco_reporte()
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_BANCO_PROYECTOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		public DataSet sp_rpt_alertas(bool p_activas)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
				{
					CommandType = CommandType.StoredProcedure
				};
				oDB.MySQLAddParameter(MySqlCmd, "p_activas", p_activas);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_BANCO_PROYECTOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		#region sp_u_banco
		public string sp_u_banco(
			string p_idbanco,
			string p_idtipo_proyecto,
			string p_idprioridad,
			string p_nombre,
			string p_idestado_proyecto,
			string p_descripcion,
			string p_fec_radicacion,
			string p_idlocalidad,
			string p_idupz,
			string p_idupl,
			string p_idinstrumento,
			string p_idvinculacion,
			string p_poblacion_beneficiaria,
			string p_area_bruta,
			string p_area_neta,
			string p_area_suelo_util,
			string p_viviendas_vip,
			string p_viviendas_vis,
			string p_viviendas_novip,
			string p_cesion_parque,
			string p_cesion_equipamiento,
			string p_fec_inicio_ventas,
			string p_fec_inicio_construccion,
			string p_cod_usu_responsable
		)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                oDB.MySQLAddParameter(MySqlCmd, "p_idbanco", p_idbanco);
				oDB.MySQLAddParameter(MySqlCmd, "p_idtipo_proyecto", p_idtipo_proyecto);
				oDB.MySQLAddParameter(MySqlCmd, "p_idprioridad", p_idprioridad);
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre", p_nombre);
				oDB.MySQLAddParameter(MySqlCmd, "p_idestado_proyecto", p_idestado_proyecto);

				oDB.MySQLAddParameter(MySqlCmd, "p_descripcion", p_descripcion);
				oDB.MySQLAddParameter(MySqlCmd, "p_fec_radicacion", p_fec_radicacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_idlocalidad", p_idlocalidad);
				oDB.MySQLAddParameter(MySqlCmd, "p_idupz", p_idupz == "0" ? null : p_idupz);
				oDB.MySQLAddParameter(MySqlCmd, "p_idupl", p_idupl);

				oDB.MySQLAddParameter(MySqlCmd, "p_idinstrumento", p_idinstrumento);
				oDB.MySQLAddParameter(MySqlCmd, "p_idvinculacion", p_idvinculacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_poblacion_beneficiaria", p_poblacion_beneficiaria);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_bruta", p_area_bruta);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_neta", p_area_neta);

				oDB.MySQLAddParameter(MySqlCmd, "p_area_suelo_util", p_area_suelo_util);
				oDB.MySQLAddParameter(MySqlCmd, "p_viviendas_vip", p_viviendas_vip);
				oDB.MySQLAddParameter(MySqlCmd, "p_viviendas_vis", p_viviendas_vis);
				oDB.MySQLAddParameter(MySqlCmd, "p_viviendas_novip", p_viviendas_novip);
				oDB.MySQLAddParameter(MySqlCmd, "p_cesion_parque", p_cesion_parque);

				oDB.MySQLAddParameter(MySqlCmd, "p_cesion_equipamiento", p_cesion_equipamiento);
				oDB.MySQLAddParameter(MySqlCmd, "p_fec_inicio_ventas", p_fec_inicio_ventas);
				oDB.MySQLAddParameter(MySqlCmd, "p_fec_inicio_construccion", p_fec_inicio_construccion);			
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu_responsable", p_cod_usu_responsable);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "Error:" + Error.Message;
			}
		}
		#endregion


		public DataSet sp_s_banco_colaboradores(string p_idbanco)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                oDB.MySQLAddParameter(MySqlCmd, "p_idbanco", p_idbanco);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_BANCO_PROYECTOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		public string sp_i_banco_usuario_colaborador(
			int p_idbanco,
			string p_cod_usu_colaborador
		)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                oDB.MySQLAddParameter(MySqlCmd, "p_idbanco", p_idbanco);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu_colaborador", p_cod_usu_colaborador);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "Error:" + Error.Message;
			}
		}
		public string sp_d_banco_usuario_colaborador(int p_idbanco)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                oDB.MySQLAddParameter(MySqlCmd, "p_idbanco", p_idbanco);

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "Error:" + Error.Message;
			}
		}


		#region-----DISPOSE
		// Metodo para el manejo del GC
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}

		~BANCOPROYECTOS_DAL()
		{
			// Finalizer calls Dispose(false)
			Dispose(false);
		}

		// Free the instance variables of this object.
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