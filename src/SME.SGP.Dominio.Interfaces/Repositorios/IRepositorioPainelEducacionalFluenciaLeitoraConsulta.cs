using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalFluenciaLeitoraConsulta 
    {
        Task<IEnumerable<PainelEducacionalFluenciaLeitoraDto>> ObterFluenciaLeitora(string periodo, string anoLetivo, string codigoDre, string codigoUe);
    }
}
