using MediatR;
using SME.SGP.Infra;
using SME.SGP.OtimizarArquivos.Worker.Commands.ComprimirPdf;
using SME.SGP.OtimizarArquivos.Worker.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.OtimizarArquivos.Worker.UseCases
{
    public class ComprimirPdfUsecase : IComprimirPdfUsecase
    {
        private readonly IMediator mediator;

        public ComprimirPdfUsecase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            return await mediator.Send(new ComprimirPdfCommand(mensagem.Mensagem.ToString()));
        }
    }
}
