using MediatR;
using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand : IRequest<bool>
    {
        public IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand(long turmaId, DateTime dataAula)
        {
            TurmaId = turmaId;
            DataAula = dataAula;
        }

        public long TurmaId { get; set; }
        public DateTime DataAula { get; set; }
    }
}
