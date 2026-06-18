using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConselhoClasseAlunoRecomendacaoConsulta
    {
        Task<IEnumerable<RecomendacaoConselhoClasseAlunoDTO>> ObterRecomendacoesPorAlunoTurma(string codigoAluno, string codigoTurma, int anoLetivo, int? modalidade, int semestre);
        Task<IEnumerable<RecomendacoesAlunoFamiliaDto>> ObterRecomendacoesAlunoFamiliaPorAlunoETurma(string codigoAluno, string codigoTurma);
    }
}
