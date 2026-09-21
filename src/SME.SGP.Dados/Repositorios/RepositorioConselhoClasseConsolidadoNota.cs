using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioConselhoClasseConsolidadoNota : IRepositorioConselhoClasseConsolidadoNota
    {
        protected readonly ISgpContext database;

        public RepositorioConselhoClasseConsolidadoNota(ISgpContext database)
        {
            this.database = database;
        }

        public Task<ConselhoClasseConsolidadoTurmaAlunoNota> ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoNotaAsync(long consolidadoTurmaAlunoId, int? bimestre, long? componenteCurricularId)
        {
            var query = $@" {CAMPOS_TABELA_CONSOLIDADO_CONSELHO_CLASSE_ALUNO_TURMA_NOTA}    
                            from consolidado_conselho_classe_aluno_turma_nota
                            where consolidado_conselho_classe_aluno_turma_id = @consolidadoTurmaAlunoId ";

            query += " and coalesce(bimestre, 0) = @bimestre ";
            if (componenteCurricularId.HasValue)
                query += " and componente_curricular_id = @componenteCurricularId";
            query += " order by id desc ";
            return database.Conexao.QueryFirstOrDefaultAsync<ConselhoClasseConsolidadoTurmaAlunoNota>(query, new { consolidadoTurmaAlunoId, bimestre = bimestre ?? 0, componenteCurricularId });
        }

        public Task<ConselhoClasseConsolidadoTurmaAlunoNota> ObterConselhoClasseConsolidadoAlunoNotaPorConsolidadoBimestreDisciplinaAsync(long consolidacaoId, int bimestre, long disciplinaId)
        {
            var query = $@" {CAMPOS_TABELA_CONSOLIDADO_CONSELHO_CLASSE_ALUNO_TURMA_NOTA}   
                            from consolidado_conselho_classe_aluno_turma_nota
                            where consolidado_conselho_classe_aluno_turma_id = @consolidacaoId 
                                  and coalesce(bimestre, 0) = @bimestre
                                  and componente_curricular_id = @disciplinaId";

            return database.Conexao.QueryFirstOrDefaultAsync<ConselhoClasseConsolidadoTurmaAlunoNota>(query, new { consolidacaoId, bimestre, disciplinaId });
        }

        public async Task<long> SalvarAsync(ConselhoClasseConsolidadoTurmaAlunoNota consolidadoNota)
        {
            
            if (consolidadoNota.Id > 0)
            {
                var sucesso = await database.Conexao.UpdateMappedAsync(consolidadoNota);
                return sucesso > 0 ? consolidadoNota.Id : 0;
            }
            else
                return (await database.Conexao.InsertMappedAsync(consolidadoNota));
        }

        public async Task<bool> ExcluirConsolidacaoConselhoClasseNotaPorIdsConsolidacaoAlunoEBimestre(long[] idsConsolidacao)
        {
            var query = $@"delete from consolidado_conselho_classe_aluno_turma_nota where id = ANY(@idsConsolidacao)";

            return await database.Conexao.ExecuteScalarAsync<bool>(query, new { idsConsolidacao});
        }

        public async Task<IEnumerable<long>> ObterConsolidacoesConselhoClasseNotaIdsPorConsolidacoesAlunoTurmaIds(long[] consolidacoesAlunoTurmaIds, int bimestre = 0)
        {
            var query = $@"select id from consolidado_conselho_classe_aluno_turma_nota where consolidado_conselho_classe_aluno_turma_id = ANY(@consolidacoesAlunoTurmaIds)";

            if (bimestre > 0)
                query += $@" and coalesce(bimestre, 0) = @bimestre";

            return await database.Conexao.QueryAsync<long>(query, new { consolidacoesAlunoTurmaIds, bimestre });
        }

        private const string CAMPOS_TABELA_CONSOLIDADO_CONSELHO_CLASSE_ALUNO_TURMA_NOTA = @"select
	                                            id as Id,
	                                            consolidado_conselho_classe_aluno_turma_id as ConselhoClasseConsolidadoTurmaAlunoId,
	                                            bimestre as Bimestre,
	                                            nota as Nota,
	                                            conceito_id as ConceitoId,
	                                            componente_curricular_id as ComponenteCurricularId";
    }
}
