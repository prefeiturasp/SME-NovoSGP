using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public interface IRepositorioMatrizSaberPlano : IRepositorioBase<MatrizSaberPlano>
    {
        MatrizSaberPlano ObterMatrizesSaberDoPlano(IEnumerable<long> matrizesId, long idPlano);
        MatrizSaberPlano ObterPorMatrizSaberPlano(long matrizId, long idPlano);
    }
}