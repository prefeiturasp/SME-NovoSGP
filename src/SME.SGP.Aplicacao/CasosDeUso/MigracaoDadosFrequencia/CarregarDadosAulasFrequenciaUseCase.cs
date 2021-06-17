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
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.SincronizarDadosFrequenciaMigracao, new FiltroMigracaoFrequenciaDto(anosLetivos), Guid.NewGuid(), null));

            return true;
        }
    }
}
