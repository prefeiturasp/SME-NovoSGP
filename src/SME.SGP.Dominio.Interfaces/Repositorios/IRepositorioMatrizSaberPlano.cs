using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioMatrizSaberPlano : IRepositorioBase<MatrizSaberPlano>
    {
        IEnumerable<MatrizSaberPlano> ObterMatrizesPorIdPlano(long idPlano);
    }
}