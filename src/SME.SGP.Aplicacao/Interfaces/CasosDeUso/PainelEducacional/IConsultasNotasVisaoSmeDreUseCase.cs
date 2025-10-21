using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoSmeDre;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasNotasVisaoSmeDreUseCase
    {
        Task<IEnumerable<PainelEducacionalNotasVisaoSmeDreDto>> ObterNotasVisaoSmeDre(string codigoDre, int anoLetivo, int bimestre, string anoTurma);
    }
}
