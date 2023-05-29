using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioConsolidadoAtendimentoNAAPA : RepositorioBase<ConsolidadoAtendimentoNAAPA>,IRepositorioConsolidadoAtendimentoNAAPA
    {
        private string TODOS = "-99";
        public RepositorioConsolidadoAtendimentoNAAPA(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<ConsolidadoAtendimentoNAAPA>> ObterPorUeIdAnoLetivo(long ueId, int anoLetivo)
        {
            var query = " select * from consolidado_atendimento_naapa can where can.ue_id = @ueId and can.ano_letivo = @anoLetivo ";
            return await database.Conexao.QueryAsync<ConsolidadoAtendimentoNAAPA>(query, new {  ueId,anoLetivo }, commandTimeout: 60);
        }

        public async Task<IEnumerable<QuantidadeEncaminhamentoNAAPAEmAbertoDto>> ObterQuantidadeEncaminhamentoNAAPAEmAberto(int anoLetivo, string codigoDre)
        {
            var situacaoEncerrada = (int)SituacaoNAAPA.Encerrado;
            var query = @"  select dre.dre_id CodigoDre, dre.nome DescricaoDre, sum(quantidade) Quantidade, max(cen.criado_em) DataUltimaConsolidacao
                            from consolidado_encaminhamento_naapa cen
                            inner join ue on ue.id = cen.ue_id
                            inner join dre on dre.id = ue.dre_id
                            where ano_letivo = @anoLetivo 
                              and situacao <> @situacaoEncerrada";

            if (!string.IsNullOrEmpty(codigoDre) && codigoDre != TODOS)
                query += " and dre.dre_id = @codigoDre";

            query += @" group by dre.dre_id, dre.nome
                        order by dre.dre_id, dre.nome";

            return await database.Conexao.QueryAsync<QuantidadeEncaminhamentoNAAPAEmAbertoDto>(query, new { anoLetivo, codigoDre, situacaoEncerrada }, commandTimeout: 60);
        }
    }
}