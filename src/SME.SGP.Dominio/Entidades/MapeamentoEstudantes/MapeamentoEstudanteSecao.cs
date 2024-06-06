using System.Collections.Generic;
namespace SME.SGP.Dominio
{
    public class MapeamentoEstudanteSecao : EntidadeBase
    {
        public MapeamentoEstudanteSecao()
        {
            Questoes = new List<QuestaoMapeamentoEstudante>();
        }

        public MapeamentoEstudante MapeamentoEstudante { get; set; }
        public long MapeamentoEstudanteId { get; set; }

        public SecaoMapeamentoEstudante SecaoMapeamentoEstudante { get; set; }
        public long SecaoMapeamentoEstudanteId { get; set; }

        public bool Concluido { get; set; }
        public bool Excluido { get; set; }

        public List<QuestaoMapeamentoEstudante> Questoes { get; set; }
    }
}
