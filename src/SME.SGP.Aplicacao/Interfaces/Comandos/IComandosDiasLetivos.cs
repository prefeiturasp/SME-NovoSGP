using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosDiasLetivos
    {
        Task<List<DateTime>> BuscarDiasLetivos(long tipoCalendarioId);

        List<DateTime> ObterDias(IEnumerable<Evento> eventos, List<DateTime> dias, EventoLetivo eventoTipo);

        bool VerificarSeDataLetiva(IEnumerable<Evento> eventos, DateTime data);
    }
}