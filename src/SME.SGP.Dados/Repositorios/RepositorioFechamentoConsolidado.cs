﻿using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioFechamentoConsolidado : RepositorioBase<FechamentoConsolidadoComponenteTurma>, IRepositorioFechamentoConsolidado
    {
        public RepositorioFechamentoConsolidado(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<FechamentoConsolidadoComponenteTurma> ObterFechamentoConsolidadoPorTurmaBimestreComponenteCurricularAsync(long turmaId, long componenteCurricularId, int? bimestre)
        {
            var query = $@"select id, dt_atualizacao, status, componente_curricular_id, professor_nome, professor_rf, 
                                   turma_id, bimestre, criado_em, criado_por, alterado_em, alterado_por, criado_rf, 
                                   alterado_rf, excluido
                            from public.consolidado_fechamento_componente_turma 
                          where not excluido 
                            and turma_id = @turmaId
                            and bimestre {(bimestre.HasValue ? "= @bimestre " : "is null ")}
                            and componente_curricular_id = @componenteCurricularId";

            return await database.Conexao.QueryFirstOrDefaultAsync<FechamentoConsolidadoComponenteTurma>(query, new { turmaId, componenteCurricularId, bimestre });
        }

        public async Task<IEnumerable<FechamentoConsolidadoComponenteTurma>> ObterFechamentosConsolidadoPorTurmaBimestre(long turmaId, int bimestre, int[] situacoesFechamento)
        {
            var query = new StringBuilder(@" select * from (
                                                select id, dt_atualizacao, status, componente_curricular_id, professor_nome, professor_rf, turma_id, bimestre, 
                                                     criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, excluido,
                                                row_number() over (partition by professor_rf) sequencia
                                                from consolidado_fechamento_componente_turma 
                                               where not excluido 
                                                 and turma_id = @turmaId
                                                 and bimestre = @bimestre ");
            if (!situacoesFechamento.Any(c => c == -99))
                query.AppendLine(@"and EXISTS(select 1 from consolidado_fechamento_componente_turma 
                                              where turma_id = @turmaId and bimestre = @bimestre and status = ANY(@situacoesFechamento))");

            query.AppendLine(") x where x.sequencia = 1");
            
            return await database.Conexao.QueryAsync<FechamentoConsolidadoComponenteTurma>(query.ToString(), new { turmaId, bimestre, situacoesFechamento });
        }
        public async Task<IEnumerable<ConsolidacaoTurmaComponenteCurricularDto>> ObterComponentesFechamentoConsolidadoPorTurmaBimestre(long turmaId, int bimestre, int[] situacoesFechamento)
        {
            var query = new StringBuilder(@" select * from (
                                               select cc.id,
                                                    coalesce(cc.descricao_sgp, cc.descricao) as descricao,
                                                    cfct.professor_nome as professorNome,
                                                    cfct.professor_rf as professorRf,
                                                    cfct.status as SituacaoFechamentoCodigo, 
                                                    cc.grupo_matriz_id as GrupoMatrizId,
                                                    cc.area_conhecimento_id as AreaConnhecimentoId,
                                                    row_number() over (partition by cc.id) sequencia
                                               from consolidado_fechamento_componente_turma cfct   
                                               inner join componente_curricular cc on cc.id = cfct.componente_curricular_id           
                                               where cfct.turma_id = @turmaId
                                                 and cfct.bimestre = @bimestre  ");                             

            if (!situacoesFechamento.Any(c => c == -99))
                query.AppendLine(@"and EXISTS(select 1 from consolidado_fechamento_componente_turma 
                                              where turma_id = @turmaId and bimestre = @bimestre and status = ANY(@situacoesFechamento)) ");
            
            query.AppendLine(") x where x.sequencia = 1");

            return await database.Conexao.QueryAsync<ConsolidacaoTurmaComponenteCurricularDto>(query.ToString(), new { turmaId, bimestre, situacoesFechamento });
        }

        public async Task ExcluirConsolidacaoPorTurmaIdEBimestre(long turmaId, int bimestre)
        {
            var query = @"delete from consolidado_fechamento_componente_turma where turma_id = @turmaId and bimestre = @bimestre";

            await database.Conexao.ExecuteScalarAsync(query, new { turmaId, bimestre });
        }


    }
}
