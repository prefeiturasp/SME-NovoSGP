using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PendenciaDevolutiva
{
    public class Ao_gerar_pendencia_devolutiva : TesteBase
    {
        public Ao_gerar_pendencia_devolutiva(CollectionFixture collectionFixture) : base(collectionFixture)
        {

        }

        [Fact]
        public async Task Nao_Deve_Gerar_Pendencia_ComParamentro_GerarPendenciaDevolutivaSemDiarioBordo_Desativado()
        {

        }

        [Fact]
        public async Task Nao_Deve_Gerar_Pendencia_ComParamentro_DataInicioGeracaoPendencias_Desativado()
        {

        }
    }
}
