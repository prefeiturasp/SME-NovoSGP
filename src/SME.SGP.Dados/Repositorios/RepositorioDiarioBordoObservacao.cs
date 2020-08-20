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

        public async Task<IEnumerable<ListarObservacaoDiarioBordoDto>> ListarPorDiarioBordoAsync(long diarioBordoId, long usuarioLogadoId)
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
							diario_bordo_observacao
						where
							diario_bordo_id = @diarioBordoId
                        order by criado_em desc";

            return await database.Conexao.QueryAsync<ListarObservacaoDiarioBordoDto>(sql, new { diarioBordoId, usuarioLogadoId });
        }
    }
}
