using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirCompensacaoAusenciaPorIdsCommandHandler : IRequestHandler<ExcluirCompensacaoAusenciaPorIdsCommand, bool>
    {
        private readonly IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia;

        public ExcluirCompensacaoAusenciaPorIdsCommandHandler(IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia)
        {
            this.repositorioCompensacaoAusencia = repositorioCompensacaoAusencia ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusencia));
        }

        public async Task<bool> Handle(ExcluirCompensacaoAusenciaPorIdsCommand request, CancellationToken cancellationToken)
        {
            var compensacoesAusenciasSemAlunosEAulas = await repositorioCompensacaoAusencia.ObterSemAlunoPorIds(request.CompensacaoAusenciaIds);

            if (compensacoesAusenciasSemAlunosEAulas.Any())
                await repositorioCompensacaoAusencia.RemoverLogico(compensacoesAusenciasSemAlunosEAulas.ToArray());

            return true;
        }
    }
}
