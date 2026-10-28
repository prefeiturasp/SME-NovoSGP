using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra.ElasticSearch
{
    public static class RegistrarElasticSearch
    {
        public static void RegistrarElastic(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is null)
                return;

            var configElastic = configuration.GetSection(ElasticOptions.Secao);
            services.AddOptions<ElasticOptions>()
                .Bind(configElastic, c => c.BindNonPublicProperties = true);

            services.AddSingleton<ElasticOptions>();

            var nodes = new List<Uri>();
            var elasticOptions = configElastic.Get<ElasticOptions>();

            if (elasticOptions.Urls.Contains(','))
            {
                foreach (string url in elasticOptions.Urls.Split(','))
                    nodes.Add(new Uri(url));
            }
            else
            {
                nodes.Add(new Uri(elasticOptions.Urls));
            }

            var nodePool = new StaticNodePool(nodes);

            var settings = new ElasticsearchClientSettings(nodePool)
                .DefaultIndex(elasticOptions.IndicePadrao)
                .ServerCertificateValidationCallback((sender, cert, chain, errors) => true);

            if (!string.IsNullOrEmpty(elasticOptions.CertificateFingerprint))
                settings = settings.CertificateFingerprint(elasticOptions.CertificateFingerprint);

            if (!string.IsNullOrEmpty(elasticOptions.Usuario) && !string.IsNullOrEmpty(elasticOptions.Senha))
                settings = settings.Authentication(new BasicAuthentication(elasticOptions.Usuario, elasticOptions.Senha));

            var elasticClient = new ElasticsearchClient(settings);
            services.AddSingleton<ElasticsearchClient>(elasticClient);
        }
    }
}