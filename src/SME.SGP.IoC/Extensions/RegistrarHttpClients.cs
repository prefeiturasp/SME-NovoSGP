using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Mensageria;
using System;
using System.Net;
using System.Net.Http;

namespace SME.SGP.IoC
{
    internal static class RegistrarHttpClients
    {
        internal static void AdicionarHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IServicoJurema, ServicoJurema>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlApiJurema").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddHttpClient(name: ServicosEolConstants.SERVICO, c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlApiEOL").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("x-api-eol-key", configuration.GetSection("ApiKeyEolApi").Value);
                
                if (configuration.GetSection("HttpClientTimeoutSecond").Value.NaoEhNulo())
                    c.Timeout = TimeSpan.FromSeconds(double.Parse(configuration.GetSection("HttpClientTimeoutSecond").Value));
            });

            services.AddHttpClient(name: ServicoSondagemConstants.Servico, c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlApiSondagem").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("x-sondagem-api-key", configuration.GetSection("ChaveIntegracaoSondagemApi").Value);
                
                if (configuration.GetSection("HttpClientTimeoutSecond").Value.NaoEhNulo())
                    c.Timeout = TimeSpan.FromSeconds(double.Parse(configuration.GetSection("HttpClientTimeoutSecond").Value));
            }).AddPolicyHandler(GetRetryPolicy());

            services.AddHttpClient(name: ServicoSerapConstants.ServicoSERApLegado, c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlApiSERApLegado").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("keyApi", configuration.GetSection("ChaveIntegracaoSERApLegadoApi").Value);

                if (configuration.GetSection("HttpClientTimeoutSecond").Value.NaoEhNulo())
                    c.Timeout = TimeSpan.FromSeconds(double.Parse(configuration.GetSection("HttpClientTimeoutSecond").Value));
            }).AddPolicyHandler(GetRetryPolicy());

            services.AddHttpClient<IServicoAcompanhamentoEscolar, ServicoAcompanhamentoEscolar>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlApiAE").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddHttpClient(name: "servicoAcompanhamentoEscolar", c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlApiAE").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("x-integration-key", configuration.GetSection("AE_ChaveIntegracao").Value);
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

            services.AddHttpClient<IServicoServidorRelatorios, ServicoServidorRelatorios>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlServidorRelatorios").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("x-sr-api-key", configuration.GetSection("ApiKeySr").Value);
            });

            services.AddHttpClient(name: "servicoServidorRelatorios", c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlServidorRelatorios").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("x-sr-api-key", configuration.GetSection("ApiKeySr").Value);
            });

            services.AddHttpClient(name: ServicoConectaFormacaoConstants.Servico, c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlApiConectaFormacao").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                if (configuration.GetSection("HttpClientTimeoutSecond").Value.NaoEhNulo())
                    c.Timeout = TimeSpan.FromSeconds(double.Parse(configuration.GetSection("HttpClientTimeoutSecond").Value));
            }).AddPolicyHandler(GetRetryPolicy());

            services.AdicionarHttpClientsProdam(configuration);
        }

        private static void AdicionarHttpClientsProdam(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<ConfiguracaoProdamOptions>()
                .Bind(configuration.GetSection(ConfiguracaoProdamOptions.Secao), c => c.BindNonPublicProperties = true);

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<ConfiguracaoProdamOptions>>().Value;

            var basicAuth = $"{options.Usuario}:{options.Senha}".EncodeTo64();
            services.AddHttpClient(name: "servicoAtualizacaoCadastralProdam", c =>
            {
                c.BaseAddress = new Uri(options.Url);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("Authorization", $"Basic {basicAuth}");
            }).AddPolicyHandler(GetRetryPolicy());
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
