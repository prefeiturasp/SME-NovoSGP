using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioMapeamentoEstudanteSecao : RepositorioBase<MapeamentoEstudanteSecao>, IRepositorioMapeamentoEstudanteSecao
    {
        public RepositorioMapeamentoEstudanteSecao(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<long>> ObterIdsSecoesPorMapeamentoEstudanteId(long mapeamentoEstudanteId)
        {
            var query = "select id from mapeamento_estudante_secao  where not excluido and mapeamento_estudante_id = @mapeamentoEstudanteId";
            return await database.Conexao.QueryAsync<long>(query, new { mapeamentoEstudanteId });
        }
    }
}
