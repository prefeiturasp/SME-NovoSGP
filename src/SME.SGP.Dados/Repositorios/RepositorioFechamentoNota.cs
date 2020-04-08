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

        public async Task<IEnumerable<WfAprovacaoNotaFechamento>> ObterNotasEmAprovacaoPorFechamento(long fechamentoTurmaDisciplinaId)
        {
            var query = @"select w.*
                            from wf_aprovacao_nota_fechamento w
                          inner join fechamento_nota n on n.id = w.fechamento_nota_id 
                          inner join fechamento_aluno a on a.id = n.fechamento_aluno_id
                          where a.fechamento_turma_disciplina_id = @fechamentoTurmaDisciplinaId";

            return await database.Conexao.QueryAsync<WfAprovacaoNotaFechamento>(query, new { fechamentoTurmaDisciplinaId });
        }

        public async Task<IEnumerable<WfAprovacaoNotaFechamento>> ObterNotasEmAprovacaoWf(long workFlowId)
        {
            var query = @"select w.*, n.*, a.*, d.*, f.*, e.*
                            from wf_aprovacao_nota_fechamento w
                          inner join fechamento_nota n on n.id = w.fechamento_nota_id 
                          inner join fechamento_aluno a on a.id = n.fechamento_aluno_id
                          inner join fechamento_turma_disciplina d on d.id = a.fechamento_turma_disciplina_id
                          inner join fechamento_turma f on f.id = d.fechamento_turma_id
                          inner join periodo_escolar e on e.id = f.periodo_escolar_id
                          where w.wf_aprovacao_id = @workFlowId";

            return await database.Conexao.QueryAsync<WfAprovacaoNotaFechamento, FechamentoNota, FechamentoAluno, FechamentoTurmaDisciplina
                                    , FechamentoTurma, PeriodoEscolar, WfAprovacaoNotaFechamento>(query
                , (wfAprovacaoNota, fechamentoNota, fechamentoAluno, fechamentoTurmaDisciplina, fechamentoTurma, periodoEscolar) =>
                {
                    fechamentoTurma.PeriodoEscolar = periodoEscolar;
                    fechamentoTurmaDisciplina.FechamentoTurma = fechamentoTurma;
                    fechamentoAluno.FechamentoTurmaDisciplina = fechamentoTurmaDisciplina;
                    fechamentoNota.FechamentoAluno = fechamentoAluno;
                    wfAprovacaoNota.FechamentoNota = fechamentoNota;
                    return wfAprovacaoNota;
                }
                , new { workFlowId });
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