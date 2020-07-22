using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System;
using System.Net;

namespace SME.SGP.Api
{
    public static class RegistraClientesHttp
    {
        public static void Registrar(IServiceCollection services, IConfiguration configuration)
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

            services.AddHttpClient(name: "servicoEOL", c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlApiEOL").Value);
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
            });
        }
    }
}