﻿using System;

namespace SME.SGP.Dominio
{
    public class FechamentoConsolidadoComponenteTurma : EntidadeBase
    {
        public DateTime DataAtualizacao { get; set; }

        public SituacaoFechamento Status { get; set; }

        public long ComponenteCurricularCodigo { get; set; }

        public string ProfessorRf { get; set; }

        public string ProfessorNome { get; set; }

        public long TurmaId { get; set; }

        public int? Bimestre { get; set; }
    }
}
