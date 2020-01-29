using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioResposta : RepositorioBase<Resposta>, IRepositorioResposta
    {
        public RepositorioResposta(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<RespostaDto>> ListarRespostas()
        {
            var query = @"select
                            r.id,
                            r.nome,
                            r.descricao,
                            r.sim,
                            o.id as objetivoId
                          from resposta r
                            inner join objetivo_resposta o on r.id = o.resposta_id
                          where r.excluido = false 
                            and o.excluido = false 
                            and (dt_fim is null or dt_fim <= now())";
            return await database.Conexao.QueryAsync<RespostaDto>(query.ToString());
        }
    }
}