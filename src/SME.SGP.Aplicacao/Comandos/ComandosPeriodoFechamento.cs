using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ComandosPeriodoFechamento : IComandosPeriodoFechamento
    {
        private readonly IServicoPeriodoFechamento servicoPeriodoFechamento;

        public ComandosPeriodoFechamento(IServicoPeriodoFechamento servicoPeriodoFechamento)
        {
            this.servicoPeriodoFechamento = servicoPeriodoFechamento ?? throw new System.ArgumentNullException(nameof(servicoPeriodoFechamento));
        }
    }
}