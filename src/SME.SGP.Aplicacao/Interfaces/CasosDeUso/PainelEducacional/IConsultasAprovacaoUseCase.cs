using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasAprovacaoUseCase
    {
        Task<IEnumerable<PainelEducacionalAprovacaoDto>> ObterAprovacao(int anoLetivo, string codigoDre);
    }
}
