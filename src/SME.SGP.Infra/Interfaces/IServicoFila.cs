using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Interfaces
{
   public interface IServicoFila
    {
        Task AdicionarFila(AdicionaFilaDto adicionaFilaDto);
    }
}
