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
    public class ObterFrequenciasPreDefinidasPorAlunoETurmaQueryHandler : IRequestHandler<ObterFrequenciasPreDefinidasPorAlunoETurmaQuery, IEnumerable<FrequenciaPreDefinidaDto>>
    {
        private readonly IRepositorioFrequenciaPreDefinida repositorioFrequenciaPreDefinida;

        public ObterFrequenciasPreDefinidasPorAlunoETurmaQueryHandler(IRepositorioFrequenciaPreDefinida repositorioFrequenciaPreDefinida)
        {
            this.repositorioFrequenciaPreDefinida = repositorioFrequenciaPreDefinida ?? throw new ArgumentNullException(nameof(repositorioFrequenciaPreDefinida));
        }

        public async Task<IEnumerable<FrequenciaPreDefinidaDto>> Handle(ObterFrequenciasPreDefinidasPorAlunoETurmaQuery request, CancellationToken cancellationToken)
        {
            var frequenciasPreDefinidas = await repositorioFrequenciaPreDefinida.Listar(request.TurmaId, request.ComponenteCurricularId, request.AlunoCodigo);

            return frequenciasPreDefinidas;
        }
    }
}
