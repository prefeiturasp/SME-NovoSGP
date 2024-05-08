using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class CopiarRelatorioPAPUseCase : AbstractUseCase, ICopiarRelatorioPAPUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioCache repositorioCache;
        public CopiarRelatorioPAPUseCase(IMediator mediator,IUnitOfWork unitOfWork,IRepositorioCache repositorioCache) : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<bool> Executar(CopiarPapDto copiarPapDto)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(copiarPapDto.CodigoTurma));
            var obterSecoesOrigem = await mediator.Send(new ObterSecoesPAPQuery(copiarPapDto.CodigoTurmaOrigem, copiarPapDto.CodigoAlunoOrigem, copiarPapDto.PeriodoRelatorioPAPId));
            unitOfWork.IniciarTransacao();
            try
            {
                foreach (var estudante in copiarPapDto.Estudantes)
                {

                    var obterSecoesDestino = await mediator.Send(new ObterSecoesPAPQuery(copiarPapDto.CodigoTurma,
                        estudante.AlunoCodigo, copiarPapDto.PeriodoRelatorioPAPId));
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
                        var sessaoDestino = new RelatorioPAPSecaoDto()
                            { Id = questionarioSecaoId.PAPSecaoId, SecaoId = questionarioSecaoId.Id };
                        var secaoOrigem = obterSecoesOrigem.Secoes.FirstOrDefault(x => x.Id == questionarioSecaoId.Id);
                        var questoesOrigem = await ObterQuestoesOrigem(copiarPapDto, secaoOrigem);

                        var questoesDestino = await mediator.Send(new ObterQuestionarioPAPPorPeriodoQuery(
                            copiarPapDto.CodigoTurma, estudante.AlunoCodigo,
                            copiarPapDto.PeriodoRelatorioPAPId, questionarioSecaoId.QuestionarioId,
                            questionarioSecaoId.PAPSecaoId));

                        foreach (var questao in questoesDestino)
                        {
                            if (copiarPapDto.Secoes.SelectMany(s => s.QuestoesIds).Contains(questao.Id))
                            {
                                var questaoOrigem = questoesOrigem.FirstOrDefault(x => x.Id == questao.Id);
                                CriarRelatorioPAPRespostaDto(questaoOrigem, sessaoDestino, true);
                            }
                            else
                            {
                                CriarRelatorioPAPRespostaDto(questao, sessaoDestino, false);
                            }
                        }

                        relatorioPAPDto.Secoes.Add(sessaoDestino);

                    }

                    await mediator.Send(new PersistirRelatorioPAPCommand(relatorioPAPDto));
                    unitOfWork.PersistirTransacao();
                }
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }
            return true;
        }

        private async Task<IEnumerable<QuestaoDto>> ObterQuestoesOrigem(CopiarPapDto copiarPapDto, SecaoPAPDto secaoOrigem)
        {
            var chaveCache = string.Format(NomeChaveCache.OBTER_QUESTOES_ORIGEM_RELATORIO_PAP, copiarPapDto.CodigoTurmaOrigem, copiarPapDto.CodigoAlunoOrigem,secaoOrigem.PAPSecaoId);
            return await repositorioCache.ObterAsync(chaveCache,
                async () => await mediator.Send(new ObterQuestionarioPAPPorPeriodoQuery(copiarPapDto.CodigoTurmaOrigem,
                    copiarPapDto.CodigoAlunoOrigem, copiarPapDto.PeriodoRelatorioPAPId, secaoOrigem.QuestionarioId,
                    secaoOrigem.PAPSecaoId)));
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