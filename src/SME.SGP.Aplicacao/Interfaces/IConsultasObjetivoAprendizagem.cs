using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasObjetivoAprendizagem
    {
        Task<IEnumerable<ObjetivoAprendizagemDto>> Listar(FiltroObjetivosAprendizagemDto filtroObjetivosAprendizagemDto);
    }
}