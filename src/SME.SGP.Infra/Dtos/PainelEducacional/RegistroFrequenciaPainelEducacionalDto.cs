namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class RegistroFrequenciaPainelEducacionalDto
    {
        public long Id { get; set; }
        public long TurmaId { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string Dre { get; set; }
        public string Ue { get; set; }
        public string CodigoAluno { get; set; }
        public int Mes { get; set; }
        public decimal Percentual { get; set; }
        public int QuantidadeAulas { get; set; }
        public int QuantidadeAusencias { get; set; }
        public int ModalidadeCodigo { get; set; }
        public string Modalidade { get; set; }
        public int AnoLetivo { get; set; }
        public string AnoTurma { get; set; }
    }
}
