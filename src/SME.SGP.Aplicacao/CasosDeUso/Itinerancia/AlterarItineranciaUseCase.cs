using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public class AlterarItineranciaUseCase : AbstractUseCase, IAlterarItineranciaUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public AlterarItineranciaUseCase(IUnitOfWork unitOfWork, IMediator mediator) : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<AuditoriaDto> Executar(ItineranciaDto dto)
        {
            var itinerancia = await mediator.Send(new ObterItineranciaPorIdQuery(dto.Id));

            var dataRetornoAlterada = itinerancia.DataRetornoVerificacao != dto.DataRetornoVerificacao;
            var dataRetornoAnterior = itinerancia.DataRetornoVerificacao;

            if (itinerancia == null)
                throw new NegocioException($"Não foi possível localizar a itinerância de Id {dto.Id}");

            itinerancia.AnoLetivo = dto.AnoLetivo;
            itinerancia.DataVisita = dto.DataVisita;
            itinerancia.DataRetornoVerificacao = dto.DataRetornoVerificacao;            
            itinerancia.EventoId = dto.EventoId;

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    var auditoriaDto = await mediator.Send(new AlterarItineranciaCommand(itinerancia));
                    if(auditoriaDto == null)
                        throw new NegocioException($"Não foi possível alterar a itinerância de Id {itinerancia.Id}");
                    
                    await ExluirFilhosItinerancia(itinerancia);
                    
                    await SalvarFilhosItinerancia(dto, itinerancia);

                    if (dataRetornoAlterada)
                        await AltararDataEventosItinerancias(dto, dataRetornoAnterior);

                    unitOfWork.PersistirTransacao();

                    await EnviarNotificacao(itinerancia, dto);

                    return auditoriaDto;
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        private async Task AltararDataEventosItinerancias(ItineranciaDto dto, DateTime? dataRetornoAnterior)
        {
            if (UsuarioRemoveuDataDeRetorno(dto))
                await RemoverEventosItinerancia(dto.Id);
            else if (UsuarioAlterouDataDeRetorno(dto, dataRetornoAnterior))
                await AtualizaDatasEventos(dto.Id, dto.DataRetornoVerificacao);
            else // UsuarioIncluiuDataRetorno
                await SalvarEventosItinerancia(dto);
        }

        private bool UsuarioAlterouDataDeRetorno(ItineranciaDto dto, DateTime? dataRetornoAnterior)
            => dataRetornoAnterior.HasValue && dto.DataRetornoVerificacao.HasValue;

        private bool UsuarioRemoveuDataDeRetorno(ItineranciaDto dto)
            => !dto.DataRetornoVerificacao.HasValue;


        private async Task SalvarEventosItinerancia(ItineranciaDto dto)
        {
            foreach (var ue in dto.Ues)
                await mediator.Send(new CriarEventoItineranciaPAAICommand(dto.Id, ue.CodigoDre, ue.CodigoUe, dto.DataRetornoVerificacao.Value, dto.DataVisita, dto.ObjetivosVisita));
        }

        private async Task AtualizaDatasEventos(long id, DateTime? dataRetornoVerificacao)
        {
            await mediator.Send(new AtualizarDatasEventosItineranciaCommand(id, dataRetornoVerificacao.Value));
        }

        private async Task RemoverEventosItinerancia(long id)
        {
            await mediator.Send(new RemoverEventosItineranciaCommand(id));
        }

        private async Task EnviarNotificacao(Itinerancia itinerancia, ItineranciaDto dto)
        {
            SentrySdk.AddBreadcrumb($"Mensagem RotaNotificacaoRegistroItineranciaInseridoUseCase", "Rabbit - RotaNotificacaoRegistroItineranciaInseridoUseCase");

            var verificaWorkflow = await mediator.Send(new ObterWorkflowItineranciaPorItineranciaIdQuery(itinerancia.Id));
            WorkflowAprovacao workflow = null;

            if (verificaWorkflow != null)
                workflow = await mediator.Send(new ObterWorkflowPorIdQuery(verificaWorkflow.WfAprovacaoId));

            if(workflow == null || workflow.Niveis.Any(a => a.Status == WorkflowAprovacaoNivelStatus.Reprovado))
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.RotaNotificacaoRegistroItineranciaInseridoUseCase,
                    new NotificacaoSalvarItineranciaDto
                    {
                        CriadoRF = itinerancia.CriadoRF,
                        CriadoPor = itinerancia.CriadoPor,
                        DataVisita = dto.DataVisita,
                        Ues = dto.Ues,
                        Estudantes = dto.Alunos,
                        ItineranciaId = itinerancia.Id
                    }, Guid.NewGuid(), null));
        }

        public async Task<bool> ExluirFilhosItinerancia(Itinerancia itinerancia)
        {
            if (itinerancia.Alunos == null || itinerancia.Alunos.Any())
                foreach (var aluno in itinerancia.Alunos)
                    if (!await mediator.Send(new ExcluirItineranciaAlunoCommand(aluno)))
                        throw new NegocioException($"Não foi possível excluir a itinerância do aluno de Id {aluno.Id}");

            if (itinerancia.ObjetivosVisita == null || itinerancia.ObjetivosVisita.Any())
                foreach (var objetivo in itinerancia.ObjetivosVisita)
                    if (!await mediator.Send(new ExcluirItineranciaObjetivoCommand(objetivo.Id)))
                        throw new NegocioException($"Não foi possível excluir o objetivo da itinerância de Id {objetivo.Id}");

            if (itinerancia.Questoes == null || itinerancia.Questoes.Any())
                foreach (var questao in itinerancia.Questoes)
                    if (!await mediator.Send(new ExcluirItineranciaQuestaoCommand(questao.Id)))
                        throw new NegocioException($"Não foi possível excluir a quesão da itinerância de Id {questao.Id}");

            if (itinerancia.Ues == null || itinerancia.Ues.Any())
                foreach (var ue in itinerancia.Ues)
                    if (!await mediator.Send(new ExcluirItineranciaUeCommand(ue.Id)))
                        throw new NegocioException($"Não foi possível excluir a ue da itinerância de Id {ue.Id}");

            return true;
        }

        public async Task<bool> SalvarFilhosItinerancia(ItineranciaDto itineranciaDto, Itinerancia itinerancia)
        {
            if (itineranciaDto.Alunos.Any())
            {
                await TrataTurmasCodigos(itineranciaDto);
            }
            if (itineranciaDto.Alunos == null || itineranciaDto.Alunos.Any())
                foreach (var aluno in itineranciaDto.Alunos)
                    await mediator.Send(new SalvarItineranciaAlunoCommand(aluno, itinerancia.Id));

            if (itineranciaDto.ObjetivosVisita == null || itineranciaDto.ObjetivosVisita.Any())
                foreach (var objetivo in itineranciaDto.ObjetivosVisita)
                    await mediator.Send(new SalvarItineranciaObjetivoCommand(objetivo.ItineranciaObjetivoBaseId, itinerancia.Id, objetivo.Descricao, objetivo.TemDescricao));

            if (itineranciaDto.Questoes == null || itineranciaDto.Questoes.Any())
                foreach (var questao in itineranciaDto.Questoes)
                    await mediator.Send(new SalvarItineranciaQuestaoCommand(questao.QuestaoId, itinerancia.Id, questao.Resposta));

            if (itineranciaDto.Ues == null || itineranciaDto.Ues.Any())
                foreach (var ue in itineranciaDto.Ues)
                    await mediator.Send(new SalvarItineranciaUeCommand(ue.UeId, itinerancia.Id));

            return true;
        }

        private async Task TrataTurmasCodigos(ItineranciaDto itineranciaDto)
        {
            var turmasCodigos = itineranciaDto.Alunos.Select(a => a.TurmaId.ToString()).Distinct().ToList();

            if (turmasCodigos != null && turmasCodigos.Any())
            {
                var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(turmasCodigos.ToArray()));
                if (turmas.Count() != turmasCodigos.Count())
                    throw new NegocioException("Não foi possível localizar as turmas no SGP.");

                foreach (var item in itineranciaDto.Alunos)
                {
                    item.TurmaId = turmas.FirstOrDefault(a => a.CodigoTurma == item.TurmaId.ToString()).Id;
                }
            }
        }
    }
}
