using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasRegistradasPorTurmasComponentesCurricularesQueryHandler : IRequestHandler<ObterFrequenciasRegistradasPorTurmasComponentesCurricularesQuery, IEnumerable<RegistroFrequenciaAlunoBimestreDto>>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ObterFrequenciasRegistradasPorTurmasComponentesCurricularesQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
        }
        public async Task<IEnumerable<RegistroFrequenciaAlunoBimestreDto>> Handle(ObterFrequenciasRegistradasPorTurmasComponentesCurricularesQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterFrequenciasRegistradasPorTurmasComponentesCurriculares(request.CodigoAluno, request.CodigosTurma, request.ComponentesCurricularesId, request.PeriodoEscolarId);;
        }
    }
}
