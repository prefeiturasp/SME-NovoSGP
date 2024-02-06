using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRegistroAcaoBuscaAtivaSecao : RepositorioBase<RegistroAcaoBuscaAtivaSecao>, IRepositorioRegistroAcaoBuscaAtivaSecao
    {
        public RepositorioRegistroAcaoBuscaAtivaSecao(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<long>> ObterIdsSecoesPorRegistroAcaoId(long registroAcaoId)
        {
            var query = "select id from registro_acao_busca_ativa_secao  where not excluido and registro_acao_busca_ativa_id = @registroAcaoId";
            return await database.Conexao.QueryAsync<long>(query, new { registroAcaoId });
        }
    }
}
