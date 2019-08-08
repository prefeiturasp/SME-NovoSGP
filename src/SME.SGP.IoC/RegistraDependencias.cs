using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dados;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.IoC
{
    public static class RegistraDependencias
    {
        public static void Registrar(IServiceCollection services)
        {
            RegistrarRepositorios(services);
            RegistrarContextos(services);
            RegistrarComandos(services);
            RegistrarConsultas(services);
        }

        private static void RegistrarComandos(IServiceCollection services)
        {
            services.TryAddScoped<IComandosPlanoCiclo, ComandosPlanoCiclo>();
        }

        private static void RegistrarConsultas(IServiceCollection services)
        {
            services.TryAddScoped<IConsultasPlanoCiclo, ConsultasPlanoCiclo>();
        }

        private static void RegistrarContextos(IServiceCollection services)
        {
            services.TryAddScoped<ISgpContext, SgpContext>();
            services.TryAddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static void RegistrarRepositorios(IServiceCollection services)
        {
            services.TryAddScoped<IRepositorioPlanoCiclo, RepositorioPlanoCiclo>();
            services.TryAddScoped<IRepositorioMatrizSaberPlano, RepositorioMatrizSaberPlano>();
            services.TryAddScoped<IRepositorioObjetivoDesenvolvimentoPlano, RepositorioObjetivoDesenvolvimentoPlano>();
        }
    }
}