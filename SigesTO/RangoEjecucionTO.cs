using System;

namespace SigesTO
{
    public class RangoEjecucionTO
    {
        public int IdRangoEjecucionTO { get; set; }
        public int IdEstado{ get; set; }
        public int DiasLimite { get; set; }
        public double PorcentajeLimite { get; set; }
        public int DiasLimiteCritico { get; set; }
        public double PorcentajeLimiteCritico { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
    }
}
