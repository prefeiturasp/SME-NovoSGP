using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConselhoClasseAlunoRecomendacao 
    {
        Task<IEnumerable<RecomendacoesAlunoFamiliaDto>> ObterRecomendacoesDoAlunoPorConselho(string alunoCodigo, int? bimestre, long fechamentoTurmaId);
        Task InserirRecomendacaoAlunoFamilia(long recomendacaoId, long conselhoClasseAlunoId);
        Task<IEnumerable<RecomendacoesAlunoFamiliaDto>> ObterRecomendacoesDoAlunoPorConselhoAlunoId(long conselhoClasseAlunoId);
        Task ExcluirRecomendacoesPorConselhoAlunoId(long conselhoClasseAlunoId);
    }
}
