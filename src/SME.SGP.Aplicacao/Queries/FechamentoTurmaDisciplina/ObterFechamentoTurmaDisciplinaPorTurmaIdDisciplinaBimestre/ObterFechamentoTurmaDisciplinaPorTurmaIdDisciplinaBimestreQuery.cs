using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinaBimestreQuery : IRequest<FechamentoTurmaDisciplina>
    {
        public ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinaBimestreQuery(string turmaCodigo, long disciplinaId)
        {
            TurmaCodigo = turmaCodigo;
            DisciplinaId = disciplinaId;
        }

        public string TurmaCodigo { get; set; }
        public long DisciplinaId { get; set; }
    }
}
