using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioResposta : IRepositorioBase<Resposta>
    {
        Task<IEnumerable<RespostaDto>> Listar(long periodoId);
    }
}