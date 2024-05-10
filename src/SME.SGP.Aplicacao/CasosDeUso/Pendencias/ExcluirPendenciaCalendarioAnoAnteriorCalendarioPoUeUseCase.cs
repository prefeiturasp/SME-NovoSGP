using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaCalendarioAnoAnteriorCalendarioPoUeUseCase : AbstractUseCase, IExcluirPendenciaCalendarioAnoAnteriorCalendarioPoUeUseCase
    {
        public ExcluirPendenciaCalendarioAnoAnteriorCalendarioPoUeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            IEnumerable<long> pendenciasIds = new List<long>();

            try
            {
                var filtro = param.ObterObjetoMensagem<ExcluirPendenciaCalendarioAnoAnteriorPorUeDto>();

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPendencias.RotaExcluirPendenciaCalendarioAnoAnteriorCalendarioIdsPendencias, filtro.PendenciasId, Guid.NewGuid(), null));

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