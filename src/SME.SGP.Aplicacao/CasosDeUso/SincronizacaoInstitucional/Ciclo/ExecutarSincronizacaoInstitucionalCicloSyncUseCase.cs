using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
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
                try
                {
                    var publicarTratamentoCiclo = await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.SincronizaEstruturaInstitucionalCicloTratar, ciclo, param.CodigoCorrelacao, null));
                    if (!publicarTratamentoCiclo)
                    {
                        var mensagem = $"Não foi possível inserir o ciclo : {publicarTratamentoCiclo} na fila de sync.";
                        SentrySdk.CaptureMessage(mensagem);
                    }
                }
                catch (Exception ex)
                {
                    SentrySdk.CaptureException(ex);
                }
            }

            return true;

        }
    }
}
