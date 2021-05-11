using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioConselhoClasseConsolidado : RepositorioBase<ConselhoClasseConsolidadoComponenteTurma>, IRepositorioConselhoClasseConsolidado
    {
        public RepositorioConselhoClasseConsolidado(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<ConselhoClasseConsolidadoComponenteTurma>> ObterConselhosClasseConsolidadoPorTurmaBimestre(long turmaId, int bimestre)
        {
            var query = $@" select id, dt_atualizacao, status, aluno_codigo, parecer_conclusivo_id, turma_id, bimestre,  
                                   criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, excluido
                            from consolidado_conselho_classe_aluno_turma 
                          where not excluido 
                            and turma_id = @turmaId
                            and bimestre = @bimestre";

            return await database.Conexao.QueryAsync<ConselhoClasseConsolidadoComponenteTurma>(query, new { turmaId, bimestre });
        }
    }
}
