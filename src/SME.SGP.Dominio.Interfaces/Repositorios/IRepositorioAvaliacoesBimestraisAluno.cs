using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioAvaliacoesBimestraisAluno
    {
        Task<IEnumerable<IndicadorAvaliacaoDto>> ObterIndicadoresPorBimestre(string codigoAluno, long turmaId, string codigoTurma, int bimestre, int anoLetivo);
        Task<IEnumerable<IndicadorAvaliacaoDto>> ObterIndicadoresAvaliacaoFinal(string codigoAluno, long turmaId, string codigoTurma, int anoLetivo);
    }
}