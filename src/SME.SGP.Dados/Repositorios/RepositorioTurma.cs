using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTurma : IRepositorioTurma
    {
        private readonly ISgpContext database;

        public RepositorioTurma(ISgpContext database)
        {
            this.database = database;
        }

        public Turma ObterPorId(string turmaId)
        {
            return database.QueryFirstOrDefault<Turma>("select * from turma where turma_id = @turmaId", new { turmaId });
        }
    }
}