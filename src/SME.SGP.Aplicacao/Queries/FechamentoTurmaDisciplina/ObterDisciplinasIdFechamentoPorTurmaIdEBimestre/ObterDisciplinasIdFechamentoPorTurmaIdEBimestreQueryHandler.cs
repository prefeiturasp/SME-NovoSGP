using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDisciplinasIdFechamentoPorTurmaIdEBimestreQueryHandler : IRequestHandler<ObterDisciplinasIdFechamentoPorTurmaIdEBimestreQuery, IEnumerable<int>>
    {
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;

        public ObterDisciplinasIdFechamentoPorTurmaIdEBimestreQueryHandler(IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
        }

        public async Task<IEnumerable<int>> Handle(ObterDisciplinasIdFechamentoPorTurmaIdEBimestreQuery request, CancellationToken cancellationToken)
            => await repositorioFechamentoTurmaDisciplina.ObterDisciplinaIdsPorTurmaIdBimestre(request.TurmaId, request.Bimestre);
    }
}
