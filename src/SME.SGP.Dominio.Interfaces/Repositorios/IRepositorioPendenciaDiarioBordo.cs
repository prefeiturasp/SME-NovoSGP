using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio 
{
    public interface IRepositorioPendenciaDiarioBordo : IRepositorioBase<PendenciaDiarioBordo>
    {
        Task Excluir(long pendenciaId);
        Task<IEnumerable<Aula>> ListarPendenciasDiario(long dreId, int anoLetivo);
    }
}
