
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistraConsolidacaoMatriculaTurmaCommandHandler : IRequestHandler<RegistraConsolidacaoMatriculaTurmaCommand, long>
    {
        private readonly IRepositorioConsolidacaoMatriculaTurma repositorioConsolidacaoMatriculaTurma;

        public RegistraConsolidacaoMatriculaTurmaCommandHandler(IRepositorioConsolidacaoMatriculaTurma repositorioConsolidacaoMatriculaTurma)
        {
            this.repositorioConsolidacaoMatriculaTurma = repositorioConsolidacaoMatriculaTurma ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoMatriculaTurma));
        }

        public async Task<long> Handle(RegistraConsolidacaoMatriculaTurmaCommand request, CancellationToken cancellationToken)
        {
            return await repositorioConsolidacaoMatriculaTurma.Inserir(new ConsolidacaoMatriculaTurma(request.TurmaId, request.Quantidade));
        }
    }
}
