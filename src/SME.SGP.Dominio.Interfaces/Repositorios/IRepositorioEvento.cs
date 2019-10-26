using System;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEvento : IRepositorioBase<Evento>
    {
        bool ExisteEventoNaDataEspecificada(DateTime dataInicio);
    }
}