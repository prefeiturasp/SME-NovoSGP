using Microsoft.Extensions.Options;
using Nest;
using SME.SGP.Dados.ElasticSearch;
using SME.SGP.Infra;
using SME.SGP.Infra.ElasticSearch;
using SME.SGP.Metrica.Worker.Entidade;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;

namespace SME.SGP.Metrica.Worker.Repositorios
{
    public class RepositorioFrequenciaAlunoDuplicado : RepositorioElasticBase<FrequenciaAlunoDuplicado>, IRepositorioFrequenciaAlunoDuplicado
    {
        public RepositorioFrequenciaAlunoDuplicado(IElasticClient elasticClient, IServicoTelemetria servicoTelemetria, IOptions<ElasticOptions> elasticOptions) 
            : base(elasticClient, servicoTelemetria, elasticOptions, "metricas_sgp_frequencia_aluno_duplicado")
        {
        }
    }
}
