using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Infra
{
    public class FiltroPesquisaItineranciasDto
    {
        public long DreId { get; set; }
        public long UeId { get; set; }
        public long TurmaId { get; set; }
        public int AnoLetivo { get; set; }
        public string AlunoCodigo { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public SituacaoItinerancia Situacao { get; set; }
        public string CriadoRf { get; set; }
    }
}
