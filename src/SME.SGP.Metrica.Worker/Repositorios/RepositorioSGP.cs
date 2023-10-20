using Dapper;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.Repositorios
{
    public class RepositorioSGP : IRepositorioSGP
    {
        private readonly ISgpContext database;

        public RepositorioSGP(ISgpContext database)
        {
            this.database = database ?? throw new System.ArgumentNullException(nameof(database));
        }

        public Task AtualizarConselhosClasseDuplicados(long fechamentoTurmaId, long ultimoId)
            => database.Conexao.ExecuteScalarAsync(
                @"update conselho_classe_aluno
			        set conselho_classe_id = @ultimoId
		        where conselho_classe_id in (
			        select id from conselho_classe 
			        where fechamento_turma_id = @fechamentoTurmaId
			          and id <> @ultimoId
		        );", new { fechamentoTurmaId, ultimoId });

        public Task ExcluirConselhosClasseDuplicados(long fechamentoTurmaId, long ultimoId)
            => database.Conexao.ExecuteScalarAsync(
                @"delete from conselho_classe 
		        where fechamento_turma_id = @fechamentoTurmaId
		            and id <> @ultimoId;", new { fechamentoTurmaId, ultimoId });

    }
}
