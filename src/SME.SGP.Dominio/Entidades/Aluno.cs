namespace SME.SGP.Dominio
{
    public class Aluno : EntidadeBase
    {
        public string Email { get; set; }
        public string Nome { get; set; }
        public Professor Professor { get; set; }
        public long ProfessorId { get; set; }
    }
}