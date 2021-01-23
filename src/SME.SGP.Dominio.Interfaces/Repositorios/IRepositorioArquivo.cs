using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioArquivo : IRepositorioBase<Arquivo>
    {
        Task<Arquivo> ObterPorCodigo(Guid codigo);
        Task<bool> ExcluirArquivoPorCodigo(Guid codigoArquivo);
        Task<bool> ExcluirArquivoPorId(long id);
        Task<long> ObterIdPorCodigo(Guid arquivoCodigo);
    }
}
