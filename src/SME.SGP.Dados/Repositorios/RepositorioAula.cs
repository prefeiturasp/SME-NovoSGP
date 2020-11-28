﻿using Dapper;
using Dommel;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
            })).Count() > 0;
        }

        public async Task<bool> ExisteAulaNaDataDataTurmaDisciplinaProfessorRfAsync(DateTime data, string turmaCodigo, string componenteCurricular, string professorRf)
        {
            var query = @"select 1
                 from aula
                where not excluido
                  and DATE(data_aula) = @data
                  and turma_id = @turmaCodigo
                  and disciplina_id = @componenteCurricular
                  and professor_rf = @professorRf";

            return (await database.Conexao.QueryAsync<int>(query, new
            {
                data = data.Date,
                turmaCodigo,
                componenteCurricular,
                professorRf
            })).Count() > 0;
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
            var query = @"select *
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

        public async Task<IEnumerable<AulaDto>> ObterAulas(long tipoCalendarioId, string turmaId, string ueId, string codigoRf)
        {
            StringBuilder query = new StringBuilder();
            MontaCabecalho(query);
            query.AppendLine("FROM public.aula a");
            MontaWhere(query, tipoCalendarioId, turmaId, ueId, null, null, codigoRf);
            return (await database.Conexao.QueryAsync<AulaDto>(query.ToString(), new { tipoCalendarioId, turmaId, ueId, codigoRf }));
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
                //mudará para int
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

        public async Task<IEnumerable<AulaConsultaDto>> ObterAulasPorDataTurmaComponenteCurricularProfessorRf(DateTime data, string turmaId, string disciplinaId, string professorRf)
        {
            var query = @"select *
                 from aula
                where not excluido
                  and DATE(data_aula) = @data
                  and turma_id = @turmaId
                  and disciplina_id = @disciplinaId
                  and professor_rf = @professorRf";

            return await database.Conexao.QueryAsync<AulaConsultaDto>(query, new
            {
                data = data.Date,
                turmaId,
                disciplinaId,
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

        public async Task<IEnumerable<Aula>> ObterAulasProfessorCalendarioPorData(long tipoCalendarioId, string turmaCodigo, string ueCodigo, DateTime dataDaAula)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("SELECT a.id,");
            query.AppendLine("a.data_aula,");
            query.AppendLine("a.tipo_aula,");
            query.AppendLine("a.aula_cj,");
            query.AppendLine("a.disciplina_id,");
            query.AppendLine("a.quantidade,");
            query.AppendLine("a.status,");
            query.AppendLine("a.professor_rf");
            query.AppendLine("FROM public.aula a");
            query.AppendLine("WHERE a.excluido = false");
            query.AppendLine("AND a.status <> 3");
            query.AppendLine("AND a.tipo_calendario_id = @tipoCalendarioId");
            query.AppendLine("AND a.turma_id = @turmaCodigo");
            query.AppendLine("AND a.data_aula::date = @dataDaAula");

            return (await database.Conexao.QueryAsync<Aula>(query.ToString(), new { tipoCalendarioId, turmaCodigo, ueCodigo, dataDaAula }));
        }

        public async Task<IEnumerable<Aula>> ObterAulasProfessorCalendarioPorMes(long tipoCalendarioId, string turmaCodigo, string ueCodigo, int mes)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("SELECT a.id,");
            query.AppendLine("a.data_aula,");
            query.AppendLine("a.tipo_aula,");
            query.AppendLine("a.aula_cj,");
            query.AppendLine("a.disciplina_id,");
            query.AppendLine("a.professor_rf");
            query.AppendLine("FROM public.aula a");
            query.AppendLine("WHERE a.excluido = false");
            query.AppendLine("AND a.status <> 3");
            query.AppendLine("AND a.tipo_calendario_id = @tipoCalendarioId");
            query.AppendLine("AND a.turma_id = @turmaCodigo");
            query.AppendLine("AND extract(month from a.data_aula) = @mes");

            return (await database.Conexao.QueryAsync<Aula>(query.ToString(), new { tipoCalendarioId, turmaCodigo, ueCodigo, mes }));
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
            var query = @"select *
                            from aula a
                            left join registro_frequencia r on r.aula_id = a.id
                           where turma_id = @codigoTurma
	                            and disciplina_id = @disciplinaId
	                            and data_aula >= @inicioPeriodo
	                            and data_aula <= @fimPeriodo
                                and data_aula <= @dataAtual
	                            and r.id is null";
            return database.Conexao.Query<Aula>(query, new
            {
                codigoTurma,
                disciplinaId,
                inicioPeriodo,
                fimPeriodo,
                dataAtual = DateTime.Now
            });
        }

        public IEnumerable<Aula> ObterAulasSemPlanoAulaNaDataAtual(string codigoTurma, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            var query = @"select *
                            from aula a
                            left join plano_aula p on p.aula_id = a.id
                           where turma_id = @codigoTurma
                                and disciplina_id = @disciplinaId
                                and data_aula >= @inicioPeriodo
                                and data_aula <= @fimPeriodo
                                and data_aula <= @dataAtual
                                and p.id is null";

            return database.Conexao.Query<Aula>(query, new
            {
                codigoTurma,
                disciplinaId,
                inicioPeriodo,
                fimPeriodo,
                dataAtual = DateTime.Today
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

        public async Task<int> ObterQuantidadeAulasTurmaComponenteCurricularDiaProfessor(string turma, string componenteCurricular, DateTime dataAula, string codigoRf)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select sum(quantidade) ");
            query.AppendLine("from aula ");
            query.AppendLine("where not excluido and tipo_aula = @aulaNomal ");

            if (!string.IsNullOrEmpty(codigoRf))
                query.AppendLine("and professor_rf = @codigoRf");

            query.AppendLine("and turma_id = @turma ");
            query.AppendLine("and disciplina_id = @componenteCurricular ");
            query.AppendLine("and date(data_aula) = @dataAula ");

            var qtd = await database.Conexao.QueryFirstOrDefaultAsync<int?>(query.ToString(), new
            {
                codigoRf,
                turma,
                componenteCurricular,
                dataAula = dataAula.Date,
                aulaNomal = TipoAula.Normal
            }) ?? 0;
            database.Conexao.Close();

            return qtd;
        }

        public async Task<IEnumerable<AulasPorTurmaDisciplinaDto>> ObterAulasTurmaDisciplinaSemanaProfessor(string turma, string disciplina, int semana, string codigoRf)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select professor_rf, quantidade, data_aula");
            query.AppendLine("from aula ");
            query.AppendLine("where not excluido and tipo_aula = @aulaNomal ");

            if (!string.IsNullOrEmpty(codigoRf))
                query.AppendLine("and professor_rf = @codigoRf");

            query.AppendLine("and turma_id = @turma ");
            query.AppendLine("and disciplina_id = @disciplina ");
            query.AppendLine("and extract('week' from data_aula) = @semana ");

            return await database.Conexao.QueryAsync<AulasPorTurmaDisciplinaDto>(query.ToString(), new
            {
                codigoRf,
                turma,
                disciplina,
                semana,
                aulaNomal = TipoAula.Normal
            });
        }

        public async Task<int> ObterQuantidadeAulasTurmaDisciplinaSemanaProfessor(string turma, string componenteCurricular, int semana, string codigoRf, DateTime dataExcecao)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select sum(quantidade)");
            query.AppendLine("from aula ");
            query.AppendLine("where not excluido and tipo_aula = @aulaNomal ");

            if (!string.IsNullOrEmpty(codigoRf))
                query.AppendLine("and professor_rf = @codigoRf");

            query.AppendLine("and turma_id = @turma ");
            query.AppendLine("and disciplina_id = @componenteCurricular ");
            query.AppendLine("and extract('week' from data_aula) = @semana ");
            query.AppendLine("and Date(data_aula) <> @dataExcecao");

            var qtd = await database.Conexao.QueryFirstOrDefaultAsync<int?>(query.ToString(), new
            {
                codigoRf,
                turma,
                componenteCurricular,
                semana,
                aulaNomal = TipoAula.Normal,
                dataExcecao
            }) ?? 0;
            database.Conexao.Close();

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

        public async Task<int> ObterQuantidadeAulasTurmaExperienciasPedagogicasSemana(string turma, int semana)
        {
            var query = @"select sum(quantidade)
                 from aula
                where not excluido
                  and turma_id = @turma
                  and disciplina_id in ('1214','1215','1216','1217','1218','1219','1220','1221','1222','1223')
                  and extract('week' from data_aula) = @semana";

            var qtd = await database.Conexao.QueryFirstOrDefaultAsync<int?>(query, new
            {
                turma,
                semana
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

        public async Task<IEnumerable<DateTime>> ObterDatasAulasExistentes(List<DateTime> datas, string turmaId, string disciplinaId, bool aulaCJ, long? aulaPaiId = null)
        {
            var query = @"select DATE(data_aula)
                 from aula
                where not excluido
                  and DATE(data_aula) = ANY(@datas)
                  and turma_id = @turmaId
                  and disciplina_id = @disciplinaId
                  and aula_cj = @aulaCJ";

            if (aulaPaiId.HasValue)
                query += " and ((aula_pai_id is null and id <> @aulaPaiId) or (aula_pai_id is not null and aula_pai_id <> @aulaPaiId))";

            return (await database.Conexao.QueryAsync<DateTime>(query.ToString(), new
            {
                datas,
                turmaId,
                disciplinaId,
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
            {
                query.AppendLine("and a.professor_rf = @usuarioRF ");
            }

            if (ehProfessor)
            {
                var filtroAulaCJ = aulaCJ ? "" : "not";
                query.AppendLine($"and {filtroAulaCJ} a.aula_cj ");
            }

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

        public bool VerificarAulaPorWorkflowId(long workflowId)
        {
            var query = @"select count(a.id)
                             from aula a
                            where a.excluido = false
                              and a.migrado = false
                              and tipo_aula = 2
                              and a.wf_aprovacao_id = @workflowId";

            int qtde = database.Conexao.QueryFirst<int>(query.ToString(), new
            {
                workflowId
            });

            return qtde > 0 ? true : false;
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
                query.AppendLine("AND a.professor_rf = @codigoRf");
            if (!string.IsNullOrEmpty(disciplinaId))
                query.AppendLine("AND a.disciplina_id = @disciplinaId");
            if (disciplinasId != null && disciplinasId.Length > 0)
                query.AppendLine("AND a.disciplina_id = ANY(@disciplinasId)");
        }

        public async Task<DateTime> ObterDataAula(long aulaId)
        {
            var query = "select data_aula from aula where id = @aulaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<DateTime>(query, new { aulaId });
        }

        public async Task<IEnumerable<AulaConsultaDto>> ObterAulasPorDataTurmaComponenteCurricular(DateTime dataAula, string codigoTurma, string componenteCurricularCodigo, bool aulaCJ)
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

        public async Task<bool> ObterTurmaInfantilPorAula(long aulaId)
        {
            var query = @"select t.modalidade_codigo
                            from aula a
                           inner join turma t on t.turma_id = a.turma_id
                           where a.id = @aulaId";

            var modalidade = await database.Conexao.QueryFirstAsync<int>(query, new { aulaId });

            return modalidade == (int)Modalidade.Infantil;
        }

        public async Task<IEnumerable<Aula>> ObterAulasPorTurmaETipoCalendario(long tipoCalendarioId, string turmaId)
        {
            var query = @"select * from aula where tipo_calendario_id = @tipoCalendarioId and turma_id = @turmaId and not excluido";
            return await database.Conexao.QueryAsync<Aula>(query.ToString(), new { tipoCalendarioId, turmaId });
        }

        public void SalvarVarias(IEnumerable<Aula> aulas)
        {
            var sql = @"copy aula ( 
                                        data_aula, 
                                        disciplina_id, 
                                        quantidade, 
                                        recorrencia_aula, 
                                        tipo_aula, 
                                        tipo_calendario_id, 
                                        turma_id, 
                                        ue_id, 
                                        professor_rf,
                                        criado_em,
                                        criado_por,
                                        criado_rf)
                            from
                            stdin (FORMAT binary)";
            using (var writer = ((NpgsqlConnection)database.Conexao).BeginBinaryImport(sql))
            {
                foreach (var aula in aulas)
                {
                    writer.StartRow();
                    writer.Write(aula.DataAula);
                    writer.Write(aula.DisciplinaId);
                    writer.Write(aula.Quantidade);
                    writer.Write((int)aula.RecorrenciaAula, NpgsqlDbType.Integer);
                    writer.Write((int)aula.TipoAula, NpgsqlDbType.Integer);
                    writer.Write(aula.TipoCalendarioId);
                    writer.Write(aula.TurmaId);
                    writer.Write(aula.UeId);
                    writer.Write(aula.ProfessorRf);
                    writer.Write(aula.CriadoEm);
                    writer.Write("Sistema");
                    writer.Write("Sistema");
                }
                writer.Complete();
            }
        }

        public async Task ExcluirPeloSistemaAsync(long[] idsAulas)
        {
            var sql = "update aula set excluido = true, alterado_por = @alteradoPor, alterado_em = @alteradoEm, alterado_rf = @alteradoRf where id = any(@idsAulas)";
            await database.Conexao.ExecuteAsync(sql, new { idsAulas, alteradoPor = "Sistema", alteradoEm = DateTime.Now, alteradoRf = "Sistema" });
        }

        public async Task<Aula> ObterAulaPorComponenteCurricularIdTurmaIdEData(string componenteCurricularId, string turmaId, DateTime data)
        {
            var query = @"select a.* from aula a
                                where a.disciplina_id  = @ccid and turma_id = @turmaid and data_aula::date = @data";

            return (await database.Conexao.QueryAsync<Aula>(query, new { ccid = componenteCurricularId, turmaid = turmaId, data = data.Date })).FirstOrDefault();
        }
    }
}