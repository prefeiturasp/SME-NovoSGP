using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class AulaDeExperienciaPedagogicaQuery: IRequest<bool>
    {
        public AulaDeExperienciaPedagogicaQuery(string componenteCurricular)
        {
            ComponenteCurricular = componenteCurricular;
        }

        public string ComponenteCurricular { get; set; }
    }
}
