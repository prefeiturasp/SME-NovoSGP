namespace SME.SGP.Infra.Dtos.ImportarArquivo
{
    public class ProficienciaIdepDto : ArquivoImportacaoBaseDto
    {
        public ProficienciaIdepDto(int serieAno, string codigoEOLEscola, int anoLetivo, int componenteCurricular, decimal proficiencia, string boletim = "")
        {
            SerieAno = serieAno;
            AnoLetivo = anoLetivo;
            CodigoEOLEscola = codigoEOLEscola;
            ComponenteCurricular = componenteCurricular;
            Proficiencia = proficiencia;
            Boletim = boletim;
        }

        public long? Id { get; set; }
        public int SerieAno { get; set; }
        public int AnoLetivo { get; set; }
        public int ComponenteCurricular { get; set; }
        public decimal Proficiencia { get; set; }
        public string Boletim { get; set; }
    }
}
