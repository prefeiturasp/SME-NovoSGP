using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamentoNota : RepositorioBase<FechamentoNota>, IRepositorioFechamentoNota
    {
        const string queryPorFechamento = @"select n.*, fa.* 
                                             from fechamento_nota n
                                            inner join fechamento_aluno fa on fa.id = n.fechamento_aluno_id
                                            where fa.fechamento_turma_disciplina_id = @fechamentoTurmaDisciplinaId";
        public RepositorioFechamentoNota(ISgpContext database) : base(database)
        {
        }

        public async Task<FechamentoNota> ObterPorAlunoEFechamento(long fechamentoTurmaDisciplinaId, string alunoCodigo)
        {
            var query = queryPorFechamento + " and aluno_codigo = @alunoCodigo";

            var consultaFechamento = await database.Conexao.QueryAsync<FechamentoNota, FechamentoAluno, FechamentoNota>(query
                , (fechamentoNota, fechamentoAluno) =>
                {
                    fechamentoNota.FechamentoAluno = fechamentoAluno;
                    return fechamentoNota;
                }
                , new { fechamentoTurmaDisciplinaId, alunoCodigo });

            return consultaFechamento.FirstOrDefault();
        }

        public async Task<IEnumerable<FechamentoNota>> ObterPorFechamentoTurma(long fechamentoTurmaDisciplinaId)
        {
            return await database.Conexao.QueryAsync<FechamentoNota, FechamentoAluno, FechamentoNota>(queryPorFechamento
                , (fechamentoNota, fechamentoAluno) => 
                {
                    fechamentoNota.FechamentoAluno = fechamentoAluno;
                    return fechamentoNota;
                }     
                , new { fechamentoTurmaDisciplinaId });
        }
    }
}