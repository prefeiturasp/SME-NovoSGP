using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class Ao_inserir_frequencia_com_inconsistencias : FrequenciaTesteBase
    {

        public Ao_inserir_frequencia_com_inconsistencias(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Frequência - Deve retornar exceção 'Lista de alunos e o componente devem ser informados'")]
        public async Task Lista_de_alunos_e_o_componente_devem_ser_informados()
        {
            await CriarDadosBasicos(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());

            var useCase = ServiceProvider.GetService<IInserirFrequenciaUseCase>();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(new FrequenciaDto(AULA_ID_1)));

            excecao.Message.ShouldBe(MensagensNegocioFrequencia.Lista_de_alunos_e_o_componente_devem_ser_informados);
        }

        [Fact(DisplayName = "Frequência - Deve retornar exceção 'A aula informada não foi encontrada'")]
        public async Task A_aula_informada_nao_foi_encontrada()
        {
            await CriarDadosBaseSemTurma(ObterPerfilCJ(), ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2);

            var useCase = ServiceProvider.GetService<IInserirFrequenciaUseCase>();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(ObterFrequenciaDto()));

            excecao.Message.ShouldBe(MensagensNegocioFrequencia.A_aula_informada_nao_foi_encontrada);
        }

        [Fact(DisplayName = "Frequência - Deve retornar exceção 'A turma não foi encontrada'")]
        public async Task Turma_informada_nao_foi_encontrada()
        {
            await CriarDadosBaseSemTurma(ObterPerfilProfessor(), ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05, RecorrenciaAula.AulaUnica, NUMERO_AULAS_1);

            var useCase = ServiceProvider.GetService<IInserirFrequenciaUseCase>();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(ObterFrequenciaDto()));

            excecao.Message.ShouldBe(MensagensNegocioFrequencia.Turma_informada_nao_foi_encontrada);
        }

        [Fact(DisplayName = "Frequência - Professor cj nao possui permissão para inserir neste periodo")]
        public async Task Professor_cj_nao_possui_permissão_para_inserir_neste_periodo()
        {
            await CriarDadosBasicos(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            await CriarAtribuicaoEsporadica(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4);

            var useCase = ServiceProvider.GetService<IInserirFrequenciaUseCase>();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(ObterFrequenciaDto()));

            excecao.Message.ShouldBe(MensagensNegocioFrequencia.Nao_possui_permissão_para_inserir_neste_periodo);
        }

        [Fact(DisplayName = "Frequência - Nao e permitido registro de frequencia para este componente")]
        public async Task Nao_e_permitido_registro_de_frequencia_para_este_componente()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_AULA_COMPARTILHADA.ToString());

            var useCase = ServiceProvider.GetService<IInserirFrequenciaUseCase>();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(ObterFrequenciaDto()));

            excecao.Message.ShouldBe(MensagensNegocioFrequencia.Nao_e_permitido_registro_de_frequencia_para_este_componente);
        }

        [Fact(DisplayName = "Frequência - Não e possível registrar a frequência o componente nao permite substituicao")]
        public async Task Nao_e_possível_registrar_a_frequência_o_componente_nao_permite_substituicao()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_AEE_COLABORATIVO.ToString());

            var useCase = ServiceProvider.GetService<IInserirFrequenciaUseCase>();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(ObterFrequenciaDto()));

            excecao.Message.ShouldBe(MensagensNegocioFrequencia.Nao_e_possível_registrar_a_frequência_o_componente_nao_permite_substituicao);
        }

        [Fact(DisplayName = "Frequência - Não pode fazer alteracoes nesta turma componente e data")]
        public async Task Nao_pode_fazer_alteracoes_nesta_turma_componente_e_data()
        {
            collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<VerificaPodePersistirTurmaDisciplinaEOLQuery, bool>),
                    typeof(VerificaPodePersistirTurmaDisciplinaEOLQueryHandlerSemPermissaoFake), ServiceLifetime.Scoped));

            collectionFixture.BuildServiceProvider();

            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());

            var useCase = ServiceProvider.GetService<IInserirFrequenciaUseCase>();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(ObterFrequenciaDto()));

            excecao.Message.ShouldBe(MensagemNegocioComuns.Voce_nao_pode_fazer_alteracoes_ou_inclusoes_nesta_turma_componente_e_data);
        }
    }
}
