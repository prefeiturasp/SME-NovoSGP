using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioIdeb : IRepositorioBase<Ideb>
    {
        Task<Ideb> ObterRegistroIdebAsync(int anoLetivo, int serieAno, string codigoEOLEscola);
    }
}
