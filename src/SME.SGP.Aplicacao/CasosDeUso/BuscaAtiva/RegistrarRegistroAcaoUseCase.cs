using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistrarRegistroAcaoUseCase : IRegistrarRegistroAcaoUseCase
    {
        private readonly IMediator mediator;

        public RegistrarRegistroAcaoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ResultadoRegistroAcaoBuscaAtivaDto> Executar(RegistroAcaoBuscaAtivaDto registroAcaoDto)
        {
            var dadosAluno = await ValidarRegras(registroAcaoDto);           
            List<QuestaoObrigatoriaNaoRespondidaDto> questoesObrigatoriasAConsistir = await ObterQuestoesObrigatoriasNaoPreechidas(registroAcaoDto);
            ConsistirQuestoesObrigatoriasNaoPreenchidas(questoesObrigatoriasAConsistir);
            PreencherConclusaoSecoes(questoesObrigatoriasAConsistir, registroAcaoDto);
            
            if (registroAcaoDto.Id.GetValueOrDefault() > 0)
            {
                var registroAcao = await mediator.Send(new ObterRegistroAcaoPorIdQuery(registroAcaoDto.Id.GetValueOrDefault()));
                if (registroAcao.NaoEhNulo())
                {
                    await AlterarRegistroAcao(registroAcaoDto, registroAcao);
                    await RemoverArquivosNaoUtilizados(registroAcaoDto.Secoes);
                    return new ResultadoRegistroAcaoBuscaAtivaDto
                        { Id = registroAcao.Id, Auditoria = (AuditoriaDto)registroAcao };
                }
            }

            var resultadoRegistroAcao = await mediator.Send(new RegistrarRegistroAcaoCommand(registroAcaoDto.TurmaId, dadosAluno.NomeAluno, dadosAluno.CodigoAluno));
            await SalvarRegistroAcao(registroAcaoDto, resultadoRegistroAcao);
            return resultadoRegistroAcao;
        }
      
        private async Task<(string CodigoAluno, string NomeAluno)> ValidarRegras(RegistroAcaoBuscaAtivaDto registroAcaoDto)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(registroAcaoDto.TurmaId));
            if (turma.EhNulo())
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(registroAcaoDto.AlunoCodigo, DateTime.Now.Year));
            if (aluno.EhNulo())
                throw new NegocioException(MensagemNegocioAluno.ESTUDANTE_NAO_ENCONTRADO);

            return (aluno.CodigoAluno, aluno.NomeAluno);
        }

        private void PreencherConclusaoSecoes(List<QuestaoObrigatoriaNaoRespondidaDto> questoesObrigatoriasAConsistir, RegistroAcaoBuscaAtivaDto registroAcaoDto)
        {
            var secoesComQuestoesObrigatoriasAConsistir = questoesObrigatoriasAConsistir
                .Select(questao => questao.SecaoId).Distinct().ToArray();

            foreach (var secao in registroAcaoDto.Secoes)
                secao.Concluido = !secoesComQuestoesObrigatoriasAConsistir.Contains(secao.SecaoId);
        }

        private void ConsistirQuestoesObrigatoriasNaoPreenchidas(List<QuestaoObrigatoriaNaoRespondidaDto> questoesObrigatoriasAConsistir)
        {
            if (!questoesObrigatoriasAConsistir.Any())
                return;

            var mensagem = questoesObrigatoriasAConsistir.GroupBy(questao => questao.SecaoNome).Select(secao =>
                $"Seção: {secao.Key} Questões: [{string.Join(", ", secao.Select(questao => questao.QuestaoOrdem).Distinct().ToArray())}]")
            .ToList();

            throw new NegocioException(string.Format(
                MensagemNegocioRegistroAcao.EXISTEM_QUESTOES_OBRIGATORIAS_NAO_PREENCHIDAS,
                string.Join(", ", mensagem)));
        }

        private async Task RemoverArquivosNaoUtilizados(List<RegistroAcaoBuscaAtivaSecaoDto> secoes)
        {
            var resposta = new List<RegistroAcaoBuscaAtivaSecaoQuestaoDto>();
            foreach (var s in secoes)
                foreach (var q in s.Questoes)
                    if (string.IsNullOrEmpty(q.Resposta) && q.TipoQuestao == TipoQuestao.Upload)
                        resposta.Add(q);
            
            if (resposta.Any())
                foreach (var item in resposta)
                {
                    var entidadeResposta = await mediator.Send(new ObterRespostaRegistroAcaoPorIdQuery(item.RespostaRegistroAcaoId));
                    if (entidadeResposta.NaoEhNulo())
                        await mediator.Send(new ExcluirRespostaRegistroAcaoCommand(entidadeResposta));
                }
        }
        public async Task AlterarRegistroAcao(RegistroAcaoBuscaAtivaDto registroAcaoDto, RegistroAcaoBuscaAtiva registroAcao)
        {
            foreach (var secao in registroAcaoDto.Secoes)
            {
                if (!secao.Questoes.Any())
                    throw new NegocioException(string.Format(MensagemNegocioComuns.NENHUMA_QUESTAO_FOI_ENCONTRADA_NA_SECAO_X,secao.SecaoId));

                var secaoExistente = registroAcao.Secoes.FirstOrDefault(s => s.SecaoRegistroAcaoBuscaAtivaId == secao.SecaoId);
                if (secaoExistente.EhNulo())
                    secaoExistente = await mediator.Send(new RegistrarRegistroAcaoSecaoCommand(registroAcao.Id, secao.SecaoId, secao.Concluido));
                else
                {
                    secaoExistente.Concluido = secao.Concluido;
                    await mediator.Send(new AlterarRegistroAcaoSecaoCommand(secaoExistente));
                }

                foreach (var questoes in secao.Questoes.GroupBy(q => q.QuestaoId))
                {
                    var questaoExistente = secaoExistente.Questoes.FirstOrDefault(q => q.QuestaoId == questoes.FirstOrDefault().QuestaoId);

                    if (questaoExistente.EhNulo())
                    {   
                        var resultadoRegistroAcaoQuestao = await mediator.Send(new RegistrarRegistroAcaoSecaoQuestaoCommand(secaoExistente.Id, questoes.FirstOrDefault().QuestaoId));
                        await RegistrarRespostaRegistroAcao(questoes, resultadoRegistroAcaoQuestao);
                    }
                    else
                        await AlterarQuestoesExistentes(questaoExistente, questoes);
                }
                await ExcluirQuestoesExistentes(secaoExistente.Questoes.Where(x => !secao.Questoes.Any(s => s.QuestaoId == x.QuestaoId)));
            }
        }

        private async Task ExcluirQuestoesExistentes(IEnumerable<QuestaoRegistroAcaoBuscaAtiva> questoesRemovidas)
        {
            foreach (var questao in questoesRemovidas)
                await mediator.Send(new ExcluirQuestaoRegistroAcaoPorIdCommand(questao.Id));
        }

        private async Task AlterarQuestoesExistentes(QuestaoRegistroAcaoBuscaAtiva questaoExistente, IGrouping<long, RegistroAcaoBuscaAtivaSecaoQuestaoDto> questoesRespostas)
        {
            if (questaoExistente.Excluido)
                await AlterarQuestaoExcluida(questaoExistente);
            await ExcluirRespostasRegistroAcao(questaoExistente, questoesRespostas);
            await AlterarRespostasRegistroAcao(questaoExistente, questoesRespostas);
            await IncluirRespostasRegistroAcao(questaoExistente, questoesRespostas);
        }

        private async Task AlterarQuestaoExcluida(QuestaoRegistroAcaoBuscaAtiva questao)
        {
            questao.Excluido = false;
            await mediator.Send(new AlterarQuestaoRegistroAcaoCommand(questao));
        }

        private async Task IncluirRespostasRegistroAcao(QuestaoRegistroAcaoBuscaAtiva questaoExistente, IGrouping<long, RegistroAcaoBuscaAtivaSecaoQuestaoDto> respostas)
            => await RegistrarRespostaRegistroAcao(ObterRespostasAIncluir(respostas), questaoExistente.Id);

        private async Task RegistrarRespostaRegistroAcao(IEnumerable<RegistroAcaoBuscaAtivaSecaoQuestaoDto> questoes, long questaoRegistroAcaoId)
        {
            foreach (var questao in questoes)
                await mediator.Send(new RegistrarRegistroAcaoSecaoQuestaoRespostaCommand(questao.Resposta, questaoRegistroAcaoId, questao.TipoQuestao));
        }

        private async Task AlterarRespostasRegistroAcao(QuestaoRegistroAcaoBuscaAtiva questaoExistente, IGrouping<long, RegistroAcaoBuscaAtivaSecaoQuestaoDto> respostas)
        {
            foreach (var respostaAlterar in ObterRespostasAAlterar(questaoExistente, respostas))
                await mediator.Send(new AlterarRegistroAcaoSecaoQuestaoRespostaCommand(respostaAlterar, respostas.FirstOrDefault(c => c.RespostaRegistroAcaoId == respostaAlterar.Id)));
        }

        private async Task ExcluirRespostasRegistroAcao(QuestaoRegistroAcaoBuscaAtiva questoesExistentes, IGrouping<long, RegistroAcaoBuscaAtivaSecaoQuestaoDto> respostas)
        {
            foreach (var respostasExcluir in ObterRespostasAExcluir(questoesExistentes, respostas))
                await mediator.Send(new ExcluirRespostaRegistroAcaoCommand(respostasExcluir));
        }

        public async Task SalvarRegistroAcao(RegistroAcaoBuscaAtivaDto registroAcaoDto, ResultadoRegistroAcaoBuscaAtivaDto resultadoRegistroAcao)
        {
            foreach (var secao in registroAcaoDto.Secoes)
            {
                if (!secao.Questoes.Any())
                    throw new NegocioException(string.Format(MensagemNegocioComuns.NENHUMA_QUESTAO_FOI_ENCONTRADA_NA_SECAO_X,secao.SecaoId));

                var secaoRegistroAcao = await mediator.Send(new RegistrarRegistroAcaoSecaoCommand(resultadoRegistroAcao.Id, secao.SecaoId, secao.Concluido));
                foreach (var questoes in secao.Questoes.GroupBy(q => q.QuestaoId))
                {
                    var resultadoResultadoAcaoQuestao = await mediator.Send(new RegistrarRegistroAcaoSecaoQuestaoCommand(secaoRegistroAcao.Id, questoes.FirstOrDefault().QuestaoId));
                    await RegistrarRespostaRegistroAcao(questoes, resultadoResultadoAcaoQuestao);
                }
            }
        }


        private async Task<IEnumerable<RespostaQuestaoObrigatoriaDto>> ObterRespostasRegistroAcao(long? registroAcaoId)
        {
            if (registroAcaoId.HasValue)
                return (await mediator.Send(
                        new ObterQuestaoRespostaRegistroAcaoPorIdQuery(registroAcaoId.Value)))
                    .Select(resposta => new RespostaQuestaoObrigatoriaDto
                    {
                        QuestaoId = resposta.QuestaoId,
                        Resposta = resposta.RespostaId.HasValue ? resposta.RespostaId?.ToString() : resposta.Texto,
                        Persistida = true
                    });

            return Enumerable.Empty<RespostaQuestaoObrigatoriaDto>();
        }

        private async Task<List<QuestaoObrigatoriaNaoRespondidaDto>> ObterQuestoesObrigatoriasNaoPreechidas(RegistroAcaoBuscaAtivaDto registroAcaoDto)
        {
            List<QuestaoObrigatoriaNaoRespondidaDto> questoesObrigatoriasAConsistir = new List<QuestaoObrigatoriaNaoRespondidaDto>();
            var secoesEtapa = await mediator.Send(new ObterSecoesQuestionarioRegistroAcaoDtoQuery());
            IEnumerable<RespostaQuestaoObrigatoriaDto> respostasPersistidas = null;

            foreach (var secao in secoesEtapa)
            {
                var secaoPresenteDto = registroAcaoDto.Secoes.FirstOrDefault(secaoDto => secaoDto.SecaoId == secao.Id);
                IEnumerable<RespostaQuestaoObrigatoriaDto> respostasRegistroAcao;
                if (secaoPresenteDto.NaoEhNulo() && secaoPresenteDto.Questoes.Any())
                {
                    respostasRegistroAcao = secaoPresenteDto.Questoes
                        .Select(questao => new RespostaQuestaoObrigatoriaDto()
                        {
                            QuestaoId = questao.QuestaoId,
                            Resposta = questao.Resposta
                        })
                        .Where(questao => !string.IsNullOrEmpty(questao.Resposta));
                }
                else
                {
                    if (respostasPersistidas.EhNulo())
                        respostasPersistidas = await ObterRespostasRegistroAcao(registroAcaoDto.Id);
                    respostasRegistroAcao = respostasPersistidas;
                }
                questoesObrigatoriasAConsistir.AddRange(await mediator.Send(new ObterQuestoesObrigatoriasNaoRespondidasQuery(secao, respostasRegistroAcao)));
            }
            return questoesObrigatoriasAConsistir;
        }


        private IEnumerable<RegistroAcaoBuscaAtivaSecaoQuestaoDto> ObterRespostasAIncluir(IGrouping<long, RegistroAcaoBuscaAtivaSecaoQuestaoDto> respostas)
            => respostas.Where(c => c.RespostaRegistroAcaoId == 0);

        private IEnumerable<RespostaRegistroAcaoBuscaAtiva> ObterRespostasAExcluir(QuestaoRegistroAcaoBuscaAtiva questaoExistente, IGrouping<long, RegistroAcaoBuscaAtivaSecaoQuestaoDto> respostas) 
            => questaoExistente.Respostas.Where(s => !respostas.Any(c => c.RespostaRegistroAcaoId == s.Id));
            
        private IEnumerable<RespostaRegistroAcaoBuscaAtiva> ObterRespostasAAlterar(QuestaoRegistroAcaoBuscaAtiva questaoExistente, IGrouping<long, RegistroAcaoBuscaAtivaSecaoQuestaoDto> respostas)
            => questaoExistente.Respostas.Where(s => respostas.Any(c => c.RespostaRegistroAcaoId == s.Id));

    }
}


