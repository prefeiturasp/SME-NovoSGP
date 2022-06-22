using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Avaliacao
{
    public abstract class TesteAvaliacao : TesteBaseComuns
    {
        protected TesteAvaliacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
    }
}
