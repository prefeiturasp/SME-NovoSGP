using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarConsolidacaoDashBoardFrequenciaCommandHandler : IRequestHandler<SalvarConsolidacaoDashBoardFrequenciaCommand, bool>
    {
        private readonly IRepositorioConsolidacaoFrequenciaTurma repositorioConsolidacaoFrequenciaTurma;

        public SalvarConsolidacaoDashBoardFrequenciaCommandHandler(IRepositorioConsolidacaoFrequenciaTurma repositorioConsolidacaoFrequenciaTurma)
        {
            this.repositorioConsolidacaoFrequenciaTurma = repositorioConsolidacaoFrequenciaTurma ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoFrequenciaTurma));
        }

        public async Task<bool> Handle(SalvarConsolidacaoDashBoardFrequenciaCommand request, CancellationToken cancellationToken)
        {
            await repositorioConsolidacaoFrequenciaTurma.SalvarConsolidacaoDashBoardFrequencia(request.ConsolidacaoDashBoardFrequencia);

            return true;
        }
    }
}
