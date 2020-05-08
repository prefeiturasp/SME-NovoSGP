using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultaRecuperacaoParalelaPeriodo
    {
        Task<IEnumerable<RecuperacaoParalelaPeriodoPAPDto>> BuscarListaPeriodos(string turmaId);
    }
}
