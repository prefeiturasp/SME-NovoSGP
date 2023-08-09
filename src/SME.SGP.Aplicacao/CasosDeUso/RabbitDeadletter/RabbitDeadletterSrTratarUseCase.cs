using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Registry;
using RabbitMQ.Client;
using SME.SGP.Infra;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;

namespace SME.SGP.Aplicacao.CasosDeUso
{


    //nao seria mais interessante trabalhar com modelo de consumer de mensagem ao inves de polling? channel.BasicConsume algo do tipo
    //e nao ficar relancando todas vezes
    public class RabbitDeadletterSrTratarUseCase : IRabbitDeadletterSrTratarUseCase
    {
        private readonly IConfiguration configuration;
        private readonly IAsyncPolicy policy;

        public RabbitDeadletterSrTratarUseCase(IConfiguration configuration, IReadOnlyPolicyRegistry<string> registry)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.policy = registry.Get<IAsyncPolicy>(PoliticaPolly.PublicaFila);
        }


        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var fila = mensagemRabbit.Mensagem.ToString();

            var configuracaoRabbit = configuration.GetSection("ConfiguracaoRabbit");
            var connectionFactory = new ConnectionFactory
            {
                Port = configuracaoRabbit.GetValue<int>("Port"),
                HostName = configuracaoRabbit.GetValue<string>("HostName"),
                UserName = configuracaoRabbit.GetValue<string>("UserName"),
                Password = configuracaoRabbit.GetValue<string>("Password"),
                VirtualHost = configuracaoRabbit.GetValue<string>("Virtualhost")
            };

            await policy.ExecuteAsync(() => TratarMensagens(fila, connectionFactory));

            //da pra retornar direto aqui nao ?
            return true;
        }

        private async Task TratarMensagens(string fila, ConnectionFactory factory)
        {
            using var conexaoRabbit = factory.CreateConnection();
            using var channel = conexaoRabbit.CreateModel();

            //Nao seria ideal gerenciar esses tipos de controle de loop com algum dispose para suportar graceful shutdown
            //caso precise parar essas threads dos workers fora do laco de while ?
            //while (running) algo assim com dispose que seta running para false
            //ou ate mesmo algo mais inteligente no while como readers como enumerables

            //var basicGetResults = BasicGetResults(channel, $"{fila}.deadletter");
            var basicGetResults = new BasicGetResultEnumerable(channel, $"{fila}.deadletter");
            foreach(var basicGetResult in basicGetResults)
            {
                //essa mensagem não é persistente ?
                await Task.Run(() => channel.BasicPublish(ExchangeSgpRabbit.ServidorRelatorios, fila, null, basicGetResult.Body.ToArray()));
            }

        }

        //Ou metodo estatico com yield return ou algo do tipo
        public IEnumerable<BasicGetResult> BasicGetResults(IModel channel, string fila)
        {
            BasicGetResult basicGetResult;
            while ((basicGetResult = channel.BasicGet(fila, true)) is not null)
            {
                yield return basicGetResult;
            }
        }

        //Ou talvez extrair esse reader em uma classe separada que outros usariam para o modelo de pooling
        public class BasicGetResultEnumerable : IEnumerable<BasicGetResult>,IDisposable
        {
            private readonly IModel _channel;
            private readonly string _fila;

            public BasicGetResultEnumerable(IModel channel, string fila)
            {
                _channel = channel;
                _fila = fila;
            }

            public IEnumerator<BasicGetResult> GetEnumerator() => new BasicGetResultEnumerator(_channel,_fila);
            IEnumerator IEnumerable.GetEnumerator() => new BasicGetResultEnumerator(_channel,_fila);

            public void Dispose()
            {

            }

        }

        public class BasicGetResultEnumerator : IEnumerator<BasicGetResult>
        {
            private readonly IModel _channel;
            private readonly string _fila;
            private BasicGetResult _current;

            public BasicGetResultEnumerator(IModel channel, string fila)
            {
                _channel = channel;
                _fila = fila;
            }

            public bool MoveNext()
            {
                _current = _channel.BasicGet(_fila, true);
                return _current is not null;
            }

            public void Reset()
            {

            }

            public BasicGetResult Current => _current;
            object IEnumerator.Current => _current;

            public void Dispose()
            {

            }
        }

    }
}
