namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class PainelEducacionalAbandonoTurmaDto : PainelEducacionalAbandonoBaseDto
    {
        public string Turma { get; set; }
        public string Modalidade { get; set; }
        public string CodigoTurma { get; set; }
        public int QuantidadeDesistencias { get; set; }
    }
}