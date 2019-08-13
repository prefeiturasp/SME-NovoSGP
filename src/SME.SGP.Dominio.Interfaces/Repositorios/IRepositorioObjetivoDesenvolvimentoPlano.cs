using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioObjetivoDesenvolvimentoPlano : IRepositorioBase<ObjetivoDesenvolvimentoPlano>
    {
        IEnumerable<ObjetivoDesenvolvimentoPlano> ObterObjetivosDesenvolvimentoPorIdPlano(long idPlano);
    }
}