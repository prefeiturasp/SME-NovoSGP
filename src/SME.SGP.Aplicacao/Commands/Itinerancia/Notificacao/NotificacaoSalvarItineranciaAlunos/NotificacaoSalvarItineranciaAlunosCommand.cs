using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoSalvarItineranciaAlunosCommand : IRequest<bool>
    {
        public NotificacaoSalvarItineranciaAlunosCommand(long ueId, string criadoRF, string criadoPor, DateTime dataVisita, IEnumerable<ItineranciaAlunoDto> estudantes, long itineranciaId)
        {
            UeId = ueId;
            CriadoRF = criadoRF;
            CriadoPor = criadoPor;
            DataVisita = dataVisita;
            Estudantes = estudantes;
            ItineranciaId = itineranciaId;
        }

        public long ItineranciaId { get; set; }
        public long UeId { get; set; }
        public string CriadoRF { get; set; }
        public string CriadoPor { get; set; }
        public DateTime DataVisita { get; set; }
        public IEnumerable<ItineranciaAlunoDto> Estudantes { get; set; }

    }
}
