using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class PainelEducacionalIdebAgrupamentoDto
    {
        public int AnoSolicitado { get; set; }
        public int AnoUtilizado { get; set; }
        public bool AnoSolicitadoSemDados { get; set; }
        public string Serie { get; set; }
        public double MediaGeral { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public List<FaixaQuantidadeIdeb> Distribuicao { get; set; }
    }
    public class FaixaQuantidadeIdeb
    {
        public string Faixa { get; set; }
        public int Quantidade { get; set; }
    }

}

