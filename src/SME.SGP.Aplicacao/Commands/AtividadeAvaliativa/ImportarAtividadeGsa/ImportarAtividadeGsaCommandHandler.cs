using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ImportarAtividadeGsaCommandHandler : AsyncRequestHandler<ImportarAtividadeGsaCommand>
    {
        private readonly IMediator mediator;
        public ImportarAtividadeGsaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(ImportarAtividadeGsaCommand request, CancellationToken cancellationToken)
        {
            var aulaId = await mediator.Send(new ObterAulaPorCodigoTurmaComponenteEDataQuery(request.AtividadeGsa.TurmaId, request.AtividadeGsa.ComponenteCurricularId.ToString(), request.AtividadeGsa.DataCriacao));

            if (ReagendarImportacao(aulaId, request.AtividadeGsa.DataCriacao))
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAgendamento.RotaMuralAvisosSync,
                                                               new MensagemAgendamentoSyncDto(RotasRabbitSgp.RotaAtividadesSync, request.AtividadeGsa),
                                                               Guid.NewGuid(),
                                                               null));
            else
                await mediator.Send(new SalvarAtividadeAvaliativaGsaCommand(aulaId,
                                                                      request.AtividadeGsa.UsuarioRf,
                                                                      request.AtividadeGsa.TurmaId,
                                                                      request.AtividadeGsa.ComponenteCurricularId,
                                                                      request.AtividadeGsa.Titulo,
                                                                      request.AtividadeGsa.Descricao,
                                                                      request.AtividadeGsa.DataCriacao,
                                                                      request.AtividadeGsa.DataAlteracao,
                                                                      request.AtividadeGsa.AtividadeClassroomId
                                                                      ));
        }

        private bool ReagendarImportacao(long aulaId, DateTime dataCriacao)
            => aulaId == 0
            && dataCriacao.Year == DateTime.Now.Year;
    }
}
