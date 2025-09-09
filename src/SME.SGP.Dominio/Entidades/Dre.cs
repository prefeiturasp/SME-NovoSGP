using System;

namespace SME.SGP.Dominio
{
    public class Dre
    {
        public string Abreviacao { get; set; }
        public string CodigoDre { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public long Id { get; set; }
        public string Nome { get; set; }

        public string PrefixoDoNomeAbreviado
        {
            get
            {
                string novaSigla = "DRE";
                string textoParaSubstituir = "DIRETORIA REGIONAL DE EDUCACAO";
                var nomeFormatado = Nome.ToUpper().Replace(textoParaSubstituir, novaSigla).Trim();
                return nomeFormatado;
            }
            private set { }
        }
    }
}