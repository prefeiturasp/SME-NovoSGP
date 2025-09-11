using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverPendenciasDiarioDeClasseNoFinalDoAnoLetivoUseCase : AbstractUseCase, IRemoverPendenciasDiarioDeClasseNoFinalDoAnoLetivoUseCase
    {
        public RemoverPendenciasDiarioDeClasseNoFinalDoAnoLetivoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = JsonConvert.DeserializeObject<FiltroRemoverPendenciaFinalAnoLetivoDto>(param?.Mensagem?.ToString());

            if (!string.IsNullOrEmpty(filtro.CodigoUe)) 
            {
                var idsPendencia = new List<long>();
                var idsPendenciaAula = await mediator.Send(new ObterIdsPendenciaAulaPorAnoLetivoQuery(filtro.AnoLetivo, filtro.CodigoUe));
                var idsPendenciaDiario = await mediator.Send(new ObterIdsPendenciaDiarioBordoPorAnoLetivoQuery(filtro.AnoLetivo, filtro.CodigoUe));
                var idsPendenciaIndividual = await mediator.Send(new ObterIdsPendenciaIndividualPorAnoLetivoQuery(filtro.AnoLetivo, filtro.CodigoUe));
                var idsPendenciaDevolutiva = await mediator.Send(new ObterIdsPendenciaDevolutivaPorAnoLetivoQuery(filtro.AnoLetivo, filtro.CodigoUe));

                idsPendencia.AddRange(idsPendenciaAula);
                idsPendencia.AddRange(idsPendenciaDiario);
                idsPendencia.AddRange(idsPendenciaIndividual);
                idsPendencia.AddRange(idsPendenciaDevolutiva);

                if (idsPendencia.Any())
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPendencias.RotaExecutarExclusaoPendenciasNoFinalDoAnoLetivo, idsPendencia, Guid.NewGuid()));
            }

            return true;
        }
    }
}
