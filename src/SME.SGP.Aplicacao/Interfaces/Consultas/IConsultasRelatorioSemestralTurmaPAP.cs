using SME.SGP.Dominio;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasRelatorioSemestralTurmaPAP
    {
        Task<RelatorioSemestralTurmaPAP> ObterPorTurmaCodigoSemestreAsync(string turmaCodigo, int semestre);
        Task<RelatorioSemestralTurmaPAP> ObterPorIdAsync(long relatorioSemestralId);
    }
}
