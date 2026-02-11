using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasAlfabetizacaoCriticaEscritaPainelEducacionalUseCase
    {
        Task<IEnumerable<PainelEducacionalIndicadorAlfabetizacaoCriticaDto>> ObterNumeroEstudantes(int anoLetivo, string codigoDre = null, string codigoUe = null);
    }
}
