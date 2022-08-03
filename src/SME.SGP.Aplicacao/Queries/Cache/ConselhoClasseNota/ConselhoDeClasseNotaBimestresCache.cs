using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConselhoDeClasseNotaBimestresCache
    {
        public long ConselhoClasseId { get; set; }

        public string CodigoAluno { get; set; }

        public int? Bimestre { get; set; }

        public ConselhoDeClasseNotaBimestresCache(long conselhoClasseId, string codigoAluno, int? bimestre)
        {
            ConselhoClasseId = conselhoClasseId;
            CodigoAluno = codigoAluno;
            Bimestre = bimestre;
        }
    }
}
