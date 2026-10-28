using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Options;
using SME.SGP.Dados.ElasticSearch;
using SME.SGP.Infra;
using SME.SGP.Infra.ElasticSearch;
using SME.SGP.Metrica.Worker.Entidade;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;

namespace SME.SGP.Metrica.Worker.Repositorios
{
    public class RepositorioConselhosClasseAlunoDiario : RepositorioElasticBase<ConselhosClasseAlunoDiario>, IRepositorioConselhosClasseAlunoDiario
    {
        public RepositorioConselhosClasseAlunoDiario(ElasticsearchClient elasticClient, IServicoTelemetria servicoTelemetria, IOptions<ElasticOptions> elasticOptions) 
            : base(elasticClient, servicoTelemetria, elasticOptions, "metricas_sgp_conselhos_classe_aluno_diario")
        {
        }
    }
}
