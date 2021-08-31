using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ImportarnotaAtividadeGsaCommandHandler : AsyncRequestHandler<ImportarNotaAtividadeGsaCommand>
    {
        private readonly IMediator mediator;

        public ImportarnotaAtividadeGsaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(ImportarNotaAtividadeGsaCommand request,
            CancellationToken cancellationToken)
        {
            var notaConceito = await mediator.Send(
                new ObterAtividadeNotaConseitoPorAtividadeGoogleClassIdQuery(
                    request.NotaAtividadeGsaDto.AtividadeGoogleClassroomId,
                    request.NotaAtividadeGsaDto.TurmaId,
                    request.NotaAtividadeGsaDto.ComponenteCurricularId));

            if (notaConceito is null)
            {
                throw new NegocioException($"Não foi encontrado nota para lançar");
            }

            await mediator.Send(
                new SalvarNotaAtividadeAvaliativaGsaCommand(notaConceito.Id, request.NotaAtividadeGsaDto.Nota,
                    request.NotaAtividadeGsaDto.StatusGsa));
        }
    }
}