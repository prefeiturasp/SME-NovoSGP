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

        public async Task<IEnumerable<QuestaoDto>> ObterQuestoesPorSecoesId(long[] secoesId, bool? obrigatorias)
        {
            var query = @"select q.* from secao_encaminhamento_aee sea
                                     inner join questao q on q.questionario_id = sea.questionario_id
                                     where sea.id = ANY(@secoesId) ";

            if (obrigatorias.HasValue)
                query = query + " and q.obrigatorio = @obrigatorias ";

            return await database.Conexao.QueryAsync<QuestaoDto>(query, new { secoesId, obrigatorias });
        }

        public async Task<bool> VerificaObrigatoriedade(long questaoId)
        {
            var query = @"select obrigatorio from questao where id = @questaoId";

            return await database.Conexao.QueryFirstOrDefaultAsync(query, new { questaoId });
        }

        
    }
}
