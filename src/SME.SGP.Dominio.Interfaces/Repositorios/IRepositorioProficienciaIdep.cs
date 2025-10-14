using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioProficienciaIdep : IRepositorioBase<ProficienciaIdep>
    {
        Task<bool> ExcluirPorAnoEscolaSerieComponenteCurricular(int anoLetivo, string codigoEOLEscola, long serieAno, string componenteCurricular);
    }
}
