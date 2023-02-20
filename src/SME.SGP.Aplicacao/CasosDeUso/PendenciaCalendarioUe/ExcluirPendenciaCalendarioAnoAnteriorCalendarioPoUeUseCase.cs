using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaCalendarioAnoAnteriorCalendarioPoUeUseCase : AbstractUseCase, IExcluirPendenciaCalendarioAnoAnteriorCalendarioPoUeUseCase
    {
        public ExcluirPendenciaCalendarioAnoAnteriorCalendarioPoUeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            long ueId = 0;
            var anoLetivo = (DateTimeExtension.HorarioBrasilia().Year - 1);
            IEnumerable<long> pendenciasIds = new List<long>();
            try
            {
                ueId = JsonConvert.DeserializeObject<long>(param.Mensagem.ToString());
                pendenciasIds =  await mediator.Send(new ObterPendenciasCalendarioUeEAnoLetivoParaEncerramentoAutomaticoQuery(ueId,anoLetivo));

                if (pendenciasIds.Any())
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPendencias.RotaExcluirPendenciaCalendarioAnoAnteriorCalendarioIdsPendencias, pendenciasIds, Guid.NewGuid(), null));
            
                return true;
            }
            catch (Exception e)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand( mensagem:"Não foi possível realizar a exclusão das pendências após o final do ano - Calendário ",
                    LogNivel.Critico,
                    LogContexto.Calendario,
                    innerException:e.InnerException!.ToString(),
                    rastreamento:e.StackTrace,
                    observacao:$"Id da ue: {ueId}, Ano Letivo: {anoLetivo}, ID Pendencias: {JsonConvert.SerializeObject(pendenciasIds.ToArray())} ,Erro:{e.Message}"));
                throw;
            }
        }
    }
}