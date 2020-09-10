using System.Collections.Generic;

using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioParametrosSistema : IRepositorioBase<ParametrosSistema>
    {
        Task AtualizarValorPorTipoAsync(TipoParametroSistema tipo, string valor, int? ano = null);

        Task<IEnumerable<KeyValuePair<string, string>>> ObterChaveEValorPorTipo(TipoParametroSistema tipo);

        Task<KeyValuePair<string, string>?> ObterUnicoChaveEValorPorTipo(TipoParametroSistema tipo);

        Task<string> ObterValorPorTipoEAno(TipoParametroSistema tipo, int? ano = null);
    }
}