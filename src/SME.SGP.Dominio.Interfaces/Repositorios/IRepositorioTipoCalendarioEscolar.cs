
using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTipoCalendarioEscolar : IRepositorioBase<TipoCalendarioEscolar>
    {
        IEnumerable<TipoCalendarioEscolarDto> ObterTiposCalendarioEscolar();
        bool VerificarRegistroExistente(int id, string nome);
        TipoCalendarioEscolar ObterUmPorId(long id);
    }
}
