using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCartaIntencoesObservacao : IRepositorioBase<CartaIntencoesObservacao>
    {
        Task<IEnumerable<ListarObservacaoCartaIntencoesDto>> ListarPorCartaIntencoesAsync(long cartaIntencoesId, long usuarioLogadoId);
    }
}
