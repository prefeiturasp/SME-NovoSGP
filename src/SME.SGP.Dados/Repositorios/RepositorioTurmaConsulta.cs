using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTurmaConsulta : IRepositorioTurmaConsulta
    {
        private readonly ISgpContextConsultas contexto;

        public RepositorioTurmaConsulta(ISgpContextConsultas contexto)
        {
            this.contexto = contexto;
        }

        public async Task<Turma> ObterPorCodigo(string turmaCodigo)
        {
            return await contexto.Conexao.QueryFirstOrDefaultAsync<Turma>("select * from turma where turma_id = @turmaCodigo", new { turmaCodigo });
        }

        public async Task<string> ObterTurmaCodigoPorConselhoClasseId(long conselhoClasseId)
        {
            var query = @"select t.turma_id
                          from conselho_classe cc
                          inner join fechamento_turma ft on ft.id = cc.fechamento_turma_id
                          inner join turma t on t.id = ft.turma_id
                         where not cc.excluido and not ft.excluido
                           and cc.id = @conselhoClasseId";

            return await contexto.Conexao.QueryFirstOrDefaultAsync<string>(query, new { conselhoClasseId });
        }
    }
}
