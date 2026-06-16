using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SME.SGP.Aplicacao.Servicos.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Servicos
{
    public class ProcessadorImagensImageSharp : IProcessadorImagens
    {
        public async Task<byte[]> ObterImagemEmBytesAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Arquivo de imagem inválido", nameof(file));

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public async Task<byte[]> CriarMiniaturaAsync(IFormFile file, int largura, int altura)
        {
            var imagemBytes = await ObterImagemEmBytesAsync(file);
            return await CriarMiniaturaAsync(imagemBytes, largura, altura);
        }

        public async Task<byte[]> CriarMiniaturaAsync(byte[] imagemBytes, int largura, int altura)
        {
            if (imagemBytes == null || imagemBytes.Length == 0)
                throw new ArgumentException("Bytes da imagem inválidos", nameof(imagemBytes));

            using (var memoryStream = new MemoryStream(imagemBytes))
            {
                using (var image = Image.Load(memoryStream))
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(largura, altura),
                        Mode = ResizeMode.Max 
                    }));

                    using (var outputStream = new MemoryStream())
                    {
                        image.SaveAsPng(outputStream);
                        return outputStream.ToArray();
                    }
                }
            }
        }

        public string ObterTipoConteudo(string nomeArquivo)
        {
            if (string.IsNullOrEmpty(nomeArquivo))
                return "image/jpeg";

            var extensao = Path.GetExtension(nomeArquivo).ToLower();

            return extensao switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                ".bmp" => "image/bmp",
                _ => "image/jpeg"
            };
        }
    }
}