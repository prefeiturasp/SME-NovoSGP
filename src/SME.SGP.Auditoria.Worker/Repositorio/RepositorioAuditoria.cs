using Microsoft.Extensions.Options;
using Nest;
using SME.SGP.Auditoria.Worker.Repositorio.Interfaces;
using SME.SGP.Dados.ElasticSearch;
using SME.SGP.Infra;
using SME.SGP.Infra.ElasticSearch;
using System.Threading.Tasks;

namespace SME.SGP.Auditoria.Worker.Repositorio
{
    public class RepositorioAuditoria : RepositorioElasticBase<Entidade.Auditoria>, IRepositorioAuditoria
    {
        public RepositorioAuditoria(IElasticClient elasticClient,
                                    IServicoTelemetria servicoTelemetria,
                                    IOptions<ElasticOptions> elasticOptions) 
            : base(elasticClient, servicoTelemetria, elasticOptions)
        {
        }
        
        public Task Salvar(Entidade.Auditoria auditoria)
            => InserirAsync(auditoria);
        
    }
}
