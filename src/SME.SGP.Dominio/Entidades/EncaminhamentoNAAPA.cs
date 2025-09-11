using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class EncaminhamentoNAAPA : EntidadeBase
    {
        public EncaminhamentoNAAPA()
        {
            Secoes = new List<EncaminhamentoNAAPASecao>();
            Situacao = SituacaoNAAPA.Rascunho;
        }
        public Turma Turma { get; set; }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public SituacaoNAAPA Situacao { get; set; }
        public bool Excluido { get; set; }
        public List<EncaminhamentoNAAPASecao> Secoes { get; set; }
        public SituacaoMatriculaAluno? SituacaoMatriculaAluno { get; set; }
        public string MotivoEncerramento { get; set; }
        public DateTime? DataUltimaNotificacaoSemAtendimento { get; set; }

        public EncaminhamentoNAAPA Clone()
        {
            return (EncaminhamentoNAAPA)this.MemberwiseClone();
        }

    }
}
