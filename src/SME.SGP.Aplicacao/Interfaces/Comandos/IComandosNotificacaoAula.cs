using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosNotificacaoAula
    {
        Task Inserir(long notificacaoId, long aulaId);
        Task Excluir(long aulaId);
    }
}
