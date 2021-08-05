using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDiarioBordoObservacao : RepositorioBase<DiarioBordoObservacao>, IRepositorioDiarioBordoObservacao
    {
        public RepositorioDiarioBordoObservacao(ISgpContext conexao) : base(conexao) { }

        public async Task ExcluirObservacoesPorDiarioBordoId(long diarioBordoId, long? usuarioId)
        {
			var query = @"update diario_bordo_observacao 
							set excluido = true
							, alterado_por = @alteradoPor
							, alterado_rf = @alteradoRF
							, alterado_em = @alteradoEm
						where diario_bordo_id = @diarioBordoId";

			if (usuarioId.HasValue)
				query += " and usuario_id = @usuarioId";

			var parametros = new
			{
				diarioBordoId,
				alteradoPor = database.UsuarioLogadoNomeCompleto,
				alteradoRF = database.UsuarioLogadoRF,
				alteradoEm = DateTimeExtension.HorarioBrasilia()
			};

			await database.Conexao.ExecuteScalarAsync(query, parametros);
		}

		public async Task<IEnumerable<ListarObservacaoDiarioBordoDto>> ListarPorDiarioBordoAsync(long diarioBordoId, long usuarioLogadoId)
        {
            var sql = @"select
							dbo.id,
							observacao,
							(usuario_id = @usuarioLogadoId) as Proprietario,
							count(dbon.*) as QtdUsuariosNotificados,
							criado_em as CriadoEm,
							criado_por as CriadoPor,
							criado_rf as CriadoRf,
							alterado_em as AlteradoEm,
							alterado_por as AlteradoPor,
							alterado_rf as AlteradoRf
						from
							diario_bordo_observacao dbo
						left join diario_bordo_observacao_notificacao dbon on dbo.id = dbon.observacao_id
						where
							diario_bordo_id = @diarioBordoId
							and not excluido 
						group by dbo.id,
							observacao,
							usuario_id,
							criado_em,
							criado_por,
							criado_rf,
							alterado_em,
							alterado_por,
							alterado_rf
                        order by criado_em desc";

            return await database.Conexao.QueryAsync<ListarObservacaoDiarioBordoDto>(sql, new { diarioBordoId, usuarioLogadoId });
        }

        public async Task<Turma> ObterTurmaDiarioBordoAulaPorObservacaoId(long observacaoId)
        {
			const string sql = @"select
									t.*
								from
									diario_bordo_observacao dbob
								inner join
									diario_bordo db
									on dbob.diario_bordo_id = db.id
								inner join
									aula a
									on db.aula_id = a.id
								inner join
									turma t
									on a.turma_id = t.turma_id
								where
									dbob.id = @observacaoId";

			return await database.Conexao.QuerySingleOrDefaultAsync<Turma>(sql, new { observacaoId });
        }
    }
}
