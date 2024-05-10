using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaCalendarioAnoAnteriorCalendarioBuscarPorUeUseCase : AbstractUseCase, IExcluirPendenciaCalendarioAnoAnteriorCalendarioBuscarPorUeUseCase
    {
        public ExcluirPendenciaCalendarioAnoAnteriorCalendarioBuscarPorUeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<ExcluirPendenciaCalendarioAnoAnteriorBuscarPorUeDto>();

            try
            {
                var pendenciasPorUe = await mediator
                    .Send(new ObterPendenciasCalendarioPorUeQuery(filtro.AnoLetivo ?? DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, filtro.UeId));

                if (pendenciasPorUe.NaoEhNulo() && pendenciasPorUe.Any())
                {
                    var mensagem = new ExcluirPendenciaCalendarioAnoAnteriorPorUeDto(filtro.AnoLetivo, filtro.UeId, pendenciasPorUe.ToArray());

                    return await mediator
                        .Send(new PublicarFilaSgpCommand(RotasRabbitSgpPendencias.RotaExcluirPendenciaCalendarioAnoAnteriorCalendarioUe, mensagem, Guid.NewGuid()));
                }

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand(mensagem: "Não foi possível obter as pendências da UE para exclusão após o final do ano - Calendário ",
                    LogNivel.Critico,
                    LogContexto.Calendario,
                    innerException: ex.InnerException?.ToString(),
                    rastreamento: ex.StackTrace,
                    observacao: $"UE Id: {filtro.UeId},Erro:{ex.Message}"));

                throw;
            }
        }
    }
}
