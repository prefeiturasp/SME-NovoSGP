using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasseRecomendacao : RepositorioBase<ConselhoClasseRecomendacao>, IRepositorioConselhoClasseRecomendacao
    {
        public RepositorioConselhoClasseRecomendacao(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<ConselhoClasseRecomendacao>> ObterPorFiltro(ConselhoClasseRecomendacaoTipo tipo)
        {
            var query = "select * from conselho_classe_recomendacao where excluido = false and tipo = @tipo";

            return await database.Conexao.QueryAsync<ConselhoClasseRecomendacao>(query, new { tipo });
        }
    }
}