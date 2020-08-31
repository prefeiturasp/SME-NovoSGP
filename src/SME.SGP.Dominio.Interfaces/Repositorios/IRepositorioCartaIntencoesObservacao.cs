using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCartaIntencoesObservacao : IRepositorioBase<CartaIntencoesObservacao>
    {
        Task<IEnumerable<CartaIntencoesObservacaoDto>> ListarPorTurmaEComponenteCurricularAsync(long turmaId, long componenteCurricularId, long usuarioLogadoId);
    }
}
