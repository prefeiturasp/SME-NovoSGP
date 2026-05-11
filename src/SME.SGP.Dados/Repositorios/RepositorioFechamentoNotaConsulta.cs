using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamentoNotaConsulta : RepositorioBase<FechamentoNota>, IRepositorioFechamentoNotaConsulta
    {
        const string queryPorFechamento = @"with lista as (select fa.aluno_codigo as AlunoCodigo
                                            , n.disciplina_id as ComponenteCurricularId
                                            , coalesce(wf.nota, n.nota) as Nota
                                            , coalesce(wf.conceito_id, n.conceito_id) as ConceitoId
                                            , pe.bimestre
                                            , wf.id as EmAprovacao
                                            , row_number() over (partition by ft.turma_id, fa.aluno_codigo, pe.id, n.disciplina_id order by n.id desc) sequencia
                                         from fechamento_nota n
                                        inner join fechamento_aluno fa on fa.id = n.fechamento_aluno_id
                                        inner join fechamento_turma_disciplina ftd on ftd.id = fa.fechamento_turma_disciplina_id 
                                        inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id 
                                         left join periodo_escolar pe on pe.id = ft.periodo_escolar_id 
                                         left join wf_aprovacao_nota_fechamento wf on wf.fechamento_nota_id = n.id and not wf.excluido
                                        where fa.fechamento_turma_disciplina_id = ANY(@fechamentosTurmaDisciplinaId) and not n.excluido";

        const string queryNotasFechamento = @"with lista as (
                        select  fn.disciplina_id as ComponenteCurricularCodigo, 
                                fn.conceito_id as ConceitoId, 
                                fn.nota, 
                                pe.bimestre, 
                                t.turma_id as TurmaCodigo,
                                fa.aluno_codigo as AlunoCodigo,
                                row_number() over (partition by t.id, fa.aluno_codigo, pe.id, fn.disciplina_id order by fn.id desc) sequencia    
                          from fechamento_turma ft
                         inner join turma t on t.id = ft.turma_id 
                          left join periodo_escolar pe on pe.id = ft.periodo_escolar_id 
                         inner join fechamento_turma_disciplina ftd on ftd.fechamento_turma_id = ft.id
                         inner join fechamento_aluno fa on fa.fechamento_turma_disciplina_id = ftd.id
                         inner join fechamento_nota fn on fn.fechamento_aluno_id = fa.id
                         inner join componente_curricular cc on cc.id = fn.disciplina_id
                         where not fn.excluido
                               and cc.permite_lancamento_nota ";

        public RepositorioFechamentoNotaConsulta(ISgpContextConsultas database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasFinaisAlunoAsync(string[] turmasCodigos, string alunoCodigo)
        {
            var query = $@"{queryNotasFechamento}
                            and t.turma_id = ANY(@turmasCodigos) 
                            and fa.aluno_codigo = @alunoCodigo
                            and pe.id is null) select * from lista where sequencia = 1;";

            return await database.Conexao.QueryAsync<NotaConceitoBimestreComponenteDto>(query, new { turmasCodigos, alunoCodigo });
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoAnoAsync(string turmaCodigo, string alunoCodigo)
        {
            var query = $@"{queryNotasFechamento}
                           and t.turma_id = @turmaCodigo
                           and fa.aluno_codigo = @alunoCodigo) select * from lista where sequencia = 1;";

            return await database.Conexao.QueryAsync<NotaConceitoBimestreComponenteDto>(query, new { turmaCodigo, alunoCodigo });
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoBimestreAsync(long fechamentoTurmaId, string alunoCodigo)
        {
            var query = $@"{queryNotasFechamento}
                           and ftd.fechamento_turma_id = @fechamentoTurmaId
                           and fa.aluno_codigo = @alunoCodigo) select * from lista where sequencia = 1;";

            return await database.Conexao.QueryAsync<NotaConceitoBimestreComponenteDto>(query, new { fechamentoTurmaId, alunoCodigo });
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterPorFechamentoTurmaAlunoEDisciplinaAsync(long fechamentoTurmaId, string alunoCodigo, long componenteCurricularId)
        {
            var query = $@"{queryNotasFechamento}
                           and ftd.fechamento_turma_id = @fechamentoTurmaId
                           and fa.aluno_codigo = @alunoCodigo
                           and fn.disciplina_id = @componenteCurricularId) select * from lista where sequencia = 1;";

            return await database.Conexao.QueryAsync<NotaConceitoBimestreComponenteDto>(query, new { fechamentoTurmaId, alunoCodigo, componenteCurricularId });
        }
        

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoPorTurmasCodigosBimestreAsync(string[] turmasCodigos, string alunoCodigo, int bimestre, DateTime? dataMatricula = null, DateTime? dataSituacao = null, int? anoLetivo = null)
        {
            var query = $@"{queryNotasFechamento}
                           and t.turma_id = ANY(@turmasCodigos)
                           and fa.aluno_codigo = @alunoCodigo";

            if (dataMatricula.HasValue && (anoLetivo.NaoEhNulo() || anoLetivo == DateTime.Now.Year))
                query += " and @dataMatricula <= pe.periodo_fim";

            if (dataSituacao.HasValue && (anoLetivo.NaoEhNulo() || anoLetivo == DateTime.Now.Year))
                query += $@" and ((@dataSituacao <= pe.periodo_fim and @dataSituacao >= pe.periodo_inicio)
                             or @dataSituacao > pe.periodo_fim)";

            if (bimestre == 0)
                query += $@" and pe.bimestre is null";
            else
                query += $@" and pe.bimestre = @bimestre";

            query += ") select * from lista where sequencia = 1;";

            return await database.Conexao.QueryAsync<NotaConceitoBimestreComponenteDto>(query, new { turmasCodigos, alunoCodigo, bimestre, dataMatricula, dataSituacao });
        }


        public async Task<IEnumerable<WfAprovacaoNotaFechamento>> ObterNotasEmAprovacaoPorFechamento(long fechamentoTurmaDisciplinaId)
        {
            const string query = @"select w.*
                            from wf_aprovacao_nota_fechamento w
                          inner join fechamento_nota n on n.id = w.fechamento_nota_id 
                          inner join fechamento_aluno a on a.id = n.fechamento_aluno_id
                          where not w.excluido and a.fechamento_turma_disciplina_id = @fechamentoTurmaDisciplinaId";

            return await database.Conexao.QueryAsync<WfAprovacaoNotaFechamento>(query, new { fechamentoTurmaDisciplinaId });
        }

        public async Task<IEnumerable<WfAprovacaoNotaFechamentoTurmaDto>> ObterNotasEmAprovacaoWf(long wfAprovacaoId)
        {
            const string query = @"select ft.turma_id as TurmaId, pe.bimestre as Bimestre, 
                                fa.aluno_codigo as CodigoAluno, fn.nota as NotaAnterior, ftd.id as FechamentoTurmaDisciplinaId,
                                fn.conceito_id as ConceitoAnteriorId, coalesce(cc.descricao_infantil, cc.descricao_sgp, cc.descricao) as ComponenteCurricularDescricao, 
                                cc.eh_regencia as ComponenteCurricularEhRegencia, wanf.criado_rf as UsuarioSolicitanteRf, wanf.*, fn.*, fa.*, ftd.*, ft.* from wf_aprovacao_nota_fechamento wanf 
                            inner join fechamento_nota fn on fn.id = wanf.fechamento_nota_id 
                            inner join fechamento_aluno fa on fa.id = fn.fechamento_aluno_id 
                            inner join fechamento_turma_disciplina ftd on ftd.id = fa.fechamento_turma_disciplina_id 
                            inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id 
                            inner join componente_curricular cc on cc.id = fn.disciplina_id 
                            left join periodo_escolar pe on pe.id = ft.periodo_escolar_id 
                            where not wanf.excluido and wf_aprovacao_id = @wfAprovacaoId";

            return await database.Conexao.QueryAsync<WfAprovacaoNotaFechamentoTurmaDto, WfAprovacaoNotaFechamento, FechamentoNota, FechamentoAluno, FechamentoTurmaDisciplina
                                     , FechamentoTurma, WfAprovacaoNotaFechamentoTurmaDto>(query
                 , (wfAprovacaoDto, wfAprovacaoNota, fechamentoNota, fechamentoAluno, fechamentoTurmaDisciplina, fechamentoTurma) =>
                 {
                     wfAprovacaoDto.WfAprovacao = wfAprovacaoNota;
                     wfAprovacaoDto.FechamentoNota = fechamentoNota;
                     fechamentoAluno.FechamentoTurmaDisciplina = fechamentoTurmaDisciplina;
                     fechamentoTurmaDisciplina.FechamentoTurma = fechamentoTurma;
                     fechamentoNota.FechamentoAluno = fechamentoAluno;

                     return wfAprovacaoDto;
                 }, new { wfAprovacaoId });
        }

        public async Task<IEnumerable<AlunosFechamentoNotaDto>> ObterComNotaLancadaPorPeriodoEscolarUE(long ueId, long periodoEscolarId)
        {
            const string query = @"select distinct 
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
                            where cc.permite_lancamento_nota
                            and periodo_escolar_id = @periodoEscolarId and 
                            t.ue_id = @ueId";

            return await database.Conexao.QueryAsync<AlunosFechamentoNotaDto>(query, new { ueId, periodoEscolarId });
        }

        public async Task<IEnumerable<FechamentoNotaAprovacaoDto>> ObterNotasEmAprovacaoPorIdsFechamento(IEnumerable<long> Ids)
        {
            const string query = @" select coalesce(coalesce(w.nota,w.conceito_id),-1) as NotaEmAprovacao, w.fechamento_nota_id as Id 
                           from wf_aprovacao_nota_fechamento w where not w.excluido and w.fechamento_nota_id = ANY(@Ids)";

            return await database.Conexao.QueryAsync<FechamentoNotaAprovacaoDto>(query, new { Ids = Ids.Select(i => i).ToArray() });
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasPorTurmaCodigoEBimestreAsync(string turmaCodigo, int bimestre,
            DateTime? dataMatricula = null, DateTime? dataSituacao = null, int? anoLetivo = null, long? tipoCalendario = null)
        {
            var query = $@"{queryNotasFechamento} 
                            and t.turma_id = @turmaCodigo
                            and pe.bimestre = @bimestre";
            if(tipoCalendario.HasValue)
                query += " and pe.tipo_calendario_id = @tipoCalendario ";
            if (dataMatricula.HasValue && (anoLetivo.NaoEhNulo() || anoLetivo == DateTime.Now.Year))
                query += " and @dataMatricula <= pe.periodo_fim";

            if (dataSituacao.HasValue && (anoLetivo.NaoEhNulo() || anoLetivo == DateTime.Now.Year))
            {
                query += @" and ((@dataSituacao <= pe.periodo_fim and @dataSituacao >= pe.periodo_inicio)
                             or @dataSituacao > pe.periodo_fim)";
            }

            query += @") select *
                         from lista
                         where sequencia = 1;";

            return await database.Conexao.QueryAsync<NotaConceitoBimestreComponenteDto>(query, new
            {
                turmaCodigo,
                bimestre,
                dataMatricula,
                dataSituacao,
                tipoCalendario
            });
        }

        public Task<IEnumerable<FechamentoNotaAlunoAprovacaoDto>> ObterPorFechamentosTurmaAlunoCodigo(long[] fechamentosTurmaDisciplinaId, string alunoCodigo)
        {
            string query = queryPorFechamento;
            query += @" and fa.aluno_codigo = @alunoCodigo) select * from lista where sequencia = 1;";
            return database.Conexao.QueryAsync<FechamentoNotaAlunoAprovacaoDto>(query, new { fechamentosTurmaDisciplinaId, alunoCodigo });
        }
    }
}