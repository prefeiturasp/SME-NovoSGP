using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigoComponentePaiQueryHandler : IRequestHandler<ObterCodigoComponentePaiQuery, string>
    {
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;

        public ObterCodigoComponentePaiQueryHandler(IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
        }
        public async Task<string> Handle(ObterCodigoComponentePaiQuery request, CancellationToken cancellationToken)
        => await repositorioComponenteCurricular.ObterCodigoComponentePai(request.ComponenteCurricularId);
    }
}
