﻿using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.EscolaAqui.ComunicadosFiltro;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioComunicado : RepositorioBase<Comunicado>, IRepositorioComunicado
    {
        private readonly Func<string, string> camposComunicado =
            (prefixo) => string.Format(
                @"{0}.id,
                  {0}.titulo,
                  {0}.descricao,
                  {0}.data_envio,
                  {0}.data_expiracao,
                  {0}.criado_em,
                  {0}.criado_por,
                  {0}.alterado_em,
                  {0}.alterado_por,
                  {0}.criado_rf,
                  {0}.alterado_rf,
                  {0}.ano_letivo,
                  {0}.modalidade,
                  {0}.semestre,
                  {0}.tipo_comunicado,
                  {0}.codigo_dre,
                  {0}.codigo_ue,
                  {0}.excluido,
                  {0}.tipo_calendario_id,
                  {0}.evento_id,
                  {0}.alunos_especificados", prefixo);

        private string queryComunicadoListagem()
        {
            var query = new StringBuilder();

            query.AppendLine(@"
						SELECT
							{0}
						FROM ({1}) c
						LEFT JOIN comunidado_grupo cg
							on cg.comunicado_id = c.id
						LEFT join grupo_comunicado g
							on cg.grupo_comunicado_id = g.id
                        ");

            query.AppendLine(@"
						WHERE c.excluido = false");

            return query.ToString();
        }

        public RepositorioComunicado(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<PaginacaoResultadoDto<Comunicado>> ListarPaginado(FiltroComunicadoDto filtro, Paginacao paginacao)
        {
            StringBuilder query = new StringBuilder();

            var queryPrincipal = MontarConsultaPricipal(filtro, paginacao, ehContador: false);

            var queryCount = MontarConsultaPricipal(filtro, paginacao, ehContador: true);

            if (paginacao == null)
                paginacao = new Paginacao(1, 10);

            query.AppendFormat(queryComunicadoListagem(), ObterCamposListagem("c", "g", "t"), queryPrincipal);

            var parametros = new
            {
                filtro.DataEnvio,
                filtro.DataExpiracao,
                filtro.Titulo,
                filtro.GruposId,
                filtro.AnoLetivo,
                filtro.Modalidade,
                filtro.Semestre,
                filtro.CodigoDre,
                filtro.CodigoUe,
                filtro.Turmas
            };

            var retornoPaginado = new PaginacaoResultadoDto<Comunicado>()
            {
                Items = await database.Conexao.QueryAsync<Comunicado, GrupoComunicacao, Comunicado>(query.ToString(), (comunicado, grupo) =>
                {
                    comunicado.GruposComunicacao.Add(grupo);

                    return comunicado;
                },
                parametros,
                splitOn: "id,GrupoId")
            };

            retornoPaginado.TotalRegistros = (await database.Conexao.QueryAsync<int>(queryCount.ToString(), parametros)).Sum();

            retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);

            return retornoPaginado;
        }

        private string ObterConsultaListagemPrincipal(string whereGrupo, string limite, string where, string whereTurma, bool ehContador)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("select");

            if (!ehContador)
                builder.AppendLine(camposComunicado("co"));
            else
                builder.AppendLine("count(*)");

            builder.AppendLine(@" from comunicado co
                                  where co.excluido = false");

            if (!string.IsNullOrWhiteSpace(whereGrupo))
                builder.AppendLine($@"and exists(select 1
                                      from comunidado_grupo cgr
                                      where {whereGrupo}
                                      and cgr.comunicado_id = co.id)");

            if (!string.IsNullOrWhiteSpace(whereTurma))
                builder.AppendLine($@"and exists(select 1
                                      from comunicado_turma ct
                                       where {whereTurma}
                                       and ct.comunicado_id = co.id
                                       and ct.excluido = false)");

            builder.AppendLine(where);

            if (!ehContador)
                builder.AppendLine("order by co.id");

            if (!ehContador && !string.IsNullOrWhiteSpace(limite))
                builder.AppendLine(limite);

            return builder.ToString();
        }

        private string MontarConsultaPricipal(FiltroComunicadoDto filtro, Paginacao paginacao, bool ehContador = false)
        {
            var whereGrupo = filtro.GruposId?.Length > 0 ? "cgr.grupo_comunicado_id = ANY(@gruposId)" : "";

            var limite = paginacao.QuantidadeRegistros != 0 ? string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", paginacao.QuantidadeRegistrosIgnorados, paginacao.QuantidadeRegistros) : "";

            var whereTurma = (filtro.Turmas?.Any() ?? false) ? "ct.turma_codigo = Any(@Turmas)" : "";

            var where = MontaWhereListar(filtro, "co");

            return ObterConsultaListagemPrincipal(whereGrupo, limite, where, whereTurma, ehContador);
        }

        private string MontaWhereListar(FiltroComunicadoDto filtro, string prefixo)
        {
            var where = new StringBuilder();

            where.AppendLine($"AND {prefixo}.ano_letivo = @AnoLetivo");

            if (!string.IsNullOrEmpty(filtro.Titulo))
            {
                filtro.Titulo = $"%{filtro.Titulo.ToUpperInvariant()}%";
                where.AppendLine($"AND (upper(f_unaccent({prefixo}.titulo)) LIKE @titulo)");
            }

            if (filtro.DataEnvio.HasValue)
                where.AppendLine($"AND (date({prefixo}.data_envio) = @DataEnvio)");

            if (filtro.DataExpiracao.HasValue)
                where.AppendLine($"AND (date({prefixo}.data_expiracao) = @DataExpiracao)");

            if (!string.IsNullOrWhiteSpace(filtro.CodigoDre) && !filtro.CodigoDre.Equals("todas"))
                where.AppendLine($"AND {prefixo}.codigo_dre = @CodigoDre");

            if (!string.IsNullOrWhiteSpace(filtro.CodigoUe) && !filtro.CodigoUe.Equals("todas"))
                where.AppendLine($"AND {prefixo}.codigo_ue = @CodigoUe");

            if (filtro.Modalidade > 0)
                where.AppendLine($"AND {prefixo}.modalidade = @Modalidade");

            if (filtro.Semestre > 0)
                where.AppendLine($"AND {prefixo}.semestre = @Semestre");

            return where.ToString();
        }

        private string ObterCamposListagem(string prefixoComunicado, string prefixoGrupoComunicado, string prefixoTurmaComunicado)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"{camposComunicado(prefixoComunicado)},");

            builder.AppendLine($@"{prefixoGrupoComunicado}.id as GrupoId,");
            builder.AppendLine($@"{prefixoGrupoComunicado}.id,");
            builder.AppendLine($@"{prefixoGrupoComunicado}.nome,");
            builder.AppendLine($@"{prefixoGrupoComunicado}.tipo_escola_id,");
            builder.AppendLine($@"{prefixoGrupoComunicado}.tipo_ciclo_id,");
            builder.AppendLine($@"{prefixoGrupoComunicado}.etapa_ensino_id");

            return builder.ToString();
        }

        public async Task<ComunicadosTotaisResultado> ObterComunicadosTotaisSme(int anoLetivo, string codigoDre, string codigoUe)
        {

            string filtroPorDre = "";
            string filtroPorUe = "";

            if (!String.IsNullOrEmpty(codigoDre))
                filtroPorDre = " and codigo_dre = @codigoDre ";

            if (!String.IsNullOrEmpty(codigoUe))
                filtroPorDre = " and codigo_ue = @codigoUe ";

            var sql = $@"select
	                        distinct 
	                        (select count(id) from comunicado where excluido = false and ano_letivo = @anoLetivo and data_expiracao >= current_date {filtroPorDre} {filtroPorUe} ) as TotalComunicadosVigentes,
	                        (select count(id) from comunicado where excluido = false and ano_letivo = @anoLetivo and data_expiracao < current_date {filtroPorDre} {filtroPorUe}) as TotalComunicadosExpirados 
                        from comunicado";
            var parametros = new { anoLetivo, codigoDre, codigoUe };
            return await database.QueryFirstAsync<ComunicadosTotaisResultado>(sql, parametros);
        }

        public async Task<IEnumerable<ComunicadosTotaisPorDreResultado>> ObterComunicadosTotaisAgrupadosPorDre(int anoLetivo)
        {
            var sql = @"select * from (
                                        select
                                            distinct
                                            dre.dre_id,
                                            dre.abreviacao nomeAbreviadoDre, 
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao >= current_date and dre.id = 1) as totalComunicadosVigentes,
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao < current_date and dre.id = 1) as totalComunicadosExpirados 
                                        from comunicado com
                                        right join dre on dre.dre_id = com.codigo_dre 
                                        where dre.id = 1
                                        union all 
                                        select
                                            distinct
                                            dre.dre_id,
                                            dre.abreviacao nomeAbreviadoDre, 
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao >= current_date and dre.id = 2) as totalComunicadosVigentes,
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao < current_date and dre.id = 2) as totalComunicadosExpirados 
                                        from comunicado com
                                        right join dre on dre.dre_id = com.codigo_dre 
                                        where dre.id = 2
                                        union all 
                                        select
                                            distinct
                                            dre.dre_id,
                                            dre.abreviacao nomeAbreviadoDre, 
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao >= current_date and dre.id = 3) as totalComunicadosVigentes,
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao < current_date and dre.id = 3) as totalComunicadosExpirados 
                                        from comunicado com
                                        right join dre on dre.dre_id = com.codigo_dre 
                                        where dre.id = 3
                                        union all 
                                        select
                                            distinct
                                            dre.dre_id,
                                            dre.abreviacao nomeAbreviadoDre, 
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao >= current_date and dre.id = 4) as totalComunicadosVigentes,
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao < current_date and dre.id = 4) as totalComunicadosExpirados 
                                        from comunicado com
                                        right join dre on dre.dre_id = com.codigo_dre 
                                        where dre.id = 4
                                        union all 
                                        select
                                            distinct
                                            dre.dre_id,
                                            dre.abreviacao nomeAbreviadoDre, 
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao >= current_date and dre.id = 5) as totalComunicadosVigentes,
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao < current_date and dre.id = 5) as totalComunicadosExpirados 
                                        from comunicado com
                                        right join dre on dre.dre_id = com.codigo_dre 
                                        where dre.id = 5
                                        union all 
                                        select
                                            distinct
                                            dre.dre_id,
                                            dre.abreviacao nomeAbreviadoDre, 
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao >= current_date and dre.id = 6) as totalComunicadosVigentes,
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao < current_date and dre.id = 6) as totalComunicadosExpirados 
                                        from comunicado com
                                        right join dre on dre.dre_id = com.codigo_dre 
                                        where dre.id = 6
                                        union all 
                                        select
                                            distinct
                                            dre.dre_id,
                                            dre.abreviacao nomeAbreviadoDre, 
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao >= current_date and dre.id = 7) as totalComunicadosVigentes,
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao < current_date and dre.id = 7) as totalComunicadosExpirados 
                                        from comunicado com
                                        right join dre on dre.dre_id = com.codigo_dre 
                                        where dre.id = 7
                                        union all 
                                        select
                                            distinct
                                            dre.dre_id,
                                            dre.abreviacao nomeAbreviadoDre, 
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao >= current_date and dre.id = 8) as totalComunicadosVigentes,
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao < current_date and dre.id = 8) as totalComunicadosExpirados 
                                        from comunicado com
                                        right join dre on dre.dre_id = com.codigo_dre 
                                        where dre.id = 8
                                        union all 
                                        select
                                            distinct
                                            dre.dre_id,
                                            dre.abreviacao nomeAbreviadoDre, 
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao >= current_date and dre.id = 9) as totalComunicadosVigentes,
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao < current_date and dre.id = 9) as totalComunicadosExpirados 
                                        from comunicado com
                                        right join dre on dre.dre_id = com.codigo_dre 
                                        where dre.id = 9
                                        union all 
                                        select
                                            distinct
                                            dre.dre_id,
                                            dre.abreviacao nomeAbreviadoDre, 
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao >= current_date and dre.id = 10) as totalComunicadosVigentes,
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao < current_date and dre.id = 10) as totalComunicadosExpirados 
                                        from comunicado com
                                        right join dre on dre.dre_id = com.codigo_dre 
                                        where dre.id = 10
                                        union all 
                                        select
                                            distinct
                                            dre.dre_id,
                                            dre.abreviacao nomeAbreviadoDre, 
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao >= current_date and dre.id = 11) as totalComunicadosVigentes,
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao < current_date and dre.id = 11) as totalComunicadosExpirados 
                                        from comunicado com
                                        right join dre on dre.dre_id = com.codigo_dre 
                                        where dre.id = 11
                                        union all 
                                        select
                                            distinct
                                            dre.dre_id,
                                            dre.abreviacao nomeAbreviadoDre, 
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao >= current_date and dre.id = 12) as totalComunicadosVigentes,
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao < current_date and dre.id = 12) as totalComunicadosExpirados 
                                        from comunicado com
                                        right join dre on dre.dre_id = com.codigo_dre 
                                        where dre.id = 12
                                        union all 
                                        select
                                            distinct
                                            dre.dre_id,
                                            dre.abreviacao nomeAbreviadoDre, 
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao >= current_date and dre.id = 13) as totalComunicadosVigentes,
                                            (select count(com.id) from comunicado com right join dre on dre.dre_id = com.codigo_dre where excluido = false and ano_letivo = @anoLetivo and data_expiracao < current_date and dre.id = 13) as totalComunicadosExpirados 
                                        from comunicado com
                                        right join dre on dre.dre_id = com.codigo_dre 
                                        where dre.id = 13 ) as aaa order by aaa.dre_id";
            var parametros = new { anoLetivo };
            return await database.QueryAsync<ComunicadosTotaisPorDreResultado>(sql, parametros);
        }

        public Task<IEnumerable<ComunicadoParaFiltroDaDashboardDto>> ObterComunicadosParaFiltroDaDashboard(FiltroObterComunicadosParaFiltroDaDashboardDto filtro)
        {
            var comunicadoAlias = "cm";
            var comunicadoTumaAlias = "cmt";
            var turmaAlias = "tur";

            var sql = new StringBuilder($@"SELECT
                                            {comunicadoAlias}.id AS Id,
                                            {comunicadoAlias}.titulo AS Titulo,
                                            {comunicadoAlias}.data_envio AS DataEnvio,
                                            {comunicadoAlias}.codigo_dre AS CodigoDre,
                                            {comunicadoAlias}.codigo_ue AS CodigoUe,
                                            {comunicadoAlias}.modalidade AS Modalidade
                                        FROM comunicado {comunicadoAlias} ");

            if (!string.IsNullOrWhiteSpace(filtro.CodigoTurma))
            {
                sql.Append($@" INNER JOIN comunicado_turma {comunicadoTumaAlias} ON {comunicadoAlias}.id = {comunicadoTumaAlias}.comunicado_id ");
                sql.Append($@" INNER JOIN turma {turmaAlias} ON {comunicadoTumaAlias}.turma_codigo = {turmaAlias}.turma_id ");
            }

            sql.Append(MontarCondicoesDaConsultaObterComunicadosParaFiltroDaDashboard(filtro, comunicadoAlias, comunicadoTumaAlias, turmaAlias));

            sql.Append($@" ORDER BY {comunicadoAlias}.titulo LIMIT 10");

            var parametros = new
            {
                filtro.AnoEscolar,
                filtro.AnoLetivo,
                filtro.CodigoDre,
                filtro.CodigoTurma,
                filtro.CodigoUe,
                filtro.DataEnvioFinal,
                filtro.DataEnvioInicial,
                filtro.GruposIds,
                filtro.Modalidade,
                filtro.Semestre,
                filtro.Titulo
            };
            return database.QueryAsync<ComunicadoParaFiltroDaDashboardDto>(sql.ToString(), parametros);
        }

        private string MontarCondicoesDaConsultaObterComunicadosParaFiltroDaDashboard(FiltroObterComunicadosParaFiltroDaDashboardDto filtro, string comunicadoAlias,
            string comunicadoTumaAlias, string turmaAlias)
        {
            var where = new StringBuilder($" WHERE {comunicadoAlias}.ano_letivo = @anoLetivo ");
            if (!string.IsNullOrWhiteSpace(filtro.CodigoDre))
                where.Append($" AND {comunicadoAlias}.codigo_dre = @CodigoDre");

            if (!string.IsNullOrWhiteSpace(filtro.CodigoUe))
                where.Append($" AND {comunicadoAlias}.codigo_ue = @CodigoUe");

            if (filtro.GruposIds != null)
                where.Append($" AND {comunicadoAlias}.grupo_comunicado_id = ANY(@GruposIds)");

            if (filtro.Modalidade != null)
                where.Append($" AND {comunicadoAlias}.modalidade = @Modalidade");

            if (filtro.Semestre != null)
                where.Append($" AND {comunicadoAlias}.semestre = @Semestre");

            if (filtro.AnoEscolar != null)
                where.Append($" AND {comunicadoAlias}.series_resumidas = @AnoEscolar");

            if (!string.IsNullOrWhiteSpace(filtro.CodigoTurma))
                where.Append($" AND {comunicadoTumaAlias}.turma_codigo = @CodigoTurma");

            if (filtro.DataEnvioInicial != null)
                where.Append($" AND {comunicadoAlias}.data_envio >= @DataEnvioInicial");

            if (filtro.DataEnvioFinal != null)
                where.Append($" AND {comunicadoAlias}.data_envio <= @DataEnvioFinal");

            if (!string.IsNullOrWhiteSpace(filtro.Titulo))
            {
                filtro.Titulo = filtro.Titulo.ToUpperInvariant();
                where.Append($" AND lower(f_unaccent(cm.titulo)) LIKE lower(f_unaccent('%" + filtro.Titulo + "%'))");
            }

            return where.ToString();
        }

        public async Task<bool> VerificaExistenciaComunicadoParaEvento(long eventoId)
        {
            var sql = $@"select count(id) from comunicado where not excluido and data_expiracao >= current_date and evento_id = @eventoId";
            var parametros = new { eventoId };
            var quantidadeComunicadosComEvento = await database.QuerySingleAsync<int>(sql, parametros);
            return (quantidadeComunicadosComEvento > 0 ? true : false);

        }
    }
}