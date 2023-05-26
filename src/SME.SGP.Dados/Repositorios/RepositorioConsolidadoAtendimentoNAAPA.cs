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

        public async Task<ConsolidadoAtendimentoNAAPA> ObterPorUeIdMesAnoLetivoProfissional(long ueId, int mes, int anoLetivo, string profissional)
        {
            var query = " select * from consolidado_atendimento_naapa can where can.ue_id = @ueId and can.ano_letivo = @anoLetivo and can.mes = @mes and can.profissional = @profissional ";
            return await database.Conexao.QueryFirstOrDefaultAsync<ConsolidadoAtendimentoNAAPA>(query, new { ueId, mes, anoLetivo, profissional }, commandTimeout: 60);
        }
    }
}