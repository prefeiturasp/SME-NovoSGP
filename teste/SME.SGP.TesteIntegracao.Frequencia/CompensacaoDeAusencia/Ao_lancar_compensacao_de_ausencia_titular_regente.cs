using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia
{
    public class Ao_lancar_compensacao_de_ausencia_titular_regente : Ao_lancar_compensacao_de_ausencia_base
    {
        public Ao_lancar_compensacao_de_ausencia_titular_regente(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        //[Fact]
        public async Task Deve_lancar_compensacao_de_ausencia_titular_regente_fundamental()
        {
            var dto = ObtenhaDtoDadoBase(
                            COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(),
                            Modalidade.Fundamental,
                            ModalidadeTipoCalendario.FundamentalMedio,
                            ANO_1);

            //await ExecuteTeste(dto, ObtenhaListaDeRegencia());
            TesteDisciplinasRegentes();

            await Task.CompletedTask;
        }

        //[Fact]
        public async Task Deve_lancar_compensacao_de_ausencia_titular_regente_eja()
        {
            var dto = ObtenhaDtoDadoBase(
                            COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114.ToString(),
                            Modalidade.EJA,
                            ModalidadeTipoCalendario.EJA,
                            ANO_3);

            //await ExecuteTeste(dto, ObtenhaListaDeRegencia());
            TesteDisciplinasRegentes();

            await Task.CompletedTask;
        }
    }
}
