using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresQueryHandler : IRequestHandler<ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresQuery, IEnumerable<DropdownTurmaRetornoDto>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<IEnumerable<DropdownTurmaRetornoDto>> Handle(ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolaresQuery request, CancellationToken cancellationToken)
        {
            var turmas = await repositorioAbrangencia.ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolares(request.AnoLetivo, request.CodigoUe, request.Modalidades, request.Semestre, request.Anos, request.Historico);

            return turmas.OrderBy(x => x.Modalidade.ShortName()).ThenBy(y => y.Descricao);
        }
           
    }
}
