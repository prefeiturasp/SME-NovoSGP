using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioFechamentoConsolidado : IRepositorioFechamentoConsolidado
    {
        private readonly ISgpContext contexto;

        public RepositorioFechamentoConsolidado(ISgpContext contexto)
        {
            this.contexto = contexto;
        }

        public async Task<IEnumerable<FechamentoConsolidadoComponenteTurma>> ObterFechamentosConsolidadoPorTurmaBimestre(long turmaId, int bimestre)
        {
            var query = $@" select id, dt_atualizacao, status, componente_curricular_id, professor_nome, professor_rf, turma_id, bimestre 
                            from consolidado_fechamento_componente_turma 
                          where not excluido 
                            and turma_id = @turmaId
                            and bimestre = @bimestre";

            return await contexto.Conexao.QueryAsync<FechamentoConsolidadoComponenteTurma>(query, new { turmaId, bimestre });
        }

    }
}
