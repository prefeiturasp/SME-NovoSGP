using SME.SGP.Dominio.Enumerados;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class PlanoAEEPersistenciaDto
    {
        public PlanoAEEPersistenciaDto()
        {
            Questoes = new List<QuestaoDto>();
        }
        public long? Id { get; set; }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public List<QuestaoDto> Questoes { get; set; }
    }
        
}
