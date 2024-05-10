using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioObjetivoDesenvolvimentoPlano : IRepositorioBase<RecuperacaoParalelaObjetivoDesenvolvimentoPlano>
    {
        IEnumerable<RecuperacaoParalelaObjetivoDesenvolvimentoPlano> ObterObjetivosDesenvolvimentoPorIdPlano(long idPlano);
    }
}