using SME.SGP.Infra.Utilitarios;
using System;

namespace SME.SGP.Infra
{
    public class DiarioBordoDevolutivaDto
    {
        public bool AulaCj { get; set; }
        public DateTime Data { get; set; }
        public string DescricaoPlanejamento { get; set; }
        public bool InseridoCJ { get; set; }
        public string Descricao { get; set; }
        public string Planejamento
        {
            get
            {
                var descricao = ObterPlanejamento();
                
                return descricao;
            }
        }

        public string PlanejamentoSimples
        {
            get
            {
                var descricao = ObterPlanejamento(false);

                return descricao;
            }
        }


        private string ObterPlanejamento(bool textoFormatado = true)
        {
            var descricao = textoFormatado ?
                DescricaoPlanejamento :
                UtilRegex.RemoverTagsHtml(UtilRegex.RemoverTagsHtmlMidia(DescricaoPlanejamento));
            return $"<b>Planejamento</b><br/>{descricao}<br/>";
        }
    }
}
