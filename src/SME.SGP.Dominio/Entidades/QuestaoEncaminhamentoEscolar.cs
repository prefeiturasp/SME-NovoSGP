using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SME.SGP.Dominio.Entidades
{
    [ExcludeFromCodeCoverage]
    public class QuestaoEncaminhamentoEscolar : EntidadeBase
    {
        public QuestaoEncaminhamentoEscolar()
        {
            Respostas = new List<RespostaEncaminhamentoEscolar>();
        }

        public EncaminhamentoNAAPASecao EncaminhamentoEscolarSecao { get; set; }
        public long EncaminhamentoEscolarSecaoId { get; set; }
        public Questao Questao { get; set; }
        public long QuestaoId { get; set; }
        public bool Excluido { get; set; }

        public List<RespostaEncaminhamentoEscolar> Respostas { get; set; }
    }
}