using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRelatorioSemestralAluno : RepositorioBase<RelatorioSemestralAluno>, IRepositorioRelatorioSemestralAluno
    {
        public RepositorioRelatorioSemestralAluno(ISgpContext database) : base(database)
        {
        }

        public async Task<RelatorioSemestralAluno> ObterCompletoPorIdAsync(long relatorioSemestralAlunoId)
        {
            var query = @"select ra.*, rs.*, s.* from relatorio_semestral_aluno ra 
                    inner join relatorio_semestral_aluno_secao rs on rs.relatorio_semestral_aluno_id = ra.id
                    inner join secao_relatorio_semestral s on s.id = rs.secao_relatorio_semestral_id 
                    where ra.id = @relatorioSemestralAlunoId";

            RelatorioSemestralAluno relatorioAluno = null;
            await database.Conexao.QueryAsync<RelatorioSemestralAluno, RelatorioSemestralAlunoSecao, SecaoRelatorioSemestral, RelatorioSemestralAluno>(
                    query, (relatorioSemestralAluno, relatorioSemestralAlunoSecao, secaoRelatorioSemestral) =>
                    {
                        if (relatorioAluno == null)
                            relatorioAluno = relatorioSemestralAluno;

                        relatorioSemestralAlunoSecao.SecaoRelatorioSemestral = secaoRelatorioSemestral;
                        relatorioSemestralAlunoSecao.RelatorioSemestralAluno = relatorioSemestralAluno;

                        relatorioAluno.Secoes.Add(relatorioSemestralAlunoSecao);

                        return relatorioSemestralAluno;
                    }, new { relatorioSemestralAlunoId });

            return relatorioAluno;
        }

        public async Task<IEnumerable<RelatorioSemestralAlunoSecaoDto>> ObterDadosSecaoPorRelatorioSemestralAlunoIdDataReferenciaAsync(long relatorioSemestralAlunoId, DateTime dataReferencia)
        {
            var query = @"select srs.id, 
	                             srs.nome, 
	                             srs.descricao, 
	                             srs.obrigatorio,
	                             rsas.valor,
                                 srs.ordem
                            from secao_relatorio_semestral srs
                            left join relatorio_semestral_aluno_secao rsas on rsas.secao_relatorio_semestral_id = srs.id
                           where srs.inicio_vigencia <= @dataReferencia 
                             and srs.fim_vigencia >= @dataReferencia or srs.fim_vigencia is null";

            if (relatorioSemestralAlunoId > 0)
            {
                query += " and rsas.relatorio_semestral_aluno_id = @relatorioSemestralAlunoId";
            }

            query += " order by srs.ordem ";

            return await database.Conexao.QueryAsync<RelatorioSemestralAlunoSecaoDto>(query, new { relatorioSemestralAlunoId, dataReferencia });
        }


        public Task<RelatorioSemestralAluno> ObterPorTurmaAlunoAsync(long relatorioSemestralId, string alunoCodigo)
        {
            throw new System.NotImplementedException();
        }

        public async Task<RelatorioSemestralAluno> ObterRelatorioSemestralPorAlunoTurmaSemestreAsync(string alunoCodigo, string turmaCodigo, int semestre)
        {
            var query = @"select rsa.relatorio_semestral_id,
	                   rsa.id,
	                   rsa.criado_em,
	                   rsa.criado_por,
	                   rsa.criado_rf,
	                   rsa.alterado_em,
	                   rsa.alterado_por
                  from relatorio_semestral_aluno rsa 
                 inner join relatorio_semestral rs on rs.id = rsa.relatorio_semestral_id 
                 left join turma t on t.id = rs.turma_id
                where not rsa.excluido
                  and rsa.aluno_codigo = @alunoCodigo
                  and t.turma_id  = @turmaCodigo
                  and rs.semestre  = @semestre";
            return await database.Conexao.QueryFirstOrDefaultAsync<RelatorioSemestralAluno>(query, new { alunoCodigo, turmaCodigo, semestre});
        }
        
        public async Task<IEnumerable<RelatorioSemestralAluno>> ObterRelatoriosAlunosPorTurmaAsync(long turmaId, int semestre)
        {
            var query = @"select rsa.* 
                          from relatorio_semestral rs
                         inner join relatorio_semestral_aluno rsa on rsa.relatorio_semestral_id = rs.id
                         where rs.turma_id = @turmaId
                           and rs.semestre = @semestre";

            return await database.Conexao.QueryAsync<RelatorioSemestralAluno>(query, new { turmaId, semestre });
        }
    }
}