using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia
{
    public class Ao_lancar_compensacao_de_ausencia_cp_e_cj : Ao_lancar_compensacao_de_ausencia_base
    {
        
        public Ao_lancar_compensacao_de_ausencia_cp_e_cj(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        //bulk insert
        //[Fact]
        public async Task Deve_lancar_ausencia_para_cp()
        {
            var dto = ObtenhaDtoDadoBase(ObterPerfilCP(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());

            //await ExecuteTeste(dto);
        }

        //[Fact]
        public async Task Deve_lancar_ausencia_para_diretor()
        {
            var dto = ObtenhaDtoDadoBase(ObterPerfilDiretor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());

            //await ExecuteTeste(dto);
        }

        //[Fact]
        public async Task Deve_lancar_ausencia_para_cj()
        {
            var dto = ObtenhaDtoDadoBase(ObterPerfilCJ(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());

            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            //await ExecuteTeste(dto);
        }

        //[Fact]
        public async Task Deve_lancar_ausencia_para_diretor_regencia_de_classe()
        {
            var dto = ObtenhaDtoDadoBase(ObterPerfilDiretor(), COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString());

            //await ExecuteTeste(dto, ObtenhaListaDeRegencia());
            TesteDisciplinasRegentes();
        }

        //[Fact]
        public async Task Deve_lancar_ausencia_para_cp_regencia_de_classe()
        {
            var dto = ObtenhaDtoDadoBase(ObterPerfilCP(), COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString());

            //await ExecuteTeste(dto, ObtenhaListaDeRegencia());
            TesteDisciplinasRegentes();
        }

        private CompensacaoDeAusenciaDBDto ObtenhaDtoDadoBase(string perfil, string componente)
        {
            return new CompensacaoDeAusenciaDBDto()
            {
                Perfil = perfil,
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = componente,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                AnoTurma = ANO_5,
                DataReferencia = DATA_03_01_INICIO_BIMESTRE_1,
                QuantidadeAula = QUANTIDADE_AULA_4
            };
        }
    }
}
