using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dados;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.IoC;
using SME.SGP.TesteIntegracao.ServicosFakes;
using System;
using System.Data;

namespace SME.SGP.TesteIntegracao.Setup
{
    public class RegistradorDependenciasTeste : RegistrarDependencias
    {
        // Handler único para ser configurado pelos testes
        public static readonly FakeHttpMessageHandler HttpHandlerFake = new FakeHttpMessageHandler();

        public override void Registrar(IServiceCollection services, IConfiguration configuration)
        {
            // Limpa cenários anteriores para garantir o isolamento entre coleções de testes
            HttpHandlerFake.LimparCenarios();
            base.Registrar(services, configuration);
        }
        protected override void RegistrarContextos(IServiceCollection services)
        {
            services.TryAddScoped<IHttpContextAccessor, HttpContextAccessorFake>();
            services.TryAddScoped<IContextoAplicacao, ContextoHttp>();

            services.TryAddScoped<ISgpContext>(provider =>
            {
                var connection = provider.GetService<IDbConnection>();
                var contextoAplicacao = provider.GetService<IContextoAplicacao>();
                return new SgpContext(connection, contextoAplicacao);
            });

            services.TryAddScoped<ISgpContextConsultas>(provider =>
            {
                var connection = provider.GetService<IDbConnection>();
                var contextoAplicacao = provider.GetService<IContextoAplicacao>();
                return new SgpContextConsultas(connection, contextoAplicacao);
            });

            services.TryAddScoped<IUnitOfWork, UnitOfWork>();
            base.RegistrarPolicies(services);
        }

        public override void RegistrarHttpClients(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();

            services.AddHttpClient(name: ServicosEolConstants.SERVICO, c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlApiEOL").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                // Adicione outros headers se o handler real os utilizar
            })
                .ConfigurePrimaryHttpMessageHandler(() => HttpHandlerFake);

            services.AddHttpClient(name: ServicoSondagemConstants.Servico, c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlApiSondagem").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            })
                .ConfigurePrimaryHttpMessageHandler(() => HttpHandlerFake);

            services.AddHttpClient(name: ServicoConectaFormacaoConstants.Servico, configureClient =>
            {
                configureClient.BaseAddress = new Uri(configuration.GetSection("UrlApiConectaFormacao").Value);
                configureClient.DefaultRequestHeaders.Add("Accept", "application/json");
            })
                .ConfigurePrimaryHttpMessageHandler(() => HttpHandlerFake);

            // Adicione aqui os outros clients que desejar "mockar", seguindo o mesmo padrão.
        }

        protected override void RegistrarServicos(IServiceCollection services)
        {
            services.TryAddScoped<IServicoJurema, ServicoJuremaFake>();
            services.TryAddScoped<IRepositorioCache, RepositorioCacheFake>();
            services.TryAddScoped<IServicoArmazenamento, ServicoArmazenamentoFake>();
            services.TryAddScoped<IServicoMensageriaLogs, ServicoMensageriaLogsFake>();
            services.TryAddScoped<IServicoMensageriaMetricas, ServicoMensageriaMetricasFake>();
            services.TryAddScoped<IServicoTelemetria, TelemetriaFake>();
            services.TryAddScoped<IConexoesRabbitFilasLog, ConexoesRabbitFilasLogFake>();
            services.TryAddScoped<IPooledObjectPolicy<IModel>, RabbitModelPooledObjectPolicyFake>();
            services.AddSingleton<ConfiguracaoRabbitLogOptions>();
            services.TryAddScoped<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.TryAddScoped<IServicoMensageriaApiEOL, ServicoMensageriaApiEOL>();
            base.RegistrarServicos(services);
        }

        public override void RegistrarTelemetria(IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddSingleton<IServicoTelemetria, TelemetriaFake>();
        }

        public override void RegistrarRabbit(IServiceCollection services, IConfiguration configuration)
        {
            //Não faz nada, pois não registramos o Rabbit

        }
    }
}
