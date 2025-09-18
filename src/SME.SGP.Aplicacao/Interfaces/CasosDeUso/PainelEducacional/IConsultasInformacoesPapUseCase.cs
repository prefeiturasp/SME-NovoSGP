using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasInformacoesPapUseCase
    {
        Task<IEnumerable<ConsolidacaoInformacoesPap>> ObterInformacoesPap(string codigoDre, string codigoUe);
    }
}
