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


        public async Task<IEnumerable<ConselhoClasseConsolidadoTurmaAluno>> ObterConselhosClasseConsolidadoPorTurmaBimestreAsync(long turmaId, int bimestre, int situacaoConselhoClasse)
        {
            var query = new StringBuilder(@" select id, dt_atualizacao, status, aluno_codigo, parecer_conclusivo_id, turma_id, bimestre,  
                                   criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, excluido
                            from consolidado_conselho_classe_aluno_turma 
                          where not excluido 
                            and turma_id = @turmaId
                            and bimestre = @bimestre ");

            if (situacaoConselhoClasse != -99)
                query.AppendLine(@"and EXISTS(select 1 from consolidado_conselho_classe_aluno_turma
                                              where not excluido and turma_id = @turmaId 
                                                and bimestre = @bimestre and status = @situacaoConselhoClasse)");

            return await database.Conexao.QueryAsync<ConselhoClasseConsolidadoTurmaAluno>(query.ToString(), new { turmaId, bimestre, situacaoConselhoClasse });
        }
        public async Task<ConselhoClasseConsolidadoTurmaAluno> ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoAsync(long turmaId, int bimestre, string alunoCodigo)
        {
            var query = $@" select id, dt_atualizacao, status, aluno_codigo, parecer_conclusivo_id, turma_id, bimestre,  
                                   criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, excluido
                            from consolidado_conselho_classe_aluno_turma 
                          where not excluido 
                            and turma_id = @turmaId
                            and bimestre = @bimestre 
                            and aluno_codigo = @alunoCodigo";

            return await database.Conexao.QueryFirstOrDefaultAsync<ConselhoClasseConsolidadoTurmaAluno>(query, new { turmaId, bimestre, alunoCodigo });
        }     
    }
}
