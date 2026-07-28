using Dapper.Contrib.Extensions;
using System.Collections.Generic;
namespace SME.SGP.Dominio
{
    public class QuestaoMapeamentoEstudante : EntidadeBase
    {
        public QuestaoMapeamentoEstudante()
        {
            Respostas = new List<RespostaMapeamentoEstudante>();
        }

        public MapeamentoEstudanteSecao MapeamentoEstudanteSecao { get; set; }
        public long MapeamentoEstudanteSecaoId { get; set; }

        [Computed]
        public Questao Questao { get; set; }
        public long QuestaoId { get; set; }

        public bool Excluido { get; set; }

        [Computed]
        public List<RespostaMapeamentoEstudante> Respostas { get; set; }
    }
}
