using Newtonsoft.Json;
using RabbitMQ.Client;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interfaces;
using System;
using System.Text;

namespace SME.SGP.Infra
{
    public class FilaRabbit : IServicoFila
    {

        private readonly IModel rabbitChannel;

        public FilaRabbit(IModel rabbitChannel)
        {
            this.rabbitChannel = rabbitChannel ?? throw new ArgumentNullException(nameof(rabbitChannel));
        }

        public void AdicionaFilaWorkerRelatorios(AdicionaFilaDto adicionaFilaDto)
        {
            var request = new MensagemRabbit(adicionaFilaDto.Endpoint, adicionaFilaDto.Filtros, adicionaFilaDto.CodigoCorrelacao);
            var mensagem = JsonConvert.SerializeObject(request);
            var body = Encoding.UTF8.GetBytes(mensagem);
            //TODO PENSAR NA EXCHANGE
            var properties = rabbitChannel.CreateBasicProperties();
            properties.Persistent = false;
            properties.Persistent = false;

            rabbitChannel.QueueBind(RotasRabbit.WorkerRelatoriosSgp, RotasRabbit.ExchangeListenerWorkerRelatorios, RotasRabbit.RotaRelatoriosSolicitados);
            rabbitChannel.BasicPublish(RotasRabbit.ExchangeListenerWorkerRelatorios, adicionaFilaDto.Fila, properties, body);
        }
    }
}



