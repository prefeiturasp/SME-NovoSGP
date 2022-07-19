using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasseRecomendacao : RepositorioBase<ConselhoClasseRecomendacao>, IRepositorioConselhoClasseRecomendacao
    {
        public RepositorioConselhoClasseRecomendacao(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<ConselhoClasseRecomendacao>> ObterTodosAsync()
        {
            var query = "select * from conselho_classe_recomendacao where excluido = false";

            return await database.Conexao.QueryAsync<ConselhoClasseRecomendacao>(query);
        }

        public async Task<IEnumerable<RecomendacoesAlunoFamiliaDto>> ObterIdRecomendacoesETipoAsync()
        {
            var query = @"select ccr.id as Id, ccr.recomendacao as Recomendacao, ccr.tipo as Tipo
                            from conselho_classe_recomendacao ccr where ccr.excluido = false";

            return await database.Conexao.QueryAsync<RecomendacoesAlunoFamiliaDto>(query);
        }
    }
}