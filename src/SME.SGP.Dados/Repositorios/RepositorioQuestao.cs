using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioQuestao : RepositorioBase<Questao>, IRepositorioQuestao
    {
        public RepositorioQuestao(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<QuestaoIdSecaoAeeDto>> ObterQuestoesIdPorEtapa(int etapa, bool? obrigatorias)
        {
            var query = @"select q.Id, q.Ordem, sea.ordem as secaoOrdem 
                            from questao q 
                             inner join secao_encaminhamento_aee sea on sea.questionario_id = q.questionario_id
                             where sea.etapa = @etapa and not q.excluido  and not sea.excluido ";

            if (obrigatorias.HasValue)
                query = query + " and q.obrigatorio = @obrigatorias ";

            return await database.Conexao.QueryAsync<QuestaoIdSecaoAeeDto>(query, new { etapa, obrigatorias });
        }

        public async Task<bool> VerificaObrigatoriedade(long questaoId)
        {
            var query = @"select obrigatorio from questao where id = @questaoId";

            return await database.Conexao.QueryFirstOrDefaultAsync(query, new { questaoId });
        }

        
    }
}
