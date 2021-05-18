using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioFechamentoConsolidado : RepositorioBase<FechamentoConsolidadoComponenteTurma>, IRepositorioFechamentoConsolidado
    {
        public RepositorioFechamentoConsolidado(ISgpContext database) : base(database)
        {
        }

        public async Task<FechamentoConsolidadoComponenteTurma> ObterFechamentoConsolidadoPorTurmaBimestreComponenteCurricularAsync(long turmaId, long componenteCurricularId, int bimestre)
        {
            var query = $@" select id, dt_atualizacao, status, componente_curricular_id, professor_nome, professor_rf, 
                                   turma_id, bimestre, criado_em, criado_por, alterado_em, alterado_por, criado_rf, 
                                   alterado_rf, excluido
                            from public.consolidado_fechamento_componente_turma 
                          where not excluido 
                            and turma_id = @turmaId
                            and bimestre = @bimestre 
                            and componente_curricular_id = @componenteCurricularId";

            return await database.Conexao.QueryFirstOrDefaultAsync<FechamentoConsolidadoComponenteTurma>(query, new { turmaId, componenteCurricularId, bimestre });
        }

        public async Task<IEnumerable<FechamentoConsolidadoComponenteTurma>> ObterFechamentosConsolidadoPorTurmaBimestre(long turmaId, int bimestre)
        {
            var query = $@" select id, dt_atualizacao, status, componente_curricular_id, professor_nome, professor_rf, turma_id, bimestre, 
                                   criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, excluido
                            from consolidado_fechamento_componente_turma 
                          where not excluido 
                            and turma_id = @turmaId
                            and bimestre = @bimestre";

            return await database.Conexao.QueryAsync<FechamentoConsolidadoComponenteTurma>(query, new { turmaId, bimestre });
        }

    }
}
