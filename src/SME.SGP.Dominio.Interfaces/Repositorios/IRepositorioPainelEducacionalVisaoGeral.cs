using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalVisaoGeral : IRepositorioBase<PainelEducacionalVisaoGeral>
    {
        Task<IEnumerable<PainelEducacionalVisaoGeralDto>> ObterVisaoGeralPainelEducacional();
        Task<IEnumerable<PainelEducacionalVisaoGeralDto>> ObterVisaoGeralConsolidada(int anoLetivo, string codigoDre, string codigoUe);
        Task ExcluirVisaoGeral();
        Task<bool> SalvarVisaoGeral(PainelEducacionalVisaoGeral entidade);
    }
}
