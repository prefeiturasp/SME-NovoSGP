using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioProficienciaIdepConsulta
    {
        Task<IEnumerable<ProficienciaIdep>> ObterPorAnoLetivoCodigoUe(int anoLetivo, List<string> codigoUe);
    }
}
