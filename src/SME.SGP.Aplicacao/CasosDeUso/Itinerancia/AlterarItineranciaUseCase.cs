using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
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
            itinerancia.DreId = dto.DreId;
            itinerancia.UeId = dto.UeId;

            await ExcluirFilhosItinerancia(dto, itinerancia);

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    var auditoriaDto = await mediator.Send(new AlterarItineranciaCommand(itinerancia));
                    if (auditoriaDto == null)
                        throw new NegocioException($"Não foi possível alterar a itinerância de Id {itinerancia.Id}");

                    await SalvarFilhosItinerancia(dto, itinerancia);

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
            var ue = await mediator.Send(new ObterUePorIdQuery(dto.UeId));
            if (ue == null)
                throw new NegocioException("Não foi possível localizar um Unidade Escolar!");

            await mediator.Send(new CriarEventoItineranciaPAAICommand(dto.Id, ue.Dre.CodigoDre, ue.CodigoUe, dto.DataRetornoVerificacao.Value, dto.DataVisita, ObterObjetivos(dto.ObjetivosVisita)));
        }

        private IEnumerable<ItineranciaObjetivoDescricaoDto> ObterObjetivos(IEnumerable<ItineranciaObjetivoDto> objetivosVisita)
            => objetivosVisita.Select(a => (ItineranciaObjetivoDescricaoDto)a);

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
            var verificaWorkflow = await mediator.Send(new ObterWorkflowItineranciaPorItineranciaIdQuery(itinerancia.Id));
            WorkflowAprovacao workflow = null;

            if (verificaWorkflow != null)
                workflow = await mediator.Send(new ObterWorkflowPorIdQuery(verificaWorkflow.WfAprovacaoId));

            if (workflow == null || workflow.Niveis.Any(a => a.Status == WorkflowAprovacaoNivelStatus.Reprovado))
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoRegistroItineranciaInseridoUseCase,
                    new NotificacaoSalvarItineranciaDto
                    {
                        CriadoRF = itinerancia.CriadoRF,
                        CriadoPor = itinerancia.CriadoPor,
                        DataVisita = dto.DataVisita,
                        UeId = dto.UeId,
                        Estudantes = dto.Alunos,
                        ItineranciaId = itinerancia.Id
                    }, Guid.NewGuid(), null));
        }

        public async Task<bool> ExcluirFilhosItinerancia(ItineranciaDto itineranciaDto, Itinerancia itinerancia)
        {
            if (itineranciaDto.PossuiAlunos)
                foreach (var aluno in itinerancia.Alunos)
                    if (!await mediator.Send(new ExcluirItineranciaAlunoCommand(aluno)))
                        throw new NegocioException($"Não foi possível excluir a itinerância do aluno de Id {aluno.Id}");

            if (itineranciaDto.PossuiObjetivos)
                foreach (var objetivo in itinerancia.ObjetivosVisita)
                    if (!await mediator.Send(new ExcluirItineranciaObjetivoCommand(objetivo.Id, itinerancia.Id)))
                        throw new NegocioException($"Não foi possível excluir o objetivo da itinerância de Id {objetivo.Id}");

            if (itineranciaDto.PossuiQuestoes)
                foreach (var questao in itinerancia.Questoes)
                    if (!await mediator.Send(new ExcluirItineranciaQuestaoCommand(questao.Id, itinerancia.Id)))
                        throw new NegocioException($"Não foi possível excluir a questão da itinerância de Id {questao.Id}");

            return true;
        }

        public async Task<bool> SalvarFilhosItinerancia(ItineranciaDto itineranciaDto, Itinerancia itinerancia)
        {
            if (itineranciaDto.PossuiAlunos)
            {
                await TrataTurmasCodigos(itineranciaDto);
                foreach (var aluno in itineranciaDto.Alunos)
                    await mediator.Send(new SalvarItineranciaAlunoCommand(aluno, itinerancia.Id));
            }

            if (itineranciaDto.PossuiObjetivos)
                foreach (var objetivo in itineranciaDto.ObjetivosVisita)
                    await mediator.Send(new SalvarItineranciaObjetivoCommand(objetivo.ItineranciaObjetivoBaseId, itinerancia.Id, objetivo.Descricao, objetivo.TemDescricao));

            if (itineranciaDto.PossuiQuestoes)
                foreach (var questao in itineranciaDto.Questoes)
                    await mediator.Send(new SalvarItineranciaQuestaoCommand(questao.QuestaoId, itinerancia.Id, questao.Resposta));

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
