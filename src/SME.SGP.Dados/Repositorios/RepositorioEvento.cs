using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioEvento : RepositorioBase<Evento>, IRepositorioEvento
    {
        public RepositorioEvento(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }

        public List<Evento> EhEventoLetivoPorLiberacaoExcepcional(long tipoCalendarioId, DateTime dataAula, string ueId)
        {
            dataAula = dataAula.Date;

            var query = @"select
                    e.id,
                    e.nome,
                    e.descricao,
                    e.data_inicio,
                    e.data_fim,
                    e.dre_id,
                    e.ue_id,
                    e.letivo,
                    e.feriado_id,
                    e.tipo_calendario_id,
                    e.tipo_evento_id,
                    e.criado_em,
                    e.criado_por,
                    e.alterado_em,
                    e.alterado_por,
                    e.criado_rf,
                    e.alterado_rf,
                    e.status,
                    e.wf_aprovacao_id as WorkflowAprovacaoId,
                    et.id as TipoEventoId,
                    et.codigo,
                    et.ativo,
                    et.tipo_data,
                    et.descricao,
                    et.excluido,
                    tc.id as TipoCalendarioId,
                    tc.Nome,
                    tc.Ano_Letivo,
                    tc.Situacao
                from
                   evento e
                inner join evento_tipo et on
                e.tipo_evento_id = et.id
                inner join tipo_calendario tc
                on e.tipo_calendario_id = tc.id
               WHERE e.excluido = false
                 AND e.tipo_calendario_id = @tipoCalendarioId
                 and e.ue_id = @ueId
                 and e.data_inicio <= @dataAula
                 AND e.data_fim >= @dataAula ";

            return database.Conexao.Query<Evento, EventoTipo, TipoCalendario, Evento>(query.ToString(), (evento, tipoEvento, tipoCalendario) =>
            {
                evento.AdicionarTipoEvento(tipoEvento);

                return evento;
            }, new
            {
                tipoCalendarioId,
                dataAula,
                ueId
            },
                splitOn: "EventoId,TipoEventoId,TipoCalendarioId").ToList();
        }

        private async Task<bool> VerificaEventoLetivoPorTipoCalendarioDataDreUe(long tipoCalendarioId, DateTime data, string dreId, string ueId, bool letivo = true)
        {
            string cabecalho = @"select count(e.id) 
                                   from evento e 
                                  inner join evento_tipo et on et.id = e.tipo_evento_id
                                  where e.excluido = false ";
            string whereTipoCalendario = "and e.tipo_calendario_id = @tipoCalendarioId";

            StringBuilder query = new StringBuilder();

            ObterContadorEventosLetivos(cabecalho, whereTipoCalendario, query, ueId, letivo);

            var retorno = await database.Conexao.QueryAsync<int?>(query.ToString(),
                new { tipoCalendarioId, dreId, ueId, data = data.Date });

            return retorno.NaoEhNulo() && retorno.Sum() > 0;
        }

        public async Task<bool> EhEventoLetivoPorTipoDeCalendarioDataDreUe(long tipoCalendarioId, DateTime data, string dreId, string ueId)
            => await VerificaEventoLetivoPorTipoCalendarioDataDreUe(tipoCalendarioId, data, dreId, ueId);

        public async Task<bool> EhEventoNaoLetivoPorTipoDeCalendarioDataDreUe(long tipoCalendarioId, DateTime data, string dreId, string ueId)
            => await VerificaEventoLetivoPorTipoCalendarioDataDreUe(tipoCalendarioId, data, dreId, ueId, false);

        public async Task<IEnumerable<Evento>> EventosNosDiasETipo(DateTime dataInicio, DateTime dataFim, TipoEvento tipoEventoCodigo, long tipoCalendarioId, string UeId, string DreId, bool utilizarRangeDatas = true)
        {
            var query = new StringBuilder();

            query.AppendLine("select");
            query.AppendLine("e.id,");
            query.AppendLine("e.nome,");
            query.AppendLine("e.descricao,");
            query.AppendLine("e.data_inicio,");
            query.AppendLine("e.data_fim,");
            query.AppendLine("e.dre_id,");
            query.AppendLine("e.ue_id,");
            query.AppendLine("e.letivo,");
            query.AppendLine("e.feriado_id,");
            query.AppendLine("e.tipo_calendario_id,");
            query.AppendLine("e.tipo_evento_id,");
            query.AppendLine("e.criado_em,");
            query.AppendLine("e.criado_por,");
            query.AppendLine("e.alterado_em,");
            query.AppendLine("e.alterado_por,");
            query.AppendLine("e.criado_rf,");
            query.AppendLine("e.alterado_rf,");
            query.AppendLine("e.status,");
            query.AppendLine("e.wf_aprovacao_id as WorkflowAprovacaoId,");
            query.AppendLine("e.tipo_perfil_cadastro as TipoPerfilCadastro,");
            query.AppendLine("et.id as TipoEventoId,");
            query.AppendLine("et.id,");
            query.AppendLine("et.codigo,");
            query.AppendLine("et.ativo,");
            query.AppendLine("et.tipo_data,");
            query.AppendLine("et.descricao,");
            query.AppendLine("et.excluido");
            query.AppendLine("from evento e");
            query.AppendLine("inner join");
            query.AppendLine("evento_tipo et");
            query.AppendLine("on e.tipo_evento_id = et.id");
            query.AppendLine("where");
            query.AppendLine("et.codigo = @tipoEventoCodigo");
            query.AppendLine("and et.ativo = true");
            query.AppendLine("and et.excluido = false");
            query.AppendLine("and e.excluido = false");

            if (!string.IsNullOrEmpty(UeId))
                query.AppendLine("and e.ue_id = @ueId");

            if (!string.IsNullOrEmpty(DreId))
                query.AppendLine("and e.dre_id = @dreId");

            if (utilizarRangeDatas)
            {
                query.AppendLine("and ((e.data_inicio <= TO_DATE(@dataInicio, 'yyyy/mm/dd') and e.data_fim >= TO_DATE(@dataInicio, 'yyyy/mm/dd'))");
                query.AppendLine("or (e.data_inicio <= TO_DATE(@dataFim, 'yyyy/mm/dd') and e.data_fim >= TO_DATE(@dataFim, 'yyyy/mm/dd'))");
                query.AppendLine("or (e.data_inicio >= TO_DATE(@dataInicio, 'yyyy/mm/dd') and e.data_fim <= TO_DATE(@dataFim, 'yyyy/mm/dd'))");
                query.AppendLine(")");
            }
            else
            {
                query.AppendLine("and e.data_inicio = TO_DATE(@dataInicio, 'yyyy/mm/dd') ");
                query.AppendLine("and e.data_fim = TO_DATE(@dataFim, 'yyyy/mm/dd')");
            }
            query.AppendLine("and e.tipo_calendario_id = @tipoCalendarioId");

            var lookup = new Dictionary<long, Evento>();

            await database.Conexao.QueryAsync<Evento, EventoTipo, Evento>(query.ToString(), (evento, eventoTipo) =>
            {
                var eventoRetorno = new Evento();
                if (!lookup.TryGetValue(evento.Id, out eventoRetorno))
                {
                    eventoRetorno = evento;
                    lookup.Add(evento.Id, eventoRetorno);
                }

                eventoRetorno.AdicionarTipoEvento(eventoTipo);
                return eventoRetorno;
            }, new
            {
                dataInicio = dataInicio.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo),
                dataFim = dataFim.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo),
                tipoEventoCodigo = (int)tipoEventoCodigo,
                tipoCalendarioId,
                UeId,
                DreId
            });

            return lookup.Values;
        }

        public bool ExisteEventoPorEventoTipoId(long eventoTipoId)
        {
            var query = "select 1 from evento where tipo_evento_id = @eventoTipoId;";
            return database.Conexao.QueryFirstOrDefault<bool>(query, new { eventoTipoId });
        }

        public bool ExisteEventoPorFeriadoId(long feriadoId)
        {
            var query = "select 1 from evento where feriado_id = @feriadoId;";
            return database.Conexao.QueryFirstOrDefault<bool>(query, new { feriadoId });
        }

        public bool ExisteEventoPorTipoCalendarioId(long tipoCalendarioId)
        {
            var query = "select 1 from evento where tipo_calendario_id = @tipoCalendarioId and excluido = false;";
            return database.Conexao.QueryFirstOrDefault<bool>(query, new { tipoCalendarioId });
        }

        public IEnumerable<Evento> ObterEventosPorRecorrencia(long eventoId, long eventoPaiId, DateTime dataEvento)
        {
            var query = "select * from evento where id <> @eventoId and evento_pai_id = @eventoPaiId and data_inicio ::date >= @dataEvento ";
            return database.Conexao.Query<Evento>(query, new { eventoId, eventoPaiId, dataEvento });
        }

        public IEnumerable<Evento> ObterEventosPorTipoDeCalendarioDreUe(long tipoCalendarioId, string dreId, string ueId, bool EhEventoSme = false, bool filtroDreUe = true, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme = true)
        {
            var query = ObterEventos(dreId, ueId, null, null, EhEventoSme, !EhEventoSme, filtroDreUe, podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme);
            return database.Conexao.Query<Evento>(query.ToString(), new { tipoCalendarioId, dreId, ueId });
        }
        public async Task<IEnumerable<Evento>> ObterEventosCalendarioProfessorPorMes(long tipoCalendarioId, string dreCodigo, string ueCodigo, int mes, bool VisualizarEventosSME = false, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme = false)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select");
            query.AppendLine("  e.id as EventoId,");
            query.AppendLine("  e.id,");
            query.AppendLine("  e.nome,");
            query.AppendLine("  e.descricao,");
            query.AppendLine("  e.data_inicio,");
            query.AppendLine("  e.data_fim,");
            query.AppendLine("  e.dre_id,");
            query.AppendLine("  e.letivo,");
            query.AppendLine("  e.feriado_id,");
            query.AppendLine("  e.tipo_calendario_id,");
            query.AppendLine("  e.tipo_evento_id,");
            query.AppendLine("  e.ue_id,");
            query.AppendLine("  e.criado_em,");
            query.AppendLine("  e.criado_por,");
            query.AppendLine("  e.alterado_em,");
            query.AppendLine("  e.alterado_por,");
            query.AppendLine("  e.criado_rf,");
            query.AppendLine("  e.alterado_rf,");
            query.AppendLine("  et.id as TipoEventoId,");
            query.AppendLine("  et.ativo,");
            query.AppendLine("  et.tipo_data,");
            query.AppendLine("  et.descricao,");
            query.AppendLine("  et.excluido");
            query.AppendLine("from");
            query.AppendLine("  evento e");
            query.AppendLine("      inner join evento_tipo et on");
            query.AppendLine("          e.tipo_evento_id = et.id");
            query.AppendLine("      inner join tipo_calendario tc on");
            query.AppendLine("          e.tipo_calendario_id = tc.id");
            query.AppendLine("where");
            query.AppendLine("e.excluido = false");
            query.AppendLine("and e.status = 1");
            query.AppendLine("and et.ativo = true");
            query.AppendLine("and et.excluido = false");
            query.AppendLine("and e.tipo_calendario_id = @tipoCalendarioId");

            if (!podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme)
                query.AppendFormat(" and et.codigo not in ({0}) ", string.Join(",", new int[] { (int)TipoEvento.LiberacaoExcepcional, (int)TipoEvento.ReposicaoNoRecesso }));

            query.AppendLine("and e.dre_id = @dreCodigo and e.ue_id = @ueCodigo");
            query.AppendLine("and (extract(month from e.data_inicio) = @mes or extract(month from e.data_fim) = @mes)");
            query.AppendLine("and  extract(year from e.data_inicio) = tc.ano_letivo and extract(year from e.data_fim) = tc.ano_letivo");


            if (VisualizarEventosSME)
            {
                query.AppendLine("UNION");
                query.AppendLine("select");
                query.AppendLine("  e.id as EventoId,");
                query.AppendLine("  e.id,");
                query.AppendLine("  e.nome,");
                query.AppendLine("  e.descricao,");
                query.AppendLine("  e.data_inicio,");
                query.AppendLine("  e.data_fim,");
                query.AppendLine("  e.dre_id,");
                query.AppendLine("  e.letivo,");
                query.AppendLine("  e.feriado_id,");
                query.AppendLine("  e.tipo_calendario_id,");
                query.AppendLine("  e.tipo_evento_id,");
                query.AppendLine("  e.ue_id,");
                query.AppendLine("  e.criado_em,");
                query.AppendLine("  e.criado_por,");
                query.AppendLine("  e.alterado_em,");
                query.AppendLine("  e.alterado_por,");
                query.AppendLine("  e.criado_rf,");
                query.AppendLine("  e.alterado_rf,");
                query.AppendLine("  et.id as TipoEventoId,");
                query.AppendLine("  et.ativo,");
                query.AppendLine("  et.tipo_data,");
                query.AppendLine("  et.descricao,");
                query.AppendLine("  et.excluido");
                query.AppendLine("from");
                query.AppendLine("  evento e");
                query.AppendLine("      inner join evento_tipo et on");
                query.AppendLine("          e.tipo_evento_id = et.id");
                query.AppendLine("      inner join tipo_calendario tc on");
                query.AppendLine("          e.tipo_calendario_id = tc.id");
                query.AppendLine("where");
                query.AppendLine("e.excluido = false");
                query.AppendLine("and e.status = 1");
                query.AppendLine("and et.ativo = true");
                query.AppendLine("and et.excluido = false");
                query.AppendLine("and e.tipo_calendario_id = @tipoCalendarioId");
                query.AppendLine("and e.dre_id is null and e.ue_id is null");
                query.AppendLine("and (extract(month from e.data_inicio) = @mes or extract(month from e.data_fim) = @mes)");
                query.AppendLine("and  extract(year from e.data_inicio) = tc.ano_letivo and extract(year from e.data_fim) = tc.ano_letivo");
            }

            return await database.Conexao.QueryAsync<Evento>(query.ToString(), new
            {
                tipoCalendarioId,
                dreCodigo,
                ueCodigo,
                mes
            });

        }

        public async Task<IEnumerable<Evento>> ObterEventosCalendarioProfessorPorMesDia(string dreCodigo, string ueCodigo,
            DateTime dataDoEvento, long tipoCalendarioId, bool VisualizarEventosSME = false, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme = false)

        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select distinct");
            query.AppendLine("e.id,");
            query.AppendLine("e.nome,");
            query.AppendLine("e.descricao,");
            query.AppendLine("e.data_inicio,");
            query.AppendLine("e.data_fim,");
            query.AppendLine("e.dre_id,");
            query.AppendLine("e.ue_id,");
            query.AppendLine("e.tipo_calendario_id,");
            query.AppendLine("et.id,");
            query.AppendLine("et.descricao,");
            query.AppendLine("et.tipo_data,");
            query.AppendLine("dre.id,");
            query.AppendLine("dre.abreviacao,");
            query.AppendLine("dre.nome,");
            query.AppendLine("ue.id,");
            query.AppendLine("ue.nome");
            query.AppendLine("from");
            query.AppendLine("evento e");
            query.AppendLine("      inner join evento_tipo et on");
            query.AppendLine("          e.tipo_evento_id = et.id");
            query.AppendLine("      inner join dre dre on");
            query.AppendLine("e.dre_id = dre.dre_id");
            query.AppendLine("      inner join ue ue on");
            query.AppendLine("          e.ue_id = ue.ue_id");
            query.AppendLine("where");
            query.AppendLine("e.excluido = false");
            query.AppendLine("and e.status = 1");
            query.AppendLine("and et.ativo = true");
            query.AppendLine("and et.excluido = false");

            if (!podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme)
                query.AppendFormat("and et.codigo not in ({0}) ", string.Join(",", new int[] { (int)TipoEvento.LiberacaoExcepcional, (int)TipoEvento.ReposicaoNoRecesso }));

            query.AppendLine("and e.dre_id = @dreCodigo and e.ue_id = @ueCodigo");
            query.AppendLine("and @dataDoEvento between symmetric e.data_inicio ::date and e.data_fim ::date");
            query.AppendLine("and e.tipo_calendario_id = @tipoCalendarioId");


            if (VisualizarEventosSME)
            {
                query.AppendLine("UNION");

                query.AppendLine("select distinct");
                query.AppendLine("e.id,");
                query.AppendLine("e.nome,");
                query.AppendLine("e.descricao,");
                query.AppendLine("e.data_inicio,");
                query.AppendLine("e.data_fim,");
                query.AppendLine("e.dre_id,");
                query.AppendLine("e.ue_id,");
                query.AppendLine("e.tipo_calendario_id,");
                query.AppendLine("et.id,");
                query.AppendLine("et.descricao,");
                query.AppendLine("et.tipo_data,");
                query.AppendLine("0,");
                query.AppendLine("'',");
                query.AppendLine("'',");
                query.AppendLine("0,");
                query.AppendLine("''");
                query.AppendLine("from");
                query.AppendLine("evento e");
                query.AppendLine("inner join evento_tipo et on");
                query.AppendLine("    e.tipo_evento_id = et.id");
                query.AppendLine("where");
                query.AppendLine("e.excluido = false");
                query.AppendLine("and e.status = 1");
                query.AppendLine("and et.ativo = true");
                query.AppendLine("and et.excluido = false");
                query.AppendLine("and e.dre_id is null and e.ue_id is null");
                query.AppendLine("and @dataDoEvento between symmetric e.data_inicio ::date and e.data_fim ::date");
                query.AppendLine("and e.tipo_calendario_id = @tipoCalendarioId");
            }

            return await database.Conexao.QueryAsync<Evento, EventoTipo, Dre, Ue, Evento>(query.ToString(), (evento, eventoTipo, dre, ue) =>
            {
                evento.AdicionarTipoEvento(eventoTipo);
                evento.AdicionarDre(dre);
                evento.AdicionarUe(ue);

                return evento;

            }, param: new
            {
                dreCodigo,
                ueCodigo,
                dataDoEvento,
                tipoCalendarioId
            });

        }

        public async Task<IEnumerable<Evento>> ObterEventosPorTipoDeCalendarioDreUeDia(long tipoCalendarioId, string dreId, string ueId, DateTime data, bool EhEventoSme, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme = true)
        {
            var query = ObterEventos(dreId, ueId, null, data, EhEventoSme, !EhEventoSme, true, podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme);
            return await database.Conexao.QueryAsync<Evento>(query.ToString(), new { tipoCalendarioId, dreId, ueId, data });
        }

        public async Task<IEnumerable<Evento>> ObterEventosPorTipoDeCalendarioDreUeMes(long tipoCalendarioId, string dreId, string ueId, int mes, bool EhEventoSme, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme = true)
        {
            var query = ObterEventos(dreId, ueId, mes, null, EhEventoSme, !EhEventoSme, true, podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme);
            return await database.Conexao.QueryAsync<Evento>(query.ToString(), new { tipoCalendarioId, dreId, ueId, mes });
        }

        public async Task<IEnumerable<Evento>> ObterEventosPorTipoETipoCalendario(long tipoEventoCodigo, long tipoCalendarioId)
        {
            var query = new StringBuilder();
            query.AppendLine("select* from evento e");
            query.AppendLine("where");
            query.AppendLine("e.excluido = false");
            query.AppendLine("and e.tipo_calendario_id = @tipoCalendarioId");
            query.AppendLine("and tipo_evento_id = @tipoEventoCodigo");

            return await database.Conexao.QueryAsync<Evento>(query.ToString(), new
            {
                tipoEventoCodigo,
                tipoCalendarioId
            });
        }

        public Evento ObterPorWorkflowId(long workflowId)
        {
            var query = @"select
                    e.id,
                    e.nome,
                    e.descricao,
                    e.data_inicio,
                    e.data_fim,
                    e.dre_id,
                    e.ue_id,
                    e.letivo,
                    e.feriado_id,
                    e.tipo_calendario_id,
                    e.tipo_evento_id,
                    e.criado_em,
                    e.criado_por,
                    e.alterado_em,
                    e.alterado_por,
                    e.criado_rf,
                    e.alterado_rf,
                    e.status,
                    e.wf_aprovacao_id as WorkflowAprovacaoId,
                    et.id as TipoEventoId,
                    et.codigo,
                    et.ativo,
                    et.tipo_data,
                    et.descricao,
                    et.excluido,
                    tc.id as TipoCalendarioId,
                    tc.Nome,
                    tc.Ano_Letivo,
                    tc.Situacao
                from
                    evento e
                inner join evento_tipo et on
                    e.tipo_evento_id = et.id
                inner join tipo_calendario tc
                on e.tipo_calendario_id = tc.id
                where et.ativo = true
                and et.excluido = false
                and e.excluido = false
                and e.wf_aprovacao_id = @workflowId ";

            return database.Conexao.Query<Evento, EventoTipo, TipoCalendario, Evento>(query.ToString(), (evento, tipoEvento, tipoCalendario) =>
            {
                evento.AdicionarTipoEvento(tipoEvento);
                evento.TipoCalendario = tipoCalendario;
                return evento;
            }, new
            {
                workflowId
            },
            splitOn: "EventoId,TipoEventoId,TipoCalendarioId").FirstOrDefault();
        }

        public async Task<bool> TemEventoNosDiasETipo(DateTime dataInicio, DateTime dataFim, TipoEvento tipoEventoCodigo, long tipoCalendarioId, string UeId, string DreId, bool escopoRetroativo = false)
        {
            var query = new StringBuilder();

            query.AppendLine("select count(e.id) from evento e");
            query.AppendLine("inner join");
            query.AppendLine("evento_tipo et");
            query.AppendLine("on e.tipo_evento_id = et.id");
            query.AppendLine("where");
            query.AppendLine("et.codigo = @tipoEventoCodigo");
            query.AppendLine("and et.ativo = true");
            query.AppendLine("and et.excluido = false");
            query.AppendLine("and e.excluido = false");
            query.AppendLine("and e.status = 1");

            if (escopoRetroativo)
                ObtenhaEscopoRetroativo(UeId, DreId, query);
            else
                ObtenhaEscopoNormal(UeId, DreId, query);

            query.AppendLine("and ((e.data_inicio <= TO_DATE(@dataInicio, 'yyyy/mm/dd') and e.data_fim >= TO_DATE(@dataInicio, 'yyyy/mm/dd'))");
            query.AppendLine("or (e.data_inicio <= TO_DATE(@dataFim, 'yyyy/mm/dd') and e.data_fim >= TO_DATE(@dataFim, 'yyyy/mm/dd'))");
            query.AppendLine("or (e.data_inicio >= TO_DATE(@dataInicio, 'yyyy/mm/dd') and e.data_fim <= TO_DATE(@dataFim, 'yyyy/mm/dd'))");
            query.AppendLine(")");
            query.AppendLine("and e.tipo_calendario_id = @tipoCalendarioId");

            var parametros = new
            {
                dataInicio = dataInicio.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo),
                dataFim = dataFim.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo),
                tipoEventoCodigo = (int)tipoEventoCodigo,
                tipoCalendarioId,
                UeId,
                DreId
            };

            return (await database.Conexao.QueryFirstOrDefaultAsync<int>(query.ToString(), parametros)) > 0;
        }

        private static void MontaFiltroTipoCalendario(StringBuilder query)
        {
            query.AppendLine("where");
            query.AppendLine("e.excluido = false");
            query.AppendLine("and e.status = 1");
            query.AppendLine("and et.ativo = true");
            query.AppendLine("and et.excluido = false");
            query.AppendLine("and e.tipo_calendario_id = @tipoCalendarioId");
        }

        private static void MontaQueryCabecalho(StringBuilder query)
        {
            query.AppendLine("select distinct");
            query.AppendLine("e.id as EventoId,");
            query.AppendLine("e.id,");
            query.AppendLine("e.nome,");
            query.AppendLine("e.descricao,");
            query.AppendLine("e.data_inicio,");
            query.AppendLine("e.data_fim,");
            query.AppendLine("e.dre_id,");
            query.AppendLine("e.letivo,");
            query.AppendLine("e.feriado_id,");
            query.AppendLine("e.tipo_calendario_id,");
            query.AppendLine("e.tipo_evento_id,");
            query.AppendLine("e.ue_id,");
            query.AppendLine("e.criado_em,");
            query.AppendLine("e.criado_por,");
            query.AppendLine("e.alterado_em,");
            query.AppendLine("e.alterado_por,");
            query.AppendLine("e.criado_rf,");
            query.AppendLine("e.alterado_rf,");
            query.AppendLine("et.id as TipoEventoId,");
            query.AppendLine("et.ativo,");
            query.AppendLine("et.tipo_data,");
            query.AppendLine("et.descricao,");
            query.AppendLine("et.excluido");
        }

        private static void MontaQueryFrom(StringBuilder query)
        {
            query.AppendLine("from");
            query.AppendLine("evento e");
            query.AppendLine("inner join evento_tipo et on");
            query.AppendLine("e.tipo_evento_id = et.id");
        }

        private static void ObtenhaEscopoNormal(string UeId, string DreId, StringBuilder query)
        {
            if (string.IsNullOrEmpty(DreId))
            {
                if (string.IsNullOrEmpty(UeId))
                    query.AppendLine("and (e.dre_id is null and e.ue_id is null)");
                else
                    query.AppendLine("and e.ue_id = @ueId");
            }
            else
            {
                if (string.IsNullOrEmpty(UeId))
                    query.AppendLine("and (e.dre_id = @dreId and e.ue_id is null)");
                else
                    query.AppendLine("and (e.dre_id = @dreId and e.ue_id = @ueId)");
            }
        }

        private static void ObtenhaEscopoRetroativo(string UeId, string DreId, StringBuilder query)
        {
            query.AppendLine("and ((e.dre_id is null and e.ue_id is null)");

            if (!string.IsNullOrEmpty(DreId))
                query.AppendLine("or (e.dre_id = @dreId and e.ue_id is null)");

            if (!string.IsNullOrEmpty(UeId))
                query.AppendLine("or (e.dre_id = @dreId and e.ue_id = @ueId)");

            query.AppendLine(")");
        }

        private static void ObterContadorEventosLetivos(string cabecalho, string whereTipoCalendario, StringBuilder query, string ueId, bool letivo = true)
        {
            var queryLetivo = letivo ? "and (e.letivo in (1,3) or et.codigo = 13)" : "and (e.letivo = 2 and et.codigo <> 13)";
            query.AppendLine(cabecalho);
            query.AppendLine(whereTipoCalendario);
            query.AppendLine("and ((e.dre_id is null and e.ue_id is null)");
            query.AppendLine($"{(!string.IsNullOrEmpty(ueId) ? " or (e.ue_id = @ueId)" : "")})");
            query.AppendLine("and e.data_inicio <= @data and e.data_fim >= @data");
            query.AppendLine(queryLetivo);
        }

        #region Listar

        public async Task<PaginacaoResultadoDto<Evento>> Listar(long? tipoCalendarioId, long? tipoEventoId,
            string nomeEvento, DateTime? dataInicio, DateTime? dataFim,
            Paginacao paginacao, string dreId, string ueId, bool ehTodasDres, bool ehTodasUes, Usuario usuario,
            Guid usuarioPerfil, bool usuarioTemPerfilSupervisorOuDiretor,
            bool podeVisualizarEventosLocalOcorrenciaDre, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme,
            bool consideraHistorico, bool eventosTodaRede)
        {
            if (paginacao.EhNulo() ||
                (paginacao.QuantidadeRegistros == 0 && paginacao.QuantidadeRegistrosIgnorados == 0))
                paginacao = new Paginacao(1, 10);

            var retornoPaginado = new PaginacaoResultadoDto<Evento>();

            var queryTotalRegistros = new StringBuilder("select count(0) ");
            ObterParametrosDaFuncaoEventosListarSemPaginacao(tipoCalendarioId, tipoEventoId, nomeEvento, dataInicio,
                dataFim, dreId, ueId, ehTodasDres, ehTodasUes, usuario, usuarioPerfil,
                usuarioTemPerfilSupervisorOuDiretor, podeVisualizarEventosLocalOcorrenciaDre,
                podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme, consideraHistorico, queryTotalRegistros,
                eventosTodaRede);

            var totalRegistrosDaQuery =
                await database.Conexao.QueryFirstOrDefaultAsync<int>(queryTotalRegistros.ToString());


            var queryEventos = new StringBuilder(@"select f_eventos_listar_sem_paginacao.eventoid,
                                 f_eventos_listar_sem_paginacao.eventoid id,
                                 f_eventos_listar_sem_paginacao.nome,
                                 f_eventos_listar_sem_paginacao.descricaoevento descricao,
                                 f_eventos_listar_sem_paginacao.data_inicio,
                                 f_eventos_listar_sem_paginacao.data_fim,
                                 f_eventos_listar_sem_paginacao.dre_id,
                                 f_eventos_listar_sem_paginacao.letivo,
                                 f_eventos_listar_sem_paginacao.feriado_id,
                                 f_eventos_listar_sem_paginacao.tipo_calendario_id,
                                 f_eventos_listar_sem_paginacao.tipo_evento_id,
                                 f_eventos_listar_sem_paginacao.ue_id,
                                 f_eventos_listar_sem_paginacao.criado_em,
                                 f_eventos_listar_sem_paginacao.criado_por,
                                 f_eventos_listar_sem_paginacao.alterado_em,
                                 f_eventos_listar_sem_paginacao.alterado_por,
                                 f_eventos_listar_sem_paginacao.criado_rf,
                                 f_eventos_listar_sem_paginacao.alterado_rf,
                                 f_eventos_listar_sem_paginacao.status,    
                                 f_eventos_listar_sem_paginacao.tipoeventoid,
                                 f_eventos_listar_sem_paginacao.tipoeventoid id,
                                 f_eventos_listar_sem_paginacao.ativo,
                                 f_eventos_listar_sem_paginacao.tipo_data,
                                 f_eventos_listar_sem_paginacao.descricaotipoevento descricao,
                                 f_eventos_listar_sem_paginacao.excluido,
                                 f_eventos_listar_sem_paginacao.total_registros,
                                 ue.ue_id ue,
                                 ue.nome,
                                 dre.dre_id as dre,
                                 dre.abreviacao ");

            ObterParametrosDaFuncaoEventosListarSemPaginacao(tipoCalendarioId, tipoEventoId, nomeEvento, dataInicio,
                dataFim, dreId, ueId, ehTodasDres, ehTodasUes, usuario, usuarioPerfil,
                usuarioTemPerfilSupervisorOuDiretor, podeVisualizarEventosLocalOcorrenciaDre,
                podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme, consideraHistorico, queryEventos,
                eventosTodaRede);
            queryEventos.AppendLine("left join dre on dre.dre_id = f_eventos_listar_sem_paginacao.dre_id ");
            queryEventos.AppendLine("left join ue on ue.ue_id = f_eventos_listar_sem_paginacao.ue_id ");
            queryEventos.AppendLine("order by data_inicio ");
            queryEventos.AppendLine("offset @qtde_registros_ignorados rows fetch next @qtde_registros rows only;");

            retornoPaginado.Items = await database.Conexao.QueryAsync<Evento, EventoTipo, Ue, Dre, Evento>(
                queryEventos.ToString(),
                (evento, tipoEvento, ue, dre) =>
                {
                    evento.AdicionarDre(dre);
                    evento.AdicionarUe(ue);
                    evento.AdicionarTipoEvento(tipoEvento);
                    return evento;
                },
                param: new
                {
                    qtde_registros_ignorados = paginacao.QuantidadeRegistrosIgnorados,
                    qtde_registros = paginacao.QuantidadeRegistros
                },
                splitOn: "EventoId, TipoEventoId, ue, dre");

            retornoPaginado.TotalRegistros = totalRegistrosDaQuery;
            retornoPaginado.TotalPaginas =
                (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);

            return retornoPaginado;
        }

        private static void ObterParametrosDaFuncaoEventosListarSemPaginacao(long? tipoCalendarioId, long? tipoEventoId, string nomeEvento, DateTime? dataInicio, DateTime? dataFim, string dreId, string ueId, bool ehTodasDres, bool ehTodasUes, Usuario usuario, Guid usuarioPerfil, bool usuarioTemPerfilSupervisorOuDiretor, bool podeVisualizarEventosLocalOcorrenciaDre, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme, bool consideraHistorico, StringBuilder queryNova, bool eventosTodaRede)
        {
            var parametro_dre = string.IsNullOrWhiteSpace(dreId) ? "null" : $"'{dreId}'";
            var parametro_ue = string.IsNullOrWhiteSpace(ueId) ? "null" : $"'{ueId}'";

            queryNova.AppendLine($"from public.f_eventos_listar_sem_paginacao ('{usuario.CodigoRf}', ");
            queryNova.AppendLine($"'{usuarioPerfil}', ");
            queryNova.AppendLine($"{consideraHistorico}, ");
            queryNova.AppendLine($"{tipoCalendarioId}, ");
            queryNova.AppendLine($"{usuarioTemPerfilSupervisorOuDiretor}, ");
            queryNova.AppendLine($"{!podeVisualizarEventosLocalOcorrenciaDre},");
            queryNova.AppendLine($"{(ehTodasDres ? "null" : parametro_dre)}, ");
            queryNova.AppendLine($"{(ehTodasUes ? "null" : parametro_ue)},");
            queryNova.AppendLine($"{podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme}, ");
            queryNova.AppendLine($"{(dataInicio.HasValue ? $"TO_DATE('{dataInicio.Value.ToString("MM-dd-yyyy")}', 'MM-dd-yyyy')" : "null")}, ");
            queryNova.AppendLine($"{(dataFim.HasValue ? $"TO_DATE('{dataFim.Value.ToString("MM-dd-yyyy")}', 'MM-dd-yyyy')" : "null")}, ");
            queryNova.AppendLine($"{(tipoEventoId.HasValue ? tipoEventoId.ToString() : "null")}, ");
            queryNova.AppendLine($"{(string.IsNullOrWhiteSpace(nomeEvento) ? "null" : $"'{nomeEvento}'")},");
            queryNova.AppendLine($"{usuario.EhPerfilSME()},");
            queryNova.AppendLine($"{usuario.EhPerfilDRE()},");
            queryNova.AppendLine($"{usuario.EhPerfilUE()},");
            queryNova.AppendLine($"{eventosTodaRede})");
        }

        #endregion Listar

        #region Tipos de Eventos filtrados por Dia

        public async Task<IEnumerable<CalendarioEventosNoDiaRetornoDto>> ObterEventosPorDia(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, int mes, int dia, int anoLetivo,
            Usuario usuario)
        {
            var query = @"select id,
                                 nome,
                                 tipoevento,
                                 dreNome,
                                 ueNome,
                                 ueTipo,
                                 descricao,
                                 data_inicio,
                                 data_fim
                            from f_eventos_calendario_eventos_do_dia(@login, 
                                                                     @perfil_id, 
                                                                     @historico,
                                                                     @dataReferencia,
                                                                     @mes,
                                                                     @tipo_calendario_id,
                                                                     @considera_evento_aprovado_e_pendente_aprovacao, 
                                                                     @dre_id, 
                                                                     @ue_id, 
                                                                     @desconsidera_local_dre,
                                                                     @desconsidera_eventos_sme)";

            var dataConsulta = new DateTime(anoLetivo, mes, dia);
            return await database.Conexao.QueryAsync<CalendarioEventosNoDiaRetornoDto>(query.ToString(), new
            {
                login = usuario.CodigoRf,
                perfil_id = usuario.PerfilAtual,
                historico = calendarioEventosMesesFiltro.ConsideraHistorico,
                dataReferencia = dataConsulta.Date,
                mes,
                tipo_calendario_id = calendarioEventosMesesFiltro.IdTipoCalendario,
                considera_evento_aprovado_e_pendente_aprovacao = false,
                dre_id = calendarioEventosMesesFiltro.DreId,
                ue_id = calendarioEventosMesesFiltro.UeId,
                desconsidera_local_dre = !usuario.PodeVisualizarEventosOcorrenciaDre(),
                desconsidera_eventos_sme = !calendarioEventosMesesFiltro.EhEventoSme
            });
        }

        #endregion Tipos de Eventos filtrados por Dia

        #region Quantidade Eventos Por Meses

        public async Task<IEnumerable<CalendarioEventosMesesDto>> ObterQuantidadeDeEventosPorMeses(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, Usuario usuario, Guid usuarioPerfil,
            bool podeVisualizarEventosLocalOcorrenciaDre, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme)
        {
            var query = new StringBuilder();

            query.AppendLine("select distinct");
            query.AppendLine("a.mes,");
            query.AppendLine("count(*) eventos");
            query.AppendLine("from");
            query.AppendLine("(");
            query.AppendLine("select distinct");
            query.AppendLine("a.*,");
            query.AppendLine("rank() over(partition by a.id,");
            query.AppendLine("a.mes");
            query.AppendLine("order by");
            query.AppendLine("campo) rank_id");
            query.AppendLine("from");
            query.AppendLine("(");
            query.AppendLine("select distinct");
            query.AppendLine("id,");
            query.AppendLine("extract(month from data_inicio) mes,");
            query.AppendLine("1 campo");
            query.AppendLine("from(");

            MontaQueryEventosPorMesesCabecalho(query, true);

            MontaQueryQuantidadeEventosPorMesesVisualizacaoGeral(calendarioEventosMesesFiltro, query, podeVisualizarEventosLocalOcorrenciaDre, usuario.TemPerfilSupervisorOuDiretor(),
                podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme);

            if (calendarioEventosMesesFiltro.EhEventoSme)
            {
                query.AppendLine("union distinct");
                MontaQueryEventosPorMesesCabecalho(query, true);
                MontaQueryQuantidadeEventosPorMesesVisualizacaoGeralSmeDre(calendarioEventosMesesFiltro, query, podeVisualizarEventosLocalOcorrenciaDre, usuario.TemPerfilSupervisorOuDiretor(),
                  podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme);
            }

            query.AppendLine("union distinct");

            MontaQueryEventosPorMesesCabecalho(query, true);
            MontaQueryEventosPorMesesVisualizacaoCriador(calendarioEventosMesesFiltro, query);

            query.AppendLine("-- fim");
            query.AppendLine(") a");
            query.AppendLine("union distinct");
            query.AppendLine("select distinct");
            query.AppendLine("id,");
            query.AppendLine("extract(month from data_fim) mes,");
            query.AppendLine("1 campo");
            query.AppendLine("from(");
            query.AppendLine("--inicio");
            MontaQueryEventosPorMesesCabecalho(query, false);

            MontaQueryQuantidadeEventosPorMesesVisualizacaoGeral(calendarioEventosMesesFiltro, query, podeVisualizarEventosLocalOcorrenciaDre, usuario.TemPerfilSupervisorOuDiretor(),
                podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme);

            if (calendarioEventosMesesFiltro.EhEventoSme)
            {
                query.AppendLine("union distinct");
                MontaQueryEventosPorMesesCabecalho(query, false);
                MontaQueryQuantidadeEventosPorMesesVisualizacaoGeralSmeDre(calendarioEventosMesesFiltro, query, podeVisualizarEventosLocalOcorrenciaDre, usuario.TemPerfilSupervisorOuDiretor(),
                  podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme);
            }

            query.AppendLine("union distinct");

            MontaQueryEventosPorMesesCabecalho(query, false);
            MontaQueryEventosPorMesesVisualizacaoCriador(calendarioEventosMesesFiltro, query);

            query.AppendLine("--fim");
            query.AppendLine(") a) a) a");
            query.AppendLine("where");
            query.AppendLine("a.rank_id = 1");
            query.AppendLine("group by");
            query.AppendLine("a.mes");

            return await database.Conexao.QueryAsync<CalendarioEventosMesesDto>(query.ToString(), new
            {
                IdTipoCalendario = calendarioEventosMesesFiltro.IdTipoCalendario,
                DreId = calendarioEventosMesesFiltro.DreId,
                UeId = calendarioEventosMesesFiltro.UeId,
                usuarioId = usuario.Id,
                usuarioPerfil,
                usuarioRf = usuario.CodigoRf
            });
        }

        private static void MontaQueryEventosPorMesesCabecalho(StringBuilder query, bool ehDataInicial)
        {
            if (ehDataInicial)
                query.AppendLine("select distinct e.id, e.data_inicio");
            else query.AppendLine("select distinct e.id, e.data_fim");
        }

        private static void MontaQueryEventosPorMesesVisualizacaoCriador(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, StringBuilder query)
        {
            query.AppendLine("from");
            query.AppendLine("evento e");
            query.AppendLine("inner join evento_tipo et on");
            query.AppendLine("e.tipo_evento_id = et.id");
            query.AppendLine("where");
            query.AppendLine("et.ativo = true");
            query.AppendLine("and et.excluido = false");
            query.AppendLine("and e.excluido = false");
            query.AppendLine("and e.criado_rf = @usuarioRf");

            if (!string.IsNullOrEmpty(calendarioEventosMesesFiltro.DreId))
                query.AppendLine("and e.dre_id = @DreId");

            if (calendarioEventosMesesFiltro.IdTipoCalendario > 0)
                query.AppendLine("and e.tipo_calendario_id = @IdTipoCalendario");

            if (!string.IsNullOrEmpty(calendarioEventosMesesFiltro.UeId))
                query.AppendLine("and e.ue_id = @UeId");
        }

        private static void MontaQueryQuantidadeEventosPorMesesVisualizacaoGeral(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, StringBuilder query,
            bool podeVisualizarEventosLocalOcorrenciaDre, bool usuarioTemPerfilSupervisorOuDiretor, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme)
        {
            query.AppendLine("from");
            query.AppendLine("evento e");
            query.AppendLine("inner join evento_tipo et on");
            query.AppendLine("e.tipo_evento_id = et.id");
            query.AppendLine("inner join v_abrangencia a on");
            query.AppendLine("a.ue_codigo = e.ue_id");
            query.AppendLine("and a.dre_codigo = e.dre_id");
            query.AppendLine("and a.usuario_id = @usuarioId");
            query.AppendLine("and a.usuario_perfil = @usuarioPerfil");
            query.AppendLine("where");
            query.AppendLine("et.ativo = true");
            query.AppendLine("and et.excluido = false");
            query.AppendLine("and e.excluido = false");

            if (!string.IsNullOrEmpty(calendarioEventosMesesFiltro.DreId))
                query.AppendLine("and e.dre_id = @DreId");

            if (calendarioEventosMesesFiltro.IdTipoCalendario > 0)
                query.AppendLine("and e.tipo_calendario_id = @IdTipoCalendario");

            if (!string.IsNullOrEmpty(calendarioEventosMesesFiltro.UeId))
                query.AppendLine("and e.ue_id = @UeId");

            if (!podeVisualizarEventosLocalOcorrenciaDre)
                query.AppendLine("and et.local_ocorrencia != 2");

            if (usuarioTemPerfilSupervisorOuDiretor || podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme)
                query.AppendLine("and e.status IN (1,2)");
            else query.AppendLine("and e.status = 1");
        }

        private static void MontaQueryQuantidadeEventosPorMesesVisualizacaoGeralSmeDre(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, StringBuilder query,
           bool podeVisualizarEventosLocalOcorrenciaDre, bool usuarioTemPerfilSupervisorOuDiretor, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme)
        {
            query.AppendLine("from");
            query.AppendLine("evento e");
            query.AppendLine("inner join evento_tipo et on");
            query.AppendLine("e.tipo_evento_id = et.id");
            query.AppendLine("where");
            query.AppendLine("et.ativo = true");
            query.AppendLine("and et.excluido = false");
            query.AppendLine("and e.excluido = false");
            query.AppendLine("and e.ue_id is null or e.dre_id is null");

            if (!string.IsNullOrEmpty(calendarioEventosMesesFiltro.DreId))
                query.AppendLine("and e.dre_id = @DreId");

            if (calendarioEventosMesesFiltro.IdTipoCalendario > 0)
                query.AppendLine("and e.tipo_calendario_id = @IdTipoCalendario");

            if (!string.IsNullOrEmpty(calendarioEventosMesesFiltro.UeId))
                query.AppendLine("and e.ue_id = @UeId");

            if (!podeVisualizarEventosLocalOcorrenciaDre)
                query.AppendLine("and et.local_ocorrencia != 2");

            if (usuarioTemPerfilSupervisorOuDiretor || podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme)
                query.AppendLine("and e.status IN (1,2)");
            else query.AppendLine("and e.status = 1");
        }

        #endregion Quantidade Eventos Por Meses

        #region Quantidade De Eventos Por Dia filtrado por mes

        public async Task<IEnumerable<EventosPorDiaRetornoQueryDto>> ObterQuantidadeDeEventosPorDia(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, int mes,
              Usuario usuario, Guid usuarioPerfil, bool usuarioTemPerfilSupervisorOuDiretor, bool podeVisualizarEventosLocalOcorrenciaDre, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme)
        {
            var query = @"select dia, 
                                 tipoEvento 
                          from f_eventos_calendario_dias_com_eventos_no_mes(@login, 
                                                                            @perfil_id, 
                                                                            @historico, 
                                                                            @mes, 
                                                                            @tipo_calendario_id, 
                                                                            @considera_evento_aprovado_e_pendente_aprovacao, 
                                                                            @dre_id, 
                                                                            @ue_id, 
                                                                            @desconsidera_local_dre,
                                                                            @desconsidera_eventos_sme)";

            return await database.Conexao.QueryAsync<EventosPorDiaRetornoQueryDto>(query.ToString(), new
            {
                login = usuario.CodigoRf,
                perfil_id = usuarioPerfil,
                historico = calendarioEventosMesesFiltro.ConsideraHistorico,
                mes,
                tipo_calendario_id = calendarioEventosMesesFiltro.IdTipoCalendario,
                considera_evento_aprovado_e_pendente_aprovacao = usuarioTemPerfilSupervisorOuDiretor || podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme,
                dre_id = calendarioEventosMesesFiltro.DreId,
                ue_id = calendarioEventosMesesFiltro.UeId,
                desconsidera_local_dre = !podeVisualizarEventosLocalOcorrenciaDre,
                desconsidera_eventos_sme = !calendarioEventosMesesFiltro.EhEventoSme
            });
        }

        #endregion Quantidade De Eventos Por Dia filtrado por mes

        private string ObterEventos(string dreId, string ueId, int? mes = null, DateTime? data = null, bool EhEventoSme = false, bool naoTrazerSme = false, bool filtroDreUe = true, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme = false)
        {
            StringBuilder query = new StringBuilder();
            MontaQueryCabecalho(query);
            MontaQueryFrom(query);
            MontaFiltroTipoCalendario(query);

            if (!podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme)
                query.AppendFormat(" and et.codigo not in ({0}) ", string.Join(",", new int[] { (int)TipoEvento.LiberacaoExcepcional, (int)TipoEvento.ReposicaoNoRecesso }));

            StringBuilder queryDreUe = new StringBuilder();
            AdicionarCondicionalDreUeObterEventos(queryDreUe, dreId, ueId, EhEventoSme, filtroDreUe);

            if (!String.IsNullOrEmpty(queryDreUe.ToString()))
            {
                queryDreUe.Insert(queryDreUe.ToString().IndexOf("and") + 4, "(").Insert(queryDreUe.ToString().Length - 1, ")");
                query.AppendLine(queryDreUe.ToString());
            }

            if (mes.HasValue)
            {
                query.AppendLine("and (extract(month from e.data_inicio) = @mes");
                query.AppendLine("  or extract(month from e.data_fim) = @mes)");
            }
            if (data.HasValue)
            {
                query.AppendLine("and e.data_inicio <= @data");
                query.AppendLine("and e.data_fim >= @data");
            }

            if ((!string.IsNullOrEmpty(dreId) || !string.IsNullOrEmpty(ueId)) && !naoTrazerSme)
            {
                query.AppendLine("UNION");
                MontaQueryCabecalho(query);
                MontaQueryFrom(query);
                MontaFiltroTipoCalendario(query);
                query.AppendLine("and e.dre_id is null and e.ue_id is null");

                if (!podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme)
                    query.AppendFormat(" and et.codigo not in ({0}) ", string.Join(",", new int[] { (int)TipoEvento.LiberacaoExcepcional, (int)TipoEvento.ReposicaoNoRecesso }));

                if (mes.HasValue)
                {
                    query.AppendLine("and (extract(month from e.data_inicio) = @mes");
                    query.AppendLine("  or extract(month from e.data_fim) = @mes)");
                }
                if (data.HasValue)
                {
                    query.AppendLine("and e.data_inicio <= @data");
                    query.AppendLine("and e.data_fim >= @data");
                }
            }

            return query.ToString();
        }

        private static void AdicionarCondicionalDreUeObterEventos(StringBuilder query, string dreId, string ueId, bool ehEventoSme, bool filtroDreUe)
        {
            if (!string.IsNullOrEmpty(dreId) && !string.IsNullOrEmpty(ueId))
                query.AppendLine("and (e.dre_id = @dreId and e.ue_id = @ueId)");
            else if (!string.IsNullOrEmpty(dreId))
                query.AppendLine("and (e.dre_id = @dreId and e.ue_id is null)");
            else if (ehEventoSme)
                query.AppendLine("and (e.dre_id is null or e.ue_id is null)");
            else if (filtroDreUe)
                query.AppendLine("and (e.dre_id is not null or e.ue_id is not null)");

            if (!filtroDreUe)
                query.AppendLine($"{(String.IsNullOrEmpty(query.ToString()) ? "and" : "or")} (e.dre_id is null and e.ue_id is null)");

        }


        public async Task<bool> DataPossuiEventoLiberacaoExcepcionalAsync(long tipoCalendarioId, DateTime dataAula, string ueId)
        {
            var query = @"SELECT
                            1
                        FROM
                            evento e
                        INNER JOIN evento_tipo et ON
                            e.tipo_evento_id = et.id
                        INNER JOIN tipo_calendario tc ON
                            e.tipo_calendario_id = tc.id
                        WHERE
                            e.excluido = false
                            AND e.tipo_calendario_id = @tipoCalendarioId
                            AND e.ue_id = @ueId
                            AND e.data_inicio <= @dataAula
                            AND (e.data_fim  IS NULL OR e.data_fim >= @dataAula)
                            AND et.codigo <> @codigoLiberacaoExcepcional
                            AND e.letivo = @eventoLetivo";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new
            {
                tipoCalendarioId,
                dataAula = dataAula.Date,
                ueId,
                codigoLiberacaoExcepcional = TipoEvento.LiberacaoExcepcional,
                eventoLetivo = EventoLetivo.Sim
            });
        }

        public async Task<IEnumerable<Evento>> ObterEventosPorTipoDeCalendarioAsync(long tipoCalendarioId, string ueCodigo = "", params EventoLetivo[] tiposLetivosConsiderados)
        {
            bool possuiUeCodigo = !string.IsNullOrEmpty(ueCodigo);

            var query = new StringBuilder(@"select
                                                data_inicio,
                                                data_fim,
                                                letivo,
                                                e.ue_id,
                                                e.dre_id,
                                                e.nome,
                                                e.feriado_id,
                                                e.tipo_evento_id TipoEventoId
                                            from
                                                evento e
                                                    inner join tipo_calendario tc
                                                        on e.tipo_calendario_id = tc.id");
            if (possuiUeCodigo)
                query.AppendLine(@" left join ue u 
                                        on u.ue_id = e.ue_id
                                    left join dre d 
                                        on d.id = u.dre_id");

            query.AppendLine(@" where
                                    e.tipo_calendario_id = @tipoCalendarioId
                                and extract(year from e.data_inicio) = tc.ano_letivo     
                                and e.letivo = any(@tiposLetivos)                         
                                and not e.excluido");

            if (possuiUeCodigo)
                query.AppendLine($@" and (e.ue_id = @ueCodigo 
                                            or (e.ue_id is null and e.dre_id is null)
                                            or (e.ue_id is null and e.dre_id = d.dre_id))");

            return await database.Conexao.QueryAsync<Evento>(query.ToString(),
                new
                {
                    tipoCalendarioId,
                    ueCodigo,
                    tiposLetivos = tiposLetivosConsiderados.Select(tlc => (int)tlc).ToArray()
                });
        }

        public async Task<IEnumerable<EventoCalendarioRetornoDto>> ObterEventosPorTipoDeCalendarioDreUeModalidadeAsync(long tipoCalendario,
                                                                                                   int anoLetivo,
                                                                                                   string codigoDre,
                                                                                                   string codigoUe,
                                                                                                   int? modalidade)
        {
            var whereDre = string.IsNullOrEmpty(codigoDre) ? "" : "and (dre_id is null or dre_id = @codigoDre)";
            var whereUe = string.IsNullOrEmpty(codigoUe) ? "" : "and (ue_id is null or ue_id = @codigoUe)";
            var whereModalidade = !modalidade.HasValue ? "" : "and tc.modalidade = @modalidade";
            var query = $@"
                select 
                    e.Id As Id,
                    e.Nome as Nome,
                    te.descricao as TipoEvento
                from
                    evento e
                inner join 
                    tipo_calendario tc on tc.id = e.tipo_calendario_id         
                    inner join evento_tipo te on te.id = e.tipo_evento_id 
                where
                    e.tipo_calendario_id = @tipoCalendario
                    {whereDre}
                    {whereUe}
                    {whereModalidade}
                and not e.excluido
                and e.status = 1
                and tc.situacao
                and tc.ano_letivo = @anoLetivo
            ";
            return await database.Conexao.QueryAsync<EventoCalendarioRetornoDto>(
                query,
                new
                {
                    tipoCalendario,
                    anoLetivo,
                    codigoDre,
                    codigoUe,
                    modalidade = modalidade.HasValue ? modalidade.Value : 0
                });
        }

        public async Task<IEnumerable<Evento>> ObterEventosPorTipoCalendarioIdEPeriodoInicioEFim(long tipoCalendarioId, DateTime periodoInicio, DateTime periodoFim, long? turmaId = null)
        {
            var filtroTurma = !turmaId.HasValue ? "" :
                @"inner join (
                        select ue.ue_id, dre.dre_id from turma t
                        inner join ue on ue.id = t.ue_id
                        inner join dre on dre.id = ue.dre_id
                        where t.id = @turmaId
                    ) x on e.dre_id is null 
                        or (e.dre_id = x.dre_id and (e.ue_id is null or e.ue_id = x.ue_id))";

            var query = $@"select
                            e.id,
                            data_inicio as DataInicio,
                            data_fim as DataFim,
                            e.letivo,
                            e.nome,
                            e.descricao,
                            e.ue_id as UeId,
                            e.dre_id as DreId,
                            e.tipo_evento_id as TipoEvento,
                            et.id,
                            et.descricao
                        from
                            evento e
                        inner join evento_tipo et on et.id = e.tipo_evento_id
                        {filtroTurma}
                        where
                        e.tipo_calendario_id = @tipoCalendarioId
                        and not e.excluido and data_inicio between @periodoInicio and @periodoFim;";


            return await database.Conexao.QueryAsync<Evento, EventoTipo, Evento>(query.ToString(),
                (evento, eventoTipo) =>
                {
                    evento.TipoEvento = eventoTipo;
                    return evento;
                },
                new { tipoCalendarioId, periodoInicio, periodoFim, turmaId });

        }



        public async Task<IEnumerable<Evento>> ObterEventosPorTipoECalendarioUe(long tipoCalendarioId, string ueCodigo, TipoEvento tipoEvento)
        {
            var query = @"select * 
                        from evento 
                       where not excluido
                         and tipo_calendario_id = @tipoCalendarioId
                         and ue_id = @ueCodigo
                         and tipo_evento_id = @tipoEvento";

            return await database.Conexao.QueryAsync<Evento>(query, new { tipoCalendarioId, ueCodigo, tipoEvento });
        }

        public async Task<IEnumerable<Evento>> ObterEventosPorTipoEData(TipoEvento tipoEvento, DateTime data)
        {
            var query = @"select e.*, et.*, ue.*, dre.*
                        from evento e 
                       inner join evento_tipo et on et.id = e.tipo_evento_id
                        left join ue on ue.ue_id = e.ue_id
                        left join dre on dre.id = ue.dre_id
                       where not e.excluido
                         and e.tipo_evento_id = @tipoEvento
                         and e.data_inicio = @data";

            return await database.Conexao.QueryAsync<Evento, EventoTipo, Ue, Dre, Evento>(query,
                (evento, eventoTipo, ue, dre) =>
                {
                    evento.Ue = ue;
                    evento.Dre = dre;
                    evento.TipoEvento = eventoTipo;

                    return evento;
                }, new { tipoEvento = (int)tipoEvento, data });
        }

        public async Task<IEnumerable<EventoDataDto>> ListarEventosItinerancia(long tipoCalendarioId, long itineranciaId, string codigoUE, string login, Guid perfil, bool historico = false)
        {
            var query = @"select distinct e.id,
                            e.data_inicio as Data,
                            e.nome,
                            case
                                when e.dre_id is not null and e.ue_id is null then 'DRE'
                                  when e.dre_id is not null and e.ue_id is not null then 'UE'
                                else 'SME'
                            end tipoEvento,
                            au.Nome as UeNome,
                            au.TipoEscola
                    from evento e
                        inner join evento_tipo et
                            on e.tipo_evento_id = et.id 
                        inner join tipo_calendario tc
                            on e.tipo_calendario_id = tc.id
                        inner join f_abrangencia_ues(@login, @perfil, @historico) au
                            on e.ue_id = au.codigo
                            and ((tc.modalidade = 1 and au.modalidade_codigo in (5, 6)) 
                              or (tc.modalidade = 2 and au.modalidade_codigo = 3)
                              or (tc.modalidade = 3 and au.modalidade_codigo = 1))
                        left join itinerancia i on i.evento_id = e.id
                    where et.ativo 
                        and not et.excluido
                        and not e.excluido
                        and e.ue_id = @codigoUE
                        and (i.id is null or i.id = @itineranciaId)
                        and extract(year from e.data_inicio) = tc.ano_letivo
                        and et.codigo = 28
                        and e.tipo_calendario_id = @tipoCalendarioId";
            return await database.Conexao.QueryAsync<EventoDataDto>(query, new { tipoCalendarioId, itineranciaId, codigoUE, login, perfil, historico });
        }

        public async Task<long> ObterTipoCalendarioIdPorEvento(long eventoId)
        {
            var query = @"select tipo_calendario_id from evento where id = @eventoId";

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query, new { eventoId });
        }

        public async Task<Evento> ObterEventoAtivoPorId(long eventoId)
        {
            var query = @"select * 
                        from evento 
                       where not excluido
                       and id = @eventoId";

            return await database.Conexao.QueryFirstOrDefaultAsync<Evento>(query, new { eventoId });
        }

        public async Task<IEnumerable<EventoCalendarioRetornoDto>> ObterEventosPorTipoDeCalendarioDreUeEModalidades(long tipoCalendario,
                                                                                                                              int anoLetivo,
                                                                                                                              string dreCodigo,
                                                                                                                              string ueCodigo,
                                                                                                                              IEnumerable<int> modalidades)
        {
            var query = new StringBuilder(@"select e.Id As Id,
                                                   e.Nome as Nome,
                                                   te.descricao as TipoEvento,
                                                   e.data_inicio as DataInicio
                                              from evento e
                                             inner join tipo_calendario tc on tc.id = e.tipo_calendario_id         
                                             inner join evento_tipo te on te.id = e.tipo_evento_id 
                                             where e.tipo_calendario_id = @tipoCalendario
                                               and not e.excluido
                                               and e.status = 1
                                               and tc.situacao
                                               and tc.ano_letivo = @anoLetivo ");

            if (!string.IsNullOrEmpty(dreCodigo) && dreCodigo != "-99")
                query.AppendLine("and e.dre_id = @dreCodigo ");

            if (!string.IsNullOrEmpty(ueCodigo) && ueCodigo != "-99")
                query.AppendLine("and e.ue_id = @ueCodigo ");

            if (modalidades.NaoEhNulo() && !modalidades.Any(c => c == -99))
                query.AppendLine("and tc.modalidade = any(@modalidades) ");

            query.AppendLine(@"union select e.Id As Id,
                                                   e.Nome as Nome,
                                                   te.descricao as TipoEvento,
                                                   e.data_inicio as DataInicio
                                              from evento e

                                             inner
                                              join tipo_calendario tc on tc.id = e.tipo_calendario_id

                                        inner
                                              join evento_tipo te on te.id = e.tipo_evento_id
                                             where e.tipo_calendario_id = @tipoCalendario
                                               and not e.excluido
                                               and e.status = 1
                                               and tc.situacao
                                               and tc.ano_letivo = @anoLetivo and e.dre_id is null and e.ue_id is null");


            var parametros = new
            {
                tipoCalendario,
                anoLetivo,
                dreCodigo,
                ueCodigo,
                modalidades
            };

            return await database.Conexao.QueryAsync<EventoCalendarioRetornoDto>(query.ToString(), parametros);

        }

        public async Task<bool> DataPossuiEventoDeLiberacaoEoutroEventoLetivo(long tipoCalendarioId, DateTime data, string ueId)
        {
            var query = @"SELECT 1
                         FROM evento e
                         INNER JOIN evento_tipo et on et.id = e.tipo_evento_id
                         LEFT JOIN (SELECT e.id 
                                   FROM evento e 
                                   INNER JOIN evento_tipo et ON et.id = e.tipo_evento_id
                                   WHERE (et.codigo = @codigoLiberacaoExcepcional)) liberacao ON liberacao.id = e.id
                         LEFT JOIN (SELECT e.id 
                                    FROM evento e 
                                    INNER JOIN evento_tipo et ON et.id = e.tipo_evento_id
                                    WHERE (et.codigo <> @codigoLiberacaoExcepcional)) outros ON outros.id = e.id
                         WHERE e.excluido = false
                            AND e.letivo = @eventoLetivo
                            AND e.tipo_calendario_id = @tipoCalendarioId
                            AND ((e.dre_id is null and e.ue_id is null) OR (e.ue_id = @ueId))
                            AND e.data_inicio <= @dataAula
                            AND (e.data_fim  IS NULL OR e.data_fim >= @dataAula)
                            HAVING Count(liberacao.id) >= 1 OR Count(outros.id) >= 1";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new
            {
                tipoCalendarioId,
                dataAula = data.Date,
                ueId,
                codigoLiberacaoExcepcional = TipoEvento.LiberacaoExcepcional,
                eventoLetivo = EventoLetivo.Sim
            });
        }
    }
}