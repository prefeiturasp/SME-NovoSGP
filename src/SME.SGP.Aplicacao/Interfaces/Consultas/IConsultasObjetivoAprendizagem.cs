using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasObjetivoAprendizagem
    {
        bool DisciplinaPossuiObjetivosDeAprendizagem(long codigoDisciplina);

        Task<IEnumerable<ObjetivoAprendizagemDto>> Filtrar(FiltroObjetivosAprendizagemDto filtroObjetivosAprendizagemDto);

        Task<IEnumerable<ObjetivoAprendizagemDto>> Listar();
    }
}