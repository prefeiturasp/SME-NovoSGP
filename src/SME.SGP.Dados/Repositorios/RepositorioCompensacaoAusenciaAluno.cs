﻿using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCompensacaoAusenciaAluno : RepositorioBase<CompensacaoAusenciaAluno>, IRepositorioCompensacaoAusenciaAluno
    {
        public RepositorioCompensacaoAusenciaAluno(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<CompensacaoAusenciaAluno>> ObterCompensacoesAluno(string codigoAluno, long compensacaoIgnoradaId, int bimestre)
        {
            var query = @"select * 
                            from compensacao_ausencia_aluno a
                        inner join compensacao_ausencia c on c.id = a.compensacao_ausencia_id
                        where not a.excluido 
                          and a.codigo_aluno = @codigoAluno
                          and c.bimestre = @bimestre
                          and c.id <> @compensacaoIgnoradaId";

            return await database.Conexao.QueryAsync<CompensacaoAusenciaAluno>(query, new { codigoAluno, compensacaoIgnoradaId, bimestre });
        }

        public async Task<IEnumerable<CompensacaoAusenciaAluno>> ObterPorCompensacao(long compensacaoId)
        {
            var query = @"select * 
                            from compensacao_ausencia_aluno 
                        where not excluido 
                          and compensacao_ausencia_id = @compensacaoId";

            return await database.Conexao.QueryAsync<CompensacaoAusenciaAluno>(query, new { compensacaoId });
        }

        public int ObterTotalCompensacoesPorAlunoETurma(int bimestre, string codigoAluno, string disciplinaId, string turmaId)
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

            return database.Conexao.QueryFirst<int>(query.ToString(), new { bimestre, codigoAluno, disciplinaId, turmaId });
        }
    }
}
