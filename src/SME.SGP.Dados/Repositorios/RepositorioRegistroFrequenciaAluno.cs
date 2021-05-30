using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioRegistroFrequenciaAluno : RepositorioBase<RegistroFrequenciaAluno>, IRepositorioRegistroFrequenciaAluno
    {
        public RepositorioRegistroFrequenciaAluno(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task RemoverPorRegistroFrequenciaId(long registroFrequenciaId)
        {
            await database.Conexao.ExecuteAsync("DELETE FROM registro_frequencia_aluno WHERE registro_frequencia_id = @registroFrequenciaId", 
                new { registroFrequenciaId });
        }
    }
}
