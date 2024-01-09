using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/metricas")]
    public class MetricaController : ControllerBase
    {

        [HttpPost]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> PublicarFilasMetrica([FromServices] IMediator mediator, string fila, DateTime? mesAno, DateTime? data = null)
        {
            if (string.IsNullOrEmpty(fila))
                return BadRequest("Fila deve ser parametrizada para publicação de mensagem!");
            if (!data.HasValue && !mesAno.HasValue)
                return BadRequest("Data base ou mês/ano deve ser parametrizado para publicação de mensagem na fila!");

            if (data.HasValue)
                await mediator.Send(new PublicarFilaSgpCommand(fila, new FiltroDataMetricasDto(data.Value, true)));
            else
            {
                DateTime primeiroDiaDoMes = new DateTime(mesAno.Value.Year, mesAno.Value.Month, 1);
                DateTime ultimoDiaDoMes = primeiroDiaDoMes.AddMonths(1).AddDays(-1);
                for (DateTime dataAtual = primeiroDiaDoMes; dataAtual <= ultimoDiaDoMes; dataAtual = dataAtual.AddDays(1))
                    if (!dataAtual.FimDeSemana())
                        await mediator.Send(new PublicarFilaSgpCommand(fila, new FiltroDataMetricasDto(dataAtual, true),
                                                                       usuarioLogado: new Usuario() { CodigoRf= "Sistema", Nome = "Sistema", Login = "Sistema", PerfilAtual = new Guid()} ));
            }

            return Ok();
        }
    }
}