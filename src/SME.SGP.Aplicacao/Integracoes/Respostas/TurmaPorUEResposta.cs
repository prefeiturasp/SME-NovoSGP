using System;

namespace SME.SGP.Aplicacao.Integracoes.Respostas
{
    public class TurmaPorUEResposta
    {
        public string CodigoTurma { get; set; }
        public DateTime DataFimTurma { get; set; }
        public DateTime DataInicioTurma { get; set; }
        public string NomeTurma { get; set; }
        public string Situacao { get; set; }
        public string TipoTurma { get; set; }
    }
}