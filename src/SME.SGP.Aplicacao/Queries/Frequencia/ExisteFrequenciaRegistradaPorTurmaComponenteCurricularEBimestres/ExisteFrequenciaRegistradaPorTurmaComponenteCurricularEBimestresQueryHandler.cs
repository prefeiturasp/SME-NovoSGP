using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExisteFrequenciaRegistradaPorTurmaComponenteCurricularEBimestresQueryHandler : IRequestHandler<ExisteFrequenciaRegistradaPorTurmaComponenteCurricularEBimestresQuery, bool>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ExisteFrequenciaRegistradaPorTurmaComponenteCurricularEBimestresQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
        }

        public async Task<bool> Handle(ExisteFrequenciaRegistradaPorTurmaComponenteCurricularEBimestresQuery request, CancellationToken cancellationToken)
            => await repositorioFrequenciaAlunoDisciplinaPeriodo.ExisteFrequenciaRegistradaPorTurmaComponenteCurricularEBimestres(request.CodigoTurma, request.ComponenteCurricularId, request.PeriodosEscolaresIds);
    }
}
