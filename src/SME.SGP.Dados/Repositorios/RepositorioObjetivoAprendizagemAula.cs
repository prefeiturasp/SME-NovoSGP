using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioObjetivoAprendizagemAula : RepositorioBase<ObjetivoAprendizagemAula>, IRepositorioObjetivoAprendizagemAula
    {
        public RepositorioObjetivoAprendizagemAula(ISgpContext database) : base(database)
        { }

        public async Task<IEnumerable<ObjetivoAprendizagemAula>> ObterObjetivosPlanoAula(long planoAulaId)
        {
            var query = "select * from objetivo_aprendizagem_aula where plano_aula_id = @planoAulaId";

            return await database.Conexao.QueryAsync<ObjetivoAprendizagemAula>(query, new { planoAulaId });
        }
    }
}
