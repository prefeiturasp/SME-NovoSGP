using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/rabbit")]
    public class RabbitController : ControllerBase
    {
        [HttpGet("reiniciar")]
        public IActionResult Reiniciar([FromServices] IConnection conexaoRabbit, [FromServices] IModel canalRabbit)
        {

            if (conexaoRabbit == null || !conexaoRabbit.IsOpen)
            {
                var factory = new ConnectionFactory
                {
                    HostName = Environment.GetEnvironmentVariable("ConfiguracaoRabbit__HostName"),
                    UserName = Environment.GetEnvironmentVariable("ConfiguracaoRabbit__UserName"),
                    Password = Environment.GetEnvironmentVariable("ConfiguracaoRabbit__Password")
                };
                conexaoRabbit = factory.CreateConnection();
            }

            if (canalRabbit == null || canalRabbit.IsClosed)
            {
                canalRabbit = conexaoRabbit.CreateModel();
            }

            canalRabbit.ExchangeDeclare(ExchangeRabbit.Sgp, ExchangeType.Topic);
            canalRabbit.QueueDeclare(RotasRabbitSgp.FilaSgp, false, false, false, null);
            canalRabbit.QueueBind(RotasRabbitSgp.FilaSgp, ExchangeRabbit.Sgp, "*", null);

            canalRabbit.ExchangeDeclare(ExchangeRabbit.ServidorRelatorios, ExchangeType.Topic);
            canalRabbit.QueueDeclare(RotasRabbitSgp.FilaSgp, false, false, false, null);
            canalRabbit.QueueBind(RotasRabbitSgp.FilaSgp, ExchangeRabbit.ServidorRelatorios, "*", null);

            return Ok();
        }

    }
}