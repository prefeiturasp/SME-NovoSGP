using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosFrequenciaAlunosPorPeriodoQueryHandler : IRequestHandler<ObterRegistrosFrequenciaAlunosPorPeriodoQuery, IEnumerable<RegistroFrequenciaAlunoPorAulaDto>>
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;

        public ObterRegistrosFrequenciaAlunosPorPeriodoQueryHandler(IRepositorioFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }

        public async Task<IEnumerable<RegistroFrequenciaAlunoPorAulaDto>> Handle(ObterRegistrosFrequenciaAlunosPorPeriodoQuery request, CancellationToken cancellationToken)
            => await repositorioFrequencia.ObterFrequenciasDetalhadasPorData(request.TurmaCodigo, request.ComponenteCurricularId, request.AlunosCodigos, request.DataInicio, request.DataFim);
    }
}
