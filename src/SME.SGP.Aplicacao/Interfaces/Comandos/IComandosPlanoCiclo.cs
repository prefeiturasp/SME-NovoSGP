using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosPlanoCiclo
    {
        Task Salvar(PlanoCicloDto planoCicloDto);
    }
}