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
    public class ObterIdPendenciaDiarioBordoPorAulaEComponenteIdQueryHandler : IRequestHandler<ObterIdPendenciaDiarioBordoPorAulaEComponenteIdQuery, long>
    {
        private readonly IRepositorioPendenciaDiarioBordoConsulta repositorioPendenciaDiarioBordoConsulta;

        public ObterIdPendenciaDiarioBordoPorAulaEComponenteIdQueryHandler(IRepositorioPendenciaDiarioBordoConsulta repositorioPendenciaDiarioBordoConsulta)
        {
            this.repositorioPendenciaDiarioBordoConsulta = repositorioPendenciaDiarioBordoConsulta ?? throw new ArgumentNullException(nameof(repositorioPendenciaDiarioBordoConsulta));
        }

        public async Task<long> Handle(ObterIdPendenciaDiarioBordoPorAulaEComponenteIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPendenciaDiarioBordoConsulta.ExisteIdPendenciaDiarioBordo(request.AulaId, request.ComponenteCurricularId);

        }
    }
}
