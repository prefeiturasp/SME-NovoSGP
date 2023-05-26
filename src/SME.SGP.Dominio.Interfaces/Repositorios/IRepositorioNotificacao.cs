using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotificacao : IRepositorioBase<Notificacao>
    {
        Task ExcluirPorIdsAsync(long[] ids);
        Task ExcluirLogicamentePorIdsAsync(long[] ids);
        Task ExcluirPeloSistemaAsync(long[] ids);
        Task<IEnumerable<NotificacaoBasicaDto>> ObterNotificacoesPorRfAsync(string usuarioRf, int limite = 5);
        Task AtualizarMensagemPorWorkFlowAprovacao(long[] ids, string mensagem);

        Task<long[]> ObterIdsAsync(string turmaCodigo, NotificacaoCategoria categoria, NotificacaoTipo tipo, int ano);
    }
}