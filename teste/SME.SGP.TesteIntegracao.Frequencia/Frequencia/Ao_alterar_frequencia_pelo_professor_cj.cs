using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class Ao_alterar_frequencia_pelo_professor_cj : FrequenciaTesteBase
    {
        public Ao_alterar_frequencia_pelo_professor_cj(CollectionFixture collectionFixture) : base(collectionFixture) { }

        [Fact(DisplayName = "Frequência - Deve alterar a Frequencia quando CJ")]
        public async Task Deve_alterar_a_frequencia()
        {
            await CriarDadosBasicos(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), criarPeriodo:false);
            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await InserirFrequenciaUseCaseComValidacaoBasica(ObterFrequenciaDto());

            var frequenciaAlterada = new FrequenciaDto()
            {
                AulaId = AULA_ID_1,
                ListaFrequencia = new List<RegistroFrequenciaAlunoDto>()
                    {
                       new RegistroFrequenciaAlunoDto() {
                           Aulas = new List<FrequenciaAulaDto>() { new FrequenciaAulaDto() { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_REMOTO}},
                           CodigoAluno = CODIGO_ALUNO_99999,
                           TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_COMPARECEU
                       },
                    }
            };

            await InserirFrequenciaUseCaseComValidacaoBasica(frequenciaAlterada);

        }
    }
}
