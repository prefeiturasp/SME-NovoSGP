using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaPorTurmaPeriodoQueryHandler : IRequestHandler<ObterFrequenciaPorTurmaPeriodoQuery, IEnumerable<FrequenciaAlunoDto>>
    {
        private readonly IRepositorioFrequenciaConsulta repositorio;

        public ObterFrequenciaPorTurmaPeriodoQueryHandler(IRepositorioFrequenciaConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<IEnumerable<FrequenciaAlunoDto>> Handle(ObterFrequenciaPorTurmaPeriodoQuery request, CancellationToken cancellationToken)
        {
            return this.repositorio.ObterFrequenciaPorTurmaPeriodo(request.TurmaCodigo, request.DataInicio, request.DataFim);
        }
    }
}
