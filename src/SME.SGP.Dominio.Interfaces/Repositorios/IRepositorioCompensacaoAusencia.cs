using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCompensacaoAusencia : IRepositorioBase<CompensacaoAusencia>
    {
        Task<IEnumerable<RegistroFaltasNaoCompensadaDto>> ObterAusenciaParaCompensacao(long compensacaoId, string turmaId, string disciplinaId, string[] codigoAlunos, int bimestre);
        Task<IEnumerable<CompensacaoDataAlunoDto>> ObterAusenciaParaCompensacaoPorAlunos(string[] codigosAlunos,string disciplinaId,int bimestre,string turmacodigo);
        Task<IEnumerable<long>> ObterSemAlunoPorIds(long[] ids);
    }
}
