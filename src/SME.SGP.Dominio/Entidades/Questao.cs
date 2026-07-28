using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class Questao : EntidadeBase
    {
        public Questao()
        {
            OpcoesRespostas = new List<OpcaoResposta>();
        }

        [Computed]
        public Questionario Questionario { get; set; }
        public long QuestionarioId { get; set; }

        public int Ordem { get; set; }
        public string Nome { get; set; }
        public string Observacao { get; set; }
        public bool Obrigatorio { get; set; }
        public TipoQuestao Tipo { get; set; }
        public string Opcionais { get; set; }
        public bool SomenteLeitura { get; set; }
        public int Dimensao { get; set; }
        public int? Tamanho { get; set; }
        public string? Mascara { get; set; }
        public string? PlaceHolder { get; set; }
        public string? NomeComponente { get; set; }
        [Computed]
        public List<OpcaoResposta> OpcoesRespostas { get; set; }
    }
}
