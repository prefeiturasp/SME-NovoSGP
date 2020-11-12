using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTipoDocumento
    {
        Task<IEnumerable<TipoDocumentoDto>> ListarDocumentosPorPerfil(string[] perfil);
        Task<IEnumerable<TipoDocumentoDto>> ListarTipoDocumentoClassificacao();
    }
}