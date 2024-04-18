using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRespostaMapeamentoEstudante : RepositorioBase<RespostaMapeamentoEstudante>, IRepositorioRespostaMapeamentoEstudante
    {
        public RepositorioRespostaMapeamentoEstudante(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {}
        public async Task<IEnumerable<RespostaMapeamentoEstudante>> ObterPorQuestaoMapeamentoEstudanteId(long questaoMapeamentoEstudanteId)
        {
            var query = "select * from mapeamento_estudante_resposta where not excluido and questao_mapeamento_estudante_id = @questaoMapeamentoEstudanteId";
            return await database.Conexao.QueryAsync<RespostaMapeamentoEstudante>(query, new { questaoMapeamentoEstudanteId });
        }
    }
}
