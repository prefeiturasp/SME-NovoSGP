using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class CopiarRelatorioPAPUseCase : AbstractUseCase, ICopiarRelatorioPAPUseCase
    {
        public CopiarRelatorioPAPUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(CopiarPapDto copiarPapDto)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(copiarPapDto.CodigoTurma));
            var obterSecoesOrigem = await mediator.Send(new ObterSecoesPAPQuery(copiarPapDto.CodigoTurmaOrigem, copiarPapDto.CodigoAlunoOrigem, copiarPapDto.PeriodoRelatorioPAPId));
            foreach (var estudante in copiarPapDto.Estudantes)
            {

                var obterSecoesDestino = await mediator.Send(new ObterSecoesPAPQuery(copiarPapDto.CodigoTurma, estudante.AlunoCodigo, copiarPapDto.PeriodoRelatorioPAPId));
                var relatorioPAPDto = new RelatorioPAPDto()
                {
                    periodoRelatorioPAPId = copiarPapDto.PeriodoRelatorioPAPId,
                    TurmaId = turma.Id,
                    AlunoCodigo = estudante.AlunoCodigo,
                    AlunoNome = estudante.AlunoNome,
                    PAPTurmaId = obterSecoesDestino.PAPTurmaId,
                    PAPAlunoId = obterSecoesDestino.PAPAlunoId
                };
                
                foreach (var questionarioSecaoId in obterSecoesDestino.Secoes)
                {
                    var sessao = new RelatorioPAPSecaoDto()
                    {
                        Id = questionarioSecaoId.PAPSecaoId,
                        SecaoId = questionarioSecaoId.Id
                    };
                    var secaoOrigem = obterSecoesOrigem.Secoes.FirstOrDefault(x => x.Id == questionarioSecaoId.Id);
                    var questoesOrigem = await mediator.Send(new ObterQuestionarioPAPPorPeriodoQuery(copiarPapDto.CodigoTurmaOrigem,
                                                                                                                        copiarPapDto.CodigoAlunoOrigem,
                                                                                                                        copiarPapDto.PeriodoRelatorioPAPId,
                                                                                                                        secaoOrigem.QuestionarioId,
                                                                                                                        secaoOrigem.PAPSecaoId));
                    
                    var questoesDestino = await mediator.Send(new ObterQuestionarioPAPPorPeriodoQuery(copiarPapDto.CodigoTurma,
                                                                                                                                        estudante.AlunoCodigo,
                                                                                                                                        copiarPapDto.PeriodoRelatorioPAPId,
                                                                                                                                        questionarioSecaoId.QuestionarioId,
                                                                                                                                        questionarioSecaoId.PAPSecaoId));

                    foreach (var questao in questoesDestino)
                    {
                        if (copiarPapDto.Secoes.SelectMany(s => s.QuestoesIds).Contains(questao.Id))
                        {
                            var questaoOrigem = questoesOrigem.FirstOrDefault(x => x.Id == questao.Id);
                            CriarRelatorioPAPRespostaDto(questaoOrigem, sessao, true);
                        }
                        else
                        {
                            CriarRelatorioPAPRespostaDto(questao, sessao, false);
                        }
                    }
                    
                    relatorioPAPDto.Secoes.Add(sessao);

                }
                await mediator.Send(new PersistirRelatorioPAPCommand(relatorioPAPDto));
            }
            return true;
        }

        private static void CriarRelatorioPAPRespostaDto(QuestaoDto questao, RelatorioPAPSecaoDto sessao, bool ehquestaoOrigem)
        {
            foreach (var respostas in questao.Resposta)
            {
                var resposta = new RelatorioPAPRespostaDto()
                {
                    RelatorioRespostaId = ehquestaoOrigem ? null : respostas.Id,
                    Resposta = respostas.OpcaoRespostaId?.ToString() ?? respostas.Texto,  
                    QuestaoId = questao.Id,
                    TipoQuestao = questao.TipoQuestao
                };
                sessao.Respostas.Add(resposta);
            }
        }
    }
}