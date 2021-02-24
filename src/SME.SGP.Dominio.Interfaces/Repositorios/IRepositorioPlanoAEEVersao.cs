using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanoAEEVersao : IRepositorioBase<PlanoAEEVersao>
    {
        Task<IEnumerable<PlanoAEEVersaoDto>> ObterVersoesSemReestruturacaoPorPlanoId(long planoId, long reestruturacaoId);
        Task<IEnumerable<PlanoAEEVersaoDto>> ObterVersoesPorPlanoId(long planoId);
        Task<PlanoAEEVersaoDto> ObterUltimaVersaoPorPlanoId(long planoId);
        Task<int> ObterMaiorVersaoPlanoPorAlunoCodigo(string codigoAluno);
        Task<IEnumerable<PlanoAEEVersaoDto>> ObterVersoesPorCodigoEstudante(string codigoEstudante);
    }
}
