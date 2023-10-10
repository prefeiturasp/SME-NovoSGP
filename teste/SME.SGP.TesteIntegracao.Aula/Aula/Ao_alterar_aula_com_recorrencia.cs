using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes.Query;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AulaRecorrencia
{
    public class Ao_alterar_aula_com_recorrencia : AulaTeste
    {
        public Ao_alterar_aula_com_recorrencia(CollectionFixture collectionFixture) : base(collectionFixture) { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IServicoAuditoria), typeof(ServicoAuditoriaFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterPodeCadastrarAulaPorDataQuery, PodeCadastrarAulaPorDataRetornoDto>), typeof(ObterPodeCadastrarAulaPorDataQueryHandlerFake), ServiceLifetime.Scoped));

        }

        [Fact(DisplayName = "Aula - Deve permitir alterar a quantidade de aula recorrente no bimestre atual")]
        public async Task Altera_quantidade_de_aulas_com_recorrente_no_bimestre_atual()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriaAulaRecorrentePortugues(RecorrenciaAula.RepetirBimestreAtual);

            var usecase = ServiceProvider.GetService<IAlterarAulaUseCase>();

            var dataAula = DATA_02_05.AddDays(7);
            dataAula = dataAula.AddDays(5 - (int)dataAula.DayOfWeek);
            var aula = ObterAula(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, 138, dataAula);
            aula.Id = 1;

            await CriarPeriodoEscolarEAbertura();

            var retorno = await usecase.Executar(aula);

            var listaNotificao = ObterTodos<Notificacao>();

            retorno.ShouldNotBeNull();

            TimeSpan diasDiferenca = DATA_24_07_FIM_BIMESTRE_2 - dataAula;
            int totalAulasPorSemanasRecorrencia = diasDiferenca.Days / 7;

            listaNotificao.FirstOrDefault().Mensagem.ShouldContain($"Foram alteradas {totalAulasPorSemanasRecorrencia + 1} aulas do componente curricular Língua Portuguesa para a turma Turma Nome 1 da Nome da UE (DRE 1).");
        }

        [Fact(DisplayName = "Aula - Deve permitir alterar a quantidade de aula recorrente para todos os bimestre")]
        public async Task Altera_quantidade_de_aulas_com_recorrencia_para_todos_bimestres()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_1, false);

            await CriaAulaRecorrentePortugues(RecorrenciaAula.RepetirTodosBimestres);

            var usecase = ServiceProvider.GetService<IAlterarAulaUseCase>();

            var dataAula = DATA_02_05.AddDays(7);
            dataAula = dataAula.AddDays(5 - (int)dataAula.DayOfWeek);
            var aula = ObterAula(TipoAula.Normal, RecorrenciaAula.RepetirTodosBimestres, 138, dataAula);
            aula.Id = 1;

            await CriarPeriodoEscolarEAbertura();

            var retorno = await usecase.Executar(aula);

            var listaNotificao = ObterTodos<Notificacao>();

            retorno.ShouldNotBeNull();

            listaNotificao.ShouldNotBeEmpty();

            TimeSpan diasDiferenca = DATA_22_12_FIM_BIMESTRE_4 - dataAula;
            int totalAulasPorSemanasRecorrencia = diasDiferenca.Days / 7;

            listaNotificao.FirstOrDefault().Mensagem.ShouldContain($"Foram alteradas {totalAulasPorSemanasRecorrencia + 1} aulas do componente curricular Língua Portuguesa para a turma Turma Nome 1 da Nome da UE (DRE 1).");

        }

        [Fact(DisplayName = "Aula - Deve permitir alterar a quantidade de aula recorrente no bimestre atual")]
        public async Task Editar_quantidade_aula_recorrente_com_recorrente_no_bimestre_atual()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false);

            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.RepetirBimestreAtual);

            var dataAula = DATA_02_05.AddDays(7);
            await CriaAulaRecorrentePortugues(RecorrenciaAula.RepetirBimestreAtual, dataAula);

            dataAula = dataAula.AddDays(7);
            await CriaAulaRecorrentePortugues(RecorrenciaAula.RepetirBimestreAtual, dataAula);

            await CriarPeriodoEscolarEAbertura();

            var usecase = ServiceProvider.GetService<IAlterarAulaUseCase>();

            var aula = ObterAula(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, 138, DATA_02_05_INICIO_BIMESTRE_2);
            aula.Quantidade = 2;
            aula.Id = 1;

            var retorno = await usecase.Executar(aula);
            retorno.ShouldNotBeNull();
            retorno.Mensagens.Contains("Serão alteradas aulas recorrentes, em breve você receberá uma notificação com o resultado do processamento.").ShouldBe(true);
            var aulas = ObterTodos<Dominio.Aula>();
            aulas.All(aula => aula.Quantidade == 2).ShouldBe(true);
        }

        private async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }

        [Fact(DisplayName = "Aula - Ao alterar aula recorrente obter componentes do infantil agrupados")]
        public async Task Ao_alterar_aula_recorrente_obter_componentes_infantil_agrupados()
        {
            _collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioLogadoQuery, Usuario>), typeof(ObterUsuarioLogadoQueryHandlerFake), ServiceLifetime.Scoped));
            _collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesDoProfessorNaTurmaQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesDoProfessorNaTurmaQueryHandlerComponenteInfantilFake), ServiceLifetime.Scoped));
            _collectionFixture.BuildServiceProvider();

            var dataAtual = DateTimeExtension.HorarioBrasilia();

            await InserirNaBase(new Dre()
            {
                Id = 1,
                CodigoDre = "1",
                DataAtualizacao = dataAtual
            });

            await InserirNaBase(new Ue()
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1,
                DataAtualizacao = dataAtual
            });

            await InserirNaBase(new Dominio.Turma()
            {
                Id = 1,
                CodigoTurma = "1",
                UeId = 1,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                DataAtualizacao = dataAtual
            });

            await InserirNaBase(new TipoCalendario()
            {
                Id = 1,
                AnoLetivo = dataAtual.Year,
                Nome = "tipo cal infantil",
                Periodo = Periodo.Anual,
                Modalidade = ModalidadeTipoCalendario.Infantil,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                Id = 1,
                TipoCalendarioId = 1,
                Bimestre = 1,
                PeriodoInicio = new DateTime(dataAtual.Year, dataAtual.Month, 1),
                PeriodoFim = new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month)),
                CriadoPor = "Sistema",
                CriadoRF = "1234",
                CriadoEm = dataAtual
            });

            await InserirNaBase(new Dominio.Aula()
            {
                Id = 1,
                UeId = "1",
                DisciplinaId = "1",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = "1234",
                Quantidade = 1,
                DataAula = dataAtual.Date,
                RecorrenciaAula = RecorrenciaAula.RepetirTodosBimestres,
                TipoAula = TipoAula.Normal,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1234"
            });

            var usecase = ServiceProvider.GetService<IAlterarAulaUseCase>();

            var aulaAlteracao = new PersistirAulaDto()
            {
                DataAula = dataAtual,
                CodigoComponenteCurricular = 1,
                NomeComponenteCurricular = "comp infantil",
                DisciplinaCompartilhadaId = 0,
                Id = 1,
                Quantidade = 1,
                RecorrenciaAula = RecorrenciaAula.RepetirBimestreAtual,
                CodigoTurma = "1",
                CodigoUe = "1",
                EhRegencia = true,
                TipoAula = TipoAula.Normal,
                TipoCalendarioId = 1
            };

            var retorno = await usecase.Executar(aulaAlteracao);

            retorno.ShouldNotBeNull();
            retorno.ExistemErros.ShouldBeTrue();
            retorno.Mensagens.Single().ShouldBe("Serão alteradas aulas recorrentes, em breve você receberá uma notificação com o resultado do processamento.");
        }
    }
}
