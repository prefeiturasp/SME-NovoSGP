using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interfaces;

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

            
            var mensagem = JsonConvert.SerializeObject(adicionaFilaDto.Dados);

            var body = Encoding.UTF8.GetBytes(mensagem);
            
            //TODO PENSAR NA EXCHANGE
            rabbitChannel.BasicPublish("sme.sr.workers", adicionaFilaDto.Fila, null, body);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}


            
