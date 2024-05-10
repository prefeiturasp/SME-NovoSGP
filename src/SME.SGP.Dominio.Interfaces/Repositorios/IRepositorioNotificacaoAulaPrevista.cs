using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotificacaoAulaPrevista : IRepositorioBase<NotificacaoAulaPrevista>
    {
        Task<bool> UsuarioNotificado(long usuarioId, int bimestre, string turmaId, string disciplinaId);

        Task<IEnumerable<RegistroAulaPrevistaDivergenteDto>> ObterTurmasAulasPrevistasDivergentes(int limiteDias);
    }
}
