using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConcluirEncaminhamentoAEEUseCase : AbstractUseCase, IConcluirEncaminhamentoAEEUseCase
    {
        public const string RESPOSTA_NOME_SIM = "Sim";
        public ConcluirEncaminhamentoAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long encaminhamentoId)
        {
            var encaminhamento = await mediator.Send(new ObterEncaminhamentoAEEPorIdQuery(encaminhamentoId));

            encaminhamento.Situacao = VerificaEstudanteNecessitaAEE(encaminhamento) ? SituacaoAEE.Deferido : SituacaoAEE.Indeferido;

            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            if (!(usuarioLogado.EhProfessorPaee() || usuarioLogado.EhPerfilPaai()))
                throw new NegocioException(MensagemNegocioEncaminhamentoAee.SOMENTE_USUARIO_PAAE_OU_PAEE_PODE_CONCLUIR_O_ENCAMINHAMENTO);

            await mediator.Send(new SalvarEncaminhamentoAEECommand(encaminhamento));
            await mediator.Send(new ExecutaNotificacaoConclusaoEncaminhamentoAEECommand(encaminhamento.Id, usuarioLogado.CodigoRf, usuarioLogado.Nome));
            await ExcluirPendenciasEncaminhamentoAEE(encaminhamentoId);

            return true;
        }

        private async Task ExcluirPendenciasEncaminhamentoAEE(long encaminhamentoId)
        {
            var pendenciasEncaminhamentoAEE = await mediator.Send(new ObterPendenciasDoEncaminhamentoAEEPorIdQuery(encaminhamentoId));
            if (pendenciasEncaminhamentoAEE != null || !pendenciasEncaminhamentoAEE.Any())
            {
                foreach (var pendenciaEncaminhamentoAEE in pendenciasEncaminhamentoAEE)
                {
                    await mediator.Send(new ExcluirPendenciaEncaminhamentoAEECommand(pendenciaEncaminhamentoAEE.PendenciaId));
                }
            }
        }

        private bool VerificaEstudanteNecessitaAEE(EncaminhamentoAEE encaminhamento)
        {
            var secao = encaminhamento.Secoes.FirstOrDefault(c => c.SecaoEncaminhamentoAEE.Etapa == (int)TipoEtapaEncaminhamento.ParecerAEE);
            if (secao == null)
                throw new NegocioException(MensagemNegocioEncaminhamentoAee.NAO_LOCALIZADO_A_SECAO_ETAPA_3_NO_ENCAMINHAMENTO_AEE);

            var questao = secao.Questoes.FirstOrDefault(c => c.Questao.Ordem == 2 && c.Questao.Tipo == TipoQuestao.Radio);
            if (questao == null)
                throw new NegocioException(MensagemNegocioEncaminhamentoAee.QUESTAO_NAO_LOCALIZADO_PARA_IDENTIFICAR_SE_ESTUDANTE_CRIANCA_NECESSITA_ATENDIMENTO);

            var respostaQuestao = questao.Respostas.FirstOrDefault();
            if (respostaQuestao == null)
                throw new NegocioException(MensagemNegocioEncaminhamentoAee.NAO_LOCALIZADO_RESPOSTA_PARA_IDENTIFICAR_SE_ESTUDANTE_CRIANCA_NECESSITA_ATENDIMENTO);

            return respostaQuestao.Resposta.Nome == RESPOSTA_NOME_SIM;
        }
    }
}
