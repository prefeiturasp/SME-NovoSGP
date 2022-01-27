using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAnotacaoFrequenciaAluno : RepositorioBase<AnotacaoFrequenciaAluno>, IRepositorioAnotacaoFrequenciaAluno
    {
        public RepositorioAnotacaoFrequenciaAluno(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<bool> ExcluirAnotacoesDaAula(long aulaId)
        {
            var command = "update anotacao_frequencia_aluno set excluido = true where not excluido and aula_id = @aulaId";
            await database.ExecuteAsync(command, new { aulaId });
            return true;
        }          
    }
}