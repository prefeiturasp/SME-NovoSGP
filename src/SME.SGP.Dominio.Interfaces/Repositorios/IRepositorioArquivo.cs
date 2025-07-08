using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioArquivo : IRepositorioBase<Arquivo>
    {
        Task<Arquivo> ObterPorCodigo(Guid codigo);
        Task<IEnumerable<Arquivo>> ObterPorCodigos(Guid[] codigos);
        Task<IEnumerable<Arquivo>> ObterPorIds(long[] ids);
        Task<bool> ExcluirArquivoPorCodigo(Guid codigoArquivo);
        Task<bool> ExcluirArquivoPorId(long id);
        Task<long> ObterIdPorCodigo(Guid arquivoCodigo);
        Task<bool> ExcluirArquivosPorIds(long[] ids);
        Task<IEnumerable<Arquivo>> ObterComprimir(DateTime dataInicio, DateTime dataFim);
    }
}
