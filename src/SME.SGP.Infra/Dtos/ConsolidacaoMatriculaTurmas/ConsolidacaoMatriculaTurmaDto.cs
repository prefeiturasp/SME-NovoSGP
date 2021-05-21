using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ConsolidacaoMatriculaTurmaDto
    {
        public ConsolidacaoMatriculaTurmaDto(string turmaCodigo, int quantidade)
        {
            TurmaCodigo = turmaCodigo;
            Quantidade = quantidade;
        }

        public long TurmaId { get; set; }
        public string TurmaCodigo { get; set; }
        public int Quantidade { get; set; }
    }
}
