using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IConsultasAulaPrevista
    {
        Task<IEnumerable<AulasPrevistasDadasDto>> ObterAulaPrevistaDada(string turmaId, string disciplinaId);
    }
}
