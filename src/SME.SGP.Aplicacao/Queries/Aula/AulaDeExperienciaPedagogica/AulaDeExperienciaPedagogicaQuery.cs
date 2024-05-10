using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class AulaDeExperienciaPedagogicaQuery: IRequest<bool>
    {
        public AulaDeExperienciaPedagogicaQuery(long componenteCurricular)
        {
            ComponenteCurricular = componenteCurricular;
        }

        public long ComponenteCurricular { get; set; }
    }
}
