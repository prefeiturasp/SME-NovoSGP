using System;

namespace SME.SGP.Infra
{
    public class ItineranciaResumoDto
    {
        public ItineranciaResumoDto()
        {
            Situacao = "Digitado";
        }
        public long Id { get; set; }
        public DateTime DataVisita { get; set; }
        public string NomeUe { get; set; }
        public string NomeCriancaEstudante { get; set; }
        public string NomeTurma { get; set; }
        public string Situacao { get; set; }
    }
}
