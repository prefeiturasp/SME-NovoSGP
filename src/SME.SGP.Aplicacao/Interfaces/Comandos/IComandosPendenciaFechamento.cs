using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosPendenciaFechamento
    {
        Task<IEnumerable<AuditoriaPersistenciaDto>> Aprovar(IEnumerable<long> pendenciasIds);
    }
}
