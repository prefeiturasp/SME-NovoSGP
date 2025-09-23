using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioProficienciaIdep : IRepositorioBase<ProficienciaIdep>
    {
        Task<bool> ExcluirPorAnoEscolaSerie(int anoLetivo, string CodigoEOLEscola, long SerieAno);
    }
}
