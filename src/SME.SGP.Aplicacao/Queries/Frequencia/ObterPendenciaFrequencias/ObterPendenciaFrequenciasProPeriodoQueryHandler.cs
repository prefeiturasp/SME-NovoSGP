using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaFrequenciasProPeriodoQueryHandler : IRequestHandler<ObterPendenciaFrequenciasProPeriodoQuery, bool>
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;

        public ObterPendenciaFrequenciasProPeriodoQueryHandler(IRepositorioFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }

        public async Task<bool> Handle(ObterPendenciaFrequenciasProPeriodoQuery request, CancellationToken cancellationToken)
            => await repositorioFrequencia.ObterPendenciaFrequencias(request.TurmaCodigo, request.ComponenteCurricularId, request.DataLimite, request.AnoLetivo, request.Bimestre);
    }
}
