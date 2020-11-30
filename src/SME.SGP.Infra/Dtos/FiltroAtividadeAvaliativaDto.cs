using System;

namespace SME.SGP.Infra
{
    public class FiltroAtividadeAvaliativaDto
    {
        public DateTime? DataAvaliacao { get; set; }
        public string[] DisciplinaContidaRegenciaId { get; set; }
        public string[] DisciplinasId { get; set; }
        public string DreId { get; set; }
        public long? Id { get; set; }
        public string Nome { get; set; }
        public int? TipoAvaliacaoId { get; set; }
        public string TurmaId { get; set; }
        public string UeID { get; set; }
    }
}