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
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AulaUnica
{
    public class Ao_registrar_aula_unica : AulaTeste
    {
        public Ao_registrar_aula_unica(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesDoProfessorNaTurmaQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesDoProfessorNaTurmaQueryHandlerAulaFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Ao_registrar_aula_unica_professor_especialista()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, false);

            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);
        }

        [Fact]
        public async Task Ao_registrar_aula_unica_regente_professor_EJA()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.EJA, ModalidadeTipoCalendario.EJA, DATA_02_05, DATA_07_08, BIMESTRE_2, false);

            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1113, DATA_02_05, true);
        }

        [Fact]
        public async Task Ao_registrar_aula_unica_regente_professor_educacao_infantil()
        {
            await CriarDadosBasicosAula(Perfis.PERFIL_PROFESSOR_INFANTIL.ToString(), Modalidade.EducacaoInfantil, ModalidadeTipoCalendario.Infantil, DATA_02_05, DATA_07_08, BIMESTRE_2, false);

            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213, DATA_02_05, true);
        }

        [Fact]
        public async Task Ao_registrar_aula_unica_professor_CP()
        {
            await CriarDadosBasicosAula(Perfis.PERFIL_CP.ToString(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, false);

            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);
        }

        [Fact]
        public async Task Ao_registrar_aula_com_evento_letivo()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, false);

            await CriarEvento(EventoLetivo.Sim, DATA_02_05, DATA_02_05);

            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);
        }

        [Fact]
        public async Task Nao_e_possivel_cadastrar_aula_com_periodo_encerrado()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, false);

            await CriarPeriodoEscolarEncerrado();

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();

            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Não é possível cadastrar aula do tipo 'Normal' para o dia selecionado!");
        }

        [Fact]
        public async Task Nao_e_possivel_cadastrar_aula_no_domingo()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, false);

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();

            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);

            dto.DataAula = new(DateTimeExtension.HorarioBrasilia().Year, 05, 01);
            dto.DataAula = dto.DataAula.AddDays(7 - (int) dto.DataAula.DayOfWeek);

            await CriarPeriodoEscolarEAbertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Não é possível cadastrar aula no final de semana");
        }

        [Fact]
        public async Task Registrar_aula_com_evento_liberacao_e_evento_letivo_no_domingo()
        {
            var dataDomingo = DateTimeExtension.ObterDomingo(DATA_02_05);
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataDomingo, DATA_07_08, BIMESTRE_2);
            await CriaEventoNoDomingo(dataDomingo);
            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataDomingo);
        }

        private async Task CriaEventoNoDomingo(DateTime data)
        {
            await CriarEventoTipoResumido(REPOSICAO_AULA,
                              EventoLocalOcorrencia.UE,
                              true,
                              EventoTipoData.Unico,
                              false,
                              EventoLetivo.Sim,
                              (long)TipoEvento.ReposicaoDeAula);

            await CriarEventoResumido(REPOSICAO_AULA,
                                      data,
                                      data,
                                      EventoLetivo.Sim,
                                      TIPO_CALENDARIO_1,
                                      TIPO_EVENTO_1,
                                      DRE_CODIGO_1,
                                      UE_CODIGO_1,
                                      EntidadeStatus.Aprovado);

            await CriarEventoTipoResumido(LIBERACAO_EXCEPCIONAL,
                  EventoLocalOcorrencia.UE,
                  true,
                  EventoTipoData.Unico,
                  false,
                  EventoLetivo.Sim,
                  (long)TipoEvento.LiberacaoExcepcional);

            await CriarEventoResumido(LIBERACAO_EXCEPCIONAL,
                          data,
                          data,
                          EventoLetivo.Sim,
                          TIPO_CALENDARIO_1,
                          TIPO_EVENTO_2,
                          DRE_CODIGO_1,
                          UE_CODIGO_1,
                          EntidadeStatus.Aprovado);
        }

        private async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }
    }
}
