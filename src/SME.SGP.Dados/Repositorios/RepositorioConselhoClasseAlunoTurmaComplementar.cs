using Dapper;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasseAlunoTurmaComplementar : IRepositorioConselhoClasseAlunoTurmaComplementar
    {
        private readonly ISgpContext database;
        public RepositorioConselhoClasseAlunoTurmaComplementar(ISgpContext database)
        {
            this.database = database;
        }

        public async Task Inserir(long conselhoClasseAlunoId, long turmaId)
        {
            var query = @"INSERT INTO public.conselho_classe_aluno_turma_complementar
                                (conselho_classe_aluno_id, turma_id)
                            VALUES(@conselhoClasseAlunoId, @turmaId);";

            await database.Conexao.ExecuteAsync(query.ToString(), new { conselhoClasseAlunoId, turmaId });
        }

        public async Task<bool> VerificarSeExisteRegistro(long conselhoClasseAlunoId, long turmaId)
        {
            var query = @"SELECT 1 FROM public.conselho_classe_aluno_turma_complementar
                              where conselho_classe_aluno_id = @conselhoClasseAlunoId and 
                                    turma_id = @turmaId;";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query.ToString(), new { conselhoClasseAlunoId, turmaId });
        }
    }
}
