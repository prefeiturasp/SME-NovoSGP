using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public interface IServicoServidorRelatorios
    {
        Task<byte[]> DownloadRelatorio(Guid correlacaoId);
    }
}