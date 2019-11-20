using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Infra.Interfaces;
using SME.SGP.IoC;
using System;
using System.Linq.Expressions;

namespace SME.SGP.Background
{
    public class Processor : SME.Background.Hangfire.Processor
    {
        readonly IContextoAplicacao contextoAplicacao;

        public Processor(IConfiguration configuration, string connectionString, IContextoAplicacao contextoAplicacao)
            : base(configuration, connectionString)
        {
            this.contextoAplicacao = contextoAplicacao;
        }

        public override string Executar<T>(Expression<Action<T>> metodo)
        {
            return base.Executar(metodo);
        }
        public string ExecutarComContexto<T>(Expression<Action<T>> metodo, IContextoAplicacao contexto)
        {
            var service = new ServiceCollection();
            RegistraDependenciasWorkerServices.Registrar(service);

            service.BuildServiceProvider().GetService<IContextoAplicacao>().AtribuirContexto(contexto);
        }
    }
}
