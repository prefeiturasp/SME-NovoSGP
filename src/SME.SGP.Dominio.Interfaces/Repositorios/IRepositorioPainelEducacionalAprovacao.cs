using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoAprovacao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalAprovacao
    {
        Task<IEnumerable<ConsolidacaoAprovacaoDto>> ObterIndicadores(long[] turmasIds);
    }
}
