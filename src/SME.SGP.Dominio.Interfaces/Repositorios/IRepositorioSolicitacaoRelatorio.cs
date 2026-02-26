using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioSolicitacaoRelatorio : IRepositorioBase<SolicitacaoRelatorio>
    {
        Task<IEnumerable<SolicitacaoRelatorio>> ObterSolicitacaoRelatorioAsync(TipoRelatorio tipoRelatorio, TipoFormatoRelatorio extensaoRelatorio, string usuarioQueSolicitou, int? intervaloSolicitacao = 1);
        Task<bool> RelatorioJaSolicitadoAsync(string filtros,TipoRelatorio tipoRelatorio,string usuarioQueSolicitou);
    }
}
