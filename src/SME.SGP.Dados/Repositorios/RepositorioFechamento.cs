using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamento : RepositorioBase<Fechamento>, IRepositorioFechamento
    {
        public RepositorioFechamento(ISgpContext database) : base(database)
        {
        }

        public Fechamento ObterPorTurmaDisciplinaPeriodo(long turmaId, string disciplinaId, long periodoEscolarId)
        {
            var query = @"select
	                            *
                            from
	                            fechamento
                            where
                                turma_id = @turmaId
                                and disciplina_id = @disciplinaId
                                and periodo_escolar_id = @periodoEscolarId";

            return database.Conexao.QueryFirstOrDefault<Fechamento>(query, new
            {
                turmaId,
                disciplinaId,
                periodoEscolarId
            });
        }
    }
}