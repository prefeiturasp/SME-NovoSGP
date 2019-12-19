using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotificacaoFrequencia : IRepositorioBase<NotificacaoFrequencia>
    {
        IEnumerable<RegistroFrequenciaFaltanteDto> ObterTurmasSemRegistroDeFrequencia(TipoNotificacaoFrequencia tipoNotificacao);

        bool UsuarioNotificado(long usuarioId, TipoNotificacaoFrequencia tipo);
    }
}