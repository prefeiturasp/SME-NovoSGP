using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresQueryHandler : IRequestHandler<ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresQuery, IEnumerable<OpcaoDropdownDto>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<IEnumerable<OpcaoDropdownDto>> Handle(ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresQuery request, CancellationToken cancellationToken)
            => await repositorioAbrangencia.ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolares(request.AnoLetivo, request.CodigoUe, request.Modalidades, request.Semestre, request.Anos);
    }
}
