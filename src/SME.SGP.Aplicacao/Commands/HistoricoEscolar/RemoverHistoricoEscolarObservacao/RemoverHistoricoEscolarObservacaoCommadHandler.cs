using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.HistoricoEscolar
{
    public class RemoverHistoricoEscolarObservacaoCommadHandler : IRequestHandler<RemoverHistoricoEscolarObservacaoCommad, bool>
    {
        private readonly IRepositorioHistoricoEscolarObservacao repositorioHistoricoEscolarObservacao;
        public RemoverHistoricoEscolarObservacaoCommadHandler(IRepositorioHistoricoEscolarObservacao repositorioHistoricoEscolarObservacao)
        {
            this.repositorioHistoricoEscolarObservacao = repositorioHistoricoEscolarObservacao ?? throw new ArgumentNullException(nameof(repositorioHistoricoEscolarObservacao));
        }

        public async Task<bool> Handle(RemoverHistoricoEscolarObservacaoCommad request, CancellationToken cancellationToken)
        {
            await this.repositorioHistoricoEscolarObservacao.RemoverAsync(request.HistoricoObservacao);

            return true;
        }
    }
}
