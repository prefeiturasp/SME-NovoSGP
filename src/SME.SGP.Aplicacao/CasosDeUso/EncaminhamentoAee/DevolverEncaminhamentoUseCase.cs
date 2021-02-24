using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

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
                throw new NegocioException($"Encaminhamento só podem ser devolvidos na situação '{SituacaoAEE.Encaminhado.Name()}'");

            encaminhamento.Situacao = SituacaoAEE.Devolvido;

            await mediator.Send(new SalvarEncaminhamentoAEECommand(encaminhamento));

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            await mediator.Send(new ExecutaNotificacaoDevolucaoEncaminhamentoAEECommand(encaminhamento.Id, usuarioLogado.CodigoRf, usuarioLogado.Nome, devolucaoDto.Motivo));

            await ExcluirPendenciasEncaminhamentoAEE(encaminhamento.Id);

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
    }
}
