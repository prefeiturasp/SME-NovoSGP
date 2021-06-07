using MediatR;
using Sentry;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CarregarDadosAulasFrequenciaUseCase : AbstractUseCase, ICarregarDadosAulasFrequenciaUseCase
    {
        public CarregarDadosAulasFrequenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(int[] anosLetivos)
        {
            var dadosAulas = await mediator.Send(new ObterTurmasIdFrequenciasExistentesPorAnosLetivosQuery(anosLetivos));
            try
            {
                foreach (var aula in dadosAulas)
                {
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.SincronizarDadosAulasFrequenciaMigracao, aula, Guid.NewGuid(), null));
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
            return true;
        }
    }
}
