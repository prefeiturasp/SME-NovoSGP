namespace SME.SGP.Dominio
{
    public class AtividadeInfantil : EntidadeBase
    {
        public long AulaId { get; set; }
        public Aula Aula { get; set; }

        public long AtividadeClassroomId { get; set; }

        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public string Email { get; set; }
        public bool Excluido { get; set; }
    }
}
