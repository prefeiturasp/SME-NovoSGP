using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAula : RepositorioBase<Aula>, IRepositorioAula
    {
        public RepositorioAula(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<AulaConsultaDto> ObterAulaDataTurmaDisciplina(DateTime data, string turmaId, string disciplinaId)
        {
            var query = @"select *
                 from aula
                where not excluido
                  and DATE(data_aula) = @data
                  and turma_id = @turmaId
                  and disciplina_id = @disciplinaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<AulaConsultaDto>(query, new
            {
                data,
                turmaId,
                disciplinaId
            });
        }

        public async Task<IEnumerable<AulaDto>> ObterAulas(long tipoCalendarioId, string turmaId, string ueId, string rf, int? mes = null, int? semanaAno = null)
        {
            StringBuilder query = new StringBuilder();
            MontaCabecalho(query);
            query.AppendLine("FROM public.aula a");
            MontaWhere(query, turmaId, ueId, mes, null, rf, semanaAno);
            return (await database.Conexao.QueryAsync<AulaDto>(query.ToString(), new { tipoCalendarioId, turmaId, ueId, mes, semanaAno }));
            return (await database.Conexao.QueryAsync<AulaDto>(query.ToString(), new { tipoCalendarioId, turmaId, ueId, rf }));
        }

        public async Task<IEnumerable<AulaCompletaDto>> ObterAulasCompleto(long tipoCalendarioId, string turmaId, string ueId, DateTime data, Guid perfil, string rf)
        {
            StringBuilder query = new StringBuilder();
            MontaCabecalho(query);
            query.AppendLine(",ab.turma_nome,");
            query.AppendLine("ab.ue_nome");
            query.AppendLine("FROM public.aula a");
            query.AppendLine("INNER JOIN v_abrangencia ab on a.turma_id = ab.turma_id");
            MontaWhere(query, turmaId, ueId, null, data, rf);
            MontaGroupBy(query);
            var sql = query.ToString();
            return (await database.Conexao.QueryAsync<AulaCompletaDto>(query.ToString(), new { tipoCalendarioId, turmaId, ueId, data, perfil, rf }));
        }

        public IEnumerable<Aula> ObterAulasPorTurmaEAnoLetivo(string turmaId, string anoLetivo)
        {
            var query = "select * from aula where turma_id= @turmaId and date_part('year',data_aula) = @anoLetivo and not excluido";

            return database.Conexao.Query<Aula>(query, new
            {
                turmaId,
                anoLetivo
            });
        }

        public async Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaDisciplinaSemana(string turma, string disciplina, string semana)
        {
            var query = @"select professor_rf, quantidade, data_aula
                 from aula
                where not excluido
                  and turma_id = @turma
                  and disciplina_id = @disciplina
                  and to_char(data_aula, 'IW') = @semana";

            return await database.Conexao.QueryAsync<AulasPorTurmaDisciplinaDto>(query, new
            {
                turma,
                disciplina,
                semana
            });
        }

        public async Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaExperienciasPedagogicasSemana(string turma, string semana)
        {
            var query = @"select professor_rf, quantidade, data_aula
                 from aula
                where not excluido
                  and turma_id = @turma
                  and disciplina_id in ('1214','1215','1216','1217','1218','1219','1220','1221','1222','1223')
                  and to_char(data_aula, 'IW') = @semana";

            return await database.Conexao.QueryAsync<AulasPorTurmaDisciplinaDto>(query, new
            {
                turma,
                semana
            });
        }

        public IEnumerable<AulaConsultaDto> ObterDatasDeAulasPorAnoTurmaEDisciplina(int anoLetivo, string turmaId, string disciplinaId, long usuarioId, string usuarioRF, Guid perfil)
        {
            var query = new StringBuilder("select distinct a.*");
            query.AppendLine("from aula a ");
            query.AppendLine("inner join v_abrangencia v on ");
            query.AppendLine("a.turma_id = v.turma_id ");
            query.AppendLine("inner join tipo_calendario t on ");
            query.AppendLine("a.tipo_calendario_id = t.id ");
            query.AppendLine("where ");
            query.AppendLine("not a.excluido ");
            query.AppendLine("and v.usuario_id = @usuarioId ");
            query.AppendLine("and v.usuario_perfil = @perfil ");
            query.AppendLine("and a.turma_id = @turmaId ");
            query.AppendLine("and a.disciplina_id = @disciplinaId ");
            query.AppendLine("and t.ano_letivo = @anoLetivo");

            if (!string.IsNullOrWhiteSpace(usuarioRF))
            {
                query.AppendLine("and a.professor_rf = @usuarioRF ");
            }

            return database.Conexao.Query<AulaConsultaDto>(query.ToString(), new
            {
                usuarioRF,
                usuarioId,
                perfil,
                anoLetivo,
                turmaId,
                disciplinaId
            });
        }

        public Aula ObterPorWorkflowId(long workflowId)
        {
            var query = @"select a.id,
                                 a.ue_id,
                                 a.disciplina_id,
                                 a.turma_id,
                                 a.tipo_calendario_id,
                                 a.professor_rf,
                                 a.quantidade,
                                 a.data_aula,
                                 a.recorrencia_aula,
                                 a.tipo_aula,
                                 a.criado_em,
                                 a.criado_por,
                                 a.alterado_em,
                                 a.alterado_por,
                                 a.criado_rf,
                                 a.alterado_rf,
                                 a.excluido,
                                 a.migrado,
                                 a.aula_pai_id,
                                 a.wf_aprovacao_id,
                                 a.status
                             from  aula a
                            where a.excluido = false
                              and a.migrado = false
                              and tipo_aula = 2
                              and a.wf_aprovacao_id = @workflowId";

            return database.Conexao.QueryFirst<Aula>(query.ToString(), new
            {
                workflowId
            });
        }

        public bool UsuarioPodeCriarAulaNaUeTurmaEModalidade(Aula aula, ModalidadeTipoCalendario modalidade)
        {
            var query = new StringBuilder("select 1 from v_abrangencia where turma_id = @turmaId and ue_codigo = @ueId ");

            if (modalidade == ModalidadeTipoCalendario.EJA)
            {
                query.AppendLine($"and modalidade_codigo = {(int)Modalidade.EJA} ");
            }
            else
            {
                query.AppendLine($"and (modalidade_codigo = {(int)Modalidade.Fundamental} or modalidade_codigo = {(int)Modalidade.Medio}) ");
            }

            return database.Conexao.QueryFirstOrDefault<bool>(query.ToString(), new
            {
                aula.TurmaId,
                aula.UeId
            });
        }

        private static void MontaCabecalho(StringBuilder query)
        {
            query.AppendLine("SELECT id,");
            query.AppendLine("a.aula_pai_id,");
            query.AppendLine("a.ue_id,");
            query.AppendLine("a.disciplina_id,");
            query.AppendLine("a.turma_id,");
            query.AppendLine("a.tipo_calendario_id,");
            query.AppendLine("a.professor_rf,");
            query.AppendLine("a.quantidade,");
            query.AppendLine("a.data_aula,");
            query.AppendLine("a.recorrencia_aula,");
            query.AppendLine("a.tipo_aula,");
            query.AppendLine("a.criado_em,");
            query.AppendLine("a.criado_por,");
            query.AppendLine("a.alterado_em,");
            query.AppendLine("a.alterado_por,");
            query.AppendLine("a.criado_rf,");
            query.AppendLine("a.alterado_rf,");
            query.AppendLine("a.excluido,");
            query.AppendLine("a.status,");
            query.AppendLine("a.migrado");
        }

        private static void MontaGroupBy(StringBuilder query)
        {
            query.AppendLine("group by ab.turma_id,");
            query.AppendLine("id,");
            query.AppendLine("a.ue_id,");
            query.AppendLine("a.disciplina_id,");
            query.AppendLine("a.turma_id,");
            query.AppendLine("ab.turma_nome,");
            query.AppendLine("ab.ue_nome,");
            query.AppendLine("a.tipo_calendario_id,");
            query.AppendLine("a.professor_rf,");
            query.AppendLine("a.quantidade,");
            query.AppendLine("a.data_aula,");
            query.AppendLine("a.recorrencia_aula,");
            query.AppendLine("a.tipo_aula,");
            query.AppendLine("a.criado_em,");
            query.AppendLine("a.criado_por,");
            query.AppendLine("a.alterado_em,");
            query.AppendLine("a.alterado_por,");
            query.AppendLine("a.criado_rf,");
            query.AppendLine("a.alterado_rf,");
            query.AppendLine("a.excluido,");
            query.AppendLine("a.migrado;");
        }

        private static void MontaWhere(StringBuilder query, string turmaId, string ueId, int? mes = null, DateTime? data = null, string rf = null, int? semanaAno = null)
        {
            query.AppendLine("WHERE a.tipo_calendario_id = @tipoCalendarioId");
            query.AppendLine("and a.status <> '3'");

            if (!string.IsNullOrEmpty(turmaId))
                query.AppendLine("AND a.turma_id = @turmaId");

            if (!string.IsNullOrEmpty(ueId))
                query.AppendLine("AND a.ue_id = @ueId");

            if (mes.HasValue)
                query.AppendLine("AND extract(month from a.data_aula) = @mes");

            if (data.HasValue)
                query.AppendLine("AND DATE(a.data_aula) = @data");

            if (semanaAno.HasValue)
                query.AppendLine("AND extract(week from a.data_aula) = @semanaAno");
                
            if (!string.IsNullOrEmpty(rf))
                query.AppendLine("AND a.professor_rf = @rf");
        }

        public async Task<IEnumerable<Aula>> ObterAulasRecorrencia(long aulaPaiId, long? aulaIdInicioRecorrencia = null, DateTime? dataFinal = null)
        {
            StringBuilder query = new StringBuilder();
            MontaCabecalho(query);
            query.AppendLine("FROM public.aula a");
            query.AppendLine("where ((a.id = @aulaPaiId) or (a.aula_pai_id = @aulaPaiId))");

            if (aulaIdInicioRecorrencia.HasValue)
                query.AppendLine(" and a.id > @aulaIdInicioRecorrencia");

            if (dataFinal.HasValue)
                query.AppendLine(" and data_aula <= @dataFinal");

            query.AppendLine(" order by data_aula");

            return await database.Conexao.QueryAsync<Aula>(query.ToString(), new { aulaPaiId, aulaIdInicioRecorrencia, dataFinal });
        }
    }
}