using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioCompensacaoAusencia : RepositorioBase<CompensacaoAusencia>, IRepositorioCompensacaoAusencia
    {
        public RepositorioCompensacaoAusencia(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<CompensacaoAusencia>> Listar(string turmaId, string disciplinaId, int bimestre, string nomeAtividade)
        {
            var query = new StringBuilder(@"select c.id, c.bimestre, c.nome, a.id, a.codigo_aluno, a.qtd_faltas_compensadas, a.notificado
                          from compensacao_ausencia c
                         inner join turma t on t.id = c.turma_id
                          left join compensacao_ausencia_aluno a on a.compensacao_ausencia_id = c.id
                          where not c.excluido
                            and t.turma_id = @turmaId");

            if (!string.IsNullOrEmpty(disciplinaId))
                query.AppendLine("and c.disciplina_id = @disciplinaId");
            if (bimestre != 0)
                query.AppendLine("and c.bimestre = @bimestre");
            if (!string.IsNullOrEmpty(nomeAtividade))
                query.AppendLine("and c.nome like '%@nomeAtividade%'");

            var compensacoes = new List<CompensacaoAusencia>();
            database.Conexao.Query<CompensacaoAusencia, CompensacaoAusenciaAluno, CompensacaoAusencia>(query.ToString(),
                (compensacao, aluno) =>
                {
                    if (aluno == null)
                        compensacoes.Add(compensacao);
                    else
                    {
                        CompensacaoAusencia comp = compensacoes.Find(c => c.Id == compensacao.Id);
                        if (comp != null)
                            comp.Alunos.Add(aluno);
                        else
                        {
                            compensacao.Alunos.Add(aluno);
                            compensacoes.Add(compensacao);
                        }
                    }

                    return compensacao;
                }, new { turmaId, disciplinaId, bimestre, nomeAtividade },
                splitOn: "id, id");

            return compensacoes;
        }

        public async Task<CompensacaoAusencia> ObterPorAnoTurmaENome(int anoLetivo, long turmaId, string nome, long idIgnorar)
        {
            var query = @"select * 
                            from compensacao_ausencia c
                          where not excluido
                            and ano_letivo = @anoLetivo
                            and turma_id = @turmaId
                            and nome = @nome
                            and id <> @idIgnorar";

            return await database.Conexao.QueryFirstOrDefaultAsync<CompensacaoAusencia>(query, new { anoLetivo, turmaId, nome, idIgnorar });
        }
    }
}
