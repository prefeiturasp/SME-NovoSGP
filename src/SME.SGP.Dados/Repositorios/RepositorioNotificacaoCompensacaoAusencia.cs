using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioNotificacaoCompensacaoAusencia : IRepositorioNotificacaoCompensacaoAusencia
    {
        private readonly ISgpContext database;

        public RepositorioNotificacaoCompensacaoAusencia(ISgpContext database)
        {
            this.database = database;
        }


        public Task<IEnumerable<NotificacaoCompensacaoAusencia>> ObterPorCompensacao(long compensacaoAusenciaId)
            => database.Conexao.QueryAsync<NotificacaoCompensacaoAusencia>("select * from notificacao_compensacao_ausencia where compensacao_ausencia_id = @compensacaoAusenciaId"
                            , new { compensacaoAusenciaId });

        public async Task Excluir(NotificacaoCompensacaoAusencia notificacaoCompensacaoAusencia)
        {
            await database.Conexao.DeleteAsync(notificacaoCompensacaoAusencia);
        }

        public void Inserir(long notificacaoId, long compensacaoAusenciaId)
        {
            database.Conexao.Insert(new NotificacaoCompensacaoAusencia()
            {
                NotificacaoId = notificacaoId,
                CompensacaoAusenciaId = compensacaoAusenciaId
            });
        }
    }
}
