using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoDistorcaoIdade;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoFluenciaLeitoraUe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioConsultaFluenciaLeitoraUe
    {
        Task<IEnumerable<ConsolidacaoFluenciaLeitoraUeDto>> ObterFluenciaLeitoraUe(FiltroPainelEducacionalFluenciaLeitoraUe filtro);
    }
}

