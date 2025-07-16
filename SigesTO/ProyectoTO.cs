namespace SigesTO
{
    public struct TipoVivienda
    {
        public const int VIP = 1;
        public const int VIS = 2;
        public const int NoVIS = 3;
    }
    public class DistribucionViviendaTO
    {
        public int IdDistribucion { get; set; }
        public int IdProyecto { get; set; }
        public int idTipo { get; set; }
        public int Area { get; set; }
        public int Cantidad { get; set; }
    }
}
