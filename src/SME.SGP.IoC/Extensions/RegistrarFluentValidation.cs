using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SME.SGP.IoC
{
    public static class RegistrarFluentValidation
    {
        public static void AdicionarValidadoresFluentValidation(this IServiceCollection services)
        {
            var assemblyInfra = AppDomain.CurrentDomain.Load("SME.SGP.Infra");

            AssemblyScanner
                .FindValidatorsInAssembly(assemblyInfra)
                .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));

            var assembly = AppDomain.CurrentDomain.Load("SME.SGP.Aplicacao");

            AssemblyScanner
                .FindValidatorsInAssembly(assembly)
                .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));
        }
    }
}