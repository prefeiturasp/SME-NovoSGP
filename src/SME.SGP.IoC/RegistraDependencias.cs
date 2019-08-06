using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dados.Mapeamentos;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;

namespace SME.SGP.IoC
{
    public static class RegistraDependencias
    {
        public static void Registrar(IServiceCollection services)
        {
            RegistrarRepositorios(services);
            RegistrarContextos(services);
            RegistrarCasosDeUso(services);
        }

        private static void RegistrarCasosDeUso(IServiceCollection services)
        {
            services.TryAddScoped<IManterAluno, ManterAluno>();
            services.TryAddScoped<IManterPlanoCiclo, ManterPlanoCiclo>();
        }

        private static void RegistrarContextos(IServiceCollection services)
        {
            services.TryAddScoped<SgpContext, DbContext>();
            RegistrarMapeamentos.Registrar();
        }

        private static void RegistrarRepositorios(IServiceCollection services)
        {
            services.TryAddScoped<IRepositorioAluno, RepositorioAluno>();
            services.TryAddScoped<IRepositorioProfessor, RepositorioProfessor>();
            services.TryAddScoped<IRepositorioPlanoCiclo, RepositorioPlanoCiclo>();
            services.TryAddScoped<IRepositorioMatrizSaberPlano, RepositorioMatrizSaberPlano>();
        }
    }
}