using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaGeralPorTurmaQueryHandler : IRequestHandler<ObterFrequenciaGeralPorTurmaQuery, IEnumerable<FrequenciaAlunoDto>>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorio;

        public ObterFrequenciaGeralPorTurmaQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<FrequenciaAlunoDto>> Handle(ObterFrequenciaGeralPorTurmaQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterFrequenciaGeralPorTurma(request.TurmaCodigo);
    }
}
