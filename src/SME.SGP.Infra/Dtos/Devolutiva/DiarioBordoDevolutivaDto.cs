using SME.SGP.Infra.Utilitarios;
using System;

namespace SME.SGP.Infra
{
    public class DiarioBordoDevolutivaDto
    {
        public bool AulaCj { get; set; }
        public DateTime Data { get; set; }
        public string DescricaoPlanejamento { get; set; }
        public string DescricaoReflexoes { get; set; }
        public string Descricao { get; set; }
        public string Planejamento
        {
            get
            {
                var descricao = ObterPlanejamento();
                if (!string.IsNullOrEmpty(DescricaoReflexoes))
                    descricao += ObterReflexoes();

                return descricao;
            }
        }

        public string PlanejamentoSimples
        {
            get
            {
                var descricao = ObterPlanejamento(false);
                if (!string.IsNullOrEmpty(DescricaoReflexoes))
                    descricao += ObterReflexoes(false);

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

        private string ObterReflexoes(bool textoFormatado = true)
        {
            var descricao = textoFormatado ?
                DescricaoReflexoes :
                UtilRegex.RemoverTagsHtml(UtilRegex.RemoverTagsHtmlMidia(DescricaoReflexoes));
            return $"<br/><b>Reflexões e replanejamento</b><br/>{descricao}";
        }
    }
}
