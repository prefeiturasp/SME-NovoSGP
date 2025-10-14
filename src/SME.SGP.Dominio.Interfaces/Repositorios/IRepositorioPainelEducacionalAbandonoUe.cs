using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalAbandonoUe
    {
        Task<PaginacaoResultadoDto<PainelEducacionalAbandonoUe>> ObterAbandonoUe(int anoLetivo, string codigoDre, string codigoUe, string modalidade, int numeroPagina, int numeroRegistros);
    }
}