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
    public class RepositorioPlanoAula : RepositorioBase<PlanoAula>, IRepositorioPlanoAula
    {
        public RepositorioPlanoAula(ISgpContext conexao) : base(conexao) { }

        public async Task<PlanoAula> ObterPlanoAulaPorDataDisciplina(DateTime data, string turmaId, string disciplinaId)
        {
            var query = @"select pa.*
                 from plano_aula pa
                inner join aula a on a.Id = pa.aula_id
                where a.data_aula = @turmaId
                  and a.turma_id = @turmaId
                  and a.disciplina_id = @disciplinaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<PlanoAula>(query, new { data, turmaId, disciplinaId });
        }

    }
}
