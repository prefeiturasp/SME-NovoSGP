using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAulaPrevistaBimestreConsulta : RepositorioBase<AulaPrevistaBimestre>, IRepositorioAulaPrevistaBimestreConsulta
    {
        private readonly ISgpContext contexto;
        public RepositorioAulaPrevistaBimestreConsulta(ISgpContextConsultas conexao, ISgpContext _contexto, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
            this.contexto = _contexto;
        }

        const string Select = @"
                        select
                                apb.id, 
                                apb.criado_em as CriadoEm,
                                apb.criado_por as CriadoPor, 
                                apb.alterado_em as AlteradoEm, 
                                apb.alterado_por as AlteradoPor,
                                apb.alterado_rf as AlteradoRF, 
                                apb.criado_rf as CriadoRF, 
                                p.bimestre,
                                p.periodo_inicio as inicio, 
                                p.periodo_fim as fim,
                                apb.aulas_previstas as Previstas,
                                case when ap.disciplina_id in ('1060','1061') then false else coalesce(cc.permite_registro_frequencia, true) end as LancaFrequencia,
                                SUM(a.quantidade) filter (where a.tipo_aula = 1 and a.aula_cj = false) as CriadasTitular,
                                SUM(a.quantidade) filter (where a.tipo_aula = 1 and a.aula_cj = true) as CriadasCJ,
                                SUM(a.quantidade) filter (where a.tipo_aula = 1 and coalesce(cc.permite_registro_frequencia, true) and exists (select 1 from registro_frequencia rf where rf.aula_id = a.id)) as Cumpridas,
                                SUM(a.quantidade) filter (where a.tipo_aula = 1 and a.data_aula <= now()) as CumpridasSemFrequencia,
                                SUM(a.quantidade) filter (where a.tipo_aula = 2 and coalesce(cc.permite_registro_frequencia, true) and exists (select 1 from registro_frequencia rf where rf.aula_id = a.id)) as Reposicoes,
                                SUM(a.quantidade) filter (where a.tipo_aula = 2 and a.data_aula <= now() and not coalesce(cc.permite_registro_frequencia, true)) as ReposicoesSemFrequencia
                           from periodo_escolar p
                          inner join tipo_calendario tp on p.tipo_calendario_id = tp.id
                          inner join aula_prevista ap on ap.tipo_calendario_id = p.tipo_calendario_id
                           left join aula_prevista_bimestre apb on ap.id = apb.aula_prevista_id and p.bimestre = apb.bimestre and not apb.excluido 
                           left join aula a on a.turma_id = ap.turma_id and        
                                            (a.disciplina_id = ap.disciplina_id{0}) and
                                            a.tipo_calendario_id = p.tipo_calendario_id and
                                            a.data_aula BETWEEN p.periodo_inicio AND p.periodo_fim
                                            and (a.id is null or not a.excluido)
                                            {1}
                                left join componente_curricular cc on ap.disciplina_id::int8 = cc.id  ";

        const string GroupOrderBy = @" group by p.bimestre, p.periodo_inicio, p.periodo_fim, apb.aulas_previstas, apb.Id,
                                ap.criado_em, ap.criado_por, ap.alterado_em , ap.alterado_por,
                               ap.alterado_rf, ap.criado_rf, coalesce(cc.permite_registro_frequencia, true), ap.disciplina_id; ";
        public async Task<IEnumerable<AulaPrevistaBimestre>> ObterAulasPrevistasPorTurmaTipoCalendarioDisciplina(long tipoCalendarioId, string turmaId, string[] disciplinasId, int? bimestre)
        {

            var sql = @"select * from 
                        (select
                            apb.id, apb.aula_prevista_id, apb.aulas_previstas, apb.bimestre, row_number() over (partition by ap.disciplina_id, apb.bimestre order by apb.aula_prevista_id) sequencia
                        from
                            aula_prevista ap
                        inner join
                            aula_prevista_bimestre apb
                            on ap.id = apb.aula_prevista_id and not apb.excluido
                        where
                            ap.tipo_calendario_id = @tipoCalendarioId and
                            ap.turma_id = @turmaId and
                            ap.disciplina_id = any(@disciplinasId) ";

            if (bimestre.NaoEhNulo())
                sql += " and apb.bimestre = @bimestre";

            sql += ") as consulta_ignora_duplicidade where sequencia = 1;";

            var parametros = new { tipoCalendarioId, turmaId, disciplinasId, bimestre };
            return await database.Conexao.QueryAsync<AulaPrevistaBimestre>(sql, parametros);
        }

        public async Task<IEnumerable<AulaPrevistaBimestre>> ObterAulasPrevistasPorTurmaTipoCalendarioBimestre(long tipoCalendarioId, string turmaId, int bimestre)
        {
            var sql = @"select
                            apb.*
                        from
                            aula_prevista ap
                        inner join
                            aula_prevista_bimestre apb
                            on ap.id = apb.aula_prevista_id and not apb.excluido
                        where
                            ap.tipo_calendario_id = @tipoCalendarioId and
                            ap.turma_id = @turmaId and 
                            and apb.bimestre = @bimestre";

            var parametros = new { tipoCalendarioId, turmaId, bimestre };
            return await database.Conexao.QueryAsync<AulaPrevistaBimestre>(sql, parametros);
        }

        public async Task<IEnumerable<AulaPrevistaBimestreQuantidade>> ObterBimestresAulasPrevistasPorId(long? aulaPrevistaId, string disciplinaIdEquivalenteConsiderada = null, string professor = null)
        {
            StringBuilder query = new StringBuilder();

            query.Append(string.Format(Select, !string.IsNullOrEmpty(disciplinaIdEquivalenteConsiderada) ? " or a.disciplina_id = @disciplinaIdEquivalenteConsiderada " : string.Empty,
                                               !string.IsNullOrEmpty(professor) ? "and a.professor_rf = @professor" : string.Empty));
            query.Append(@" where tp.situacao and not tp.excluido and
                        ap.id = @aulaPrevistaId ");
            query.Append(GroupOrderBy);

            return (await contexto.QueryAsync<AulaPrevistaBimestreQuantidade>(query.ToString(), new { aulaPrevistaId, disciplinaIdEquivalenteConsiderada, professor }));
        }
        public async Task<IEnumerable<AulaPrevistaTurmaComponenteDto>> ObterBimestresAulasTurmasComponentesCumpridasAsync(string[] turmasCodigos, string[] componentesCurricularesId, long tipoCalendarioId, int[] bimestres)
        {

            var query = new StringBuilder(@"select
                            p.id PeriodoEscolarId,
                            p.bimestre,
                            ap.turma_id as TurmaCodigo,
                            ap.disciplina_id as ComponenteCurricularCodigo,    
                            SUM(a.quantidade) filter (
                            where a.tipo_aula = 1
                            and rf.id is not null) as AulasQuantidade                            
                        from
                            periodo_escolar p
                        inner join tipo_calendario tp on
                            p.tipo_calendario_id = tp.id
                        left join aula_prevista ap on
                            ap.tipo_calendario_id = p.tipo_calendario_id
                        left join aula_prevista_bimestre apb on
                            ap.id = apb.aula_prevista_id
                            and p.bimestre = apb.bimestre
                            and not apb.excluido
                        left join aula a on
                            a.turma_id = ap.turma_id
                            and a.disciplina_id = ap.disciplina_id
                            and a.tipo_calendario_id = p.tipo_calendario_id
                            and a.data_aula between p.periodo_inicio and p.periodo_fim
                            and (a.id is null
                            or not a.excluido)
                        left join registro_frequencia rf on
                            a.id = rf.aula_id
                        where
                            tp.situacao
                            and not tp.excluido
                            and ap.tipo_calendario_id = @tipoCalendarioId
                            and ap.turma_id = ANY(@turmasCodigos)
                            and ap.disciplina_id = ANY(@componentesCurricularesId)");

            if (bimestres.Length > 0)
                query.AppendLine(" AND p.bimestre = any(@bimestres)");

            query.AppendLine(@" group by
                            p.id,
                            p.bimestre,
                            ap.turma_id,
                            ap.disciplina_id;");

            return (await database.Conexao.QueryAsync<AulaPrevistaTurmaComponenteDto>(query.ToString(), new { bimestres, turmasCodigos, componentesCurricularesId, tipoCalendarioId }));
        }

    }
}