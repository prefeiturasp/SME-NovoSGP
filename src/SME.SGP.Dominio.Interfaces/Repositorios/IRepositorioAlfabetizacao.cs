using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioAlfabetizacao : IRepositorioBase<TaxaAlfabetizacao>
    {
        Task<TaxaAlfabetizacao> ObterRegistroAlfabetizacaoAsync(int anoLetivo, string codigoEOLEscola);
    }
}
