using Microsoft.AspNetCore.Http;
using SkiaSharp;
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

            return await Task.Run(() =>
            {
                // Carrega a imagem original
                using (var skBitmap = SKBitmap.Decode(imagemBytes))
                {
                    if (skBitmap == null)
                        throw new ArgumentException("Não foi possível decodificar a imagem", nameof(imagemBytes));

                    // Calcula novas dimensões mantendo proporção
                    var (novaLargura, novaAltura) = CalcularDimensoes(skBitmap.Width, skBitmap.Height, largura, altura);

                    // Opções de amostragem para redimensionamento de alta qualidade
                    var samplingOptions = new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear);

                    // Redimensiona a imagem
                    using (var resizedBitmap = skBitmap.Resize(new SKSizeI(novaLargura, novaAltura), samplingOptions))
                    {
                        if (resizedBitmap == null)
                            throw new InvalidOperationException("Falha ao redimensionar a imagem");

                        // Converte para JPEG com qualidade 85
                        using (var image = SKImage.FromBitmap(resizedBitmap))
                        {
                            var jpegData = image.Encode(SKEncodedImageFormat.Jpeg, 85);
                            return jpegData.ToArray();
                        }
                    }
                }
            });
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

        /// <summary>
        /// Calcula as novas dimensões mantendo a proporção da imagem
        /// Modo: Max (redimensiona para caber dentro do tamanho especificado)
        /// </summary>
        private (int largura, int altura) CalcularDimensoes(int larguraOriginal, int alturaOriginal, int larguraMax, int alturaMax)
        {
            // Se a imagem já é menor que o máximo, retorna as dimensões originais
            if (larguraOriginal <= larguraMax && alturaOriginal <= alturaMax)
                return (larguraOriginal, alturaOriginal);

            // Calcula as proporções
            double proporcaoLargura = (double)larguraMax / larguraOriginal;
            double proporcaoAltura = (double)alturaMax / alturaOriginal;

            // Usa a menor proporção para manter a imagem dentro dos limites
            double proporcao = Math.Min(proporcaoLargura, proporcaoAltura);

            int novaLargura = (int)(larguraOriginal * proporcao);
            int novaAltura = (int)(alturaOriginal * proporcao);

            return (novaLargura, novaAltura);
        }
    }
}