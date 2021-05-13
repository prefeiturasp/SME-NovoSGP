using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class ConsolidacaoMatriculaTurma
    {
        public ConsolidacaoMatriculaTurma(long id, long turmaId, int quantidade)
        {
            Id = id;
            TurmaId = turmaId;
            Quantidade = quantidade;
        }

        public long Id { get; set; }
        public long TurmaId { get; set; }
        public int Quantidade { get; set; }
    }
}
