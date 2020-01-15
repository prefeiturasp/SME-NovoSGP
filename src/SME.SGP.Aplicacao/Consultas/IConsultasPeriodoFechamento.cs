using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ConsultasPeriodoFechamento : IConsultasPeriodoFechamento
    {
        private readonly IRepositorioPeriodoFechamento repositorioPeriodoFechamento;

        public ConsultasPeriodoFechamento(IRepositorioPeriodoFechamento repositorioPeriodoFechamento)
        {
            this.repositorioPeriodoFechamento = repositorioPeriodoFechamento ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoFechamento));
        }

        public IEnumerable<PeriodoFechamento> GetTeste()
        {
            return repositorioPeriodoFechamento.Listar();
        }
    }
}