using MediatR;
using SME.SGP.Dominio;
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
            ValidarDataAviso(request.AvisoDto.DataCriacao);

            var aula = await mediator.Send(new ObterAulaPorCodigoTurmaComponenteEDataQuery(request.AvisoDto.TurmaId, request.AvisoDto.ComponenteCurricularId.ToString(), request.AvisoDto.DataCriacao));
            if (ReagendarImportacao(aula))
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAgendamento.RotaMuralAvisosSync,
                                                               new MensagemAgendamentoSyncDto(RotasRabbitSgp.RotaMuralAvisosSync, request.AvisoDto),
                                                               Guid.NewGuid(),
                                                               null));
            else
                await mediator.Send(new SalvarAvisoGsaNoMuralCommand(aula.AulaId,
                                                                      request.AvisoDto.UsuarioRf,
                                                                      request.AvisoDto.Mensagem,
                                                                      request.AvisoDto.AvisoClassroomId,
                                                                      request.AvisoDto.DataCriacao,
                                                                      request.AvisoDto.DataAlteracao,
                                                                      request.AvisoDto.Email));
        }

        private void ValidarDataAviso(DateTime dataCriacao)
        {
            if (dataCriacao.Year < DateTime.Now.Year)
                throw new NegocioException($"Avisos do Mural Classroom de ano anterior não serão importados. Data Aviso: {dataCriacao:dd/MM/yyyy}");
        }

        private bool ReagendarImportacao(DataAulaDto aula)
            => aula == null
            || aula.AulaId == 0;
    }
}
