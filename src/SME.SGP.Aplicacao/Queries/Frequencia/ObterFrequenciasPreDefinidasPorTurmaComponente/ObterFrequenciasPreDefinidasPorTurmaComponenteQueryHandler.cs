using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasPreDefinidasPorTurmaComponenteQueryHandler : IRequestHandler<ObterFrequenciasPreDefinidasPorTurmaComponenteQuery, IEnumerable<FrequenciaPreDefinida>>
    {
        private readonly IRepositorioFrequenciaPreDefinidaConsulta repositorioFrequenciaPreDefinida;

        public ObterFrequenciasPreDefinidasPorTurmaComponenteQueryHandler(IRepositorioFrequenciaPreDefinidaConsulta repositorioFrequenciaPreDefinida)
        {
            this.repositorioFrequenciaPreDefinida = repositorioFrequenciaPreDefinida ?? throw new ArgumentNullException(nameof(repositorioFrequenciaPreDefinida));
        }

        public async Task<IEnumerable<FrequenciaPreDefinida>> Handle(ObterFrequenciasPreDefinidasPorTurmaComponenteQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFrequenciaPreDefinida.ObterListaFrequenciaPreDefinida(request.TurmaId, request.ComponenteCurricularId);
        }
    }
}
