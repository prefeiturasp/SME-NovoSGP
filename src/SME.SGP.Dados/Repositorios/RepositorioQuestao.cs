using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioQuestao : RepositorioBase<Questao>, IRepositorioQuestao
    {
        public RepositorioQuestao(ISgpContext database, IServicoMensageria servicoMensageria) : base(database, servicoMensageria)
        {
        }

        public async Task<bool> VerificaObrigatoriedade(long questaoId)
        {
            var query = @"select obrigatorio from questao where id = @questaoId";

            return await database.Conexao.QueryFirstOrDefaultAsync(query, new { questaoId });
        }
    }
}
