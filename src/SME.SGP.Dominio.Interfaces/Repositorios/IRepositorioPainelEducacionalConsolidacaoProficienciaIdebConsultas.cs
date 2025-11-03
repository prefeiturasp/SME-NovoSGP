using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalConsolidacaoProficienciaIdebConsultas
    {
        Task<IEnumerable<PainelEducacionalConsolidacaoProficienciaIdebUe>> ObterDadosParaConsolidarPorAnoAsync(int anoLetivo);
        Task<IEnumerable<PainelEducacionalConsolidacaoProficienciaIdebUe>> ObterConsolidacaoPorAnoVisaoUeAsync(int anoLetivo, string codigoUe);
    }
}