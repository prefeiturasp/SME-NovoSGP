
namespace SME.SGP.Infra.Dtos
{
    public class RegistroAcaoBuscaAtivaRespostaDto
    {
        public long DreId { get; set; }
        public string DreNome { get; set; }
        public string DreCodigo { get; set; }
        
        public long UeId { get; set; }
        public string UeNome { get; set; }
        public string UeCodigo { get; set; }
        
        public long TurmaId { get; set; }
        public string TurmaNome { get; set; }
        public string TurmaCodigo { get; set; }
        
        public int AnoLetivo { get; set; }
        public AlunoTurmaReduzidoDto Aluno { get; set; }
        public int Modalidade { get; set; }
    }
}
