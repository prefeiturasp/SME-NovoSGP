using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoSmeDre;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioNotaConsulta 
    {
        Task<IEnumerable<PainelEducacionalNotasVisaoSmeDreRetornoSelectDto>> ObterNotasVisaoSmeDre(string codigoDre, int anoLetivo, int bimestre, string anoTurma);
        Task<PaginacaoNotaResultadoDto<PainelEducacionalNotasVisaoUeRetornoSelectDto>> ObterNotasVisaoUe(Paginacao paginacao, string codigoUe, int anoLetivo, int bimestre, Modalidade modalidade);
        Task<IEnumerable<IdentificacaoInfo>> ObterModalidadesNotasVisaoUe(int anoLetivo, string codigoUe, int bimestre);
    }
}
