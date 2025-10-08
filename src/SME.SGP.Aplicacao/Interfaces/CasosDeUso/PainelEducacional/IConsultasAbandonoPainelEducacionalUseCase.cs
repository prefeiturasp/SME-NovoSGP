using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasAbandonoPainelEducacionalUseCase
    {
        Task<IEnumerable<PainelEducacionalAbandono>> ObterAbandonoVisaoSmeDre(int anoLetivo, string codigoDre, string codigoUe);
    }
}
