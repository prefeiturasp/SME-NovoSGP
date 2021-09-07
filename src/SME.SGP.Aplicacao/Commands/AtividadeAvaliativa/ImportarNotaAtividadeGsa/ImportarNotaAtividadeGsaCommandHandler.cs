using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ImportarNotaAtividadeGsaCommandHandler : AsyncRequestHandler<ImportarNotaAtividadeGsaCommand>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private Turma turmaFechamento;

        public ImportarNotaAtividadeGsaCommandHandler(IMediator mediator, IRepositorioTurma repositorioTurma)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
        }

        protected override async Task Handle(ImportarNotaAtividadeGsaCommand request,
            CancellationToken cancellationToken)
        {
            await ValidarLancamentoNotaComponente(request.NotaAtividadeGsaDto.ComponenteCurricularId);
            await CarregarTurma(request.NotaAtividadeGsaDto.TurmaId);


            if (turmaFechamento.EhTurmaInfantil)
            {
                var registroIndividual = await mediator.Send(new ObterRegistroIndividualPorAlunoDataQuery(
                    request.NotaAtividadeGsaDto.TurmaId,
                    request.NotaAtividadeGsaDto.CodigoAluno,
                    request.NotaAtividadeGsaDto.ComponenteCurricularId,
                    request.NotaAtividadeGsaDto.DataEntregaAvaliacao));

                if (registroIndividual is null)
                {
                    await mediator.Send(new InserirRegistroIndividualCommand(
                        request.NotaAtividadeGsaDto.TurmaId,
                        request.NotaAtividadeGsaDto.CodigoAluno,
                        request.NotaAtividadeGsaDto.ComponenteCurricularId,
                        request.NotaAtividadeGsaDto.DataEntregaAvaliacao,
                        RegistroFormatado(request.NotaAtividadeGsaDto.Registro)));
                }
                else
                {
                    await mediator.Send(new AlterarRegistroIndividualCommand(
                        registroIndividual.Id, request.NotaAtividadeGsaDto.TurmaId,
                        request.NotaAtividadeGsaDto.CodigoAluno, request.NotaAtividadeGsaDto.ComponenteCurricularId,
                        request.NotaAtividadeGsaDto.DataEntregaAvaliacao,
                        RegistroFormatado(request.NotaAtividadeGsaDto.Registro)));
                }
            }
            else
            {
                var atividadeAvaliativa =
                    await mediator.Send(
                        new ObterAtividadeAvaliativaPorGoogleClassroomIdQuery(request.NotaAtividadeGsaDto
                            .AtividadeGoogleClassroomId));



                if (atividadeAvaliativa is null)
                {
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAgendamento.RotaNotaAtividadesSync,
                        new MensagemAgendamentoSyncDto(RotasRabbitSgp.RotaAtividadesNotasSync,
                            request.NotaAtividadeGsaDto),
                        Guid.NewGuid(),
                        null));
                }
                else
                {
                    var notaConceito = await mediator.Send(
                        new ObterNotaPorAtividadeGoogleClassIdQuery(
                            atividadeAvaliativa.Id,
                            request.NotaAtividadeGsaDto.CodigoAluno));

                    var periodoEscolarUltimoBimestre = await consultasPeriodoEscolar.ObterUltimoPeriodoAsync(turmaFechamento.AnoLetivo, turmaFechamento.ModalidadeTipoCalendario, turmaFechamento.Semestre);

                    var tipoNota = await mediator.Send(new ObterNotaTipoPorAnoModalidadeDataReferenciaQuery(turmaFechamento.Ano,turmaFechamento.ModalidadeCodigo, periodoEscolarUltimoBimestre.PeriodoFim));
                    
                    await mediator.Send(
                        new SalvarNotaAtividadeAvaliativaGsaCommand(notaConceito, request.NotaAtividadeGsaDto.Nota,
                            request.NotaAtividadeGsaDto.StatusGsa, atividadeAvaliativa.Id, tipoNota));
                }
            }
        }

        private async Task ValidarLancamentoNotaComponente(long componenteCurricularId)
        {
            if (!await mediator.Send(new ObterComponenteLancaNotaQuery(componenteCurricularId)))
                throw new NegocioException(
                    $"Componentes que não lançam nota não terão atividades avaliativas importada do classroom. Componente Curricular: {componenteCurricularId}");
        }

        private async Task CarregarTurma(long turmaCodigo)
        {
            turmaFechamento = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(turmaCodigo.ToString());
            if (turmaFechamento == null)
                throw new NegocioException($"Turma com código [{turmaCodigo}] não localizada!");
        }

        private string RegistroFormatado(string registro)
        {
            return $"<p>{registro}</p>";
        }
    }
}