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
    public class ExcluirPendenciaCalendarioAnoAnteriorUseCase : AbstractUseCase, IExcluirPendenciaCalendarioAnoAnteriorCalendarioUseCase
    {
        public ExcluirPendenciaCalendarioAnoAnteriorUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            IEnumerable<TodosUesIdsComPendenciaCalendarioDto> pendencias = new List<TodosUesIdsComPendenciaCalendarioDto>();
            int anoLetivo;
            try
            {
                anoLetivo = param.Mensagem.NaoEhNulo() ? JsonConvert.DeserializeObject<int>(param.Mensagem.ToString()!) : DateTimeExtension.HorarioBrasilia().AddYears(-1).Year;
                pendencias = await mediator.Send(new ObterTodosUesIdsComPendenciaCalendarioQuery(anoLetivo));
                
                foreach (var ueId in pendencias.Select(p => p.UeId).Distinct())
                {
                    var idPendencias = pendencias.Where(x => x.UeId == ueId).Select(e => e.PendenciaId).ToArray();
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPendencias.RotaExcluirPendenciaCalendarioAnoAnteriorCalendarioUe, new ExcluirPendenciaCalendarioAnoAnteriorPorUeDto(anoLetivo,ueId,idPendencias), Guid.NewGuid(), null));
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
                    observacao: e.Message));
                throw;
            }
        }
    }
}