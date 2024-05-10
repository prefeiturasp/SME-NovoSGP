using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDisciplinasIdFechamentoPorTurmaIdEBimestreQueryHandler : IRequestHandler<ObterDisciplinasIdFechamentoPorTurmaIdEBimestreQuery, IEnumerable<int>>
    {
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina;

        public ObterDisciplinasIdFechamentoPorTurmaIdEBimestreQueryHandler(IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
        }

        public async Task<IEnumerable<int>> Handle(ObterDisciplinasIdFechamentoPorTurmaIdEBimestreQuery request, CancellationToken cancellationToken)
            => await repositorioFechamentoTurmaDisciplina.ObterDisciplinaIdsPorTurmaIdBimestre(request.TurmaId, request.Bimestre);
    }
}
