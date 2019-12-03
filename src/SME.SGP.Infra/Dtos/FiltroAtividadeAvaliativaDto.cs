using System;

namespace SME.SGP.Infra
{
    public class FiltroAtividadeAvaliativaDto
    {
        public DateTime? DataAvaliacao { get; set; }
        public string DisciplinaId { get; set; }
        public string DreId { get; set; }
        public string NomeAvaliacao { get; set; }
        public int? TipoAvaliacaoId { get; set; }
        public string TurmaId { get; set; }
        public string UeID { get; set; }
    }
}