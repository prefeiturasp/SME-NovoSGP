using System.Threading.Tasks;
using Nest;
using SME.SGP.Auditoria.Worker.Interfaces;
using SME.SGP.Auditoria.Worker.Repositorio.Interfaces;
using SME.SGP.Dados.ElasticSearch;
using SME.SGP.Infra;

namespace SME.SGP.Auditoria.Worker.Repositorio
{
    public class RepositorioAuditoria : RepositorioElasticBase<Entidade.Auditoria>, IRepositorioAuditoria
    {
        private const string Indice = "auditoria";
        
        public RepositorioAuditoria(IElasticClient elasticClient, IServicoTelemetria servicoTelemetria) : base(elasticClient, servicoTelemetria)
        {
        }
        
        public Task Salvar(Entidade.Auditoria auditoria)
            => InserirAsync(auditoria, Indice);
        
    }
}
