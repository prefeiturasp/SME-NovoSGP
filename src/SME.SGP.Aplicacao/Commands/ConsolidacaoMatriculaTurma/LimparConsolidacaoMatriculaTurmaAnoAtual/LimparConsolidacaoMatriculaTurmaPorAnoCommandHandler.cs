using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class LimparConsolidacaoMatriculaTurmaPorAnoCommandHandler : IRequestHandler<LimparConsolidacaoMatriculaTurmaPorAnoCommand, bool>
    {
        private readonly IRepositorioConsolidacaoMatriculaTurma repositorioConsolidacaoMatriculaTurma;

        public LimparConsolidacaoMatriculaTurmaPorAnoCommandHandler(IRepositorioConsolidacaoMatriculaTurma repositorioConsolidacaoMatriculaTurma)
        {
            this.repositorioConsolidacaoMatriculaTurma = repositorioConsolidacaoMatriculaTurma ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoMatriculaTurma));
        }

        public async Task<bool> Handle(LimparConsolidacaoMatriculaTurmaPorAnoCommand request, CancellationToken cancellationToken)
        {
            await repositorioConsolidacaoMatriculaTurma.LimparConsolidacaoMatriculasTurmasPorAnoLetivo(request.AnoLetivo);
            return true;
        }
    }
}
