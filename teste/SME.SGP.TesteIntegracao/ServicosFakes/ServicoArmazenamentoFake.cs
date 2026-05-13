using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ServicoArmazenamentoFake : IServicoArmazenamento
    {
        private readonly string urlImagemArquivo = "http://sgp.com.br/Arquivos/imagem.png";
        private readonly ConfiguracaoArmazenamentoOptions configuracaoArmazenamentoOptions;

        public ServicoArmazenamentoFake()
        {
            var portaAleatoria = new Random();
            this.configuracaoArmazenamentoOptions = new ConfiguracaoArmazenamentoOptions
            {
                BucketTemp = "temp",
                BucketArquivos = "arquivos",
                EndPoint = "teste.minio.sgp.com.br",
                Port =  portaAleatoria.Next(1, 5000),
                AccessKey = Guid.NewGuid().ToString(),
                SecretKey = Guid.NewGuid().ToString(),
                TipoRequisicao = "https"
            };
        }

        public async  Task<string> ArmazenarTemporaria(string nomeArquivo, Stream stream, string contentType)
        {
            await Task.FromResult("");
            return ObterUrl(string.Empty, string.Empty);
        }

        public async Task<string> Armazenar(string nomeArquivo, Stream stream, string contentType)
        {
            await Task.FromResult("");
            return ObterUrl(string.Empty, string.Empty);
        }

        public async Task<string> Copiar(string nomeArquivo)
        {
            await Task.FromResult("");
            return $"{configuracaoArmazenamentoOptions.BucketArquivos}/{nomeArquivo}";
        }

        public async Task<string> Mover(string nomeArquivo)
        {
            await Task.FromResult("");
            return $"{configuracaoArmazenamentoOptions.BucketArquivos}/{nomeArquivo}";
        }

        public async Task<bool> Excluir(string nomeArquivo, string nomeBucket = "")
        {
            await Task.FromResult("");
            return true;
        }

        public async Task<IEnumerable<string>> ObterBuckets()
        {
            await Task.FromResult("");
            return new List<string>() {configuracaoArmazenamentoOptions.BucketArquivos, configuracaoArmazenamentoOptions.BucketTemp};
        }

        public string Obter(string nomeArquivo, bool ehPastaTemp)
        {
            return ObterUrl(string.Empty, string.Empty);
        }

        public Task<Stream> ObterStream(string nomeArquivo, string bucket)
        {
            var extensao = Path.GetExtension(nomeArquivo).ToLower();

            byte[] bytes = extensao switch
            {
                ".pdf" => GerarPdfFake(),
                ".png" => GerarImagemFake("png"),
                ".jpg" or ".jpeg" => GerarImagemFake("jpeg"),
                ".gif" => GerarImagemFake("gif"),
                ".mp4" or ".avi" or ".mov" => GerarVideoFake(),
                _ => new byte[] { 0x00 }
            };

            Stream stream = new MemoryStream(bytes);
            return Task.FromResult(stream);
        }

        public async Task<string> ArmazenarSemOtimizar(string nomeArquivo, Stream stream, string contentType)
        {
            await Task.FromResult("");
            return ObterUrl(string.Empty, string.Empty);
        }

        private string ObterUrl(string nomeArquivo, string bucketName)
        {
            return urlImagemArquivo;
        }
        private byte[] GerarPdfFake()
        {
            var conteudo = "%PDF-1.4\n" +
                           "1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n" +
                           "2 0 obj\n<< /Type /Pages /Kids [3 0 R] /Count 1 >>\nendobj\n" +
                           "3 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] >>\nendobj\n" +
                           "4 0 obj\n<< /Length 44 >>\nstream\nBT /F1 12 Tf 100 700 Td (Teste) Tj ET\nendstream\nendobj\n" +
                           "xref\n0 5\n" +
                           "trailer\n<< /Size 5 /Root 1 0 R >>\n" +
                           "startxref\n0\n%%EOF";

            return System.Text.Encoding.Latin1.GetBytes(conteudo);
        }
        
        private byte[] GerarVideoFake()
        {
            return new byte[]
            {
                0x00, 0x00, 0x00, 0x18,
                0x66, 0x74, 0x79, 0x70,
                0x6D, 0x70, 0x34, 0x32,
                0x00, 0x00, 0x00, 0x00,
                0x6D, 0x70, 0x34, 0x32,
                0x69, 0x73, 0x6F, 0x6D 
            };
        }
        
        private byte[] GerarImagemFake(string tipo)
        {
            if (tipo == "png")
            {
                return new byte[]
                {
                    0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A,
                    0x00, 0x00, 0x00, 0x0D, 0x49, 0x48, 0x44, 0x52,
                    0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01,
                    0x08, 0x02, 0x00, 0x00, 0x00, 0x90, 0x77, 0x53,
                    0xDE, 0x00, 0x00, 0x00, 0x0C, 0x49, 0x44, 0x41,
                    0x54, 0x08, 0xD7, 0x63, 0xF8, 0xCF, 0xC0, 0x00,
                    0x00, 0x00, 0x02, 0x00, 0x01, 0xE2, 0x21, 0xBC,
                    0x33, 0x00, 0x00, 0x00, 0x00, 0x49, 0x45, 0x4E,
                    0x44, 0xAE, 0x42, 0x60, 0x82                   
                };
            }

            if (tipo == "jpeg")
            {
                return new byte[]
                {
                    0xFF, 0xD8, 0xFF, 0xE0,
                    0x00, 0x10, 0x4A, 0x46, 0x49, 0x46, 0x00,
                    0x01, 0x01, 0x00, 0x00, 0x01, 0x00, 0x01,
                    0x00, 0x00, 0xFF, 0xD9
                };
            }

            if (tipo == "gif")
            {
                return new byte[]
                {
                    0x47, 0x49, 0x46, 0x38, 0x39, 0x61,
                    0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00,
                    0x2C, 0x00, 0x00, 0x00, 0x00,
                    0x01, 0x00, 0x01, 0x00, 0x00,
                    0x02, 0x02, 0x4C, 0x01, 0x00,
                    0x3B 
                };
            }

            return new byte[] { 0x00 };
        }
    }
}