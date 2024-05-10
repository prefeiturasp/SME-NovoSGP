using System;
using System.Collections.Generic;
using System.Linq;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class ItineranciaDto
    {
        public ItineranciaDto()
        {
            ObjetivosVisita = new List<ItineranciaObjetivoDto>();
            Alunos = new List<ItineranciaAlunoDto>();
            Questoes = new List<ItineranciaQuestaoDto>();
        }
        public long Id { get; set; }
        
        public long DreId { get; set; }
        public long UeId { get; set; }
        public int AnoLetivo { get; set; }
        public string CriadoRF { get; set; }
        public DateTime DataVisita { get; set; }
        public DateTime? DataRetornoVerificacao { get; set; }
        public IEnumerable<ItineranciaObjetivoDto> ObjetivosVisita { get; set; }
        public IEnumerable<ItineranciaAlunoDto> Alunos { get; set; }
        public IEnumerable<ItineranciaQuestaoDto> Questoes { get; set; }
        public long? TipoCalendarioId { get; set; }
        public long? EventoId { get; set; }
        public AuditoriaDto Auditoria { get; set; }
        public string StatusWorkflow { get; set; }
        public TipoQuestao TipoQuestao { get; set; }
        public bool PodeEditar { get; set; }
        public bool PossuiAlunos { get => Alunos.NaoEhNulo() && Alunos.Any(); }
        public bool PossuiObjetivos { get => ObjetivosVisita.NaoEhNulo() && ObjetivosVisita.Any(); }
        public bool PossuiQuestoes { get => Questoes.NaoEhNulo() && Questoes.Any(); }
    }
}
