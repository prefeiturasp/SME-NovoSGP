using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class Ao_apresentar_os_alunos_na_tela : FrequenciaBase
    {
        public Ao_apresentar_os_alunos_na_tela(CollectionFixture collectionFixture) : base(collectionFixture) { }

        [Fact]
        public async Task Alunos_novos_devem_aparecer_com_tooltip_durante_15_dias()
        {

        }
    }
}
