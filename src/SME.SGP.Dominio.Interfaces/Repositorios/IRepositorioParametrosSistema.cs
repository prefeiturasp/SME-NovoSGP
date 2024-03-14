using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioParametrosSistema : IRepositorioBase<ParametrosSistema>
    {
        Task AtualizarValorPorTipoAsync(TipoParametroSistema tipo, string valor, int? ano = null);
        Task<int> ReplicarParametrosAnoAnteriorAsync(int anoAtual, int anoAnterior);
        Task<int> CriarParametrosPeriodosConfiguracaoRelatorioPeriodicoPAPAnoAtualAsync(int anoAtual);
    }
}