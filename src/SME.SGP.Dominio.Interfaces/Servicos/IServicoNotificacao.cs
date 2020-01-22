using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoNotificacao
    {
        void GeraNovoCodigo(Notificacao notificacao);

        long ObtemNovoCodigo();

        void Salvar(Notificacao notificacao);

        IEnumerable<(Cargo? Cargo, string Id)> ObterFuncionariosPorNivel(string codigoUe, Cargo? cargo, bool primeiroNivel = true);

        Cargo? ObterProximoNivel(Cargo? cargo, bool primeiroNivel);
    }
}