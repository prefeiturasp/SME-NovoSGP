using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Infra
{
    public class ItineranciaRetornoDto
    {
        public long Id { get; set; }
        public DateTime DataVisita { get; set; }
        public string AlunoCodigo { get; set; }
        public long UeId { get; set; }
        public SituacaoItinerancia? Situacao { get; set; }
        public string UeNome { get; set; }
        public TipoEscola TipoEscola { get; set; }        
    }

}
