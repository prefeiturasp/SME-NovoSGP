using System;

namespace SME.SGP.Infra
{
    public class ItineranciaResumoDto
    {
        public long Id { get; set; }
        public DateTime DataVisita { get; set; }
        public string UeNome { get; set; }
        public string EstudanteNome { get; set; }
        public string TurmaNome { get; set; }
        public string Situacao { get; set; }
    }
}
