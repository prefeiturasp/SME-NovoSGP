using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPapConsulta 
    {
        Task<IEnumerable<ContagemDificuldadePorTipoDto>> ObterContagemDificuldadesConsolidadaGeral();
      
        Task<IEnumerable<ContagemDificuldadePorTipoDto>> ObterContagemDificuldadesPorTipoDetalhado(TipoPap tipoPap, string codigoDre = null, string codigoUe = null);
    }
}