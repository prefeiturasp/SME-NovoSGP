using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class DevolverEncaminhamentoUseCase : AbstractUseCase, IDevolverEncaminhamentoUseCase
    {
        public DevolverEncaminhamentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(DevolucaoEncaminhamentoAEEDto devolucaoDto)
        {
            var encaminhamento = await mediator.Send(new ObterEncaminhamentoAEEPorIdQuery(devolucaoDto.EncaminhamentoAEEId));

            if (encaminhamento.Situacao != SituacaoAEE.Encaminhado)
                throw new NegocioException(MensagemNegocioEncaminhamentoAee.ENCAMINHAMENTO_SO_PODEM_SER_DEVOLVIDOS_NA_SITUACAO_ENCAMINHADO);

            encaminhamento.Situacao = SituacaoAEE.Devolvido;

            await mediator.Send(new SalvarEncaminhamentoAEECommand(encaminhamento));

            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            if (usuarioLogado.EhGestorEscolar())
            {
                await mediator.Send(new ExecutaNotificacaoDevolucaoEncaminhamentoAEECommand(encaminhamento.Id, usuarioLogado.CodigoRf, usuarioLogado.Nome, devolucaoDto.Motivo));

                await ExcluirPendenciasEncaminhamentoAEE(encaminhamento.Id);

                return true;
            }
            throw new NegocioException(MensagemNegocioEncaminhamentoAee.ENCAMINHAMENTO_SO_PODEM_SER_DEVOLVIDO_PELA_GESTAO);
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
    }
}
