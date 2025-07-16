using GLOBAL.VAR;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace GLOBAL.PERMISOS
{
    public class clPermisos : IDisposable
	{
		clGlobalVar oVar = new clGlobalVar();
		private const string NO_RESPONSIBLE = "0";


		public bool bPermisoMenu(string objeto_permiso)
		{
			DataRow[] result = ((DataSet)(oVar.prPermisosUsuario)).Tables[0].Select(string.Format("objeto_permiso='{0}'", objeto_permiso));
			if (result.Count() > 0)
				return true;
			else
				return false;
		}

		public bool PermisosSP(string sp)
		{
			string strQuery = string.Format("objeto_permiso = '{0}'", sp);
			DataRow[] oDr = ((DataSet)oVar.prPermisosUsuario).Tables[0].Select(strQuery);
			if (oDr.Count() > 0)
				return true;
			return false;
		}

		public string GetEstiloPermisosSP(string sp)
		{
			string strQuery = string.Format("objeto_permiso = '{0}'", sp);
			DataRow[] oDr = ((DataSet)oVar.prPermisosUsuario).Tables[0].Select(strQuery);
			if (oDr.Count() > 0)
				return ConfigurationManager.AppSettings["EstiloBotonHabilitado"];
			return ConfigurationManager.AppSettings["EstiloBotonDeshabilitado"];
		}

		public bool TienePermisosSP(string sp)
		{
			if (string.IsNullOrEmpty(oVar.prUser.ToString()))
			{
				HttpResponse objResponse = null;
				objResponse.Redirect("Login");
				return false;
			}
			else
			{
				string strQuery = string.Format("objeto_permiso = '{0}'", sp);
				DataRow[] oDr = ((DataSet)oVar.prPermisosUsuario).Tables[0].Select(strQuery);
				if (oDr.Count() > 0)
					return true;
				return false;
			}
		}

		public bool TienePermisosAccion(int menu, string action, string owner = "", string customer = "")
		{
			return TienePermisosAccion(menu.ToString(), action, owner, customer);
		}

		public bool TienePermisosAccion(string menu, string action, string owner = "", string customer = "")
		{
			if (string.IsNullOrEmpty(oVar.prUser.ToString()))
			{
				HttpResponse objResponse = null;
				objResponse.Redirect("Login");
				return false;
			}
			else
			{
				string strQuery = string.Format("objeto_permiso = '{0}' and {1} = 2", menu, action);
				DataRow[] oDr = ((DataSet)oVar.prPermisosUsuario).Tables[0].Select(strQuery);
				if (oDr.Count() > 0)
					return true;
				else
				{
					string strQuery2 = string.Format("objeto_permiso = '{0}' and {1} = 1", menu, action);
					DataRow[] oDr2 = ((DataSet)oVar.prPermisosUsuario).Tables[0].Select(strQuery2);
					if (oDr2.Count() > 0 && owner == customer)
						return true;
					return false;
				}
			}
		}


		public string ValidateAccess(int section, string action, string cod_usu_responsable, bool validateResponsible = false, bool requeridedResponsible = true)
		{
			return ValidateAccess(section.ToString(), action, cod_usu_responsable, validateResponsible, requeridedResponsible);
		}
		public string ValidateAccess(string section, string action, string cod_usu_responsable, bool validateResponsible = false, bool requeridedResponsible = true)
		{
			if (TienePermisosAccion(section, action, "", oVar.prUserCod.ToString()))
				return "";

			if (cod_usu_responsable == NO_RESPONSIBLE && requeridedResponsible)
			{
				return "Esta acción requiere que se asigne un responsable";
			}

			if (!TienePermisosAccion(section, action))
			{
				return "No cuenta con permisos suficientes para realizar esta acción";
			}

			if(!validateResponsible || cod_usu_responsable == NO_RESPONSIBLE )
				return "";
			
			if (!TienePermisosAccion(section, action, cod_usu_responsable, oVar.prUserCod.ToString()))
			{	
				if(cod_usu_responsable.Contains("-"))
					return "Este registro no es editable. Si requiere su habilitación, por favor comunicarse con el administrador";
				return "Para realizar esta acción comuníquese con el usuario responsable del proyecto";
			}
			return "";
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// dispose managed resources
				oVar.Dispose();
			}
			// free native resources
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}