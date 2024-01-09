using SME.SGP.Infra.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Infra
{
    public class DiarioBordoDevolutivaDto
    {
        public DiarioBordoDevolutivaDto()
        {
            ComponentesDescricoesPlanejamento = new List<(string, string)>();
        }

        public bool AulaCj { get; set; }
        public DateTime Data { get; set; }
        public string DescricaoPlanejamento { get; set; }
        public bool InseridoCJ { get; set; }
        public string Descricao { get; set; }  
        public string Componente { get; set; }
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

        public void AdicionarDescricao(string componente, string planejamento)
        {
            ComponentesDescricoesPlanejamento.Add((componente, planejamento));
        }

        private List<(string componente,string planejamento)> ComponentesDescricoesPlanejamento { get; set; }

        private string ObterPlanejamento(bool textoFormatado = true)
        {
            if (ComponentesDescricoesPlanejamento.Any())
                return ObterPlanejamentoDescricoesComponentes(textoFormatado);

            return ObterPlanejamentoFormatado(textoFormatado, DescricaoPlanejamento, string.Empty);
        }

        private string ObterPlanejamentoDescricoesComponentes(bool textoFormatado)
        {
            var descricao = new StringBuilder();

            foreach (var item in ComponentesDescricoesPlanejamento)
            {
                descricao.AppendLine(ObterPlanejamentoFormatado(textoFormatado, item.planejamento, item.componente));
            }

            return descricao.ToString();    
        }

        private string ObterPlanejamentoFormatado(bool textoFormatado, string descricaoPlanejamento, string componente)
        {
            var descricao = textoFormatado ?
                descricaoPlanejamento :
                UtilRegex.RemoverTagsHtml(UtilRegex.RemoverTagsHtmlMidia(descricaoPlanejamento));
            var descricaoComponente = string.IsNullOrEmpty(componente) ? string.Empty : $"{componente} - ";

            return $"<b>{descricaoComponente}Planejamento</b><br/>{descricao}<br/>";
        }
    }
}
