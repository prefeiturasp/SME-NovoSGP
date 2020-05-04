using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasCalendarioProfessorPorMesDiaQuery : IRequest<IEnumerable<Aula>>
    {
        public string UeCodigo { get; set; }
        public long TipoCalendarioId { get; set; }
        public string DreCodigo { get; internal set; }
        public DateTime DiaConsulta { get; set; }        
        public string TurmaCodigo { get; set; }
    }
}
