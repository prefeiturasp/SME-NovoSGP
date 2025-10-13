using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalAbandonoUe
    {
        Task<(IEnumerable<PainelEducacionalAbandonoTurmaDto> Modalidades, int TotalPaginas, int TotalRegistros)> ObterAbandonoUe(int anoLetivo, string codigoDre, string codigoUe, int modalidade, int numeroPagina, int numeroRegistros);
    }
}