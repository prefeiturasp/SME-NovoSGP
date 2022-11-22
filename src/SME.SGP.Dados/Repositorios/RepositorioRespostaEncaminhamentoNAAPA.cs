using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRespostaEncaminhamentoNAAPA : RepositorioBase<RespostaEncaminhamentoNAAPA>, IRepositorioRespostaEncaminhamentoNAAPA
    {
        public RepositorioRespostaEncaminhamentoNAAPA(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }
        
        public async Task<IEnumerable<RespostaEncaminhamentoNAAPA>> ObterPorQuestaoEncaminhamentoId(long questaoEncaminhamentoNAAPAId)
        {
            var query = "select * from resposta_encaminhamento_naapa where not excluido and questao_encaminhamento_id = @questaoEncaminhamentoNAAPAId";

            return await database.Conexao.QueryAsync<RespostaEncaminhamentoNAAPA>(query, new { questaoEncaminhamentoNAAPAId });
        }
    }
}
