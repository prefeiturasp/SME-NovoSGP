using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Infra
{
    public class ItineranciaRetornoQueryDto
    {
        public long Id { get; set; }
        public DateTime DataVisita { get; set; }
        public int Alunos { get; set; }
        public long UeId { get; set; }
        public SituacaoItinerancia Situacao { get; set; }
        public string CriadoPor { get; set; }
    }

}
