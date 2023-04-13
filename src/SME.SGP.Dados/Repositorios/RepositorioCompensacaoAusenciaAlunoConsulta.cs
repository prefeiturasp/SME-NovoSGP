using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCompensacaoAusenciaAlunoConsulta : RepositorioBase<CompensacaoAusenciaAluno>, IRepositorioCompensacaoAusenciaAlunoConsulta
    {
        public RepositorioCompensacaoAusenciaAlunoConsulta(ISgpContextConsultas database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<CompensacaoAusenciaAluno>> ObterPorCompensacao(long compensacaoId)
        {
            var query = @"select * 
                            from compensacao_ausencia_aluno 
                        where not excluido 
                          and compensacao_ausencia_id = @compensacaoId";

            return await database.Conexao.QueryAsync<CompensacaoAusenciaAluno>(query, new { compensacaoId });
        }

        private String BuildQueryObterTotalCompensacoesPorAlunoETurma(string disciplinaId)
        {
            var query = new StringBuilder(@"select coalesce(sum(a.qtd_faltas_compensadas), 0)
                                from compensacao_ausencia_aluno a
                                inner join compensacao_ausencia c on c.id = a.compensacao_ausencia_id
                                inner join turma t on t.id = c.turma_id
                                where not a.excluido 
                                    and c.bimestre = @bimestre
                                    and a.codigo_aluno = @codigoAluno
                                    and t.turma_id = @turmaId ");
            
            if (!string.IsNullOrEmpty(disciplinaId))
                query.Append("and c.disciplina_id = @disciplinaId");
            
            return query.ToString();
        }

        private String BuildQueryObterTotalCompensacoesECompensacaoAlunoIdPorAlunoETurma(string disciplinaId)
        {
            var query = new StringBuilder(@"select a.id as CompensacaoAlunoId, coalesce(a.qtd_faltas_compensadas, 0) as Quantidade
                                from compensacao_ausencia_aluno a
                                inner join compensacao_ausencia c on c.id = a.compensacao_ausencia_id
                                inner join turma t on t.id = c.turma_id
                                where not a.excluido 
                                    and c.bimestre = @bimestre
                                    and a.codigo_aluno = @codigoAluno
                                    and t.turma_id = @turmaId ");

            if (!string.IsNullOrEmpty(disciplinaId))
                query.Append("and c.disciplina_id = @disciplinaId");

            return query.ToString();
        }

        public async Task<int> ObterTotalCompensacoesPorAlunoETurmaAsync(int bimestre, string codigoAluno, string disciplinaId, string turmaId)
        {
            var query = BuildQueryObterTotalCompensacoesPorAlunoETurma(disciplinaId);
            return await database.Conexao.QueryFirstAsync<int>(query.ToString(), new { bimestre, codigoAluno, disciplinaId, turmaId });
        }

        public async Task<TotalCompensacaoAlunoPorCompensacaoIdDto> ObterTotalCompensacoesECompensacaoIdPorAlunoETurmaAsync(int bimestre, string codigoAluno, string disciplinaId, string turmaId)
        {
            var query = BuildQueryObterTotalCompensacoesECompensacaoAlunoIdPorAlunoETurma(disciplinaId);
            return await database.Conexao.QueryFirstOrDefaultAsync<TotalCompensacaoAlunoPorCompensacaoIdDto>(query.ToString(), new { bimestre, codigoAluno, disciplinaId, turmaId });
        }

        public async Task<IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto>> ObterTotalCompensacoesPorAlunosETurmaAsync(int bimestre, List<string> alunoCodigos, string turmaCodigo)
        {
            var query = @"
                select
	                coalesce(sum(caa.qtd_faltas_compensadas), 0) as compensacoes,
	                caa.codigo_aluno as alunoCodigo,
	                c.disciplina_id as componenteCurricularId,
	                c.bimestre
                from
	                compensacao_ausencia_aluno caa
                inner join compensacao_ausencia c on
	                c.id = caa.compensacao_ausencia_id
                inner join turma t on
	                t.id = c.turma_id
                where
	                not caa.excluido
                    and not c.excluido 
	                and c.bimestre = @bimestre
	                and caa.codigo_aluno = any(@alunoCodigos)
	                and t.turma_id = @turmaCodigo
                group by
	                caa.codigo_aluno,
	                c.disciplina_id,
	                c.bimestre";

            return await database.Conexao.QueryAsync<CompensacaoAusenciaAlunoCalculoFrequenciaDto>(query, new { bimestre, alunoCodigos, turmaCodigo });
        }

        public async Task<IEnumerable<CompensacaoAusenciaAlunoEDataDto>> ObterCompensacaoAusenciaAlunoEAulaPorAulaId(long aulaId)
        {
            var query = @"select caaa.compensacao_ausencia_aluno_id CompensacaoAusenciaAlunoId, 
                                 caaa.id CompensacaoAusenciaAlunoAulaId, 
                                 caaa.registro_frequencia_aluno_id RegistroFrequenciaAlunoId
                          from compensacao_ausencia_aluno caa 
                            join compensacao_ausencia_aluno_aula caaa on caa.id = caaa.compensacao_ausencia_aluno_id
                            join registro_frequencia_aluno rfa on rfa.id = caaa.registro_frequencia_aluno_id
                          where rfa.aula_id = @aulaId
                                and not caa.excluido   
                                and not caaa.excluido";

            return await database.Conexao.QueryAsync<CompensacaoAusenciaAlunoEDataDto>(query, new { aulaId });
        }

        public async Task<IEnumerable<CompensacaoAusenciaAlunoEDataDto>> ObterCompensacoesAusenciasAlunosPorRegistroFrequenciaAlunoIdsQuery(IEnumerable<long> registroFrequenciaAlunoIds)
        {
            var query = @"select caa.id CompensacaoAusenciaAlunoId,
                                 caa.qtd_faltas_compensadas QuantidadeCompensacoes,
                                 caa.compensacao_ausencia_id CompensacaoAusenciaId,
                                 count(caaa.registro_frequencia_aluno_id) as QuantidadeRegistrosFrequenciaAluno
	                        from compensacao_ausencia_aluno caa
	                        join compensacao_ausencia_aluno_aula caaa on caa.id = caaa.compensacao_ausencia_aluno_id
	                        where caaa.registro_frequencia_aluno_id = ANY(@registroFrequenciaAlunoIds)
                                and not caa.excluido   
                                and not caaa.excluido
                            group by caa.id,caa.qtd_faltas_compensadas, caa.compensacao_ausencia_id  ";

            return await database.Conexao.QueryAsync<CompensacaoAusenciaAlunoEDataDto>(query, new { registroFrequenciaAlunoIds });
        }

        public async Task<IEnumerable<CompensacaoAusenciaAluno>> ObterPorIdsAsync(long[] ids)
        {
            var query = @"select * 
                          from compensacao_ausencia_aluno 
                          where not excluido 
                            and id = any(@ids)";

            return await database.Conexao.QueryAsync<CompensacaoAusenciaAluno>(query, new { ids });
        }
    }
}
