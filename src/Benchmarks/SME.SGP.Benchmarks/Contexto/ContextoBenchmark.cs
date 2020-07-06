using System;
using System.Collections.Generic;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.Benchmarks
{
    public class ContextoBenchmark : ContextoBase
    {
        public override void AdicionarVariaveis(IDictionary<string, object> variaveis)
        {
            throw new System.NotImplementedException();
        }

        public override IContextoAplicacao AtribuirContexto(IContextoAplicacao contexto)
        {
            throw new Exception("Este tipo de conexto não permite atribuição");
        }
    }
}
