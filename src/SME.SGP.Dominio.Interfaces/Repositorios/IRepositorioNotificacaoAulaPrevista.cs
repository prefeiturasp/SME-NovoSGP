using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotificacaoAulaPrevista : IRepositorioBase<NotificacaoAulaPrevista>
    {
        bool UsuarioNotificado(long usuarioId, int bimestre, string turmaId, string disciplinaId);

        IEnumerable<RegistroAulaPrevistaDivergenteDto> ObterTurmasAulasPrevistasDivergentes(int limiteDias);
    }
}
