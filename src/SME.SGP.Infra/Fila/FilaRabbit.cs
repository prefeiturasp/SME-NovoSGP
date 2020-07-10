using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Sentry;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interfaces;
using System;
using System.Text;

namespace SME.SGP.Infra
{
    public class FilaRabbit : IServicoFila
    {

        private readonly IModel rabbitChannel;
        private readonly IConfiguration configuration;

        public FilaRabbit(IModel rabbitChannel, IConfiguration configuration)
        {
            this.rabbitChannel = rabbitChannel ?? throw new ArgumentNullException(nameof(rabbitChannel));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void PublicaFilaWorkerServidorRelatorios(PublicaFilaRelatoriosDto adicionaFilaDto)
        {
            byte[] body = FormataBodyWorker(adicionaFilaDto);

            rabbitChannel.QueueBind(RotasRabbit.WorkerRelatoriosSgp, RotasRabbit.ExchangeServidorRelatorios, RotasRabbit.RotaRelatoriosSolicitados);
            rabbitChannel.BasicPublish(RotasRabbit.ExchangeServidorRelatorios, adicionaFilaDto.Fila, null, body);

            SentrySdk.CaptureMessage("3 - AdicionaFilaWorkerRelatorios");
        }

        public void PublicaFilaWorkerSgp(PublicaFilaSgpDto publicaFilaSgpDto)
        {
            var request = new MensagemRabbit(publicaFilaSgpDto.Filtros,
                                             publicaFilaSgpDto.CodigoCorrelacao,
                                             publicaFilaSgpDto.UsuarioLogadoNomeCompleto,
                                             publicaFilaSgpDto.UsuarioLogadoRF,
                                             publicaFilaSgpDto.PerfilUsuario,
                                             publicaFilaSgpDto.NotificarErroUsuario);

            var mensagem = JsonConvert.SerializeObject(request);
            var body = Encoding.UTF8.GetBytes(mensagem);

            rabbitChannel.QueueBind(RotasRabbit.FilaSgp, RotasRabbit.ExchangeSgp, publicaFilaSgpDto.NomeFila);
            rabbitChannel.BasicPublish(RotasRabbit.ExchangeSgp, publicaFilaSgpDto.NomeFila, null, body);
        }

        private static byte[] FormataBodyWorker(PublicaFilaRelatoriosDto adicionaFilaDto)
        {
            var request = new MensagemRabbit(adicionaFilaDto.Endpoint, adicionaFilaDto.Mensagem, adicionaFilaDto.CodigoCorrelacao,adicionaFilaDto.UsuarioLogadoRF, adicionaFilaDto.NotificarErroUsuario, adicionaFilaDto.PerfilUsuario);
            var mensagem = JsonConvert.SerializeObject(request);
            var body = Encoding.UTF8.GetBytes(mensagem);
            return body;
        }
    }
}



