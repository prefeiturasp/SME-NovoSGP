using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ItineranciaDto
    {
        public ItineranciaDto()
        {
            ObjetivosVisita = new List<ItineranciaObjetivoDto>();
            Ues = new List<ItineranciaUeDto>();
            Alunos = new List<ItineranciaAlunoDto>();
            Questoes = new List<ItineranciaQuestaoDto>();
        }
        public long Id { get; set; }
        public int AnoLetivo { get; set; }
        public string CriadoRF { get; set; }
        public DateTime DataVisita { get; set; }
        public DateTime? DataRetornoVerificacao { get; set; }
        public IEnumerable<ItineranciaObjetivoDto> ObjetivosVisita { get; set; }
        public IEnumerable<ItineranciaUeDto> Ues { get; set; }
        public IEnumerable<ItineranciaAlunoDto> Alunos { get; set; }
        public IEnumerable<ItineranciaQuestaoDto> Questoes { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }
}
