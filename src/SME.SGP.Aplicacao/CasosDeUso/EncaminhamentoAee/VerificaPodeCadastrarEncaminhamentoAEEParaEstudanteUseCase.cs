using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class VerificaPodeCadastrarEncaminhamentoAEEParaEstudanteUseCase : AbstractUseCase, IVerificaPodeCadastrarEncaminhamentoAEEParaEstudanteUseCase
    {
        public VerificaPodeCadastrarEncaminhamentoAEEParaEstudanteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(FiltroEncaminhamentoAeeDto filtroEncaminhamentoAee)
        {
            var encaminhamentoAEE = await mediator.Send(new ObterEncaminhamentoAEEPorEstudanteQuery(filtroEncaminhamentoAee.EstudanteCodigo,
                filtroEncaminhamentoAee.UeCodigo));

            if (encaminhamentoAEE.NaoEhNulo() && SituacaoDiferenteIndeferidoOuEncerradoAutomaticamente(encaminhamentoAEE))
                throw new NegocioException(MensagemNegocioEncaminhamentoAee.ESTUDANTE_JA_POSSUI_ENCAMINHAMENTO_AEE_EM_ABERTO);

            return true;
        }

        private static bool SituacaoDiferenteIndeferidoOuEncerradoAutomaticamente(EncaminhamentoAEEResumoDto encaminhamentoAEE)
        {
            return !(encaminhamentoAEE.SituacaoTipo == SituacaoAEE.Indeferido 
                     || encaminhamentoAEE.SituacaoTipo == SituacaoAEE.EncerradoAutomaticamente);
        }
    }
}
