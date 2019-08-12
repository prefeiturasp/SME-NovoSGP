using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasCiclo
    {
        IEnumerable<CicloDto> Listar(IEnumerable<int> idsTurmas);
    }
}