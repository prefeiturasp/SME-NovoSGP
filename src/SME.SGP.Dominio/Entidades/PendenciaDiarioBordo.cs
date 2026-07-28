using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class PendenciaDiarioBordo : EntidadeBase
    {
        public PendenciaDiarioBordo() { }

        public long PendenciaId { get; set; }
        [Computed]
        public Pendencia Pendencia { get; set; }
        public long AulaId { get; set; }
        [Computed]
        public Aula Aula{ get; set; }
        public long ComponenteId { get; set; }
        [Computed]
        public ComponenteCurricular ComponenteCurricular { get; set; }
        public string ProfessorRf { get; set; }
    }
}
