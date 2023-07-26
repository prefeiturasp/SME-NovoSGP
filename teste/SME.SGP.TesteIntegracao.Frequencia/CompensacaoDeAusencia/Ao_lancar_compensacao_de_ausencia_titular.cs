using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia
{
    public class Ao_lancar_compensacao_de_ausencia_titular : Ao_lancar_compensacao_de_ausencia_base
    {
        public Ao_lancar_compensacao_de_ausencia_titular(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        //[Fact]
        public async Task Deve_lancamento_compensacao_ausencia_titular_fundamental()
        {
            var dto = ObtenhaDtoDadoBase(
                        COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                        Modalidade.Fundamental,
                        ModalidadeTipoCalendario.FundamentalMedio,
                        ANO_7);

            //await ExecuteTeste(dto);
        }

        //[Fact]
        public async Task Deve_lancamento_compensacao_ausencia_titular_medio()
        {
            var dto = ObtenhaDtoDadoBase(
                        COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                        Modalidade.Medio,
                        ModalidadeTipoCalendario.FundamentalMedio,
                        ANO_1);

            //await ExecuteTeste(dto);
        }

        //[Fact]
        public async Task Deve_lancamento_compensacao_ausencia_titular_eja()
        {
            var dto = ObtenhaDtoDadoBase(
                        COMPONENTE_GEOGRAFIA_ID_8,
                        Modalidade.EJA,
                        ModalidadeTipoCalendario.EJA,
                        ANO_3);

            //await ExecuteTeste(dto);
        }
    }
}
