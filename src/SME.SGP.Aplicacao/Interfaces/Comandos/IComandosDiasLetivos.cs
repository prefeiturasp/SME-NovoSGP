using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IComandosDiasLetivos
    {
        List<DateTime> BuscarDiasLetivos(long tipoCalendarioId);

        DiasLetivosDto CalcularDiasLetivos(FiltroDiasLetivosDTO filtro);

        List<DateTime> ObterDias(IEnumerable<Dominio.Evento> eventos, List<DateTime> dias, Dominio.EventoLetivo eventoTipo);
    }
}