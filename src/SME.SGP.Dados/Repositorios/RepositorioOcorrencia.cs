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
							o.id,
							titulo,
							data_ocorrencia,
							hora_ocorrencia,
							descricao,
							ocorrencia_tipo_id,
							excluido,
							criado_rf,
							criado_em,
							alterado_em,
							alterado_por,
							alterado_rf
							ot.id,
							ot.descricao,
							oa.id,
							oa.codigo_aluno
						from
							ocorrencia o
						inner join ocorrencia_tipo ot on ot.id = o.ocorrencia_tipo_id 
						inner join ocorrencia_aluno oa on oa.ocorrencia_id = o.id
						where
							diario_bordo_id = @diarioBordoId
							and not excluido 
                        order by criado_em desc";

			return await database.Conexao.QueryAsync<Ocorrencia, OcorrenciaTipo, OcorrenciaAluno, Ocorrencia>(sql, (ocorrencia, tipo, aluno) =>
			{
				
			}, new { diarioBordoId }, splitOn: "id, id");
		}
	}
}
