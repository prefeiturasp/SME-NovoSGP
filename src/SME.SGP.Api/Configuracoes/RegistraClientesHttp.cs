using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao.Integracoes;
using System;

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
            services.AddHttpClient<IServicoEOL, ServicoEOL>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("UrlApiEOL").Value);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            services.AddHttpClient<IServicoAcompanhamentoEscolar, ServicoAcompanhamentoEscolar>(c =>
            {
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }
    }
}