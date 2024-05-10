using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public interface ISevicoJasper
    {
        Task<byte[]> DownloadRelatorio(Guid exportID, Guid requestId, string jSessionId);
    }
}