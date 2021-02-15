using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Infra
{
    public class ItineranciaResumoDto
    {
        public long Id { get; set; }
        public DateTime DataVisita { get; set; }
        public string NomeUe { get; set; }
        public string NomeCriancaEstudante { get; set; }
        public string NomeTurma { get; set; }
        public SituacaoItinerancia Situacao { get; set; }
    }
}
