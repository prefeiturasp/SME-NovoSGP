using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRelatorioSemestralTurmaPAP
    {
        Task<RelatorioSemestralTurmaPAP> ObterPorIdAsync(long id);
        Task<RelatorioSemestralTurmaPAP> ObterPorTurmaCodigoSemestreAsync(string turmaCodigo, int semestre);
        Task SalvarAsync(RelatorioSemestralTurmaPAP relatorioSemestral);
        Task<IEnumerable<long>> ObterRelatorioSemestralTurmaPAPPorAnoSemestreAsync(int anoLetivo, int semestre);
    }
}