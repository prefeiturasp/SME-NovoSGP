using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
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
                string[] urls = elasticOptions.Urls.Split(',');
                foreach (string url in urls)
                    nodes.Add(new Uri(url));
            }
            else
            {
                nodes.Add(new Uri(elasticOptions.Urls));
            }
            var connectionPool = new StaticConnectionPool(nodes);
            var connectionSettings = new ConnectionSettings(connectionPool);
            connectionSettings.ServerCertificateValidationCallback((sender, cert, chain, errors) => true);
            connectionSettings.DefaultIndex(elasticOptions.IndicePadrao);

            if (!string.IsNullOrEmpty(elasticOptions.CertificateFingerprint))
                connectionSettings.CertificateFingerprint(elasticOptions.CertificateFingerprint);

            if (!string.IsNullOrEmpty(elasticOptions.Usuario) && !string.IsNullOrEmpty(elasticOptions.Senha))
                connectionSettings.BasicAuthentication(elasticOptions.Usuario, elasticOptions.Senha);

            var elasticClient = new ElasticClient(connectionSettings);
            services.AddSingleton<IElasticClient>(elasticClient);
        }
    }
}
