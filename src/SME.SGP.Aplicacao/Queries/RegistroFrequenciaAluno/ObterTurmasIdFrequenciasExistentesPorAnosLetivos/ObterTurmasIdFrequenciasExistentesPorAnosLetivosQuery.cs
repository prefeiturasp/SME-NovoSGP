using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasIdFrequenciasExistentesPorAnosLetivosQuery : IRequest<IEnumerable<FiltroMigracaoFrequenciaAulasDto>>
    {
        public ObterTurmasIdFrequenciasExistentesPorAnosLetivosQuery(int[] anosLetivos)
        {
            AnosLetivos = anosLetivos;
        }

        public int[] AnosLetivos { get; set; }
    }
}
