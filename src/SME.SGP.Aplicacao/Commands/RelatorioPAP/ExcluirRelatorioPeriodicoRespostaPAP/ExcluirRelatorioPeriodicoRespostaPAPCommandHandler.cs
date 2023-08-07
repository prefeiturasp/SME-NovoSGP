using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRelatorioPeriodicoRespostaPAPCommandHandler : IRequestHandler<ExcluirRelatorioPeriodicoRespostaPAPCommand, bool>
    {
        protected readonly IMediator mediator;
        protected readonly IRepositorioRelatorioPeriodicoPAPResposta repositorio;

        public ExcluirRelatorioPeriodicoRespostaPAPCommandHandler(IMediator mediator, IRepositorioRelatorioPeriodicoPAPResposta repositorio)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }
        public async Task<bool> Handle(ExcluirRelatorioPeriodicoRespostaPAPCommand request, CancellationToken cancellationToken)
        {
            request.Resposta.Excluido = true;
            var arquivoId = request.Resposta.ArquivoId;
            request.Resposta.ArquivoId = null;

            await repositorio.SalvarAsync(request.Resposta);

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
