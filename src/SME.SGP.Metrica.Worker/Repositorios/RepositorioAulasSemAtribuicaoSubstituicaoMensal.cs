using Microsoft.Extensions.Options;
using Nest;
using SME.SGP.Dados.ElasticSearch;
using SME.SGP.Infra;
using SME.SGP.Infra.ElasticSearch;
using SME.SGP.Metrica.Worker.Entidade;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;

namespace SME.SGP.Metrica.Worker.Repositorios
{
    public class RepositorioAulasSemAtribuicaoSubstituicaoMensal : RepositorioElasticBase<AulasSemAtribuicaoSubstituicaoMensal>, IRepositorioAulasSemAtribuicaoSubstituicaoMensal
    {
        public RepositorioAulasSemAtribuicaoSubstituicaoMensal(IElasticClient elasticClient, IServicoTelemetria servicoTelemetria, IOptions<ElasticOptions> elasticOptions) 
            : base(elasticClient, servicoTelemetria, elasticOptions, "metricas_sgp_aulas_sem_atribuicao_substituicao_mensal")
        {
        }
    }
}
