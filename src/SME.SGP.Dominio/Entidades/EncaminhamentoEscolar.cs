#nullable enable
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio.Entidades
{
    [ExcludeFromCodeCoverage]
    public class EncaminhamentoEscolar : EntidadeBase
    {
        public EncaminhamentoEscolar()
        {
            Secoes = new List<EncaminhamentoNAAPASecao>();
            Situacao = SituacaoNovoEncaminhamentoNAAPA.EmAtendimento;
        }

        public Turma? Turma { get; set; }
        public long? TurmaId { get; set; }
        public string? AlunoCodigo { get; set; }
        public string? AlunoNome { get; set; }
        public Dre? Dre { get; set; }
        public long? DreId { get; set; }
        public Ue? Ue { get; set; }
        public long? UeId { get; set; }
        public int Tipo { get; set; }
        public SituacaoNovoEncaminhamentoNAAPA Situacao { get; set; }
        public bool Excluido { get; set; }
        public List<EncaminhamentoNAAPASecao> Secoes { get; set; }
        public SituacaoMatriculaAluno? SituacaoMatriculaAluno { get; set; }
        public string? MotivoEncerramento { get; set; }
        public DateTime? DataUltimaNotificacaoSemAtendimento { get; set; }

        public EncaminhamentoEscolar Clone()
        {
            return (EncaminhamentoEscolar)this.MemberwiseClone();
        }
    }
}