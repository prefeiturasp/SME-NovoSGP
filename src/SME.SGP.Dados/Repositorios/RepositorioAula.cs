using Dapper;
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
                data = data.Date,
                turmaId,
                disciplinaId
            });
        }

        public async Task<AulaConsultaDto> ObterAulaIntervaloTurmaDisciplina(DateTime dataInicio, DateTime dataFim, string turmaId, long atividadeAvaliativaId)
        {
            var query = @"select *
                 from aula
                where not excluido
                  and DATE(data_aula) between @dataInicio and @dataFim
                  and turma_id = @turmaId
                  and disciplina_id in (
                select disciplina_id from atividade_avaliativa_disciplina aad
               where atividade_avaliativa_id = @atividadeAvaliativaId)";

            return await database.Conexao.QueryFirstOrDefaultAsync<AulaConsultaDto>(query, new
            {
                dataInicio = dataInicio.Date,
                dataFim = dataFim.Date,
                turmaId,
                atividadeAvaliativaId
            });
        }

        public async Task<IEnumerable<AulaDto>> ObterAulas(long tipoCalendarioId, string turmaId, string ueId, string codigoRf, int? mes = null, int? semanaAno = null, string disciplinaId = null)
        {
            StringBuilder query = new StringBuilder();
            MontaCabecalho(query);
            query.AppendLine("FROM public.aula a");
            MontaWhere(query, tipoCalendarioId, turmaId, ueId, mes, null, codigoRf, disciplinaId, semanaAno);
            return (await database.Conexao.QueryAsync<AulaDto>(query.ToString(), new { tipoCalendarioId, turmaId, ueId, codigoRf, mes, semanaAno, disciplinaId }));
        }

        public async Task<IEnumerable<AulaDto>> ObterAulas(string turmaId, string ueId, string codigoRf, DateTime? data, string[] disciplinasId)
        {
            StringBuilder query = new StringBuilder();
            MontaCabecalho(query);
            query.AppendLine("FROM public.aula a");
            MontaWhere(query, null, turmaId, ueId, null, data, codigoRf, null, null, disciplinasId);
            return (await database.Conexao.QueryAsync<AulaDto>(query.ToString(), new { turmaId, ueId, data, codigoRf, disciplinasId }));
        }

        public async Task<IEnumerable<AulaDto>> ObterAulas(long tipoCalendarioId, string turmaId, string ueId, string CodigoRf)
        {
            StringBuilder query = new StringBuilder();
            MontaCabecalho(query);
            query.AppendLine("FROM public.aula a");
            MontaWhere(query, tipoCalendarioId, turmaId, ueId, null, null, CodigoRf);
            return (await database.Conexao.QueryAsync<AulaDto>(query.ToString(), new { tipoCalendarioId, turmaId, ueId, CodigoRf }));
        }

        public async Task<IEnumerable<AulaDto>> ObterAulas(long tipoCalendarioId, string turmaId, string ueId, int mes, string CodigoRf)
        {
            StringBuilder query = new StringBuilder();
            MontaCabecalho(query);
            query.AppendLine("FROM public.aula a");
            MontaWhere(query, tipoCalendarioId, turmaId, ueId, mes, null, CodigoRf);
            return (await database.Conexao.QueryAsync<AulaDto>(query.ToString(), new { tipoCalendarioId, turmaId, ueId, mes, CodigoRf }));
        }

        public async Task<IEnumerable<AulaDto>> ObterAulas(string turmaId, string ueId, string codigoRf, DateTime? data, string disciplinaId)
        {
            StringBuilder query = new StringBuilder();
            MontaCabecalho(query);
            query.AppendLine("FROM aula a");
            MontaWhere(query, null, turmaId, ueId, null, data, codigoRf, disciplinaId);

            return (await database.Conexao.QueryAsync<AulaDto>(query.ToString(), new
            {
                //mudará para int
                disciplinaId,
                turmaId,
                ueId,
                codigoRf,
                data = data.HasValue ? data.Value.Date : data
            }));
        }

        public async Task<IEnumerable<AulaCompletaDto>> ObterAulasCompleto(long tipoCalendarioId, string turmaId, string ueId, DateTime data, Guid perfil, bool turmaHistorico = false)
        {
            StringBuilder query = new StringBuilder();
            MontaCabecalho(query);
            query.AppendLine(",t.nome as turma_nome,");
            query.AppendLine("u.nome as ue_nome");
            query.AppendLine("FROM public.aula a");
            //query.AppendLine($"INNER JOIN {(turmaHistorico ? "v_abrangencia_historica" : "v_abrangencia")} ab");
            query.AppendLine($"INNER JOIN turma t");
            query.AppendLine("on a.turma_id = t.turma_id");
            query.AppendLine($"INNER JOIN ue u");
            query.AppendLine("on a.ue_id = u.ue_id");
            MontaWhere(query, tipoCalendarioId, turmaId, ueId, null, data);
            MontaGroupBy(query);

            return (await database.Conexao.QueryAsync<AulaCompletaDto>(query.ToString(), new { tipoCalendarioId, turmaId, ueId, data, perfil }));
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

        public async Task<IEnumerable<Aula>> ObterAulasRecorrencia(long aulaPaiId, long? aulaIdInicioRecorrencia = null, DateTime? dataFinal = null)
        {
            StringBuilder query = new StringBuilder();
            MontaCabecalho(query);
            query.AppendLine("FROM public.aula a");
            query.AppendLine("where not excluido");
            query.AppendLine(" and ((a.id = @aulaPaiId) or (a.aula_pai_id = @aulaPaiId))");

            if (aulaIdInicioRecorrencia.HasValue)
                query.AppendLine(" and a.id > @aulaIdInicioRecorrencia");

            if (dataFinal.HasValue)
                query.AppendLine(" and data_aula <= @dataFinal");

            query.AppendLine(" order by data_aula");

            return await database.Conexao.QueryAsync<Aula>(query.ToString(), new { aulaPaiId, aulaIdInicioRecorrencia, dataFinal });
        }

        public async Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaDisciplinaDiaProfessor(string turma, string disciplina, DateTime dataAula, string codigoRf)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select professor_rf, quantidade, data_aula");
            query.AppendLine("from aula ");
            query.AppendLine("where not excluido and tipo_aula = @aulaNomal ");

            if (!string.IsNullOrEmpty(codigoRf))
                query.AppendLine("and professor_rf = @codigoRf");

            query.AppendLine("and turma_id = @turma ");
            query.AppendLine("and disciplina_id = @disciplina ");
            query.AppendLine("and date(data_aula) = @dataAula ");

            return await database.Conexao.QueryAsync<AulasPorTurmaDisciplinaDto>(query.ToString(), new
            {
                codigoRf,
                turma,
                disciplina,
                dataAula = dataAula.Date,
                aulaNomal = TipoAula.Normal
            });
        }

        public async Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaDisciplinaSemanaProfessor(string turma, string disciplina, string semana, string codigoRf)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select professor_rf, quantidade, data_aula");
            query.AppendLine("from aula ");
            query.AppendLine("where not excluido and tipo_aula = @aulaNomal ");

            if (!string.IsNullOrEmpty(codigoRf))
                query.AppendLine("and professor_rf = @codigoRf");

            query.AppendLine("and turma_id = @turma ");
            query.AppendLine("and disciplina_id = @disciplina ");
            query.AppendLine("and to_char(data_aula, 'IW') = @semana ");

            return await database.Conexao.QueryAsync<AulasPorTurmaDisciplinaDto>(query.ToString(), new
            {
                codigoRf,
                turma,
                disciplina,
                semana,
                aulaNomal = TipoAula.Normal
            });
        }

        public async Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaExperienciasPedagogicasDia(string turma, DateTime dataAula)
        {
            var query = @"select professor_rf, quantidade, data_aula
                 from aula
                where not excluido
                  and turma_id = @turma
                  and disciplina_id in ('1214','1215','1216','1217','1218','1219','1220','1221','1222','1223')
                  and date(data_aula) = @dataAula";

            return await database.Conexao.QueryAsync<AulasPorTurmaDisciplinaDto>(query, new
            {
                turma,
                dataAula = dataAula.Date
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

        public IEnumerable<AulaConsultaDto> ObterDatasDeAulasPorAnoTurmaEDisciplina(long periodoEscolarId, int anoLetivo, string turmaCodigo, string disciplinaId, long usuarioId, string usuarioRF)
        {
            var query = new StringBuilder("select distinct a.* ");
            query.AppendLine("from aula a ");
            query.AppendLine("inner join turma t on ");
            query.AppendLine("a.turma_id = t.turma_id ");
            query.AppendLine("inner join periodo_escolar pe on pe.id = @periodoEscolarId ");
            query.AppendLine("                and pe.periodo_inicio <= a.data_aula ");
            query.AppendLine("                and pe.periodo_fim >= a.data_aula ");
            query.AppendLine("where");
            query.AppendLine("not a.excluido");
            query.AppendLine("and a.turma_id = @turmaCodigo ");
            query.AppendLine("and a.disciplina_id = @disciplinaId ");
            query.AppendLine("and t.ano_letivo = @anoLetivo");

            if (!string.IsNullOrWhiteSpace(usuarioRF))
            {
                query.AppendLine("and a.professor_rf = @usuarioRF ");
            }

            return database.Conexao.Query<AulaConsultaDto>(query.ToString(), new
            {
                periodoEscolarId,
                usuarioRF,
                usuarioId,
                anoLetivo,
                turmaCodigo,
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
            query.AppendLine("SELECT a.id,");
            query.AppendLine("a.aula_pai_id,");
            query.AppendLine("a.ue_id,");
            query.AppendLine("a.disciplina_id,");
            query.AppendLine("a.disciplina_compartilhada_id,");
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
            query.AppendLine("a.aula_cj,");
            query.AppendLine("a.migrado");
        }

        private static void MontaGroupBy(StringBuilder query)
        {
            query.AppendLine("group by t.turma_id,");
            query.AppendLine("a.id,");
            query.AppendLine("a.ue_id,");
            query.AppendLine("a.disciplina_id,");
            query.AppendLine("a.turma_id,");
            query.AppendLine("t.nome,");
            query.AppendLine("u.nome,");
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

        private static void MontaWhere(StringBuilder query, long? tipoCalendarioId, string turmaId, string ueId, int? mes = null, DateTime? data = null, string codigoRf = null, string disciplinaId = null, int? semanaAno = null, string[] disciplinasId = null)
        {
            query.AppendLine("WHERE a.excluido = false");
            query.AppendLine("AND a.status <> 3");

            if (tipoCalendarioId.HasValue)
                query.AppendLine("AND a.tipo_calendario_id = @tipoCalendarioId");
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
            if (!string.IsNullOrEmpty(codigoRf))
                query.AppendLine("AND a.professor_rf = @CodigoRf");
            if (!string.IsNullOrEmpty(disciplinaId))
                query.AppendLine("AND a.disciplina_id = @disciplinaId");
            if (disciplinasId != null && disciplinasId.Length > 0)
                query.AppendLine("AND a.disciplina_id = ANY(@disciplinasId)");
        }
    }
}