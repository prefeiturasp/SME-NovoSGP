using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class Aviso : EntidadeBase
    {
        public long AulaId { get; set; }
        public Aula Aula { get; set; }

        public long AvisoClassroomId { get; set; }

        public string Mensagem { get; set; }
        public string Email { get; set; }
        public bool Excluido { get; set; }
    }
}
