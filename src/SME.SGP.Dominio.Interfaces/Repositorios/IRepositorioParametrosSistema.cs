using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioParametrosSistema : IRepositorioBase<ParametrosSistema>
    {
        Task AtualizarValorPorTipoAsync(TipoParametroSistema tipo, string valor, int? ano = null);
        Task ReplicarParametrosAnoAnteriorAsync(int anoAtual, int anoAnterior);
        Task<IEnumerable<KeyValuePair<string, string>>> ObterChaveEValorPorTipoEAno(TipoParametroSistema tipo, int ano);
        Task<KeyValuePair<string, string>?> ObterUnicoChaveEValorPorTipo(TipoParametroSistema tipo);
        Task<string> ObterValorUnicoPorTipo(TipoParametroSistema tipo);
        Task<T> ObterValorUnicoPorTipo<T>(TipoParametroSistema tipoParametroSistema);
        Task<string> ObterNovosTiposUEPorAno(int anoLetivo);
        Task<string> ObterValorPorTipoEAno(TipoParametroSistema tipo, int? ano = null);
        Task<ParametrosSistema> ObterParametroPorTipoEAno(TipoParametroSistema tipo, int ano = 0);
        Task<IEnumerable<ParametrosSistema>> ObterPorTiposAsync(long[] tipos);
        Task<bool> VerificaSeExisteParametroSistemaPorAno(int ano);
        Task<IEnumerable<ParametrosSistema>> ObterParametrosPorTipoEAno(TipoParametroSistema tipo, int ano);
        Task<string> ObterNovasModalidadesAPartirDoAno(int anoLetivo);
    }
}