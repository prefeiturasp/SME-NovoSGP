using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioObjetivoAprendizagemConsulta
    {
        Task<IEnumerable<ObjetivoAprendizagemDto>> ObterPorAnoEComponenteCurricularJuremaIds(AnoTurma? ano, long[] juremaIds);
    }
}