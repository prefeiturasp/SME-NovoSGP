using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCompensacaoAusencia : IRepositorioBase<CompensacaoAusencia>
    {
        Task<IEnumerable<RegistroFaltasNaoCompensadaDto>> ObterAusenciaParaCompensacao(FiltroFaltasNaoCompensadasDto filtro);
    }
}
