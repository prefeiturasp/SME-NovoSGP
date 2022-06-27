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
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAulaUnica
{
    public class Ao_registrar_aula_unica : AulaTeste
    {
        private DateTime dataInicio = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        private DateTime dataFim = new(DateTimeExtension.HorarioBrasilia().Year, 07, 08);

        public Ao_registrar_aula_unica(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Ao_registrar_aula_unica_professor_especialista()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);

            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio);
        }

        [Fact]
        public async Task Ao_registrar_aula_unica_regente_professor_EJA()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.EJA, ModalidadeTipoCalendario.EJA, dataInicio, dataFim, 1);

            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1113, dataInicio, true);
        }

        [Fact]
        public async Task Ao_registrar_aula_unica_regente_professor_educacao_infantil()
        {
            await CriarDadosBasicosAula(Perfis.PERFIL_PROFESSOR_INFANTIL.ToString(), Modalidade.EducacaoInfantil, ModalidadeTipoCalendario.Infantil, dataInicio, dataFim, BIMESTRE_2);

            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213, dataInicio, true);
        }

        [Fact]
        public async Task Ao_registrar_aula_unica_professor_CP()
        {
            await CriarDadosBasicosAula(Perfis.PERFIL_CP.ToString(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);

            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio);
        }

        [Fact]
        public async Task Ao_registrar_aula_com_evento_letivo()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);
            await CriarEvento(EventoLetivo.Sim, dataInicio, dataInicio);

            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio);
        }

        [Fact]
        public async Task Nao_e_possivel_cadastrar_aula_com_periodo_encerrado()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2, false);
            await CriarPeriodoEscolarEncerrado();

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio);

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Não é possível cadastrar aula do tipo 'Normal' para o dia selecionado!");
        }

        [Fact]
        public async Task Nao_e_possivel_cadastrar_aula_no_domingo()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio);
            dto.DataAula = new(DateTimeExtension.HorarioBrasilia().Year, 05, 08);

            await CriarPeriodoEscolarEAbertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Não é possível cadastrar aula no final de semana");
        }

        private async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_1, DATA_FIM_BIMESTRE_1, BIMESTRE_1);
            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_2, DATA_FIM_BIMESTRE_2, BIMESTRE_2);
            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_3, DATA_FIM_BIMESTRE_3, BIMESTRE_3);
            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_4, DATA_FIM_BIMESTRE_4, BIMESTRE_4);
            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }
    }
}
