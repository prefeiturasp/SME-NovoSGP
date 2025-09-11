using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class RemoverPendenciasCalendarioNoFinalDoAnoLetivoUseCase : AbstractUseCase, IRemoverPendenciasCalendarioNoFinalDoAnoLetivoUseCase
    {
        public RemoverPendenciasCalendarioNoFinalDoAnoLetivoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
         {
            IEnumerable<long> pendenciasIds = new List<long>();
            try
            {
                pendenciasIds = JsonConvert.DeserializeObject<List<long>>(param.Mensagem.ToString());

                if (pendenciasIds.Any())
                    await mediator.Send(new ExcluirPendenciasPorIdsCommand() { PendenciasIds = pendenciasIds.ToArray() });

                return true;
            }
            catch (Exception e)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand(mensagem: "Não foi possível realizar a exclusão das pendências após o final do ano - Calendário ",
                    LogNivel.Critico,
                    LogContexto.Calendario,
                    innerException: e.InnerException!.ToString(),
                    rastreamento: e.StackTrace,
                    observacao: $"ID Pendencias: {JsonConvert.SerializeObject(pendenciasIds.ToArray())} ,Erro:{e.Message}"));
                throw;
            }
        }
    }
}