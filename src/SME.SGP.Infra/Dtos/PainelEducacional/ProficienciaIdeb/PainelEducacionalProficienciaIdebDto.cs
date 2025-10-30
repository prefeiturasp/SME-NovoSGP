using System;

namespace SME.SGP.Infra.Dtos.PainelEducacional.ProficienciaIdeb
{
    public class PainelEducacionalProficienciaIdebDto
    {
        public int AnoLetivo { get; set; }
        public decimal? NotaInicial { get; set; }
        public decimal? NotaFinal { get; set; }
        public decimal? NotaEnsinoMedio { get; set; }
        public string Boletim { get; set; }
        public bool EhAnoAtual => DateTime.Now.Year == AnoLetivo;
        public ProficienciaIdebResumidoDto Proficiencia { get; set; }
    }
}