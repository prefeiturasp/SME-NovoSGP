using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Minio;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public class ServicoArmazenamento : IServicoArmazenamento
    {
        private MinioClient minioClient;
        private ConfiguracaoArmazenamentoOptions configuracaoArmazenamentoOptions;
        private readonly IConfiguration configuration;
        private readonly IServicoMensageriaSGP servicoMensageria;

        public ServicoArmazenamento(IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions, IConfiguration configuration, IServicoMensageriaSGP servicoMensageria)
        {
            this.configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptions?.Value ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));

            Inicializar();
        }

        private void Inicializar()
        {
            minioClient = new MinioClient()
                .WithEndpoint(configuracaoArmazenamentoOptions.EndPoint, configuracaoArmazenamentoOptions.Port)
                .WithCredentials(configuracaoArmazenamentoOptions.AccessKey, configuracaoArmazenamentoOptions.SecretKey)
                .WithSSL()
                .Build();
        }

        public async Task<string> ArmazenarTemporaria(string nomeArquivo, Stream stream, string contentType)
        {
            await ArmazenarArquivo(nomeArquivo, stream, contentType, configuracaoArmazenamentoOptions.BucketTemp);

            return ObterUrl(nomeArquivo, configuracaoArmazenamentoOptions.BucketTemp);
        }

        public async Task<string> Armazenar(string nomeArquivo, Stream stream, string contentType)
        {
            return await ArmazenarArquivo(nomeArquivo, stream, contentType, configuracaoArmazenamentoOptions.BucketArquivos);
        }

        private async Task<string> ArmazenarArquivo(string nomeArquivo, Stream stream, string contentType, string bucket)
        {
            var args = new PutObjectArgs()
                .WithBucket(bucket)
                .WithObject(nomeArquivo)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithVersionId("1.0")
                .WithContentType(contentType);

            await minioClient.PutObjectAsync(args);

            if (bucket.Equals(configuracaoArmazenamentoOptions.BucketArquivos))
                await OtimizarArquivos(nomeArquivo);

            return ObterUrl(nomeArquivo, bucket);
        }

        private async Task<string> Copiar(string nomeArquivo)
        {
            if (!configuracaoArmazenamentoOptions.BucketTemp.Equals(configuracaoArmazenamentoOptions.BucketArquivos))
            {
                var cpSrcArgs = new CopySourceObjectArgs()
                    .WithBucket(configuracaoArmazenamentoOptions.BucketTemp)
                    .WithObject(nomeArquivo);

                var args = new CopyObjectArgs()
                    .WithBucket(configuracaoArmazenamentoOptions.BucketArquivos)
                    .WithObject(nomeArquivo)
                    .WithCopyObjectSource(cpSrcArgs);

                await minioClient.CopyObjectAsync(args);
            }

            return $"{configuracaoArmazenamentoOptions.BucketArquivos}/{nomeArquivo}";
        }

        public async Task<string> Mover(string nomeArquivo)
        {
            if (!configuracaoArmazenamentoOptions.BucketTemp.Equals(configuracaoArmazenamentoOptions.BucketArquivos))
            {
                await Copiar(nomeArquivo);

                await Excluir(nomeArquivo, configuracaoArmazenamentoOptions.BucketTemp);

                await OtimizarArquivos(nomeArquivo);
            }

            return $"{configuracaoArmazenamentoOptions.BucketArquivos}/{nomeArquivo}";
        }

        private async Task OtimizarArquivos(string nomeArquivo)
        {
            var ehImagem = nomeArquivo.EhArquivoImagemParaOtimizar();

            var ehVideo = nomeArquivo.EhArquivoVideoParaOtimizar();

            if (ehImagem || ehVideo)
            {
                var nomeFila = ehImagem ? RotasRabbitSgpComprimirArquivos.OtimizarArquivoImagem : RotasRabbitSgpComprimirArquivos.OtimizarArquivoVideo;
                await servicoMensageria.Publicar(new MensagemRabbit(nomeArquivo), nomeFila, ExchangeSgpRabbit.Sgp, "PublicarFilaSgpComprimirArquivos");
            }
        }

        public async Task<bool> Excluir(string nomeArquivo, string nomeBucket = "")
        {
            nomeBucket = string.IsNullOrEmpty(nomeBucket)
                ? configuracaoArmazenamentoOptions.BucketArquivos
                : nomeBucket;

            var args = new RemoveObjectArgs()
                .WithBucket(nomeBucket)
                .WithObject(nomeArquivo);

            await minioClient.RemoveObjectAsync(args);
            return true;
        }

        public async Task<IEnumerable<string>> ObterBuckets()
        {
            var nomesBuckets = new List<string>();

            var buckets = await minioClient.ListBucketsAsync();

            foreach (var bucket in buckets.Buckets)
                nomesBuckets.Add(bucket.Name);

            return nomesBuckets;
        }

        public string Obter(string nomeArquivo, bool ehPastaTemp)
        {
            var bucketNome = ehPastaTemp
                ? configuracaoArmazenamentoOptions.BucketTemp
                : configuracaoArmazenamentoOptions.BucketArquivos;

            return ObterUrl(nomeArquivo, bucketNome);
        }

        private string ObterUrl(string nomeArquivo, string bucketName)
        {
            var hostAplicacao = configuracaoArmazenamentoOptions.EndPoint;
            var path = $"{hostAplicacao}/{bucketName}/{nomeArquivo}";

            var arquivoExiste = VerificarArquivo(path);
            if (!arquivoExiste)
                path = $"{hostAplicacao}/{configuracaoArmazenamentoOptions.BucketArquivosOld}/{nomeArquivo}";

            return path;
        }

        private bool VerificarArquivo(string arquivopath)
        { 
            if (!arquivopath.StartsWith("http://") && !arquivopath.StartsWith("https://"))
                arquivopath = "http://" + arquivopath;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(arquivopath).Result;
                    if (response.IsSuccessStatusCode)
                        return true;
                }
                catch (HttpRequestException e)
                {
                    return false;
                }
            }

            return false;
        }
    }
}