using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.EncaminhamentoNAAPA
{
    public class RegistrarEncaminhamentoItinerarioNAAPAUseCase : IRegistrarEncaminhamentoItinerarioNAAPAUseCase
    {
        private readonly IMediator mediator;

        public RegistrarEncaminhamentoItinerarioNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(EncaminhamentoNAAPAItineranciaDto encaminhamentoNAAPAItineranciaDto)
        {
            var encaminhamentoNAAPA = await mediator.Send(new ObterEncaminhamentoNAAPAPorIdQuery(encaminhamentoNAAPAItineranciaDto.EncaminhamentoId));

            await Validar(encaminhamentoNAAPA, encaminhamentoNAAPAItineranciaDto.EncaminhamentoNAAPASecao);

            if (encaminhamentoNAAPAItineranciaDto.EncaminhamentoNAAPASecaoId.HasValue)
            {
                return await Alterar(encaminhamentoNAAPA, encaminhamentoNAAPAItineranciaDto);
            }

            return await Salvar(encaminhamentoNAAPA, encaminhamentoNAAPAItineranciaDto);
        }

        private async Task<bool> Alterar(Dominio.EncaminhamentoNAAPA encaminhamentoNAAPA, EncaminhamentoNAAPAItineranciaDto encaminhamentoNAAPAItineranciaDto)
        {
            var secaoDto = encaminhamentoNAAPAItineranciaDto.EncaminhamentoNAAPASecao;
            var secaoExistente = encaminhamentoNAAPA.Secoes.FirstOrDefault(secao => secao.SecaoEncaminhamentoNAAPAId == encaminhamentoNAAPAItineranciaDto.EncaminhamentoNAAPASecaoId);

            ValidarAlteracao(secaoExistente, secaoDto);

            return await mediator.Send(new AlterarEncaminhamentoNAAPASecaoQuestaoCommand(secaoDto, secaoExistente));
        }

        private async Task<bool> Salvar(Dominio.EncaminhamentoNAAPA encaminhamentoNAAPA, EncaminhamentoNAAPAItineranciaDto encaminhamentoNAAPAItineranciaDto)
        {
            var secaoDto = encaminhamentoNAAPAItineranciaDto.EncaminhamentoNAAPASecao;

            ValidarQuestao(secaoDto);

            await AlterarSituacaoDoAtendimento(encaminhamentoNAAPA);

            var secaoEncaminhamento = await mediator.Send(new RegistrarEncaminhamentoNAAPASecaoCommand(encaminhamentoNAAPAItineranciaDto.EncaminhamentoId, secaoDto.SecaoId, true));

            foreach (var questao in secaoDto.Questoes)
            {
                var secaoQuestaoId = await mediator.Send(new RegistrarEncaminhamentoNAAPASecaoQuestaoCommand(secaoEncaminhamento.Id, questao.QuestaoId));
                
                await mediator.Send(new RegistrarEncaminhamentoNAAPASecaoQuestaoRespostaCommand(questao.Resposta, secaoQuestaoId, questao.TipoQuestao));
            }

            return true;
        }

        private async Task AlterarSituacaoDoAtendimento(Dominio.EncaminhamentoNAAPA encaminhamentoNAAPA)
        {
            encaminhamentoNAAPA.Situacao = SituacaoNAAPA.EmAtendimento;

            await mediator.Send(new SalvarEncaminhamentoNAAPACommand(encaminhamentoNAAPA));
        }

        private void ValidarAlteracao(EncaminhamentoNAAPASecao secaoExistente, EncaminhamentoNAAPASecaoDto secaoDto)
        {
            if (secaoExistente == null)
                throw new NegocioException(MensagemNegocioEncaminhamentoNAAPA.SECAO_NAO_ENCONTRADA);

            ValidarQuestao(secaoDto);
        }

        private void ValidarQuestao(EncaminhamentoNAAPASecaoDto secao)
        {
            if (!secao.Questoes.Any())
                throw new NegocioException(string.Format(MensagemNegocioComuns.NENHUMA_QUESTAO_FOI_ENCONTRADA_NA_SECAO_X, secao.SecaoId));
        }

        private async Task Validar(Dominio.EncaminhamentoNAAPA encaminhamentoNAAPA, EncaminhamentoNAAPASecaoDto encaminhamentoNAAPASecaoDto)
        {
            ValidarEncaminhamento(encaminhamentoNAAPA);

            await ValidarCamposObrigatorios(encaminhamentoNAAPASecaoDto);
        }

        private void ValidarEncaminhamento(Dominio.EncaminhamentoNAAPA encaminhamentoNAAPA)
        {
            if (encaminhamentoNAAPA == null)
                throw new NegocioException(MensagemNegocioEncaminhamentoNAAPA.ENCAMINHAMENTO_NAO_ENCONTRADO);

            if (encaminhamentoNAAPA.Situacao == SituacaoNAAPA.Rascunho)
                throw new NegocioException(MensagemNegocioEncaminhamentoNAAPA.SITUACAO_ENCAMINHAMENTO_DEVE_SER_DIFERENTE_RASCUNHO);
        }

        private async Task ValidarCamposObrigatorios(EncaminhamentoNAAPASecaoDto encaminhamentoNAAPASecaoDto)
        {
            var secao = await mediator.Send(new ObterSecaoQuestionarioEncaminhamentoNAAPAPorIdQuery(encaminhamentoNAAPASecaoDto.SecaoId));
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
