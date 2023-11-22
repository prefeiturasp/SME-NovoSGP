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
        private readonly string nomeArquivo = "imagem.png";
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

        private string ObterUrl(string nomeArquivo, string bucketName)
        {
            return urlImagemArquivo;
        }
    }
}