using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
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

		private readonly string queryObterIdGrupoPorModalidade = @"select id, etapa_ensino_id from grupo_comunicado
																	where excluido = false and etapa_ensino_id notnull";


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

        public async Task<IEnumerable<GrupoComunicacaoCompletoRespostaDto>> ObterCompletoPorIdAsync(long id)
        {
            var where = "AND gc.id = @id";
            var query = string.Format(queryGrupo, where);

            return await database.Conexao.QueryAsync<GrupoComunicacaoCompletoRespostaDto>(query, new { id });
        }

		public async Task<IEnumerable<GrupoComunicacaoCompletoRespostaDto>> ObterCompletoPorListaId(IEnumerable<long> ids)
		{
			var where = "AND gc.id = Any(@ids)";

			var query = string.Format(queryGrupo, where);

			return await database.Conexao.QueryAsync<GrupoComunicacaoCompletoRespostaDto>(query, new { ids = ids.ToList() });
		}

		public async Task<IEnumerable<long>> ObterIdsGrupoComunicadoPorModalidade(Modalidade modalidade)
		{
			var grupos = await database.Conexao.QueryAsync<GrupoComunicacao>(queryObterIdGrupoPorModalidade);

			if (grupos == null || !grupos.Any())
				throw new NegocioException($"Não foi encontrado grupos com a modalidade {modalidade.ToString()}");

			var modalidadeString = ((int)modalidade).ToString();

			var etapaEnsinoSplit = grupos.Select(x => new
			{
				Id = x.Id,
				Etapas = x.EtapaEnsino.Split(',')
			});

			return etapaEnsinoSplit.Where(x => x.Etapas.Contains(modalidadeString)).Select(x => x.Id);
		}
	}
}