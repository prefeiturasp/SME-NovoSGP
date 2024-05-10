using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiarioBordoComAulaETurmaPorCodigoHandler : IRequestHandler<ObterDiarioBordoComAulaETurmaPorCodigoQuery, DiarioBordo>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public ObterDiarioBordoComAulaETurmaPorCodigoHandler(IRepositorioDiarioBordo repositorioDiarioBordo)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo;
        }

        public async Task<DiarioBordo> Handle(ObterDiarioBordoComAulaETurmaPorCodigoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioDiarioBordo.ObterDiarioBordoComAulaETurmaPorCodigo(request.DiarioBordoId);
        }
    }
}
