using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalConsolidacaoProficienciaIdebUe
    {
        public int Id { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoUe { get; set; }
        public SerieAnoIndiceDesenvolvimentoEnum SerieAno { get; set; }
        public ComponenteCurricularEnum? ComponenteCurricular { get; set; }
        public decimal? Nota { get; set; }
        public decimal? Proficiencia { get; set; }
        public string Boletim { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }
}