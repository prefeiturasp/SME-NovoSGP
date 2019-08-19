using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasObjetivoAprendizagem
    {
        IEnumerable<ObjetivoAprendizagemDto> Listar(FiltroObjetivosAprendizagemDto filtroObjetivosAprendizagemDto);
    }
}