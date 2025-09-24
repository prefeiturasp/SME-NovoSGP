using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioAlfabetizacao : IRepositorioBase<Alfabetizacao>
    {
        Task<Alfabetizacao> ObterRegistroAlfabetizacaoAsync(int anoLetivo, string codigoEOLEscola);
    }
}
