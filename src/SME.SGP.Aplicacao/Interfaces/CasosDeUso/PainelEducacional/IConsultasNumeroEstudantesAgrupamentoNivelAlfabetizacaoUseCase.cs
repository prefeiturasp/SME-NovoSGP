using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasNumeroEstudantesAgrupamentoNivelAlfabetizacaoUseCase
    {
        Task<IEnumerable<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoDto>> ObterNumeroEstudantes(string anoLetivo, string periodo);
    }
}
