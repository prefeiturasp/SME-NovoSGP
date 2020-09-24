using SME.SGP.Infra.Dtos.Relatorios;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IRelatorioPlanoAulaUseCase
    {
        Task<bool> Executar(FiltroRelatorioPlanoAulaDto filtro);
    }
}