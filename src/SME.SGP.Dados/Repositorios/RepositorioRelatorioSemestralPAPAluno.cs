using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRelatorioSemestralPAPAluno : RepositorioBase<RelatorioSemestralPAPAluno>, IRepositorioRelatorioSemestralPAPAluno
    {
        public RepositorioRelatorioSemestralPAPAluno(ISgpContext database) : base(database)
        {
        }

        public async Task<RelatorioSemestralPAPAluno> ObterCompletoPorIdAsync(long relatorioSemestralAlunoId)
        {
            var query = @"select ra.*, rs.*, s.* from relatorio_semestral_pap_aluno ra 
                    inner join relatorio_semestral_pap_aluno_secao rs on rs.relatorio_semestral_pap_aluno_id = ra.id
                    inner join secao_relatorio_semestral_pap s on s.id = rs.secao_relatorio_semestral_pap_id 
                    where ra.id = @relatorioSemestralAlunoId";

            RelatorioSemestralPAPAluno relatorioAluno = null;
            await database.Conexao.QueryAsync<RelatorioSemestralPAPAluno, RelatorioSemestralPAPAlunoSecao, SecaoRelatorioSemestralPAP, RelatorioSemestralPAPAluno>(
                    query, (relatorioSemestralAluno, relatorioSemestralAlunoSecao, secaoRelatorioSemestral) =>
                    {
                        if (relatorioAluno == null)
                            relatorioAluno = relatorioSemestralAluno;

                        relatorioSemestralAlunoSecao.SecaoRelatorioSemestralPAP = secaoRelatorioSemestral;
                        relatorioSemestralAlunoSecao.RelatorioSemestralPAPAluno = relatorioSemestralAluno;

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
                                 srs.ordem";
            if (relatorioSemestralAlunoId > 0)
            {
                query += " ,rsas.valor";
            }
            query += @" from secao_relatorio_semestral_pap srs ";

            if (relatorioSemestralAlunoId > 0)
            {
                query += @"left join relatorio_semestral_pap_aluno_secao rsas on rsas.secao_relatorio_semestral_pap_id = srs.id  and rsas.relatorio_semestral_pap_aluno_id = @relatorioSemestralAlunoId
                           where srs.inicio_vigencia <= @dataReferencia
                             and srs.fim_vigencia >= @dataReferencia or srs.fim_vigencia is null";
            }

            query += " order by srs.ordem ";

            return await database.Conexao.QueryAsync<RelatorioSemestralAlunoSecaoDto>(query, new { relatorioSemestralAlunoId, dataReferencia });
        }


        public Task<RelatorioSemestralPAPAluno> ObterPorTurmaAlunoAsync(long relatorioSemestralId, string alunoCodigo)
        {
            throw new System.NotImplementedException();
        }

        public async Task<RelatorioSemestralPAPAluno> ObterRelatorioSemestralPorAlunoTurmaSemestreAsync(string alunoCodigo, string turmaCodigo, int semestre)
        {
            var query = @"select rsa.*, rs.*
                  from relatorio_semestral_pap_aluno rsa 
                 inner join relatorio_semestral_turma_pap rs on rs.id = rsa.relatorio_semestral_turma_pap_id 
                 left join turma t on t.id = rs.turma_id
                where not rsa.excluido
                  and rsa.aluno_codigo = @alunoCodigo
                  and t.turma_id  = @turmaCodigo
                  and rs.semestre  = @semestre";

            return (await database.Conexao.QueryAsync<RelatorioSemestralPAPAluno, RelatorioSemestralTurmaPAP, RelatorioSemestralPAPAluno>(query,
                (relatorioSemestralAluno, relatorioSemestralTurma) =>
                {
                    relatorioSemestralAluno.RelatorioSemestralTurmaPAP = relatorioSemestralTurma;
                    return relatorioSemestralAluno;
                },
                new { alunoCodigo, turmaCodigo, semestre })).FirstOrDefault();
        }
        
        public async Task<IEnumerable<RelatorioSemestralPAPAluno>> ObterRelatoriosAlunosPorTurmaAsync(long turmaId, int semestre)
        {
            var query = @"select rsa.* 
                          from relatorio_semestral_turma_pap rs
                         inner join relatorio_semestral_pap_aluno rsa on rsa.relatorio_semestral_turma_pap_id = rs.id
                         where rs.turma_id = @turmaId
                           and rs.semestre = @semestre";

            return await database.Conexao.QueryAsync<RelatorioSemestralPAPAluno>(query, new { turmaId, semestre });
        }
    }
}