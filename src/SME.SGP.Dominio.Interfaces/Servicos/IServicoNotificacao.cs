using System.Threading.Tasks;

using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoNotificacao
    {
        Task ExcluirFisicamenteAsync(long[] ids);

        void GeraNovoCodigo(Notificacao notificacao);

        Task<long> ObtemNovoCodigoAsync();

        Task Salvar(Notificacao notificacao);

        IEnumerable<(Cargo? Cargo, string Id)> ObterFuncionariosPorNivel(string codigoUe, Cargo? cargo, bool primeiroNivel = true, bool? notificacaoExigeAcao = false);
        Task<IEnumerable<(Cargo? Cargo, string Id)>> ObterFuncionariosPorNivelAsync(string codigoUe, Cargo? cargo, bool primeiroNivel = true, bool? notificacaoExigeAcao = false);

        Cargo? ObterProximoNivel(Cargo? cargo, bool primeiroNivel);

        Task<Notificacao> ObterPorCodigo(long codigo);        

        Task ExcluirPeloSistemaAsync(long[] ids);

    }
}