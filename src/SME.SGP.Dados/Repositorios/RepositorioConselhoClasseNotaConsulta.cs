using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasseNotaConsulta : RepositorioBase<ConselhoClasseNota>, IRepositorioConselhoClasseNotaConsulta
    {
        public RepositorioConselhoClasseNotaConsulta(ISgpContextConsultas database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<ConselhoClasseNota> ObterPorConselhoClasseAlunoComponenteCurricularAsync(long conselhoClasseAlunoId, long componenteCurricularCodigo)
        {
            const string query = @"select * 
                        from conselho_classe_nota 
                        where conselho_classe_aluno_id = @conselhoClasseAlunoId
                          and componente_curricular_codigo = @componenteCurricularCodigo
                          and not excluido";

            return await database.Conexao.QueryFirstOrDefaultAsync<ConselhoClasseNota>(query, new { conselhoClasseAlunoId, componenteCurricularCodigo });
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoAsync(long conselhoClasseId, string alunoCodigo, long? componenteCurricularId)
        {
            var query = $@"select ccn.id, ccn.componente_curricular_codigo as ComponenteCurricularCodigo, ccn.conceito_id as ConceitoId, ccn.nota
                          from conselho_classe_aluno cca 
                         inner join conselho_classe_nota ccn on ccn.conselho_classe_aluno_id = cca.id and not ccn.excluido
                          where cca.aluno_codigo = @alunoCodigo
                            and cca.conselho_classe_id = @conselhoClasseId";
            if (componenteCurricularId != null)
                query += @"  and ccn.componente_curricular_codigo = @componenteCurricularId";

            return await database.Conexao.QueryAsync<NotaConceitoBimestreComponenteDto>(query, new { conselhoClasseId, alunoCodigo, componenteCurricularId });
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoAsync(string alunoCodigo, string turmaCodigo, long? periodoEscolarId = null)
        {
            var condicaoPeriodoEscolar = periodoEscolarId.HasValue ? "ft.periodo_escolar_id = @periodoEscolarId" : "ft.periodo_escolar_id is null";

            var query = $@"select distinct * from (
                select fn.disciplina_id as ComponenteCurricularCodigo,
                       coalesce(ccn.conceito_id, fn.conceito_id) as ConceitoId,
                       coalesce(ccn.nota, fn.nota) as Nota,
                       coalesce(cca.aluno_codigo, fa.aluno_codigo) as AlunoCodigo
                  from fechamento_turma ft
                 inner join turma t on t.id = ft.turma_id 
                 inner join fechamento_turma_disciplina ftd on ftd.fechamento_turma_id = ft.id
                 inner join fechamento_aluno fa on fa.fechamento_turma_disciplina_id = ftd.id
                 inner join fechamento_nota fn on fn.fechamento_aluno_id = fa.id
                  left join conselho_classe cc on cc.fechamento_turma_id = ft.id
                  left join conselho_classe_aluno cca on cca.conselho_classe_id  = cc.id
		                                        and cca.aluno_codigo = fa.aluno_codigo 
                  left join conselho_classe_nota ccn on ccn.conselho_classe_aluno_id = cca.id 
		                                        and ccn.componente_curricular_codigo = fn.disciplina_id and not ccn.excluido
                 where {condicaoPeriodoEscolar}
                   and t.turma_id = @turmaCodigo
                   and fa.aluno_codigo = @alunoCodigo
                union all 
                select ccn.componente_curricular_codigo as ComponenteCurricularCodigo,
                       coalesce(ccn.conceito_id, fn.conceito_id) as ConceitoId, 
                       coalesce(ccn.nota, fn.nota) as Nota,
                       coalesce(cca.aluno_codigo, fa.aluno_codigo) as AlunoCodigo
                  from fechamento_turma ft
                 inner join turma t on t.id = ft.turma_id 
                 inner join conselho_classe cc on cc.fechamento_turma_id = ft.id
                 inner join conselho_classe_aluno cca on cca.conselho_classe_id  = cc.id
                 inner join conselho_classe_nota ccn on ccn.conselho_classe_aluno_id = cca.id and not ccn.excluido 
                  left join fechamento_turma_disciplina ftd on ftd.fechamento_turma_id = ft.id
                  left join fechamento_aluno fa on fa.fechamento_turma_disciplina_id = ftd.id
		                                        and cca.aluno_codigo = fa.aluno_codigo 
                  left join fechamento_nota fn on fn.fechamento_aluno_id = fa.id
		                                        and ccn.componente_curricular_codigo = fn.disciplina_id 
                 where {condicaoPeriodoEscolar}
                   and t.turma_id = @turmaCodigo
                   and cca.aluno_codigo = @alunoCodigo
                ) x ";

            return await database.Conexao.QueryAsync<NotaConceitoBimestreComponenteDto>(query, new { alunoCodigo, turmaCodigo, periodoEscolarId });
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasConselhoAlunoAsync(string alunoCodigo, string[] turmasCodigos, long? periodoEscolarId = null)
        {
            var condicaoPeriodoEscolar = periodoEscolarId.HasValue ? "ft.periodo_escolar_id = @periodoEscolarId" : "ft.periodo_escolar_id is null";
            var query = $@"select ccn.componente_curricular_codigo as ComponenteCurricularCodigo, ccn.conceito_id as ConceitoId, ccn.nota as Nota
                  from fechamento_turma ft
                 inner join turma t on t.id = ft.turma_id 
                 inner join conselho_classe cc on cc.fechamento_turma_id = ft.id
                 inner join conselho_classe_aluno cca on cca.conselho_classe_id  = cc.id
                 inner join conselho_classe_nota ccn on ccn.conselho_classe_aluno_id = cca.id and not ccn.excluido 
                 where {condicaoPeriodoEscolar}
                   and t.turma_id = ANY(@turmasCodigos)
                   and cca.aluno_codigo = @alunoCodigo ";

            return await database.Conexao.QueryAsync<NotaConceitoBimestreComponenteDto>(query, new { alunoCodigo, turmasCodigos, periodoEscolarId });
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoPorTurmasAsync(string alunoCodigo, IEnumerable<string> turmasCodigos, long? periodoEscolarId)
        {
            var condicaoPeriodoEscolar = periodoEscolarId.HasValue ? "ft.periodo_escolar_id = @periodoEscolarId" : "ft.periodo_escolar_id is null";
            var query = $@"select * from (
               select ccn.componente_curricular_codigo as ComponenteCurricularCodigo, ccn.conceito_id as ConceitoId, ccn.nota as Nota
                  from fechamento_turma ft
                 inner join turma t on t.id = ft.turma_id 
                 inner join conselho_classe cc on cc.fechamento_turma_id = ft.id
                 inner join conselho_classe_aluno cca on cca.conselho_classe_id  = cc.id
                 inner join conselho_classe_nota ccn on ccn.conselho_classe_aluno_id = cca.id and not ccn.excluido
                 where {condicaoPeriodoEscolar}
                   and t.turma_id = ANY(@turmasCodigos)
                   and cca.aluno_codigo = @alunoCodigo 
               union
               select ftd.disciplina_id as ComponenteCurricularCodigo,fn.conceito_id as ConceitoId, fn.nota as nota 
                 from fechamento_nota fn 
                inner join fechamento_aluno fa on fa.id = fn.fechamento_aluno_id 
                inner join fechamento_turma_disciplina ftd on ftd.id = fa.fechamento_turma_disciplina_id 
                inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id
                inner join turma t on t.id = ft.turma_id
                where {condicaoPeriodoEscolar}
                  and t.turma_id = ANY(@turmasCodigos)
                  and fa.aluno_codigo = @alunoCodigo 
                ) as x"; ;

            return await database.Conexao.QueryAsync<NotaConceitoBimestreComponenteDto>(query, new { alunoCodigo, turmasCodigos, periodoEscolarId });
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasConceitosFechamentoPorTurmaCodigoEBimestreAsync(string turmaCodigo, int bimestre = 0,
            DateTime? dataMatricula = null, DateTime? dataSituacao = null, long? tipoCalendario = null)
        {
            var condicaoBimestre = bimestre > 0 ? "and bimestre = @bimestre" : string.Empty;
            var condicaoDataMatricula = dataMatricula.HasValue ? $"and (@dataMatricula <= pe.periodo_fim {(bimestre == 0 ? "or pe.id is null" : string.Empty)})" : string.Empty;
            var condicaoDataSituacao = dataSituacao.HasValue ? $"and (@dataSituacao >= pe.periodo_fim {(bimestre == 0 ? "or pe.id is null" : string.Empty)})" : string.Empty;
            var condicaoTipoCalendario = tipoCalendario.HasValue ? $"and (pe.tipo_calendario_id =@tipoCalendario or pe.tipo_calendario_id is null)" : string.Empty;
            var query = $@"select distinct * from (
                select pe.bimestre, 
                       fn.disciplina_id as ComponenteCurricularCodigo, 
                       fn.conceito_id as ConceitoId, 
                       fn.nota as Nota,
                       fa.aluno_codigo as AlunoCodigo,
                       t.turma_id TurmaCodigo
                  from fechamento_turma ft
                  left join periodo_escolar pe on pe.id = ft.periodo_escolar_id 
                 inner join turma t on t.id = ft.turma_id 
                 inner join fechamento_turma_disciplina ftd on ftd.fechamento_turma_id = ft.id
                 inner join fechamento_aluno fa on fa.fechamento_turma_disciplina_id = ftd.id
                 inner join fechamento_nota fn on fn.fechamento_aluno_id = fa.id 
                 where not ftd.excluido and t.turma_id = @turmaCodigo
                   {condicaoTipoCalendario}
                   {condicaoBimestre}
                   {condicaoDataMatricula}
                   {condicaoDataSituacao}
            ) x";

            return await database.Conexao.QueryAsync<NotaConceitoBimestreComponenteDto>(query, new { turmaCodigo, bimestre, dataMatricula, dataSituacao, tipoCalendario });
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasFinaisBimestresAlunoAsync(string alunoCodigo, string[] turmasCodigo, int bimestre = 0, DateTime? dataMatricula = null, DateTime? dataSituacao = null, bool validaMatricula = true)
        {
            var condicaoBimestre = bimestre > 0 ? "and bimestre = @bimestre" : string.Empty;
            var condicaoDataMatricula = dataMatricula.HasValue && validaMatricula ? $"and (@dataMatricula <= pe.periodo_fim {(bimestre == 0 ? "or pe.id is null" : string.Empty)})" : string.Empty;
            var condicaoDataSituacao = dataSituacao.HasValue ? $"and (@dataSituacao >= pe.periodo_fim {(bimestre == 0 ? "or pe.id is null" : string.Empty)})" : string.Empty;
            var query = $@"select distinct * from (
                select pe.bimestre, fn.disciplina_id as ComponenteCurricularCodigo, ccn.id as ConselhoClasseNotaId, coalesce(ccn.conceito_id, fn.conceito_id) as ConceitoId, coalesce(ccn.nota, fn.nota) as Nota
                  from fechamento_turma ft
                  left join periodo_escolar pe on pe.id = ft.periodo_escolar_id 
                 inner join turma t on t.id = ft.turma_id 
                 inner join fechamento_turma_disciplina ftd on ftd.fechamento_turma_id = ft.id
                 inner join fechamento_aluno fa on fa.fechamento_turma_disciplina_id = ftd.id
                 inner join fechamento_nota fn on fn.fechamento_aluno_id = fa.id
                  left join conselho_classe cc on cc.fechamento_turma_id = ft.id
                  left join conselho_classe_aluno cca on cca.conselho_classe_id  = cc.id
		                                        and cca.aluno_codigo = fa.aluno_codigo 
                  left join conselho_classe_nota ccn on ccn.conselho_classe_aluno_id = cca.id 
		                                        and ccn.componente_curricular_codigo = fn.disciplina_id and not ccn.excluido
                 where t.turma_id = ANY(@turmasCodigo)
                   and fa.aluno_codigo = @alunoCodigo
                   {condicaoBimestre}
                   {condicaoDataMatricula}
                   {condicaoDataSituacao}
                union all 
                select pe.bimestre, ccn.componente_curricular_codigo as ComponenteCurricularCodigo, ccn.id as ConselhoClasseNotaId, coalesce(ccn.conceito_id, fn.conceito_id) as ConceitoId, coalesce(ccn.nota, fn.nota) as Nota
                  from fechamento_turma ft
                  left join periodo_escolar pe on pe.id = ft.periodo_escolar_id 
                 inner join turma t on t.id = ft.turma_id 
                 inner join conselho_classe cc on cc.fechamento_turma_id = ft.id
                 inner join conselho_classe_aluno cca on cca.conselho_classe_id  = cc.id
                 inner join conselho_classe_nota ccn on ccn.conselho_classe_aluno_id = cca.id and not ccn.excluido
                  left join fechamento_turma_disciplina ftd on ftd.fechamento_turma_id = ft.id
                  left join fechamento_aluno fa on fa.fechamento_turma_disciplina_id = ftd.id
		                                        and cca.aluno_codigo = fa.aluno_codigo 
                  left join fechamento_nota fn on fn.fechamento_aluno_id = fa.id
		                                        and ccn.componente_curricular_codigo = fn.disciplina_id 
                 where t.turma_id = ANY(@turmasCodigo)
                   and cca.aluno_codigo = @alunoCodigo
                   {condicaoBimestre}
                   {condicaoDataMatricula}
                   {condicaoDataSituacao}
                ) x ";

            return await database.Conexao.QueryAsync<NotaConceitoBimestreComponenteDto>(query, new { alunoCodigo, turmasCodigo, bimestre, dataMatricula, dataSituacao });
        }


        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasConceitosConselhoClassePorTurmaCodigoEBimestreAsync(string turmaCodigo, int? bimestre,
            DateTime? dataMatricula = null, DateTime? dataSituacao = null, long? tipoCalendario = null)
        {
            var condicaoBimestre = bimestre.HasValue ? bimestre > 0 ? "and bimestre = @bimestre" : "and ft.periodo_escolar_id is null" : string.Empty;
            var condicaoDataMatricula = dataMatricula.HasValue ? $"and (@dataMatricula <= pe.periodo_fim {(bimestre == 0 ? "or pe.id is null" : string.Empty)})" : string.Empty;
            var condicaoDataSituacao = dataSituacao.HasValue ? $"and (@dataSituacao >= pe.periodo_fim {(bimestre == 0 ? "or pe.id is null" : string.Empty)})" : string.Empty;
            var condicaoTipoCalendario = tipoCalendario.HasValue ? $"and (pe.tipo_calendario_id =@tipoCalendario or pe.tipo_calendario_id is null)" : string.Empty;
            
            var query = $@"select distinct * from (
                select pe.bimestre,
                    ccn.componente_curricular_codigo as ComponenteCurricularCodigo,
                    cc.id as ConselhoClasseId,
                    ccn.id as ConselhoClasseNotaId,
                    ccn.conceito_id as ConceitoId,
                    ccn.nota as Nota,
                    cca.aluno_codigo as AlunoCodigo,
                    t.turma_id as TurmaCodigo
                  from fechamento_turma ft
                  left join periodo_escolar pe on pe.id = ft.periodo_escolar_id 
                 inner join turma t on t.id = ft.turma_id 
                 inner join conselho_classe cc on cc.fechamento_turma_id = ft.id
                 inner join conselho_classe_aluno cca on cca.conselho_classe_id  = cc.id
                 inner join conselho_classe_nota ccn on ccn.conselho_classe_aluno_id = cca.id and not ccn.excluido
                 where t.turma_id = @turmaCodigo
                   {condicaoTipoCalendario}
                   {condicaoBimestre}
                   {condicaoDataMatricula}
                   {condicaoDataSituacao}
            ) x ";
            
            return await database.Conexao.QueryAsync<NotaConceitoBimestreComponenteDto>(query, new { turmaCodigo, bimestre, dataMatricula, dataSituacao, tipoCalendario });
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasBimestresAluno(string alunoCodigo, string ueCodigo, string turmaCodigo, int[] bimestres)
        {
            var condicaoBimestre = string.Empty;
            
            if (bimestres != null && bimestres.Any())
                condicaoBimestre = $" and ({(bimestres.Contains(0) ? " bimestre is null or " : "")}  bimestre = any(@bimestres)) ";

            var query = $@"select distinct * from (
                select pe.bimestre, fn.disciplina_id as ComponenteCurricularCodigo, 
                    coalesce(disciplina.descricao_sgp,disciplina.descricao) as ComponenteCurricularNome,  
                    ccn.id as ConselhoClasseNotaId, 
                    coalesce(ccn.conceito_id, fn.conceito_id) as ConceitoId, coalesce(ccn.nota, fn.nota) as Nota,
                    coalesce(cca.aluno_codigo, fa.aluno_codigo) as AlunoCodigo
                  from fechamento_turma ft
                  left join periodo_escolar pe on pe.id = ft.periodo_escolar_id 
                 inner join turma t on t.id = ft.turma_id 
                 inner join ue on t.ue_id = ue.id
                 inner join fechamento_turma_disciplina ftd on ftd.fechamento_turma_id = ft.id
                 inner join fechamento_aluno fa on fa.fechamento_turma_disciplina_id = ftd.id
                 inner join fechamento_nota fn on fn.fechamento_aluno_id = fa.id
                 inner join componente_curricular disciplina on fn.disciplina_id = disciplina.id
                  left join conselho_classe cc on cc.fechamento_turma_id = ft.id
                  left join conselho_classe_aluno cca on cca.conselho_classe_id  = cc.id
		                                        and cca.aluno_codigo = fa.aluno_codigo 
                  left join conselho_classe_nota ccn on ccn.conselho_classe_aluno_id = cca.id 
		                                        and ccn.componente_curricular_codigo = fn.disciplina_id and not ccn.excluido
                 where cca.id is not null 
                   and t.turma_id = @turmaCodigo
                   and ue.ue_id = @ueCodigo
                   and fa.aluno_codigo = @alunoCodigo 
                   {condicaoBimestre}
                union all 
                select pe.bimestre, ccn.componente_curricular_codigo as ComponenteCurricularCodigo, 
                    coalesce(disciplina.descricao_sgp,disciplina.descricao) as ComponenteCurricularNome,  
                    ccn.id as ConselhoClasseNotaId, 
                    coalesce(ccn.conceito_id, fn.conceito_id) as ConceitoId, coalesce(ccn.nota, fn.nota) as Nota,
                    coalesce(cca.aluno_codigo, fa.aluno_codigo) as AlunoCodigo
                  from fechamento_turma ft
                  left join periodo_escolar pe on pe.id = ft.periodo_escolar_id 
                 inner join turma t on t.id = ft.turma_id 
                 inner join ue on t.ue_id = ue.id
                 inner join conselho_classe cc on cc.fechamento_turma_id = ft.id
                 inner join conselho_classe_aluno cca on cca.conselho_classe_id  = cc.id
                 inner join conselho_classe_nota ccn on ccn.conselho_classe_aluno_id = cca.id and not ccn.excluido
                 inner join componente_curricular disciplina on ccn.componente_curricular_codigo = disciplina.id
                  left join fechamento_turma_disciplina ftd on ftd.fechamento_turma_id = ft.id
                  left join fechamento_aluno fa on fa.fechamento_turma_disciplina_id = ftd.id
		                                        and cca.aluno_codigo = fa.aluno_codigo 
                  left join fechamento_nota fn on fn.fechamento_aluno_id = fa.id
		                                        and ccn.componente_curricular_codigo = fn.disciplina_id 
                 where  cca.id is not null 
                   and t.turma_id = @turmaCodigo
                   and ue.ue_id = @ueCodigo
                   and cca.aluno_codigo = @alunoCodigo 
                   {condicaoBimestre}
                ) x ";

            return await database.Conexao.QueryAsync<NotaConceitoBimestreComponenteDto>(query, new { alunoCodigo, ueCodigo, turmaCodigo, bimestres });
        }

        public async Task<double?> VerificaNotaConselhoEmAprovacao(long conselhoClasseNotaId)
        {
            var query = $@"select coalesce(coalesce(wf.nota, wf.conceito_id),-1) from wf_aprovacao_nota_conselho wf 
                                where wf.conselho_classe_nota_id = @conselhoClasseNotaId and not wf.excluido";

            return await database.Conexao.QueryFirstOrDefaultAsync<double?>(query, new { conselhoClasseNotaId });
        }

        public async Task<WFAprovacaoNotaConselho> ObterNotaEmAprovacaoWf(long workFlowId)
        {
            const string query = @"select w.*, n.*, cca.*, cc.*, ft.*
                            from wf_aprovacao_nota_conselho w
                          inner join conselho_classe_nota n on n.id = w.conselho_classe_nota_id 
                          inner join conselho_classe_aluno cca on cca.id = n.conselho_classe_aluno_id
                          inner join conselho_classe cc on cc.id = cca.conselho_classe_id
                          inner join fechamento_turma ft on ft.id = cc.fechamento_turma_id
                          where w.wf_aprovacao_id = @workFlowId and not w.excluido";

            return (await database.Conexao.QueryAsync<WFAprovacaoNotaConselho, ConselhoClasseNota, ConselhoClasseAluno, ConselhoClasse, FechamentoTurma, WFAprovacaoNotaConselho>(query,
                (wfAprovacaoNota, conselhoNota, conselhoClasseAluno, conselhoClasse, fechamentoTurma) =>
                {
                    conselhoClasse.FechamentoTurma = fechamentoTurma;
                    conselhoClasseAluno.ConselhoClasse = conselhoClasse;
                    conselhoNota.ConselhoClasseAluno = conselhoClasseAluno;
                    wfAprovacaoNota.ConselhoClasseNota = conselhoNota;
                    return wfAprovacaoNota;
                },
                new { workFlowId })).FirstOrDefault();
        }       
    }
}