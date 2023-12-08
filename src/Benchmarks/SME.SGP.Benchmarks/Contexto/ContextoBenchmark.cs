using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;

namespace SME.SGP.Benchmarks
{
    public class ContextoBenchmark : ContextoBase
    {
        public override void AdicionarVariaveis(IDictionary<string, object> variaveis)
        {
            throw new NotImplementedException();
        }

        public override IContextoAplicacao AtribuirContexto(IContextoAplicacao contexto)
        {
            throw new NotImplementedException("Este tipo de conexto não permite atribuição");
        }
    }
}
