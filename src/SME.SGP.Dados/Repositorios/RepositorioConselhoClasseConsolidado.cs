using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioConselhoClasseConsolidado : RepositorioBase<ConselhoClasseConsolidadoTurmaAluno>, IRepositorioConselhoClasseConsolidado
    {
        public RepositorioConselhoClasseConsolidado(ISgpContext database) : base(database)
        {
        }


        public async Task<IEnumerable<ConselhoClasseConsolidadoTurmaAluno>> ObterConselhosClasseConsolidadoPorTurmaBimestreAsync(long turmaId, int situacaoConselhoClasse)
        {
            var query = new StringBuilder(@" select id, dt_atualizacao, status, aluno_codigo, parecer_conclusivo_id, turma_id,  
                                   criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, excluido
                            from consolidado_conselho_classe_aluno_turma 
                          where not excluido 
                            and turma_id = @turmaId ");

            if (situacaoConselhoClasse != -99)
                query.AppendLine(@"and EXISTS(select 1 from consolidado_conselho_classe_aluno_turma
                                              where not excluido and turma_id = @turmaId 
                                                and status = @situacaoConselhoClasse)");

            return await database.Conexao.QueryAsync<ConselhoClasseConsolidadoTurmaAluno>(query.ToString(), new { turmaId, situacaoConselhoClasse });
        }
        public Task<ConselhoClasseConsolidadoTurmaAluno> ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoAsync(long turmaId, string alunoCodigo)
        {
            var query = $@" select id, dt_atualizacao, status, aluno_codigo, parecer_conclusivo_id, turma_id,   
                                   criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, excluido           
                            from consolidado_conselho_classe_aluno_turma 
                            where not excluido 
                            and turma_id = @turmaId
                            and aluno_codigo = @alunoCodigo";

            return database.Conexao.QueryFirstOrDefaultAsync<ConselhoClasseConsolidadoTurmaAluno>(query, new { turmaId, alunoCodigo });
        }

        public async Task<IEnumerable<ConsolidacaoConselhoClasseAlunoMigracaoDto>> ObterConselhoClasseConsolidadoPorTurmaAsync(long turmaId)
        {
            var query = @"select id as ConsolidacaoId, 
                        turma_id as TurmaId,
                        aluno_codigo as AlunoCodigo
                        from consolidado_conselho_classe_aluno_turma 
                        where not excluido 
                        and turma_id = @turmaId";

            return await database.Conexao.QueryAsync<ConsolidacaoConselhoClasseAlunoMigracaoDto>(query, new { turmaId });
        }
    }
}
