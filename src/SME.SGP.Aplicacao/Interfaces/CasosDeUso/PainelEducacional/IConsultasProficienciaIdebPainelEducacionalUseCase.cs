using SME.SGP.Infra.Dtos.PainelEducacional.ProficienciaIdeb;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasProficienciaIdebPainelEducacionalUseCase
    {
        Task<IEnumerable<PainelEducacionalProficienciaIdebDto>> ObterProficienciaIdeb(int anoLetivo, string codigoUe);
    }
}
