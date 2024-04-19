using System.Collections.Generic;
namespace SME.SGP.Dominio
{
    public class MapeamentoEstudante : EntidadeBase
    {
        public MapeamentoEstudante()
        {
            Secoes = new List<MapeamentoEstudanteSecao>();
        }
        public Turma Turma { get; set; }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public int Bimestre { get; set; }
        public bool Excluido { get; set; }
        public List<MapeamentoEstudanteSecao> Secoes { get; set; }

        public MapeamentoEstudante Clone()
        {
            return (MapeamentoEstudante)this.MemberwiseClone();
        }

    }
}
