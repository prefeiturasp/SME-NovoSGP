using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotificacaoFrequencia : IRepositorioBase<NotificacaoFrequencia>
    {
        Task<IEnumerable<RegistroFrequenciaFaltanteDto>> ObterTurmasSemRegistroDeFrequencia(TipoNotificacaoFrequencia tipoNotificacao);

        bool UsuarioNotificado(long usuarioId, TipoNotificacaoFrequencia tipo);
    }
}