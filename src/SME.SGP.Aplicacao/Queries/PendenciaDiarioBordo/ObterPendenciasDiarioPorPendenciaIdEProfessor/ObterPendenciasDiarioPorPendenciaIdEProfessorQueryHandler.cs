using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao 
{ 
    public class ObterPendenciasDiarioPorPendenciaIdEProfessorQueryHandler : IRequestHandler<ObterPendenciasDiarioPorPendenciaIdEProfessorQuery, IEnumerable<PendenciaDiarioBordoDescricaoDto>>
    {
        private readonly IRepositorioPendenciaDiarioBordoConsulta repositorioPendenciaDiarioBordo;

        public ObterPendenciasDiarioPorPendenciaIdEProfessorQueryHandler(IRepositorioPendenciaDiarioBordoConsulta repositorioPendenciaDiarioBordo)
        {
            this.repositorioPendenciaDiarioBordo = repositorioPendenciaDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioPendenciaDiarioBordo));
        }

        public async Task<IEnumerable<PendenciaDiarioBordoDescricaoDto>> Handle(ObterPendenciasDiarioPorPendenciaIdEProfessorQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaDiarioBordo.ObterPendenciasDiarioPorPendencia(request.PendenciaId, request.CodigoRf);
    }
}
