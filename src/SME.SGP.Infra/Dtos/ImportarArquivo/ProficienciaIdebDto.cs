using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra.Dtos.ImportarArquivo
{
    public class ProficienciaIdebDto : ArquivoImportacaoBaseDto
    {
        public ProficienciaIdebDto() { }
        public ProficienciaIdebDto(short serieAno, string codigoEOLEscola, int anoLetivo, string componenteCurricular, decimal proficiencia, string boletim = "")
        {
            SerieAno = (SerieAnoIndiceDesenvolvimentoEnum)serieAno;
            AnoLetivo = anoLetivo;
            CodigoEOLEscola = codigoEOLEscola;
            ComponenteCurricular = componenteCurricular;
            Proficiencia = proficiencia;
            Boletim = boletim;
        }

        public long? Id { get; set; }
        public SerieAnoIndiceDesenvolvimentoEnum SerieAno { get; set; }
        public int AnoLetivo { get; set; }
        public string ComponenteCurricular { get; set; }
        public decimal Proficiencia { get; set; }
        public string Boletim { get; set; }
    }
}
