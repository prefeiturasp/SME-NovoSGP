using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dados;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.IoC;
using SME.SGP.IoC.Extensions;
using SME.SGP.TesteIntegracao.ServicosFakes;

namespace SME.SGP.TesteIntegracao.Setup
{
    public class RegistradorDependencias : RegistraDependencias
    {
        public override void RegistrarRabbit(IServiceCollection services, ConfiguracaoRabbitOptions configRabbit)
        {
            //Não registra Rabbit
        }

        protected override void RegistrarContextos(IServiceCollection services)
        {
            services.TryAddScoped<IHttpContextAccessor, HttpContextAccessorFake>();
            services.TryAddScoped<IContextoAplicacao, ContextoHttp>();
            services.TryAddScoped<ISgpContext, SgpContext>();
            services.TryAddScoped<ISgpContextConsultas, SgpContextConsultas>();
            services.TryAddScoped<IUnitOfWork, UnitOfWork>();
            services.AddPolicies();
        }

        protected override void RegistrarServicos(IServiceCollection services)
        {
            services.TryAddScoped<IServicoTelemetria, TelemetriaFake>();
            services.TryAddScoped<IServicoEol, ServicoEOLFake>();
            base.RegistrarServicos(services);
        }
    }
}
