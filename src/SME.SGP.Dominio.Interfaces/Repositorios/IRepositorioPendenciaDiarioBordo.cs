using SME.SGP.Dominio.Interfaces;
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
    }
}
