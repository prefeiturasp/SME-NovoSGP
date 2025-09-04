using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class PainelEducacionalIdepDto
    {
        public int AnoSolicitado { get; set; }
        public int AnoUtilizado { get; set; }
        //public bool UltimoResultadoDisponivel { get; set; }
        public bool AnoSolicitadoSemDados { get; set; }
        public string Etapa { get; set; }
        public double MediaGeral { get; set; }
        public List<FaixaQuantidade> Distribuicao { get; set; }
    }

    public class FaixaQuantidade
    {
        public string Faixa { get; set; }
        public int Quantidade { get; set; }
    }
}
