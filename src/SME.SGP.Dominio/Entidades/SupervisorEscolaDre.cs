
namespace SME.SGP.Dominio
{
    public class SupervisorEscolaDre : EntidadeBase
    {
        public long SupervisorId { get; set; }
        public long EscolaId { get; set; }
        public long DreId { get; set; }
    }
}
