using System;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class PainelEducacionalProficienciaIdepDto
    {
        public int AnoLetivo { get; set; }
        public decimal? PercentualInicial { get; set; }
        public decimal? PercentualFinal { get; set; }
        public string Boletim { get; set; }
        public bool EhAnoAtual => DateTime.Now.Year == AnoLetivo;
        public ProficienciaIdepResumidoDto Proficiencia { get; set; }
    }
}
