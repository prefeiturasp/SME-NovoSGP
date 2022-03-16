using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.IoC;

namespace SME.SGP.TesteIntegracao
{
    public class RegistradorDependencias : RegistraDependencias
    {
        protected override void RegistrarRabbit(IServiceCollection services, ConfiguracaoRabbitOptions configRabbit)
        {
            //Não registra Rabbit
        }

        protected override void RegistrarContextos(IServiceCollection services)
        {
            //Não registra contextos originais
        }
    }
}
