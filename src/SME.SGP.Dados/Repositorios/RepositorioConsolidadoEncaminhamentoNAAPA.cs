using System.Collections.Generic;
using System.Linq;
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
        private string TODOS = "-99";
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

        public async Task<IEnumerable<GraficoQuantitativoNAAPADto>> ObterQuantidadeEncaminhamentoNAAPAEmAberto(int anoLetivo, long? dreId)
        {
            var situacaoEncerrada = (int)SituacaoNAAPA.Encerrado;
            var query = @"  select dre.dre_id CodigoDre, dre.abreviacao Descricao, sum(quantidade) Quantidade,
                            COALESCE(max(cen.alterado_em), max(cen.criado_em)) as DataUltimaConsolidacao
                            from consolidado_encaminhamento_naapa cen
                            inner join ue on ue.id = cen.ue_id
                            inner join dre on dre.id = ue.dre_id
                            where ano_letivo = @anoLetivo 
                              and situacao <> @situacaoEncerrada";

            if (dreId.HasValue)
                query += " and dre.id = @dreId";

            query += @" group by dre.dre_id, dre.abreviacao
                        order by dre.dre_id, dre.abreviacao";

            return await database.Conexao.QueryAsync<GraficoQuantitativoNAAPADto>(query, new { anoLetivo, dreId, situacaoEncerrada }, commandTimeout: 60);
        }

        public async Task<IEnumerable<long>> ObterIds(long ueId, int anoLetivo, int[] situacoesIgnoradas)
        {
            var query = @$"  select cen.id
                            from consolidado_encaminhamento_naapa cen
                            where cen.ue_id = @ueId and ano_letivo = @anoLetivo 
                            {(situacoesIgnoradas.Any() ? $"and situacao not in ({string.Join(",", situacoesIgnoradas)})" : string.Empty)}";

            return await database.Conexao.QueryAsync<long>(query, new { anoLetivo, ueId }, commandTimeout: 60);
        }
    }
}