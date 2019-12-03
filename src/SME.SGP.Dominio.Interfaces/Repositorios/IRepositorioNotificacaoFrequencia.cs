using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotificacaoFrequencia : IRepositorioBase<NotificacaoFrequencia>
    {
        bool UsuarioNotificado(long usuarioId, TipoNotificacaoFrequencia tipo);
    }
}
