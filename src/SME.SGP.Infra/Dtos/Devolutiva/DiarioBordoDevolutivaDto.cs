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
        public string Planejamento { get => $@"<b>Planejamento</b><br/>
                                    {DescricaoPlanejamento}<br/>
                                    <br/><b>Reflexões e replanejamento</b><br/>
                                    {DescricaoReflexoes}"; }
        public string PlanejamentoSimples { get => UtilRegex.RemoverTagsHtml(UtilRegex.RemoverTagsHtmlMidia(Planejamento)); }
    }
}
