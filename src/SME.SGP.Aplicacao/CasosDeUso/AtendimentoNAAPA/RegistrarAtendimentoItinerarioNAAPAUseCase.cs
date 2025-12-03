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

namespace SME.SGP.Aplicacao.CasosDeUso.EncaminhamentoNAAPA
{
    public class RegistrarAtendimentoItinerarioNAAPAUseCase : IRegistrarAtendimentoItinerarioNAAPAUseCase
    {
        private readonly IMediator mediator;

        public RegistrarAtendimentoItinerarioNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(AtendimentoNAAPAItineranciaDto encaminhamentoNAAPAItineranciaDto)
        {
            Dominio.EncaminhamentoNAAPA encaminhamentoNaapa;

            if (!encaminhamentoNAAPAItineranciaDto.EncaminhamentoNAAPASecaoId.HasValue)
            {
                encaminhamentoNaapa =
                    await mediator.Send(
                        new ObterEncaminhamentoNAAPAPorIdQuery(encaminhamentoNAAPAItineranciaDto.EncaminhamentoId));
            }
            else
            {
                encaminhamentoNaapa = await mediator.Send(new ObterEncaminhamentoNAAPAPorIdESecaoQuery(
                    encaminhamentoNAAPAItineranciaDto.EncaminhamentoId,
                    encaminhamentoNAAPAItineranciaDto.EncaminhamentoNAAPASecaoId.GetValueOrDefault()));                
            }
            
            await Validar(encaminhamentoNaapa, encaminhamentoNAAPAItineranciaDto.EncaminhamentoNAAPASecao);

            if (encaminhamentoNAAPAItineranciaDto.EncaminhamentoNAAPASecaoId.HasValue)
                return await Alterar(encaminhamentoNaapa, encaminhamentoNAAPAItineranciaDto);

            return await Salvar(encaminhamentoNaapa, encaminhamentoNAAPAItineranciaDto);
        }

        private async Task<bool> Alterar(Dominio.EncaminhamentoNAAPA encaminhamentoNAAPA, AtendimentoNAAPAItineranciaDto encaminhamentoNAAPAItineranciaDto)
        {
            var secaoDto = encaminhamentoNAAPAItineranciaDto.EncaminhamentoNAAPASecao;
            var secaoExistente = encaminhamentoNAAPA.Secoes.FirstOrDefault(secao => secao.Id == encaminhamentoNAAPAItineranciaDto.EncaminhamentoNAAPASecaoId);

            ValidarAlteracao(secaoExistente, secaoDto);

            await mediator.Send(new AlterarAtendimentoNAAPASecaoCommand(secaoExistente));

            await mediator.Send(new RegistrarHistoricoDeAlteracaoAtendimentoNAAPACommand(secaoDto, secaoExistente, TipoHistoricoAlteracoesEncaminhamentoNAAPA.Alteracao));

            await mediator.Send(new AlterarAtendimentoNAAPASecaoQuestaoCommand(secaoDto, secaoExistente));
            await RemoverArquivosNaoUtilizados(secaoDto);
            return true;
        }

        private async Task RemoverArquivosNaoUtilizados(AtendimentoNAAPASecaoDto secao)
        {
            var resposta = new List<AtendimentoNAAPASecaoQuestaoDto>();

            foreach (var q in secao.Questoes)
            {
                if (string.IsNullOrEmpty(q.Resposta) && q.TipoQuestao == TipoQuestao.Upload)
                    resposta.Add(q);
            }
            
            if (resposta.Any())
            {
                foreach (var item in resposta)
                {
                    var entidadeResposta = await mediator.Send(new ObterRespostaEncaminhamentoNAAPAPorIdQuery(item.RespostaEncaminhamentoId));
                    if (entidadeResposta.NaoEhNulo())
                        await mediator.Send(new ExcluirRespostaAtendimentoNAAPACommand(entidadeResposta));
                }
            }
        }

        private async Task<bool> Salvar(Dominio.EncaminhamentoNAAPA encaminhamentoNAAPA, AtendimentoNAAPAItineranciaDto encaminhamentoNAAPAItineranciaDto)
        {
            var secaoDto = encaminhamentoNAAPAItineranciaDto.EncaminhamentoNAAPASecao;

            ValidarQuestao(secaoDto);

            await AlterarSituacaoDoAtendimento(encaminhamentoNAAPA);

            var secaoEncaminhamento = await mediator.Send(new RegistrarAtendimentoNAAPASecaoCommand(encaminhamentoNAAPAItineranciaDto.EncaminhamentoId, secaoDto.SecaoId, true));

            foreach (var questoes in secaoDto.Questoes.GroupBy(q => q.QuestaoId))
            {
                var secaoQuestaoId = await mediator.Send(new RegistrarAtendimentoNAAPASecaoQuestaoCommand(secaoEncaminhamento.Id, questoes.FirstOrDefault().QuestaoId));
                await RegistrarRespostaEncaminhamento(questoes, secaoQuestaoId);
            }

            await mediator.Send(new RegistrarHistoricoDeAlteracaoAtendimentoNAAPACommand(secaoDto, secaoEncaminhamento, TipoHistoricoAlteracoesEncaminhamentoNAAPA.Inserido));
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.RotaExcluirNotificacaoInatividadeAtendimento, encaminhamentoNAAPAItineranciaDto.EncaminhamentoId, Guid.NewGuid()));
            return true;
        }

        private async Task RegistrarRespostaEncaminhamento(IEnumerable<AtendimentoNAAPASecaoQuestaoDto> questoes, long questaoEncaminhamentoId)
        {
            foreach (var questao in questoes)
                await mediator.Send(new RegistrarAtendimentoNAAPASecaoQuestaoRespostaCommand(questao.Resposta, questaoEncaminhamentoId, questao.TipoQuestao));
        }

        private async Task AlterarSituacaoDoAtendimento(Dominio.EncaminhamentoNAAPA encaminhamentoNAAPA)
        {
            await mediator.Send(new AlterarSituacaoNAAPACommand(encaminhamentoNAAPA, SituacaoNAAPA.EmAtendimento));
        }

        private void ValidarAlteracao(EncaminhamentoNAAPASecao secaoExistente, AtendimentoNAAPASecaoDto secaoDto)
        {
            if (secaoExistente.EhNulo())
                throw new NegocioException(MensagemNegocioEncaminhamentoNAAPA.SECAO_NAO_ENCONTRADA);

            ValidarQuestao(secaoDto);
        }

        private void ValidarQuestao(AtendimentoNAAPASecaoDto secao)
        {
            if (!secao.Questoes.Any())
                throw new NegocioException(string.Format(MensagemNegocioComuns.NENHUMA_QUESTAO_FOI_ENCONTRADA_NA_SECAO_X, secao.SecaoId));
        }

        private async Task Validar(Dominio.EncaminhamentoNAAPA encaminhamentoNAAPA, AtendimentoNAAPASecaoDto encaminhamentoNAAPASecaoDto)
        {
            ValidarEncaminhamento(encaminhamentoNAAPA);
            await ValidarCamposObrigatorios(encaminhamentoNAAPASecaoDto);
        }

        private void ValidarEncaminhamento(Dominio.EncaminhamentoNAAPA encaminhamentoNAAPA)
        {
            if (encaminhamentoNAAPA.EhNulo())
                throw new NegocioException(MensagemNegocioEncaminhamentoNAAPA.ENCAMINHAMENTO_NAO_ENCONTRADO);

            if (encaminhamentoNAAPA.Situacao == SituacaoNAAPA.Rascunho)
                throw new NegocioException(MensagemNegocioEncaminhamentoNAAPA.SITUACAO_ENCAMINHAMENTO_DEVE_SER_DIFERENTE_RASCUNHO);
        }

        private void ValidarSecaoItinerancia(SecaoQuestionarioDto secaoQuestionarioDto)
        {
            if (secaoQuestionarioDto.NomeComponente != EncaminhamentoNAAPAConstants.SECAO_ITINERANCIA)
                throw new NegocioException(MensagemNegocioEncaminhamentoNAAPA.SECAO_NAO_VALIDA_ITINERANCIA);
        }

        private async Task ValidarCamposObrigatorios(AtendimentoNAAPASecaoDto encaminhamentoNAAPASecaoDto)
        {
            var secao = await mediator.Send(new ObterSecaoQuestionarioEncaminhamentoNAAPAPorIdQuery(encaminhamentoNAAPASecaoDto.SecaoId));
            ValidarSecaoItinerancia(secao);
            var respostasEncaminhamento = encaminhamentoNAAPASecaoDto.Questoes
                                             .Select(questao => new RespostaQuestaoObrigatoriaDto()
                                             {
                                                 QuestaoId = questao.QuestaoId,
                                                 Resposta = questao.Resposta
                                             })
                                             .Where(questao => !string.IsNullOrEmpty(questao.Resposta));
            var questoesObrigatorias = await mediator.Send(new ObterQuestoesObrigatoriasNaoRespondidasQuery(secao, respostasEncaminhamento));

            if (questoesObrigatorias.Any())
            {
                var mensagem = questoesObrigatorias.GroupBy(questao => questao.SecaoNome).Select(secao =>
                        $"Seção: {secao.Key} Questões: [{string.Join(", ", secao.Select(questao => questao.QuestaoOrdem).Distinct().ToArray())}]")
                    .ToList();

                throw new NegocioException(string.Format(
                    MensagemNegocioEncaminhamentoNAAPA.EXISTEM_QUESTOES_OBRIGATORIAS_NAO_PREENCHIDAS,
                    string.Join(", ", mensagem)));
            }
        }
    }
}
