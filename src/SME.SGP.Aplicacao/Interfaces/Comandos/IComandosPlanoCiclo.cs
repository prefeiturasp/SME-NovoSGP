using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IComandosPlanoCiclo
    {
        void Salvar(PlanoCicloDto planoCicloDto);
    }
}