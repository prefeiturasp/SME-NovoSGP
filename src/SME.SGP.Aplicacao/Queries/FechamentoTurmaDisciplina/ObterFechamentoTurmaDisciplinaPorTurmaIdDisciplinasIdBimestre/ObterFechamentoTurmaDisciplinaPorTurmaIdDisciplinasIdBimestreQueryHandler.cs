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
    public class ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinasIdBimestreQueryHandler : IRequestHandler<ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinasIdBimestreQuery, IEnumerable<FechamentoTurmaDisciplina>>
    {
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina;

        public ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinasIdBimestreQueryHandler(IRepositorioFechamentoTurmaDisciplinaConsulta repositorio)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorio;
        }
        public async  Task<IEnumerable<FechamentoTurmaDisciplina>> Handle(ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinasIdBimestreQuery request, CancellationToken cancellationToken)
            => await repositorioFechamentoTurmaDisciplina.ObterFechamentosTurmaDisciplinas(request.TurmaId, request.DisciplinasId, request.Bimestre, request.TipoCalendario);
    }
}
