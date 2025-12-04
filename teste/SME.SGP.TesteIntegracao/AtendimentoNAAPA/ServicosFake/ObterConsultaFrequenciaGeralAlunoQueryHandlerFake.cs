using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConsultaFrequenciaGeralAlunoQueryHandlerFake : IRequestHandler<ObterConsultaFrequenciaGeralAlunoQuery, string>
    {
        private readonly IMediator mediator;
        public ObterConsultaFrequenciaGeralAlunoQueryHandlerFake(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<string> Handle(ObterConsultaFrequenciaGeralAlunoQuery request, CancellationToken cancellationToken)
        {
            return await Task.Run(() => "89%");
        }
    }
}
