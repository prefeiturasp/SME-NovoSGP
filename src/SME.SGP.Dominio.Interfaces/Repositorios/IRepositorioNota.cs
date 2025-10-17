using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoSmeDre;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioNota
    {
        Task<IEnumerable<PainelEducacionalNotasVisaoSmeDreRetornoSelectDto>> ObterNotasVisaoSmeDre(string codigoDre, int anoLetivo, int bimestre, int anoTurma);
        Task<IEnumerable<PainelEducacionalNotasVisaoUeRetornoSelectDto>> ObterNotasVisaoUe(string codigoDre, int anoLetivo, int bimestre, Modalidade modalidade);
    }
}
