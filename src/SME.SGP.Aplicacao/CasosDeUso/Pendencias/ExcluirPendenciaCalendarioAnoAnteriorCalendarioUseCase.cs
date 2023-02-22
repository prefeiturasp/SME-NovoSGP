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
    public class ExcluirPendenciaCalendarioAnoAnteriorCalendarioUseCase : AbstractUseCase, IExcluirPendenciaCalendarioAnoAnteriorCalendarioUseCase
    {
        public ExcluirPendenciaCalendarioAnoAnteriorCalendarioUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            IEnumerable<long> idsUes = new List<long>(); ;
            long ueId = 0;
            try
            {
                idsUes = await mediator.Send(new ObterTodasUesIdsQuery());

                foreach (var idUe in idsUes)
                {
                    ueId = idUe;
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPendencias.RotaExcluirPendenciaCalendarioAnoAnteriorCalendarioUe, idUe, Guid.NewGuid(), null));
                }

                return true;
            }
            catch (Exception e)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand(mensagem: "Não foi possível realizar a exclusão das pendências após o final do ano - Calendário ",
                    LogNivel.Critico,
                    LogContexto.Calendario,
                    innerException: e.InnerException!.ToString(),
                    rastreamento: e.StackTrace,
                    observacao: $"Id das UEs: {JsonConvert.SerializeObject(idsUes.ToArray())}, Id da UE = {ueId} ,Erro:{e.Message}"));
                throw;
            }
        }
    }
}