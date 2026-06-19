using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Servicos.Interfaces
{
    public interface IProcessadorImagens
    {
        Task<byte[]> ObterImagemEmBytesAsync(IFormFile file);
        Task<byte[]> CriarMiniaturaAsync(byte[] imagemBytes, int largura, int altura);
        Task<byte[]> CriarMiniaturaAsync(IFormFile file, int largura, int altura);
        string ObterTipoConteudo(string nomeArquivo);
    }
}
