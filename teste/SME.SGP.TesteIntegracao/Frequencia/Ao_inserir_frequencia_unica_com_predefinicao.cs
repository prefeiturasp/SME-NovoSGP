using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class Ao_inserir_frequencia_unica_com_predefinicao : FrequenciaTesteBase
    {
        public Ao_inserir_frequencia_unica_com_predefinicao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<VerificaPodePersistirTurmaDisciplinaEOLQuery, bool>), typeof(VerificaPodePersistirTurmaDisciplinaEOLQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Frequência - Deve permitir inserir frequencia com ausencia com predefinicao compareceu")]
        public async Task Deve_permitir_inserir_frequencia_com_ausencia_com_predefinicao_compareceu()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), false, TIPO_CALENDARIO_1, true, NUMERO_AULAS_1);

            await CriarPredefinicaoAluno(CODIGO_ALUNO_99999, TipoFrequencia.C, COMPONENTE_CURRICULAR_PORTUGUES_ID_138,TURMA_ID_1);

            var frequencia = new FrequenciaDto()
            {
                AulaId = AULA_ID_1,
                ListaFrequencia = new List<RegistroFrequenciaAlunoDto>()
                {
                    new RegistroFrequenciaAlunoDto() {
                        Aulas = new List<FrequenciaAulaDto>() { new FrequenciaAulaDto() { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_FALTOU}},
                        CodigoAluno = CODIGO_ALUNO_99999,
                        TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_COMPARECEU
                    },
                }
            };

            await InserirFrequenciaUseCaseComValidacaoCompleta(frequencia, TipoFrequencia.C, TipoFrequencia.F, PERCENTUAL_ZERO, NUMERO_AULAS_1, QTDE_1, ZERO);
        }
        
        [Fact(DisplayName = "Frequência - Deve permitir inserir frequencia com remoto com predefinicao compareceu modificando pre definicao para remoto")]
        public async Task Deve_permitir_inserir_frequencia_com_remoto_com_predefinicao_compareceu_modificando_pre_definicao_para_remoto()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), false, TIPO_CALENDARIO_1, true, NUMERO_AULAS_1);

            await CriarPredefinicaoAluno(CODIGO_ALUNO_99999, TipoFrequencia.C, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TURMA_ID_1);

            var frequencia = new FrequenciaDto()
            {
                AulaId = AULA_ID_1,
                ListaFrequencia = new List<RegistroFrequenciaAlunoDto>()
                {
                    new RegistroFrequenciaAlunoDto() {
                        Aulas = new List<FrequenciaAulaDto>() { new FrequenciaAulaDto() { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_REMOTO}},
                        CodigoAluno = CODIGO_ALUNO_99999,
                        TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_REMOTO
                    },
                }
            };

            await InserirFrequenciaUseCaseComValidacaoCompleta(frequencia, TipoFrequencia.R, TipoFrequencia.R, PERCENTUAL_100, NUMERO_AULAS_1, ZERO, ZERO);
        }

        [Fact(DisplayName = "Frequência - Deve permitir inserir frequencia com compareceu com predefinicao remoto")]
        public async Task Deve_permitir_inserir_frequencia_com_compareceu_com_predefinicao_remoto()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), false, TIPO_CALENDARIO_1, true, NUMERO_AULAS_1);

            await CriarPredefinicaoAluno(CODIGO_ALUNO_99999, TipoFrequencia.R, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TURMA_ID_1);

            var frequencia = new FrequenciaDto()
            {
                AulaId = AULA_ID_1,
                ListaFrequencia = new List<RegistroFrequenciaAlunoDto>()
                {
                    new RegistroFrequenciaAlunoDto() {
                        Aulas = new List<FrequenciaAulaDto>() { new FrequenciaAulaDto() { NumeroAula = NUMERO_AULAS_1, TipoFrequencia = TIPO_FREQUENCIA_COMPARECEU}},
                        CodigoAluno = CODIGO_ALUNO_99999,
                        TipoFrequenciaPreDefinido = TIPO_FREQUENCIA_REMOTO
                    },
                }
            };

            await InserirFrequenciaUseCaseComValidacaoCompleta(frequencia, TipoFrequencia.R, TipoFrequencia.C, PERCENTUAL_100, NUMERO_AULAS_1, ZERO, ZERO);
        }
    }
}
