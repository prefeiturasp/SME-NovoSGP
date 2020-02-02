using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEventoFechamento : RepositorioBase<EventoFechamento>, IRepositorioEventoFechamento
    {
        public RepositorioEventoFechamento(ISgpContext database) : base(database)
        {
        }

        public EventoFechamento ObterPorIdFechamento(long fechamentoId)
        {
            return database.Conexao.QueryFirstOrDefault<EventoFechamento>("select * from evento_fechamento where fechamento_id = @fechamentoId", new
            {
                fechamentoId,
            });
        }
    }
}