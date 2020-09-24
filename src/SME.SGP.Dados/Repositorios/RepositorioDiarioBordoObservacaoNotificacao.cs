using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDiarioBordoObservacaoNotificacao : IRepositorioDiarioBordoObservacaoNotificacao
    {
        private readonly ISgpContext database;

        public RepositorioDiarioBordoObservacaoNotificacao(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<DiarioBordoObservacaoNotificacao>> ObterPorDiarioBordoObservacaoId(long diarioBordoObservacaoId)
        {
            var query = "select * from diario_bordo_observacao_notificacao where observacao_id = @diarioBordoObservacaoId";

            return (await database.Conexao.QueryAsync<DiarioBordoObservacaoNotificacao>(query, new { diarioBordoObservacaoId }));
        }

        public async Task<IEnumerable<long>> ObterObservacaoPorId(long diarioBordoId)
        {
            var query = "select id from diario_bordo_observacao where diario_bordo_id = @diarioBordoId";

            return await database.Conexao.QueryAsync<long>(query, new { diarioBordoId });
        }

        public async Task Excluir(DiarioBordoObservacaoNotificacao notificacao)
        {
            await database.Conexao.DeleteAsync(notificacao);
        }

        public async Task Salvar(DiarioBordoObservacaoNotificacao notificacao)
        {
            await database.Conexao.InsertAsync(notificacao);
        }

    }
}
