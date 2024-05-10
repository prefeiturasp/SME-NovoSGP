using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioHistoricoEscolarObservacao : IRepositorioBase<HistoricoEscolarObservacao>
    {
        Task<HistoricoEscolarObservacao> ObterPorCodigoAlunoAsync(string codigoAluno);
        Task<IEnumerable<HistoricoEscolarObservacao>> ObterPorCodigosAlunosAsync(string[] codigosAlunos);
    }
}
