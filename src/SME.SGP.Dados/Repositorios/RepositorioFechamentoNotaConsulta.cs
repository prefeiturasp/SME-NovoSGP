﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamentoNotaConsulta : RepositorioBase<FechamentoNota>, IRepositorioFechamentoNotaConsulta
    {
        const string queryPorFechamento = @"select n.*, fa.* 
                                             from fechamento_nota n
                                            inner join fechamento_aluno fa on fa.id = n.fechamento_aluno_id
                                            where fa.fechamento_turma_disciplina_id = ANY(@fechamentosTurmaDisciplinaId)";

        const string queryNotasFechamento = @"select fn.disciplina_id as ComponenteCurricularCodigo, fn.conceito_id as ConceitoId, fn.nota, pe.bimestre 
                          from fechamento_turma ft
                         inner join turma t on t.id = ft.turma_id 
                          left join periodo_escolar pe on pe.id = ft.periodo_escolar_id 
                         inner join fechamento_turma_disciplina ftd on ftd.fechamento_turma_id = ft.id
                         inner join fechamento_aluno fa on fa.fechamento_turma_disciplina_id = ftd.id
                         inner join fechamento_nota fn on fn.fechamento_aluno_id = fa.id
                         inner join componente_curricular cc on cc.id = fn.disciplina_id
                         where cc.permite_lancamento_nota 
                         ";

        public RepositorioFechamentoNotaConsulta(ISgpContextConsultas database) : base(database)
        {
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasFinaisAlunoAsync(string[] turmasCodigos, string alunoCodigo)
        {
            var query = $@"{ queryNotasFechamento}
                            and t.turma_id = ANY(@turmasCodigos) 
                            and fa.aluno_codigo = @alunoCodigo
                            and pe.id is null";

            return await database.Conexao.QueryAsync<NotaConceitoBimestreComponenteDto>(query, new { turmasCodigos, alunoCodigo });
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoAnoAsync(string turmaCodigo, string alunoCodigo)
        {
            var query = $@"{queryNotasFechamento}
                           and t.turma_id = @turmaCodigo
                           and fa.aluno_codigo = @alunoCodigo";

            return await database.Conexao.QueryAsync<NotaConceitoBimestreComponenteDto>(query, new { turmaCodigo, alunoCodigo });
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoBimestreAsync(long fechamentoTurmaId, string alunoCodigo)
        {
            var query = $@"{queryNotasFechamento}
                           and ftd.fechamento_turma_id = @fechamentoTurmaId
                           and fa.aluno_codigo = @alunoCodigo";

            return await database.Conexao.QueryAsync<NotaConceitoBimestreComponenteDto>(query, new { fechamentoTurmaId, alunoCodigo });
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoPorTurmasCodigosBimestreAsync(string[] turmasCodigos, string alunoCodigo, int bimestre)
        {
            var query = $@"{queryNotasFechamento}
                           and t.turma_id = ANY(@turmasCodigos)
                           and fa.aluno_codigo = @alunoCodigo 
                           and pe.bimestre = @bimestre";

            return await database.Conexao.QueryAsync<NotaConceitoBimestreComponenteDto>(query, new { turmasCodigos, alunoCodigo, bimestre });
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
                , new { fechamentosTurmaDisciplinaId = new long[] { fechamentoTurmaDisciplinaId } });
        }

        public async Task<IEnumerable<FechamentoNota>> ObterPorFechamentosTurma(long[] fechamentosTurmaDisciplinaId)
        {
            return await database.Conexao.QueryAsync<FechamentoNota, FechamentoAluno, FechamentoNota>(queryPorFechamento
                , (fechamentoNota, fechamentoAluno) =>
                {
                    fechamentoNota.FechamentoAluno = fechamentoAluno;
                    return fechamentoNota;
                }
                , new { fechamentosTurmaDisciplinaId });
        }

        public async Task<IEnumerable<AlunosFechamentoNotaDto>> ObterComNotaLancadaPorPeriodoEscolarUE(long ueId, long periodoEscolarId)
        {
            var query = @"select distinct 
	                            ftd.disciplina_id as ComponenteCurricularId,
	                            cc.descricao as ComponenteCurricularDescricao,
	                            nota as Nota,
	                            cv.valor as NotaConceito,
                                cv.aprovado as NotaConceitoAprovado,
                                case when nota is not null then false else true end as EhConceito,
	                            fa.aluno_codigo as AlunoCodigo,
                                ftd.criado_rf as ProfessorRf,
                                ftd.criado_por as ProfessorNome,
	                            ftd.justificativa as Justificativa,
	                            t.id as TurmaId,
	                            t.ue_id as UeId,
	                            bimestre
                            from fechamento_nota fn
                            left join fechamento_aluno fa ON fn.fechamento_aluno_id = fa.id
                            inner join fechamento_turma_disciplina ftd on fa.fechamento_turma_disciplina_id = ftd.id 
                            inner join fechamento_turma ft on ftd.fechamento_turma_id = ft.id 
                            inner join componente_curricular cc on ftd.disciplina_id = cc.id 
                            left join conceito_valores cv on fn.conceito_id = cv.id 
                            inner join periodo_escolar pe on periodo_escolar_id = pe.id 
                            inner join turma t on ft.turma_id = t.id
                            where 
                            not ftd.excluido
                            and cc.permite_lancamento_nota = true
                            and periodo_escolar_id = @periodoEscolarId and 
                            t.ue_id = @ueId";

            return await database.Conexao.QueryAsync<AlunosFechamentoNotaDto>(query, new { ueId, periodoEscolarId });
        }
    }
}