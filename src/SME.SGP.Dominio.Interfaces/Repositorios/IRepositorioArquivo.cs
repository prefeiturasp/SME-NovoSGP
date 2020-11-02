using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioArquivo : IRepositorioBase<Arquivo>
    {
        Task<Arquivo> ObterPorCodigo(Guid codigo);
    }
}
