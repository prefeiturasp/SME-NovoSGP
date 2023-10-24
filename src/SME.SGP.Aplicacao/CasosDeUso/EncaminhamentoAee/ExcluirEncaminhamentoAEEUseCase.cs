using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class ExcluirEncaminhamentoAEEUseCase : AbstractUseCase, IExcluirEncaminhamentoAEEUseCase
    {
        public ExcluirEncaminhamentoAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long encaminhamentoAeeId)
        {
            var encaminhamentoAee = await mediator.Send(new ObterEncaminhamentoAEEPorIdQuery(encaminhamentoAeeId));

            if (encaminhamentoAee.EhNulo() )
                throw new NegocioException(MensagemNegocioEncaminhamentoAee.ENCAMINHAMENTO_NAO_ENCONTRADO);
            
            if (!(encaminhamentoAee.Situacao == SituacaoAEE.Rascunho || encaminhamentoAee.Situacao == SituacaoAEE.Encaminhado))
                throw new NegocioException(MensagemNegocioEncaminhamentoAee.ENCAMINHAMENTO_NAO_PODE_SER_EXCLUIDO_NESSA_SITUACAO);
            
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            
            if (usuarioLogado.EhGestorEscolar() || encaminhamentoAee.CriadoRF.Equals(usuarioLogado.CodigoRf))
            {
                await mediator.Send(new ExcluirEncaminhamentoAEECommand(encaminhamentoAeeId));

                await ExcluirPendenciasEncaminhamentoAEE(encaminhamentoAeeId);
                
                return true;
            }
            
            throw new NegocioException(MensagemNegocioEncaminhamentoAee.ENCAMINHAMENTO_NAO_PODE_SER_EXCLUIDO_PELO_USUARIO_LOGADO);
        }

        private async Task ExcluirPendenciasEncaminhamentoAEE(long encaminhamentoId)
        {
            var pendenciasEncaminhamentoAEE = await mediator.Send(new ObterPendenciasDoEncaminhamentoAEEPorIdQuery(encaminhamentoId));
            if (pendenciasEncaminhamentoAEE.NaoEhNulo() || !pendenciasEncaminhamentoAEE.Any())
            {
                foreach (var pendenciaEncaminhamentoAEE in pendenciasEncaminhamentoAEE)
                {
                    await mediator.Send(new ExcluirPendenciaEncaminhamentoAEECommand(pendenciaEncaminhamentoAEE.PendenciaId));
                }
            }
        }
    }
}
