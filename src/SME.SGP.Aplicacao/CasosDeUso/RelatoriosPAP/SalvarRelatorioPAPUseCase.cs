using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarRelatorioPAPUseCase : AbstractUseCase, ISalvarRelatorioPAPUseCase
    {
        public SalvarRelatorioPAPUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ResultadoRelatorioPAPDto> Executar(RelatorioPAPDto relatorioPAPDto)
        {
            var turmaCodigo = await mediator.Send(new ObterTurmaCodigoPorIdQuery(relatorioPAPDto.TurmaId));
            if (string.IsNullOrEmpty(turmaCodigo))
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(relatorioPAPDto.AlunoCodigo, DateTime.Now.Year));
            if (aluno == null)
                throw new NegocioException(MensagemNegocioAluno.ESTUDANTE_NAO_ENCONTRADO);

            await ValidarQuestoesObrigatorias(relatorioPAPDto, turmaCodigo);

            if (relatorioPAPDto.PAPTurmaId.HasValue)
                return await AlterarRelatorioPAP(relatorioPAPDto);

            return await SalvarRelatorioPAP(relatorioPAPDto);
        }

        private async Task<ResultadoRelatorioPAPDto> AlterarRelatorioPAP(RelatorioPAPDto relatorioPAPDto)
        {
            var resultado = new ResultadoRelatorioPAPDto();
            var relatorioAluno = await PersistirRelatorioAluno(relatorioPAPDto, relatorioPAPDto.PAPTurmaId.Value);

            resultado.PAPTurmaId = relatorioPAPDto.PAPTurmaId.Value;
            resultado.PAPAlunoId = relatorioAluno.Id;

            foreach (var secao in relatorioPAPDto.Secoes)
            {
                resultado.Secoes.Add(await AlterarSecao(secao, relatorioAluno.Id));
            }

            return resultado;
        }

        private async Task<ResultadoRelatorioPAPSecaoDto> AlterarSecao(RelatorioPAPSecaoDto secao, long relatorioAlunoId)
        {
            var relatorioSecao = await PersistirRelatorioSecao(secao, relatorioAlunoId);
            
            foreach (var questaoRespostas in secao.Respostas.GroupBy(q => q.QuestaoId))
            {
                var questaoExistente = relatorioSecao.Questoes.FirstOrDefault(q => q.QuestaoId == questaoRespostas.Key);

                if (questaoExistente == null)
                    await SalvarQuestao(relatorioSecao.Id, questaoRespostas.Key, questaoRespostas);
                else
                    await AlterarQuestao(questaoExistente, questaoRespostas);
            }

            return ObterResultadoSecao(relatorioSecao, secao.SecaoId);
        }

        private async Task<RelatorioPeriodicoPAPSecao> PersistirRelatorioSecao(RelatorioPAPSecaoDto secao, long relatorioAlunoId)
        {
            if (!secao.Respostas.Any())
                throw new NegocioException(string.Format(MensagemNegocioComuns.NENHUMA_QUESTAO_FOI_ENCONTRADA_NA_SECAO_X, secao.SecaoId));

            if (secao.Id.HasValue)
            {
                var relatorioSecao = await mediator.Send(new ObterRelatorioPeriodicoSecaoPAPQuery(secao.Id.Value));

                await mediator.Send(new AlterarRelatorioPeriodicoSecaoPAPCommand(relatorioSecao));

                return relatorioSecao;
            }

            return await mediator.Send(new SalvarRelatorioPeriodicoSecaoPAPCommand(secao.SecaoId, relatorioAlunoId)); 
        }

        private async Task<ResultadoRelatorioPAPDto> SalvarRelatorioPAP(RelatorioPAPDto relatorioPAPDto)
        {
            var resultado = new ResultadoRelatorioPAPDto();
            var relatorioTurma = await mediator.Send(new SalvarRelatorioPeriodicoTurmaPAPCommand(relatorioPAPDto.TurmaId, relatorioPAPDto.periodoRelatorioPAPId));
            var relatorioAluno = await PersistirRelatorioAluno(relatorioPAPDto, relatorioTurma.Id);

            resultado.PAPTurmaId = relatorioTurma.Id;
            resultado.PAPAlunoId = relatorioAluno.Id;

            foreach (var secao in relatorioPAPDto.Secoes)
            {
                resultado.Secoes.Add(await SalvarSecao(secao, relatorioAluno.Id));
            }

            return resultado;
        }

        private async Task<ResultadoRelatorioPAPSecaoDto> SalvarSecao(RelatorioPAPSecaoDto secao, long relatorioAlunoId)
        {
            var relatorioSecao = await PersistirRelatorioSecao(secao, relatorioAlunoId);

            foreach (var questoes in secao.Respostas.GroupBy(q => q.QuestaoId))
            {
                await SalvarQuestao(relatorioSecao.Id, questoes.Key, questoes);
            }

            return ObterResultadoSecao(relatorioSecao, secao.SecaoId);
        }

        private async Task SalvarQuestao(long relatorioSecaoId, long questaoId, IEnumerable<RelatorioPAPRespostaDto> respostas)
        {
            var relatorioQuestao = await mediator.Send(new SalvarRelatorioPeriodicoQuestaoPAPCommand(relatorioSecaoId, questaoId));

            await SalvarResposta(respostas, relatorioQuestao.Id);
        }

        private async Task AlterarQuestao(RelatorioPeriodicoPAPQuestao questaoExistente, IEnumerable<RelatorioPAPRespostaDto> respostas)
        {
            if (questaoExistente.Excluido)
                await AlterarQuestaoExcluida(questaoExistente);

            await ExcluirRespostasEncaminhamento(questaoExistente, respostas);

            await AlterarRespostasEncaminhamento(questaoExistente, respostas);

            await IncluirRespostasEncaminhamento(questaoExistente, respostas);
        }

        private async Task AlterarQuestaoExcluida(RelatorioPeriodicoPAPQuestao questao)
        {
            questao.Excluido = false;
            await mediator.Send(new AlterarRelatorioPeriodicoQuestaoPAPCommand(questao));
        }

        private async Task AlterarRespostasEncaminhamento(RelatorioPeriodicoPAPQuestao questaoExistente, IEnumerable<RelatorioPAPRespostaDto> respostas)
        {
            var repostasAlteradas = questaoExistente.Respostas.Where(s => respostas.Any(c => c.RelatorioRespostaId == s.Id));
            
            foreach (var respostaAlterar in repostasAlteradas)
                await mediator.Send(new AlterarRelatorioPeriodicoRespostaPAPCommand(respostaAlterar, respostas.FirstOrDefault(c => c.RelatorioRespostaId == respostaAlterar.Id)));
        }

        private async Task ExcluirRespostasEncaminhamento(RelatorioPeriodicoPAPQuestao questoesExistentes, IEnumerable<RelatorioPAPRespostaDto> respostas)
        {
            var respostasExcluidas = questoesExistentes.Respostas.Where(s => !respostas.Any(c => c.RelatorioRespostaId == s.Id));

            foreach (var respostasExcluir in respostasExcluidas)
                await mediator.Send(new ExcluirRelatorioPeriodicoRespostaPAPCommand(respostasExcluir));
        }

        private async Task IncluirRespostasEncaminhamento(RelatorioPeriodicoPAPQuestao questaoExistente, IEnumerable<RelatorioPAPRespostaDto> respostas)
        {
            var repostasIncluidas = respostas.Where(c => !c.RelatorioRespostaId.HasValue);

            foreach (var questao in repostasIncluidas)
                await mediator.Send(new SalvarRelatorioPeriodicoRespostaPAPCommand(questao.Resposta, questao.TipoQuestao, questaoExistente.Id));

        }

        private ResultadoRelatorioPAPSecaoDto ObterResultadoSecao(RelatorioPeriodicoPAPSecao relatorioSecao, long secaoId)
        {
            return new ResultadoRelatorioPAPSecaoDto()
            {
                SecaoId = relatorioSecao.Id,
                Auditoria = new AuditoriaDto()
                {
                    Id = secaoId,
                    CriadoEm = relatorioSecao.CriadoEm,
                    CriadoPor = relatorioSecao.CriadoPor,
                    CriadoRF = relatorioSecao.CriadoRF,
                    AlteradoEm = relatorioSecao.AlteradoEm,
                    AlteradoPor = relatorioSecao.AlteradoPor,
                    AlteradoRF = relatorioSecao.AlteradoRF
                }
            }; 
        }

        private async Task SalvarResposta(IEnumerable<RelatorioPAPRespostaDto> respostas, long relatorioQuestaoId)
        {
            foreach (var resposta in respostas)
            {
                await mediator.Send(new SalvarRelatorioPeriodicoRespostaPAPCommand(resposta.Resposta, resposta.TipoQuestao, relatorioQuestaoId));
            }
        }

        private async Task<RelatorioPeriodicoPAPAluno> PersistirRelatorioAluno(RelatorioPAPDto relatorioPAPDto, long relatorioTurmaId)
        {
            if (relatorioPAPDto.PAPAlunoId.HasValue)
                return await mediator.Send(new ObterRelatorioPeriodicoAlunoPAPQuery(relatorioPAPDto.PAPAlunoId.Value));

            return await mediator.Send(new SalvarRelatorioPeriodicoAlunoPAPCommand(relatorioPAPDto.AlunoCodigo, 
                                                                                   relatorioPAPDto.AlunoNome,
                                                                                   relatorioTurmaId));
        }

        private async Task ValidarQuestoesObrigatorias(RelatorioPAPDto relatorioPAPDto, string turmaCodigo)
        {
            var questoesObrigatoriasAConsistir = await ObterQuestoesObrigatoriasNaoPreechidas(relatorioPAPDto, turmaCodigo);
            if (questoesObrigatoriasAConsistir.Any())
            {
                var mensagem = questoesObrigatoriasAConsistir.GroupBy(questao => questao.SecaoNome).Select(secao =>
                        $"Seção: {secao.Key} Questões: [{string.Join(", ", secao.Select(questao => questao.QuestaoOrdem).Distinct().ToArray())}]")
                    .ToList();

                throw new NegocioException(string.Format(
                    MensagemNegocioRelatorioPAP.EXISTEM_QUESTOES_OBRIGATORIAS_NAO_PREENCHIDAS,
                    string.Join(", ", mensagem)));
            }
        }

        private async Task<List<QuestaoObrigatoriaNaoRespondidaDto>> ObterQuestoesObrigatoriasNaoPreechidas(RelatorioPAPDto relatorioPAPDto, string turmaCodigo)

        {
            List<QuestaoObrigatoriaNaoRespondidaDto> questoesObrigatoriasAConsistir = new List<QuestaoObrigatoriaNaoRespondidaDto>();
            var secoesEtapa = await mediator.Send(new ObterSecoesPAPQuery(turmaCodigo, relatorioPAPDto.AlunoCodigo, relatorioPAPDto.periodoRelatorioPAPId));
            IEnumerable<RespostaQuestaoObrigatoriaDto> respostasPersistidas = null;

            foreach (var secao in secoesEtapa.Secoes)
            {
                var secaoPresenteDto = relatorioPAPDto.Secoes.FirstOrDefault(secaoDto => secaoDto.SecaoId == secao.Id);

                IEnumerable<RespostaQuestaoObrigatoriaDto> respostasSecao;
                if (secaoPresenteDto != null && secaoPresenteDto.Respostas.Any())
                {
                    respostasSecao = secaoPresenteDto.Respostas
                        .Select(questao => new RespostaQuestaoObrigatoriaDto()
                        {
                            QuestaoId = questao.QuestaoId,
                            Resposta = questao.Resposta
                        })
                        .Where(questao => !string.IsNullOrEmpty(questao.Resposta));
                }
                else
                {
                    if (respostasPersistidas == null)
                        respostasPersistidas = await ObterRespostasPersistidas(secao.PAPSecaoId);
                    respostasSecao = respostasPersistidas;
                }

                var secaoQuestionario = new SecaoQuestionarioDto()
                {
                    Id = secao.Id,
                    Nome = secao.Nome,
                    QuestionarioId = secao.QuestionarioId,
                    Auditoria = secao.Auditoria
                };

                questoesObrigatoriasAConsistir.AddRange(await mediator.Send(new ObterQuestoesObrigatoriasNaoRespondidasQuery(secaoQuestionario, respostasSecao)));
            }

            return questoesObrigatoriasAConsistir;
        }

        private async Task<IEnumerable<RespostaQuestaoObrigatoriaDto>> ObterRespostasPersistidas(long? PAPSecaoId)
        {
            return PAPSecaoId.HasValue ? (await mediator.Send( new ObterRespostaPorSecaoPAPQuery(PAPSecaoId.Value)))
                .Select(resposta => new RespostaQuestaoObrigatoriaDto
                {
                    QuestaoId = resposta.RelatorioPeriodicoQuestao.QuestaoId,
                    Resposta = (resposta.RespostaId ?? 0) != 0 ? resposta.RespostaId?.ToString() : resposta.Texto,
                    Persistida = true
                })
                : Enumerable.Empty<RespostaQuestaoObrigatoriaDto>();
        }
    }
}
