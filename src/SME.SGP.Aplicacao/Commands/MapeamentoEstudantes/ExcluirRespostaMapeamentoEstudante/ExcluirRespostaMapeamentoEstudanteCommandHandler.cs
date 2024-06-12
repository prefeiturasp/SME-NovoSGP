using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRespostaMapeamentoEstudanteCommandHandler : IRequestHandler<ExcluirRespostaMapeamentoEstudanteCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRespostaMapeamentoEstudante repositorioResposta;

        public ExcluirRespostaMapeamentoEstudanteCommandHandler(IMediator mediator, IRepositorioRespostaMapeamentoEstudante repositorioResposta)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioResposta = repositorioResposta ?? throw new ArgumentNullException(nameof(repositorioResposta));
        }

        public async Task<bool> Handle(ExcluirRespostaMapeamentoEstudanteCommand request, CancellationToken cancellationToken)
        {
            request.Resposta.Excluido = true;
            var arquivoId = request.Resposta.ArquivoId;
            request.Resposta.ArquivoId = null;
            await repositorioResposta.SalvarAsync(request.Resposta);
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
