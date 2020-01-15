using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasCompensacaoAusenciaAluno
    {
        Task<IEnumerable<CompensacaoAusenciaAluno>> ObterPorCompensacao(long compensacaoId);
    }
}
