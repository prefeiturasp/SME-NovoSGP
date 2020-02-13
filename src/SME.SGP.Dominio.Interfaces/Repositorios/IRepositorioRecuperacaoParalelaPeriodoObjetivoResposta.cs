using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRecuperacaoParalelaPeriodoObjetivoResposta : IRepositorioBase<RecuperacaoParalelaPeriodoObjetivoResposta>
    {
        Task Excluir(long RecuperacaoParalelaId, long PeriodoId);
    }
}