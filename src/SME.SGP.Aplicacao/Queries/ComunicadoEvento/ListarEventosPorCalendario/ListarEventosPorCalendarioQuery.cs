using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{ 
    public class ListarEventosPorCalendarioQuery : IRequest<IEnumerable<ListarEventosPorCalendarioRetornoDto>>
    {
        public ListarEventosPorCalendarioQuery(long tipoCalendario,
                                               string codigoDre,
                                               string codigoUe,
                                               int? modalidade)
        {
            TipoCalendario = tipoCalendario;
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
            Modalidade = modalidade;
        }

        public long TipoCalendario { get; }
        public string CodigoDre { get; }
        public string CodigoUe { get; }
        public int? Modalidade { get; }
    }
}
