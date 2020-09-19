using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanejamentoAnualObjetivosAprendizagem : IRepositorioBase<PlanejamentoAnualObjetivoAprendizagem>
    {
        void SalvarVarios(IEnumerable<PlanejamentoAnualObjetivoAprendizagem> objetivos);
    }
}
