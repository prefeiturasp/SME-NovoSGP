namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class PainelEducacionalAprovacaoUeDto
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string Turma { get; set; }
        public string Modalidade { get; set; }
        public int TotalPromocoes { get; set; }
        public int TotalRetencoesAusencias { get; set; }
        public int TotalRetencoesNotas { get; set; }
        public int AnoLetivo { get; set; }
    }
}
