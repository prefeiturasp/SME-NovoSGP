using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoPorTurmaPeriodoQuery : IRequest<FechamentoTurma>
    {
        public long PeriodoEscolarId { get; internal set; }
        public long TurmaId { get; set; }

    }
}
