using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioClassificacaoDocumento
    {
        Task<bool> ValidarTipoDocumento(long classificacaoDocumentoId, int tipoDocumento);
        Task<ClassificacaoDocumento> ObterPorIdAsync(long id);
    }
}