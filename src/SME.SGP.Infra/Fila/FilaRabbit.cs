using Newtonsoft.Json;
using RabbitMQ.Client;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public class FilaRabbit : IServicoFila
    {

        private readonly IModel rabbitChannel;

        public FilaRabbit(IModel rabbitChannel)
        {
            this.rabbitChannel = rabbitChannel ?? throw new ArgumentNullException(nameof(rabbitChannel));
        }
        public async Task AdicionaFila(AdicionaFilaDto adicionaFilaDto)
        {
            try
            {
                var request = new { action = adicionaFilaDto.Endpoint, adicionaFilaDto.Filtros };
                var mensagem = JsonConvert.SerializeObject(request);
                var body = Encoding.UTF8.GetBytes(mensagem);
                //TODO PENSAR NA EXCHANGE
                var properties = rabbitChannel.CreateBasicProperties();
                properties.Persistent = false;
                properties.Persistent = false;
                rabbitChannel.BasicPublish("sme.sr.workers", adicionaFilaDto.Fila, properties, body);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

