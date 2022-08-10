using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Minio;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Infra
{
    public class ServicoArmazenamento : IServicoArmazenamento
    {
        private MinioClient minioClient;
        private ConfiguracaoArmazenamentoOptions configuracaoArmazenamentoOptions;

        public ServicoArmazenamento(IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions)
        {
            this.configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptions?.Value ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions));

            Inicializar();
        }

        private void Inicializar()
        {
            minioClient = new MinioClient()
                .WithEndpoint(configuracaoArmazenamentoOptions.EndPoint,configuracaoArmazenamentoOptions.Port)
                .WithCredentials(configuracaoArmazenamentoOptions.AccessKey,configuracaoArmazenamentoOptions.SecretKey)
                .Build();
        }
        
        public async Task<string> ArmazenarTemporaria(string nomeArquivo, Stream stream, string contentType)
        {
            await ArmazenarArquivo(nomeArquivo, stream, contentType,configuracaoArmazenamentoOptions.BucketTemp);
            
            return ObterUrl(nomeArquivo,configuracaoArmazenamentoOptions.BucketTemp);
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
                
             return ObterUrl(nomeArquivo,bucket);
        }
        
        public async Task<string> Copiar(string nomeArquivo)
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
            
                await Excluir(nomeArquivo,configuracaoArmazenamentoOptions.BucketTemp);
            }
            return $"{configuracaoArmazenamentoOptions.BucketArquivos}/{nomeArquivo}";
        }

        public async Task<bool> Excluir(string nomeArquivo, string nomeBucket = "")
        {
            nomeBucket = nomeBucket == string.Empty 
                                    ? configuracaoArmazenamentoOptions.BucketArquivos
                                    : nomeBucket;
            try
            {
                var args = new RemoveObjectArgs()
                    .WithBucket(nomeBucket)
                    .WithObject(nomeArquivo);
                
                await minioClient.RemoveObjectAsync(args);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<IEnumerable<string>> ObterBuckets()
        {
            var nomesBuckets = new List<string>();
            
            var buckets = await minioClient.ListBucketsAsync();
            
            foreach (var bucket in buckets.Buckets)
                nomesBuckets.Add(bucket.Name);

            return nomesBuckets;
        }
        
        public async Task<string> Obter(string nomeArquivo, bool ehPastaTemp)
        {
            var bucketNome = ehPastaTemp
                ? configuracaoArmazenamentoOptions.BucketTemp
                : configuracaoArmazenamentoOptions.BucketArquivos;
            
            var statObjectArgs = new StatObjectArgs()
                .WithBucket(bucketNome)
                .WithObject(nomeArquivo);
            
            var arquivo = await minioClient.StatObjectAsync(statObjectArgs);
            
            return ObterUrl(nomeArquivo,bucketNome);
        }
        
        private string ObterUrl(string nomeArquivo, string bucketName)
        {
            return $"{configuracaoArmazenamentoOptions.TipoRequisicao}://{configuracaoArmazenamentoOptions.EndPoint}:{configuracaoArmazenamentoOptions.Port}/{bucketName}/{nomeArquivo}";
        }
    }
}