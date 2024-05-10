using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFrequenciaTurmaEvasaoAluno : IRepositorioFrequenciaTurmaEvasaoAluno
    {
        private readonly ISgpContext database;

        public RepositorioFrequenciaTurmaEvasaoAluno(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<long> Inserir(FrequenciaTurmaEvasaoAluno frequenciaTurmaEvasaoAluno)
        {
            return (long)(await database.Conexao.InsertAsync(frequenciaTurmaEvasaoAluno));
        }

        public async Task LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMeses(long[] turmasIds, int[] meses)
        {
            const string query = @"delete from frequencia_turma_evasao_aluno 
                                   where frequencia_turma_evasao_id in (
                                          select id from frequencia_turma_evasao
                                          where turma_id = any(@turmasIds)
                                          and mes = any(@meses)
                                    )";

            await database.Conexao.ExecuteScalarAsync(query, new { turmasIds, meses });
        }
    }
}
