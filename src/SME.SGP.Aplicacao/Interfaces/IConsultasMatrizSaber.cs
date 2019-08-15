using System.Collections.Generic;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasMatrizSaber
    {
        IEnumerable<MatrizSaberDto> Listar();
    }
}