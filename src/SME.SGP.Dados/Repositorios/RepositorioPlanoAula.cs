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

        public async Task ExcluirPlanoDaAula(long aulaId)
        {
            // Excluir objetivos de aprendizagem do plano
            var command = @"delete from objetivo_aprendizagem_aula 
                            where plano_aula_id in (
                                select id from plano_aula where aula_id = @aulaId) ";
            await database.ExecuteAsync(command, new { aulaId });

            // Excluir plano de aula
            command = "delete from plano_aula where aula_id = @aulaId";
            await database.ExecuteAsync(command, new { aulaId });
        }

        public async Task<PlanoAula> ObterPlanoAulaPorAula(long aulaId)
        {
            var query = "select * from plano_aula where aula_id = @aulaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<PlanoAula>(query, new { aulaId });
        }

        public async Task<PlanoAula> ObterPlanoAulaPorDataDisciplina(DateTime data, string turmaId, string disciplinaId)
        {
            var query = @"select pa.*
                 from plano_aula pa
                inner join aula a on a.Id = pa.aula_id
                where DATE(a.data_aula) = @data
                  and a.turma_id = @turmaId
                  and a.disciplina_id = @disciplinaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<PlanoAula>(query, new { data, turmaId, disciplinaId });
        }

    }
}
