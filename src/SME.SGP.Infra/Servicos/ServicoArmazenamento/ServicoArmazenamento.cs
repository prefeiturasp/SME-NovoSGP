using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Minio;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Infra
{
    public class ServicoArmazenamento : IServicoArmazenamento
    {
        private MinioClient minioClient;
        private ConfiguracaoArmazenamentoOptions configuracaoArmazenamentoOptions;

        public ServicoArmazenamento(ConfiguracaoArmazenamentoOptions configuracaoArmazenamentoOptions)
        {
            this.configuracaoArmazenamentoOptions = GetConfiguration(configuracaoArmazenamentoOptions ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions)));

            Inicializar();
        }

        private ConfiguracaoArmazenamentoOptions GetConfiguration(ConfiguracaoArmazenamentoOptions configuracaoArmazenamentoOptions)
        {
            return configuracaoArmazenamentoOptions;
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
            await ArmazenarArquivo(nomeArquivo, stream, contentType,configuracaoArmazenamentoOptions.BucketTempSGPName);
            
            return ObterUrl(nomeArquivo,configuracaoArmazenamentoOptions.BucketTempSGPName);
        }

        

        public async Task<string> Armazenar(string nomeArquivo, Stream stream, string contentType)
        {
            return await ArmazenarArquivo(nomeArquivo, stream, contentType,configuracaoArmazenamentoOptions.BucketSGP);
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
            var cpSrcArgs = new CopySourceObjectArgs()
                .WithBucket(configuracaoArmazenamentoOptions.BucketTempSGPName)
                .WithObject(nomeArquivo);
                
            var args = new CopyObjectArgs()
                .WithBucket(configuracaoArmazenamentoOptions.BucketSGP)
                .WithObject(nomeArquivo)
                .WithCopyObjectSource(cpSrcArgs);
                
            await minioClient.CopyObjectAsync(args);

            return $"{configuracaoArmazenamentoOptions.BucketSGP}/{nomeArquivo}";
        }

        public async Task<string> Mover(string nomeArquivo)
        {
            await Copiar(nomeArquivo);
            
            await Excluir(nomeArquivo,configuracaoArmazenamentoOptions.BucketTempSGPName);
            
            return $"{configuracaoArmazenamentoOptions.BucketSGP}/{nomeArquivo}";
        }

        public async Task<bool> Excluir(string nomeArquivo, string nomeBucket)
        {
            nomeBucket = nomeBucket == string.Empty 
                                    ? configuracaoArmazenamentoOptions.BucketTempSGPName 
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
                ? configuracaoArmazenamentoOptions.BucketTempSGPName
                : configuracaoArmazenamentoOptions.BucketSGP;
            
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