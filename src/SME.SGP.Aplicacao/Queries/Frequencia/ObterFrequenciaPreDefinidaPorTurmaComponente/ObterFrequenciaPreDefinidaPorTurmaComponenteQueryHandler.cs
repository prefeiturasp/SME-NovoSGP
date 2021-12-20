using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaPreDefinidaPorTurmaComponenteQueryHandler : IRequestHandler<ObterFrequenciaPreDefinidaPorTurmaComponenteQuery, IEnumerable<FrequenciaPreDefinidaDto>>
    {
        private readonly IRepositorioFrequenciaPreDefinida repositorioFrequenciaPreDefinida;

        public ObterFrequenciaPreDefinidaPorTurmaComponenteQueryHandler(IRepositorioFrequenciaPreDefinida repositorioFrequenciaPreDefinida)
        {
            this.repositorioFrequenciaPreDefinida = repositorioFrequenciaPreDefinida ?? throw new ArgumentNullException(nameof(repositorioFrequenciaPreDefinida));
        }

        public async Task<IEnumerable<FrequenciaPreDefinidaDto>> Handle(ObterFrequenciaPreDefinidaPorTurmaComponenteQuery request, CancellationToken cancellationToken)
            => await repositorioFrequenciaPreDefinida.ObterPorTurmaEComponente(request.TurmaId, request.ComponenteCurricularId);
    }
}
