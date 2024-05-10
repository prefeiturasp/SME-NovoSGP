using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCompensacaoAusencia : IRepositorioBase<CompensacaoAusencia>
    {
        Task<IEnumerable<RegistroFaltasNaoCompensadaDto>> ObterAusenciaParaCompensacao(long compensacaoId, string turmaId, string[] disciplinasId, string[] codigoAlunos, int bimestre, string professor = null);
        Task<IEnumerable<CompensacaoDataAlunoDto>> ObterAusenciaParaCompensacaoPorAlunos(string[] codigosAlunos, string[] disciplinasId, int bimestre, string turmacodigo, string professor = null);
        Task<IEnumerable<long>> ObterSemAlunoPorIds(long[] ids);
    }
}
