using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioParametrosSistema : IRepositorioBase<ParametrosSistema>
    {
        IEnumerable<KeyValuePair<string, string>> ObterChaveEValorPorTipo(TipoParametroSistema tipo);

        string ObterValorPorTipoEAno(TipoParametroSistema tipo, int? ano = null);
    }
}