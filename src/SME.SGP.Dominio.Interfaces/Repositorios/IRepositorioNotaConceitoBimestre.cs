using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotaConceitoBimestre : IRepositorioBase<NotaConceitoBimestre>
    {
        Task<IEnumerable<NotaConceitoBimestre>> ObterPorFechamentoTurma(long fechamentoId);
        Task<NotaConceitoBimestre> ObterPorAlunoEFechamento(long fechamentoId, string codigoAluno);
        Task<AnotacaoAlunoCompletoDto> ObterAnotacaoAlunoPorFechamento(long fechamentoId, string codigoAluno);
    }
}