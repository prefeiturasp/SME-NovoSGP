using Dapper.Contrib.Extensions;
using System.Collections.Generic;
namespace SME.SGP.Dominio
{
    public class MapeamentoEstudante : EntidadeBase
    {
        public MapeamentoEstudante()
        {
            Secoes = new List<MapeamentoEstudanteSecao>();
        }
        [Computed]
        public Turma Turma { get; set; }
        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public bool Excluido { get; set; }
        [Computed]
        public List<MapeamentoEstudanteSecao> Secoes { get; set; }

        public MapeamentoEstudante Clone()
        {
            return (MapeamentoEstudante)this.MemberwiseClone();
        }

    }
}
