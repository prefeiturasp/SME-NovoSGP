using System;
using System.Threading.Tasks;
using Minio;
using MongoDB.Bson;
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
                // EndPoint = "http://localhost:9001",
                EndPoint = "localhost:9001",
                AccessKey = "minio",
                SecretKey = "miniosecret",
                BucketTempSGPName = "temp",
                BucketSGP = "SGP"
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
                .WithEndpoint("localhost",9000)
                .WithCredentials(configuracaoArmazenamentoOptions.AccessKey,configuracaoArmazenamentoOptions.SecretKey)
                // .WithSSL()
                .Build();
        }
        
        public Task ArmazenarTemporaria()
        {
            throw new NotImplementedException();
        }

        public Task Armazenar()
        {
            throw new NotImplementedException();
        }

        public Task Copiar()
        {
            throw new NotImplementedException();
        }

        public Task Mover()
        {
            throw new NotImplementedException();
        }

        public Task Excluir()
        {
            throw new NotImplementedException();
        }

        public async Task Obter()
        {
            var buckets = await minioClient.ListBucketsAsync();
            foreach (var bucket in buckets.Buckets)
            {
                var nome = bucket.Name;
                var creationTime = bucket.CreationDateDateTime;
                var json = bucket.ToJson();
            }
        }
    }
}