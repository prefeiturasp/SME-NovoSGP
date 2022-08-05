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
            // this.configuracaoArmazenamentoOptions = GetConfiguration(configuracaoArmazenamentoOptions ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions)));
            this.configuracaoArmazenamentoOptions = new ConfiguracaoArmazenamentoOptions()
            {
                EndPoint = "localhost",
                Port = 9000,
                AccessKey = "minio",
                SecretKey = "miniosecret",
                BucketTempSGPName = "bucket-temp-sgp",
                BucketSGP = "bucket-sgp",
                TipoRequisicao = "http"
            };
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
        
        public async Task<bool> ArmazenarTemporaria(string nomeArquivo, Stream stream, string contentType)
        {
            return await ArmazenarArquivo(nomeArquivo, stream, contentType,configuracaoArmazenamentoOptions.BucketTempSGPName);
        }

        public async Task<bool> Armazenar(string nomeArquivo, Stream stream, string contentType)
        {
            return await ArmazenarArquivo(nomeArquivo, stream, contentType,configuracaoArmazenamentoOptions.BucketSGP);
        }

        private async Task<bool> ArmazenarArquivo(string nomeArquivo, Stream stream, string contentType, string bucket)
        {
            try
            {
                var args = new PutObjectArgs()
                    .WithBucket(bucket)
                    .WithObject(nomeArquivo)
                    .WithStreamData(stream)
                    .WithObjectSize(stream.Length)
                    .WithVersionId("1.0")
                    .WithContentType(contentType);
                
                await minioClient.PutObjectAsync(args);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        
        public async Task<bool> Copiar(string nomeArquivo)
        {
            try
            {
                var cpSrcArgs = new CopySourceObjectArgs()
                    .WithBucket(configuracaoArmazenamentoOptions.BucketTempSGPName)
                    .WithObject(nomeArquivo);
                    
                var args = new CopyObjectArgs()
                    .WithBucket(configuracaoArmazenamentoOptions.BucketSGP)
                    .WithObject(nomeArquivo)
                    .WithCopyObjectSource(cpSrcArgs);
                    
                await minioClient.CopyObjectAsync(args);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> Mover(string nomeArquivo)
        {
            try
            {
                await Copiar(nomeArquivo);
                await Excluir(nomeArquivo,configuracaoArmazenamentoOptions.BucketTempSGPName);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
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
            
            return $"{configuracaoArmazenamentoOptions.TipoRequisicao}://{configuracaoArmazenamentoOptions.EndPoint}:{configuracaoArmazenamentoOptions.Port}/{bucketNome}/{nomeArquivo}";
        }
    }
}