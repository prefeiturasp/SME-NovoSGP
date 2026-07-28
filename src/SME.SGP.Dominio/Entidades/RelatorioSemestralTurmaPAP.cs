using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class RelatorioSemestralTurmaPAP
    {
        [Key]
        public long Id { get; set; }
        public long TurmaId { get; set; }
        [Computed]
        public Turma Turma { get; set; }
        public int Semestre { get; set; }
    }
}