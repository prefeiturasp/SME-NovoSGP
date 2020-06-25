using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Pipelines;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.IoC;
using System;
using System.Net;

namespace SME.SGP.Worker.Rabbit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    RegistraDependencias.Registrar(services);
                    RegistrarMediator(services);
                    RegistrarHttpClients(services, hostContext.Configuration);
                    RegistrarRabbit(services);
                    services.AddDistributedRedisCache(options =>
                    {
                        options.Configuration = hostContext.Configuration.GetConnectionString("SGP-Redis");
                        options.InstanceName = hostContext.Configuration.GetValue<string>("Nome-Instancia-Redis");
                    });

                    services.TryAddScoped<IHttpContextAccessor, NoHttpContext>();

                    services.AddApplicationInsightsTelemetryWorkerService(hostContext.Configuration);

                    services.AddHostedService<WorkerRabbitMQ>();
                });



        private static void RegistrarMediator(IServiceCollection services)
        {
            var assembly = AppDomain.CurrentDomain.Load("SME.SGP.Aplicacao");
            services.AddMediatR(assembly);
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidacoesPipeline<,>));
        }
        private static void RegistrarHttpClients(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IServicoJurema, ServicoJurema>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlApiJurema").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            services.AddHttpClient<IServicoEol, ServicoEOL>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlApiEOL").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            services.AddHttpClient<IServicoAcompanhamentoEscolar, ServicoAcompanhamentoEscolar>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlApiAE").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddHttpClient<IServicoGithub, SevicoGithub>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlApiGithub").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });


            var cookieContainer = new CookieContainer();
            var jasperCookieHandler = new JasperCookieHandler() { CookieContainer = cookieContainer };

            services.AddSingleton(jasperCookieHandler);

            var basicAuth = $"{configuration.GetValue<string>("ConfiguracaoJasper:Username")}:{configuration.GetValue<string>("ConfiguracaoJasper:Password")}".EncodeTo64();
            var jasperUrl = configuration.GetValue<string>("ConfiguracaoJasper:Hostname");

            services.AddHttpClient<ISevicoJasper, SevicoJasper>(c =>
            {
                c.BaseAddress = new Uri(jasperUrl);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("Authorization", $"Basic {basicAuth}");
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new JasperCookieHandler() { CookieContainer = cookieContainer };
            });
        }
        private static void RegistrarRabbit(IServiceCollection services)
        {
            var factory = new ConnectionFactory
            {
                HostName = Environment.GetEnvironmentVariable("ConfiguracaoRabbit__HostName"),
                UserName = Environment.GetEnvironmentVariable("ConfiguracaoRabbit__UserName"),
                Password = Environment.GetEnvironmentVariable("ConfiguracaoRabbit__Password")
            };

            var conexaoRabbit = factory.CreateConnection();
            IModel canalRabbit = conexaoRabbit.CreateModel();
            services.AddSingleton(conexaoRabbit);
            services.AddSingleton(canalRabbit);

            canalRabbit.ExchangeDeclare(RotasRabbit.ExchangeSgp, ExchangeType.Topic);
            canalRabbit.QueueDeclare(RotasRabbit.FilaSgp, false, false, false, null);
            canalRabbit.QueueBind(RotasRabbit.FilaSgp, RotasRabbit.ExchangeSgp, "*");
        }

    }
}
