using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioPendenciaDiarioBordoConsulta
    {
        Task<long> ExisteIdPendenciaDiarioBordo(long aulaId, long componenteCurricularId);
    }
}
