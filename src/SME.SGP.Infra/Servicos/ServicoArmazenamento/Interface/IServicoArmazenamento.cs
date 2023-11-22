using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Interface
{
    public interface IServicoArmazenamento
    {
        Task<string> ArmazenarTemporaria(string nomeArquivo, Stream stream, string contentType);
        Task<string> Armazenar(string nomeArquivo, Stream stream, string contentType);
        Task<string> Mover(string nomeArquivo);
        Task<bool> Excluir(string nomeArquivo, string nomeBucket = "");
        Task<IEnumerable<string>> ObterBuckets();
        string Obter(string nomeArquivo, bool ehPastaTemp);
    }
}
