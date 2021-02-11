using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigoDREPorTurmaIdQueryHandler : IRequestHandler<ObterCodigoDREPorTurmaIdQuery, string>
    {
        private readonly IRepositorioDre repositorioDre;

        public ObterCodigoDREPorTurmaIdQueryHandler(IRepositorioDre repositorioDre)
        {
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
        }

        public async Task<string> Handle(ObterCodigoDREPorTurmaIdQuery request, CancellationToken cancellationToken)
            => await repositorioDre.ObterCodigoDREPorTurmaId(request.TurmaId);
    }
}
