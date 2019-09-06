using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioObjetivoAprendizagemPlano : IRepositorioBase<ObjetivoAprendizagemPlano>
    {
        IEnumerable<ObjetivoAprendizagemPlano> ObterObjetivosAprendizagemPorIdPlano(long idPlano);
    }
}