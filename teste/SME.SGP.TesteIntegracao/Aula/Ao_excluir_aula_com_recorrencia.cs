using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AulaRecorrencia
{
    public class Ao_excluir_aula_com_recorrencia : AulaTeste
    {
        public Ao_excluir_aula_com_recorrencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFakePortugues), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Excluir_aula_com_recorrencia_no_bimestre()
        {
            await Excluir_aula_com_regencia(RecorrenciaAula.RepetirBimestreAtual);
        }

        [Fact]
        public async Task Excluir_aula_com_recorrencia_em_todos_os_bimestres()
        {
            await Excluir_aula_com_regencia(RecorrenciaAula.RepetirTodosBimestres);
        }

        [Fact]
        public async Task Aula_com_avaliacao_vinculada()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false);
            
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05, RecorrenciaAula.RepetirBimestreAtual);
            
            await CriaAulaRecorrentePortugues(RecorrenciaAula.RepetirBimestreAtual);
            
            await CriarAtividadeAvaliativaFundamental(DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());

            await CriarPeriodoEscolarEAbertura();

            var useCase = ServiceProvider.GetService<IExcluirAulaUseCase>();
            
            var dto = ObterExcluirAulaDto(RecorrenciaAula.RepetirBimestreAtual);
            
            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();

            var lista = ObterTodos<Notificacao>();

            lista.ShouldNotBeEmpty();
            
            lista.FirstOrDefault().Mensagem.ShouldContain("Aula com avaliação vinculada. Para excluir esta aula primeiro deverá ser excluída a avaliação.");
        }

        protected async Task Excluir_aula_com_regencia(RecorrenciaAula recorrencia)
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false);

            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05, recorrencia);
            
            await CriaAulaRecorrentePortugues(recorrencia);

            await CriarPeriodoEscolarEAbertura();

            var useCase = ServiceProvider.GetService<IExcluirAulaUseCase>();
            
            var dto = ObterExcluirAulaDto(recorrencia);

            await CriarPeriodoEscolarEAbertura();

            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();

            var lista = ObterTodos<Dominio.Aula>();

            lista.ShouldNotBeEmpty();

            lista.FirstOrDefault().Excluido.ShouldBe(true);
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
