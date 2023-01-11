﻿using System;

namespace SME.SGP.Dominio
{
    public class WFAprovacaoParecerConclusivo : EntidadeBase
    {
        public WFAprovacaoParecerConclusivo()
        {}

        public long? WfAprovacaoId { get; set; }
        public WorkflowAprovacao WfAprovacao { get; set; }
        public long ConselhoClasseAlunoId { get; set; }
        public ConselhoClasseAluno ConselhoClasseAluno { get; set; }
        public long UsuarioSolicitanteId { get; set; }

        public long? ConselhoClasseParecerId { get; set; }
        public ConselhoClasseParecerConclusivo ConselhoClasseParecer { get; set; }
    }
}
