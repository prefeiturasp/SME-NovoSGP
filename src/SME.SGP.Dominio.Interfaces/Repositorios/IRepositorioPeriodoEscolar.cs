using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPeriodoEscolar : IRepositorioBase<PeriodoEscolar>
    {
        IList<PeriodoEscolar> ObterPorTipoCalendario(long codigoTipoCalendario);
    }
}
