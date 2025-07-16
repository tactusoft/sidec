using System;
using System.Web;

namespace GLOBAL.VAR
{
	[Serializable()]
	public class clGlobalVar : IDisposable
	{
		public clGlobalVar()
		{
		}

		#region-------------------------------------------------------------------- info usuario
		public object prUserCod
		{
			get
			{
				if (HttpContext.Current == null)
					return "";
				return HttpContext.Current.Session["UserCod"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["UserCod"] = value;
			}
		}

		public object prUser
		{
			get
			{
				if (HttpContext.Current == null)
					return "";
				return HttpContext.Current.Session["User"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["User"] = value;
			}
		}

		public object prUserName
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["UserName"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["UserName"] = value;
				}
			}
		}

		public object prUserRevisaGestion
		{
			get
			{
				if (HttpContext.Current == null)
					return "";
				return HttpContext.Current.Session["prUserRevisaGestion"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["prUserRevisaGestion"] = value;
			}
		}

		public object prUserEditaActos
		{
			get
			{
				if (HttpContext.Current == null)
					return "";
				return HttpContext.Current.Session["prUserEditaActos"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["prUserEditaActos"] = value;
			}
		}

		public object prUserEditaDocumentos
		{
			get
			{
				if (HttpContext.Current == null)
					return "";
				return HttpContext.Current.Session["prUserEditaDocumentos"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["prUserEditaDocumentos"] = value;
			}
		}

		public object prUserEliminaDocumentos
		{
			get
			{
				if (HttpContext.Current == null)
					return "";
				return HttpContext.Current.Session["prUserEliminaDocumentos"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["prUserEliminaDocumentos"] = value;
			}
		}

		public object prUserAsignaUsuarioPredios
		{
			get
			{
				if (HttpContext.Current == null)
					return "";
				return HttpContext.Current.Session["prUserAsignaUsuarioPredios"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["prUserAsignaUsuarioPredios"] = value;
			}
		}

		public object prUserRecibePrestamos
		{
			get
			{
				if (HttpContext.Current == null)
					return "";
				return HttpContext.Current.Session["prUserRecibePrestamos"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["prUserRecibePrestamos"] = value;
			}
		}

		public object prPermisosUsuario
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["PermisosSession"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["PermisosSession"] = value;
				}
			}
		}

		public object prUserCargo
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["UserCargo"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["UserCargo"] = value;
				}
			}
		}

		public object prUserCambiarPW
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["CambiarPW"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["CambiarPW"] = value;
				}
			}
		}

		public object prUserResponsablePredioDec
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["UserResponsablePredioDec"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["UserResponsablePredioDec"] = value;
				}
			}
		}
		#endregion

		#region-------------------------------------------------------------------- sesion
		public object prIP
		{
			get
			{
				if (HttpContext.Current.Session == null)
				{
					//return null;
					return "0";
				}
				return HttpContext.Current.Session["IPSource"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["IPSource"] = value;
				}
			}
		}

		public object prPCInfo
		{
			get
			{
				if (HttpContext.Current.Session == null)
				{
					//return null;
					return "0";
				}
				return HttpContext.Current.Session["PCInfo"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["PCInfo"] = value;
				}
			}
		}

		public object prViewState
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["ViewState"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["ViewState"] = value;
				}
			}
		}
		#endregion

		#region-------------------------------------------------------------------- datasources
		public object prDataSet
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DataSet"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DataSet"] = value;
				}
			}
		}

		public object prDSActosAdm
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSActosAdm"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSActosAdm"] = value;
				}
			}
		}

		public object prDSActosAdmFiltro
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSActosAdmFiltro"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSActosAdmFiltro"] = value;
				}
			}
		}

		public object prDSBarriosInfo
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSBarriosInfo"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSBarriosInfo"] = value;
				}
			}
		}

		public object prDSCargos
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSCargos"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSCargos"] = value;
				}
			}
		}

		public object prDSCargosFiltro
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSCargosFiltro"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSCargosFiltro"] = value;
				}
			}
		}

		public object prDSConceptos
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSConceptos"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSConceptos"] = value;
				}
			}
		}

		public object prDSConceptosFiltro
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSConceptosFiltro"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSConceptosFiltro"] = value;
				}
			}
		}

		public object prDSObservaciones
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSObservaciones"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSObservaciones"] = value;
				}
			}
		}

		public object prDSObservacionesFiltro
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSObservacionesFiltro"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSObservacionesFiltro"] = value;
				}
			}
		}

		public object prDSDeclaratorias
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSDeclaratorias"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSDeclaratorias"] = value;
				}
			}
		}

		public object prDSIdCategorias
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSIdCategorias"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSIdCategorias"] = value;
				}
			}
		}

		public object prDSIdCategoriasFiltro
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSIdCategoriasFiltro"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSIdCategoriasFiltro"] = value;
				}
			}
		}

		public object prDSDocumentos
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSDocumentos"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSDocumentos"] = value;
				}
			}
		}

		public object prDSDocumentosFiltro
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSDocumentosFiltro"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSDocumentosFiltro"] = value;
				}
			}
		}

		public object prDSIdentidadesCat
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSIdentidadesCat"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSIdentidadesCat"] = value;
				}
			}
		}

		public object prDSIdentidades
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSIdentidades"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSIdentidades"] = value;
				}
			}
		}

		public object prDSIdentidadesFiltro
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSIdentidadesFiltro"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSIdentidadesFiltro"] = value;
				}
			}
		}

		public object prDSLicencias
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSLicencias"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSLicencias"] = value;
				}
			}
		}

		public object prDSLicenciasFiltro
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSLicenciasFiltro"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSLicenciasFiltro"] = value;
				}
			}
		}

		public object prDSPerfiles
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSPerfiles"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSPerfiles"] = value;
				}
			}
		}

		public object prDSPredios
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSPredios"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSPredios"] = value;
				}
			}
		}

		public object prDSPrediosFiltro
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSPrediosFiltro"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSPrediosFiltro"] = value;
				}
			}
		}

		public object prDSPrediosDec
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSPrediosDec"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSPrediosDec"] = value;
				}
			}
		}

		public object prDSPrediosDecFiltro
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSPrediosDecFiltro"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSPrediosDecFiltro"] = value;
				}
			}
		}

		public object prDSPrediosPropietarios
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSPrediosPropietarios"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSPrediosPropietarios"] = value;
				}
			}
		}

		public object prDSPrediosPropietariosFiltro
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSPrediosPropietariosFiltro"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSPrediosPropietariosFiltro"] = value;
				}
			}
		}

		public object prDSPropietarios
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSPropietarios"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSPropietarios"] = value;
				}
			}
		}

		public object prDSPropietariosFiltro
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSPropietariosFiltro"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSPropietariosFiltro"] = value;
				}
			}
		}

		public object prDSPrestamos
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSPrestamos"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSPrestamos"] = value;
				}
			}
		}

		public object prDSPrestamosUltimo
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSPrestamosUltimo"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSPrestamosUltimo"] = value;
				}
			}
		}
		public object prDSPlanesP
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSPlanesP"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSPlanesP"] = value;
				}
			}
		}

		public object prDSPlanesPManzanas
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["prDSPlanesPManzanas"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["prDSPlanesPManzanas"] = value;
				}
			}
		}

		public object prDSPlanesPCesiones
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["prDSPlanesPCesiones"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["prDSPlanesPCesiones"] = value;
				}
			}
		}

		public object prDSPlanesPActos
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["prDSPlanesPActos"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["prDSPlanesPActos"] = value;
				}
			}
		}

		public object prDSPlanesPLicencias
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["prDSPlanesPLicencias"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["prDSPlanesPLicencias"] = value;
				}
			}
		}

		public object prDSPlanesPVisitas
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["prDSPlanesPVisitas"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["prDSPlanesPVisitas"] = value;
				}
			}
		}

		public object prDSProyectos
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSProyectos"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSProyectos"] = value;
				}
			}
		}

		public object prDSProyectosPredios
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSProyectosPredios"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSProyectosPredios"] = value;
				}
			}
		}

		public object prDSProyectosCartas
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSProyectosCartas"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSProyectosCartas"] = value;
				}
			}
		}

		public object prDSProyectosLicencias
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSProyectosLicencias"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSProyectosLicencias"] = value;
				}
			}
		}
		public object prDS_rpt_estado_predios
		{
			get
			{
				if (HttpContext.Current == null)
					return null;
				return HttpContext.Current.Session["DS_rpt_estado_predios"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["DS_rpt_estado_predios"] = value;
			}
		}

		public object prDS_rpt_planesp
		{
			get
			{
				if (HttpContext.Current == null)
					return null;
				return HttpContext.Current.Session["DS_rpt_planesp"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["DS_rpt_planesp"] = value;
			}
		}

		public object prDS_rpt_planesp_general
		{
			get
			{
				if (HttpContext.Current == null)
					return null;
				return HttpContext.Current.Session["DS_rpt_planesp_general"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["DS_rpt_planesp_general"] = value;
			}
		}

		public object prDS_rpt_planesp_cesiones
		{
			get
			{
				if (HttpContext.Current == null)
					return null;
				return HttpContext.Current.Session["DS_rpt_planesp_cesiones"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["DS_rpt_planesp_cesiones"] = value;
			}
		}

		public object prDS_rpt_planesp_indicadores
		{
			get
			{
				if (HttpContext.Current == null)
					return null;
				return HttpContext.Current.Session["DS_rpt_planesp_indicadores"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["DS_rpt_planesp_indicadores"] = value;
			}
		}

		public object prDS_rpt_gestion_documentos
		{
			get
			{
				if (HttpContext.Current == null)
					return null;
				return HttpContext.Current.Session["DS_rpt_gestion_documentos"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["DS_rpt_gestion_documentos"] = value;
			}
		}

		public object prDS_rpt_gestion_usuarios
		{
			get
			{
				if (HttpContext.Current == null)
					return null;
				return HttpContext.Current.Session["DS_rpt_gestion_usuarios"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["DS_rpt_gestion_usuarios"] = value;
			}
		}

		public object prDS_rpt_prestamos_abiertos
		{
			get
			{
				if (HttpContext.Current == null)
					return null;
				return HttpContext.Current.Session["DS_rpt_prestamos_abiertos"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["DS_rpt_prestamos_abiertos"] = value;
			}
		}
		public object prDSUsuarios
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSUsuarios"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSUsuarios"] = value;
				}
			}
		}
		public object prDSVisitas
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSVisitas"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSVisitas"] = value;
				}
			}
		}
		public object prDSVisitasFiltro
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSVisitasFiltro"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSVisitasFiltro"] = value;
				}
			}
		}
		public object prDSBanco
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSBanco"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSBanco"] = value;
				}
			}
		}
		public object prDSTracing
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DSTracing"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DSTracing"] = value;
				}
			}
		}

		#endregion

		#region-------------------------------------------------------------------- list / nav active
		public int prItemReportePlanesP
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return 0;
				}
				return Convert.ToInt32(HttpContext.Current.Application["ItemReportePlanesP"]);
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["ItemReportePlanesP"] = value;
				}
			}
		}
		#endregion

		#region-------------------------------------------------------------------- parametros
		public string prDescPredios
		{
			get
			{
				if (HttpContext.Current == null)
					return null;
				return HttpContext.Current.Application["DescPredios"].ToString();
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Application["DescPredios"] = value;
			}
		}

		public string prDescPlanesP
		{
			get
			{
				if (HttpContext.Current == null)
					return null;
				return HttpContext.Current.Application["DescPlanesP"].ToString();
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Application["DescPlanesP"] = value;
			}
		}

		public string prDescProyectos
		{
			get
			{
				if (HttpContext.Current == null)
					return null;
				return HttpContext.Current.Application["DescProyectos"].ToString();
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Application["DescProyectos"] = value;
			}
		}

		public string prDescGeosidec
		{
			get
			{
				if (HttpContext.Current == null)
					return null;
				return HttpContext.Current.Application["DescGeosidec"].ToString();
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Application["DescGeosidec"] = value;
			}
		}
		#endregion

		#region-------------------------------------------------------------------- rutas
		public object prDocAyuda
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["DocAyuda"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["DocAyuda"] = value;
				}
			}
		}

		public object prDocFormatoFO35
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["FO35"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["FO35"] = value;
				}
			}
		}

		public object prDocFormatoFO379
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["FO379"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["FO379"] = value;
				}
			}
		}

		public object prLogError
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["LogError"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["LogError"] = value;
				}
			}
		}

		public object prLogErrorDestino
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["DestinoErrorLog"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["DestinoErrorLog"] = value;
				}
			}
		}

		public object prLogInfo
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["LogInfo"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["LogInfo"] = value;
				}
			}
		}

		public object prLogInfoDestino
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["DestinoInfoLog"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["DestinoInfoLog"] = value;
				}
			}
		}

		public object prPathDeclaratorias
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["PathDeclaratorias"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["PathDeclaratorias"] = value;
				}
			}
		}

		public object prFullPathDoc
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["FullPathDoc"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["FullPathDoc"] = value;
				}
			}
		}

		public object prPathDocumentos
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["PathDocumentos"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["PathDocumentos"] = value;
				}
			}
		}

		public object prPathFormatosOrigen
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["PathFormatosOrigen"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["PathFormatosOrigen"] = value;
				}
			}
		}

		public object prPathFormatosDestino
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["PathFormatosDestino"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["PathFormatosDestino"] = value;
				}
			}
		}

		public object prPathPrediosVisitas
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["PathPrediosVisitas"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["PathPrediosVisitas"] = value;
				}
			}
		}

		public object prPathPlanesPActos
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["PathPlanesPActos"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["PathPlanesPActos"] = value;
				}
			}
		}

		public object prPathPlanesPLicencias
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["PathPlanesPLicencias"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["PathPlanesPLicencias"] = value;
				}
			}
		}

		public object prPathResoluciones
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["PathResoluciones"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["PathResoluciones"] = value;
				}
			}
		}

		public object prPathPlanesP
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["PathPlanesP"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["PathPlanesP"] = value;
				}
			}
		}

		public object prPathPlanesPVisitas
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["PathPlanesPVisitas"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["PathPlanesPVisitas"] = value;
				}
			}
		}

		public object prPathPlanesPDoc1
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["PathPlanesPDoc1"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["PathPlanesPDoc1"] = value;
				}
			}
		}

		public object prPathPlanesPGeneral
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["PathPlanesPDoc2"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["PathPlanesPDoc2"] = value;
				}
			}
		}

		public object prPathPlanesPCesiones
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["PathPlanesPCesiones"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["PathPlanesPCesiones"] = value;
				}
			}
		}

		public object prPathPlanesPIndicadores
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["PathPlanesPIndicadores"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["PathPlanesPIndicadores"] = value;
				}
			}
		}

		public object prPathPlanesPlanos
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["PathPlanesPlanos"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["PathPlanesPlanos"] = value;
				}
			}
		}

		public object prPathPlantillaVisitas
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["PathPlantillaVisitas"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["PathPlantillaVisitas"] = value;
				}
			}
		}

		public object prPathPlantillaVisitasOld
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["PathPlantillaVisitasOld"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["PathPlantillaVisitasOld"] = value;
				}
			}
		}

		public object prPathPlantillaConceptos
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["PathPlantillaConceptos"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["PathPlantillaConceptos"] = value;
				}
			}
		}
		public object prPathPlantillaCartaTerminos
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["PathPlantillaCartaTerminos"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["PathPlantillaCartaTerminos"] = value;
				}
			}
		}
		public object prPathPlantillaGestionDocumental
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["PathPlantillaGestionDocumental"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["PathPlantillaGestionDocumental"] = value;
				}
			}
		}
		public object prPathPlantillaEstadoPredios
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["PathPlantillaEstadoPredios"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["PathPlantillaEstadoPredios"] = value;
				}
			}
		}
		public object prTmpImg
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["TmpImg"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["TmpImg"] = value;
				}
			}
		}
		public object prPathComisionIntersectorial
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["prPathComisionIntersectorial"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["prPathComisionIntersectorial"] = value;
				}
			}
		}
		public object prPathDocumentosProyectos
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["prPathLicenciasCuraduria"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["prPathLicenciasCuraduria"] = value;
				}
			}
		}
		#endregion

		#region-------------------------------------------------------------------- gridview
		public object prDeclaratoriasAu
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["DeclaratoriasAu"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["DeclaratoriasAu"] = value;
				}
			}
		}
		public object prVisitaAu
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["VisitaAu"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["VisitaAu"] = value;
				}
			}
		}

		public object prPlanesPAu
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["PlanesPAu"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["PlanesPAu"] = value;
				}
			}
		}

		public object prPlanesPVisitasAu
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["PlanesPVisitasAu"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["PlanesPVisitasAu"] = value;
				}
			}
		}

		public object prProyectosAu
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["ProyectosAu"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["ProyectosAu"] = value;
				}
			}
		}
		public object prIndexValue
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["IndexValue"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["IndexValue"] = value;
				}
			}
		}
		public object prBancoAu
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["BancoAu"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["BancoAu"] = value;
				}
			}
		}


		#endregion

		#region-------------------------------------------------------------------- varios
		public object prError
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Session["Error"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["Error"] = value;
				}
			}
		}

		public object prSidecFolder
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["SidecFolder"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["SidecFolder"] = value;
				}
			}
		}

		public object prPrediosDocumentos_FolioInicial
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return null;
				}
				return HttpContext.Current.Application["PrediosDocumentos_FolioInicial"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Application["PrediosDocumentos_FolioInicial"] = value;
				}
			}
		}

		public object prPermisosMenu
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return "";
				}
				return HttpContext.Current.Session["sPermisosMenu"];
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session["sPermisosMenu"] = value;
				}
			}
		}
		#endregion

		#region-------------------------------------------------------------------- hay cambios
		public object prHayCambiosLicencias
		{
			get
			{
				if (HttpContext.Current == null)
					return null;
				return HttpContext.Current.Session["HayCambiosLicencias"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["HayCambiosLicencias"] = value;
			}
		}

		public object prHayCambiosConceptos
		{
			get
			{
				if (HttpContext.Current == null)
					return null;
				return HttpContext.Current.Session["HayCambiosConceptos"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["HayCambiosConceptos"] = value;
			}
		}

		public object prHayCambiosActosAdm
		{
			get
			{
				if (HttpContext.Current == null)
					return null;
				return HttpContext.Current.Session["HayCambiosActosAdm"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["HayCambiosActosAdm"] = value;
			}
		}

		public object prHayCambiosPlanesPManzanas
		{
			get
			{
				if (HttpContext.Current == null)
					return null;
				return HttpContext.Current.Session["HayCambiosPlanesPManzanas"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["HayCambiosPlanesPManzanas"] = value;
			}
		}

		public object prHayCambiosPlanesPCesiones
		{
			get
			{
				if (HttpContext.Current == null)
					return null;
				return HttpContext.Current.Session["HayCambiosPlanesPCesiones"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["HayCambiosPlanesPCesiones"] = value;
			}
		}

		public object prHayCambiosPlanesPActos
		{
			get
			{
				if (HttpContext.Current == null)
					return null;
				return HttpContext.Current.Session["HayCambiosPlanesPActos"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["HayCambiosPlanesPActos"] = value;
			}
		}

		public object prHayCambiosPlanesPLicencias
		{
			get
			{
				if (HttpContext.Current == null)
					return null;
				return HttpContext.Current.Session["HayCambiosPlanesPLicencias"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["HayCambiosPlanesPLicencias"] = value;
			}
		}

		public object prHayCambiosPlanesPVisitas
		{
			get
			{
				if (HttpContext.Current == null)
					return null;
				return HttpContext.Current.Session["HayCambiosPlanesPVisitas"];
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Session["HayCambiosPlanesPVisitas"] = value;
			}
		}
		#endregion

		#region-------------------------------------------------------------------- dispose
		// Metodo para el manejo del GC
		public void Dispose()
		{
			GC.SuppressFinalize(true);
		}
        #endregion
	}
}