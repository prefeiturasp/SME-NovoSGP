using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
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
    public class Ao_alterar_frequencia_pelo_professor_titular : FrequenciaTesteBase
    {
        public Ao_alterar_frequencia_pelo_professor_titular(CollectionFixture collectionFixture) : base(collectionFixture) { }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<VerificaPodePersistirTurmaDisciplinaEOLQuery, bool>), typeof(VerificaPodePersistirTurmaDisciplinaEOLQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }

        //[Fact]
        public async Task Deve_alterar_a_Frequencia()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), false);

            var frequencia = new FrequenciaDto()
            {
                AulaId = AULA_ID_1,
                ListaFrequencia = new List<RegistroFrequenciaAlunoDto>()
                    {
                       new RegistroFrequenciaAlunoDto() {
                           Aulas = new List<FrequenciaAulaDto>() { new FrequenciaAulaDto() { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_COMPARECEU}},
                           CodigoAluno = CODIGO_ALUNO_99999,
                           TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_COMPARECEU
                       },
                    }
            };

            await InserirFrequenciaUseCaseComValidacaoBasica(frequencia);

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
