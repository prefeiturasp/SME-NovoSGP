using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverPendenciasNoFinalDoAnoLetivoUseCase : AbstractUseCase, IRemoverPendenciasNoFinalDoAnoLetivoUseCase
    {
        public RemoverPendenciasNoFinalDoAnoLetivoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var pendenciasIds = JsonConvert.DeserializeObject<List<long>>(param?.Mensagem?.ToString());

            if (pendenciasIds.Any())
            {
                await mediator.Send(new ExcluirPendenciasPorIdsCommand() { PendenciasIds = pendenciasIds.ToArray() });
            }

            return true;
        }
    }
}
