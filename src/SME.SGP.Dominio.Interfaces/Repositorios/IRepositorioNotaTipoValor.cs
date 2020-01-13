using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Interfaces
{
    public  interface IRepositorioNotaTipoValor : IRepositorioBase<NotaTipoValor>
    {
        NotaTipoValor ObterPorCicloIdDataAvalicacao(long cicloId, DateTime dataAvalicao);
    }
}
