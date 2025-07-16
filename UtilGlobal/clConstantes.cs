using System;

namespace GLOBAL.CONST
{
    public class clConstantes
    {
        public struct Section
        {
            //Opciones de Menú
            public const string PROYECTO_ASOCIATIVO = "2";
            public const string PROYECTO_ESTRATEGICO = "8";
            public const string FICHA_PREDIAL = "10";
            public const string CARGUE_LICENCIA = "11";

            public const string REPORTE_ESTADO_PREDIOS_DECLARADOS = "701";
            public const string REPORTE_GESTION_USUARIOS = "702";
            public const string REPORTE_GESTION_DOCUMENTOS = "703";
            public const string REPORTE_PRESTAMOS_ABIERTOS = "704";
            public const string REPORTE_PLANES_PARCIALES = "705";

            public const string ADMINISTRACION_USUARIO = "901";
            public const string ADMINISTRACION_PERFIL = "902";
            public const string ADMINISTRACION_IDENTIDAD = "905";
            public const string ADMINISTRACION_ALERTA = "906";

            //Opciones de Sección
            public const string FICHA_BANCO = "ProjectBank"; 
            public const string ACTIVITY = "Activity";
            public const string ACTOR = "Agent";
            public const string TRACING = "Tracing";

            public const string PLAN_PARC_DOCUMENTOS = "PartialPlanDocuments";

            public const string PROY_ASOC_PREDIOS = "AssocProjProperties";
            public const string PROY_ASOC_CARTAS_INTENCION = "AssocProjTermSheet";
            public const string PROY_ASOC_LICENCIAS = "AssocProjLicense";
            public const string PROY_ASOC_VISITAS = "AssocProjVisit";

            public const string PRED_DECL_DOCUMENTOS_EXTRAS = "DeclaredPropExtraDocs";
            public const string PRED_DECL_INTERESADO = "DeclaredPropInterested";
            public const string PRED_DECL_CARTA = "DeclaredPropLetter";
            public const string ACOMPANAM_PROYECTO = "ProjectAccompaniment";

            public const string COM_INTERSEC_FOLIOS = "ComIntersecFolios";
        }
        public enum TipoArchivo
        {   
            ND = 0,
            LIC_URB = 1,
            PLA_LIC = 2,
            IMG_BP = 3,
            PDF_PP = 4,
            PDF_PD = 5,
            IMG_VPA = 6,
            FLCI = 7,
            ANCI = 8
        }
        public struct Accion
        {
            public const string CONSULTAR = "consultar";
            public const string INSERTAR = "insertar";
            public const string EDITAR = "modificar";
            public const string ELIMINAR = "eliminar";
            public const string NIVEL = "nivel";
        }
        public struct Color
        {
            public const string VERDE = "00FF00";
            public const string AMARILLO = "FFE900";
            public const string NARANJA = "FF7F00";
            public const string ROJO = "FF0000";
        }

        public struct PAEstadoAsociativo
        {
            public const string ESTADO_ASOC_EN_PROCESO = "430";
            public const string ESTADO_ASOC_CUMPLIDO = "431";
            public const string ESTADO_ASOC_VENC_TERMI = "494";
        }


        #region CONSTANTES DESDE BASES DE DATOS   

        /// <summary>
        /// Acciones definidas
        /// </summary>
        public enum AccionRol
        {
            CREAR = 0,
            CONSULTAR = 1,
            MODIFICAR = 2,
            ELIMINAR = 3
        };

        /// <summary>
        /// Codigos de Autenticacion
        /// </summary>
        public enum Autenticacion
        {
            ErrorDesconocido = -10,
            CambioClave = -7,
            DobleAutenticacion = -6,
            ErrorBD = -5,
            IPBloqueada = -4,
            IntentosFallidos = -3,
            ErrorCredenciales = -2,
            NoExisteUsuario = -1,
            Autenticado = 0,
            FueradeLinea = 1
        }

        /// <summary>
        /// Nivel de Mensaje.
        /// </summary>
        public enum NivelMensaje
        {
            // 0: info  1: success 2: warning 3: danger
            Informacion = 0,
            Exitoso = 1,
            Alerta = 2,
            Error = 3
        }
        #endregion

        #region CONSTANTES STRING

        public const int MAX_PDF_SIZE = 102400000;

        public const string MSG_CONTADOR = "Registro {0} de {1}";

        public const string DB_ACTION_OK = "00000";
        public const string DB_ACTION_ERR_DATOS = "00100";
        public const string DB_ACTION_ERR = "00101";
        public const string DB_ACTION_ERR_PERMISO = "00102";
        public const string DB_ACTION_ERR_ORDER = "00103";
        public const string DB_ACTION_ERR_PARCIAL = "00104";
        public const string DB_ACTION_ERR_DELETE = "00105";
        public const string DB_ACTION_SIN_CAMBIOS = "00200";
        public const string DB_ACTION_FILELOAD = "00300";

        public const string TABLE_NO_RECORDS = "No hay registros asociados para este módulo";

        public const string MSG_I = "Registro para crear";
        public const string MSG_U = "Registro para editar";
        public const string MSG_D = "Registro para eliminar";

        public const string MSG_OK_I = "Registro creado exitosamente";
        public const string MSG_OK_U = "Registro actualizado exitosamente";
        public const string MSG_OK_D = "Registro eliminado exitosamente";
        public const string MSG_OK_DESHABILITAR = "Registro deshabilitado exitosamente";

        public const string MSG_OK_FILELOAD_I = "Archivo cargado exitosamente";
        public const string MSG_OK_FILELOAD_U = "Archivo actualizado exitosamente";
        public const string MSG_OK_FILELOAD_D = "Archivo eliminado exitosamente";

        public const string MSG_OK_FILELOAD = "Archivo cargado exitosamente";
        public const string MSG_OK_FILE_CREADO = "Archivo creado";
        public const string MSG_OK_FILE_NO_DATA = "No hay datos para exportar";

        public const string MSG_ERR_I = "No fue posible crear el registro";
        public const string MSG_ERR_U = "No fue posible actualizar el registro";
        public const string MSG_ERR_D = "No fue posible eliminar el registro";

        public const string MSG_ERR_FILELOAD_I = "No fue posible cargar el archivo";
        public const string MSG_ERR_FILELOAD_U = "No fue posible actualizar el archivo";
        public const string MSG_ERR_FILELOAD_D = "No fue posible eliminar el archivo";

        public const string MSG_ERR_DESHABILITAR = "No fue posible deshabilitar el registro";
        public const string MSG_ERR_PERMISO = "No tiene permisos para ejecutar la acción";
        public const string MSG_ERR_ORDEN = "Los registros solo pueden desplazarse hacia adelante";
        public const string MSG_ERR_PARCIAL = "No se ejecutó completamente la acción";
        public const string MSG_ERR_FILELOAD = "No fue posible cargar el archivo";
        public const string MSG_ERR_DATOS = "Los datos ingresados presentan inconsistencias";
        public const string MSG_ERR_FILE_NO_EXISTS = "No existe el archivo solicitado";
        public const string MSG_ERR_FILE_CREADO = "No fue posible crear el archivo";

        public const string MSG_SIN_CAMBIOS = "No se realizaron modificaciones";

        public const string FILE_NO_EXISTE = "No existe el formato: {0}";
        public const string PLANTILLA_NO_EXISTE = "No existe la plantilla";
        public const string FECHA_RANGO_ERR = "La fecha no puede ser anterior al {1} ni posterior al {2}";
        public const string FECHA_RANGO_ERR_old = " ( {0} : La fecha no puede ser anterior al {1} ni posterior al {2} ) ";

        public const string FILE_NO_PDF = "Solo se admiten archivos en formato PDF";
        public const string FILE_ERR_LOAD = "Error al cargar archivo";
        #endregion
    }
}
