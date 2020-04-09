using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
	public class RepositorioGrupoComunicacao : RepositorioBase<GrupoComunicacao>, IRepositorioGrupoComunicacao
    {
        private readonly string queryGrupo = @"
						SELECT
							gc.id,
							gc.nome,
							gc.criado_em as CriadoEm,
							gc.criado_por as CriadoPor,
							gc.alterado_em as AlteradoEm,
							gc.alterado_por as AlteradoPor,
							gc.criado_rf as CriadoRf,
							gc.alterado_rf as AlteradoRf,
							c.id as IdCicloEnsino,
							c.cod_ciclo_ensino_eol as CodCicloEnsino,
							c.descricao as CicloEnsino,
							null as IdTipoEscola,
							null as CodTipoEscola,
							null as TipoEscola
						FROM   grupo_comunicado gc,
							unnest(string_to_array(gc.tipo_ciclo_id, ',')) ciclo(cod_ciclo)
							left join ciclo_ensino c on cast(cod_ciclo as int8) = c.cod_ciclo_ensino_eol
						WHERE (gc.excluido = false)
						{0}
						UNION
						SELECT
							gc.id,
							gc.nome,
							gc.criado_em as CriadoEm,
							gc.criado_por as CriadoPor,
							gc.alterado_em as AlteradoEm,
							gc.alterado_por as AlteradoPor,
							gc.criado_rf as CriadoRf,
							gc.alterado_rf as AlteradoRf,
							null as IdCicloEnsino,
							null as CodCicloEnino,
							null as CicloEnsino,
							t.id as IdTipoEscola,
							t.cod_tipo_escola_eol as CodTipoEscola,
							t.descricao as TipoEscola
						FROM   grupo_comunicado gc,
							unnest(string_to_array(gc.tipo_escola_id, ',')) te(tipo_escola)
							left join tipo_escola t on cast(te.tipo_escola as int) = t.cod_tipo_escola_eol
						WHERE (gc.excluido = false)
						{0}";

        public RepositorioGrupoComunicacao(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<GrupoComunicacaoCompletoRespostaDto>> Listar(FiltroGrupoComunicacaoDto filtro)
        {
            var where = string.Empty;
            if (!string.IsNullOrEmpty(filtro.Nome))
                where += "AND upper(f_unaccent(gc.nome)) LIKE @nome ";
            var query = string.Format(queryGrupo, where);

            return await database.Conexao.QueryAsync<GrupoComunicacaoCompletoRespostaDto>(query, new { filtro.Nome });
        }

        public async Task<IEnumerable<GrupoComunicacaoCompletoRespostaDto>> ObterPorIdAsync(long id)
        {
            var where = "AND gc.id = @id";
            var query = string.Format(queryGrupo, where);

            return await database.Conexao.QueryAsync<GrupoComunicacaoCompletoRespostaDto>(query, new { id });
        }
    }
}