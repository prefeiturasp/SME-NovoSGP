using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigoDREPorUeIdQueryHandler : IRequestHandler<ObterCodigoDREPorUeIdQuery, string>
    {
        private readonly IRepositorioDreConsulta repositorioDre;

        public ObterCodigoDREPorUeIdQueryHandler(IRepositorioDreConsulta repositorioDre)
        {
            this.repositorioDre = repositorioDre;
        }

        public async Task<string> Handle(ObterCodigoDREPorUeIdQuery request, CancellationToken cancellationToken)
            => await repositorioDre.ObterCodigoDREPorUEId(request.UeId);
    }
}
