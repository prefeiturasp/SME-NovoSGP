using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasDiarioBordoQueryHandler : IRequestHandler<ObterPendenciasDiarioBordoQuery, IEnumerable<AulaComComponenteDto>>
    {
        private readonly IRepositorioPendenciaDiarioBordo repositorioPendenciaDiarioBordo;

        public ObterPendenciasDiarioBordoQueryHandler(IRepositorioPendenciaDiarioBordo repositorioPendenciaDiarioBordo)
        {
            this.repositorioPendenciaDiarioBordo = repositorioPendenciaDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioPendenciaDiarioBordo));
        }

        public async Task<IEnumerable<AulaComComponenteDto>> Handle(ObterPendenciasDiarioBordoQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaDiarioBordo.ListarPendenciasDiario(request.TurmaId, request.ComponentesCurricularesId);
    }
}
