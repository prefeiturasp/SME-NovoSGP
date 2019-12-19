using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class AulaCompletaDto
    {
        public DateTime DataAula { get; set; }
        public string DisciplinaId { get; set; }
        public int Id { get; set; }
        public int Quantidade { get; set; }
        public RecorrenciaAula RecorrenciaAula { get; set; }
        public EntidadeStatus Status { get; set; }
        public TipoAula TipoAula { get; set; }
        public long TipoCalendarioId { get; set; }
        public string TurmaId { get; set; }
        public string TurmaNome { get; set; }
        public string UeId { get; set; }
        public string UeNome { get; set; }
        public bool AulaCJ { get; set; }
    }
}