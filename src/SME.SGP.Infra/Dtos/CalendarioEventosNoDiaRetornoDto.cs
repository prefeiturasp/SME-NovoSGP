using System;

namespace SME.SGP.Infra
{
    public class CalendarioEventosNoDiaRetornoDto
    {
        public long Id { get; set; }
        public string InicioFimDesc { get; set; }
        public string Nome { get { return $"{_nome} { InicioFimDesc.Replace("inicio", "início") }"; } set { _nome = value; } }
        public string TipoEvento { get; set; }
        public string DreNome { get; set; }
        public string UeNome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        private string _nome { get; set; }
        
    }
}