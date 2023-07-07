using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public class ConsolidadoConselhoClasseAlunoNotaCacheDto
    {
        public string AlunoCodigo { get; set; }
        public int Bimestre { get; set; }
        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public double? NotaFechamento { get; set; }
        public long? ConceitoIdFechamento { get; set; }
        public double? NotaConselhoClasse { get; set; }
        public long? ConceitoIdConselhoClasse { get; set; }
    }
}
