using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ExcluirObservacaoNAAPACommandHandler : IRequestHandler<ExcluirObservacaoNAAPACommand,bool>
    {
        private readonly IRepositorioObservacaoEncaminhamentoNAAPA repositorioObs;

        public ExcluirObservacaoNAAPACommandHandler(IRepositorioObservacaoEncaminhamentoNAAPA repositorioObs)
        {
            this.repositorioObs = repositorioObs ?? throw new ArgumentNullException(nameof(repositorioObs));
        }

        public async Task<bool> Handle(ExcluirObservacaoNAAPACommand request, CancellationToken cancellationToken)
        {
            await repositorioObs.RemoverLogico(request.ObservacaoId);
            return true;
        }
    }
}