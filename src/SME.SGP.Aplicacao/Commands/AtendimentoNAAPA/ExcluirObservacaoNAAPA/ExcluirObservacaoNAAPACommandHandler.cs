using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ExcluirObservacaoNAAPACommandHandler : IRequestHandler<ExcluirObservacaoNAAPACommand,bool>
    {
        private readonly IRepositorioObservacaoAtendimentoNAAPA repositorioObs;

        public ExcluirObservacaoNAAPACommandHandler(IRepositorioObservacaoAtendimentoNAAPA repositorioObs)
        {
            this.repositorioObs = repositorioObs ?? throw new ArgumentNullException(nameof(repositorioObs));
        }

        public async Task<bool> Handle(ExcluirObservacaoNAAPACommand request, CancellationToken cancellationToken)
        {
            var observacao = await repositorioObs.ObterPorIdAsync(request.ObservacaoId);

            if(observacao.EhNulo())
                throw new NegocioException("Observação não encontrada");

            await repositorioObs.RemoverLogico(request.ObservacaoId);
            return true;
        }
    }
}