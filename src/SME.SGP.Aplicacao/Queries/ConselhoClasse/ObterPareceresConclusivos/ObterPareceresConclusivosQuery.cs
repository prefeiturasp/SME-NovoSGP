using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPareceresConclusivosQuery : IRequest<IEnumerable<ConselhoClasseParecerConclusivoDto>>
    {
        public ObterPareceresConclusivosQuery(DateTime dataVigencia)
        {
            DataVigente = dataVigencia;
        }

        public ObterPareceresConclusivosQuery() : this(DateTime.Now)
        {
        }

        public DateTime DataVigente { get; set; }
    }
}
