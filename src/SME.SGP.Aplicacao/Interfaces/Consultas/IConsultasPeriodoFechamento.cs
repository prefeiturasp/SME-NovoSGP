using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasPeriodoFechamento
    {
        IEnumerable<PeriodoFechamento> GetTeste();
    }
}