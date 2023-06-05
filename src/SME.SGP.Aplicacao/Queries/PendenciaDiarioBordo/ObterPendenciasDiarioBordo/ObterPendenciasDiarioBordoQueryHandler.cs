using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasDiarioBordoQueryHandler : IRequestHandler<ObterPendenciasDiarioBordoQuery, IEnumerable<AulaComComponenteDto>>
    {
        private readonly IRepositorioPendenciaDiarioBordoConsulta repositorioPendenciaDiarioBordo;

        public ObterPendenciasDiarioBordoQueryHandler(IRepositorioPendenciaDiarioBordoConsulta repositorioPendenciaDiarioBordo)
        {
            this.repositorioPendenciaDiarioBordo = repositorioPendenciaDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioPendenciaDiarioBordo));
        }

        public async Task<IEnumerable<AulaComComponenteDto>> Handle(ObterPendenciasDiarioBordoQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaDiarioBordo.ListarPendenciasDiario(request.TurmaId, request.ComponentesCurricularesId);
    }
}
