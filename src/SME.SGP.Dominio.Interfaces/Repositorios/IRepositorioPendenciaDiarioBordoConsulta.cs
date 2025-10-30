using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioPendenciaDiarioBordoConsulta
    {
        Task<long> ExisteIdPendenciaDiarioBordo(long aulaId, long componenteCurricularId);
        Task<IEnumerable<PendenciaUsuarioDto>> ObterIdPendenciaDiarioBordoPorAulaId(long aulaId);
        Task<IEnumerable<PendenciaDiarioBordoDescricaoDto>> ObterPendenciasDiarioPorPendencia(long pendenciaId, string codigoRf);
        Task<IEnumerable<long>> ObterIdsPendencias(int anoLetivo, string codigoUE);
        Task<IEnumerable<PendenciaDiarioBordoParaExcluirDto>> ListarPendenciaDiarioBordoParaExcluirPorIdTurma(string turmaId);
    }
}
