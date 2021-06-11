using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigoDREPorUeIdQueryHandler : IRequestHandler<ObterCodigoDREPorUeIdQuery, string>
    {
        private readonly IRepositorioDre repositorioDre;

        public ObterCodigoDREPorUeIdQueryHandler(IRepositorioDre repositorioDre)
        {
            this.repositorioDre = repositorioDre;
        }

        public async Task<string> Handle(ObterCodigoDREPorUeIdQuery request, CancellationToken cancellationToken)
            => await repositorioDre.ObterCodigoDREPorUEId(request.UeId);
    }
}
