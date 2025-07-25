using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamentoReaberturaBimestre : RepositorioBase<FechamentoReaberturaBimestre>, IRepositorioFechamentoReaberturaBimestre
    {
        public RepositorioFechamentoReaberturaBimestre(ISgpContextConsultas database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<FechamentoReaberturaBimestre>> ObterPorFechamentoReaberturaIdAsync(long fechamentoReaberturaId)
        {
            var query = @"select * from fechamento_reabertura_bimestre where fechamento_reabertura_id = @fechamentoReaberturaId";

            return await database.Conexao.QueryAsync<FechamentoReaberturaBimestre>(query, new { fechamentoReaberturaId });
        }
    }
}
