using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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