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
            var listaDeQuestoesIds = new List<long>();
            var secoesIds = copiarPapDto.Secoes.Select(x => x.SecaoId);
            var questoesIds = copiarPapDto.Secoes.Select(dto => dto.QuestoesIds);
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(copiarPapDto.CodigoTurma));
            foreach (var ids in questoesIds) 
                listaDeQuestoesIds.AddRange(ids);
            
            foreach (var estudante in copiarPapDto.Estudantes)
            {

                var obterSecoes = await mediator.Send(new ObterSecoesPAPQuery(copiarPapDto.CodigoTurma, estudante.AlunoCodigo, copiarPapDto.PeriodoRelatorioPAPId));
                var relatorioPAPDto = new RelatorioPAPDto()
                {
                    periodoRelatorioPAPId = copiarPapDto.PeriodoRelatorioPAPId,
                    TurmaId = turma.Id,
                    AlunoCodigo = estudante.AlunoCodigo,
                    AlunoNome = estudante.AlunoNome,
                    PAPTurmaId = obterSecoes.PAPTurmaId,
                    PAPAlunoId = obterSecoes.PAPAlunoId
                };
                var questionariosSecaoIds = obterSecoes.Secoes.Where(x => secoesIds.Contains(x.Id));

                var questoesSelecionadas =
                    questionariosSecaoIds.Where(x => listaDeQuestoesIds.Contains(x.QuestionarioId));
                
                
                foreach (var questionarioSecaoId in questoesSelecionadas)
                {
                    var sessao = new RelatorioPAPSecaoDto()
                    {
                        Id = questionarioSecaoId.Id,
                        SecaoId = questionarioSecaoId.Id
                    };
                    var questoes = await mediator.Send(new ObterQuestionarioPAPPorPeriodoQuery(copiarPapDto.CodigoTurma,
                                                                                                                        estudante.AlunoCodigo,
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