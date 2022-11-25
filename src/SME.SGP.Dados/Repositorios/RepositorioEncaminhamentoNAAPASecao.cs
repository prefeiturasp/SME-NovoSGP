using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEncaminhamentoNAAPASecao : RepositorioBase<EncaminhamentoNAAPASecao>, IRepositorioEncaminhamentoNAAPASecao
    {
        public RepositorioEncaminhamentoNAAPASecao(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<long>> ObterIdsSecoesPorEncaminhamentoNAAPAId(long encaminhamentoNAAPAId)
        {
            var query = "select id from encaminhamento_naapa_secao eas where not excluido and encaminhamento_naapa_id = @encaminhamentoNAAPAId";

            return await database.Conexao.QueryAsync<long>(query, new { encaminhamentoNAAPAId });
        }
    }
}
