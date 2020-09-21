using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioObjetivo : IRepositorioBase<RecuperacaoParalelaObjetivo>
    {
        Task<IEnumerable<ObjetivoDto>> Listar(long periodoId);
    }
}