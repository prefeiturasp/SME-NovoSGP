using MediatR;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestresLiberacaoBoletimUseCase : IObterBimestresLiberacaoBoletimUseCase
    {
        private readonly IMediator mediator;

        public ObterBimestresLiberacaoBoletimUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<int[]> Executar()
        {
            return await mediator.Send(new ObterBimestresEventoLiberacaoBoletimQuery(DateTime.Now));
        }
    }
}
