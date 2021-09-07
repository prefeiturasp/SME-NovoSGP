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
        private Turma turmaFechamento;

        public ImportarNotaAtividadeGsaCommandHandler(IMediator mediator, IRepositorioTurma repositorioTurma)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
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

                var notaConceito = await mediator.Send(
                    new ObterAtividadePorAtividadeGoogleClassIdQuery(
                        request.NotaAtividadeGsaDto.AtividadeGoogleClassroomId,
                        request.NotaAtividadeGsaDto.TurmaId.ToString(),
                        request.NotaAtividadeGsaDto.ComponenteCurricularId.ToString()));

                if (atividadeAvaliativa is null || notaConceito is null)
                {
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAgendamento.RotaNotaAtividadesSync,
                        new MensagemAgendamentoSyncDto(RotasRabbitSgp.RotaAtividadesNotasSync,
                            request.NotaAtividadeGsaDto),
                        Guid.NewGuid(),
                        null));
                }
                else
                {
                    await mediator.Send(
                        new SalvarNotaAtividadeAvaliativaGsaCommand(notaConceito.Id, request.NotaAtividadeGsaDto.Nota,
                            request.NotaAtividadeGsaDto.StatusGsa));
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