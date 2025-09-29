using SME.SGP.Dominio.Entidades;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioProficienciaIdeb : IRepositorioBase<ProficienciaIdeb>
    {
        Task<bool> ExcluirPorAnoEscolaSerie(int anoLetivo, string codigoEOLEscola, long serieAno);
    }
}
