using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class ConselhoClasseAlunoRecomendacao 
    {
        public long Id { get; set; }
        public long ConselhoClasseAlunoId { get; set; }
        public ConselhoClasseAluno ConselhoClasseAluno { get; set; }
        public long ConselhoClasseRecomendacaoId { get; set; }
        public ConselhoClasseRecomendacao ConselhoClasseRecomendacao { get; set; }
    }
}
