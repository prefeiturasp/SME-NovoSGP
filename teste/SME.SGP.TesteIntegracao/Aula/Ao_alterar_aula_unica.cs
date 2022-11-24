using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.ServicosFakes;

namespace SME.SGP.TesteIntegracao.AulaUnica
{
    public class Ao_alterar_aula_unica : AulaTeste
    {
        public Ao_alterar_aula_unica(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }


        [Fact]
        public async Task Ja_existe_aula_criada_no_dia_para_o_componente()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2, false);

            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05, RecorrenciaAula.AulaUnica);

            var useCase = ServiceProvider.GetService<IAlterarAulaUseCase>();

            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);

            dto.Id = 2;

            await CriarPeriodoEscolarEAbertura();

            await useCase.Executar(dto).ShouldThrowAsync<NegocioException>();
        }

        [Fact]
        public async Task Nao_e_possivel_alterar_aula_fora_do_periodo()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2, false);

            await CriarPeriodoEscolarEncerrado();

            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05, RecorrenciaAula.AulaUnica);

            var useCase = ServiceProvider.GetService<IAlterarAulaUseCase>();

            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);

            dto.Id = 1;

            await useCase.Executar(dto).ShouldThrowAsync<NegocioException>();
        }

        [Fact]
        public async Task Altera_aula()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2, false);

            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05, RecorrenciaAula.AulaUnica);

            var useCase = ServiceProvider.GetService<IAlterarAulaUseCase>();

            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);

            dto.Id = 1;

            dto.Quantidade = 1;

            await CriarPeriodoEscolarEAbertura();

            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();

            var lista = ObterTodos<Dominio.Aula>();

            lista.ShouldNotBeEmpty();

            lista.FirstOrDefault().Quantidade.ShouldBe(1);
        }

        [Fact]
        public async Task Altera_aula_regente_diferente_do_atual()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2,false);

            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05, RecorrenciaAula.AulaUnica, USUARIO_PROFESSOR_CODIGO_RF_1111111);

            var useCase = ServiceProvider.GetService<IAlterarAulaUseCase>();

            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);

            dto.Id = 1;

            dto.Quantidade = 1;

            dto.EhRegencia = true;

            await CriarPeriodoEscolarEAbertura();

            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();

            var lista = ObterTodos<Dominio.Aula>();

            lista.ShouldNotBeEmpty();

            lista.FirstOrDefault().Quantidade.ShouldBe(1);
        }

        private async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_28_04_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }
    }
}
