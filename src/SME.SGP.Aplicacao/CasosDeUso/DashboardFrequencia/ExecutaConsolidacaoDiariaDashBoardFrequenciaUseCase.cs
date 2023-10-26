using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class ExecutaConsolidacaoDiariaDashBoardFrequenciaUseCase : AbstractUseCase, IExecutaConsolidacaoDiariaDashBoardFrequenciaUseCase
    {
        public ExecutaConsolidacaoDiariaDashBoardFrequenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtroConsolicacao = mensagem.ObterObjetoMensagem<FiltroConsolicacaoDiariaDashBoardFrequenciaDTO>();

            var uesCodigos = await mediator.Send(ObterCodigosUEsQuery.Instance);

            if (uesCodigos.EhNulo() || !uesCodigos.Any())
                return false;

            foreach (var ueCodigo in uesCodigos)
            {
                var dados = new ConsolidacaoPorUeDashBoardFrequencia()
                {
                    AnoLetivo = filtroConsolicacao.AnoLetivo,
                    Mes = filtroConsolicacao.Mes,
                    UeCodigo = ueCodigo
                };

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaConsolidacaoDiariaDashBoardFrequenciaPorUe, dados, Guid.NewGuid(), null));
            }
            return true;
        }
    }
}
