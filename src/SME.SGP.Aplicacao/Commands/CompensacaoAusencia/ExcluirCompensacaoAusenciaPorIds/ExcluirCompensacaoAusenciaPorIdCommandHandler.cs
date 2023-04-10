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
        private readonly IMediator mediator;
        
        public ExcluirCompensacaoAusenciaPorIdsCommandHandler(IMediator mediator, IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioCompensacaoAusencia = repositorioCompensacaoAusencia ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusencia));
        }
        public async Task<bool> Handle(ExcluirCompensacaoAusenciaPorIdsCommand request, CancellationToken cancellationToken)
        {
            var compensacoesAusenciasSemAlunosEAulas = await mediator.Send(new ObterCompensacaoAusenciaSemAlunoEAulaPorIdsQuery(request.CompensacaoAusenciaIds), cancellationToken);
            
            if (compensacoesAusenciasSemAlunosEAulas.Any())
                await repositorioCompensacaoAusencia.RemoverLogico(compensacoesAusenciasSemAlunosEAulas.ToArray());
            
            return true;
        }
    }
}

