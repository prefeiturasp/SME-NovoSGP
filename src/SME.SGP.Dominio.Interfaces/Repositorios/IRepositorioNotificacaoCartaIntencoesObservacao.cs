using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotificacaoCartaIntencoesObservacao
    {
        Task Salvar(NotificacaoCartaIntencoesObservacao notificacao);
        Task Excluir(NotificacaoCartaIntencoesObservacao notificacao);
        Task<IEnumerable<NotificacaoCartaIntencoesObservacao>> ObterPorCartaIntencoesObservacaoId(long cartaIntencoesObservacaoId);
    }
}
