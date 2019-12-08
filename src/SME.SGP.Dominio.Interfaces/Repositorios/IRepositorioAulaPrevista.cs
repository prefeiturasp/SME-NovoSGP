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
        Task<IEnumerable<AulasPrevistasDadasDto>> ObterAulaPrevistaDada(long tipoCalendarioId, string turmaId, string disciplinaId);

        Task<IEnumerable<AulaPrevista>> ObterAulasPrevistasPorFiltro(int bimestre, long tipoCalendarioId, string turmaId, string disciplinaId);

        Task<string> ObterProfessorTurmaDisciplinaAulasPrevistasDivergente(int bimestre, string turmaId, string disciplinaId, int limiteDias);
    }
}
