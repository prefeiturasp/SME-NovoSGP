using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConsolidadoEncaminhamentoNAAPA: RepositorioBase<ConsolidadoEncaminhamentoNAAPA>, IRepositorioConsolidadoEncaminhamentoNAAPA
    {
        public RepositorioConsolidadoEncaminhamentoNAAPA(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<ConsolidadoEncaminhamentoNAAPA>> ObterPorUeIdAnoLetivo(long ueId,int anoLetivo)
        {
            var query = " select * from consolidado_encaminhamento_naapa cen where cen.ue_id = @ueId and cen.ano_letivo = @anoLetivo ";
            return await database.Conexao.QueryAsync<ConsolidadoEncaminhamentoNAAPA>(query, new {  ueId,anoLetivo }, commandTimeout: 60);
        }

        public async Task<ConsolidadoEncaminhamentoNAAPA> ObterPorUeIdAnoLetivoSituacao(long ueId, int anoLetivo, int situacao)
        {
           var query = "select * from consolidado_encaminhamento_naapa cen where cen.ue_id = @ueId and cen.ano_letivo = @anoLetivo and  cen.situacao = @situacao ";

            return await database.Conexao.QueryFirstOrDefaultAsync<ConsolidadoEncaminhamentoNAAPA>(query, new { ueId, anoLetivo, situacao }, commandTimeout:60);
        }

        public async Task<IEnumerable<DadosGraficoSitaucaoPorUeAnoLetivoDto>> ObterDadosGraficoSitaucaoPorUeAnoLetivo(int anoLetivo,long? ueId,long? dreId)
        {
            var sql = new StringBuilder(); 
            sql.AppendLine(@"select");
            sql.AppendLine(@"	 cen.situacao,");
            sql.AppendLine(@"	 sum(cen.quantidade)::int4 as quantidade,");
            sql.AppendLine(@"	 COALESCE(max(cen.alterado_em), max(cen.criado_em))DataUltimaConsolidacao");
            sql.AppendLine(@"from consolidado_encaminhamento_naapa cen");
            sql.AppendLine(@"inner join ue u on u.id = cen.ue_id");
            sql.AppendLine(@"where cen.ano_Letivo = @anoLetivo ");
            if(ueId != null)
               sql.AppendLine(@"	and cen.ue_id= @ueId ");
            if(dreId != null)
               sql.AppendLine(@"	and u.dre_id = @dreId ");
            sql.AppendLine(@"group by cen.situacao;");
            return await database.Conexao.QueryAsync<DadosGraficoSitaucaoPorUeAnoLetivoDto>(sql.ToString(), new {  ueId,anoLetivo,dreId }, commandTimeout: 60);
        }
    }
}