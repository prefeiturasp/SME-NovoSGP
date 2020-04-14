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
        private readonly string fromComunicado = @"comunicado";

        private readonly string fromOffSet = @"(SELECT id, titulo, descricao, data_envio, data_expiracao, criado_em, criado_por,alterado_em,alterado_por,criado_rf,alterado_rf,excluido from comunicado {0})";

        private readonly string queryComunicado = @"
						SELECT
							{0}
						FROM {1} c
						INNER JOIN comunidado_grupo cg
								on cg.comunicado_id = c.id
							inner join grupo_comunicado g
								on cg.grupo_comunicado_id = g.id
						WHERE (c.excluido = false)
						{2}";

        public RepositorioComunicado(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<PaginacaoResultadoDto<Comunicado>> ListarPaginado(FiltroComunicadoDto filtro, Paginacao paginacao)
        {
            StringBuilder query = new StringBuilder();
            string where = MontaWhereListar(filtro);
            string from;

            if (paginacao.QuantidadeRegistros != 0)
                from = string.Format(fromOffSet, string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", paginacao.QuantidadeRegistrosIgnorados, paginacao.QuantidadeRegistros));
            else from = fromComunicado;
            query.AppendFormat(queryComunicado, Montarcampos(), from, where);

            if (paginacao == null)
                paginacao = new Paginacao(1, 10);

            var retornoPaginado = new PaginacaoResultadoDto<Comunicado>()
            {
                Items = await database.Conexao.QueryAsync<Comunicado, ComunicadoGrupo, GrupoComunicacao, Comunicado>(query.ToString(), (comunicado, g, grupo) =>
                   {
                       comunicado.AdicionarGrupo(grupo);
                       return comunicado;
                   }, new
                   {
                       filtro.DataEnvio,
                       filtro.DataExpiracao,
                       filtro.Titulo,
                       filtro.GruposId
                   },
            splitOn: "id,ComunicadoGrupoId,GrupoId")
            };

            var queryCount = new StringBuilder(string.Format(queryComunicado, "count(distinct c.id)", fromComunicado, where));

            retornoPaginado.TotalRegistros = (await database.Conexao.QueryAsync<int>(queryCount.ToString(), new
            {
                filtro.DataEnvio,
                filtro.DataExpiracao,
                filtro.Titulo,
                filtro.GruposId
            })).Sum();

            retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);

            return retornoPaginado;
        }

        public async Task<IEnumerable<ComunicadoResultadoDto>> ObterPorIdAsync(long id)
        {
            var where = "AND c.id = @id";
            var query = string.Format(queryComunicado, Montarcampos(false), fromComunicado, where);

            return await database.Conexao.QueryAsync<ComunicadoResultadoDto>(query, new { id });
        }

        private static string MontaWhereListar(FiltroComunicadoDto filtro)
        {
            var where = "";
            if (!string.IsNullOrEmpty(filtro.Titulo))
            {
                filtro.Titulo = $"%{filtro.Titulo.ToUpperInvariant()}%";
                where += " AND (upper(f_unaccent(c.titulo)) LIKE @titulo)";
            }
            if (filtro.DataEnvio.HasValue)
                where += " AND (date(c.data_envio) = @DataEnvio)";
            if (filtro.DataExpiracao.HasValue)
                where += " AND (date(c.data_expiracao) = @DataExpiracao)";
            if (filtro.GruposId?.Length > 0)
                where += " AND (g.id = ANY(@gruposId))";
            return where;
        }

        private string Montarcampos(bool EhListagem = true)
        {
            StringBuilder campos = new StringBuilder();
            campos.Append(@"c.id,
							c.titulo,
							c.descricao,
							c.data_envio as DataEnvio,
							c.data_expiracao as DataExpiracao,
							c.criado_em as CriadoEm,
							c.criado_por as CriadoPor,
							c.alterado_em as AlteradoEm,
							c.alterado_por as AlteradoPor,
							c.criado_rf as CriadoRf,
							c.alterado_rf as AlteradoRf,");
            if (!EhListagem)
                campos.Append(@"g.nome as Grupo,
                                g.id as GrupoId");
            else
                campos.Append(@"cg.id AS ComunicadoGrupoId,
                                cg.comunicado_id,
                                cg.grupo_comunicado_id,
                                g.id as GrupoId,
                                g.id,
                                g.nome");
            return campos.ToString();
        }
    }
}