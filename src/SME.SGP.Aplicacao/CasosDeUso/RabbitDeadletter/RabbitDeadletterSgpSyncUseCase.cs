using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class RabbitDeadletterSgpSyncUseCase : IRabbitDeadletterSgpSyncUseCase
    {
        private readonly IMediator mediator;

        public RabbitDeadletterSgpSyncUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar()
        {
            var parametrosSistema = await mediator.Send(
                new ObterParametroSistemaPorTipoEAnoQuery(
                    TipoParametroSistema.ProcessarDeadletter,
                    DateTime.Now.Year
                )
            );

            var processarDeadletter = parametrosSistema != null && Convert.ToBoolean(parametrosSistema.Valor); 
            
            if (processarDeadletter)
            {
                foreach (var fila in typeof(RotasRabbitSgp).ObterConstantesPublicas<string>())
                {
                    await mediator.Send(new PublicarFilaSgpCommand(
                            RotasRabbitSgp.RotaRabbitDeadletterTratar,
                            fila,
                            Guid.NewGuid(),
                            null,
                            false
                        )
                    );
                }    
            }
            
            return true;
        }
    }
}