using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistrarMapeamentoEstudanteUseCase : IRegistrarMapeamentoEstudanteUseCase
    {
        private readonly IMediator mediator;

        public RegistrarMapeamentoEstudanteUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ResultadoMapeamentoEstudanteDto> Executar(MapeamentoEstudanteDto mapeamentoDto)
        {
            var dadosAluno = await ValidarRegras(mapeamentoDto);           
            List<QuestaoObrigatoriaNaoRespondidaDto> questoesObrigatoriasAConsistir = await ObterQuestoesObrigatoriasNaoPreechidas(mapeamentoDto);
            ConsistirQuestoesObrigatoriasNaoPreenchidas(questoesObrigatoriasAConsistir);
            PreencherConclusaoSecoes(questoesObrigatoriasAConsistir, mapeamentoDto);

            if (mapeamentoDto.Id.GetValueOrDefault() > 0)
            {
                var mapeamentoEstudante = await mediator.Send(new ObterMapeamentoEstudantePorIdQuery(mapeamentoDto.Id.GetValueOrDefault()));
                if (mapeamentoEstudante.NaoEhNulo())
                {
                    await AlterarMapeamentoEstudante(mapeamentoDto, mapeamentoEstudante);
                    await RemoverArquivosNaoUtilizados(mapeamentoDto.Secoes);
                    return new ResultadoMapeamentoEstudanteDto
                        { Id = mapeamentoEstudante.Id, Auditoria = (AuditoriaDto)mapeamentoEstudante };
                }
            }

            var resultadoMapeamento = await mediator.Send(new RegistrarMapeamentoEstudanteCommand(mapeamentoDto.TurmaId, dadosAluno.NomeAluno, dadosAluno.CodigoAluno, mapeamentoDto.Bimestre));
            await SalvarMapeamentoEstudante(mapeamentoDto, resultadoMapeamento);
            return resultadoMapeamento;
        }
      
        private async Task<(string CodigoAluno, string NomeAluno)> ValidarRegras(MapeamentoEstudanteDto mapeamento)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(mapeamento.TurmaId));
            if (turma.EhNulo())
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(mapeamento.AlunoCodigo, DateTime.Now.Year));
            if (aluno.EhNulo())
                throw new NegocioException(MensagemNegocioAluno.ESTUDANTE_NAO_ENCONTRADO);

            if (!mapeamento.Id.HasValue)
            {
                var idMapeamentoExistente = await mediator.Send(new ObterIdentificadorMapeamentoEstudanteQuery(mapeamento.AlunoCodigo, mapeamento.TurmaId, mapeamento.Bimestre));
                if (idMapeamentoExistente.HasValue)
                    throw new NegocioException(MensagemNegocioMapeamentoEstudante.MAPEAMENTO_ESTUDANTE_JA_EXISTENTE);
            }

            return (aluno.CodigoAluno, aluno.NomeAluno);
        }

        private void PreencherConclusaoSecoes(List<QuestaoObrigatoriaNaoRespondidaDto> questoesObrigatoriasAConsistir, MapeamentoEstudanteDto mapeamentoDto)
        {
            var secoesComQuestoesObrigatoriasAConsistir = questoesObrigatoriasAConsistir
                .Select(questao => questao.SecaoId).Distinct().ToArray();

            foreach (var secao in mapeamentoDto.Secoes)
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
                MensagemNegocioMapeamentoEstudante.EXISTEM_QUESTOES_OBRIGATORIAS_NAO_PREENCHIDAS,
                string.Join(", ", mensagem)));
        }

        private async Task RemoverArquivosNaoUtilizados(List<MapeamentoEstudanteSecaoDto> secoes)
        {
            var resposta = new List<MapeamentoEstudanteSecaoQuestaoDto>();
            foreach (var s in secoes)
                foreach (var q in s.Questoes)
                    if (string.IsNullOrEmpty(q.Resposta) && q.TipoQuestao == TipoQuestao.Upload)
                        resposta.Add(q);
            
            if (resposta.Any())
                foreach (var item in resposta)
                {
                    var entidadeResposta = await mediator.Send(new ObterRespostaMapeamentoEstudantePorIdQuery(item.RespostaMapeamentoEstudanteId));
                    if (entidadeResposta.NaoEhNulo())
                        await mediator.Send(new ExcluirRespostaMapeamentoEstudanteCommand(entidadeResposta));
                }
        }
        public async Task AlterarMapeamentoEstudante(MapeamentoEstudanteDto mapeamentoEstudanteDto, MapeamentoEstudante mapeamentoEstudante)
        {
            foreach (var secao in mapeamentoEstudanteDto.Secoes)
            {
                if (!secao.Questoes.Any())
                    throw new NegocioException(string.Format(MensagemNegocioComuns.NENHUMA_QUESTAO_FOI_ENCONTRADA_NA_SECAO_X,secao.SecaoId));

                var secaoExistente = mapeamentoEstudante.Secoes.FirstOrDefault(s => s.SecaoMapeamentoEstudanteId == secao.SecaoId);
                if (secaoExistente.EhNulo())
                    secaoExistente = await mediator.Send(new RegistrarMapeamentoEstudanteSecaoCommand(mapeamentoEstudante.Id, secao.SecaoId, secao.Concluido));
                else
                {
                    secaoExistente.Concluido = secao.Concluido;
                    await mediator.Send(new AlterarMapeamentoEstudanteSecaoCommand(secaoExistente));
                }

                foreach (var questoes in secao.Questoes.GroupBy(q => q.QuestaoId))
                {
                    var questaoExistente = secaoExistente.Questoes.FirstOrDefault(q => q.QuestaoId == questoes.FirstOrDefault().QuestaoId);

                    if (questaoExistente.EhNulo())
                    {   
                        var resultadoMapeamentoQuestao = await mediator.Send(new RegistrarMapeamentoEstudanteSecaoQuestaoCommand(secaoExistente.Id, questoes.FirstOrDefault().QuestaoId));
                        await RegistrarRespostaMapeamento(questoes, resultadoMapeamentoQuestao);
                    }
                    else
                        await AlterarQuestoesExistentes(questaoExistente, questoes);
                }
                await ExcluirQuestoesExistentes(secaoExistente.Questoes.Where(x => !secao.Questoes.Any(s => s.QuestaoId == x.QuestaoId)));
            }
        }

        private async Task ExcluirQuestoesExistentes(IEnumerable<QuestaoMapeamentoEstudante> questoesRemovidas)
        {
            foreach (var questao in questoesRemovidas)
                await mediator.Send(new ExcluirQuestaoMapeamentoEstudantePorIdCommand(questao.Id));
        }

        private async Task AlterarQuestoesExistentes(QuestaoMapeamentoEstudante questaoExistente, IGrouping<long, MapeamentoEstudanteSecaoQuestaoDto> questoesRespostas)
        {
            if (questaoExistente.Excluido)
                await AlterarQuestaoExcluida(questaoExistente);
            await ExcluirRespostasMapeamentoEstudante(questaoExistente, questoesRespostas);
            await AlterarRespostasMapeamentoEstudante(questaoExistente, questoesRespostas);
            await IncluirRespostasMapeamento(questaoExistente, questoesRespostas);
        }

        private async Task AlterarQuestaoExcluida(QuestaoMapeamentoEstudante questao)
        {
            questao.Excluido = false;
            await mediator.Send(new AlterarQuestaoMapeamentoEstudanteCommand(questao));
        }

        private async Task IncluirRespostasMapeamento(QuestaoMapeamentoEstudante questaoExistente, IGrouping<long, MapeamentoEstudanteSecaoQuestaoDto> respostas)
            => await RegistrarRespostaMapeamento(ObterRespostasAIncluir(respostas), questaoExistente.Id);

        private async Task RegistrarRespostaMapeamento(IEnumerable<MapeamentoEstudanteSecaoQuestaoDto> questoes, long questaoMapeamentoId)
        {
            foreach (var questao in questoes)
                await mediator.Send(new RegistrarMapeamentoEstudanteSecaoQuestaoRespostaCommand(questao.Resposta, questaoMapeamentoId, questao.TipoQuestao));
        }

        private async Task AlterarRespostasMapeamentoEstudante(QuestaoMapeamentoEstudante questaoExistente, IGrouping<long, MapeamentoEstudanteSecaoQuestaoDto> respostas)
        {
            foreach (var respostaAlterar in ObterRespostasAAlterar(questaoExistente, respostas))
                await mediator.Send(new AlterarMapeamentoEstudanteSecaoQuestaoRespostaCommand(respostaAlterar, respostas.FirstOrDefault(c => c.RespostaMapeamentoEstudanteId == respostaAlterar.Id)));
        }

        private async Task ExcluirRespostasMapeamentoEstudante(QuestaoMapeamentoEstudante questoesExistentes, IGrouping<long, MapeamentoEstudanteSecaoQuestaoDto> respostas)
        {
            foreach (var respostasExcluir in ObterRespostasAExcluir(questoesExistentes, respostas))
                await mediator.Send(new ExcluirRespostaMapeamentoEstudanteCommand(respostasExcluir));
        }

        public async Task SalvarMapeamentoEstudante(MapeamentoEstudanteDto mapeamentoEstudanteDto, ResultadoMapeamentoEstudanteDto resultadoMapeamentoEstudante)
        {
            foreach (var secao in mapeamentoEstudanteDto.Secoes)
            {
                if (!secao.Questoes.Any())
                    throw new NegocioException(string.Format(MensagemNegocioComuns.NENHUMA_QUESTAO_FOI_ENCONTRADA_NA_SECAO_X,secao.SecaoId));

                var secaoMapeamentoEstudante = await mediator.Send(new RegistrarMapeamentoEstudanteSecaoCommand(resultadoMapeamentoEstudante.Id, secao.SecaoId, secao.Concluido));
                foreach (var questoes in secao.Questoes.GroupBy(q => q.QuestaoId))
                {
                    var resultadoMapeamentoEstudanteQuestao = await mediator.Send(new RegistrarMapeamentoEstudanteSecaoQuestaoCommand(secaoMapeamentoEstudante.Id, questoes.FirstOrDefault().QuestaoId));
                    await RegistrarRespostaMapeamento(questoes, resultadoMapeamentoEstudanteQuestao);
                }
            }
        }


        private async Task<IEnumerable<RespostaQuestaoObrigatoriaDto>> ObterRespostasMapeamentoEstudante(long? mapeamentoEstudanteId)
        {
            if (mapeamentoEstudanteId.HasValue)
                return (await mediator.Send(
                        new ObterQuestaoRespostaMapeamentoEstudantePorIdQuery(mapeamentoEstudanteId.Value)))
                    .Select(resposta => new RespostaQuestaoObrigatoriaDto
                    {
                        QuestaoId = resposta.QuestaoId,
                        Resposta = resposta.RespostaId.HasValue ? resposta.RespostaId?.ToString() : resposta.Texto,
                        Persistida = true
                    });

            return Enumerable.Empty<RespostaQuestaoObrigatoriaDto>();
        }
        
        private async Task<List<QuestaoObrigatoriaNaoRespondidaDto>> ObterQuestoesObrigatoriasNaoPreechidas(MapeamentoEstudanteDto mapeamentoDto)
        {
            List<QuestaoObrigatoriaNaoRespondidaDto> questoesObrigatoriasAConsistir = new List<QuestaoObrigatoriaNaoRespondidaDto>();
            var secoesEtapa = await mediator.Send(new ObterSecoesQuestionarioMapeamentoEstudanteDtoQuery());
            IEnumerable<RespostaQuestaoObrigatoriaDto> respostasPersistidas = null;

            foreach (var secao in secoesEtapa)
            {
                var secaoPresenteDto = mapeamentoDto.Secoes.FirstOrDefault(secaoDto => secaoDto.SecaoId == secao.Id);
                IEnumerable<RespostaQuestaoObrigatoriaDto> respostasMapeamentoEstudante;
                if (secaoPresenteDto.NaoEhNulo() && secaoPresenteDto.Questoes.Any())
                {
                    respostasMapeamentoEstudante = secaoPresenteDto.Questoes
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
                        respostasPersistidas = await ObterRespostasMapeamentoEstudante(mapeamentoDto.Id);
                    respostasMapeamentoEstudante = respostasPersistidas;
                }
                questoesObrigatoriasAConsistir.AddRange(await mediator.Send(new ObterQuestoesObrigatoriasNaoRespondidasQuery(secao, respostasMapeamentoEstudante)));
            }
            return questoesObrigatoriasAConsistir;
        }


        private IEnumerable<MapeamentoEstudanteSecaoQuestaoDto> ObterRespostasAIncluir(IGrouping<long, MapeamentoEstudanteSecaoQuestaoDto> respostas)
            => respostas.Where(c => c.RespostaMapeamentoEstudanteId == 0);

        private IEnumerable<RespostaMapeamentoEstudante> ObterRespostasAExcluir(QuestaoMapeamentoEstudante questaoExistente, IGrouping<long, MapeamentoEstudanteSecaoQuestaoDto> respostas) 
            => questaoExistente.Respostas.Where(s => !respostas.Any(c => c.RespostaMapeamentoEstudanteId == s.Id));
            
        private IEnumerable<RespostaMapeamentoEstudante> ObterRespostasAAlterar(QuestaoMapeamentoEstudante questaoExistente, IGrouping<long, MapeamentoEstudanteSecaoQuestaoDto> respostas)
            => questaoExistente.Respostas.Where(s => respostas.Any(c => c.RespostaMapeamentoEstudanteId == s.Id));

    }
}


