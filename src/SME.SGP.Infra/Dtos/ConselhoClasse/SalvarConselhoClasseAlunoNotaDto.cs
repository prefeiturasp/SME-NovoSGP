namespace SME.SGP.Infra
{
    public class SalvarConselhoClasseAlunoNotaDto
    {
        public ConselhoClasseNotaDto ConselhoClasseNotaDto { get; set; } 
        public string CodigoAluno { get; set; } 
        public long ConselhoClasseId { get; set; } 
        public long FechamentoTurmaId { get; set; } 
        public string CodigoTurma { get; set; } 
        public int Bimestre { get; set; }        
    }
}