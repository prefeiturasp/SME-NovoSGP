using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalCicloSyncUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalCicloSyncUseCase
    {
        public ExecutarSincronizacaoInstitucionalCicloSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {

            var ciclos = await mediator.Send(new ObterCiclosEolQuery());

            if (ciclos == null || !ciclos.Any())
            {
                throw new NegocioException("Não foi possível localizar tipos de ciclos no EOL para a sincronização instituicional");
            }

            foreach (var ciclo in ciclos)
            {

                var publicarTratamentoCiclo = await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.SincronizaEstruturaInstitucionalCicloTratar, ciclo, param.CodigoCorrelacao, null));
                if (!publicarTratamentoCiclo)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível inserir o ciclo : {publicarTratamentoCiclo} na fila de sync.", LogNivel.Negocio, LogContexto.SincronizacaoInstitucional));
                }

            }
            return true;
        }
    }
}
