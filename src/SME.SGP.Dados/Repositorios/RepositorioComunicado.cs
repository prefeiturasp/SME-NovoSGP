using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
	public class RepositorioComunicado : RepositorioBase<Comunicado>, IRepositorioComunicado
    {
        private readonly string queryComunicado = @"
						SELECT
							c.id,
							c.titulo,
							c.descricao,
							c.data_envio as DataEnvio,
							c.data_expiracao as DataExpiracao,
							c.criado_em as CriadoEm,
							c.criado_por as CriadoPor,
							c.alterado_em as AlteradoEm,
							c.alterado_por as AlteradoPor,
							c.criado_rf as CriadoRf,
							c.alterado_rf as AlteradoRf,
							g.id  as GrupoId,
							g.nome as Grupo
						FROM comunicado c
						INNER JOIN comunidado_grupo cg
							on cg.comunicado_id = c.id
						inner join grupo_comunicado g on cg.grupo_comunicado_id = g.id
						{0}";

        public RepositorioComunicado(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<ComunicadoResultadoDto>> ObterPorIdAsync(long id)
        {
            var where = "AND c.id = @id";
            var query = string.Format(queryComunicado, where);

            return await database.Conexao.QueryAsync<ComunicadoResultadoDto>(query, new { id });
        }
    }
}