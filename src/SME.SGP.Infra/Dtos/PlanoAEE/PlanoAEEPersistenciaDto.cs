using SME.SGP.Dominio.Enumerados;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class PlanoAEEPersistenciaDto
    {
        public PlanoAEEPersistenciaDto()
        {
            Questoes = new List<PlanoAEEQuestaoDto>();
        }

        public long? Id { get; set; }
        public long TurmaId { get; set; }
        public string TurmaCodigo { get; set; }
        public string AlunoCodigo { get; set; }
        public SituacaoPlanoAEE Situacao { get; set; }
        public List<PlanoAEEQuestaoDto> Questoes { get; set; }
    }
}
