using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados
{
    public class RepositorioConsolidadoAtendimentoNAAPA : RepositorioBase<ConsolidadoAtendimentoNAAPA>,IRepositorioConsolidadoAtendimentoNAAPA
    {
        public RepositorioConsolidadoAtendimentoNAAPA(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<ConsolidadoAtendimentoNAAPA>> ObterPorUeIdAnoLetivo(long ueId, int anoLetivo)
        {
            var query = " select * from consolidado_atendimento_naapa can where can.ue_id = @ueId and can.ano_letivo = @anoLetivo ";
            return await database.Conexao.QueryAsync<ConsolidadoAtendimentoNAAPA>(query, new {  ueId,anoLetivo }, commandTimeout: 60);
        }
    }
}