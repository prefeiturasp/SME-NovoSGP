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
            foreach (var estudante in copiarPapDto.Estudantes)
            {

                var obterSecoesOrigem = await mediator.Send(new ObterSecoesPAPQuery(copiarPapDto.CodigoTurmaOrigem, copiarPapDto.CodigoAlunoOrigem, copiarPapDto.PeriodoRelatorioPAPId));
                var relatorioPAPDto = new RelatorioPAPDto()
                {
                    periodoRelatorioPAPId = copiarPapDto.PeriodoRelatorioPAPId,
                    TurmaId = turma.Id,
                    AlunoCodigo = estudante.AlunoCodigo,
                    AlunoNome = estudante.AlunoNome,
                    PAPTurmaId = obterSecoesOrigem.PAPTurmaId,
                    PAPAlunoId = obterSecoesOrigem.PAPAlunoId
                };
                
                foreach (var questionarioSecaoId in obterSecoesOrigem.Secoes)
                {
                    var sessao = new RelatorioPAPSecaoDto()
                    {
                        Id = questionarioSecaoId.Id,
                        SecaoId = questionarioSecaoId.Id
                    };
                    var questoes = await mediator.Send(new ObterQuestionarioPAPPorPeriodoQuery(copiarPapDto.CodigoTurmaOrigem,
                                                                                                                        copiarPapDto.CodigoAlunoOrigem,
                                                                                                                        copiarPapDto.PeriodoRelatorioPAPId,
                                                                                                                        questionarioSecaoId.QuestionarioId,
                                                                                                                        questionarioSecaoId.Id));

                    foreach (var questao in questoes)
                    {
                        foreach (var respostas in questao.Resposta)
                        {
                            var resposta = new RelatorioPAPRespostaDto()
                            {
                                RelatorioRespostaId = respostas.Id,
                                Resposta = respostas.Texto, 
                                QuestaoId = questao.Id,
                                TipoQuestao = questao.TipoQuestao
                            };
                            sessao.Respostas.Add(resposta);
                        }
                    }
                    
                    relatorioPAPDto.Secoes.Add(sessao);

                }
                await mediator.Send(new PersistirRelatorioPAPCommand(relatorioPAPDto));
            }
            return true;
        }
    }
}