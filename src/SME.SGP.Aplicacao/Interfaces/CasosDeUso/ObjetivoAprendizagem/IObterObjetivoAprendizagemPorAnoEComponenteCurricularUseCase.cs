using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IListarObjetivoAprendizagemPorAnoTurmaEComponenteCurricularUseCase
    {
        Task<IEnumerable<ObjetivoAprendizagemDto>> Executar(string ano, long componenteCurricularId, bool ensinoEspecial,long turmaId);
    }
}
