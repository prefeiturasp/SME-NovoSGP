using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Servicos;
using SME.SGP.Dados;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using SME.SGP.IoC;
using SME.SGP.TesteIntegracao.ServicosFakes;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.TesteIntegracao.Setup
{
    public class RegistradorDependencias : RegistrarDependencias
    {
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
