using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterItineranciaPorIdUseCase : AbstractUseCase, IObterItineranciaPorIdUseCase
    {
        public ObterItineranciaPorIdUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ItineranciaDto> Executar(long id)
        {
            var itinerancia = await mediator.Send(new ObterItineranciaPorIdQuery(id));

            if (itinerancia.EhNulo())
                throw new NegocioException($"Não foi possível localizar a itinerância de Id {id}");

            var questoesBase = await mediator.Send(ObterQuestoesBaseItineranciaEAlunoQuery.Instance);

            var verificaWorkflow = await mediator.Send(new ObterWorkflowItineranciaPorItineranciaIdQuery(itinerancia.Id));
            WorkflowAprovacao workflow = new WorkflowAprovacao();

            if (verificaWorkflow.NaoEhNulo())
                workflow = await mediator.Send(new ObterWorkflowPorIdQuery(verificaWorkflow.WfAprovacaoId));

            var itineranciaDto = new ItineranciaDto()
            {
                AnoLetivo = itinerancia.AnoLetivo,
                DataVisita = itinerancia.DataVisita,
                DataRetornoVerificacao = itinerancia.DataRetornoVerificacao,
                ObjetivosVisita = MontarObjetivosItinerancia(itinerancia),
                Questoes = await MontarQuestoesItinerancia(itinerancia, questoesBase),
                TipoCalendarioId = await ObterTipoCalendario(itinerancia.EventoId),
                DreId = itinerancia.DreId,
                UeId = itinerancia.UeId,
                EventoId = itinerancia.EventoId,
                CriadoRF = itinerancia.CriadoRF,
                Auditoria = (AuditoriaDto)itinerancia,
                StatusWorkflow = workflow.NaoEhNulo() ? ObterMensagemStatus(workflow.Niveis, verificaWorkflow.StatusAprovacao) : "",
                PodeEditar = workflow.NaoEhNulo() ? VerificaPodeEditar(workflow.Niveis) : true
            };

            if (itinerancia.Alunos.NaoEhNulo() && itinerancia.Alunos.Any())
            {
                var CodigosAluno = itinerancia.Alunos.Select(a => a.CodigoAluno).ToArray();

                var alunosEol = await mediator.Send(new ObterAlunosEolPorCodigosQuery(CodigosAluno.Select(long.Parse).ToArray()));

                var turmasIds = itinerancia.Alunos.Select(al => al.TurmaId).Distinct().ToArray();

                var turmas = await mediator.Send(new ObterTurmasPorIdsQuery(turmasIds));

                itineranciaDto.Alunos = MontarAlunosItinerancia(itinerancia, alunosEol, questoesBase, turmas);
            }

            return itineranciaDto;
        }

        private bool VerificaPodeEditar(IEnumerable<WorkflowAprovacaoNivel> niveis)
        {
            if (niveis.FirstOrDefault(a => a.Status == WorkflowAprovacaoNivelStatus.Reprovado).NaoEhNulo())
                return true;
            else
                return false;
        }

        private string ObterMensagemStatus(IEnumerable<WorkflowAprovacaoNivel> niveis, bool statusAprovacao)
        {
            if (statusAprovacao)
            {
                var nivel = niveis.Where(a => a.Status == WorkflowAprovacaoNivelStatus.Aprovado).OrderByDescending(b => b.AlteradoEm).FirstOrDefault();
                return $"Aceito por {nivel.AlteradoPor} ({nivel.AlteradoRF}) em {nivel.AlteradoEm:dd/MM/yyy HH:mm}";
            }
            else if (niveis.FirstOrDefault(a => a.Status == WorkflowAprovacaoNivelStatus.Excluido).NaoEhNulo())
            {
                var nivel = niveis.Where(a => a.Status == WorkflowAprovacaoNivelStatus.Excluido).OrderByDescending(b => b.AlteradoEm).FirstOrDefault();
                return $"Excluído por {nivel.AlteradoPor} ({nivel.AlteradoRF}) em {nivel.AlteradoEm:dd/MM/yyy HH:mm}";
            }
            else if (niveis.FirstOrDefault(a => a.Status == WorkflowAprovacaoNivelStatus.Reprovado).NaoEhNulo())
            {
                var nivel = niveis.Where(a => a.Status == WorkflowAprovacaoNivelStatus.Reprovado).OrderByDescending(b => b.AlteradoEm).FirstOrDefault();
                return $"Reprovado por {nivel.AlteradoPor} ({nivel.AlteradoRF}) em {nivel.AlteradoEm:dd/MM/yyy HH:mm}";
            }
            else if (niveis.FirstOrDefault(a => a.Status == WorkflowAprovacaoNivelStatus.Substituido).NaoEhNulo())
            {
                var nivel = niveis.Where(a => a.Status == WorkflowAprovacaoNivelStatus.Substituido).OrderByDescending(b => b.AlteradoEm).FirstOrDefault();
                return $"Substituído por {nivel.AlteradoPor} ({nivel.AlteradoRF}) em {nivel.AlteradoEm:dd/MM/yyy HH:mm}";
            }
            else if (niveis.FirstOrDefault(a => a.Status == WorkflowAprovacaoNivelStatus.AguardandoAprovacao).NaoEhNulo())
            {
                return $"Aguardando aprovação";
            }
            else
                return "Sem status informado";
        }
        
        private async Task<long> ObterTipoCalendario(long? eventoId)
            => eventoId.HasValue ? await mediator.Send(new ObterTipoCalendarioIdPorEventoQuery(eventoId.Value)) : 0;

        private async Task<IEnumerable<ItineranciaQuestaoDto>> MontarQuestoesItinerancia(Itinerancia itinerancia, ItineranciaQuestoesBaseDto questoesBase)
        {            
            var tiposQuestoes = await mediator.Send(new ObterTipoDaQuestaoItineranciaQuery(itinerancia.Id));
            var questoesItinerancia = itinerancia.Questoes.Select(questao => {
                var questaoBase = questoesBase.ItineranciaQuestao.FirstOrDefault(q => q.QuestaoId == questao.QuestaoId);
                var arquivo = (questaoBase?.TipoQuestao == TipoQuestao.Upload) ? tiposQuestoes.FirstOrDefault(x => x.QuestaoId == questao.Id) : null;

                return new ItineranciaQuestaoDto
                {
                    Id = questao.Id,
                    QuestaoId = questao.QuestaoId,
                    Descricao = questaoBase?.Descricao,
                    ItineranciaId = questao.ItineranciaId,
                    Resposta = questao.Resposta,
                    NomeComponente = questaoBase?.NomeComponente,
                    Obrigatorio = questaoBase?.Obrigatorio,
                    TipoQuestao = (questaoBase?.TipoQuestao ?? TipoQuestao.Texto),
                    ArquivoId = arquivo?.ArquivoId,
                    ArquivoNome = arquivo?.ArquivoNome
            };
            }).ToList();

            var questoesUploadNaoRespondidas = questoesBase.ItineranciaQuestao.Where(questaoBase => !itinerancia.Questoes.Any(questao => questao.QuestaoId == questaoBase.QuestaoId));
            if (questoesUploadNaoRespondidas.Any())
            {
                questoesItinerancia.AddRange(questoesUploadNaoRespondidas.Select(questao => new ItineranciaQuestaoDto
                {
                    Id = questao.Id,
                    QuestaoId = questao.QuestaoId,
                    Descricao = questao.Descricao,
                    ItineranciaId = itinerancia.Id,
                    Resposta = questao.Resposta,
                    Obrigatorio = questao.Obrigatorio,
                    TipoQuestao = questao.TipoQuestao
                }));
            }
            return questoesItinerancia;
        }

        private IEnumerable<ItineranciaObjetivoDto> MontarObjetivosItinerancia(Itinerancia itinerancia)
        {
            return itinerancia.ObjetivosVisita.Select(o =>
            {
                return new ItineranciaObjetivoDto
                {
                    Id = o.Id,
                    ItineranciaObjetivoBaseId = o.ItineranciaObjetivosBaseId,
                    Nome = itinerancia.ObjetivosBase.FirstOrDefault(ob => ob.Id == o.ItineranciaObjetivosBaseId).Nome,
                    TemDescricao = itinerancia.ObjetivosBase.FirstOrDefault(ob => ob.Id == o.ItineranciaObjetivosBaseId).TemDescricao,
                    Descricao = string.IsNullOrEmpty(o.Descricao) ? "" : o.Descricao
                };
            });
        }

        private IEnumerable<ItineranciaAlunoDto> MontarAlunosItinerancia(Itinerancia itinerancia, IEnumerable<TurmasDoAlunoDto> alunosEol, ItineranciaQuestoesBaseDto questoesBase, IEnumerable<Turma> turmas)
        {
            return itinerancia.Alunos.Select(aluno =>
            {
                var alunoEol = alunosEol.FirstOrDefault(a => a.CodigoAluno == int.Parse(aluno.CodigoAluno));
                return new ItineranciaAlunoDto
                {
                    Id = aluno.Id,
                    AlunoCodigo = aluno.CodigoAluno,
                    TurmaId = alunoEol.CodigoTurma,
                    AlunoNome = alunoEol.NomeAluno,
                    NomeAlunoComTurmaModalidade = $"{alunoEol.NomeAluno} - {turmas.FirstOrDefault(t => t.Id == aluno.TurmaId).NomeComModalidade()}",
                    Questoes = MontarQuestoesItineranciaAluno(aluno, questoesBase)
                };
            });
        }

        private IEnumerable<ItineranciaAlunoQuestaoDto> MontarQuestoesItineranciaAluno(ItineranciaAluno aluno, ItineranciaQuestoesBaseDto questoesBase)
        {
            return aluno.AlunosQuestoes.Select(questao =>
            {
                return new ItineranciaAlunoQuestaoDto
                {
                    QuestaoId = questao.QuestaoId,
                    Descricao = questoesBase.ItineranciaAlunoQuestao.FirstOrDefault(q => q.QuestaoId == questao.QuestaoId).Descricao,
                    ItineranciaAlunoId = questao.ItineranciaAlunoId,
                    Resposta = questao.Resposta,
                    Obrigatorio = questoesBase.ItineranciaAlunoQuestao.FirstOrDefault(q => q.QuestaoId == questao.QuestaoId).Obrigatorio,
                    NomeComponente = questoesBase.ItineranciaAlunoQuestao.FirstOrDefault(q => q.QuestaoId == questao.QuestaoId).NomeComponente
                };
            });
        }
    }
}
