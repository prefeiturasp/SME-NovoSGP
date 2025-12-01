using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioAulaConsulta : RepositorioBase<Aula>, IRepositorioAulaConsulta
    {
        public RepositorioAulaConsulta(ISgpContextConsultas conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
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

        public async Task<bool> ExisteAulaNaDataAsync(DateTime data, string turmaCodigo, string componenteCurricular)
        {
            var query = @"select 1
                 from aula
                where not excluido
                  and DATE(data_aula) = @data
                  and turma_id = @turmaCodigo
                  and disciplina_id = @componenteCurricular";

            return (await database.Conexao.QueryAsync<int>(query, new
            {
                data = data.Date,
                turmaCodigo,
                componenteCurricular
            })).Any();
        }

        public async Task<bool> ExisteAulaNaDataDataTurmaDisciplinaProfessorRfAsync(DateTime data, string turmaCodigo, string[] componentesCurriculares, TipoAula tipoAula, string professorRf = null)
        {
            var query = $@"select 1
                 from aula
                where not excluido
                  and DATE(data_aula) = @data
                  and turma_id = @turmaCodigo
                  and disciplina_id = any(@componentesCurriculares)
                  {(string.IsNullOrEmpty(professorRf) ? string.Empty : " and professor_rf = @professorRf ")}
                  and tipo_aula = @tipoAula";

            return (await database.Conexao.QueryAsync<int>(query, new
            {
                data = data.Date,
                turmaCodigo,
                componentesCurriculares,
                professorRf,
                tipoAula
            })).Any();
        }

        public async Task<AulaConsultaDto> ObterAulaDataTurmaDisciplinaProfessorRf(DateTime data, string turmaId, string disciplinaId, string professorRf)
        {
            var query = @"select *
                 from aula
                where not excluido
                  and DATE(data_aula) = @data
                  and turma_id = @turmaId
                  and disciplina_id = @disciplinaId
                  and professor_rf = @professorRf";

            return await database.Conexao.QueryFirstOrDefaultAsync<AulaConsultaDto>(query, new
            {
                data = data.Date,
                turmaId,
                disciplinaId,
                professorRf
            });
        }

        public async Task<AulaConsultaDto> ObterAulaIntervaloTurmaDisciplina(DateTime dataInicio, DateTime dataFim, string turmaId, long atividadeAvaliativaId)
        {
            var query = @"select *,tipo_calendario_id AS TipoCalendarioId
                 from aula
                where not excluido
                  and DATE(data_aula) between @dataInicio and @dataFim
                  and turma_id = @turmaId
                  and disciplina_id in (
                select disciplina_id::varchar from atividade_avaliativa_disciplina aad
               where atividade_avaliativa_id = @atividadeAvaliativaId)";

            return await database.Conexao.QueryFirstOrDefaultAsync<AulaConsultaDto>(query, new
            {
                dataInicio = dataInicio.Date,
                dataFim = dataFim.Date,
                turmaId,
                atividadeAvaliativaId
            });
        }

        public async Task<IEnumerable<AulaDto>> ObterAulas(string turmaId, string ueId, string codigoRf, DateTime? data, string[] disciplinasId, bool ehCj)
        {
            StringBuilder query = new StringBuilder();
            MontaCabecalho(query);
            query.AppendLine("FROM public.aula a");
            MontaWhere(query, null, turmaId, ueId, null, data, codigoRf, null, null, disciplinasId, ehCj);
            return (await database.Conexao.QueryAsync<AulaDto>(query.ToString(), new { turmaId, ueId, data, codigoRf, disciplinasId }));
        }

        public async Task<IEnumerable<AulaDto>> ObterAulas(long tipoCalendarioId, string turmaId, string ueId, string codigoRf, int? mes = null)
        {
            StringBuilder query = new StringBuilder();
            MontaCabecalho(query);
            query.AppendLine("FROM public.aula a");
            MontaWhere(query, tipoCalendarioId, turmaId, ueId, mes, null, codigoRf);
            return (await database.Conexao.QueryAsync<AulaDto>(query.ToString(), new { tipoCalendarioId, turmaId, ueId, mes, codigoRf }));
        }

        public async Task<IEnumerable<AulaDto>> ObterAulas(string turmaId, string ueId, string codigoRf, DateTime? data, string disciplinaId)
        {
            StringBuilder query = new StringBuilder();
            MontaCabecalho(query);
            query.AppendLine("FROM aula a");
            MontaWhere(query, null, turmaId, ueId, null, data, codigoRf, disciplinaId);

            return (await database.Conexao.QueryAsync<AulaDto>(query.ToString(), new
            {
                disciplinaId,
                turmaId,
                ueId,
                codigoRf,
                data = data.HasValue ? data.Value.Date : data
            }));
        }

        public async Task<IEnumerable<AulaCompletaDto>> ObterAulasCompleto(long tipoCalendarioId, string turmaId, string ueId, DateTime data, Guid perfil)
        {
            StringBuilder query = new StringBuilder();
            MontaCabecalho(query);
            query.AppendLine(",t.nome as turma_nome,");
            query.AppendLine("u.nome as ue_nome");
            query.AppendLine("FROM public.aula a");
            query.AppendLine($"INNER JOIN turma t");
            query.AppendLine("on a.turma_id = t.turma_id");
            query.AppendLine($"INNER JOIN ue u");
            query.AppendLine("on a.ue_id = u.ue_id");
            MontaWhere(query, tipoCalendarioId, turmaId, ueId, null, data);
            MontaGroupBy(query);

            return (await database.Conexao.QueryAsync<AulaCompletaDto>(query.ToString(), new { tipoCalendarioId, turmaId, ueId, data, perfil }));
        }

        public async Task<IEnumerable<AulaConsultaDto>> ObterAulasPorDataTurmaComponenteCurricularProfessorRf(DateTime data, string turmaId, string[] disciplinasIdsConsideradas, string professorRf = null)
        {
            var query = @$"select *, tipo_aula as TipoAula
                 from aula
                where not excluido
                  and DATE(data_aula) = @data
                  and turma_id = @turmaId
                  and disciplina_id = any(@disciplinasIdsConsideradas)
                  {(!string.IsNullOrEmpty(professorRf) ? "and professor_rf = @professorRf" : string.Empty)}";

            return await database.Conexao.QueryAsync<AulaConsultaDto>(query, new
            {
                data = data.Date,
                turmaId,
                disciplinasIdsConsideradas,
                professorRf
            });
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

        public async Task<IEnumerable<Aula>> ObterAulasProfessorCalendarioPorData(string turmaCodigo, string ueCodigo, DateTime dataDaAula)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("SELECT a.id,");
            query.AppendLine("a.data_aula,");
            query.AppendLine("a.tipo_aula,");
            query.AppendLine("a.aula_cj,");
            query.AppendLine("a.disciplina_id,");
            query.AppendLine("a.quantidade,");
            query.AppendLine("a.status,");
            query.AppendLine("a.professor_rf,");
            query.AppendLine("a.tipo_calendario_id ");
            query.AppendLine("FROM public.aula a");
            query.AppendLine("WHERE a.excluido = false");
            query.AppendLine("AND a.status <> 3");
            query.AppendLine("AND a.turma_id = @turmaCodigo");
            query.AppendLine("AND a.data_aula::date = @dataDaAula");

            return (await database.Conexao.QueryAsync<Aula>(query.ToString(), new { turmaCodigo, ueCodigo, dataDaAula }));
        }

        public async Task<IEnumerable<Aula>> ObterAulasProfessorCalendarioPorMes(string turmaCodigo, string ueCodigo, int mes)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("SELECT distinct a.id,");
            query.AppendLine("                a.data_aula,");
            query.AppendLine("                a.tipo_aula,");
            query.AppendLine("                a.aula_cj,");
            query.AppendLine("                a.disciplina_id,");
            query.AppendLine("                a.professor_rf,");
            query.AppendLine("                a.tipo_calendario_id");
            query.AppendLine("  FROM public.aula a");
            query.AppendLine("      INNER JOIN turma t");
            query.AppendLine("          ON a.turma_id = t.turma_id");
            query.AppendLine("WHERE not a.excluido");
            query.AppendLine("AND a.status <> 3");
            query.AppendLine("AND a.turma_id = @turmaCodigo");
            query.AppendLine("AND extract(month from a.data_aula) = @mes");
            query.AppendLine("AND extract(year from a.data_aula) = t.ano_letivo");

            return (await database.Conexao.QueryAsync<Aula>(query.ToString(), new { turmaCodigo, ueCodigo, mes }));
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

        public IEnumerable<Aula> ObterAulasReposicaoPendentes(string codigoTurma, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            var query = @"select
                                *
                            from
                                aula
                            where
                                tipo_aula = 2
                                and status = 2
                                and turma_id = @codigoTurma
                                and disciplina_id = @disciplinaId
                                and data_aula >= @inicioPeriodo
                                and data_aula <= @fimPeriodo";
            return database.Conexao.Query<Aula>(query, new
            {
                codigoTurma,
                disciplinaId,
                inicioPeriodo,
                fimPeriodo
            });
        }

        public IEnumerable<Aula> ObterAulasSemFrequenciaRegistrada(string codigoTurma, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            var query = @"select a.*, t.id, t.modalidade_codigo
                            from aula a
                            inner join turma t on a.turma_id = t.turma_id
                            left join componente_curricular cc on a.disciplina_id::int8 = cc.id 
                            left join registro_frequencia r on r.aula_id = a.id
                           where a.turma_id = @codigoTurma
                                and disciplina_id = @disciplinaId
                                and data_aula >= @inicioPeriodo
                                and data_aula <= @fimPeriodo
                                and data_aula <= @dataAtual
                                and r.id is null
                                and coalesce(cc.permite_registro_frequencia, true)
                                and not a.excluido;";
            return database.Conexao.Query<Aula, Turma, Aula>(query, (aula, turma) =>
            {
                aula.Turma = turma;
                return aula;
            },
            new
            {
                codigoTurma,
                disciplinaId,
                inicioPeriodo,
                fimPeriodo,
                dataAtual = DateTimeExtension.HorarioBrasilia().Date
            }, splitOn: "id");
        }

        public IEnumerable<Aula> ObterAulasSemPlanoAulaNaDataAtual(string codigoTurma, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            var query = @"select *
                            from aula a
                            left join plano_aula p on p.aula_id = a.id and not p.excluido 
                           where turma_id = @codigoTurma
                                and disciplina_id = @disciplinaId
                                and data_aula >= @inicioPeriodo
                                and data_aula <= @fimPeriodo
                                and data_aula <= @dataAtual
                                and not a.excluido 
                                and p.id is null";

            return database.Conexao.Query<Aula>(query, new
            {
                codigoTurma,
                disciplinaId,
                inicioPeriodo,
                fimPeriodo,
                dataAtual = DateTimeExtension.HorarioBrasilia().Date
            });
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

        public async Task<int> ObterQuantidadeAulasTurmaComponenteCurricularDiaProfessor(string turma, string[] componentesCurriculares, DateTime dataAula, string codigoRf, bool ehGestor)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select sum(quantidade) ");
            query.AppendLine("from aula ");
            query.AppendLine("where not excluido and tipo_aula = @aulaNomal ");

            if (!string.IsNullOrEmpty(codigoRf) && !ehGestor)
                query.AppendLine("and professor_rf = @codigoRf");

            query.AppendLine("and turma_id = @turma ");
            query.AppendLine("and disciplina_id = any(@componentesCurriculares) ");
            query.AppendLine("and date(data_aula) = @dataAula ");

            var qtd = await database.Conexao.QueryFirstOrDefaultAsync<int?>(query.ToString(), new
            {
                codigoRf,
                turma,
                componentesCurriculares,
                dataAula = dataAula.Date,
                aulaNomal = TipoAula.Normal
            }) ?? 0;

            return qtd;
        }

        public async Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaDisciplinaSemanaProfessor(string turma, string[] disciplinas, int semana, string codigoRf)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select professor_rf, quantidade, data_aula");
            query.AppendLine("from aula ");
            query.AppendLine("where not excluido and tipo_aula = @aulaNomal ");

            if (!string.IsNullOrEmpty(codigoRf))
                query.AppendLine("and professor_rf = @codigoRf");

            query.AppendLine("and turma_id = @turma ");
            query.AppendLine("and disciplina_id = any(@disciplinas) ");
            query.AppendLine("and extract('week' from data_aula) = @semana ");

            return await database.Conexao.QueryAsync<AulasPorTurmaDisciplinaDto>(query.ToString(), new
            {
                codigoRf,
                turma,
                disciplinas,
                semana,
                aulaNomal = TipoAula.Normal
            });
        }

        public async Task<int> ObterQuantidadeAulasTurmaDisciplinaSemanaProfessor(string turma, string[] componentesCurriculares, int semana, string codigoRf, DateTime dataExcecao, bool ehGestor)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select sum(quantidade)");
            query.AppendLine("from aula ");
            query.AppendLine("where not excluido and tipo_aula = @aulaNomal ");
            query.AppendLine("and turma_id = @turma ");
            query.AppendLine("and disciplina_id = any(@componentesCurriculares) ");
            query.AppendLine("and extract('week' from data_aula::date + 1) = @semana");
            query.AppendLine("and Date(data_aula) <> @dataExcecao");

            if (!string.IsNullOrEmpty(codigoRf) && !ehGestor)
                query.AppendLine("and professor_rf = @codigoRf");

            var qtd = await database.Conexao.QueryFirstOrDefaultAsync<int?>(query.ToString(), new
            {
                codigoRf,
                turma,
                componentesCurriculares,
                semana,
                aulaNomal = TipoAula.Normal,
                dataExcecao
            }) ?? 0;

            return qtd;
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

        public async Task<int> ObterQuantidadeAulasTurmaExperienciasPedagogicasDia(string turma, DateTime dataAula)
        {
            var query = @"select sum(quantidade)
                 from aula
                where not excluido
                  and turma_id = @turma
                  and disciplina_id in ('1214','1215','1216','1217','1218','1219','1220','1221','1222','1223')
                  and date(data_aula) = @dataAula";

            var qtd = await database.Conexao.QueryFirstOrDefaultAsync<int?>(query, new
            {
                turma,
                dataAula = dataAula.Date
            }) ?? 0;
            database.Conexao.Close();

            return qtd;
        }

        public async Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaExperienciasPedagogicasSemana(string turma, int semana)
        {
            var query = @"select professor_rf, quantidade, data_aula
                 from aula
                where not excluido
                  and turma_id = @turma
                  and disciplina_id in ('1214','1215','1216','1217','1218','1219','1220','1221','1222','1223')
                  and extract('week' from data_aula) = @semana";

            return await database.Conexao.QueryAsync<AulasPorTurmaDisciplinaDto>(query, new
            {
                turma,
                semana
            });
        }

        public async Task<int> ObterQuantidadeAulasTurmaExperienciasPedagogicasSemana(string turma, int semana, string[] disciplinas)
        {
            var query = @"select sum(quantidade)
                 from aula
                where not excluido
                  and turma_id = @turma
                  and disciplina_id = any(@disciplinas)
                  and extract('week' from data_aula) = @semana";

            var qtd = await database.Conexao.QueryFirstOrDefaultAsync<int?>(query, new
            {
                turma,
                semana,
                disciplinas
            }) ?? 0;
            database.Conexao.Close();

            return qtd;
        }

        public async Task<Aula> ObterCompletoPorIdAsync(long id)
        {
            var query = @"select a.*,t.*, ue.*, dre.* from aula a
                            inner join turma t
                            on a.turma_id  = t.turma_id
                            inner join ue ue
                            on t.ue_id  = ue.id
                            inner join dre dre
                            on dre.id = ue.dre_id
                                where a.id  = @Id ";

            return (await database.Conexao.QueryAsync<Aula, Turma, Ue, Dre, Aula>(query,
                        (aula, turma, ue, dre) =>
                        {
                            turma.AdicionarUe(ue);
                            ue.AdicionarDre(dre);
                            aula.AtualizaTurma(turma);

                            return aula;
                        }, param: new { id })).FirstOrDefault();
        }

        public async Task<IEnumerable<DateTime>> ObterDatasAulasExistentes(List<DateTime> datas, string turmaId, string[] disciplinasId, bool aulaCJ, long? aulaPaiId = null)
        {
            var query = @"select DATE(data_aula)
                 from aula
                where not excluido
                  and DATE(data_aula) = ANY(@datas)
                  and turma_id = @turmaId
                  and disciplina_id = any(@disciplinasId)
                  and aula_cj = @aulaCJ";

            if (aulaPaiId.HasValue)
                query += " and ((aula_pai_id is null and id <> @aulaPaiId) or (aula_pai_id is not null and aula_pai_id <> @aulaPaiId))";

            return (await database.Conexao.QueryAsync<DateTime>(query.ToString(), new
            {
                datas,
                turmaId,
                disciplinasId,
                aulaCJ,
                aulaPaiId
            }));
        }

        public IEnumerable<Aula> ObterDatasDeAulasPorAnoTurmaEDisciplina(long periodoEscolarId, int anoLetivo, string turmaCodigo, string disciplinaId, string usuarioRF, bool aulaCJ = false, bool ehProfessor = false)
        {
            var query = new StringBuilder("select distinct a.*, t.* ");
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
                query.AppendLine("and a.professor_rf = @usuarioRF ");

            return database.Conexao.Query<Aula, Turma, Aula>(query.ToString(), (aula, turma) =>
            {
                aula.Turma = turma;
                return aula;
            }, new
            {
                periodoEscolarId,
                usuarioRF,
                anoLetivo,
                turmaCodigo,
                disciplinaId
            });
        }

        public async Task<IEnumerable<AulaPossuiFrequenciaAulaRegistradaDto>> ObterDatasDeAulasPorAnoTurmaEDisciplinaVerificandoSePossuiFrequenciaAulaRegistrada(IEnumerable<long> periodosEscolaresId, int anoLetivo, string turmaCodigo,
            string[] disciplinaId, string usuarioRF, DateTime? aulaInicio, DateTime? aulaFim, bool aulaCj)
        {
            var query = new StringBuilder(@"SELECT DISTINCT a.id,
                                                            a.data_aula AS DataAula,
                                                            a.aula_cj AS AulaCJ, 
                                                            a.professor_rf AS ProfessorRf,
                                                            a.criado_por AS CriadoPor, 
                                                            a.tipo_aula AS TipoAula,
                                                            CASE WHEN rf.id > 0 THEN TRUE ELSE false END PossuiFrequenciaRegistrada ");
            query.AppendLine("from aula a ");
            query.AppendLine("inner join turma t on ");
            query.AppendLine("a.turma_id = t.turma_id ");
            query.AppendLine("inner join periodo_escolar pe on pe.id = ANY(@periodoEscolarId) ");
            query.AppendLine("                and pe.periodo_inicio <= a.data_aula ");
            query.AppendLine("                and pe.periodo_fim >= a.data_aula ");
            query.AppendLine(" LEFT JOIN registro_frequencia rf ON rf.aula_id = a.id ");
            query.AppendLine("where");
            query.AppendLine("not a.excluido");
            query.AppendLine("and a.turma_id = @turmaCodigo ");
            query.AppendLine("and a.disciplina_id = any(@disciplinaId) ");
            query.AppendLine("and t.ano_letivo = @anoLetivo ");

            if (aulaInicio.HasValue && aulaFim.HasValue)
                query.AppendLine("and a.data_aula between @aulaInicio and @aulaFim ");

            if (!string.IsNullOrWhiteSpace(usuarioRF))
                query.AppendLine("and a.professor_rf = @usuarioRF ");

            if (aulaCj)
                query.AppendLine("and a.aula_cj = true");

            var parametros = new
            {
                periodoEscolarId = periodosEscolaresId.Select(c => c).ToArray(),
                usuarioRF,
                anoLetivo,
                turmaCodigo,
                disciplinaId,
                aulaInicio,
                aulaFim
            };
            return await database.Conexao.QueryAsync<AulaPossuiFrequenciaAulaRegistradaDto>(query.ToString(), parametros);
        }
        public IEnumerable<Aula> ObterDatasDeAulasPorAnoTurmaEDisciplina(IEnumerable<long> periodosEscolaresId, int anoLetivo, string turmaCodigo, string disciplinaId, string usuarioRF, DateTime? aulaInicio, DateTime? aulaFim, bool aulaCj)
        {
            var query = new StringBuilder("select distinct a.*, t.* ");
            query.AppendLine("from aula a ");
            query.AppendLine("inner join turma t on ");
            query.AppendLine("a.turma_id = t.turma_id ");
            query.AppendLine("inner join periodo_escolar pe on pe.id = ANY(@periodoEscolarId) ");
            query.AppendLine("                and pe.periodo_inicio <= a.data_aula ");
            query.AppendLine("                and pe.periodo_fim >= a.data_aula ");
            query.AppendLine("where");
            query.AppendLine("not a.excluido");
            query.AppendLine("and a.turma_id = @turmaCodigo ");
            query.AppendLine("and a.disciplina_id = @disciplinaId ");
            query.AppendLine("and t.ano_letivo = @anoLetivo ");

            if (aulaInicio.HasValue && aulaFim.HasValue)
                query.AppendLine("and a.data_aula between @aulaInicio and @aulaFim ");

            if (!string.IsNullOrWhiteSpace(usuarioRF))
                query.AppendLine("and a.professor_rf = @usuarioRF ");

            if (aulaCj)
                query.AppendLine("and a.aula_cj = true");

            return database.Conexao.Query<Aula, Turma, Aula>(query.ToString(), (aula, turma) =>
            {
                aula.Turma = turma;
                return aula;
            }, new
            {
                periodoEscolarId = periodosEscolaresId.Select(c => c).ToArray(),
                usuarioRF,
                anoLetivo,
                turmaCodigo,
                disciplinaId,
                aulaInicio,
                aulaFim
            });
        }
        public async Task<Aula> ObterPorWorkflowId(long workflowId)
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
                                 a.status,
                                 a.aula_cj 
                             from  aula a
                            where a.excluido = false
                              and a.migrado = false
                              and tipo_aula = 2
                              and a.wf_aprovacao_id = @workflowId";

            return await database.Conexao.QueryFirstAsync<Aula>(query.ToString(), new
            {
                workflowId
            });
        }

        public async Task<bool> VerificarAulaPorWorkflowId(long workflowId)
        {
            var query = @"select count(a.id)
                             from aula a
                            where not excluido
                              and not migrado
                              and tipo_aula = 2
                              and a.wf_aprovacao_id = @workflowId";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query.ToString(), new { workflowId });
        }

        public async Task<int> ObterQuantidadeDeAulasPorTurmaDisciplinaPeriodoAsync(string turmaId, string disciplinaId, DateTime inicio, DateTime fim)
        {
            var query = @"select count(id)
                          from aula
                         where turma_id = @turmaId
                          and disciplina_id = @disciplinaId
                          and data_aula between @inicio and @fim";

            return await database.Conexao.QueryFirstOrDefaultAsync<int?>(query, new
            {
                turmaId,
                disciplinaId,
                inicio,
                fim
            }) ?? 0;
        }

        public IEnumerable<DateTime> ObterUltimosDiasLetivos(DateTime dataReferencia, int quantidadeDias, long tipoCalendarioId)
        {
            var query = @"select distinct a.data_aula::date
                          from aula a
                         where not a.excluido
                           and a.data_aula <= @dataReferencia
                           and a.tipo_calendario_id = @tipoCalendarioId
                         order by a.data_aula::date  desc
                        limit @quantidadeDias";

            return database.Conexao.Query<DateTime>(query, new { dataReferencia, tipoCalendarioId, quantidadeDias });
        }

        private static void MontaCabecalho(StringBuilder query)
        {
            query.AppendLine("SELECT a.id,");
            query.AppendLine("a.aula_pai_id,");
            query.AppendLine("a.ue_id,");
            query.AppendLine("a.disciplina_id,");
            query.AppendLine("a.disciplina_compartilhada_id,");
            query.AppendLine("a.turma_id,");
            query.AppendLine("a.tipo_calendario_id TipoCalendarioId,");
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

        private static void MontaWhere(StringBuilder query, long? tipoCalendarioId, string turmaId, string ueId, int? mes = null, DateTime? data = null, string codigoRf = null, string disciplinaId = null, int? semanaAno = null, string[] disciplinasId = null, bool ehCj = false)
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
                query.AppendLine("AND DATE(a.data_aula) = @data::date");
            if (semanaAno.HasValue)
                query.AppendLine("AND extract(week from a.data_aula) = @semanaAno");
            if (!string.IsNullOrEmpty(codigoRf))
                query.AppendLine("AND a.professor_rf = @codigoRf");
            if (!string.IsNullOrEmpty(disciplinaId))
                query.AppendLine("AND a.disciplina_id = @disciplinaId");
            if (disciplinasId.NaoEhNulo() && disciplinasId.Length > 0)
                query.AppendLine("AND a.disciplina_id = ANY(@disciplinasId)");
            if (ehCj)
                query.AppendLine("AND a.aula_cj = true");
        }

        public async Task<DateTime> ObterDataAula(long aulaId)
        {
            var query = "select data_aula from aula where id = @aulaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<DateTime>(query, new { aulaId });
        }

        public async Task<IEnumerable<AulaConsultaDto>> ObterAulasPorDataTurmaComponenteCurricularCJ(DateTime dataAula, string codigoTurma, string componenteCurricularCodigo, bool aulaCJ)
        {
            var query = @"select *
                 from aula
                where not excluido
                  and DATE(data_aula) = @data
                  and turma_id = @codigoTurma
                  and disciplina_id = @componenteCurricularCodigo
                  and aula_cj = @aulaCJ";

            return await database.Conexao.QueryAsync<AulaConsultaDto>(query, new
            {
                data = dataAula.Date,
                codigoTurma,
                componenteCurricularCodigo,
                aulaCJ
            });
        }

        public async Task<IEnumerable<AulaConsultaDto>> ObterAulasPorDataTurmaComponenteCurricular(DateTime dataAula, string codigoTurma, string componenteCurricularCodigo)
        {
            var query = @"select id, ue_id Ueid, disciplina_id DisciplinaId, turma_id TurmaId,tipo_calendario_id TipoCalendarioId, professor_rf ProfessorRf, quantidade, 
                                 data_aula DataAula, recorrencia_aula RecorrenciaAula, tipo_aula TipoAula, criado_em CriadoEm, criado_por CriadoPor, alterado_em AlteradoEm, 
                                 alterado_por AlteradoPor, criado_rf CriadoRf, alterado_rf AlteradoRf, excluido, migrado, aula_cj AulaCj 
                 from aula
                where not excluido
                  and DATE(data_aula) = @data
                  and turma_id = @codigoTurma
                  and disciplina_id = @componenteCurricularCodigo";

            var retorno = await database.Conexao.QueryAsync<AulaConsultaDto>(query, new
            {
                data = dataAula.Date,
                codigoTurma,
                componenteCurricularCodigo
            });
            return retorno;
        }

        public async Task<bool> ObterTurmaInfantilPorAula(long aulaId)
        {
            var query = @"select t.modalidade_codigo
                            from aula a
                           inner join turma t on t.turma_id = a.turma_id
                           where a.id = @aulaId";

            var modalidade = await database.Conexao.QueryFirstAsync<int>(query, new { aulaId });

            return modalidade == (int)Modalidade.EducacaoInfantil;
        }

        public async Task<IEnumerable<Aula>> ObterAulasPorTurmaETipoCalendario(long tipoCalendarioId, string turmaId, string criadoPor = null)
        {
            var query = @"select a.*,
                                 a.id aula_id,
                                 rf.id is not null PossuiFrequencia,
                                 rf.id is not null and rf.excluido RegistroFerquenciaExcluido,
                                 pa.id is not null PossuiPlanoAula,
                                 pa.id is not null and pa.excluido RegistroPlanoAulaExcluido
                            from aula a
                                inner join turma t
                                    on a.turma_id = t.turma_id
                                left join registro_frequencia rf
                                    on a.id = rf.aula_id
                                left join plano_aula pa
                                    on a.id = pa.aula_id
                          where a.tipo_calendario_id = @tipoCalendarioId and
                                a.excluido = false and
                                a.turma_id = @turmaId";

            var criadoRf = new string[] { criadoPor?.ToUpper() };

            if (!string.IsNullOrWhiteSpace(criadoPor))
            {
                if (criadoPor.Equals("Sistema", StringComparison.InvariantCultureIgnoreCase))
                    criadoRf = criadoRf.Concat(new string[] { "0", string.Empty }).ToArray();

                query += @" and upper(a.criado_por) = upper(@criadoPor)
                            and upper(a.criado_rf) = any(@criadoRf) ";
            }

            query += " order by a.data_aula, a.id;";

            return await database.Conexao.QueryAsync<Aula, AulaDadosComplementares, Aula>(query.ToString(), (aula, dadosComplementares) =>
            {
                aula.DadosComplementares = new AulaDadosComplementares()
                {
                    PossuiFrequencia = dadosComplementares.PossuiFrequencia,
                    PossuiPlanoAula = dadosComplementares.PossuiPlanoAula,
                    RegistroFrequenciaExcluido = dadosComplementares.RegistroFrequenciaExcluido,
                    RegistroPlanoAulaExcluido = dadosComplementares.RegistroPlanoAulaExcluido
                };
                return aula;
            }, new { tipoCalendarioId, turmaId, criadoPor, criadoRf }, splitOn: "aula_id");
        }

        public async Task<IEnumerable<AulaReduzidaDto>> ObterAulasReduzidasParaPendenciasAulaDiasNaoLetivos(long tipoCalendarioId, TipoEscola[] tiposEscola)
        {
            var query = @"select
                                  a.id as aulaId,
                               a.data_aula as Data,
                               a.quantidade as Quantidade,
                               a.criado_por as Professor,
                               a.criado_rf as ProfessorRf,
                               a.turma_id as TurmaId,
                               a.disciplina_id as DisciplinaId,
                               ue.ue_id as CodigoUe,
                               dre.dre_id as CodigoDre,
                               t.id as IdTurma
                          from aula a 
                        inner join turma t on t.turma_id = a.turma_id
                        inner join ue on ue.id = t.ue_id
                        inner join dre on dre.id = ue.dre_id
                        where not excluido and tipo_calendario_id = @tipoCalendarioId ";

            int[] tiposEscolaFiltro = null;
            if (tiposEscola?.Any() ?? false)
            {
                tiposEscolaFiltro = tiposEscola.Select(x => (int)x).ToArray();
                query += " AND ue.tipo_escola = any(@tiposEscolaFiltro)";
            }

            return await database.Conexao.QueryAsync<AulaReduzidaDto>(query, new { tipoCalendarioId, tiposEscolaFiltro });
        }

        public async Task<Aula> ObterAulaPorComponenteCurricularIdTurmaIdEData(string componenteCurricularId, string turmaId, DateTime data)
        {
            var query = @"select a.* from aula a
                                where a.disciplina_id  = @ccid and turma_id = @turmaid and data_aula::date = @data";

            return (await database.Conexao.QueryAsync<Aula>(query, new { ccid = componenteCurricularId, turmaid = turmaId, data = data.Date })).FirstOrDefault();
        }

        public async Task<long?> ObterAulaIdPorComponenteCurricularIdTurmaIdEDataProfessor(string componenteCurricularId, string turmaId, DateTime data, string professorRf)
        {
            var query = @"select a.id from aula a
                                where a.disciplina_id  = @ccid and turma_id = @turmaid and data_aula::date = @data and a.professor_rf = @professorRf";
            return (await database.Conexao.QueryAsync<long?>(query, new { ccid = componenteCurricularId, turmaid = turmaId, data = data.Date, professorRf })).FirstOrDefault();
        }
        public async Task<IEnumerable<AulaReduzidaDto>> ObterQuantidadeAulasReduzido(long turmaId, string componenteCurricularId, long tipoCalendarioId, int bimestre, bool professorCJ)
        {
            var query = @"select
                            a.data_aula as Data,
                            sum(a.quantidade) as Quantidade,
                            a.criado_por as Professor,
                            a.criado_rf as ProfessorRf
                        from
                            aula a
                        inner join turma t on
                            a.turma_id = t.turma_id
                        inner join periodo_escolar pe on
                            a.data_aula between pe.periodo_inicio and pe.periodo_fim
                        where
                            disciplina_id = @componenteCurricularId
                            and t.id = @turmaId
                            and pe.bimestre = @bimestre
                            and not a.excluido 
                            and pe.tipo_calendario_id = @tipoCalendarioId
                            and a.aula_cj = @professorCJ
                        group by
                            a.data_aula,
                            a.criado_por,
                            a.criado_por,
                            a.criado_rf
                        order by
                            data_aula";

            return (await database.Conexao.QueryAsync<AulaReduzidaDto>(query, new { turmaId, componenteCurricularId, tipoCalendarioId, bimestre, professorCJ }));

        }

        public async Task<int> ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolar(string turmaCodigo, long[] componentesCurricularesId, long tipoCalendarioId, IEnumerable<long> periodosEscolaresIds, string professor = null)
        {
            var sql = $@"select sum(quantidade) from (
                                 select distinct a.id, a.quantidade as quantidade
                                                 from 
                                                     aula a 
                                                 inner join 
                                                     periodo_escolar pe 
                                                     on a.data_aula BETWEEN pe.periodo_inicio AND pe.periodo_fim
                                                 inner join
                                                     registro_frequencia rf
                                                     on a.id = rf.aula_id
                                                 where 
                                                     not a.excluido
                                                     and not rf.excluido
                                                     and a.turma_id = @turmaCodigo
                                                     and a.disciplina_id = any(@componentesCurricularesId)
                                                     and a.tipo_calendario_id = @tipoCalendarioId
                                                     and pe.id = ANY(@periodosEscolaresIds)
                                                     {(!string.IsNullOrEmpty(professor) ? " and a.professor_rf = @professor " : string.Empty)}
                                 group by a.id) x";

            var parametros = new { turmaCodigo, componentesCurricularesId = componentesCurricularesId.Select(cc => cc.ToString()).ToArray(), tipoCalendarioId, periodosEscolaresIds = periodosEscolaresIds.ToList(), professor };
            return await database.Conexao.QueryFirstOrDefaultAsync<int?>(sql, parametros) ?? default;
        }

        public async Task<int> ObterAulasDadasPorTurmaEPeriodoEscolar(long turmaId, long tipoCalendarioId, IEnumerable<long> periodosEscolaresIds)
        {
            const string sql = @"select 
                                    sum(a.quantidade)
                                from 
                                    aula a 
                                inner join
                                    turma t
                                    on a.turma_id = t.turma_id
                                inner join 
                                    periodo_escolar pe 
                                    on a.data_aula BETWEEN pe.periodo_inicio AND pe.periodo_fim
                                inner join
                                    registro_frequencia rf 
                                    on a.id = rf.aula_id
                                where 
                                    not a.excluido
                                    and t.id = @turmaId
                                    and a.tipo_calendario_id = @tipoCalendarioId
                                    and pe.id = any(@periodosEscolaresIds)";

            var parametros = new { turmaId, tipoCalendarioId, periodosEscolaresIds = periodosEscolaresIds.ToList() };
            return await database.Conexao.QueryFirstOrDefaultAsync<int?>(sql, parametros) ?? default;
        }

        public async Task<IEnumerable<Aula>> ObterAulasExcluidasComDiarioDeBordoAtivos(string codigoTurma, long tipoCalendarioId)
        {
            var sqlQuery = @"select a.*
                                from diario_bordo db
                                    inner join aula a
                                        on db.aula_id = a.id
                                    inner join turma t
                                        on a.turma_id = t.turma_id
                             where t.turma_id = @codigoTurma and
                                   a.tipo_calendario_id = @tipoCalendarioId and
                                   a.excluido and
                                   not db.excluido and
                                   a.criado_por = 'Sistema' and
                                   a.criado_rf = 'Sistema';";

            return (await database.Conexao.QueryAsync<Aula>(sqlQuery, new { codigoTurma, tipoCalendarioId }));
        }

        public async Task<PeriodoEscolarInicioFimDto> ObterPeriodoEscolarDaAula(long aulaId)
        {
            var query = @"select pe.id, pe.bimestre, pe.periodo_inicio as DataInicio, pe.periodo_fim as DataFim
                          from aula a
                         inner join periodo_escolar pe on pe.tipo_calendario_id = a.tipo_calendario_id 
                         where a.id = @aulaId
                           and a.data_aula between pe.periodo_inicio and pe.periodo_fim ";

            return await database.Conexao.QueryFirstOrDefaultAsync<PeriodoEscolarInicioFimDto>(query, new { aulaId });
        }

        public async Task<IEnumerable<PeriodoEscolarAulaDto>> ObterPeriodosEscolaresDasAulas(long[] aulasId)
        {
            var query = @"select pe.id, pe.bimestre, pe.periodo_inicio as DataInicio, pe.periodo_fim as DataFim, a.data_aula as DataAula, a.id as AulaId
                          from aula a
                         inner join periodo_escolar pe on pe.tipo_calendario_id = a.tipo_calendario_id 
                         where a.id = any(@aulasId)
                           and a.data_aula between pe.periodo_inicio and pe.periodo_fim ";

            return await database.Conexao.QueryAsync<PeriodoEscolarAulaDto>(query, new { aulasId });
        }

        public async Task<DataAulaDto> ObterAulaPorCodigoTurmaComponenteEData(string turmaId, string componenteCurricularId, DateTime dataCriacao)
        {
            var query = @"select a.id as AulaId,
                                 a.data_aula as DataAula,       
                                 (t.modalidade_codigo = 1) as EhModalidadeInfantil   
                         from aula a
                         join turma t on a.turma_id = t.turma_id 
                         where not a.excluido 
                            and t.turma_id = @turmaId
                            and a.disciplina_id = @componenteCurricularId
                            and a.data_aula >= @dataCriacao
                        order by data_aula
                        limit 1";

            return await database.Conexao.QueryFirstOrDefaultAsync<DataAulaDto>(query, new { turmaId, componenteCurricularId, dataCriacao = dataCriacao.Date });
        }

        public Task<IEnumerable<Aula>> ObterAulasPorDataPeriodo(DateTime dataInicio, DateTime dataFim, string turmaId, string[] disciplinasId, bool aulaCj, string professor = null)
        {
            var query = new StringBuilder(@"select *
                 from aula
                where not excluido
                  and DATE(data_aula) between Date(@dataInicio) and Date(@dataFim)
                  and turma_id = @turmaId
                  and disciplina_id = any(@disciplinasId)");

            if (aulaCj)
                query.AppendLine(" and aula_cj");

            if (!string.IsNullOrWhiteSpace(professor))
                query.AppendLine(" and professor_rf = @professor");

            return database.Conexao.QueryAsync<Aula>(query.ToString(), new
            {
                dataInicio = dataInicio.Date,
                dataFim = dataFim.Date,
                turmaId,
                disciplinasId,
                professor
            });
        }

        public async Task<IEnumerable<DiarioBordoPorPeriodoDto>> ObterDatasAulaDiarioBordoPorPeriodo(string turmaCodigo, long componenteCurricularId, DateTime dataInicio, DateTime dataFim)
        {
            var query = @"select a.id as AulaId, 
                            db.id as DiarioBordoId, 
                            a.data_aula as DataAula, 
                            db.planejamento as Planejamento 
                            from aula a 
                            left join diario_bordo db on db.aula_id = a.id 
                            where a.turma_id = @turmaCodigo 
                            and a.data_aula between @dataInicio and @dataFim
                            and db.componente_curricular_id = @componenteCurricularId
                            and not a.excluido and not db.excluido
                            order by a.data_aula desc";

            return await database.Conexao.QueryAsync<DiarioBordoPorPeriodoDto>(query, new { turmaCodigo, componenteCurricularId, dataInicio, dataFim });
        }

        public async Task<IEnumerable<DiarioBordoPorPeriodoDto>> ObterAulasDiariosPorPeriodo(string turmaCodigo, string componenteCurricularFilhoCodigo, string componenteCurricularPaiCodigo, DateTime dataFim, DateTime dataInicio)
        {

            var query = @"
                         select distinct db.id as DiarioBordoId, a.data_aula DataAula, a.id as AulaId, db.criado_rf CodigoRf,
                                db.criado_por Nome, db.planejamento as Planejamento, 
                                a.tipo_aula as Tipo, db.inserido_cj as InseridoCJ, 
                                case when db.id is null then true else false end Pendente
                                ,db.id, db.alterado_em as AlteradoEm, db.alterado_por as AlteradoPor, db.alterado_rf as AlteradoRF, db.criado_em as CriadoEm, db.criado_por as CriadoPor, db.criado_rf as CriadoRF
                         from aula a
                         inner join turma t on a.turma_id = t.turma_id
                         left join diario_bordo db on a.id = db.aula_id and db.componente_curricular_id = @componenteCurricularFilho and not db.excluido
                         where t.turma_id = @turmaCodigo
                           and a.disciplina_id = @componenteCurricularPaiCodigo 
                           and not a.excluido
                           and a.data_aula >= @dataInicio
                           and a.data_aula <= @dataFim ";

            var lookup = new List<DiarioBordoPorPeriodoDto>();
            await database.Conexao.QueryAsync<DiarioBordoPorPeriodoDto, AuditoriaDto, DiarioBordoPorPeriodoDto>(query, (diarioBordoPorPeriodoDto, auditoriaDto) =>
                 {
                     if (auditoriaDto.NaoEhNulo())
                         diarioBordoPorPeriodoDto.Auditoria = auditoriaDto;

                     lookup.Add(diarioBordoPorPeriodoDto);

                     return diarioBordoPorPeriodoDto;
                 }, new
                 {
                     turmaCodigo,
                     componenteCurricularFilho = int.Parse(componenteCurricularFilhoCodigo),
                     componenteCurricularPaiCodigo,
                     dataFim,
                     dataInicio
                 });

            return lookup;

        }

        public async Task<IEnumerable<Aula>> ObterAulasPorIds(IEnumerable<long> aulasIds)
        {
            var query = "select * from aula where id = ANY(@aulasIds)";

            return await database.Conexao.QueryAsync<Aula>(query, new { aulasIds = aulasIds.ToList() });
        }

        public async Task<IEnumerable<TotalAulasNaoLancamNotaDto>> ObterTotalAulasPorTurmaDisciplinaAluno(string disciplinaId, string codigoTurma, string codigoAluno)
        {
            var sql = @"select disciplina_id as disciplinaid, SUM(total_aulas) as TotalAulas from frequencia_aluno fa 
                        where tipo = 1 
                        and disciplina_id = @disciplinaId
                        and codigo_aluno = @codigoAluno
                        and turma_id = @codigoTurma 
                        group by disciplina_id ";

            return await database.Conexao.QueryAsync<TotalAulasNaoLancamNotaDto>(sql, new { disciplinaId, codigoAluno, codigoTurma });
        }

        public async Task<IEnumerable<RegistroFrequenciaAulaParcialDto>> ObterListaDeRegistroFrequenciaAulaPorTurma(string codigoTurma)
        {
            var sql = @"SELECT rf.id as RegistroFrequenciaId, rf.aula_id as AulaId
                            FROM aula a
                              INNER JOIN turma t ON a.turma_id = t.turma_id
                              INNER JOIN registro_frequencia rf ON rf.aula_id = a.id
                             where t.turma_id = @codigoTurma";

            return await database.Conexao.QueryAsync<RegistroFrequenciaAulaParcialDto>(sql, new { codigoTurma });
        }

        public async Task<bool> ExisteAulaNoPeriodoTurmaDisciplinaAsync(DateTime periodoInicio, DateTime periodoFim, string turmaCodigo, string disciplinaId)
        {
            var tipoAula = TipoAula.Normal;

            var query = @"SELECT 1
                          FROM aula
                          WHERE NOT excluido
                            AND data_aula::date BETWEEN @periodoInicio AND @periodoFim
                            AND turma_id = @turmaCodigo
                            AND disciplina_id = @disciplinaId
                            AND tipo_aula = @tipoAula
                          LIMIT 1";

            return (await database.Conexao.QueryAsync<int>(query, new
            {
                periodoInicio = periodoInicio.Date,
                periodoFim = periodoFim.Date,
                turmaCodigo,
                disciplinaId,
                tipoAula
            })).Any();
        }

        public async Task<IEnumerable<AulaConsultaDto>> ObterAulasFuturasPorDataTurmaComponentesCurriculares(DateTime dataBase, string codigoTurma, string[] componentesCurricularesCodigo)
        {
            var query = @"select id, ue_id Ueid, disciplina_id DisciplinaId, turma_id TurmaId,
                                 tipo_calendario_id TipoCalendarioId, professor_rf ProfessorRf, quantidade, 
                                 data_aula DataAula, recorrencia_aula RecorrenciaAula, tipo_aula TipoAula, 
                                 migrado, aula_cj AulaCj 
                          from aula
                          where not excluido
                                and DATE(data_aula) >= @data
                                and turma_id = @codigoTurma
                                and disciplina_id = any(@componentesCurricularesCodigo) ";

            return await database.Conexao.QueryAsync<AulaConsultaDto>(query, new
            {
                data = dataBase.Date,
                codigoTurma,
                componentesCurricularesCodigo
            });
        }
    }
}