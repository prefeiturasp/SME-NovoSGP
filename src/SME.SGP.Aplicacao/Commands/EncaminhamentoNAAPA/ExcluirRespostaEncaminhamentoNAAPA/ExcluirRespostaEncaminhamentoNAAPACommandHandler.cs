using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRespostaEncaminhamentoNAAPACommandHandler : IRequestHandler<ExcluirRespostaEncaminhamentoNAAPACommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRespostaEncaminhamentoNAAPA repositorioRespostaEncaminhamentoNAAPA;

        public ExcluirRespostaEncaminhamentoNAAPACommandHandler(IMediator mediator, IRepositorioRespostaEncaminhamentoNAAPA repositorioRespostaEncaminhamentoNAAPA)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioRespostaEncaminhamentoNAAPA = repositorioRespostaEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioRespostaEncaminhamentoNAAPA));
        }

        public async Task<bool> Handle(ExcluirRespostaEncaminhamentoNAAPACommand request, CancellationToken cancellationToken)
        {
            request.Resposta.Excluido = true;
            var arquivoId = request.Resposta.ArquivoId;
            request.Resposta.ArquivoId = null;

            await repositorioRespostaEncaminhamentoNAAPA.SalvarAsync(request.Resposta);

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
