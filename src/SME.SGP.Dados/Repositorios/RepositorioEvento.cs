using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEvento : RepositorioBase<Evento>, IRepositorioEvento
    {
        public RepositorioEvento(ISgpContext conexao) : base(conexao)
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

        public bool EhEventoLetivoPorTipoDeCalendarioDataDreUe(long tipoCalendarioId, DateTime data, string dreId, string ueId)
        {
            string cabecalho = "select count(id) from evento e where e.excluido = false";
            string whereTipoCalendario = "and e.tipo_calendario_id = @tipoCalendarioId";

            StringBuilder query = new StringBuilder();

            ObterContadorEventosNaoLetivosSME(cabecalho, whereTipoCalendario, query, true);

            if (!string.IsNullOrEmpty(ueId))
            {
                query.AppendLine("UNION");
                ObterContadorEventosNaoLetivosUE(cabecalho, whereTipoCalendario, query, true);
            }

            var retorno = database.Conexao.Query<int?>(query.ToString(),
                new { tipoCalendarioId, dreId, ueId, data = data.Date });

            return retorno != null && retorno.Sum() > 0;
        }
        public bool EhEventoNaoLetivoPorTipoDeCalendarioDataDreUe(long tipoCalendarioId, DateTime data, string dreId, string ueId)
        {
            string cabecalho = "select count(id) from evento e where e.excluido = false";
            string whereTipoCalendario = "and e.tipo_calendario_id = @tipoCalendarioId";

            StringBuilder query = new StringBuilder();

            ObterContadorEventosNaoLetivosSME(cabecalho, whereTipoCalendario, query, false);

            if (!string.IsNullOrEmpty(ueId))
            {
                query.AppendLine("UNION");
                ObterContadorEventosNaoLetivosUE(cabecalho, whereTipoCalendario, query, false);
            }

            var retorno = database.Conexao.Query<int?>(query.ToString(),
                new { tipoCalendarioId, dreId, ueId, data = data.Date });

            return retorno != null && retorno.Sum() > 0;
        }
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
            query.AppendLine("from");
            query.AppendLine("evento e");
            query.AppendLine("inner join evento_tipo et on");
            query.AppendLine("e.tipo_evento_id = et.id");
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


            if (VisualizarEventosSME)
            {
                query.AppendLine("UNION");
                query.AppendLine("select");
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
                query.AppendLine("from");
                query.AppendLine("evento e");
                query.AppendLine("inner join evento_tipo et on");
                query.AppendLine("e.tipo_evento_id = et.id");
                query.AppendLine("where");
                query.AppendLine("e.excluido = false");
                query.AppendLine("and e.status = 1");
                query.AppendLine("and et.ativo = true");
                query.AppendLine("and et.excluido = false");
                query.AppendLine("and e.tipo_calendario_id = @tipoCalendarioId");
                query.AppendLine("and e.dre_id is null and e.ue_id is null");
                query.AppendLine("and (extract(month from e.data_inicio) = @mes or extract(month from e.data_fim) = @mes)");
            }

            return await database.Conexao.QueryAsync<Evento>(query.ToString(), new
            {
                tipoCalendarioId,
                dreCodigo,
                ueCodigo,
                mes
            });

        }

        public async Task<IEnumerable<Evento>> ObterEventosCalendarioProfessorPorMesDia(long tipoCalendarioId, string dreCodigo, string ueCodigo,
            DateTime dataDoEvento, bool VisualizarEventosSME = false, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme = false)

        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select distinct");
            query.AppendLine("e.id,");
            query.AppendLine("e.nome,");
            query.AppendLine("e.descricao,");
            query.AppendLine("e.data_inicio,");
            query.AppendLine("e.data_fim,");
            query.AppendLine("et.id,");
            query.AppendLine("et.descricao,");
            query.AppendLine("et.tipo_data");
            query.AppendLine("from");
            query.AppendLine("evento e");
            query.AppendLine("inner join evento_tipo et on");
            query.AppendLine("e.tipo_evento_id = et.id");
            query.AppendLine("where");
            query.AppendLine("e.excluido = false");
            query.AppendLine("and e.status = 1");
            query.AppendLine("and et.ativo = true");
            query.AppendLine("and et.excluido = false");
            query.AppendLine("and e.tipo_calendario_id = @tipoCalendarioId");

            if (!podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme)
                query.AppendFormat("and et.codigo not in ({0}) ", string.Join(",", new int[] { (int)TipoEvento.LiberacaoExcepcional, (int)TipoEvento.ReposicaoNoRecesso }));

            query.AppendLine("and e.dre_id = @dreCodigo and e.ue_id = @ueCodigo");
            query.AppendLine("and @dataDoEvento between symmetric e.data_inicio ::date and e.data_fim ::date");


            if (VisualizarEventosSME)
            {
                query.AppendLine("UNION");
                query.AppendLine("select distinct");
                query.AppendLine("e.id,");
                query.AppendLine("e.nome,");
                query.AppendLine("e.descricao,");
                query.AppendLine("e.data_inicio,");
                query.AppendLine("e.data_fim,");
                query.AppendLine("et.id,");
                query.AppendLine("et.descricao,");
                query.AppendLine("et.tipo_data");

                query.AppendLine("from");
                query.AppendLine("evento e");
                query.AppendLine("inner join evento_tipo et on");
                query.AppendLine("e.tipo_evento_id = et.id");
                query.AppendLine("where");
                query.AppendLine("e.excluido = false");
                query.AppendLine("and e.status = 1");
                query.AppendLine("and et.ativo = true");
                query.AppendLine("and et.excluido = false");
                query.AppendLine("and e.tipo_calendario_id = @tipoCalendarioId");
                query.AppendLine("and e.dre_id is null and e.ue_id is null");
                query.AppendLine("and @dataDoEvento between symmetric e.data_inicio ::date and e.data_fim ::date");
            }

            return await database.Conexao.QueryAsync<Evento, EventoTipo, Evento>(query.ToString(), (evento, eventoTipo) =>
            {
                evento.AdicionarTipoEvento(eventoTipo);

                return evento;

            }, param: new
            {
                tipoCalendarioId,
                dreCodigo,
                ueCodigo,
                dataDoEvento
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

        private static void ObterContadorEventosNaoLetivosSME(string cabecalho, string whereTipoCalendario, StringBuilder query, bool letivo)
        {
            var queryLetivo = letivo ? "and e.letivo in (1,3)" : "and e.letivo = 2";
            query.AppendLine(cabecalho);
            query.AppendLine(whereTipoCalendario);
            query.AppendLine("and e.dre_id is null and e.ue_id is null");
            query.AppendLine("and e.data_inicio <= @data and e.data_fim >= @data");
            query.AppendLine(queryLetivo);
        }

        private static void ObterContadorEventosNaoLetivosUE(string cabecalho, string whereTipoCalendario, StringBuilder query, bool letivo)
        {
            var queryLetivo = letivo ? "and e.letivo in (1,3)" : "and e.letivo = 2";
            query.AppendLine(cabecalho);
            query.AppendLine(whereTipoCalendario);
            query.AppendLine("and e.ue_id = @ueId");
            query.AppendLine("and e.data_inicio <= @data and e.data_fim >= @data");
            query.AppendLine(queryLetivo);
        }

        #region Listar

        public async Task<PaginacaoResultadoDto<Evento>> Listar(long? tipoCalendarioId, long? tipoEventoId, string nomeEvento, DateTime? dataInicio, DateTime? dataFim,
            Paginacao paginacao, string dreId, string ueId, bool ehTodasDres, bool ehTodasUes, Usuario usuario, Guid usuarioPerfil, bool usuarioTemPerfilSupervisorOuDiretor,
            bool podeVisualizarEventosLocalOcorrenciaDre, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme, bool consideraHistorico)
        {
            try
            {


                if (paginacao == null || (paginacao.QuantidadeRegistros == 0 && paginacao.QuantidadeRegistrosIgnorados == 0))
                    paginacao = new Paginacao(1, 10);

                var retornoPaginado = new PaginacaoResultadoDto<Evento>();

                var queryTotalRegistros = new StringBuilder("select count(0) ");
                ObterParametrosDaFuncaoEventosListarSemPaginacao(tipoCalendarioId, tipoEventoId, nomeEvento, dataInicio, dataFim, dreId, ueId, ehTodasDres, ehTodasUes, usuario, usuarioPerfil, usuarioTemPerfilSupervisorOuDiretor, podeVisualizarEventosLocalOcorrenciaDre, podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme, consideraHistorico, queryTotalRegistros);

                var totalRegistrosDaQuery = await database.Conexao.QueryFirstOrDefaultAsync<int>(queryTotalRegistros.ToString());


                var queryEventos = new StringBuilder(@"select eventoid,
                                 eventoid id,
								 nome,
								 descricaoevento descricao,
								 data_inicio,
								 data_fim,
								 dre_id,
								 letivo,
								 feriado_id,
								 tipo_calendario_id,
								 tipo_evento_id,
								 ue_id,
								 criado_em,
								 criado_por,
							     alterado_em,
							     alterado_por,
							     criado_rf,
								 alterado_rf,
								 status,	
								 tipoeventoid,
                                 tipoeventoid id,
								 ativo,
								 tipo_data,
								 descricaotipoevento descricao,
								 excluido,
								 total_registros ");

                ObterParametrosDaFuncaoEventosListarSemPaginacao(tipoCalendarioId, tipoEventoId, nomeEvento, dataInicio, dataFim, dreId, ueId, ehTodasDres, ehTodasUes, usuario, usuarioPerfil, usuarioTemPerfilSupervisorOuDiretor, podeVisualizarEventosLocalOcorrenciaDre, podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme, consideraHistorico, queryEventos);
                queryEventos.AppendLine("offset @qtde_registros_ignorados rows fetch next @qtde_registros rows only;");

                retornoPaginado.Items = await database.Conexao.QueryAsync<Evento, EventoTipo, Evento>(queryEventos.ToString(), (evento, tipoEvento) =>
                {
                    evento.AdicionarTipoEvento(tipoEvento);
                    return evento;
                },
               param: new { qtde_registros_ignorados = paginacao.QuantidadeRegistrosIgnorados, qtde_registros = paginacao.QuantidadeRegistros },
               splitOn: "EventoId, TipoEventoId");

                retornoPaginado.TotalRegistros = totalRegistrosDaQuery;
                retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);

                return retornoPaginado;
            }

            catch (Exception)
            {
                throw;
            }

        }

        private static void ObterParametrosDaFuncaoEventosListarSemPaginacao(long? tipoCalendarioId, long? tipoEventoId, string nomeEvento, DateTime? dataInicio, DateTime? dataFim, string dreId, string ueId, bool ehTodasDres, bool ehTodasUes, Usuario usuario, Guid usuarioPerfil, bool usuarioTemPerfilSupervisorOuDiretor, bool podeVisualizarEventosLocalOcorrenciaDre, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme, bool consideraHistorico, StringBuilder queryNova)
        {
            queryNova.AppendLine($"from public.f_eventos_listar_sem_paginacao ('{usuario.CodigoRf}', ");
            queryNova.AppendLine($"'{usuarioPerfil}', ");
            queryNova.AppendLine($"{consideraHistorico}, ");
            queryNova.AppendLine($"{tipoCalendarioId}, ");
            queryNova.AppendLine($"{usuarioTemPerfilSupervisorOuDiretor}, ");
            queryNova.AppendLine($"{!podeVisualizarEventosLocalOcorrenciaDre},");
            queryNova.AppendLine($"{ (ehTodasDres ? "null" : string.IsNullOrWhiteSpace(dreId) ? "null" : $"'{dreId}'")}, ");
            queryNova.AppendLine($"{(ehTodasUes ? "null" : string.IsNullOrWhiteSpace(ueId) ? "null" : $"'{ueId}'")},");
            queryNova.AppendLine($"{!podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme}, ");
            queryNova.AppendLine($"{(dataInicio.HasValue ? dataInicio.Value.Date.ToString() : "null")}, ");
            queryNova.AppendLine($"{(dataFim.HasValue ? dataFim.Value.Date.ToString() : "null")}, ");
            queryNova.AppendLine($"{(tipoEventoId.HasValue ? tipoEventoId.ToString() : "null")}, ");
            queryNova.AppendLine($"{(string.IsNullOrWhiteSpace(nomeEvento) ? "null" : $"'{nomeEvento}'")})");
        }

        #endregion Listar

        #region Tipos de Eventos filtrados por Dia

        public async Task<IEnumerable<CalendarioEventosNoDiaRetornoDto>> ObterEventosPorDia(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, int mes, int dia,
            Usuario usuario)
        {
            var query = @"select id,
                                 iniciofimdesc,
                                 nome,
                                 tipoevento
                            from f_eventos_calendario_eventos_do_dia(@login, 
                                                                     @perfil_id, 
                                                                     @historico,
                                                                     @dia,
                                                                     @mes,
                                                                     @tipo_calendario_id,
                                                                     @considera_evento_aprovado_e_pendente_aprovacao, 
                                                                     @dre_id, 
                                                                     @ue_id, 
                                                                     @desconsidera_local_dre,
                                                                     @desconsidera_eventos_sme)";

            // Está sendo utilizado a função com intuíto da melhoria de performance
            return await database.Conexao.QueryAsync<CalendarioEventosNoDiaRetornoDto>(query.ToString(), new
            {
                login = usuario.CodigoRf,
                perfil_id = usuario.PerfilAtual,
                historico = calendarioEventosMesesFiltro.ConsideraHistorico,
                dia,
                mes,
                tipo_calendario_id = calendarioEventosMesesFiltro.IdTipoCalendario,
                considera_evento_aprovado_e_pendente_aprovacao = usuario.TemPerfilSupervisorOuDiretor() || usuario.PodeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme(),
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

            // Está sendo utilizado a função com intuíto da melhoria de performance
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

            if (!string.IsNullOrEmpty(dreId) && !string.IsNullOrEmpty(ueId))
                queryDreUe.AppendLine("and (e.dre_id = @dreId and e.ue_id = @ueId)");
            else if (!string.IsNullOrEmpty(dreId))
                queryDreUe.AppendLine("and (e.dre_id = @dreId and e.ue_id is null)");
            else if (EhEventoSme)
                queryDreUe.AppendLine("and (e.dre_id is null or e.ue_id is null)");
            else if (filtroDreUe)
                queryDreUe.AppendLine("and (e.dre_id is not null or e.ue_id is not null)");

            if (!filtroDreUe)
                queryDreUe.AppendLine($"{(String.IsNullOrEmpty(queryDreUe.ToString()) ? "and" : "or")} (e.dre_id is null and e.ue_id is null)");

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
    }
}