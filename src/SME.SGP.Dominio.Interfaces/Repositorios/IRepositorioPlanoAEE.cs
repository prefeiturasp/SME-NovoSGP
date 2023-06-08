using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanoAEE : IRepositorioBase<PlanoAEE>
    {
        Task<int> AtualizarSituacaoPlanoPorVersao(long versaoId, int situacao);
        Task<int> AtualizarTurmaParaRegularPlanoAEE(long planoAEEId, long turmaId);
        Task<IEnumerable<PlanoAEETurmaDto>> ObterPlanosComSituacaoDiferenteDeEncerrado();

    }
}
