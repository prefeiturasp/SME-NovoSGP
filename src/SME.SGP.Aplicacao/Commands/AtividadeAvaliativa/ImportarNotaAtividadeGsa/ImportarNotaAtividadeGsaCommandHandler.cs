using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ImportarNotaAtividadeGsaCommandHandler : AsyncRequestHandler<ImportarNotaAtividadeGsaCommand>
    {
        private readonly IMediator mediator;

        public ImportarNotaAtividadeGsaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(ImportarNotaAtividadeGsaCommand request,
            CancellationToken cancellationToken)
        {
            if (!await ValidarLancamentoNotaComponente(request.NotaAtividadeGsaDto.ComponenteCurricularId))
                return;

            var turma = await CarregarTurma(request.NotaAtividadeGsaDto.TurmaId);
            if (turma is null)
                return;

            if (turma.EhTurmaInfantil)
                await GerarRegistroIndividual(request.NotaAtividadeGsaDto);
            else
                await GerarNotaAtividade(request.NotaAtividadeGsaDto, turma);
        }

        private async Task GerarNotaAtividade(NotaAtividadeGsaDto notaAtividadeGsaDto, Turma turma)
        {
            var atividadeAvaliativa =
                    await mediator.Send(
                        new ObterAtividadeAvaliativaPorGoogleClassroomIdQuery(notaAtividadeGsaDto.AtividadeGoogleClassroomId));

            if (atividadeAvaliativa is null)
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAgendamento.RotaNotaAtividadesSync,
                    new MensagemAgendamentoSyncDto(RotasRabbitSgp.RotaAtividadesNotasSync,
                                                   notaAtividadeGsaDto),
                    Guid.NewGuid(),
                    null));
            }
            else
            {
                var notaConceito = await mediator.Send(
                    new ObterNotaPorAtividadeGoogleClassIdQuery(
                        atividadeAvaliativa.Id,
                        notaAtividadeGsaDto.CodigoAluno.ToString()));

                var tipoNota = await mediator.Send(new ObterNotaTipoPorAnoModalidadeDataReferenciaQuery(turma.Ano, turma.ModalidadeCodigo, DateTime.Now));

                await mediator.Send(
                    new SalvarNotaAtividadeAvaliativaGsaCommand(
                        notaConceito,
                        notaAtividadeGsaDto.Nota,
                        notaAtividadeGsaDto.StatusGsa,
                        atividadeAvaliativa.Id,
                        tipoNota));
            }
        }

        private async Task GerarRegistroIndividual(NotaAtividadeGsaDto notaAtividadeGsaDto)
        {
            var registroIndividual = await mediator.Send(new ObterRegistroIndividualPorAlunoDataQuery(
                    notaAtividadeGsaDto.TurmaId,
                    notaAtividadeGsaDto.CodigoAluno,
                    notaAtividadeGsaDto.ComponenteCurricularId,
                    notaAtividadeGsaDto.DataInclusao));

            if (registroIndividual is null)
            {
                await mediator.Send(new InserirRegistroIndividualCommand(
                    notaAtividadeGsaDto.TurmaId,
                    notaAtividadeGsaDto.CodigoAluno,
                    notaAtividadeGsaDto.ComponenteCurricularId,
                    notaAtividadeGsaDto.DataInclusao,
                    RegistroFormatado(notaAtividadeGsaDto.Titulo)));
            }
            else
            {
                await mediator.Send(new AlterarRegistroIndividualCommand(
                    registroIndividual.Id,
                    notaAtividadeGsaDto.TurmaId,
                    notaAtividadeGsaDto.CodigoAluno,
                    notaAtividadeGsaDto.ComponenteCurricularId,
                    notaAtividadeGsaDto.DataInclusao,
                    RegistroFormatado(notaAtividadeGsaDto.Titulo)));
            }
        }

        private async Task<bool> ValidarLancamentoNotaComponente(long componenteCurricularId)
        {
            return await mediator.Send(new ObterComponenteLancaNotaQuery(componenteCurricularId));
        }

        private async Task<Turma> CarregarTurma(long turmaCodigo)
        {
            return await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo.ToString()));
        }

        private string RegistroFormatado(string registro)
        {
            return $"<p>{registro}</p>";
        }
    }
}