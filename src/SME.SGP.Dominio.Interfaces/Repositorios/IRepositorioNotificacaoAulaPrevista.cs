using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotificacaoAulaPrevista : IRepositorioBase<NotificacaoAulaPrevista>
    {
        bool UsuarioNotificado(long usuarioId, int bimestre, string turmaId, string disciplinaId);

        IEnumerable<RegistroAulaPrevistaDivergenteDto> ObterTurmasAulasPrevistasDivergentes(int limiteDias);
    }
}
