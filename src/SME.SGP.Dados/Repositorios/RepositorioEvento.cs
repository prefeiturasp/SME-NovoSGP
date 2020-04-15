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

            ObterContadorEventosNaoLetivosSME(cabecalho, whereTipoCalendario, query);

            if (!string.IsNullOrEmpty(ueId))
            {
                query.AppendLine("UNION");
                ObterContadorEventosNaoLetivosUE(cabecalho, whereTipoCalendario, query);
            }

            var retorno = database.Conexao.Query<int?>(query.ToString(),
                new { tipoCalendarioId, dreId, ueId, data = data.Date });

            return retorno == null || retorno.Sum() == 0;
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
            return database.Conexao.QueryFirstOrDefault<bool>(query, new { eventoTipoId }); ;
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

        private static void ObterContadorEventosNaoLetivosSME(string cabecalho, string whereTipoCalendario, StringBuilder query)
        {
            query.AppendLine(cabecalho);
            query.AppendLine(whereTipoCalendario);
            query.AppendLine("and e.dre_id is null and e.ue_id is null");
            query.AppendLine("and e.data_inicio <= @data and e.data_fim >= @data");
            query.AppendLine("and e.letivo = 2");
        }

        private static void ObterContadorEventosNaoLetivosUE(string cabecalho, string whereTipoCalendario, StringBuilder query)
        {
            query.AppendLine(cabecalho);
            query.AppendLine(whereTipoCalendario);
            query.AppendLine("and e.ue_id = @ueId");
            query.AppendLine("and e.data_inicio <= @data and e.data_fim >= @data");
            query.AppendLine("and e.letivo = 2");
        }

        #region Listar

        public async Task<PaginacaoResultadoDto<Evento>> Listar(long? tipoCalendarioId, long? tipoEventoId, string nomeEvento, DateTime? dataInicio, DateTime? dataFim,
            Paginacao paginacao, string dreId, string ueId, bool ehTodasDres, bool ehTodasUes, Usuario usuario, Guid usuarioPerfil, bool usuarioTemPerfilSupervisorOuDiretor,
            bool podeVisualizarEventosLocalOcorrenciaDre, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme)
        {
            StringBuilder query = new StringBuilder();

            if (paginacao == null)
                paginacao = new Paginacao(1, 10);

            MontaQueryCabecalho(query);
            MontaQueryListarFromWhereParaVisualizacaoGeral(query, tipoCalendarioId, tipoEventoId, dataInicio, dataFim, nomeEvento, dreId, ueId, podeVisualizarEventosLocalOcorrenciaDre);
            query.AppendLine("union distinct");
            MontaQueryCabecalho(query);
            MontaQueryListarFromWhereParaCriador(query, tipoCalendarioId, tipoEventoId, dataInicio, dataFim, nomeEvento, dreId, ueId);

            if (usuarioTemPerfilSupervisorOuDiretor)
            {
                query.AppendLine("union distinct");
                MontaQueryCabecalho(query);
                MontaQueryListarFromWhereParaSupervisorDiretorVisualizarEmAprovacao(query, tipoCalendarioId, tipoEventoId, dataInicio, dataFim, nomeEvento, dreId, ueId, ehTodasDres, ehTodasUes, podeVisualizarEventosLocalOcorrenciaDre);
            }

            if (podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme)
            {
                query.AppendLine("union distinct");
                MontaQueryCabecalho(query);
                MontaQueryListarFromWhereParaVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme(query, tipoCalendarioId, tipoEventoId, dataInicio, dataFim, nomeEvento, dreId, ueId, ehTodasDres, ehTodasUes);
            }

            if (paginacao.QuantidadeRegistros != 0)
                query.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", paginacao.QuantidadeRegistrosIgnorados, paginacao.QuantidadeRegistros);

            if (!string.IsNullOrEmpty(nomeEvento))
            {
                nomeEvento = $"%{nomeEvento}%";
            }

            var retornoPaginado = new PaginacaoResultadoDto<Evento>();

            retornoPaginado.Items = await database.Conexao.QueryAsync<Evento, EventoTipo, Evento>(query.ToString(), (evento, tipoEvento) =>
            {
                evento.AdicionarTipoEvento(tipoEvento);
                return evento;
            }, new
            {
                tipoCalendarioId,
                tipoEventoId,
                nomeEvento,
                dataInicio,
                dataFim,
                dreId,
                ueId,
                usuarioId = usuario.Id,
                usuarioPerfil,
                usuarioRf = usuario.CodigoRf
            },
            splitOn: "EventoId,TipoEventoId");

            var queryCountCabecalho = "select count(distinct e.id)";
            var queryCount = new StringBuilder(queryCountCabecalho);
            MontaQueryListarFromWhereParaVisualizacaoGeral(queryCount, tipoCalendarioId, tipoEventoId, dataInicio, dataFim, nomeEvento, dreId, ueId, podeVisualizarEventosLocalOcorrenciaDre);

            queryCount.AppendLine("union distinct");
            queryCount.AppendLine(queryCountCabecalho);
            MontaQueryListarFromWhereParaCriador(queryCount, tipoCalendarioId, tipoEventoId, dataInicio, dataFim, nomeEvento, dreId, ueId);

            if (usuarioTemPerfilSupervisorOuDiretor)
            {
                queryCount.AppendLine("union distinct");
                queryCount.AppendLine(queryCountCabecalho);
                MontaQueryListarFromWhereParaSupervisorDiretorVisualizarEmAprovacao(queryCount, tipoCalendarioId, tipoEventoId, dataInicio, dataFim, nomeEvento, dreId, ueId, ehTodasDres, ehTodasUes, podeVisualizarEventosLocalOcorrenciaDre);
            }
            if (podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme)
            {
                queryCount.AppendLine("union distinct");
                queryCount.AppendLine(queryCountCabecalho);
                MontaQueryListarFromWhereParaVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme(queryCount, tipoCalendarioId, tipoEventoId, dataInicio, dataFim, nomeEvento, dreId, ueId, ehTodasDres, ehTodasUes);
            }
            retornoPaginado.TotalRegistros = (await database.Conexao.QueryAsync<int>(queryCount.ToString(), new
            {
                tipoCalendarioId,
                tipoEventoId,
                nomeEvento,
                dataInicio,
                dataFim,
                dreId,
                ueId,
                usuarioId = usuario.Id,
                usuarioPerfil,
                usuarioRf = usuario.CodigoRf
            })).Sum();

            retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);

            return retornoPaginado;
        }

        private void MontaQueryListarFromWhereParaCriador(StringBuilder query, long? tipoCalendarioId, long? tipoEventoId, DateTime? dataInicio, DateTime? dataFim, string nomeEvento,
            string dreId, string ueId)
        {
            query.AppendLine("from evento e");
            query.AppendLine("inner join evento_tipo et on e.tipo_evento_id = et.id");
            query.AppendLine(" left join v_abrangencia a on a.dre_codigo = e.dre_id");
            query.AppendLine("and (a.ue_codigo = e.ue_id or (e.ue_id is null and a.ue_codigo is null)) ");
            query.AppendLine("  and a.usuario_id = @usuarioId");
            query.AppendLine("  and a.usuario_perfil = @usuarioPerfil");
            query.AppendLine("where et.ativo");
            query.AppendLine("and not et.excluido");
            query.AppendLine("and e.excluido = false");
            query.AppendLine("and e.status = 2");
            query.AppendLine("and e.criado_rf = @usuarioRf");

            query.AppendLine("and(a.usuario_id is not null ");
            query.AppendLine("  or(e.dre_id is null ");
            query.AppendLine("    and e.ue_id is null)) ");

            if (!string.IsNullOrEmpty(dreId) && dreId != "0")
                query.AppendLine($"and e.dre_id = @dreId");

            if (!string.IsNullOrEmpty(ueId) && ueId != "0")
                query.AppendLine($"and e.ue_id  = @ueId");

            if (tipoCalendarioId.HasValue)
                query.AppendLine("and e.tipo_calendario_id = @tipoCalendarioId");

            if (tipoEventoId.HasValue)
                query.AppendLine("and e.tipo_evento_id = @tipoEventoId");

            if (dataInicio.HasValue && dataFim.HasValue)
            {
                query.AppendLine("and e.data_inicio >= @dataInicio");
                query.AppendLine("and (e.data_fim is null OR e.data_fim <= @dataFim)");
            }

            if (!string.IsNullOrWhiteSpace(nomeEvento))
                query.AppendLine("and lower(f_unaccent(e.nome)) LIKE @nomeEvento");
        }

        private void MontaQueryListarFromWhereParaSupervisorDiretorVisualizarEmAprovacao(StringBuilder query, long? tipoCalendarioId, long? tipoEventoId, DateTime? dataInicio, DateTime? dataFim,
               string nomeEvento, string dreId, string ueId, bool ehTodasDres, bool ehTodasUes, bool podeVisualizarEventosLocalOcorrenciaDre)
        {
            query.AppendLine("from");
            query.AppendLine("evento e");
            query.AppendLine("inner");
            query.AppendLine("join evento_tipo et on");
            query.AppendLine("e.tipo_evento_id = et.id");
            query.AppendLine("inner");
            query.AppendLine("join v_abrangencia a on");
            query.AppendLine("a.ue_codigo = e.ue_id");
            query.AppendLine("and a.dre_codigo = e.dre_id");
            query.AppendLine("and a.usuario_id = @usuarioId");
            query.AppendLine("and a.usuario_perfil = @usuarioPerfil");
            query.AppendLine("where");
            query.AppendLine("et.ativo = true");
            query.AppendLine("and et.excluido = false");
            query.AppendLine("and e.excluido = false");
            query.AppendLine("and e.status = 2");

            if (string.IsNullOrEmpty(dreId))
                query.AppendLine($"and e.dre_id is {(ehTodasDres ? "not" : "")} null");

            if (string.IsNullOrEmpty(ueId))
                query.AppendLine($"and e.ue_id is {(ehTodasUes ? "not" : "")} null");

            if (!string.IsNullOrEmpty(dreId))
                query.AppendLine($"and e.dre_id = @dreId");

            if (!string.IsNullOrEmpty(ueId))
                query.AppendLine($"and e.ue_id =  @ueId");

            if (tipoCalendarioId.HasValue)
                query.AppendLine("and e.tipo_calendario_id = @tipoCalendarioId");

            if (tipoEventoId.HasValue)
                query.AppendLine("and e.tipo_evento_id = @tipoEventoId");

            if (dataInicio.HasValue && dataFim.HasValue)
            {
                query.AppendLine("and e.data_inicio >= @dataInicio");
                query.AppendLine("and (e.data_fim is null OR e.data_fim <= @dataFim)");
            }

            if (!string.IsNullOrWhiteSpace(nomeEvento))
                query.AppendLine("and lower(f_unaccent(e.nome)) LIKE @nomeEvento");

            if (!podeVisualizarEventosLocalOcorrenciaDre)
                query.AppendLine("and et.local_ocorrencia != 2");
        }

        private void MontaQueryListarFromWhereParaVisualizacaoGeral(StringBuilder query, long? tipoCalendarioId, long? tipoEventoId, DateTime? dataInicio, DateTime? dataFim, string nomeEvento,
            string dreId, string ueId, bool podeVisualizarEventosLocalOcorrenciaDre)
        {
            query.AppendLine("from");
            query.AppendLine("evento e");
            query.AppendLine("inner join evento_tipo et on e.tipo_evento_id = et.id");
            query.AppendLine(" left join v_abrangencia a on a.dre_codigo = e.dre_id");
            query.AppendLine("and (a.ue_codigo = e.ue_id or (e.ue_id is null and a.ue_codigo is null)) ");
            query.AppendLine("and a.usuario_id = @usuarioId");
            query.AppendLine("and a.usuario_perfil = @usuarioPerfil");
            query.AppendLine("where et.ativo");
            query.AppendLine("and not et.excluido");
            query.AppendLine("and e.status = 1");
            query.AppendLine("and not e.excluido");
            query.AppendLine("and ( a.usuario_id is not null");
            query.AppendLine("  or (e.dre_id is null");
            query.AppendLine("  and e.ue_id is null) )");
            query.AppendFormat(" and et.codigo not in ({0}) ", string.Join(",", new int[] { (int)TipoEvento.LiberacaoExcepcional, (int)TipoEvento.ReposicaoNoRecesso }));

            if (!string.IsNullOrEmpty(dreId) && dreId != "0")
                query.AppendLine("  and e.dre_id = @dreId");

            if (!string.IsNullOrEmpty(ueId) && ueId != "0")
                query.AppendLine("  and e.ue_id = @ueId");

            if (tipoCalendarioId.HasValue)
                query.AppendLine("and e.tipo_calendario_id = @tipoCalendarioId");

            if (tipoEventoId.HasValue)
                query.AppendLine("and e.tipo_evento_id = @tipoEventoId");

            if (dataInicio.HasValue && dataFim.HasValue)
            {
                query.AppendLine("and e.data_inicio >= @dataInicio");
                query.AppendLine("and (e.data_fim is null OR e.data_fim <= @dataFim)");
            }

            if (!string.IsNullOrWhiteSpace(nomeEvento))
                query.AppendLine("and lower(f_unaccent(e.nome)) LIKE @nomeEvento");

            if (!podeVisualizarEventosLocalOcorrenciaDre)
                query.AppendLine("and et.local_ocorrencia != 2");
        }

        private void MontaQueryListarFromWhereParaVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme(StringBuilder query, long? tipoCalendarioId, long? tipoEventoId, DateTime? dataInicio, DateTime? dataFim,
                    string nomeEvento, string dreId, string ueId, bool ehTodasDres, bool ehTodasUes)
        {
            query.AppendLine("from");
            query.AppendLine("evento e");
            query.AppendLine("inner");
            query.AppendLine("join evento_tipo et on");
            query.AppendLine("e.tipo_evento_id = et.id");
            query.AppendLine("inner");
            query.AppendLine("join v_abrangencia a on");
            query.AppendLine("a.ue_codigo = e.ue_id");
            query.AppendLine("and a.dre_codigo = e.dre_id");
            query.AppendLine("and a.usuario_id = @usuarioId");
            query.AppendLine("and a.usuario_perfil = @usuarioPerfil");
            query.AppendLine("where");
            query.AppendLine("et.ativo = true");
            query.AppendLine("and et.excluido = false");
            query.AppendLine("and e.excluido = false");
            query.AppendLine("and e.status in (1, 2)");
            query.AppendFormat(" and et.codigo in ({0}) ", string.Join(",", new int[] { (int)TipoEvento.LiberacaoExcepcional, (int)TipoEvento.ReposicaoNoRecesso }));

            //if (string.IsNullOrEmpty(dreId))
            //    query.AppendLine($"and e.dre_id is {(ehTodasDres ? "not" : "")} null");

            //if (string.IsNullOrEmpty(ueId))
            //    query.AppendLine($"and e.ue_id is {(ehTodasUes ? "not" : "")} null");

            if (!string.IsNullOrEmpty(dreId))
                query.AppendLine($"and e.dre_id = @dreId");

            if (!string.IsNullOrEmpty(ueId))
                query.AppendLine($"and e.ue_id =  @ueId");

            if (tipoCalendarioId.HasValue)
                query.AppendLine("and e.tipo_calendario_id = @tipoCalendarioId");

            if (tipoEventoId.HasValue)
                query.AppendLine("and e.tipo_evento_id = @tipoEventoId");

            if (dataInicio.HasValue && dataFim.HasValue)
            {
                query.AppendLine("and e.data_inicio >= @dataInicio");
                query.AppendLine("and (e.data_fim is null OR e.data_fim <= @dataFim)");
            }

            if (!string.IsNullOrWhiteSpace(nomeEvento))
                query.AppendLine("and lower(f_unaccent(e.nome)) LIKE @nomeEvento");
        }

        #endregion Listar

        #region Tipos de Eventos filtrados por Dia

        public async Task<IEnumerable<CalendarioEventosNoDiaRetornoDto>> ObterEventosPorDia(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, int mes, int dia,
            Usuario usuario)
        {
            var query = new StringBuilder();

            MontaQueryEventosPorDiaCabecalho(query);
            if (usuario.EhPerfilSME())
            {
                if (string.IsNullOrEmpty(calendarioEventosMesesFiltro.DreId))
                {
                    //Obter somente eventos SME
                    MontaQueryEventosPorDiaFromWhereVisualizacaoSME(query);
                }
                else
                {
                    MontaQueryEventosPorDiaFromWhereVisualizacaoSMEDreUe(query, calendarioEventosMesesFiltro.DreId, calendarioEventosMesesFiltro.UeId, calendarioEventosMesesFiltro.EhEventoSme);
                }
            }
            else if (usuario.EhPerfilDRE())
            {
                MontaQueryEventosPorDiaFromWhereVisualizacaoDre(query, calendarioEventosMesesFiltro.UeId, calendarioEventosMesesFiltro.EhEventoSme, calendarioEventosMesesFiltro.DreId);
            }
            else if (usuario.EhPerfilUE())
            {
                if (usuario.TemPerfilGestaoUes())
                {
                    MontaQueryEventosPorDiaFromWhereVisualizacaoGestaoUe(query, calendarioEventosMesesFiltro.EhEventoSme);
                }
                else
                {
                    MontaQueryEventosPorDiaFromWhereVisualizacaoUe(query, calendarioEventosMesesFiltro.DreId, calendarioEventosMesesFiltro.UeId, calendarioEventosMesesFiltro.EhEventoSme);
                }
            }

            return await database.Conexao.QueryAsync<CalendarioEventosNoDiaRetornoDto>(query.ToString(), new
            {
                IdTipoCalendario = calendarioEventosMesesFiltro.IdTipoCalendario,
                DreId = calendarioEventosMesesFiltro.DreId,
                UeId = calendarioEventosMesesFiltro.UeId,
                mes,
                dia,
                usuarioId = usuario.Id,
                usuarioPerfil = usuario.PerfilAtual,
                usuarioRf = usuario.CodigoRf
            });
        }

        private static void MontaQueryEventosPorDiaCabecalho(StringBuilder query)
        {
            query.AppendLine("select  e.id, e.nome,");
            query.AppendLine("case");
            query.AppendLine("when e.dre_id is not null and e.ue_id is null then 'DRE'");
            query.AppendLine("when e.ue_id is not null and e.dre_id is not null then 'UE'");
            query.AppendLine("when e.ue_id is null and e.dre_id is null then 'SME'");
            query.AppendLine("end as TipoEvento,");
            query.AppendLine("case");
            query.AppendLine("when(e.data_fim = e.data_inicio) then ''");
            query.AppendLine("when((extract(month from e.data_inicio) = @mes) and(extract(day from e.data_inicio) = @dia)) then '(início)'");
            query.AppendLine("when((extract(month from e.data_fim) = @mes) and(extract(day from e.data_fim) = @dia)) then '(fim)'");
            query.AppendLine("end as InicioFimDesc");
        }

        private void MontaQueryEventosPorDiaFromWhereVisualizacaoDre(StringBuilder query, string ueId, bool ehEventoSme, string dreId)
        {
            query.AppendLine("from");
            query.AppendLine("evento e");
            query.AppendLine("inner");
            query.AppendLine("join dre d");
            query.AppendLine("on e.dre_id = d.dre_id");
            query.AppendLine("inner");
            query.AppendLine("join v_abrangencia_sintetica a");
            query.AppendLine("on d.id = a.dre_id");
            query.AppendLine("join evento_tipo et on");
            query.AppendLine("e.tipo_evento_id = et.id");
            query.AppendLine("where");
            query.AppendLine("et.ativo = true");
            query.AppendLine("and et.excluido = false");
            query.AppendLine("and e.excluido = false");
            query.AppendLine("and e.tipo_calendario_id = @IdTipoCalendario");

            if (string.IsNullOrEmpty(ueId))
            {
                query.AppendLine("and e.ue_id is null");
                query.AppendLine("and e.status = 1");
            }
            else
            {
                query.AppendLine("and e.ue_id = @ueId");
                query.AppendLine("and e.status IN (1,2)");
            }

            query.AppendLine("and e.dre_id = @dreId");

            query.AppendLine("and((extract(month from e.data_fim) = @mes) or(extract(month from e.data_inicio) = @mes))");
            query.AppendLine("and((extract(day from e.data_fim) = @dia) or(extract(day from e.data_inicio) = @dia))");

            if (ehEventoSme)
            {
                query.AppendLine("union");
                MontaQueryEventosPorDiaCabecalho(query);
                MontaQueryEventosPorDiaFromWhereVisualizacaoSME(query);
            }
        }

        private void MontaQueryEventosPorDiaFromWhereVisualizacaoGestaoUe(StringBuilder query, bool ehEventoSme)
        {
            query.AppendLine("from");
            query.AppendLine("evento e");
            query.AppendLine("inner");
            query.AppendLine("join evento_tipo et on");
            query.AppendLine("e.tipo_evento_id = et.id");
            query.AppendLine("where");
            query.AppendLine("et.ativo = true");
            query.AppendLine("and et.excluido = false");
            query.AppendLine("and e.excluido = false");
            query.AppendLine("and e.status = 1");
            query.AppendLine("and ((e.dre_id = @dreId and e.ue_id is null) or (e.dre_id = @dreId and e.ue_id = @ueId))");
            query.AppendLine("and((extract(month from e.data_fim) = @mes) or(extract(month from e.data_inicio) = @mes))");
            query.AppendLine("and((extract(day from e.data_fim) = @dia) or(extract(day from e.data_inicio) = @dia))");
            query.AppendLine("and e.tipo_calendario_id = @IdTipoCalendario");

            if (ehEventoSme)
            {
                query.AppendLine("union");
                MontaQueryEventosPorDiaCabecalho(query);
                MontaQueryEventosPorDiaFromWhereVisualizacaoSME(query);
            }
        }

        private void MontaQueryEventosPorDiaFromWhereVisualizacaoSME(StringBuilder query)
        {
            query.AppendLine("from");
            query.AppendLine("evento e");
            query.AppendLine("inner");
            query.AppendLine("join evento_tipo et on");
            query.AppendLine("e.tipo_evento_id = et.id");
            query.AppendLine("where");
            query.AppendLine("et.ativo = true");
            query.AppendLine("and et.excluido = false");

            query.AppendLine("and e.excluido = false");

            query.AppendLine("and e.status = 1");
            query.AppendLine("and e.dre_id is null and e.ue_id is null");

            query.AppendLine("and ((extract(month from e.data_fim) = @mes) or (extract(month from e.data_inicio) = @mes)) ");
            query.AppendLine("and ((extract(day from e.data_fim) = @dia) or (extract(day from e.data_inicio) = @dia)) ");
            query.AppendLine("and e.tipo_calendario_id = @IdTipoCalendario");
        }

        private void MontaQueryEventosPorDiaFromWhereVisualizacaoSMEDreUe(StringBuilder query, string dreId, string ueId, bool mostraEventosSme)
        {
            query.AppendLine("from");
            query.AppendLine("evento e");
            query.AppendLine("inner");
            query.AppendLine("join evento_tipo et on");
            query.AppendLine("e.tipo_evento_id = et.id");
            query.AppendLine("where");
            query.AppendLine("et.ativo = true");
            query.AppendLine("and et.excluido = false");
            query.AppendLine("and e.excluido = false");
            query.AppendLine("and e.status = 1");
            query.AppendLine("and((extract(month from e.data_fim) = @mes) or(extract(month from e.data_inicio) = @mes))");
            query.AppendLine("and((extract(day from e.data_fim) = @dia) or(extract(day from e.data_inicio) = @dia))");

            if (!string.IsNullOrEmpty(dreId))
                query.AppendLine("and e.dre_id = @dreId");

            if (string.IsNullOrEmpty(ueId))
            {
                query.AppendLine("and e.ue_id is null");
            }
            else query.AppendLine("and e.ue_id = @ueId");

            query.AppendLine("and e.tipo_calendario_id = @IdTipoCalendario");

            if (mostraEventosSme)
            {
                query.AppendLine("union");
                MontaQueryEventosPorDiaCabecalho(query);
                MontaQueryEventosPorDiaFromWhereVisualizacaoSME(query);
            }
        }

        private void MontaQueryEventosPorDiaFromWhereVisualizacaoUe(StringBuilder query, string dreId, string ueId, bool mostraEventosSme)
        {
            query.AppendLine("from");
            query.AppendLine("evento e");
            query.AppendLine("inner");
            query.AppendLine("join ue u");
            query.AppendLine("on e.ue_id = u.ue_id");
            query.AppendLine("inner");
            query.AppendLine("join v_abrangencia_sintetica a");
            query.AppendLine("on u.id = a.ue_id");
            query.AppendLine("join evento_tipo et on");
            query.AppendLine("e.tipo_evento_id = et.id");
            query.AppendLine("where");
            query.AppendLine("et.ativo = true");
            query.AppendLine("and et.excluido = false");
            query.AppendLine("and e.excluido = false");

            query.AppendLine("and e.ue_id = @ueId");
            query.AppendLine("and e.status = 1");

            query.AppendLine("and((extract(month from e.data_fim) = @mes) or(extract(month from e.data_inicio) = @mes))");
            query.AppendLine("and((extract(day from e.data_fim) = @dia) or(extract(day from e.data_inicio) = @dia))");
            query.AppendLine("and a.usuario_id = (select id from usuario u where u.login = @usuarioRf)");
            query.AppendLine("and a.perfil = @usuarioPerfil");

            if (mostraEventosSme)
            {
                query.AppendLine("union");
                MontaQueryEventosPorDiaCabecalho(query);
                MontaQueryEventosPorDiaFromWhereVisualizacaoSME(query);
            }
        }

        private void MontaQueryEventosPorDiaVisualizacaoCriador(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, StringBuilder query, bool EhDataInicial)
        {
            query.AppendLine("from");
            query.AppendLine("evento e");
            query.AppendLine("inner");
            query.AppendLine("join evento_tipo et on");
            query.AppendLine("e.tipo_evento_id = et.id");
            query.AppendLine("where");
            query.AppendLine("et.ativo = true");
            query.AppendLine("and et.excluido = false");
            query.AppendLine("and e.excluido = false");
            query.AppendLine("and e.criado_rf = @usuarioRf");

            if (EhDataInicial)
            {
                query.AppendLine("and extract(month from e.data_inicio) = @mes");
                query.AppendLine("and extract(day from e.data_inicio) = @dia");
            }
            else
            {
                query.AppendLine("and extract(month from e.data_fim) = @mes");
                query.AppendLine("and extract(day from e.data_fim) = @dia");
            }

            if (!string.IsNullOrEmpty(calendarioEventosMesesFiltro.DreId))
                query.AppendLine("and e.dre_id = @DreId");

            if (calendarioEventosMesesFiltro.IdTipoCalendario > 0)
                query.AppendLine("and e.tipo_calendario_id = @IdTipoCalendario");

            if (!string.IsNullOrEmpty(calendarioEventosMesesFiltro.UeId))
                query.AppendLine("and e.ue_id = @UeId");
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
            var query = new StringBuilder();
            query.AppendLine("select distinct a.dia, a.tipoevento from(");

            MontaQueryQuantidadeDeEventosPorDiaCabecalho(query, true);
            MontaQueryQuantidadeDeEventosPorDiaWhereFromParaVisualizacaoGeral(calendarioEventosMesesFiltro, query, podeVisualizarEventosLocalOcorrenciaDre,
                usuarioTemPerfilSupervisorOuDiretor, podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme, true);

            query.AppendLine("group by e.id, dia, tipoevento");
            query.AppendLine("union distinct");

            MontaQueryQuantidadeDeEventosPorDiaCabecalho(query, false);
            MontaQueryQuantidadeDeEventosPorDiaWhereFromParaVisualizacaoGeral(calendarioEventosMesesFiltro, query, podeVisualizarEventosLocalOcorrenciaDre,
                usuarioTemPerfilSupervisorOuDiretor, podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme, false);

            query.AppendLine("group by e.id, dia, tipoevento");
            query.AppendLine("union distinct");

            /////

            MontaQueryQuantidadeDeEventosPorDiaCabecalho(query, true);
            MontaQueryQuantidadeDeEventosPorDiaWhereFromParaVisualizacaoParaCriador(calendarioEventosMesesFiltro, query, true);

            query.AppendLine("group by e.id, dia, tipoevento");
            query.AppendLine("union distinct");

            MontaQueryQuantidadeDeEventosPorDiaCabecalho(query, false);
            MontaQueryQuantidadeDeEventosPorDiaWhereFromParaVisualizacaoParaCriador(calendarioEventosMesesFiltro, query, false);

            query.AppendLine("group by e.id, dia, tipoevento");

            if (calendarioEventosMesesFiltro.EhEventoSme)
            {
                query.AppendLine("union distinct");

                MontaQueryQuantidadeDeEventosPorDiaCabecalho(query, true);
                MontaQueryQuantidadeDeEventosPorDiaWhereFromParaVisualizacaoSmeDre(calendarioEventosMesesFiltro, query, podeVisualizarEventosLocalOcorrenciaDre, usuarioTemPerfilSupervisorOuDiretor,
                    podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme, true);

                query.AppendLine("group by e.id, dia, tipoevento");
                query.AppendLine("union distinct");

                MontaQueryQuantidadeDeEventosPorDiaCabecalho(query, false);
                MontaQueryQuantidadeDeEventosPorDiaWhereFromParaVisualizacaoSmeDre(calendarioEventosMesesFiltro, query, podeVisualizarEventosLocalOcorrenciaDre, usuarioTemPerfilSupervisorOuDiretor,
                    podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme, false);

                query.AppendLine("group by e.id, dia, tipoevento");
            }

            query.AppendLine(")a");
            query.AppendLine("order by a.dia");

            return await database.Conexao.QueryAsync<EventosPorDiaRetornoQueryDto>(query.ToString(), new
            {
                IdTipoCalendario = calendarioEventosMesesFiltro.IdTipoCalendario,
                DreId = calendarioEventosMesesFiltro.DreId,
                UeId = calendarioEventosMesesFiltro.UeId,
                mes,
                usuarioId = usuario.Id,
                usuarioPerfil,
                usuarioRf = usuario.CodigoRf
            });
        }

        private static void MontaQueryQuantidadeDeEventosPorDiaCabecalho(StringBuilder query, bool dataInicio)
        {
            if (dataInicio)
                query.AppendLine("select distinct e.id, extract(day from e.data_inicio) as dia,");
            else query.AppendLine("select distinct e.id, extract(day from e.data_fim) as dia,");

            query.AppendLine("case");
            query.AppendLine("when e.dre_id is not null and e.ue_id is null then 'DRE'");
            query.AppendLine("when e.ue_id is not null and e.dre_id is not null  then 'UE'");
            query.AppendLine("when e.ue_id is null and e.dre_id is null then 'SME'");
            query.AppendLine("end as TipoEvento");
        }

        private static void MontaQueryQuantidadeDeEventosPorDiaWhereFromParaVisualizacaoGeral(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, StringBuilder query,
            bool podeVisualizarEventosLocalOcorrenciaDre, bool usuarioTemPerfilSupervisorOuDiretor, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme, bool ehDataInicio)
        {
            query.AppendLine("from evento e");
            query.AppendLine("inner");
            query.AppendLine("join evento_tipo et on");
            query.AppendLine("e.tipo_evento_id = et.id");
            query.AppendLine("inner");
            query.AppendLine("join v_abrangencia a on");
            query.AppendLine("a.ue_codigo = e.ue_id");
            query.AppendLine("and a.dre_codigo = e.dre_id");
            query.AppendLine("and a.usuario_id = @usuarioId");
            query.AppendLine("and a.usuario_perfil = @usuarioPerfil");
            query.AppendLine("where");
            query.AppendLine("et.ativo = true");
            query.AppendLine("and et.excluido = false");
            query.AppendLine("and e.excluido = false");

            if (ehDataInicio)
                query.AppendLine("and extract(month from e.data_inicio) = @mes");
            else query.AppendLine("and extract(month from e.data_fim) = @mes");

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

        private static void MontaQueryQuantidadeDeEventosPorDiaWhereFromParaVisualizacaoSmeDre(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, StringBuilder query,
             bool podeVisualizarEventosLocalOcorrenciaDre, bool usuarioTemPerfilSupervisorOuDiretor, bool podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme, bool ehDataInicio)
        {
            query.AppendLine("from evento e");
            query.AppendLine("inner");
            query.AppendLine("join evento_tipo et on");
            query.AppendLine("e.tipo_evento_id = et.id");
            query.AppendLine("where");
            query.AppendLine("et.ativo = true");
            query.AppendLine("and et.excluido = false");
            query.AppendLine("and e.excluido = false");

            if (ehDataInicio)
                query.AppendLine("and extract(month from e.data_inicio) = @mes");
            else query.AppendLine("and extract(month from e.data_fim) = @mes");

            if (calendarioEventosMesesFiltro.IdTipoCalendario > 0)
                query.AppendLine("and e.tipo_calendario_id = @IdTipoCalendario");

            StringBuilder queryDreUe = new StringBuilder();

            if (!string.IsNullOrEmpty(calendarioEventosMesesFiltro.DreId))
                queryDreUe.AppendLine("and e.dre_id = @DreId");

            if (!string.IsNullOrEmpty(calendarioEventosMesesFiltro.UeId))
                queryDreUe.AppendLine("and e.ue_id = @UeId");

            if (!String.IsNullOrEmpty(queryDreUe.ToString()))
                queryDreUe.AppendLine(")");

            if (podeVisualizarEventosLocalOcorrenciaDre)
            {
                queryDreUe.AppendLine($"{(string.IsNullOrEmpty(queryDreUe.ToString()) ? "and" : "or")} ((e.dre_id is null and e.ue_id is null) or (e.dre_id is not null and e.ue_id is null))");
            }
            else
            {
                queryDreUe.AppendLine($"{(string.IsNullOrEmpty(queryDreUe.ToString()) ? "and" : "or")} ((e.dre_id is null and e.ue_id is null)");
                queryDreUe.AppendLine("and et.local_ocorrencia != 2)");
            }

            if (!String.IsNullOrEmpty(queryDreUe.ToString()))
            {
                queryDreUe.Insert(queryDreUe.ToString().IndexOf("and") + 4, "((").Insert(queryDreUe.ToString().Length - 1, ")");
                query.AppendLine(queryDreUe.ToString());
            }

            if (usuarioTemPerfilSupervisorOuDiretor || podeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme)
                query.AppendLine("and e.status IN (1,2)");
            else query.AppendLine("and e.status = 1");
        }

        private void MontaQueryQuantidadeDeEventosPorDiaWhereFromParaVisualizacaoParaCriador(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, StringBuilder query, bool ehDataInicio)
        {
            query.AppendLine("from");
            query.AppendLine("evento e");
            query.AppendLine("inner");
            query.AppendLine("join evento_tipo et on");
            query.AppendLine("e.tipo_evento_id = et.id");
            query.AppendLine("where");
            query.AppendLine("et.ativo = true");
            query.AppendLine("and et.excluido = false");
            query.AppendLine("and e.excluido = false");
            query.AppendLine("and e.status = 2");
            query.AppendLine("and e.criado_rf = @usuarioRf");

            if (ehDataInicio)
                query.AppendLine("and extract(month from e.data_inicio) = @mes");
            else query.AppendLine("and extract(month from e.data_fim) = @mes");

            if (calendarioEventosMesesFiltro.IdTipoCalendario > 0)
                query.AppendLine("and e.tipo_calendario_id = @IdTipoCalendario");

            if (!string.IsNullOrEmpty(calendarioEventosMesesFiltro.DreId))
                query.AppendLine("and e.dre_id = @DreId");

            if (!string.IsNullOrEmpty(calendarioEventosMesesFiltro.UeId))
                query.AppendLine("and e.ue_id = @UeId");
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
    }
}