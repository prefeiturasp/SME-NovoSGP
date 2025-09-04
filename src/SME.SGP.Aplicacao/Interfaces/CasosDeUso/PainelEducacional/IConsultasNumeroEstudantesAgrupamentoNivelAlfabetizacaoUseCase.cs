using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasNumeroEstudantesAgrupamentoNivelAlfabetizacaoUseCase
    {
        Task<IEnumerable<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoDto>> ObterNumeroEstudantes(int anoLetivo, int periodo, string codigoDre = null, string codigoUe = null);
    }
}
