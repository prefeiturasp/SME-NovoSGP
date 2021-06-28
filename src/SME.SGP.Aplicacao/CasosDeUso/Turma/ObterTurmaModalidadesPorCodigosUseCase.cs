using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaModalidadesPorCodigosUseCase : IObterTurmaModalidadesPorCodigosUseCase
    {
        private readonly IMediator mediator;

        public ObterTurmaModalidadesPorCodigosUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<TurmaModalidadeCodigoDto>> Executar(string[] turmaCodigos)
        {
            return await mediator.Send(new ObterTurmaModalidadesPorCodigosQuery(turmaCodigos));
        }
    }
}
