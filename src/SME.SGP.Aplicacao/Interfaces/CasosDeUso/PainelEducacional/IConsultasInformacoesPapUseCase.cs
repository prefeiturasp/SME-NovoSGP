using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasInformacoesPapUseCase
    {
        Task<IEnumerable<PainelEducacionalInformacoesPapDto>> ObterInformacoesPap(string codigoDre, string codigoUe);
    }
}
