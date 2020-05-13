using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterEventosCalendarioProfessorPorMesDiaQuery : IRequest<IEnumerable<Evento>>
    {
        public string UeCodigo { get; set; }
        public long TipoCalendarioId { get; set; }
        public string DreCodigo { get; internal set; }
        public DateTime DataConsulta { get; set; }
    }
}
