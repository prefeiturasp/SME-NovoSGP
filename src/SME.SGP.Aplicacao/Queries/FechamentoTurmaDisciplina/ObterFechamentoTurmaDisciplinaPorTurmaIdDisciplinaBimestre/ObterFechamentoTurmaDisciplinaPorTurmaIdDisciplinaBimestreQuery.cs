using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinaBimestreQuery : IRequest<IEnumerable<FechamentoTurmaDisciplina>>
    {
        public ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinaBimestreQuery(string turmaCodigo, long disciplinaId, int? bimestre = 0)
        {
            TurmaCodigo = turmaCodigo;
            DisciplinaId = disciplinaId;
            Bimestre = bimestre;
        }

        public string TurmaCodigo { get; set; }
        public long DisciplinaId { get; set; }
        public int? Bimestre { get; set; }
    }
}