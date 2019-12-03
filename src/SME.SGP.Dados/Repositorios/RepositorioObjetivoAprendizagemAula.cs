using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioObjetivoAprendizagemAula : RepositorioBase<ObjetivoAprendizagemAula>, IRepositorioObjetivoAprendizagemAula
    {
        public RepositorioObjetivoAprendizagemAula(ISgpContext database) : base(database)
        { }

        public async Task LimparObjetivosAula(long planoAulaId)
        {
            var command = "delete from objetivo_aprendizagem_aula where plano_aula_id = @planoAulaId";

            await database.ExecuteAsync(command, new { planoAulaId });
        }

        public async Task<IEnumerable<ObjetivoAprendizagemAula>> ObterObjetivosPlanoAula(long planoAulaId)
        {
            var query = @"select a.id, a.plano_aula_id, a.objetivo_aprendizagem_plano_id, 
                            p.id as objetivoPlanoId, p.objetivo_aprendizagem_jurema_id, p.componente_curricular_id
                          from objetivo_aprendizagem_aula a
                         inner join objetivo_aprendizagem_plano p on p.id = a.objetivo_aprendizagem_plano_id
                         where not a.excluido 
                           and a.plano_aula_id = @planoAulaId";

            return await database.Conexao.QueryAsync<ObjetivoAprendizagemAula, ObjetivoAprendizagemPlano, ObjetivoAprendizagemAula>(query, 
                (objetivoAula, objetivoPlano) =>
                {
                    objetivoAula.ObjetivoAprendizagemPlano = objetivoPlano;
                    return objetivoAula;
                },
                new { planoAulaId },
                splitOn: "id, objetivoPlanoId");
        }
    }
}
