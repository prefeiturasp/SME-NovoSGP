using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioSecaoEncaminhamentoAEE : RepositorioBase<SecaoEncaminhamentoAEE>, IRepositorioSecaoEncaminhamentoAEE
    {
        public RepositorioSecaoEncaminhamentoAEE(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> ObterSecaoEncaminhamentoPorEtapa(long etapa)
        {
            var query = @"SELECT sea.id
	                            , sea.nome
	                            , sea.questionario_id
                         FROM secao_encaminhamento_aee sea
                         WHERE not sea.excluido 
                           AND sea.etapa = @etapa
                         ORDER BY sea.ordem ";

            return await database.Conexao.QueryAsync<SecaoQuestionarioDto>(query, new { etapa });
        }
    }
}
