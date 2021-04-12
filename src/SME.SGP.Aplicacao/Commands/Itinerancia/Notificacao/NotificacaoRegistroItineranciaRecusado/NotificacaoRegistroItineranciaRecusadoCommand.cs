using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoRegistroItineranciaRecusadoCommand : IRequest<bool>
    {
        public NotificacaoRegistroItineranciaRecusadoCommand(string ueCodigo, string criadoRF, string criadoPor, DateTime dataVisita, IEnumerable<ItineranciaAlunoDto> estudantes)
        {
            UeCodigo = ueCodigo;
            CriadoRF = criadoRF;
            CriadoPor = criadoPor;
            DataVisita = dataVisita;
            Estudantes = estudantes;
        }

        public string UeCodigo { get; set; }
        public string CriadoRF { get; set; }
        public string CriadoPor { get; set; }
        public DateTime DataVisita { get; set; }

        public IEnumerable<ItineranciaAlunoDto> Estudantes { get; set; }

    }
}
