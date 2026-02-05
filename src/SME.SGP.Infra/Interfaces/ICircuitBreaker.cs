using SME.SGP.Infra.Enumerados;

namespace SME.SGP.Infra.Interfaces
{
    public interface ICircuitBreaker
    {
        bool PodeExecutar();
        void RegistrarSucesso();
        void RegistrarFalha();
        EstadoCircuito EstadoAtual { get; }
    }
}
