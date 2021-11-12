using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAtividadeInfantil : RepositorioBase<AtividadeInfantil>, IRepositorioAtividadeInfantil
    {
        public RepositorioAtividadeInfantil(ISgpContext context) : base(context){}
        public async Task<IEnumerable<AtividadeInfantilDto>> ObterPorAulaId(long aulaId)
        {
            var query = @"";
            return await database.Conexao.QueryAsync<AtividadeInfantilDto>(query, new { aulaId });
        }
    }
}
