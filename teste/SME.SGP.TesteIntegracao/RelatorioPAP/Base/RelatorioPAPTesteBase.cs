using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.RelatorioPAP.Base
{
    public class RelatorioPAPTesteBase : TesteBaseComuns
    {
        public RelatorioPAPTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected async Task CriarDadosBase()
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(ObterPerfilPAP());

            await CriarUsuarios();

            await CriarTurmaTipoCalendario();

            await CriarPeriodoEscolar(false);
        }

        protected async Task CriarTurmaTipoCalendario()
        {
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio, false);
            await CriarTurma(Modalidade.Fundamental, "7", false, TipoTurma.Programa);
        }

        protected async Task CriarPeriodoEscolar(bool considerarAnoAnterior = false)
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4, TIPO_CALENDARIO_1, considerarAnoAnterior);
        }
    }
}
