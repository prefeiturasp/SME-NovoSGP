using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFrequencia : RepositorioBase<RegistroFrequencia>, IRepositorioFrequencia
    {
        public RepositorioFrequencia(ISgpContext database) : base(database)
        {
        }

        public async Task ExcluirFrequenciaAula(long aulaId)
        {
            // Exclui registros de ausencia do aluno
            var command = @"delete from registro_ausencia_aluno 
                            where registro_frequencia_id in (
                                select id from registro_frequencia where aula_id = @aulaId)";
            await database.ExecuteAsync(command, new { aulaId });

            // Exclui registro de frequencia da aula
            command = "delete from registro_frequencia where aula_id = @aulaId ";
            await database.ExecuteAsync(command, new { aulaId });
        }

        public IEnumerable<RegistroAusenciaAluno> ObterListaFrequenciaPorAula(long aulaId)
        {
            var query = @"select ra.*
                        from
	                        registro_frequencia rf
                        inner join registro_ausencia_aluno ra on
	                        rf.id = ra.registro_frequencia_id
                        inner join aula a on
	                        a.id = rf.aula_id
                        where ra.excluido = false and
	                        a.id = @aulaId";

            return database.Conexao.Query<RegistroAusenciaAluno>(query, new { aulaId });
        }

        public RegistroFrequencia ObterRegistroFrequenciaPorAulaId(long aulaId)
        {
            var query = @"select *
                            from registro_frequencia
                          where aula_id = @aulaId";

            return database.Conexao.QueryFirstOrDefault<RegistroFrequencia>(query, new { aulaId });
        }
    }
}