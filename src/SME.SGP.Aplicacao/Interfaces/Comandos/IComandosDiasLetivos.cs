using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosDiasLetivos
    {
        Task<List<DateTime>> BuscarDiasLetivos(long tipoCalendarioId);

        Task<DiasLetivosDto> CalcularDiasLetivos(FiltroDiasLetivosDTO filtro);

        List<DateTime> ObterDias(IEnumerable<Evento> eventos, List<DateTime> dias, EventoLetivo eventoTipo);

        bool VerificarSeDataLetiva(IEnumerable<Evento> eventos, DateTime data);
    }
}