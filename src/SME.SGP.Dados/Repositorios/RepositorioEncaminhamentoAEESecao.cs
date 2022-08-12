using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEncaminhamentoAEESecao : RepositorioBase<EncaminhamentoAEESecao>, IRepositorioEncaminhamentoAEESecao
    {
        public RepositorioEncaminhamentoAEESecao(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<long>> ObterIdsSecoesPorEncaminhamentoAEEId(long encaminhamentoAEEId)
        {
            var query = "select id from encaminhamento_aee_secao eas where not excluido and encaminhamento_aee_id = @encaminhamentoAEEId";

            return await database.Conexao.QueryAsync<long>(query, new { encaminhamentoAEEId });
        }
    }
}
