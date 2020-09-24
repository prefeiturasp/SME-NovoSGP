using System;

namespace SME.SGP.Dominio
{
    public class RecuperacaoParalelaResposta : EntidadeBase
    {
        public string Descricao { get; set; }
        public DateTime DtFim { get; set; }
        public DateTime DtInicio { get; set; }
        public bool Excluido { get; set; }
        public string Nome { get; set; }
        public bool Sim { get; set; }
    }
}