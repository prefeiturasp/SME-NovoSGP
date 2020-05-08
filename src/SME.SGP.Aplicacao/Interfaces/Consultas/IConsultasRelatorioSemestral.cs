using SME.SGP.Dominio;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasRelatorioSemestral
    {
        Task<RelatorioSemestral> ObterPorTurmaCodigoSemestreAsync(string turmaCodigo, int semestre);
        Task<RelatorioSemestral> ObterPorIdAsync(long relatorioSemestralId);
    }
}
