using Elastic.Apm;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Excecoes;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.Infra.Worker;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Workers
{
    public abstract class WorkerRabbitAplicacao : WorkerRabbitSGP
    {
        private readonly IMediator mediator;

        protected WorkerRabbitAplicacao(IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaSGP servicoMensageria,
            IServicoMensageriaMetricas servicoMensageriaMetricas,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory,
            string apmTransactionType,
            Type tipoRotas)
            : base(serviceScopeFactory, servicoTelemetria, servicoMensageria, servicoMensageriaMetricas, telemetriaOptions, consumoFilasOptions, factory, apmTransactionType, tipoRotas)
        {
            var scope = serviceScopeFactory.CreateScope();
            mediator = scope.ServiceProvider.GetService<IMediator>();
        }


        protected override async Task RegistrarErroTratamentoMensagem(BasicDeliverEventArgs ea, MensagemRabbit mensagemRabbit, Exception ex, LogNivel logNivel, string observacao)
        {
            var mensagem = $"{mensagemRabbit.UsuarioLogadoRF} - {mensagemRabbit.CodigoCorrelacao.ToString()[..3]} - ERRO - {ea.RoutingKey}";
            await RegistrarErro(mensagem, logNivel, observacao, ex?.StackTrace, ex?.InnerException?.Message);
        }

        protected override Task RegistrarErro(string mensagem, LogNivel logNivel, string observacao = "", string rastreamento = "", string excecaoInterna = "")
            => mediator.Send(new SalvarLogViaRabbitCommand(mensagem, logNivel, LogContexto.WorkerRabbit, observacao, rastreamento: rastreamento, excecaoInterna: excecaoInterna));

        protected override void AtribuirContextoAplicacao(MensagemRabbit mensagemRabbit, IServiceScope scope)
        {
            if (!string.IsNullOrWhiteSpace(mensagemRabbit?.UsuarioLogadoRF))
            {
                var contextoAplicacao = scope.ServiceProvider.GetService<IContextoAplicacao>();

                var variaveis = new Dictionary<string, object>
                {
                    { "NomeUsuario", mensagemRabbit.UsuarioLogadoNomeCompleto },
                    { "UsuarioLogado", mensagemRabbit.UsuarioLogadoRF },
                    { "RF", mensagemRabbit.UsuarioLogadoRF },
                    { "login", mensagemRabbit.UsuarioLogadoRF },
                    { "Claims", new List<InternalClaim> { new InternalClaim { Value = mensagemRabbit.PerfilUsuario, Type = "perfil" } } },
                    { "Administrador", mensagemRabbit.Administrador }
                };

                contextoAplicacao.AdicionarVariaveis(variaveis);
            }
        }

        protected override void NotificarErroUsuario(string message, string usuarioRf, string nomeProcesso)
        {
            if (!string.IsNullOrEmpty(usuarioRf))
            {
                var command = new NotificarUsuarioCommand($"Ocorreu um erro ao: '{nomeProcesso}'",
                    message,
                    usuarioRf,
                    NotificacaoCategoria.Aviso,
                    NotificacaoTipo.Worker);

                var request = new MensagemRabbit(string.Empty, command, Guid.NewGuid(), usuarioRf);
                var mensagem = JsonConvert.SerializeObject(request);
                var body = Encoding.UTF8.GetBytes(mensagem);

                canalRabbit.BasicPublish(ExchangeSgpRabbit.Sgp, RotasRabbitSgp.RotaNotificacaoUsuario, null, body);
            }
        }
    }
}
