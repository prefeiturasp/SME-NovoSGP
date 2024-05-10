using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaSeExistePendenciaDiarioComPendenciaIdQueryHandler : IRequestHandler<VerificaSeExistePendenciaDiarioComPendenciaIdQuery, bool>
    {
        private readonly IRepositorioPendenciaDiarioBordo repositorioPendenciaDiarioBordo;

        public VerificaSeExistePendenciaDiarioComPendenciaIdQueryHandler(IRepositorioPendenciaDiarioBordo repositorioPendenciaDiarioBordo)
        {
            this.repositorioPendenciaDiarioBordo = repositorioPendenciaDiarioBordo;
        }

        public async Task<bool> Handle(VerificaSeExistePendenciaDiarioComPendenciaIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPendenciaDiarioBordo.VerificarSeExistePendenciaDiarioComPendenciaId(request.PendenciaId);
        }
    }
}
