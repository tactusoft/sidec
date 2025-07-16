using System;

namespace SigesTO
{
    public class InteresadoTO
    {
        public int IdInteresado { get; set; }
        public int IdPredioDeclarado { get; set; }
        public int IdTipoInteresado { get; set; }
        public string TipoInteresado { get; set; }
        public string Documento { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
        public string Otro { get; set; }
        public decimal Valor2 { get; set; }
        public int CodUsu { get; set; }
        public string Color { get; set; }
        public DateTime fecha { get; set; }
        public bool Estado { get; set; }
    }
}
