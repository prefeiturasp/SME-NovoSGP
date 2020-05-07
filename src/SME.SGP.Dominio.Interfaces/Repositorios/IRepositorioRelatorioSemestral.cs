using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRelatorioSemestral
    {
        Task<RelatorioSemestral> ObterPorIdAsync(long id);
        Task<RelatorioSemestral> ObterPorTurmaCodigoSemestreAsync(string turmaCodigo, int semestre);
        Task SalvarAsync(RelatorioSemestral relatorioSemestral);
    }
}