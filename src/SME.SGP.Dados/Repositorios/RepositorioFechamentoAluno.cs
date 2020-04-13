using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamentoAluno : RepositorioBase<FechamentoAluno>, IRepositorioFechamentoAluno
    {
        public RepositorioFechamentoAluno(ISgpContext conexao): base(conexao)
        {
        }

        public async Task<FechamentoAluno> ObterFechamentoAluno(long fechamentoTurmaDisciplinaId, string alunoCodigo)
        {
            var query = @"select * 
                        from fechamento_aluno
                        where not excluido 
                          and fechamento_turma_disciplina_id = @fechamentoTurmaDisciplinaId
                          and aluno_codigo = @alunoCodigo";

            return await database.Conexao.QueryFirstOrDefaultAsync<FechamentoAluno>(query, new { fechamentoTurmaDisciplinaId, alunoCodigo });
        }

        public async Task<FechamentoAluno> ObterFechamentoAlunoENotas(long fechamentoTurmaDisciplinaId, string alunoCodigo)
        {
            var query = @"select a.*, n.*
                            from fechamento_aluno a
                           inner join fechamento_nota n on n.fechamento_aluno_id = a.id
                           where not a.excluido
                             and a.fechamento_turma_disciplina_id = @fechamentoTurmaDisciplinaId
                             and a.aluno_codigo = @alunoCodigo";

            return (await database.Conexao.QueryAsync<FechamentoAluno, FechamentoNota, FechamentoAluno>(query
                , (fechamentoAluno, fechamentoNota) =>
                {
                    fechamentoAluno.FechamentoNotas.Add(fechamentoNota);
                    return fechamentoAluno;
                }
                , new { fechamentoTurmaDisciplinaId , alunoCodigo })).FirstOrDefault();
        }

        public async Task<IEnumerable<FechamentoAluno>> ObterPorFechamentoTurmaDisciplina(long fechamentoTurmaDisciplinaId)
        {
            var query = @"select fa.*, n.*
                            from fechamento_aluno fa
                           inner join fechamento_nota n on n.fechamento_aluno_id = fa.id
                           where fa.fechamento_turma_disciplina_id = @fechamentoTurmaDisciplinaId";

            return await database.Conexao.QueryAsync<FechamentoAluno, FechamentoNota, FechamentoAluno>(query
                , (fechamentoAluno, fechamentoNota) =>
                {
                    fechamentoAluno.FechamentoNotas.Add(fechamentoNota);
                    return fechamentoAluno;
                }
                , new { fechamentoTurmaDisciplinaId });

        }
    }
}
