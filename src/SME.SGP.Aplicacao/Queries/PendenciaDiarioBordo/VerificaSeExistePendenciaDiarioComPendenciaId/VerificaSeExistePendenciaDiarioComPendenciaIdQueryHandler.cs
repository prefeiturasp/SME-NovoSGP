using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaSeExistePendenciaDiarioComPendenciaIdQueryHandler : IRequestHandler<VerificaSeExistePendenciaDiarioComPendenciaIdQuery, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPendenciaDiarioBordoConsulta repositorioPendenciaDiarioBordo;

        public VerificaSeExistePendenciaDiarioComPendenciaIdQueryHandler(IMediator mediator, IRepositorioPendenciaDiarioBordoConsulta repositorioPendenciaDiarioBordo)
        {
            this.mediator = mediator;
            this.repositorioPendenciaDiarioBordo = repositorioPendenciaDiarioBordo;
        }

        public async Task<bool> Handle(VerificaSeExistePendenciaDiarioComPendenciaIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPendenciaDiarioBordo.VerificarSeExistePendenciaDiarioComPendenciaId(request.PendenciaId);
        }
    }
}
