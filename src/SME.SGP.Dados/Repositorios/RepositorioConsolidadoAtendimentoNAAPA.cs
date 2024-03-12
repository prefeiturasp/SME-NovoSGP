using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioConsolidadoAtendimentoNAAPA : RepositorioBase<ConsolidadoAtendimentoNAAPA>,IRepositorioConsolidadoAtendimentoNAAPA
    {
        public RepositorioConsolidadoAtendimentoNAAPA(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<long>> ObterIds(long ueId, int mes, int anoLetivo, string[] rfsProfissionaisIgnorados)
        {
            var query = @$"  select can.id
                            from consolidado_atendimento_naapa can
                            where can.ue_id = @ueId and can.ano_letivo = @anoLetivo and can.mes =@mes
                            {(rfsProfissionaisIgnorados.Any() ? $"and can.rf_profissional not in ({string.Join(",", rfsProfissionaisIgnorados.Select(rf => $"'{rf}'"))})" : string.Empty)}";

            return await database.Conexao.QueryAsync<long>(query, new { anoLetivo, ueId, mes }, commandTimeout: 60);
        }

        public async Task<ConsolidadoAtendimentoNAAPA> ObterPorUeIdMesAnoLetivoProfissional(long ueId, int mes, int anoLetivo, string rfProfissional, int modalidade)
        {
            var query = @" select * from consolidado_atendimento_naapa can 
                           where can.ue_id = @ueId 
                             and can.ano_letivo = @anoLetivo 
                             and can.mes = @mes 
                             and can.rf_profissional = @rfProfissional 
                             and can.modalidade_codigo = @modalidade";
            return await database.Conexao.QueryFirstOrDefaultAsync<ConsolidadoAtendimentoNAAPA>(query, new { ueId, mes, anoLetivo, rfProfissional, modalidade }, commandTimeout: 60);
        }

        public async Task<IEnumerable<GraficoQuantitativoNAAPADto>> ObterQuantidadeAtendimentoNAAPAPorProfissionalMes(int anoLetivo, long dreId, long? ueId, int? mes)
        {
            var query = @"  select nome_profissional as Descricao, sum(quantidade) as Quantidade,
                            COALESCE(max(can.alterado_em), max(can.criado_em)) as DataUltimaConsolidacao
                            from consolidado_atendimento_naapa can
                            inner join ue on ue.id = can.ue_id
                            where ano_letivo = @anoLetivo 
                              and ue.dre_id = @dreId";

            if (ueId.HasValue)
                query += " and ue.id = @ueId";

            if (mes.HasValue)
                query += " and can.mes = @mes";

            query += @" group by nome_profissional
                        order by nome_profissional";

            return await database.Conexao.QueryAsync<GraficoQuantitativoNAAPADto>(query, new { anoLetivo, dreId, ueId, mes }, commandTimeout: 60);
        }
    }
}