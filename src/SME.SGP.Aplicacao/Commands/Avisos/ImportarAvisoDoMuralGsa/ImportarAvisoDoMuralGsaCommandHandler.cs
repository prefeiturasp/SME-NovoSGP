using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ImportarAvisoDoMuralGsaCommandHandler : AsyncRequestHandler<ImportarAvisoDoMuralGsaCommand>
    {
        private readonly IMediator mediator;

        public ImportarAvisoDoMuralGsaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(ImportarAvisoDoMuralGsaCommand request, CancellationToken cancellationToken)
        {
            var aulaId = await mediator.Send(new ObterAulaPorCodigoTurmaComponenteEDataQuery(request.AvisoDto.TurmaId, request.AvisoDto.ComponenteCurricularId.ToString(), request.AvisoDto.DataCriacao));

            if (ReagendarImportacao(aulaId, request.AvisoDto.DataCriacao))
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAgendamento.RotaMuralAvisosSync,
                                                               new MensagemAgendamentoSyncDto(RotasRabbitSgp.RotaMuralAvisosSync, request.AvisoDto),
                                                               Guid.NewGuid(),
                                                               null));
            else
                await mediator.Send(new SalvarAvisoGsaNoMuralCommand(aulaId,
                                                                      request.AvisoDto.UsuarioRf,
                                                                      request.AvisoDto.Mensagem,
                                                                      request.AvisoDto.AvisoClassroomId,
                                                                      request.AvisoDto.DataCriacao,
                                                                      request.AvisoDto.DataAlteracao,
                                                                      request.AvisoDto.Email));
        }

        private bool ReagendarImportacao(long aulaId, DateTime dataCriacao)
            => aulaId == 0 
            && dataCriacao.Year == DateTime.Now.Year;
    }
}
