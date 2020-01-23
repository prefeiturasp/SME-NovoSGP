using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ComandosPeriodoFechamento : IComandosPeriodoFechamento
    {
        private readonly IServicoFechamento servicoPeriodoFechamento;

        public ComandosPeriodoFechamento(IServicoFechamento servicoPeriodoFechamento)
        {
            this.servicoPeriodoFechamento = servicoPeriodoFechamento ?? throw new System.ArgumentNullException(nameof(servicoPeriodoFechamento));
        }
    }
}