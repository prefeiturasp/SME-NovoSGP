using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPendenciaDiarioBordoConsulta
    {
        Task<bool> ExisteIdPendenciaDiarioBordo(long pendenciaId);
    }
}
