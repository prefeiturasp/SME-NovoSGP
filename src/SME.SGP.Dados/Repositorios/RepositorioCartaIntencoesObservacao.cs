using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCartaIntencoesObservacao : RepositorioBase<CartaIntencoesObservacao>, IRepositorioCartaIntencoesObservacao
    {
		public RepositorioCartaIntencoesObservacao(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
		{ }

		public async Task<IEnumerable<CartaIntencoesObservacaoDto>> ListarPorTurmaEComponenteCurricularAsync(long turmaId, long componenteCurricularId, long usuarioLogadoId)
        {
            var sql = @"select
							id,
							observacao,
							(usuario_id = @usuarioLogadoId) as Proprietario,
							criado_em as CriadoEm,
							criado_por as CriadoPor,
							criado_rf as CriadoRf,
							alterado_em as AlteradoEm,
							alterado_por as AlteradoPor,
							alterado_rf as AlteradoRf
						from
							carta_intencoes_observacao
						where
							turma_id = @turmaId and
							componente_curricular_id = @componenteCurricularId and					
							not excluido 
                        order by criado_em desc";

            return await database.Conexao.QueryAsync<CartaIntencoesObservacaoDto>(sql, new { turmaId, componenteCurricularId, usuarioLogadoId });
        }

        public async Task<CartaIntencoesObservacaoDto> ObterCartaIntencoesObservacaoPorObservacaoId(long observacaoId)
        {
            const string sql = @" select dbob.Observacao, dbob.diario_bordo_id as DiarioBordoId, u.rf_codigo as UsuarioCodigoRfDiarioBordo, u.nome as UsuarioNomeDiarioBordo
								  from diario_bordo_observacao dbob
								  inner join diario_bordo db on dbob.diario_bordo_id = db.id
								  inner join usuario u on u.rf_codigo = db.criado_rf 	
								where dbob.id = @observacaoId";

            return await database.Conexao.QuerySingleOrDefaultAsync<CartaIntencoesObservacaoDto>(sql, new { observacaoId });
        }
    }
}
