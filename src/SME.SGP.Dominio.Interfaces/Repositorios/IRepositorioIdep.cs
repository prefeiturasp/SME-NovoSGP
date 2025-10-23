using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioIdep : IRepositorioBase<Idep>
    {
        Task<Idep> ObterRegistroIdepAsync(int anoLetivo, int serieAno, string codigoEOLEscola);
        Task<IEnumerable<Idep>> ObterRegistrosIdepsAsync(int anoLetivo, string codigoEOLEscola);
    }
}
