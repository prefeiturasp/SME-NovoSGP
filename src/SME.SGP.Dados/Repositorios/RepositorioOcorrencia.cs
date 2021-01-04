using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioOcorrencia : RepositorioBase<Ocorrencia>, IRepositorioOcorrencia
    {
        public RepositorioOcorrencia(ISgpContext conexao) : base(conexao) { }

		public async Task<IEnumerable<Ocorrencia>> Listar(long diarioBordoId, long usuarioLogadoId)
		{
			var sql = @"select
							id,
							data_ocorrencia,
							descricao,
							excluido,
							hora_ocorrencia,
							criado_rf as CriadoRf,
							alterado_em as AlteradoEm,
							alterado_por as AlteradoPor,
							alterado_rf as AlteradoRf
						from
							diario_bordo_observacao
						where
							diario_bordo_id = @diarioBordoId
							and not excluido 
                        order by criado_em desc";

			return await database.Conexao.QueryAsync<Ocorrencia>(sql, new { diarioBordoId, usuarioLogadoId });
		}
	}
}
