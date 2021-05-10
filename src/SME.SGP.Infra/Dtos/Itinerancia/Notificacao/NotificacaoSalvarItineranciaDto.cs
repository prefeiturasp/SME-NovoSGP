using System;
using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos
{
    public class NotificacaoSalvarItineranciaDto
    {
        public DateTime DataVisita { get; set; }
        public long UeId { get; set; }
        public long ItineranciaId { get; set; }
        public IEnumerable<ItineranciaAlunoDto> Estudantes { get; set; }
        public string CriadoRF { get; set; }
        public string CriadoPor { get; set; }
    }
}
