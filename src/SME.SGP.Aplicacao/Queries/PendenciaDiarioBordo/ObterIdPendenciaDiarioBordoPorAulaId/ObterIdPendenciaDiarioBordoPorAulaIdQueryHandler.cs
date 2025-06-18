using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdPendenciaDiarioBordoPorAulaIdQueryHandler : IRequestHandler<ObterIdPendenciaDiarioBordoPorAulaIdQuery, IEnumerable<PendenciaUsuarioDto>>
    {
        private readonly IRepositorioPendenciaDiarioBordoConsulta repositorioPendenciaDiarioBordoConsulta;

        public ObterIdPendenciaDiarioBordoPorAulaIdQueryHandler(IRepositorioPendenciaDiarioBordoConsulta repositorioPendenciaDiarioBordoConsulta)
        {
            this.repositorioPendenciaDiarioBordoConsulta = repositorioPendenciaDiarioBordoConsulta ?? throw new ArgumentNullException(nameof(repositorioPendenciaDiarioBordoConsulta));
        }

        public async Task<IEnumerable<PendenciaUsuarioDto>> Handle(ObterIdPendenciaDiarioBordoPorAulaIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPendenciaDiarioBordoConsulta.ObterIdPendenciaDiarioBordoPorAulaId(request.AulaId);
        }
    }
}
