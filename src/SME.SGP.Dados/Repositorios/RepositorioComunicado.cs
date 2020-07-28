using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
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
    }
}