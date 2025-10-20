using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoSmeDre;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasNotasUseCase
    {
        Task<IEnumerable<PainelEducacionalNotasVisaoSmeDreDto>> ObterNotasVisaoSmeDre(string codigoDre, int anoLetivo, int bimestre, string anoTurma);
        Task<PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>> ObterNotasVisaoUe(string codigoUe, int anoLetivo, int bimestre, Modalidade modalidade);
    }
}
