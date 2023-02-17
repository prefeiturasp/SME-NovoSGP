using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverPendenciasNoFinalDoAnoLetivoPorUeUseCase : AbstractUseCase, IRemoverPendenciasNoFinalDoAnoLetivoPorUeUseCase
    {
        public RemoverPendenciasNoFinalDoAnoLetivoPorUeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = JsonConvert.DeserializeObject<FiltroRemoverPendenciaFinalAnoLetivoDto>(param.Mensagem.ToString());

            if (filtro.DreId > 0)
            {
                var IdsUes = await mediator.Send(new ObterUEsIdsPorDreQuery(filtro.DreId));

                foreach(var idUe in IdsUes)
                {
                    var filtroUe = new FiltroRemoverPendenciaFinalAnoLetivoDto(filtro.AnoLetivo, filtro.DreId, idUe);
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPendencias.RotaExecutarExclusaoPendenciasDiarioDeClasseNoFinalDoAnoLetivo, filtroUe, Guid.NewGuid()));
                }
            }

            return true;
        }
    }
}
