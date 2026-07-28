using Dapper.Contrib.Extensions;
using System.Collections.Generic;
namespace SME.SGP.Dominio
{
    public class MapeamentoEstudanteSecao : EntidadeBase
    {
        public MapeamentoEstudanteSecao()
        {
            Questoes = new List<QuestaoMapeamentoEstudante>();
        }

        [Computed]
        public MapeamentoEstudante MapeamentoEstudante { get; set; }
        public long MapeamentoEstudanteId { get; set; }

        [Computed]
        public SecaoMapeamentoEstudante SecaoMapeamentoEstudante { get; set; }
        public long SecaoMapeamentoEstudanteId { get; set; }

        public bool Concluido { get; set; }
        public bool Excluido { get; set; }

        [Computed]
        public List<QuestaoMapeamentoEstudante> Questoes { get; set; }
    }
}
