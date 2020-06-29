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

            canalRabbit.ExchangeDeclare(RotasRabbit.ExchangeSgp, ExchangeType.Topic);
            canalRabbit.QueueDeclare(RotasRabbit.FilaSgp, false, false, false, null);
            canalRabbit.QueueBind(RotasRabbit.FilaSgp, RotasRabbit.ExchangeSgp, "*", null);

            canalRabbit.ExchangeDeclare(RotasRabbit.ExchangeServidorRelatorios, ExchangeType.Topic);
            canalRabbit.QueueDeclare(RotasRabbit.FilaSgp, false, false, false, null);
            canalRabbit.QueueBind(RotasRabbit.FilaSgp, RotasRabbit.ExchangeServidorRelatorios, "*", null);

            return Ok();
        }

    }
}