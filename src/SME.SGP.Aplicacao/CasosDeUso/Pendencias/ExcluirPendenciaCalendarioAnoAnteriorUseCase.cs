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
            try
            {
                var anoLetivo = param?.Mensagem?.NaoEhNulo() ?? false ? JsonConvert.DeserializeObject<int>(param.Mensagem.ToString()!) : DateTimeExtension.HorarioBrasilia().AddYears(-1).Year;
                var uesId = await mediator.Send(ObterTodasUesIdsQuery.Instance);

                foreach (var ueId in uesId)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPendencias.RotaExcluirPendenciaCalendarioAnoAnteriorCalendarioUes, new ExcluirPendenciaCalendarioAnoAnteriorBuscarPorUeDto(anoLetivo, ueId), Guid.NewGuid(), null));

                return true;
            }
            catch (Exception e)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand(mensagem: "Não foi possível realizar a exclusão das pendências após o final do ano - Calendário ",
                    LogNivel.Critico,
                    LogContexto.Calendario,
                    innerException: e.InnerException?.ToString() ?? string.Empty,
                    rastreamento: e.StackTrace,
                    observacao: e.Message));
                throw;
            }
        }
    }
}