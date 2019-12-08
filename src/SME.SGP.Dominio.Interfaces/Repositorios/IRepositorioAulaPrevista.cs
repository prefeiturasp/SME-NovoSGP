using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioAulaPrevista : IRepositorioBase<AulaPrevista>
    {
        Task<IEnumerable<AulasPrevistasDadasDto>> ObterAulaPrevistaDada(string turmaId, string disciplinaId);

        Task<AulaPrevista> ObterAulaPrevistaPorFiltro(int bimestre, long tipoCalendarioId, string turmaId, string disciplinaId);
    }
}
