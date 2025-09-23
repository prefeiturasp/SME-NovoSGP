using SME.SGP.Dominio.Entidades;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioProficienciaIdebConsulta
    {
        Task<ProficienciaIdeb> ObterPorAnoLetivoCodigoUe(int anoLetivo, string codigoUe);
    }
}