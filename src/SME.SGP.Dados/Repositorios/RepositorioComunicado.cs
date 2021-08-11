using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
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
        private string ObterCamposListagem(string prefixoComunicado, string prefixoGrupoComunicado, string prefixoTurmaComunicado)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"{camposComunicado(prefixoComunicado)},");
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
            var comunicadoModalidadeAlias = "cmm";

            var sql = new StringBuilder($@"SELECT
                                            {comunicadoAlias}.id AS Id,
                                            {comunicadoAlias}.titulo AS Titulo,
                                            {comunicadoAlias}.data_envio AS DataEnvio,
                                            {comunicadoAlias}.codigo_dre AS CodigoDre,
                                            {comunicadoAlias}.codigo_ue AS CodigoUe,
                                            {comunicadoAlias}.modalidade AS Modalidade,
                                            CASE
                                               WHEN (select count(id) from comunicado_modalidade where comunicado_id = {comunicadoAlias}.id) = 0 THEN 0
                                               WHEN (select count(id) from comunicado_modalidade where comunicado_id = {comunicadoAlias}.id) = 1 THEN 0
                                               WHEN (select count(id) from comunicado_modalidade where comunicado_id = {comunicadoAlias}.id) > 1 THEN 1
                                            END agruparModalidade
                                        FROM comunicado {comunicadoAlias} ");

            if (!string.IsNullOrWhiteSpace(filtro.CodigoTurma))
            {
                sql.Append($@" INNER JOIN comunicado_turma {comunicadoTumaAlias} ON {comunicadoAlias}.id = {comunicadoTumaAlias}.comunicado_id ");
                sql.Append($@" INNER JOIN turma {turmaAlias} ON {comunicadoTumaAlias}.turma_codigo = {turmaAlias}.turma_id ");
            }

            if (filtro.Modalidades != null && filtro.Modalidades.Any())
            {
                sql.Append($@" INNER JOIN comunicado_modalidade {comunicadoModalidadeAlias} ON {comunicadoAlias}.id = {comunicadoModalidadeAlias}.comunicado_id ");
            }

            sql.Append(MontarCondicoesDaConsultaObterComunicadosParaFiltroDaDashboard(filtro, comunicadoAlias, comunicadoTumaAlias, turmaAlias, comunicadoModalidadeAlias));

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
                filtro.Modalidades,
                filtro.Semestre,
                filtro.Titulo
            };
            return database.QueryAsync<ComunicadoParaFiltroDaDashboardDto>(sql.ToString(), parametros);
        }

        private string MontarCondicoesDaConsultaObterComunicadosParaFiltroDaDashboard(FiltroObterComunicadosParaFiltroDaDashboardDto filtro, string comunicadoAlias,
            string comunicadoTumaAlias, string turmaAlias, string comunicadoModalidadeAlias)
        {
            var where = new StringBuilder($" WHERE {comunicadoAlias}.ano_letivo = @anoLetivo ");

            where.Append(!string.IsNullOrWhiteSpace(filtro.CodigoDre) ? $" AND {comunicadoAlias}.codigo_dre = @CodigoDre" : $" AND {comunicadoAlias}.codigo_dre is null");

            where.Append(!string.IsNullOrWhiteSpace(filtro.CodigoUe) ? $" AND {comunicadoAlias}.codigo_ue = @CodigoUe" : $" AND {comunicadoAlias}.codigo_ue is null");

            if (filtro.Modalidades != null)
                where.Append($" AND {comunicadoModalidadeAlias}.modalidade = ANY(@Modalidades)");

            if (filtro.Semestre != null)
                where.Append($" AND {comunicadoAlias}.semestre = @Semestre");

            if (filtro.AnoEscolar != null)
                where.Append($" AND {comunicadoAlias}.series_resumidas = @AnoEscolar");

            if (!string.IsNullOrWhiteSpace(filtro.CodigoTurma))
                where.Append($" AND {comunicadoTumaAlias}.turma_codigo = @CodigoTurma");

            if (filtro.DataEnvioInicial != null)
                where.Append($" AND date({comunicadoAlias}.data_envio) >= @DataEnvioInicial");

            if (filtro.DataEnvioFinal != null)
                where.Append($" AND date({comunicadoAlias}.data_envio) <= @DataEnvioFinal");

            if (!string.IsNullOrWhiteSpace(filtro.Titulo))
            {
                filtro.Titulo = filtro.Titulo.ToUpperInvariant();
                where.Append($" AND lower(f_unaccent(cm.titulo)) LIKE lower(f_unaccent('%" + filtro.Titulo + "%'))");
            }

            where.Append($" and not {comunicadoAlias}.excluido ");

            return where.ToString();
        }

        public async Task<bool> VerificaExistenciaComunicadoParaEvento(long eventoId)
        {
            var sql = $@"select count(id) from comunicado where not excluido and data_expiracao >= current_date and evento_id = @eventoId";
            var parametros = new { eventoId };
            var quantidadeComunicadosComEvento = await database.QuerySingleAsync<int>(sql, parametros);
            return (quantidadeComunicadosComEvento > 0 ? true : false);

        }

        public Task<IEnumerable<ComunicadoTurmaDto>> ObterComunicadosTurma(long comunicadoId)
        {
            var sql = $@"select turma_codigo AS CodigoTurma from comunicado_turma ct where comunicado_id = @comunicadoId";
            var parametros = new { comunicadoId };
            return database.QueryAsync<ComunicadoTurmaDto>(sql, parametros);

        }

        public async Task<IEnumerable<ComunicadoAlunoReduzidoDto>> ObterComunicadosReduzidosPorTipo(TipoComunicado tipoComunicado)
        {
            var sql = $@"select data_envio, tipo_comunicado, titulo, false as leitura from comunicado c where c.tipo_comunicado = @tipoComunicado;";
            var parametros = new { tipoComunicado };
            return await database.QueryAsync<ComunicadoAlunoReduzidoDto>(sql, parametros);
        }

        public async Task<PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto>> ObterComunicadosReduzidos(string dreCodigo, string ueCodigo, string turmaCodigo, string alunoCodigo, Paginacao paginacao)
        {
            StringBuilder sql = new StringBuilder();

            MontaQueryObterComunicados(paginacao, sql, false);

            sql.AppendLine(";");

            MontaQueryObterComunicados(paginacao, sql, true);

            var parametros = new { dreCodigo, ueCodigo, turmaCodigo, alunoCodigo };

            var retorno = new PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto>();

            using (var multi = await database.Conexao.QueryMultipleAsync(sql.ToString(), parametros))
            {
                retorno.Items = multi.Read<ComunicadoAlunoReduzidoDto>();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private void MontaQueryObterComunicados(Paginacao paginacao, StringBuilder sql, bool contador)
        {
            if (contador)
            {
                sql.AppendLine(" select count(n.ComunicadoId) ");
            }
            else
            {
                sql.AppendLine(" select n.* ");
            }

            sql.AppendLine(@" from
	                    ( (
	                    select
                            comunicado.id as ComunicadoId,
		                    data_envio as DataEnvio,
		                    tipo_comunicado as Categoria,
		                    titulo,
		                    '' as leitura
	                    from
		                    comunicado
	                    where
		                    not comunicado.excluido
		                    and comunicado.tipo_comunicado = 1)
                    union (
                    select
                        comunicado.id,
	                    data_envio as DataEnvio,
	                    tipo_comunicado as Categoria,
	                    titulo,
	                    '' as leitura
                    from
	                    comunicado
                    where
	                    not comunicado.excluido
	                    and comunicado.tipo_comunicado = 2 and comunicado.codigo_dre = @dreCodigo)
	                    union (
                    select
                        comunicado.id,
	                    data_envio as DataEnvio,
	                    tipo_comunicado as Categoria,
	                    titulo,
	                    '' as leitura
                    from
	                    comunicado
                    where
	                    not comunicado.excluido
	                    and comunicado.tipo_comunicado = 3 and comunicado.codigo_ue = @ueCodigo)
	                    union (
                    select
                        comunicado.id,
	                    data_envio as DataEnvio,
	                    tipo_comunicado as Categoria,
	                    titulo,
	                    '' as leitura
                    from
	                    comunicado
                    inner join comunicado_turma ct on comunicado.id = ct.comunicado_id 
                    where
	                    not comunicado.excluido
	                    and comunicado.tipo_comunicado = 5 and ct.turma_codigo = @turmaCodigo)
		                    union (
                    select
                        comunicado.id,
	                    data_envio as DataEnvio,
	                    tipo_comunicado as Categoria,
	                    titulo,
	                    '' as leitura from comunicado inner join comunicado_aluno ca on 
            comunicado.id = ca.comunicado_id where not comunicado.excluido and comunicado.tipo_comunicado = 6 and ca.aluno_codigo = @alunoCodigo)) n ");


            if (!contador)
                sql.AppendLine(" order by n.DataEnvio desc ");


            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
        }

        public async Task<IEnumerable<Comunicado>> ObterComunicadosPorIds(long[] ids)
        {
            var sql = @"select *  
                          from comunicado 
                         where id = ANY(@ids)
                           and not excluido ";
            var parametros = new { ids };
            return await database.QueryAsync<Comunicado>(sql, parametros);
        }
        public async Task<IEnumerable<int>> ObterAnosLetivosComHistoricoDeComunicados(DateTime? dataInicio, DateTime dataAtual)
        {
            var query = new StringBuilder(@"select distinct c.ano_letivo 
                                              from comunicado c 
                                             where not c.excluido ");

            if (dataInicio.HasValue)
                query.AppendLine("and c.criado_em::date between @dataInicio::date and @dataAtual::date  ");
            else
                query.AppendLine("and c.criado_em::date < @dataAtual::date ");

            query.AppendLine("order by c.ano_letivo desc");

            return await database.QueryAsync<int>(query.ToString(), new { dataInicio, dataAtual });
        }

        public async Task<PaginacaoResultadoDto<ComunicadoListaPaginadaDto>> ListarComunicados(int anoLetivo, string dreCodigo, string ueCodigo, int[] modalidades, int semestre, DateTime? dataEnvioInicio, DateTime? dataEnvioFim, DateTime? dataExpiracaoInicio, DateTime? dataExpiracaoFim, string titulo, string[] turmasCodigo, string[] anosEscolares, int[] tiposEscolas, Paginacao paginacao)
        {
            var tituloFormatado = "";
            var query = new StringBuilder(@"DROP TABLE IF EXISTS comunicadoTempPaginado;
                                            select distinct id,
                                                titulo,
                                                data_envio,
                                                data_expiracao,
                                                modalidade
                                              into temporary table comunicadoTempPaginado
                                              from (");

            query.AppendLine(MontaQueryListarComunicados(dreCodigo, ueCodigo, modalidades, dataEnvioInicio, dataEnvioFim, dataExpiracaoInicio, dataExpiracaoFim, titulo, turmasCodigo, anosEscolares, tiposEscolas));
            query.AppendLine(" union ");
            query.AppendLine(MontaQueryListarComunicadosEja(dreCodigo, ueCodigo, dataEnvioInicio, dataEnvioFim, dataExpiracaoInicio, dataExpiracaoFim, titulo, turmasCodigo, anosEscolares, tiposEscolas));
            query.AppendLine(") tb1;");

            query.AppendLine(@"select temp.id,
	                                  temp.titulo,
	                                  temp.data_envio as DataEnvio,
	                                  temp.data_expiracao as DataExpiracao,
	                                  temp.modalidade as modalidadeCodigo                                 
                                 from comunicadoTempPaginado temp
                                order by temp.data_envio desc ");

            if (paginacao.QuantidadeRegistros > 0)
                query.AppendLine($" OFFSET @quantidadeRegistrosIgnorados ROWS FETCH NEXT @quantidadeRegistros ROWS ONLY ");

            query.AppendLine("; ");

            query.AppendLine("select count(distinct temp.id) from comunicadoTempPaginado temp");

            if (!string.IsNullOrEmpty(titulo))            
                tituloFormatado = $"%{titulo.ToUpperInvariant()}%";                
           

            var retorno = new PaginacaoResultadoDto<ComunicadoListaPaginadaDto>();

            var parametros = new
            {
                paginacao.QuantidadeRegistrosIgnorados,
                paginacao.QuantidadeRegistros,
                anoLetivo,
                dreCodigo,
                ueCodigo,
                modalidades,
                semestre,
                dataEnvioInicio,
                dataEnvioFim,
                dataExpiracaoInicio,
                dataExpiracaoFim,
                tituloFormatado,
                turmasCodigo,
                anosEscolares,
                tiposEscolas
            };


            using (var multi = await database.Conexao.QueryMultipleAsync(query.ToString(), parametros))
            {
                retorno.Items = multi.Read<ComunicadoListaPaginadaDto>();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }


        private string MontaQueryListarComunicados(string dreCodigo, string ueCodigo, int[] modalidades, DateTime? dataEnvioInicio, DateTime? dataEnvioFim, DateTime? dataExpiracaoInicio, DateTime? dataExpiracaoFim, string titulo, string[] turmasCodigo, string[] anosEscolares, int[] tiposEscolas)
        {
            var query = new StringBuilder(@"select distinct c.id,
	                                               c.titulo,
	                                               c.data_envio,
	                                               c.data_expiracao,
	                                               (select array_agg(modalidade) 
                                                      from comunicado_modalidade cm2 
                                                     where cm2.comunicado_id = c.id) as Modalidade
                                              from comunicado c 
                                             inner join comunicado_modalidade cm on cm.comunicado_id = c.id 
                                              left join comunicado_turma ct on ct.comunicado_id = c.id
                                              left join turma t on t.turma_id = ct.turma_codigo
                                              left join ue on ue.ue_id = c.codigo_ue
                                             where c.ano_letivo = @anoLetivo
                                               and not c.excluido ");

            if (!string.IsNullOrEmpty(dreCodigo) && dreCodigo != "-99")
                query.AppendLine("and c.codigo_dre = @dreCodigo ");
            else
                query.AppendLine("and c.codigo_dre is null ");

            if (!string.IsNullOrEmpty(ueCodigo) && ueCodigo != "-99")
                query.AppendLine("and c.codigo_ue = @ueCodigo ");
            else
                query.AppendLine("and c.codigo_ue is null ");

            if (modalidades != null && !modalidades.Any(c => c == -99))
                query.AppendLine("and cm.modalidade = any(@modalidades) ");           

            if (anosEscolares != null && !anosEscolares.Any(c => c == "-99"))
                query.AppendLine("and t.ano = any(@anosEscolares) ");

            if (turmasCodigo != null && !turmasCodigo.Any(c => c == "-99"))
                query.AppendLine("and ct.turma_codigo = any(@turmasCodigo) ");

            if (tiposEscolas != null && !tiposEscolas.Any(c => c == -99))
                query.AppendLine("and ue.tipo_escola = any(@tiposEscolas) ");

            if (!string.IsNullOrEmpty(titulo))
                query.AppendLine("and (upper(f_unaccent(c.titulo)) LIKE @tituloFormatado) ");            

            if (dataEnvioInicio.HasValue && dataEnvioFim.HasValue)
                query.AppendLine("and c.data_envio::date between @dataEnvioInicio::date and @dataEnvioFim::date ");

            if (dataExpiracaoInicio.HasValue && dataExpiracaoFim.HasValue)
                query.AppendLine("and c.data_expiracao::date between @dataExpiracaoInicio::date and @dataExpiracaoFim::date ");

            return query.ToString();
        }

        private string MontaQueryListarComunicadosEja(string dreCodigo, string ueCodigo, DateTime? dataEnvioInicio, DateTime? dataEnvioFim, DateTime? dataExpiracaoInicio, DateTime? dataExpiracaoFim, string titulo, string[] turmasCodigo, string[] anosEscolares, int[] tiposEscolas)
        {
            var query = new StringBuilder(@"select distinct c.id,
	                                               c.titulo,
	                                               c.data_envio,
	                                               c.data_expiracao,
	                                               (select array_agg(modalidade) 
                                                      from comunicado_modalidade cm2 
                                                     where cm2.comunicado_id = c.id) as Modalidade
                                              from comunicado c 
                                             inner join comunicado_modalidade cm on cm.comunicado_id = c.id 
                                              left join comunicado_turma ct on ct.comunicado_id = c.id
                                              left join turma t on t.turma_id = ct.turma_codigo
                                              left join ue on ue.ue_id = c.codigo_ue
                                             where c.ano_letivo = @anoLetivo
                                               and not c.excluido
                                               and cm.modalidade = any(@modalidades)
                                               and t.semestre = @semestre ");

            if (!string.IsNullOrEmpty(dreCodigo) && dreCodigo != "-99")
                query.AppendLine("and c.codigo_dre = @dreCodigo ");
            else
                query.AppendLine("and c.codigo_dre is null ");

            if (!string.IsNullOrEmpty(ueCodigo) && ueCodigo != "-99")
                query.AppendLine("and c.codigo_ue = @ueCodigo ");
            else
                query.AppendLine("and c.codigo_ue is null ");            

            if (anosEscolares != null && !anosEscolares.Any(c => c == "-99"))
                query.AppendLine("and t.ano = any(@anosEscolares) ");

            if (turmasCodigo != null && !turmasCodigo.Any(c => c == "-99"))
                query.AppendLine("and ct.turma_codigo = any(@turmasCodigo) ");

            if (tiposEscolas != null && !tiposEscolas.Any(c => c == -99))
                query.AppendLine("and ue.tipo_escola = any(@tiposEscolas) ");            

            if (!string.IsNullOrEmpty(titulo))                            
                query.AppendLine("and (upper(f_unaccent(c.titulo)) LIKE @tituloFormatado) ");            

            if (dataEnvioInicio.HasValue && dataEnvioFim.HasValue)
                query.AppendLine("and c.data_envio::date between @dataEnvioInicio::date and @dataEnvioFim::date ");

            if (dataExpiracaoInicio.HasValue && dataExpiracaoFim.HasValue)
                query.AppendLine("and c.data_expiracao::date between @dataExpiracaoInicio::date and @dataExpiracaoFim::date ");

            return query.ToString();
        }

        public async Task<IEnumerable<int>> ObterSemestresPorAnoLetivoModalidadeEUeCodigo(string login, Guid perfil, int modalidade, bool consideraHistorico, int anoLetivo, string ueCodigo)
        {
            var query = new StringBuilder(@"select distinct act.turma_semestre
                                              from v_abrangencia_nivel_dre a
                                             inner join v_abrangencia_cadeia_turmas act on a.dre_id = act.dre_id
                                             where a.login = @login
                                               and a.perfil_id = @perfil
                                               and act.turma_historica = @consideraHistorico ");

            if (!string.IsNullOrEmpty(ueCodigo) && ueCodigo != "-99")
                query.AppendLine("and act.ue_codigo = @ueCodigo ");

            query.AppendLine(@"and (@modalidade = 0 or (@modalidade <> 0 and act.modalidade_codigo = @modalidade))
                               and(@anoLetivo = 0 or(@anoLetivo <> 0 and act.turma_ano_letivo = @anoLetivo)) ");

            query.AppendLine("union ");

            query.AppendLine(@"select distinct act.turma_semestre
                                 from v_abrangencia_nivel_ue a
                                inner join v_abrangencia_cadeia_turmas act on a.ue_id = act.ue_id
                                where a.login = @login
                                  and a.perfil_id = @perfil
                                  and act.turma_historica = @consideraHistorico ");

            if (!string.IsNullOrEmpty(ueCodigo) && ueCodigo != "-99")
                query.AppendLine("and act.ue_codigo = @ueCodigo ");

            query.AppendLine(@"and (@modalidade = 0 or (@modalidade <> 0 and act.modalidade_codigo = @modalidade))
                               and(@anoLetivo = 0 or(@anoLetivo <> 0 and act.turma_ano_letivo = @anoLetivo)) ");


            query.AppendLine("union ");


            query.AppendLine(@"select distinct act.turma_semestre
                                 from v_abrangencia_nivel_turma a
                                inner join v_abrangencia_cadeia_turmas act on a.turma_id = act.turma_id
                                where a.login = @login
                                  and a.perfil_id = @perfil");

            if (!string.IsNullOrEmpty(ueCodigo) && ueCodigo != "-99")
                query.AppendLine("and act.ue_codigo = @ueCodigo ");

            query.AppendLine(@"and ((@consideraHistorico = true and a.historico = true) or 
                                   (@consideraHistorico = false and a.historico = false and act.turma_historica = @consideraHistorico)) 
                               and (@modalidade = 0 or(@modalidade <> 0 and act.modalidade_codigo = @modalidade))
                               and (@anoLetivo = 0 or(@anoLetivo <> 0 and act.turma_ano_letivo = @anoLetivo)) ");

            var parametros = new
            {
                login,
                perfil,
                modalidade,
                consideraHistorico,
                anoLetivo,
                ueCodigo
            };

            return await database.QueryAsync<int>(query.ToString(), parametros);
        }
    }
}