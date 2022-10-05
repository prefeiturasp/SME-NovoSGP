using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinaBimestreQueryHandler : IRequestHandler<ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinaBimestreQuery, IEnumerable<FechamentoTurmaDisciplina>>
    {
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina;

        public ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinaBimestreQueryHandler(IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
        }

        public async Task<IEnumerable<FechamentoTurmaDisciplina>> Handle(ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinaBimestreQuery request, CancellationToken cancellationToken)
            => await repositorioFechamentoTurmaDisciplina.ObterFechamentoTurmaDisciplinaPorTurmaidDisciplinaId(request.TurmaCodigo, request.DisciplinaId, request?.Bimestre);

    }
}