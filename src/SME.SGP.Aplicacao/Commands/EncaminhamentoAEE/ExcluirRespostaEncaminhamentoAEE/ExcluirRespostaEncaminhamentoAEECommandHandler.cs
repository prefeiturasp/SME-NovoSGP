using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRespostaEncaminhamentoAEECommandHandler : IRequestHandler<ExcluirRespostaEncaminhamentoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRespostaEncaminhamentoAEE repositorioRespostaEncaminhamentoAEE;

        public ExcluirRespostaEncaminhamentoAEECommandHandler(IMediator mediator, IRepositorioRespostaEncaminhamentoAEE repositorioRespostaEncaminhamentoAEE)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioRespostaEncaminhamentoAEE = repositorioRespostaEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioRespostaEncaminhamentoAEE));
        }

        public async Task<bool> Handle(ExcluirRespostaEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            request.Resposta.Excluido = true;
            var arquivoId = request.Resposta.ArquivoId;
            request.Resposta.ArquivoId = null;

            await repositorioRespostaEncaminhamentoAEE.SalvarAsync(request.Resposta);

            if (arquivoId.HasValue)
                await RemoverArquivo(arquivoId);

            return true;
        }

        private async Task RemoverArquivo(long? arquivoId)
        {
            await mediator.Send(new ExcluirArquivoPorIdCommand(arquivoId.Value));
        }
    }
}
