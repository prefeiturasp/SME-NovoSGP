using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IServicoServidorRelatorios
    {
        Task<byte[]> DownloadRelatorio(Guid correlacaoId);
    }
}