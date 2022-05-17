using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Dtos.ConselhoClasse
{
    public class TotalAulasPorAlunoTurmaDto
    {
        public string DisciplinaId { get; set; }
        public string TotalAulas { get; set; }
        public string CodigoAluno { get; set; }
    }
}
