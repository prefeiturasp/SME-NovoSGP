using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioUE : IRepositorioUE
    {
        private readonly ISgpContext database;

        public RepositorioUE(ISgpContext database)
        {
            this.database = database;
        }

        public UE ObterUEPorTurma(string turmaId)
        {
            var query = @"select
                            escola.*
                        from
                            ue escola
                        inner
                        join turma t on
                        t.ue_id = escola.id
                        where
                            t.turma_id = @turmaId";
            return database.QueryFirstOrDefault<UE>(query, new { turmaId });
        }
    }
}