using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Interface
{
    public interface IServicoArmazenamento
    {
        Task<bool> ArmazenarTemporaria(string nomeArquivo, Stream stream, string contentType);
        Task<bool> Armazenar(string nomeArquivo, Stream stream, string contentType);
        Task<bool> Copiar(string nomeArquivo);
        Task<bool> Mover(string nomeArquivo);
        Task<bool> Excluir(string nomeArquivo, string nomeBucket);
        Task<IEnumerable<string>> ObterBuckets();
        Task<string> Obter(string nomeArquivo, bool ehPastaTemp);
    }
}
